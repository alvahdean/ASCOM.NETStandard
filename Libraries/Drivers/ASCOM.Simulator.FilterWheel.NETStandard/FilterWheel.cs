using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using ASCOM.DeviceInterface;
using ASCOM.Utilities;
using System.Globalization;
using ASCOM;

namespace xASCOM.Simulator
{
    //
    // Your driver's DeviceID is ASCOM.Simulator.FilterWheel
    //
    // The Guid attribute sets the CLSID for ASCOM.Simulator.FilterWheel
    // The ClassInterface/None addribute prevents an empty interface called
    // _Conceptual from being created and used as the [default] interface
    //

    /// <summary>
    /// ASCOM FilterWheel Driver for a FilterWheel.
    /// This class is the implementation of the public ASCOM interface.
    /// </summary>
    public class FilterWheel : IFilterWheelV2, IDisposable
    {
        #region Constants

        /// <summary>
        /// Name of the Driver
        /// </summary>
        private const string driverType = "FilterWheel";
        private const string name = "ASCOM.Simulator."+driverType;
        /// <summary>
        /// Driver version number
        /// </summary>
        private const string driverVersion = "6.3";

        /// <summary>
        /// Description of the driver
        /// </summary>
        private const string description = "ASCOM "+driverType+" Simulator Driver (.NETStandard)";

        /// <summary>
        /// Driver information
        /// </summary>
        private const string driverInfo = driverType+" Simulator Driver";

        /// <summary>
        /// Driver interface version
        /// </summary>
        private const short interfaceVersion = 2;

        
        /// <summary>
        /// ASCOM DeviceID (COM ProgID) for this driver.
        /// The DeviceID is used by ASCOM applications to load the driver at runtime.
        /// </summary>
        private const string sCsDriverId = name;

        /// <summary>
        /// Driver description that displays in the ASCOM Chooser.
        /// </summary>
        private const string sCsDriverDescription = description;

        /// <summary>
        /// Sets up the permenant store for saved settings
        /// </summary>
        private static Profile _profile;
        private static TraceLogger _tlog;
        private static object syncRoot = new object();
        #endregion

        // Shared tracelogger between this instances classes

        internal static TraceLogger TL
        {
            get
            {
                if (_tlog == null)
                {
                    lock (syncRoot)
                    {
                        if (_tlog == null)
                        {
                            _tlog = new TraceLogger("", driverType);
#if DEBUG
                            _tlog.EchoConsole = true;
                            _tlog.EchoDebug = true;
#else
                            _tlog.EchoConsole = false;
                            _tlog.EchoDebug = false;
#endif
                            _tlog.Enabled = RegistryCommonCode.GetBool(GlobalConstants.SIMULATOR_TRACE, GlobalConstants.SIMULATOR_TRACE_DEFAULT);
                            _tlog.LogMessage("New", "Started");
                        }
                    }
                }
                return _tlog;
            }
        }
        internal static Profile Profile
        {
            get
            {
                if (_profile == null)
                {
                    lock (syncRoot)
                    {
                        if (_profile == null)
                        {
                            _profile = new Profile() { DeviceType = driverType };
                            TL.LogMessage("Init", "Created shared Profile object");
                        }
                    }
                }
                return _profile;
            }
        }

        #region local parameters

        private bool _isConnected;
        private System.Timers.Timer _moveTimer; // drives the position and temperature changers
        private int _position;
        internal int Target;
        private double _lastTemp;
        //private FilterWheelHandboxForm Handbox;
        private DateTime lastTempUpdate;
        private Random RandomGenerator;
        internal double stepSize;
        internal bool tempComp;

        private enum MotorState
        {
            idle,
            moving,
            settling
        }

        private MotorState motorState = MotorState.idle;
        internal int settleTime;
        private DateTime settleFinishTime;

        #endregion

        #region Constructor and dispose

