using ASCOM;
using ASCOM.DeviceInterface;
using ASCOM.DriverAccess;
using ASCOM.Utilities;
using ASCOM.Utilities.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections;

namespace xASCOM.Simulator
{
    public class Dome :  IDomeV2
    {
        private SimulatorBase<IDomeV2> _simBase;
        private const string _description="Dome Simulator for xASCOM";
        private bool _connected;
        private bool _slaved;
        private int UpdateIntervalMillis;
        private System.Timers.Timer _updateTimer;
        private bool _isMoving = false;
        private bool _motorEnabled = false;
        private AltAz _currPosition;
        private AltAz _homePosition;
        private AltAz _parkPosition;
        private AltAz _targetPosition;
        private DateTime _lastStateDate=DateTime.MinValue;
        private bool _stateDirty;
        private ShutterState _shutterState;
        private AltAz _hwPrecision;
        private static object _slewSync = new object();
        public Dome()
        {
            _stateDirty = true;
            _connected = false;
            _targetPosition = null;
            UpdateIntervalMillis = 100;
            AllowTargetInterrupt = true;
            _simBase = new SimulatorBase<IDomeV2>(this);
            _updateTimer = new System.Timers.Timer(UpdateIntervalMillis) { Enabled = false, AutoReset = true };
            _updateTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnUpdateTimer);
        }

        #region IAscomDriver implmentation
        public bool Connected
        {
            get
            {
                return _connected;
            }
            set
            {
                if (Connected != value)
                {

                    //OnDisconnecting
                    if (!value)     
                    {
                        SaveState();
                    }
                    _connected = value;
                    //OnConnected
                    if(Connected)
                    {
                        QueryCapabilities(true);
                        QueryState(true);
                    }
                    //OnDisconnected
                    else
                    {
                        QueryCapabilities(false);
                        QueryState(false);
                    }
                }
            }
        }

        public string Description
        {
            get => _simBase.Description;
        }

        public string DriverInfo
        {
            get => _simBase.DriverInfo;
        }

        public string DriverVersion
        {
            get => _simBase.DriverVersion;
        }

        public short InterfaceVersion
        {
            get => _simBase.InterfaceVersion;
        }

        public string Name => GetType().FullName;

        public ArrayList SupportedActions => _simBase.SupportedActions;

        public string Action(string ActionName, string ActionParameters)
        {
            return _simBase.Action(ActionName, ActionParameters);
        }

        public void CommandBlind(string Command, bool Raw)
        {
            _simBase.CommandBlind(Command, Raw);
        }

        public bool CommandBool(string Command, bool Raw)
        {
            return _simBase.CommandBool(Command, Raw);
        }

        public string CommandString(string Command, bool Raw)
        {
            return _simBase.CommandString(Command, Raw);
        }

        public void SetupDialog()
        {
            throw new System.NotImplementedException();
        }

        public void Dispose()
        {
            Shutdown();
            Connected = false;
            _simBase?.Dispose();
            _simBase = null;
        }

        #endregion

        #region IDomeV2 implementation
        public bool CanFindHome { get; internal set; }
        public bool CanPark { get; internal set; }
        public bool CanSetAltitude { get; internal set; }
        public bool CanSetAzimuth { get; internal set; }
        public bool CanSetPark { get; internal set; }
        public bool CanSetShutter { get; internal set; }
        public bool CanSlave { get; internal set; }
        public bool CanSyncAzimuth { get; internal set; }

