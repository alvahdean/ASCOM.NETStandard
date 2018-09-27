using ASCOM;
using ASCOM.DeviceInterface;
using ASCOM.Internal;
using ASCOM.Utilities;
#if NETCORE_2_0
using ASCOM.Internal;
using ASCOM.DriverAccess;
#endif

using Microsoft.Extensions.Logging;
using RACI.Data;
using RACI.Data.RestObjects;
using RestSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RACI.Client
{
    public class RestDriver : RaciClient, IAscomDriver, INotifyPropertyChanged
    //where TDriver: IAscomDriver
    {
        #region Internal 
        private int _updateInterval;
        private int _timeoutTx;
        private int _timeoutUpdate;
        private bool _isConnected;
        private string _name;
        private string _description;
        private string _driverInfo;
        private string _driverVersion;
        private short _interfaceVersion;
        private ObservableCollection<string> _supportedActions;
        //private bool qrySvcConnected() { return RascomGet<bool>($"devices/{_driverName}/Connected"); }
        #endregion

        #region Instance management
        public RestDriver() : this("", "") { }
        public RestDriver(RaciEndpoint ep, string driverName) : this(ep?.ServiceRoot, driverName) { }
        public RestDriver(string epUrl, string driverName)
            : base(epUrl, driverName)
        {
            _name = driverName?.Trim() ?? "";
            if (IsConfigured)
                ConnectService(epUrl, driverName);
            else
                Logger.LogInformation("Connection info not specified, skipping initial connect");
        }
        #endregion

        #region Internal state utilities
        virtual protected void clearDriverState()
        {
            Description = "";
            DriverInfo = "";
            DriverVersion = "";
            InterfaceVersion = 0;
            SupportedActions.Clear();
        }
        protected void monitorState(TimeSpan interval, TimeSpan timeout) =>
            monitorState(interval, timeout, null);
        protected void monitorState(TimeSpan interval, Func<bool> until) =>
            monitorState(interval, TimeSpan.Zero, until);
        protected void monitorState(Func<bool> until) =>
            monitorState(TimeSpan.FromMilliseconds(UpdateInterval), until);
        protected bool monitorState(TimeSpan interval, TimeSpan timeout, Func<bool> until)
        {
            CancellationTokenSource cancelToken = new CancellationTokenSource();
            DateTime timeoutDate = DateTime.Now.Add(timeout);
            bool haveUntil = until != null;
            if (!haveUntil)
                until = () => { return false; };
            bool done = haveUntil && until();
            while (!done && (timeout == TimeSpan.Zero || DateTime.Now <= timeoutDate))
            {
                updateState();
                Thread.Sleep((int)interval.TotalMilliseconds);
                done = haveUntil && until();
            }
            bool timedOut = (timeout != TimeSpan.Zero && DateTime.Now >= timeoutDate);

            return !timedOut && (!haveUntil || done);
        }

        //TODO: provide method to send multiple requests in parallel
        protected void updateState(CancellationToken? token = null) => updateStateAsync(token).RunSynchronously();
        virtual protected async Task updateStateAsync(CancellationToken? token = null)
        {

            Func<bool, bool> cbFunc = (result) => { Connected = result; return Connected == result; };
            bool success = false;
            try
            {
                success = await RascomExecuteAsync(CreateRequest<bool>(Method.GET, $"devices/{Name}/Connected"), cbFunc, token);
                if (!success)
                    throw new ApplicationException($"Request completed but validation failed");
                if (!Connected)
                {
                    Logger.LogInformation($"[Update.State.Connect] Driver is disconnected");
                    Logger.LogInformation($"[Update.State.Connect] \tClearing driver state");
                    clearDriverState();
                    Logger.LogInformation($"[Update.State.Connect] \tSkipping remaining driver state checks...");
                    return;
                }
            }
            catch (Exception ex)
            {
                Logger.LogWarning($"[Update.State] Exception:[{ex.GetType().Name}] => {ex.Message}");
                throw;
            }
        }
        virtual protected void updateCapabilities()
        {
            //
            var saList = getRestProperty<List<string>>($"SupportedActions");
            //var currList = SupportedActions.ToList();
            //SupportedActions automatically enumerated by Except before operating on it...
            foreach (var sa in saList.Except(SupportedActions.ToStrings()))
                SupportedActions.Add(sa);
            //MUST enumerate actions before operating on it...
            foreach (var sa in SupportedActions.ToStrings().Except(saList).ToList())
                SupportedActions.Remove(sa);
        }
        virtual protected void updateMetadata()
        {
            Name = getRestProperty<string>($"Name");
            Description = getRestProperty<string>($"Description");
            DriverInfo = getRestProperty<string>($"DriverInfo");
            DriverVersion = getRestProperty<string>($"DriverVersion");
            InterfaceVersion = getRestProperty<short>($"InterfaceVersion");
        }
        virtual protected T getRestProperty<T>(string propName)
        {

            if (String.IsNullOrWhiteSpace(propName))
                throw new ApplicationException($"Invalid property name '{propName}'");
            EnsureDriverConfigured();
            string reqUrl = $"{DriverUrl}/{propName}";
            return RascomGet<T>(reqUrl);
        }
        virtual protected T setRestProperty<T>(string propName, T value)
        {
            if (String.IsNullOrWhiteSpace(propName))
                throw new ApplicationException($"Invalid property name '{propName}'");
            EnsureDriverConfigured();
            string reqUrl = $"{DriverUrl}/{propName}/{value}";
            return RascomPost<T>(reqUrl);
        }
        #endregion

        #region Non-ASCOM properties
        public int UpdateInterval
        {
            get => _updateInterval;
            set
            {
                if (UpdateInterval != value)
                {
                    _updateInterval = value;
                    RaisePropertyChanged("UpdateInterval");
                }
            }
        }
        public int TimeoutTx
        {
            get => _timeoutTx;
            set
            {
                if (TimeoutTx != value)
                {
                    _timeoutTx = value;
                    RaisePropertyChanged("TimeoutTx");
                }
            }
        }
        public int TimeoutUpdate
        {
            get => _timeoutUpdate;
            set
            {
                if (TimeoutUpdate != value)
                {
                    _timeoutUpdate = value;
                    RaisePropertyChanged("TimeoutUpdate");
                }
            }
        }
        #endregion

        #region ASCOM API
        virtual public bool Connected
        {
            get => _isConnected;
            set
            {
                if (Connected != value)
                {
                    Logger.LogInformation($"[{Name}.Connected]: Changing connection state {Connected} => {value}");
                    _isConnected = RascomPost<bool>($"devices/{Name}/Connected/{value}");
                    if (Connected == value)
                    {
                        Logger.LogInformation($"[{Name}.Connected]: Connection state updated! Connected = {Connected}");
                        RaisePropertyChanged("Connected");
                        if (Connected)
                        {
                            Logger.LogInformation($"[{Name}.Connected]: Connected: updating local state");
                            Logger.LogInformation($"[{Name}.Connected]: \tMetadata...");
                            updateMetadata();
                            Logger.LogInformation($"[{Name}.Connected]: \tCapabilities...");
                            updateCapabilities();
                            Logger.LogInformation($"[{Name}.Connected]: \tState...");
                            updateState();
                        }
                        else
                        {
                            Logger.LogInformation($"[{Name}.Connected]: Disonnected: clearing driver information");
                            clearDriverState();
                        }
                    }
                    else
                        Logger.LogWarning($"[{Name}.Connected]: Failed to change state Connected = {Connected}");
                }
            }
        }
        virtual public string Name
        {
            get => _name;
            set
            {
                if (Name != value)
                {
                    _name = value;
                    RaisePropertyChanged("Name");
                }
            }

        }
        virtual public string Description
        {
            get => _description;
            set
            {
                if (Description != value)
                {
                    _description = value;
                    RaisePropertyChanged("Description");
                }
            }
        }
        virtual public string DriverInfo
        {
            get => _driverInfo;
            set
            {
                if (DriverInfo != value)
                {
                    _driverInfo = value;
                    RaisePropertyChanged("DriverInfo");
                }
            }

        }
        virtual public string DriverVersion
        {
            get => _driverVersion;
            set
            {
                if (DriverVersion != value)
                {
                    _driverVersion = value;
                    RaisePropertyChanged("DriverVersion");
                }
            }
        }
        virtual public short InterfaceVersion
        {
            get => _interfaceVersion;
            set
            {
                if (InterfaceVersion != value)
                {
                    _interfaceVersion = value;
                    RaisePropertyChanged("InterfaceVersion");
                }
            }
        }
        virtual public ArrayList SupportedActions
        {
            get
            {
                return _supportedActions != null 
                    ? new ArrayList(_supportedActions) 
                    : new ArrayList();
            }
            protected set
            {
                if (_supportedActions == null)
                    _supportedActions = new ObservableCollection<string>();
                _supportedActions.Clear();
                if (value != null && value.Count > 0)
                    foreach (var sa in value)
                        _supportedActions.Add(sa.ToString());
                RaisePropertyChanged("SupportedActions");
            }
        }
        virtual public string Action(string ActionName, string ActionParameters)
        {
            return RascomPost<string>($"devices/{Name}/action/{ActionName}/{ActionParameters}");
        }
        virtual public void CommandBlind(string Command, bool Raw)
        {
            RascomPost<string>($"devices/{Name}/CommandBlind/{Command}");
        }
        virtual public bool CommandBool(string Command, bool Raw)
        {
            return RascomPost<bool>($"devices/{Name}/CommandBool/{Command}/{Raw}");
        }
        virtual public string CommandString(string Command, bool Raw)
        {
            return RascomPost<string>($"devices/{Name}/CommandString/{Command}/{Raw}");
        }
        virtual public void SetupDialog()
        {
            System.Diagnostics.Process.Start($"{Driver.Parent.ServiceRoot}/devices/{Name}/SetupDialog");
        }
        #endregion

        #region IDisposable Support
        protected bool disposedValue = false; // To detect redundant calls
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (Connected)
                        Connected = false;
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }
        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~RestDriver() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged(string propName)
        {
            Logger.LogDebug($"Property[{propName}] changed.");
            PropertyChangedEventArgs args = new PropertyChangedEventArgs(propName);
            PropertyChanged?.Invoke(this, args);
        }
        #endregion

    }
}