        /// <summary>
        /// Initializes a new instance of the <see cref=driverType/> class.
        /// Must be public for COM registration.
        /// </summary>
        public FilterWheel()
        {
            try
            {

                TL.Enabled = RegistryCommonCode.GetBool(GlobalConstants.SIMULATOR_TRACE, GlobalConstants.SIMULATOR_TRACE_DEFAULT);
                TL.LogMessage("New", "Started");

                //check to see if the profile is ok
                if (ValidateProfile())
                {
                    TL.LogMessage("New", "Validated OK");
                    KeepMoving = false;
                    LastOffset = 0;
                    RateOfChange = 1;
                    MouseDownTime = DateTime.MaxValue; //Initialise to "don't accelerate" value
                    RandomGenerator = new Random(); //Temperature fluctuation random generator
                    LoadDriverKeyValues();
                    TL.LogMessage("New", "Loaded Key Values");
                    //Handbox = new FilterWheelHandboxForm(this);
                    //Handbox.Hide();
                    //TL.LogMessage("FocusSettingsForm", "Created Handbox");

                    // start a timer that monitors and moves the FilterWheel
                    _moveTimer = new System.Timers.Timer();
                    _moveTimer.Elapsed += new System.Timers.ElapsedEventHandler(MoveTimer_Tick);
                    _moveTimer.Interval = 100;
                    _moveTimer.Enabled = true;
                    _lastTemp = Temperature;
                    Target = _position;

                    TL.LogMessage("New", "Started Simulation");
                }
                else
                {
                    TL.LogMessage("New", "Registering Profile");
                    RegisterWithProfile();
                    TL.LogMessage("New", "Registered OK");
                }

                TL.LogMessage("New", "Completed");
            }
            catch (Exception ex)
            {
                TL.LogMessageCrLf("New Exception", ex.ToString());
                Console.WriteLine($"FilterWheel: {ex}");
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                try { TL.LogMessage("Dispose", "Dispose called: " + disposing.ToString()); } catch { }
                //try { Handbox.Close(); } catch { }
                //try { Handbox.Dispose(); } catch { }
                try { _moveTimer.Stop(); } catch { }
                try { _moveTimer.Close(); } catch { }
                try { _moveTimer.Dispose(); } catch { }

                //Don't dispose shared TraceLogger
                //try { TL.Enabled = false; } catch { }
                //try { TL.Dispose(); } catch { }
            }
        }

