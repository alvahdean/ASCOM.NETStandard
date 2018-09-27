using ASCOM.DeviceInterface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using ASCOM.Utilities;
using RACI.Settings;
using Microsoft.Extensions.FileProviders;
using RACI.Data;

namespace ASCOM.DriverAccess
{

    //TODO: Implement locking while registering/unregistering types
    public static class DriverLoader
    {
        #region Internal fields
        private static Dictionary<Type, IAscomDriver> _devInstances;
        private static Dictionary<Type, Assembly> _deviceTypes;
        private static Dictionary<Assembly,string> _assemblies;
        private static Dictionary<Type,Type> _apiMap;
        private static TraceLogger tlog;
        #endregion

        #region Instance management
        static DriverLoader()
        {
            tlog = new TraceLogger("AscomDriverLoader");
#if DEBUG
            tlog.EchoDebug = true;
            tlog.EchoConsole = true;
#endif
            AppSettings appSettings = new AppSettings();
            _devInstances = new Dictionary<Type, IAscomDriver>();
            tlog.LogMessage("AscomDriverLoader", $"Initializing AscomDriverLoader...");
            _apiMap = new Dictionary<Type, Type>()
            {
                { typeof(ICameraV2),typeof(Camera) },
                { typeof(IDomeV2),typeof(Dome) },
                { typeof(IRotatorV2),typeof(Rotator) },
                { typeof(IFilterWheelV2),typeof(FilterWheel) },
                { typeof(IFocuserV2),typeof(Focuser) },
                { typeof(IObservingConditions),typeof(ObservingConditions) },
                { typeof(ISafetyMonitor),typeof(SafetyMonitor) },
                { typeof(ISwitchV2),typeof(Switch) },
                { typeof(ITelescopeV3),typeof(Telescope) },
                { typeof(IVideo),typeof(Video) }
            };
            DriverRoot = appSettings.PathSettings.DriverRoot?.Trim() ?? "";
            if (String.IsNullOrWhiteSpace(DriverRoot))
                DriverRoot = "Drivers";
            if (!Path.IsPathRooted(DriverRoot))
                DriverRoot = $"{appSettings.PathSettings.Application}\\{DriverRoot}";

        }
        public static void Init()
        {
            SystemHelper sh = new SystemHelper();
            foreach(var dtk in _apiMap.Values.Select(t=>$"{t.Name} Drivers"))
            {
                tlog.LogMessage("DriverLoader", $"Loading/Creating node for Ascom {dtk}");
                DriverTypeNode dn = sh.SubNode<DriverTypeNode>(sh.Ascom.ProfileNodeId, $"{dtk}", true);
                if (dn == null)
                {
                    tlog.LogMessage("DriverLoader", $"Failed to access Ascom {dtk}");
                    throw new DriverException("Driver loader initialization failed");
                }
                tlog.LogMessage("DriverLoader", $"Got handle to Ascom {dtk} [{dn.ProfileNodeId}]");
            }

            tlog.LogMessage("AscomDriverLoader", $"Driver Root: {DriverRoot}");
            tlog.LogMessage("AscomDriverLoader", $"Loading assemblies...");
            LoadAssemblies();
            tlog.LogMessage("AscomDriverLoader", $"{Assemblies.Count()} assemblies loaded");
            foreach (var a in Assemblies)
            {
                Version aver = a?.GetName().Version;
                tlog.LogMessage("AscomDriverLoader", $"\t {a.FullName} v{aver.Major}.{aver.Minor}.{aver.MajorRevision}.{aver.MinorRevision}");
            }
            tlog.LogMessage("AscomDriverLoader", $"Registering drivers...");
            RegisterDrivers();
            tlog.LogMessage("AscomDriverLoader", $"{DriverTypes.Count()} drivers loaded");
            foreach (var dt in DriverTypes)
            {
                tlog.LogMessage("AscomDriverLoader", $"\t {dt.FullName}");
            }
        }
        #endregion

