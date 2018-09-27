using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using ASCOM;
using ASCOM.DeviceInterface;
using Microsoft.Extensions.Logging;
using RACI.Data;
using RestSharp;

namespace RACI.Client
{
    public class DomeDriver : RestDriver,  IDomeV2
    {
        #region Internal 
        private int _updateInterval=500;
        private double _altitude;
        private double _azimuth;
        private bool _slewing;
        private bool _slaved;
        private bool _atHome;
        private bool _atPark;
        private ShutterState _shutterStatus;
        private double? _targetAltitude;
        private double? _targetAzimuth;    
        private bool _canFindHome;
        private bool _canPark;
        private bool _canSetAltitude;
        private bool _canSetAzimuth;
        private bool _canSetPark;
        private bool _canSetShutter;
        private bool _canSlave;
        private bool _canSyncAzimuth;
        #endregion

        #region Instance management
        public DomeDriver() : base("", "") { }
        public DomeDriver(RaciEndpoint ep,string driverName) : base(ep?.ServiceRoot, driverName) { }
        public DomeDriver(string epUrl, string driverName) : base(epUrl, driverName) { }
        #endregion

        #region Internal methods
        override protected void clearDriverState()
        {
            base.clearDriverState();
            Altitude = Double.NaN;
            Azimuth = Double.NaN;
            TargetAltitude= Double.NaN;
            TargetAzimuth= Double.NaN;
            Slaved = false;
            Slewing = false;
            AtPark = false;
            AtHome = false;
            ShutterStatus = ShutterState.shutterClosed;

            CanFindHome = false;
            CanPark = false;
            CanSetAltitude = false;
            CanSetAzimuth = false;
            CanSetPark = false;
            CanSetShutter = false;
            CanSlave = false;
            CanSyncAzimuth = false;
        }
        protected override async Task updateStateAsync(CancellationToken? token = null)
        {
            double tomillis = 500;
            DateTime di = DateTime.Now;
            double dt = 0;
            CancellationTokenSource tokenSource = new CancellationTokenSource(TimeSpan.FromMilliseconds(tomillis));
            if (!token.HasValue)
            {
                token = tokenSource.Token;
            }
            dt = (DateTime.Now - di).TotalMilliseconds;
            Logger.LogInformation($"{dt}ms: Updating base driver state...");
            await base.updateStateAsync(token);

            dt = (DateTime.Now - di).TotalMilliseconds;
            Logger.LogInformation($"{dt}ms: Updating Altitudee...");
            await RascomExecuteAsync<double>(
                CreateRequest<double>(Method.GET, $"devices/{Name}/Altitude"),
                (result) => { Altitude = result; }, token);

            dt = (DateTime.Now - di).TotalMilliseconds;
            Logger.LogInformation($"{dt}ms: Updating Azimuth...");
            await RascomExecuteAsync<double>(
                CreateRequest<double>(Method.GET, $"devices/{Name}/Azimuth"),
                (result) => { Azimuth = result; }, token);

            dt = (DateTime.Now - di).TotalMilliseconds;
            Logger.LogInformation($"{dt}ms: Updating Slaved...");
            await RascomExecuteAsync<bool>(
                CreateRequest<bool>(Method.GET, $"devices/{Name}/Slaved"),
                (result) => { Slaved = result; }, token);


            dt = (DateTime.Now - di).TotalMilliseconds;
            Logger.LogInformation($"{dt}ms: Updating Slewing...");
            await RascomExecuteAsync<bool>(
                CreateRequest<bool>(Method.GET, $"devices/{Name}/Slewing"),
                (result) => { Slewing = result; }, token);


            dt = (DateTime.Now - di).TotalMilliseconds;
            Logger.LogInformation($"{dt}ms: Updating ShutterStatus...");
            await RascomExecuteAsync<ShutterState>(
                CreateRequest<ShutterState>(Method.GET, $"devices/{Name}/ShutterStatus"),
                (result) => { ShutterStatus = result; }, token);

            dt = (DateTime.Now - di).TotalMilliseconds;
            Logger.LogInformation($"{dt}ms: Updating AtPark...");
            await RascomExecuteAsync<bool>(
                CreateRequest<bool>(Method.GET, $"devices/{Name}/AtPark"),
                (result) => { AtPark = result; }, token);

            dt = (DateTime.Now - di).TotalMilliseconds;
            Logger.LogInformation($"{dt}ms: Updating AtHome...");
            await RascomExecuteAsync<bool>(
                CreateRequest<bool>(Method.GET, $"devices/{Name}/AtHome"),
                (result) => { AtHome = result; }, token);

            dt = (DateTime.Now - di).TotalMilliseconds;
            Logger.LogInformation($"{dt}ms: State update complate...");
        }
        override protected void updateCapabilities()
        {
            DateTime di = DateTime.Now;
            double dt = 0;
            Logger.LogInformation($"Updating Capabilities...");

            dt = (DateTime.Now - di).TotalMilliseconds;
            Logger.LogInformation($"{dt}ms: Updating CanFindHome");
            CanFindHome = getRestProperty<bool>($"CanFindHome");
            //CanFindHome = RascomGet<bool>($"devices/{Name}/CanFindHome");
            dt = (DateTime.Now - di).TotalMilliseconds;
            Logger.LogInformation($"{dt}ms: Updating CanPark");
            CanPark = getRestProperty<bool>($"CanPark");
            //CanPark = RascomGet<bool>($"devices/{Name}/CanPark");

            dt = (DateTime.Now - di).TotalMilliseconds;
            Logger.LogInformation($"{dt}ms: Updating CanSetAltitude");
            CanSetAltitude = getRestProperty<bool>($"CanSetAltitude");
            //CanSetAltitude = RascomGet<bool>($"devices/{Name}/CanSetAltitude");

            dt = (DateTime.Now - di).TotalMilliseconds;
            Logger.LogInformation($"{dt}ms: Updating CanSetAzimuth");
            CanSetAzimuth = getRestProperty<bool>($"CanSetAzimuth");
            //CanSetAzimuth = RascomGet<bool>($"devices/{Name}/CanSetAzimuth");

            dt = (DateTime.Now - di).TotalMilliseconds;
            Logger.LogInformation($"{dt}ms: Updating CanSetPark");
            CanSetPark = getRestProperty<bool>($"CanSetPark");
            //CanSetPark = RascomGet<bool>($"devices/{Name}/CanSetPark");

            dt = (DateTime.Now - di).TotalMilliseconds;
            Logger.LogInformation($"{dt}ms: Updating CanSetShutter");
            CanSetShutter = getRestProperty<bool>($"CanSetShutter");
            //CanSetShutter = RascomGet<bool>($"devices/{Name}/CanSetShutter");

            dt = (DateTime.Now - di).TotalMilliseconds;
            Logger.LogInformation($"{dt}ms: Updating CanSlave");
            CanSlave = getRestProperty<bool>($"CanSlave");
            //CanSlave = RascomGet<bool>($"devices/{Name}/CanSlave");

            dt = (DateTime.Now - di).TotalMilliseconds;
            Logger.LogInformation($"{dt}ms: Updating CanSyncAzimuth");
            CanSyncAzimuth = getRestProperty<bool>($"CanSyncAzimuth");
            //CanSyncAzimuth = RascomGet<bool>($"devices/{Name}/CanSyncAzimuth");

            dt = (DateTime.Now - di).TotalMilliseconds;
            Logger.LogInformation($"{dt}ms: Updating CanFindHome");
            CanFindHome = getRestProperty<bool>($"CanFindHome");
            //CanFindHome = RascomGet<bool>($"devices/{Name}/CanFindHome");

            Logger.LogInformation($"{dt}ms: Capabilities update complate...");
        }
        override protected void updateMetadata()
        {
            DateTime di = DateTime.Now;
            double dt = 0;
            Logger.LogInformation($"Updating Metadata...");
            dt = (DateTime.Now - di).TotalMilliseconds;
            Logger.LogInformation($"{dt}ms: Metadata update complate...");
        }
        #endregion

