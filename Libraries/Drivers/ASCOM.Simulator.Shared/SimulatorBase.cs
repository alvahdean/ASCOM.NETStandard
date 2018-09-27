using ASCOM.DeviceInterface;
using ASCOM.DriverAccess;
using ASCOM.Utilities;
using RACI.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using ASCOM.Internal;
using System.Linq;
using ASCOM;

namespace xASCOM.Simulator
{
    internal class SimulatorBase<TDriver> where TDriver : class, IAscomDriver
    {
        #region Internal fields
        private SystemHelper _sys;
        private Dictionary<string,Func<string,string>> _supportedActions;
        private AscomDeviceNode _devNode;
        private string driverType;
        private string driverName;
        private TraceLogger _logger;
        #endregion

        #region Instance management
        internal SimulatorBase(TDriver driver) : this(driver, null) { }
        internal SimulatorBase(TDriver driver, TraceLogger log)
        {
            _supportedActions = new Dictionary<string, Func<string,string>>(new CIKeyComparer());
            Driver = driver;
            _logger = log;
            loadSettings();
        }
        #endregion

        #region Internal accessors
        private AscomDeviceNode dbDevice { get => _devNode ?? (_devNode = sys.AscomDevice(DriverType, DriverName)); }
        private SystemHelper sys { get => _sys ?? (_sys = new SystemHelper()); }
        #endregion

        #region IAsconDriver implementation

        public string Description { get; protected set; }

        public string DriverInfo { get; protected set; }

        public string DriverVersion { get; protected set; }

        public short InterfaceVersion { get; protected set; }

        public string DriverName
        {
            get
            {
                if (String.IsNullOrWhiteSpace(driverName))
                    driverName = Driver?.Name ?? (driverName = Driver?.GetType().FullName);
                return driverName;
            }
        }
        virtual public ArrayList SupportedActions { get => new ArrayList(_supportedActions.Keys); }

        virtual public string Action(string ActionName, string ActionParameters)
        {
            if (!_supportedActions.ContainsKey(ActionName))
                throw new ActionNotImplementedException(ActionName);
            if (_supportedActions[ActionName] == null)
                throw new ActionNotImplementedException(ActionName,new DriverException($"Action[{ActionName}] is registered but the implementation method is not set"));
            return _supportedActions[ActionName](ActionParameters);
        }

        virtual public void CommandBlind(string Command, bool Raw)
        {
            throw new System.NotImplementedException();
        }

        virtual public bool CommandBool(string Command, bool Raw)
        {
            throw new System.NotImplementedException();
        }

        virtual public string CommandString(string Command, bool Raw)
        {
            throw new System.NotImplementedException();
        }

        virtual public void SetupDialog()
        {
            throw new System.NotImplementedException();
        }

        virtual public void Dispose()
        {
            saveSettings();
        }
        #endregion

        #region Utility methods
        internal static void RegisterASCOM(Type implType,string description=null, bool unRegister=false)
        {
            using (var p = new Profile())
            {
                string driverId = implType.FullName;
                p.DeviceType = DriverLoader.ApiTypeFor(implType).Name;
                if (unRegister)
                    p.Unregister(implType.FullName);
                else
                {
                    if (String.IsNullOrWhiteSpace(description))
                        description = description?.Trim()??String.Empty;
                    p.Register(driverId, description);
                }
            }
        }
        internal TDriver Driver { get; set; }
        internal bool RetainState { get; set; }
        internal bool RetainCapabilities { get; set; }
        internal TraceLogger Logger { get => _logger ?? (_logger = new TraceLogger(typeof(TDriver).FullName, "Simulator")); }
        internal string DriverType { get => driverType ?? (driverType = DriverLoader.ApiTypeFor(typeof(TDriver)).Name); }
        internal void AddSuppoertedAction(string name,Func<string,string> impl)
        {
            if (_supportedActions.ContainsKey(name))
                _supportedActions[name] = impl;
            else
                _supportedActions.Add(name, impl);
        }
        internal string GetVal(String path) => sys.ValueByPath(dbDevice, path);
        internal string GetVal(String path,string defaultValue) => sys.ValueByPath(dbDevice, path,defaultValue);
        internal TValue GetVal<TValue>(String path) where TValue : struct => sys.ValueByPath<TValue>(dbDevice, path);
        internal TValue GetVal<TValue>(String path, TValue defaultValue)
        {
            Type vType = typeof(TValue);
            string sDefault = vType.IsEnum
                ? defaultValue.ToString()
                : JsonConvert.SerializeObject(defaultValue);
            string value=GetVal(path, sDefault)?.Trim()??String.Empty;

            return value==String.Empty
                ? defaultValue
                : vType.IsEnum
                    ? (TValue)Enum.Parse(vType,value)
                    : JsonConvert.DeserializeObject<TValue>(value);
        }
        internal void SetVal(String path, string value) => sys.SetValueByPath(dbDevice, path, value?.Trim()??String.Empty);
        internal void SetVal<TValue>(String path, TValue value)
        {
            Type vType = typeof(TValue);
            string sVal = vType.IsEnum
                ? value.ToString()
                : JsonConvert.SerializeObject(value);
            SetVal(path, sVal);
        }