        #region Info routines
        public static string DriverRoot { get; private set; }
        public static IEnumerable<Type> Interfaces { get => _apiMap.Keys; }
        public static Type MapType(Type dType)
        {
            return dType!=null
                ? dType.IsInterface
                    ? _apiMap.Where(t => t.Key == dType).Select(t => t.Value).FirstOrDefault()
                    : _apiMap.Where(t => t.Value == dType).Select(t => t.Key).FirstOrDefault()
                : null;
        }
        public static Type InterfaceFor(Type cType)
        {
            if (cType == null)
                return null;
            if (cType.IsInterface && Interfaces.Contains(cType))
                return cType;
            List<Type> cifList = cType.GetTypeInfo().ImplementedInterfaces.ToList() ?? new List<Type>();
            List<Type> ascomIfList = Interfaces.Where(t => cifList.Contains(t)).ToList();
            return ascomIfList.Count > 0
                ? ascomIfList[0]
                : null;
        }
        public static Type ApiTypeFor(Type cType)
        {
            Type ifType = InterfaceFor(cType);
            return MapType(ifType);
        }
        public static bool IsApiType(Type cType) =>
            _apiMap?.Any(t => t.Value == cType) ?? false;
        public static bool Implements<T>(Type driverType) where T : class, IAscomDriver
        {
            return typeof(T).IsInterface
                ? driverType.GetTypeInfo().ImplementedInterfaces.Contains(typeof(T))
                : driverType.IsAssignableFrom(typeof(T));
        }
        public static IEnumerable<Type> AscomInterfaces(Type driverType)
        {
            return driverType.GetTypeInfo().ImplementedInterfaces.Where(t => Interfaces.Contains(t));
        }
        public static bool IsDriver(Type driverType)
        {
            return Implements<IAscomDriver>(driverType);
        }
        public static Type DriverType(string driverName) => DriverTypes.FirstOrDefault(t => t.FullName == driverName);
        public static IEnumerable<Assembly> Assemblies
        {
            get
            {
                LoadAssemblies();
                return _assemblies.Keys;
            }
        }
        public static IEnumerable<Type> DriverTypes
        {
            get
            {
                RegisterDrivers();
                return _deviceTypes.Keys;
            }
        }
        public static IEnumerable<Type> ApiTypes => _apiMap.Values.AsEnumerable();
        public static bool DriverExists(String driverName) =>
            DriverTypes.Any(t => t.FullName == driverName);
        public static bool DriverExists(Type driverType) =>
            DriverExists(driverType.FullName);
        public static bool DriverExists<TDriver>() where TDriver : IAscomDriver =>
            DriverExists(typeof(TDriver));
        public static String DriverType(Type dType) => ApiTypeFor(dType)?.Name ?? "";
        #endregion