        #region Non-ASCOM properties
        public double? TargetAltitude
        {
            get => _targetAltitude;
            set
            {
                if (TargetAltitude != value)
                {
                    _targetAltitude = value;
                    RaisePropertyChanged("TargetAltitude");
                }
            }
        }
        public double? TargetAzimuth
        {
            get => _targetAzimuth;
            set
            {
                if (TargetAzimuth != value)
                {
                    _targetAzimuth = value;
                    RaisePropertyChanged("TargetAzimuth");
                }
            }
        }
        #endregion

        #region ASCOM API

        #region State properties       
        public double Altitude
        {
            get => _altitude;
            private set
            {
                if (Altitude != value)
                {
                    _altitude = value;
                    RaisePropertyChanged("Altitude");
                }
            }
        }
        public double Azimuth
        {
            get => _azimuth;
            private set
            {
                if (Azimuth != value)
                {
                    _azimuth = value;
                    RaisePropertyChanged("Azimuth");
                }
            }
        }
        public bool Slaved
        {
            get => _slaved;
            set
            {
                if (Slaved != value)
                {
                    setRestProperty("Slaved", value);
                    _slaved = value;
                    RaisePropertyChanged("Slaved");
                }
            }
        }
        public bool Slewing
        {
            get => _slewing;
            protected set
            {
                if (Slewing != value)
                {
                    _slewing = value;
                    RaisePropertyChanged("Slewing");
                }
            }
        }
        public ShutterState ShutterStatus
        {
            get => _shutterStatus;
            protected set
            {
                if (ShutterStatus != value)
                {
                    _shutterStatus = value;
                    RaisePropertyChanged("ShutterStatus");
                }
            }
        }
        public bool AtHome
        {
            get => _atHome;
            protected set
            {
                if (AtHome != value)
                {
                    _atHome = value;
                    RaisePropertyChanged("AtHome");
                }
            }
        }
        public bool AtPark
        {
            get => _atPark;
            protected set
            {
                if (AtPark != value)
                {
                    _atPark = value;
                    RaisePropertyChanged("AtPark");
                }
            }
        }
        #endregion