        public ShutterState ShutterStatus
        {
            get
            {
                EnsureConnected();
                updateShutterState();
                return _shutterState;
            }
            //internal set
            //{
            //    EnsureConnected();
            //    if (ShutterStatus != value)
            //    {
            //        lock (_slewSync)
            //        {
            //            if (ShutterStatus != value)
            //            {
            //                if (value == ShutterState.shutterClosed)
            //                {
            //                    setTargetAltitude(MinAltitude);
            //                }
            //                if (value == ShutterState.shutterOpen)
            //                {
            //                    _updateTimer.Enabled = true;
            //                }
            //                else
            //                    throw new DriverException($"Internal driver error: Unsupported shutter state: '{value.ToString()}', only {ShutterState.shutterOpen} and {ShutterState.shutterClosed} are supported");

            //            }
            //        }
            //    }
            //}
        }
        public double Altitude { get { EnsureConnected(); return _currPosition.Altitude; } }
        public double Azimuth { get { EnsureConnected(); return _currPosition.Azimuth; } }
        public bool AtHome
        {
            get
            {
                EnsureConnected();
                if (!CanFindHome)
                    throw new PropertyNotImplementedException("AtHome");
                return _currPosition.AzimuthEquals(_homePosition.Azimuth, _hwPrecision.Azimuth);
            }
        }
        public bool AtPark
        {
            get
            {
                EnsureConnected();
                if (!CanPark)
                    throw new PropertyNotImplementedException("AtPark");
                return _currPosition.AzimuthEquals(_parkPosition.Azimuth, _hwPrecision.Azimuth);
            }
        }
        public bool Slaved
        {
            get
            {
                EnsureConnected();
                return CanSlave ? _slaved : false;
            }
            set
            {
                EnsureConnected();
                if (Slaved != value)
                {
                    if (!CanSlave)
                        throw new PropertyNotImplementedException("Dome does not support slaving");
                    //TODO: Implement slaving???
                    _slaved = value;

                }
            }
        }
        public bool Slewing
        {
            get
            {
                EnsureConnected();
                return _isMoving;
            }
            protected set
            {
                if(Slewing!=value)
                {
                    _isMoving = value;
                    _updateTimer.Enabled = _isMoving;
                }
            }
        }

        public void AbortSlew()
        {
            _motorEnabled = false;
            _targetPosition = null;
        }
        public void CloseShutter()
        {
            EnsureConnected();
            if (ShutterStatus == ShutterState.shutterClosed || ShutterStatus == ShutterState.shutterClosing)
                return;
            SlewToAltitude(MinAltitude);
        }
        public void OpenShutter()
        {
            EnsureConnected();
            if (ShutterStatus == ShutterState.shutterOpen || ShutterStatus == ShutterState.shutterOpening)
                return;
            double targAlt = Target.Altitude > MinAltitude ? Target.Altitude : MaxAltitude;
            SlewToAltitude(targAlt);
        }
        public void FindHome()
        {
            EnsureConnected();
            if (!CanFindHome)
                throw new MethodNotImplementedException("FindHome");
            if (Slaved)
                throw new DriverException("FindHome: Invalid operation while slaved");
            //Check if already at or slewing to home position
            if ((!Slewing && AtHome) || (Slewing && _targetPosition.AzimuthEquals(_homePosition.Azimuth, _hwPrecision.Azimuth)))
                return;

            SlewToAzimuth(_homePosition.Azimuth);
        }