        #region Driver Registration
        private static int RegisterDrivers(bool forceReload = false)
        {
            if (LoadAssemblies() == 0)
            {
                tlog.LogMessage("RegisterDrivers", $"No driver assemblies found, no work to do...");
                return 0;
            }
            tlog.LogMessage("RegisterDrivers", $"RegisterDrivers(forceReload={forceReload})");
            if (_deviceTypes == null || forceReload)
            {
                UnregisterDrivers();
                _devInstances = new Dictionary<Type, IAscomDriver>();
                _deviceTypes = new Dictionary<Type, Assembly>();

                foreach (var da in _assemblies.Keys)
                {
                    tlog.LogMessage("RegisterDrivers", $"Loading drivers from {da.FullName}");
                    foreach (var dt in da.GetTypes())
                    {
                        tlog.LogMessage("RegisterDrivers", $"Examining type {dt.FullName}");
                        bool loadType = true;

                        try
                        {
                            if (dt.IsInterface || dt.IsValueType || dt.IsEnum)
                                loadType = false;
                            else
                            {
                                TypeInfo ti = dt.GetTypeInfo();
                                var ifList = ti.ImplementedInterfaces;
                                if (!ifList.Contains(typeof(IAscomDriver)))
                                    loadType = false;
                            }
                        }
                        catch(Exception ex)
                        { 
                            tlog.LogMessage("RegisterDrivers", $"Error examining type '{dt.FullName}'");
                            loadType = false;
                        }
                        if (!loadType)
                        {
                            tlog.LogMessage("RegisterDrivers", $"Skipping non-driver type {dt.FullName}");
                            continue;
                        }
                        if(!IsValidAscomType(dt))
                            tlog.LogMessage("RegisterDrivers", $"Skipping non-ASCOM type '{dt.FullName}'");
                        String drvType = DriverType(dt);
                        String drvName = dt.FullName;
                        String aName = da.FullName;

                        SystemHelper sh = new SystemHelper();
                        DriverTypeNode dn = sh.SubNode<DriverTypeNode>(sh.Ascom.ProfileNodeId, $"{drvType} Drivers", true);

                        tlog.LogMessage("RegisterDrivers", $"Registering Type '{drvName}'");
                        if (_deviceTypes.Keys.Contains(dt))
                            throw new AscomException($"Duplicate driver type registration '{drvName}' in  assembly {aName}. Previous registration is from assemply {_deviceTypes[dt].FullName}");

                        if (RegisterAscomProfile(dt))
                        {
                            _deviceTypes.Add(dt, da);
                            _devInstances.Add(dt, null);
                        }
                    }
                }

            }
            return _devInstances.Count;
        }
        private static bool IsValidAscomType(Type dt)
        {
            tlog.LogMessage(nameof(IsValidAscomType), $"Validating Driver type '{dt?.FullName}'");
            if (dt == null)
            {
                tlog.LogMessage(nameof(IsValidAscomType), $"Invalid Type 'null'");
                return false;
            }
            if (!IsDriver(dt))
            {
                tlog.LogMessage(nameof(IsValidAscomType), $"Invalid type: '{dt.FullName}': Does not implement the required ASCOM driver interfaces");
                return false;
            }
            String drvType = DriverType(dt);
            String drvName = dt.FullName;
            if (String.IsNullOrWhiteSpace(drvType))
            {
                tlog.LogMessage(nameof(IsValidAscomType), $"Invalid type: '{dt.FullName}': Cannot determine the ASCOM API Driver Type");
                return false;
            }
            return true;
        }
        private static bool RegisterAscomProfile(Type dt)
        {
            bool result = false;
            if(!IsValidAscomType(dt))
                return result;
            String drvType = DriverType(dt);
            String drvName = dt.FullName;
            Profile prf = new Profile(true) { DeviceType = drvType };
            if (prf.IsRegistered(drvName))
            {
                tlog.LogMessage(nameof(RegisterAscomProfile), $"Driver named '{drvName}' of type '{drvType}' is already registered, OK");
                result = true;
            }
            else
            {
                string regMethName = "RegisterASCOM";
                var regMeth = dt.GetMethod(regMethName);
                if (regMeth == null)
                {
                    tlog.LogMessage(nameof(RegisterAscomProfile), $"Cannot find registration method '{regMethName}' in driver type '{drvType}')");
                    tlog.LogMessage(nameof(RegisterAscomProfile), $"Trying registration via instance creation '{regMethName}' in driver type '{drvType}')");
                    var inst = CreateInstance(dt);
                    if (inst == null)
                        tlog.LogMessage(nameof(RegisterAscomProfile), $"Unable to create {drvName} instance");
                    if (!prf.IsRegistered(drvName))
                    {
                        string desc = inst?.Description ?? $"{drvName} Driver";
                        tlog.LogMessage(nameof(RegisterAscomProfile), $"Resorting to default registration via profile...");
                        prf.Register(drvName, desc);
                    }
                }
                else
                {
                    tlog.LogMessage(nameof(RegisterAscomProfile), $"Calling {dt.FullName}.{regMethName}");
                    regMeth.Invoke(null, new object[] { dt });
                }
            }
            tlog.LogMessage(nameof(RegisterAscomProfile), $"Checking for registration result for {dt.FullName}");
            result = prf.IsRegistered(drvName);
            string regName = $"{drvName} ({prf.DeviceType})";
            if (result)
                tlog.LogMessage(nameof(RegisterAscomProfile), $"Driver successfully registered: {regName}");
            else 
                tlog.LogMessage(nameof(RegisterAscomProfile), $"Failed to register driver: {regName}");
            return result;
        }
        private static void UnregisterDrivers(bool unregProfile = false)
        {
            if (_devInstances != null)
            {
                foreach (var inst in _devInstances?.Values)
                    inst?.Dispose();
                _devInstances?.Clear();
                _devInstances = null;
            }
            if (_deviceTypes != null)
            {
                List<Type> devTypes = _deviceTypes.Keys.ToList();
                foreach (var dt in devTypes)
                {
                    if (unregProfile)
                        UnregisterAscomProfile(dt);
                    _deviceTypes.Remove(dt);
                }
                _deviceTypes = null;
            }
        }
        private static bool UnregisterAscomProfile(Type dt)
        {
            if (!IsValidAscomType(dt))
                return false;
            String drvType = DriverType(dt);
            String drvName = dt.FullName;
            Profile prf = new Profile(true) { DeviceType = drvType };
            if (!prf.IsRegistered(drvName))
            {
                tlog.LogMessage(nameof(UnregisterAscomProfile), $"Driver named '{drvName}' of type '{drvType}' is not registered, OK");
                return true;
            }
            string regMethName = "UnregisterASCOM";
            var regMeth = dt.GetMethod(regMethName);
            if (regMeth == null)
            {
                tlog.LogMessage(nameof(UnregisterAscomProfile), $"Cannot find unregister method '{regMethName}' in driver type '{drvType}')");
                return false;
            }
            tlog.LogMessage(nameof(UnregisterAscomProfile), $"Calling {dt.FullName}.{regMethName}");
            regMeth.Invoke(null, new object[] { dt });
            if (prf.IsRegistered(drvName))
            {
                tlog.LogMessage(nameof(UnregisterAscomProfile), $"Driver successfully registered via {dt.FullName}.{regMethName}");
                return true;
            }
            tlog.LogMessage(nameof(UnregisterAscomProfile), $"Driver not registered after calling {dt.FullName}.{regMethName}");
            return false;
        }
        #endregion