        #region Capabilities
        public bool CanFindHome
        {
            get => _canFindHome;
            protected set
            {
                if (AtPark != value)
                {
                    _atPark = value;
                    RaisePropertyChanged("AtPark");
                }
            }
        }
        public bool CanPark
        {
            get => _canPark;
            protected set
            {
                if (CanPark != value)
                {
                    _canPark = value;
                    RaisePropertyChanged("CanPark");
                }
            }
        }
        public bool CanSetAltitude
        {
            get => _canSetAltitude;
            protected set
            {
                if (CanSetAltitude != value)
                {
                    _canSetAltitude = value;
                    RaisePropertyChanged("CanSetAltitude");
                }
            }
        }
        public bool CanSetAzimuth
        {
            get => _canSetAzimuth;
            protected set
            {
                if (CanSetAzimuth != value)
                {
                    _canSetAzimuth = value;
                    RaisePropertyChanged("CanSetAzimuth");
                }
            }
        }
        public bool CanSetPark
        {
            get => _canSetPark;
            protected set
            {
                if (CanSetPark != value)
                {
                    _canSetPark = value;
                    RaisePropertyChanged("CanSetPark");
                }
            }
        }
        public bool CanSetShutter
        {
            get => _canSetShutter;
            protected set
            {
                if (CanSetShutter != value)
                {
                    _canSetShutter = value;
                    RaisePropertyChanged("CanSetShutter");
                }
            }
        }
        public bool CanSlave
        {
            get => _canSlave;
            protected set
            {
                if (CanSlave != value)
                {
                    _canSlave = value;
                    RaisePropertyChanged("CanSlave");
                }
            }
        }
        public bool CanSyncAzimuth
        {
            get => _canSyncAzimuth;
            protected set
            {
                if (CanSyncAzimuth != value)
                {
                    _canSyncAzimuth = value;
                    RaisePropertyChanged("CanSyncAzimuth");
                }
            }
        }
        #endregion