        private IProfileNode _capabilitiesNode = null;
        internal IProfileNode CapabilitiesNode { get => _capabilitiesNode??(_capabilitiesNode=sys.SubNode(dbDevice,"Capabilities",false)); }
        private IProfileNode _stateNode = null;
        internal IProfileNode StateNode { get => _stateNode ?? (_stateNode = sys.SubNode(dbDevice, "State", false)); }
        internal void SetCapability(String key, bool value)
        {
            key = key?.Trim() ?? "";
            if (key == "")
                throw new ArgumentException("Capability key cannot be empty");

            string path = $"Capabilities\\{key}";
            SetVal(path, value);
        }
        internal bool GetCapability(String key, bool defaultValue)
        {
            key = key?.Trim() ?? "";
            if (key == "")
                throw new ArgumentException("Capability key cannot be empty");

            string path = $"Capabilities\\{key}";
            return GetVal(path, defaultValue);
        }
        internal void ClearCapabilities()
        {

            IProfileNode node = CapabilitiesNode;
            if(node!=null)
                sys.DeleteNode(node.ProfileNodeId);
        }
        internal void ClearState()
        {
            IProfileNode node = StateNode;
            if (node != null)
                sys.DeleteNode(node.ProfileNodeId);
        }
        internal void SetState<TValue>(String key, TValue value)
        {
            key = key?.Trim() ?? "";
            if (key == "")
                throw new ArgumentException("State key cannot be empty");

            string path = $"State\\{key}";
            string json = JsonConvert.SerializeObject(value);
            SetVal(path, json);
        }
        internal TValue GetState<TValue>(String key, TValue defaultValue)
        {
            key = key?.Trim() ?? "";
            if (key == "")
                throw new ArgumentException("State key cannot be empty");

            string path = $"State\\{key}";

            
            TValue result = defaultValue;
            try
            {
                result=GetVal(path, defaultValue);
            }
            catch(Exception ex)
            {
                Logger.LogMessage("GetState", $"[{ex.GetType().Name}]: Error getting state value '{key}', returning defaultValue '{result}'");
                Logger.LogMessage("GetState", $"Exception Detail: {ex.Message}");
                result = defaultValue;
            }
            return result;
        }

        virtual internal void loadSettings()
        {
            Version aVer = Driver.GetType().Assembly.GetName().Version;
            InterfaceVersion = GetVal<short>("InterfaceVersion", 2);
            Description = GetVal("", $"{DriverName} Driver");
            DriverInfo = GetVal("DriverInfo",  Description);
            DriverVersion = GetVal("DriverVersion",$"{aVer.Major}.{aVer.Minor}");
            RetainCapabilities = GetVal("RetainCapabilities", false);
            RetainState = GetVal("RetainState", false);
            _supportedActions.Clear();
            if (!RetainCapabilities)
                ClearCapabilities();
            if (!RetainState)
                ClearState();
            else
            {
                string[] sa = GetState("SupportedActions", "").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in sa)
                    AddSuppoertedAction(item, null);
            }
        }
        virtual internal void saveSettings()
        {
            Version aVer = Driver.GetType().Assembly.GetName().Version;
            SetVal<short>("InterfaceVersion", InterfaceVersion);
            SetVal("", Description);
            SetVal("Description", Description);
            SetVal("DriverInfo", DriverInfo);
            SetVal("DriverVersion", $"{aVer.Major}.{aVer.Minor}");
            SetVal("RetainCapabilities", RetainCapabilities);
            SetVal("RetainState", RetainState);
            if(RetainState)
                SetState("SupportedActions", String.Join(",", SupportedActions.ToStrings()));
        }
        #endregion
    }
}