        #region Assembly management
        /// <summary>
        /// Returns assemblies loaded from inside of DriverRoot
        /// </summary>
        /// <returns>A list of Assemblies found in the DriverRoot path</returns>
        private static int LoadAssemblies()
        {
            if (_assemblies != null)
                return _assemblies.Count;
            _assemblies = new Dictionary<Assembly, string>();
            DirectoryInfo binPath = new DirectoryInfo(DriverRoot);
            foreach (var fileSystemInfo in binPath.GetFileSystemInfos("*.dll"))
            {
                string dllPath = fileSystemInfo.FullName;
                tlog.LogMessage($"AscomDriverLoader.GetDriverAssemblies()", $"Loading assembly: '{dllPath}'");
                var a = Assembly.LoadFile(dllPath);
                //Assembly.Load(AssemblyName.GetAssemblyName(fileSystemInfo.FullName));
                //yield return assembly2;
                _assemblies.Add(a,dllPath);
            }
            return _assemblies.Count;
        }
        private static void UnloadAssemblies()
        {

            if (_assemblies == null)
                return;
            UnregisterDrivers();
            throw new NotImplementedException("Assembly unloading is not supported as of .NETCore 2.1");
            foreach (Assembly a in _assemblies.Keys)
            {
                //https://github.com/dotnet/coreclr/pull/8677#issuecomment-363958774
                //Assembly unloading is NOT supported in .NETCore!
                //a.Unload();
            }

        }
        #endregion