        #region Method calls
        public void AbortSlew()
        {
            Func<bool> isDone = () => { return !Slewing; };
            if (!isDone())
            {
                RascomPost<string>($"devices/{Name}/AbortSlew");
                TimeSpan uIntvl = TimeSpan.FromMilliseconds(500);
                TimeSpan timeout = TimeSpan.FromMilliseconds(10000);
                monitorState(uIntvl, TimeSpan.Zero, isDone);
            }
        }
        public void CloseShutter()
        {
            Func<bool> isDone = () => { return ShutterStatus == ShutterState.shutterClosed; };
            if (!isDone())
            {
                RascomPost<string>($"devices/{Name}/CloseShutter");
                TimeSpan uIntvl = TimeSpan.FromMilliseconds(500);
                TimeSpan timeout = TimeSpan.FromMilliseconds(10000);
                monitorState(uIntvl, TimeSpan.Zero, isDone);
            }
        }
        public void FindHome()
        {
            Func<bool> isDone = () => { return AtHome; };
            if (!isDone())
            {
                RascomPost<string>($"devices/{Name}/FindHome");
                TimeSpan uIntvl = TimeSpan.FromMilliseconds(500);
                TimeSpan timeout = TimeSpan.FromMilliseconds(10000);
                monitorState(uIntvl, TimeSpan.Zero, isDone);
            }
        }
        public void OpenShutter()
        {
            Func<bool> isDone = () => { return ShutterStatus == ShutterState.shutterOpen; };
            if (!isDone())
            {
                RascomPost<string>($"devices/{Name}/OpenShutter");
                TimeSpan uIntvl = TimeSpan.FromMilliseconds(500);
                TimeSpan timeout = TimeSpan.FromMilliseconds(10000);
                monitorState(uIntvl, TimeSpan.Zero, isDone);
            }

        }
        public void Park()
        {
            Func<bool> isDone = () => { return AtPark; };
            if (!isDone())
            {
                RascomPost<string>($"devices/{Name}/Park");
                TimeSpan uIntvl = TimeSpan.FromMilliseconds(500);
                TimeSpan timeout = TimeSpan.FromMilliseconds(10000);
                monitorState(uIntvl, TimeSpan.Zero, isDone);
            }
        }
        public void SetPark()
        {
            RascomPost<string>($"devices/{Name}/SetPark");
        }
        public void SlewToAltitude(double alt)
        {
            TargetAltitude = alt;
            Func<bool> isDone = () => { return Math.Abs(TargetAltitude.Value-Altitude)<1.0; };
            if (!isDone())
            {
                RascomPost<string>($"devices/{Name}/SlewToAltitude/{Altitude}");
                TimeSpan uIntvl = TimeSpan.FromMilliseconds(500);
                TimeSpan timeout = TimeSpan.FromMilliseconds(10000);
                monitorState(uIntvl, TimeSpan.Zero, isDone);
            }
        }
        public void SlewToAzimuth(double az)
        {
            TargetAzimuth = az;
            Func<bool> isDone = () => { return Math.Abs(TargetAzimuth.Value - Azimuth) < 1.0; };
            if (!isDone())
            {
                RascomPost<string>($"devices/{Name}/SlewToAltitude/{az}");
                TimeSpan uIntvl = TimeSpan.FromMilliseconds(500);
                TimeSpan timeout = TimeSpan.FromMilliseconds(10000);
                monitorState(uIntvl, TimeSpan.Zero, isDone);
            }
        }
        public void SyncToAzimuth(double az)
        {
            TargetAzimuth = az;
            Func<bool> isDone = () => { return Math.Abs(TargetAzimuth.Value - Azimuth) < 1.0; };
            if (!isDone())
            {
                RascomPost<string>($"devices/{Name}/SyncToAzimuth/{Azimuth}");
                TimeSpan uIntvl = TimeSpan.FromMilliseconds(500);
                TimeSpan timeout = TimeSpan.FromMilliseconds(10000);
                monitorState(uIntvl, TimeSpan.Zero, isDone);
            }
        }
        #endregion

        #endregion
    }
}