        public void Park()
        {
            EnsureConnected();
            if (!CanPark)
                throw new MethodNotImplementedException("Park");
            if (Slaved)
                throw new DriverException("Park: Invalid operation while slaved");
            if (_parkPosition == null)
                throw new DriverException("Park: Park position not set, use SetPark()");

            //Check if already at or slewing to park position
            if ((!Slewing && AtPark) || (Slewing && _targetPosition.AzimuthEquals(_parkPosition.Azimuth, _hwPrecision.Azimuth)))
                return;

            SlewToAzimuth(_parkPosition.Azimuth);
        }
        public void SetPark()
        {
            EnsureConnected();
            if (!CanSetPark)
                throw new MethodNotImplementedException("SetPark");
            _parkPosition = new AltAz(_currPosition);
            _simBase.SetState("ParkPosition", _parkPosition);

        }
        public void SlewToAltitude(double alt)
        {
            EnsureConnected();
            if (Slewing && Target.Altitude == alt)
                return;
            if (Slewing && AllowTargetInterrupt)
                AbortSlew();

            if (Slewing)
                throw new DriverException("SlewToAltitude: Dome is busy");
            setTargetAltitude(alt);
            Slewing = true;
        }
        public void SlewToAzimuth(double az)
        {
            EnsureConnected();
            if (Slewing && Target.Azimuth == az)
                return;
            if (Slewing && AllowTargetInterrupt)
                AbortSlew();

            if (Slewing)
                throw new DriverException("SlewToAzimuth: Dome is busy");
            setTargetAzimuth(az);
            Slewing = true;
        }
        public void SyncToAzimuth(double az)
        {
            EnsureConnected();
            if (!CanSyncAzimuth)
                throw new MethodNotImplementedException("CanSyncAzimuth");
            _currPosition.Azimuth = az;
        }
        #endregion

#region Utility members
#if ENABLE_COM
        [System.Runtime.InteropServices.ComRegisterFunction]
#endif
        public static void RegisterASCOM(Type t)
        {
            SimulatorBase<IDomeV2>.RegisterASCOM(t, _description);
        }

#if ENABLE_COM
        [System.Runtime.InteropServices.ComUnregisterFunction]
#endif
        public static void UnregisterASCOM(Type t)
        {
            SimulatorBase<IDomeV2>.RegisterASCOM(t, null,true);
        }