        #region Internal Driver creation
        private static IAscomDriver CreateInstance(Type driverType)
        {
            IAscomDriver driver = null;
            tlog.LogMessage("AscomDriverLoader", $"Creating instance of {driverType.FullName}");
            if (!DriverExists(driverType))
                throw new AscomException($"An assembly providing Driver '{driverType.FullName}' not found under '{DriverRoot}'");
            if (!IsDriver(driverType))
                throw new AscomException($"Type '{driverType.FullName}' is not an ASCOM Driver");
            driver = Activator.CreateInstance(driverType) as IAscomDriver;

            if (driver == null)
                tlog.LogMessage($"AscomDriverLoader.GetInstance<{driverType.Name}>()", $"Failed to create an instance of '{driverType.Name}'");
            return driver;
        }
        private static IAscomDriver CreateApiInstance(Type driverType)
        {
            IAscomDriver driver = null;
            tlog.LogMessage("AscomDriverLoader", $"Creating API instance for {driverType.FullName}");
            if (!DriverExists(driverType))
                throw new AscomException($"'{driverType.FullName}' is not a registered type");
            Type apiType = ApiTypeFor(driverType)
                ?? throw new AscomException($"Could not determine the API type for '{driverType.FullName}'");
            string driverName = driverType.FullName;

            driver = Activator.CreateInstance(apiType, new object[] { driverName }) as IAscomDriver;
            if (driver == null)
                tlog.LogMessage($"AscomDriverLoader.CreateApiInstance<{driverType.Name}>()", $"Failed to create an api object of Type '{apiType.FullName}' for class '{driverType.FullName}'");
            return driver;
        }
        #endregion

        #region Pubic Driver creation
        public static IAscomDriver GetImplementation(Type driverType)
        {
            if (driverType == null || !IsDriver(driverType) || !DriverExists(driverType))
                return null;
            if (_devInstances[driverType] == null)
                _devInstances[driverType] = CreateInstance(driverType);
            return _devInstances[driverType];
        }
        public static IAscomDriver GetImplementation(String driverName) => GetImplementation(DriverTypes.FirstOrDefault(t => t.FullName == driverName));
        public static TDriver GetImplementation<TDriver>(Type driverType) where TDriver : class, IAscomDriver => GetImplementation(driverType) as TDriver;
        public static TDriver GetImplementation<TDriver>(String driverName) where TDriver : class, IAscomDriver => GetImplementation(driverName) as TDriver;
        public static TDriver GetImplementation<TDriver>() where TDriver : IAscomDriver =>
            (TDriver)GetImplementation(typeof(TDriver));

        public static IAscomDriver GetApiDriver(IAscomDriver driver) => GetApiDriver(driver?.GetType());
        public static IAscomDriver GetApiDriver(String driverName) => GetApiDriver(DriverType(driverName));
        public static IAscomDriver GetApiDriver(Type driverType)
        {
            if (driverType == null || !DriverExists(driverType))
                throw new DriverException($"Cannot create API driver object. Invalid Driver instance type '{driverType?.FullName}'");
            if (IsApiType(driverType))
                throw new DriverException($"Source type '{driverType.FullName}' is an API type. A driver implementation type must be provided");

            IAscomDriver apiDriver = CreateApiInstance(driverType);

            if (apiDriver == null)
                throw new DriverException($"API driver instance creation failed for implementation '{driverType.FullName}'");
            return apiDriver;
        }

        public static TDriver GetApiDriver<TDriver>(IAscomDriver driver) where TDriver : class, IAscomDriver =>
            GetApiDriver(driver) as TDriver;
        public static TDriver GetApiDriver<TDriver>(Type driverType) where TDriver : class, IAscomDriver =>
            GetApiDriver(driverType) as TDriver;
        public static TDriver GetApiDriver<TDriver>(String driverName) where TDriver : class, IAscomDriver =>
            GetApiDriver(driverName) as TDriver;
        #endregion

    }
}