        public void Dispose()
        {
            try { TL.LogMessage("Dispose", "Dispose called."); } catch { }
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Private Properties

        internal bool CanHalt { get; set; }
        internal bool TempProbe { get; set; }
        internal bool Synchronous { get; set; }
        internal bool CanStepSize { get; set; }
        internal bool KeepMoving { get; set; }
        internal int LastOffset { get; set; }
        internal double Temperature { get; set; }
        internal double TempMax { get; set; }
        internal double TempMin { get; set; }
        internal double TempPeriod { get; set; }
        internal int TempSteps { get; set; }
        internal int RateOfChange { get; set; }
        internal int MotorSpeed { get; set; } //Steps/sec
        internal DateTime MouseDownTime { get; set; }

        //internal double MaxPosition { get { return Math.Max(Name?.Length-1 ?? 0,0); } }
        internal int MaxPosition { get; set; }
        internal int MaxIncrement { get; set; }
        internal bool TempCompAvailable { get; set; }
        internal bool Absolute { get; set; }
        #endregion


        #region Private Members

        /// <summary>
        /// Ticks 10 times a second, updating the FilterWheel position and IsMoving properties
        /// </summary>
        private void MoveTimer_Tick(object source, System.Timers.ElapsedEventArgs e)
        {
            // Change at introduction of IFilterWheelV3 - only allow random temperature induced changes when the motor is in the idle state
            // This is because IFilterWheel V3 allows moves when temperature compensation is active
            if (motorState == MotorState.idle)
            {
                //Create random temperature change
                if (DateTime.Now.Subtract(lastTempUpdate).TotalSeconds > TempPeriod)
                {
                    lastTempUpdate = DateTime.Now;
                    // apply a random change to the temperature
                    double tempOffset = (RandomGenerator.NextDouble() - 0.5);// / 10.0;
                    Temperature = Math.Round(Temperature + tempOffset, 2);

                    // move the FilterWheel target to track the temperature if required
                    if (tempComp)
                    {
                        var dt = (int)((Temperature - _lastTemp) * TempSteps);
                        if (dt != 0)// return;
                        {
                            Target += dt;
                            _lastTemp = Temperature;
                        }
                    }
                }
            }

            if (Target > MaxPosition) Target = MaxPosition; // Condition target within the acceptable range
            if (Target < 0) Target = 0;

            if (_position != Target) //Actually move the focuse if necessary
            {
                TL.LogMessage("Moving", "LastOffset, Position, Target RateOfChange " + LastOffset + " " + _position + " " + Target + " " + RateOfChange);

                if (Math.Abs(_position - Target) <= RateOfChange)
                {
                    _position = Target;
                    TL.LogMessage("Moving", "  Set position = target");

                }
                else
                {
                    _position += (_position > Target) ? -RateOfChange : RateOfChange;
                    TL.LogMessage("Moving", "  Updated position = " + _position);
                }
                TL.LogMessage("Moving", "  New position = " + _position);
            }
            if (KeepMoving & (DateTime.Now.Subtract(MouseDownTime).TotalSeconds > 0.5))
            {
                Target = (Math.Sign(LastOffset) > 0) ? MaxPosition : 0;
                MouseDownTime = DateTime.Now;
                if (RateOfChange < 100)
                {
                    RateOfChange = (int)Math.Ceiling((double)RateOfChange * 1.2);
                }
                TL.LogMessage("KeepMoving", "LastOffset, Position, Target, RateOfChange MouseDownTime " + LastOffset + " " + _position + " " + Target + " " + RateOfChange + " " + MouseDownTime.ToLongTimeString());
            }

            // handle MotorState
            switch (motorState)
            {
                case MotorState.moving:
                    if (_position == Target)
                    {
                        motorState = MotorState.settling;
                        settleFinishTime = DateTime.Now + TimeSpan.FromMilliseconds(settleTime);
                        TL.LogMessage("MoveTimer", "Settle start, time " + settleTime.ToString());
                    }
                    return;
                case MotorState.settling:
                    if (settleFinishTime < DateTime.Now)
                    {
                        motorState = MotorState.idle;
                        TL.LogMessage("MoveTimer", "settle finished");
                    }
                    return;
            }
        }

        private void CheckConnected(string property)
        {
            if (!_isConnected)
                throw new NotConnectedException(property);
        }

        /// <summary>
        /// Truncate val to be between min and max
        /// </summary>
        /// <param name="min"></param>
        /// <param name="val"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        private static int Truncate(int min, int val, int max)
        {
            return Math.Max(Math.Min(max, val), min);
        }

        /// <summary>
        /// Validate the profile is in good shape
        /// </summary>
        private static bool ValidateProfile()
        {
            try
            {
                Profile.DeviceType = driverType;
                //check profile if the driver id is registered
                return Profile.IsRegistered(sCsDriverId);
            }
            catch (DirectoryNotFoundException)
            {
                return false;
            }
        }

        /// <summary>
        /// Load the profile values
        /// </summary>
        private void LoadDriverKeyValues()
        {
            string filterNames = Profile.GetValue(sCsDriverId, "FilterNames", string.Empty, "None,Luminance,Red,Green,Blue,Ha,OIII,SII");
            Names = filterNames.Split(new char[] { ',', ':' }, StringSplitOptions.RemoveEmptyEntries);

            Absolute = Convert.ToBoolean(Profile.GetValue(sCsDriverId, "Absolute", string.Empty, "true"), CultureInfo.InvariantCulture);
            MaxIncrement =
                driverType == "FilterWheel"
                ? 1
                : Convert.ToInt32(Profile.GetValue(sCsDriverId, "MaxIncrement", string.Empty, "50000"), CultureInfo.InvariantCulture);
            MaxPosition =
                driverType == "FilterWheel"
                ? (Names?.Length??0) - 1
                : Convert.ToInt32(Profile.GetValue(sCsDriverId, "MaxPosition", string.Empty, "50000"), CultureInfo.InvariantCulture);
            int defPos = driverType == "FilterWheel" ? 0 : 25000;
            _position = Convert.ToInt32(Profile.GetValue(sCsDriverId, "Position", string.Empty, defPos.ToString()), CultureInfo.InvariantCulture);
            int defSpeed = driverType == "FilterWheel" ? 1 : 40;
            MotorSpeed = Convert.ToInt32(Profile.GetValue(sCsDriverId, "MotorSpeed", string.Empty, defSpeed.ToString()), CultureInfo.InvariantCulture);
            int defStepSize = driverType == "FilterWheel" ? 1 : 20;
            stepSize = Convert.ToDouble(Profile.GetValue(sCsDriverId, "StepSize", string.Empty, defStepSize.ToString()), CultureInfo.InvariantCulture);
            tempComp = Convert.ToBoolean(Profile.GetValue(sCsDriverId, "TempComp", string.Empty, "false"), CultureInfo.InvariantCulture);
            TempCompAvailable = Convert.ToBoolean(Profile.GetValue(sCsDriverId, "TempCompAvailable", string.Empty, "true"), CultureInfo.InvariantCulture);
            Temperature = Convert.ToDouble(Profile.GetValue(sCsDriverId, "Temperature", string.Empty, "5"), CultureInfo.InvariantCulture);
            //extended FilterWheel items
            CanHalt = Convert.ToBoolean(Profile.GetValue(sCsDriverId, "CanHalt", string.Empty, "true"), CultureInfo.InvariantCulture);
            CanStepSize = Convert.ToBoolean(Profile.GetValue(sCsDriverId, "CanStepSize", string.Empty, "true"), CultureInfo.InvariantCulture);
            Synchronous = Convert.ToBoolean(Profile.GetValue(sCsDriverId, "Synchronous", string.Empty, "true"), CultureInfo.InvariantCulture);
            TempMax = Convert.ToDouble(Profile.GetValue(sCsDriverId, "TempMax", string.Empty, "50"), CultureInfo.InvariantCulture);
            TempMin = Convert.ToDouble(Profile.GetValue(sCsDriverId, "TempMin", string.Empty, "-50"), CultureInfo.InvariantCulture);
            TempPeriod = Convert.ToDouble(Profile.GetValue(sCsDriverId, "TempPeriod", string.Empty, "3"), CultureInfo.InvariantCulture);
            TempProbe = Convert.ToBoolean(Profile.GetValue(sCsDriverId, "TempProbe", string.Empty, "true"), CultureInfo.InvariantCulture);
            TempSteps = Convert.ToInt32(Profile.GetValue(sCsDriverId, "TempSteps", string.Empty, "10"), CultureInfo.InvariantCulture);
            settleTime = Convert.ToInt32(Profile.GetValue(sCsDriverId, "SettleTime", string.Empty, "500"), CultureInfo.InvariantCulture);
            if (!TL.Enabled)
                TL.Enabled = Convert.ToBoolean(Profile.GetValue(sCsDriverId, "Logging", string.Empty, "false"), CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Register the driver to setup a Profile
        /// </summary>
        private static void RegisterWithProfile()
        {
            Profile.Register(sCsDriverId, sCsDriverId);
            if (ValidateProfile())
            {
                Profile.WriteValue(sCsDriverId, driverType, "false");
                return;
            }
            return;
        }

        /// <summary>
        /// Save profile values
        /// </summary>
        public void SaveProfileSettings()
        {
            if (Temperature > TempMax) Temperature = TempMax;
            if (Temperature < TempMin) Temperature = TempMin;
            if (_position > MaxPosition) _position = MaxPosition;

            //ascom items
            Profile.WriteValue(sCsDriverId, "FilterNames", String.Join(",",Names).ToString(CultureInfo.InvariantCulture));
            Profile.WriteValue(sCsDriverId, "Absolute", Absolute.ToString(CultureInfo.InvariantCulture));
            Profile.WriteValue(sCsDriverId, "MaxIncrement", MaxIncrement.ToString(CultureInfo.InvariantCulture));
            Profile.WriteValue(sCsDriverId, "MaxPosition", MaxPosition.ToString(CultureInfo.InvariantCulture));
            Profile.WriteValue(sCsDriverId, "Position", _position.ToString(CultureInfo.InvariantCulture));
            Profile.WriteValue(sCsDriverId, "MotorSpeed", MotorSpeed.ToString(CultureInfo.InvariantCulture));
            Profile.WriteValue(sCsDriverId, "StepSize", stepSize.ToString(CultureInfo.InvariantCulture));
            Profile.WriteValue(sCsDriverId, "TempComp", tempComp.ToString(CultureInfo.InvariantCulture));
            Profile.WriteValue(sCsDriverId, "TempCompAvailable", TempCompAvailable.ToString(CultureInfo.InvariantCulture));
            Profile.WriteValue(sCsDriverId, "Temperature", Temperature.ToString(CultureInfo.InvariantCulture));
            //extended FilterWheel items
            Profile.WriteValue(sCsDriverId, "CanHalt", CanHalt.ToString(CultureInfo.InvariantCulture));
            Profile.WriteValue(sCsDriverId, "CanStepSize", CanStepSize.ToString(CultureInfo.InvariantCulture));
            Profile.WriteValue(sCsDriverId, "Synchronous", Synchronous.ToString(CultureInfo.InvariantCulture));
            Profile.WriteValue(sCsDriverId, "TempMax", TempMax.ToString(CultureInfo.InvariantCulture));
            Profile.WriteValue(sCsDriverId, "TempMin", TempMin.ToString(CultureInfo.InvariantCulture));
            Profile.WriteValue(sCsDriverId, "TempPeriod", TempPeriod.ToString(CultureInfo.InvariantCulture));
            Profile.WriteValue(sCsDriverId, "TempProbe", TempProbe.ToString(CultureInfo.InvariantCulture));
            Profile.WriteValue(sCsDriverId, "TempSteps", TempSteps.ToString(CultureInfo.InvariantCulture));
            Profile.WriteValue(sCsDriverId, "SettleTime", settleTime.ToString(CultureInfo.InvariantCulture));
            Profile.WriteValue(sCsDriverId, "Logging", TL.Enabled.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Saves specific state setting to the profile a switchdevice
        /// </summary>
        public static void SaveProfileSetting(string keyName, string value)
        {
            Profile.WriteValue(sCsDriverId, keyName, value);
        }

        #endregion

        #region IAscomDriver implementation

        public bool Connected { get => _isConnected; set => _isConnected = value; }

        public string Description { get => description; }

        public string DriverInfo { get => driverInfo; }

        public string DriverVersion { get => driverVersion; }

        public short InterfaceVersion { get => interfaceVersion; }

        public string Name { get => name; }

        public ArrayList SupportedActions { get => new ArrayList(); }

        public string Action(string ActionName, string ActionParameters)
        {
            throw new ASCOM.ActionNotImplementedException();
        }

        public void CommandBlind(string Command, bool Raw)
        {
            throw new ASCOM.ActionNotImplementedException();
        }

        public bool CommandBool(string Command, bool Raw)
        {
            throw new ASCOM.ActionNotImplementedException();

        }

        public string CommandString(string Command, bool Raw)
        {
            throw new ASCOM.ActionNotImplementedException();
        }

        public void SetupDialog()
        {
            throw new ASCOM.ActionNotImplementedException();
        }

        #endregion

        #region IFilterWheelV2 implementation

        public int[] FocusOffsets
        {
            get
            {
                throw new System.NotImplementedException();
            }
        }

        public string[] Names { get; private set; }

        public short Position
        {
            get { return (short)_position; }
            set { _position = value; }
        }

        #endregion

        #region ASCOM Registration

        //
        // Register or unregister driver for ASCOM. This is harmless if already
        // registered or unregistered. 
        //
        /// <summary>
        /// Register or unregister the driver with the ASCOM Platform.
        /// This is harmless if the driver is already registered/unregistered.
        /// </summary>
        /// <param name="bRegister">If <c>true</c>, registers the driver, otherwise unregisters it.</param>
        private static void RegUnregASCOM(bool bRegister)
        {
            using (var p = new Profile())
            {
                p.DeviceType = driverType;
                if (bRegister)
                {
                    p.Register(sCsDriverId, sCsDriverDescription);
                }
                else
                {
                    p.Unregister(sCsDriverId);
                }
            }
        }

        /// <summary>
        /// This function registers the driver with the ASCOM Chooser and
        /// is called automatically whenever this class is registered for COM Interop.
        /// </summary>
        /// <param name="t">Type of the class being registered, not used.</param>
        /// <remarks>
        /// This method typically runs in two distinct situations:
        /// <list type="numbered">
        /// <item>
        /// In Visual Studio, when the project is successfully built.
        /// For this to work correctly, the option <c>Register for COM Interop</c>
        /// must be enabled in the project settings.
        /// </item>
        /// <item>During setup, when the installer registers the assembly for COM Interop.</item>
        /// </list>
        /// This technique should mean that it is never necessary to manually register a driver with ASCOM.
        /// </remarks>
#if ENABLE_COM
        [ComRegisterFunction]
#endif
        public static void RegisterASCOM(Type t)
        {
            RegUnregASCOM(true);
        }

        /// <summary>
        /// This function unregisters the driver from the ASCOM Chooser and
        /// is called automatically whenever this class is unregistered from COM Interop.
        /// </summary>
        /// <param name="t">Type of the class being registered, not used.</param>
        /// <remarks>
        /// This method typically runs in two distinct situations:
        /// <list type="numbered">
        /// <item>
        /// In Visual Studio, when the project is cleaned or prior to rebuilding.
        /// For this to work correctly, the option <c>Register for COM Interop</c>
        /// must be enabled in the project settings.
        /// </item>
        /// <item>During uninstall, when the installer unregisters the assembly from COM Interop.</item>
        /// </list>
        /// This technique should mean that it is never necessary to manually unregister a driver from ASCOM.
        /// </remarks>
#if ENABLE_COM
        [ComUnregisterFunction]
#endif
        public static void UnregisterASCOM(Type t)
        {
            RegUnregASCOM(false);
        }
#endregion

        
    }
}