        public ITraceLogger Logger { get => _simBase.Logger; }
        public double MinAltitude { get; set; }
        public double MaxAltitude { get; set; }
        internal AltAz Target => _targetPosition ?? new AltAz(_currPosition); 
        public bool AtTargetAlt => _currPosition.AltitudeEquals(Target.Altitude, _hwPrecision.Altitude); 
        public bool AtTargetAz => _currPosition.AzimuthEquals(Target.Azimuth, _hwPrecision.Azimuth);
        public bool AtTarget => AtTargetAlt && AtTargetAz;
        virtual public void QueryCapabilities(bool autoSave = true)
        {
            if (Connected)
            {
                CanSyncAzimuth = true;
                CanSlave = true;
                CanSetPark = true;
                CanSetAzimuth = true;
                CanSetAltitude = true;
                CanFindHome = true;
                CanSetShutter = true;
            }
            else
            {                
                CanSyncAzimuth = _simBase.GetCapability("CanSyncAzimuth", false);
                CanSlave = _simBase.GetCapability("CanSlave", false);
                CanSetPark = _simBase.GetCapability("CanSetPark", false);
                CanSetAzimuth = _simBase.GetCapability("CanSetAzimuth", false);
                CanSetAltitude = _simBase.GetCapability("CanSetAltitude", false);
                CanFindHome = _simBase.GetCapability("CanFindHome", false);
                CanSetShutter = _simBase.GetCapability("CanSetShutter", false);
            }
            if(autoSave)
                SaveCapabilities();
        }
        virtual public void SaveCapabilities()
        {
            _simBase.SetCapability("CanSyncAzimuth", CanSyncAzimuth);
            _simBase.SetCapability("CanSlave", CanSlave);
            _simBase.SetCapability("CanSetPark", CanSetPark);
            _simBase.SetCapability("CanSetAzimuth", CanSetAzimuth);
            _simBase.SetCapability("CanSetAltitude", CanSetAltitude);
            _simBase.SetCapability("CanFindHome", CanFindHome);
            _simBase.SetCapability("CanSetShutter", CanSetShutter);
        }
        virtual public void QueryState(bool autoSave = true)
        {
            if (Connected)
            {
                _slaved = false;
                MinAltitude = 0d;
                MaxAltitude = 90d;
                _currPosition = AltAz.Normalized(_currPosition ?? new AltAz(15, 90));
                _parkPosition = AltAz.Normalized(_parkPosition ?? new AltAz());
                _homePosition = AltAz.Normalized(_homePosition ?? new AltAz(_parkPosition));
                _hwPrecision = AltAz.Normalized(_hwPrecision ?? new AltAz(2.5, 2.5));
                updateShutterState();
                UpdateIntervalMillis = 250;
                AllowTargetInterrupt = true;
                ShutterRate = 10;   // deg/sec
                RotateRate = 36;    // deg/sec
            }
            else
            {
                _shutterState = ShutterState.shutterError;
                _slaved = false;
                MinAltitude = Double.NaN;
                MaxAltitude = Double.NaN;
                _currPosition = null;
                _homePosition = null;
                _parkPosition = null;
                _hwPrecision = null;
                UpdateIntervalMillis = 0;
                AllowTargetInterrupt = false;
                ShutterRate = double.NaN;   // deg/sec
                RotateRate = double.NaN;    // deg/sec
            }
            if (autoSave)
                SaveState();
        }
        virtual public void LoadLastState()
        {
            Logger.LogMessage("LoadLastState", "Loading last saved device state");
            _currPosition = _simBase.GetState("CurrentPosition", _currPosition);
            _homePosition = _simBase.GetState("HomePosition", _homePosition);
            _parkPosition = _simBase.GetState("ParkPosition", _parkPosition);
            _hwPrecision = _simBase.GetState("HardwarePrecision", _hwPrecision);
            //_shutterState=_simBase.GetState("ShutterStatus", _shutterState);
            MinAltitude = _simBase.GetState("MinAltitude", MinAltitude);
            MaxAltitude = _simBase.GetState("MaxAltitude", MaxAltitude);
            ShutterRate = _simBase.GetState("ShutterRate", ShutterRate);
            RotateRate = _simBase.GetState("RotateRate", RotateRate);
            UpdateIntervalMillis = _simBase.GetState("UpdateIntervalMillis", UpdateIntervalMillis);
            AllowTargetInterrupt = _simBase.GetState("AllowTargetInterrupt", AllowTargetInterrupt);
            _lastStateDate = _simBase.GetState("StateDate", DateTime.MinValue);
            updateShutterState();
            _stateDirty = false;
        }
        virtual public void SaveState()
        {
            if (Connected)
            {
                Logger.LogMessage("SaveState", "Saving device state");
                _simBase.SetState("CurrentPosition", _currPosition);
                _simBase.SetState("HomePosition", _homePosition);
                _simBase.SetState("ParkPosition", _parkPosition);
                _simBase.SetState("HardwarePrecision", _hwPrecision);
                _simBase.SetState("ShutterStatus", ShutterStatus);
                _simBase.SetState("MinAltitude", MinAltitude);
                _simBase.SetState("MaxAltitude", MaxAltitude);
                _simBase.SetState("ShutterRate", ShutterRate);
                _simBase.SetState("RotateRate", RotateRate);
                _simBase.SetState("UpdateIntervalMillis", UpdateIntervalMillis);
                _simBase.SetState("AllowTargetInterrupt", AllowTargetInterrupt);
                _lastStateDate = DateTime.Now;
                _simBase.SetState("StateDate", _lastStateDate);
                _stateDirty = false;
            }
            else
                Logger.LogMessage("SaveState", "Not connected, skipping save"); 
        }

        /// <summary>
        /// Gets/Sets the Shutter motor rate in deg/s
        /// Default=10 deg/s
        /// </summary>
        protected double ShutterRate { get; set; } = 10.0;
        /// <summary>
        /// Gets/Sets the dome rotation rate in deg/s
        /// Default=10 deg/s
        /// </summary>        
        protected double RotateRate { get; set; } = 10.0;
        private void OnUpdateTimer(object source, System.Timers.ElapsedEventArgs e)
        {
            if (!_motorEnabled)
                return;
            if (AtTarget)
            {
                AbortSlew();
                return;
            }

            AltAz targ = Target;
            double altDelta = _currPosition.Altitude - targ.Altitude;
            double azDelta = _currPosition.Azimuth - targ.Azimuth;

            double altMove=ShutterRate* UpdateIntervalMillis / 1000;
            double azMove = RotateRate * UpdateIntervalMillis / 1000;

            if (altDelta<0) altMove *= -1;
            if (azDelta < 0) azMove *= -1;

            if (Math.Abs(altDelta) > _hwPrecision.Altitude)
                _currPosition.Altitude += altMove;
            if (Math.Abs(azDelta) > _hwPrecision.Altitude)
                _currPosition.Azimuth += azMove;
        }

        protected void updateShutterState()
        {
            AltAz targ = Target;
            ShutterState currState = _shutterState;
            if (_currPosition.Altitude <= MinAltitude)
            {
                _currPosition.Altitude = MinAltitude;
                _shutterState = ShutterState.shutterClosed;
            }
            else if (_currPosition.Altitude >= MaxAltitude)
            {
                _currPosition.Altitude = MaxAltitude;
                _shutterState = ShutterState.shutterOpen;
            }
            else if (_currPosition.Altitude > targ.Altitude)
            {
                _shutterState = ShutterState.shutterClosing;
            }
            else if (_currPosition.Altitude < targ.Altitude)
            {
                _shutterState = ShutterState.shutterOpening;
            }
            if(currState!=_shutterState)
                _simBase.SetState("ShutterStatus", _shutterState);
        }
        protected void setTargetAltitude(double value)
        {
            _targetPosition = Target;
            _targetPosition.Altitude = value;

        }
        protected void setTargetAzimuth(double value)
        {
            _targetPosition = Target;
            _targetPosition.Azimuth = value;
        }

        protected bool AllowTargetInterrupt { get; set; }
        protected void EnsureConnected()
        {
            if (!Connected)
                throw new NotConnectedException($"{_simBase.DriverType}<{Name}> not connected");
        }
        //TODO: Add these methods as actions
        public virtual bool ResetShutter()
        {
            CloseShutter();
            return ShutterStatus == ShutterState.shutterClosed;
        }
        public virtual bool ResetRotation()
        {
            SlewToAzimuth(0);
            return _currPosition.Equals(new AltAz(Altitude, 0));
        }
        public virtual bool Shutdown()
        {
            bool cleanShutdown = true;
            Logger.LogMessage("Shutdown", $"Shutting down dome device...");
            if (Connected)
            {
                Logger.LogMessage("Shutdown", $"Device is connected, attempting to return dome to 'closed' state");
                double finalAz = _currPosition.Azimuth;
                Logger.LogMessage("Shutdown", $"Closing shutter...");
                CloseShutter();
                if (ShutterStatus == ShutterState.shutterClosed)
                    Logger.LogMessage("Shutdown", $"Successfully closed shutter...");
                else
                    Logger.LogMessage("Shutdown", $"Shutter left in '{ShutterStatus}' state!");
                if (CanPark)
                {
                    Logger.LogMessage("Shutdown", $"CanPark...Attempting to park dome");
                    Park();
                    finalAz = _parkPosition.Azimuth;
                }
                else if (CanFindHome)
                {
                    Logger.LogMessage("Shutdown", $"Park not supported, using FindHome");
                    FindHome();
                    finalAz = _homePosition.Azimuth;
                }
                else
                {
                    Logger.LogMessage("Shutdown", $"No Home or Park position available, skipping shutdown rotation");
                    finalAz = _currPosition.Azimuth;
                }
                cleanShutdown = ShutterStatus == ShutterState.shutterClosed
                    && _currPosition.Equals(new AltAz(_currPosition.Altitude, finalAz));
                if(cleanShutdown)
                    Logger.LogMessage("Shutdown", $"Shutdown was successful");
                else 
                    Logger.LogMessage("Shutdown", $"Shutdown was not completed successfully");
                Logger.LogMessage("Shutdown", $"Dome final state: Shutter={_shutterState}, Position={_currPosition}");
                //Logger.LogMessage("Shutdown", $"Disconnecting");
                //Connected = false;
            }
            else
            {
                Logger.LogMessage("Shutdown", $"Device not connected, nothing to do");
            }
            return cleanShutdown;
        }
#endregion
    }
}
