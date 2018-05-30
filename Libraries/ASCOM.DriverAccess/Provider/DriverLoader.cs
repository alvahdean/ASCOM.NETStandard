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

namespace ASCOM.DriverAccess
{
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
            tlog.LogMessage("AscomDriverLoader", $"Driver Root: {DriverRoot}");
        }
        #endregion

        #region Info routines
        public static string DriverRoot { get; private set; }
        public static IEnumerable<Type> Interfaces { get => _apiMap.Keys; }
        public static Type MapType(Type dType)
        {
            return dType.IsInterface
                ? _apiMap.Where(t => t.Key == dType).Select(t => t.Value).FirstOrDefault()
                : _apiMap.Where(t => t.Value == dType).Select(t => t.Key).FirstOrDefault();
        }
        public static Type InterfaceFor(Type cType) =>
            cType.IsInterface ? cType : MapType(cType);
        public static Type ApiTypeFor(Type cType) =>
             cType.IsInterface 
            ? MapType(cType)
            : IsApiType(cType)
                ? cType
                : ApiTypes.FirstOrDefault(t=>t.IsAssignableFrom(cType));
        public static bool IsApiType(Type cType) =>
            _apiMap?.Any(t => t.Value == cType) ?? false;
        public static bool Implements<T>(Type driverType) where T : class, IAscomDriver
        {
            return driverType.IsInterface
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
        #endregion

        #region Driver Registration
        private static int RegisterDrivers(bool forceReload = false)
        {
            tlog.LogMessage("RegisterDrivers", $"RegisterDrivers(forceReload={forceReload})");
            if (_deviceTypes == null || forceReload)
            {
                UnregisterDrivers();
                if (LoadAssemblies() == 0)
                {
                    tlog.LogMessage("RegisterDrivers", $"No driver assemblies found, no work to do...");
                    return 0;
                }
                _devInstances = new Dictionary<Type, IAscomDriver>();
                _deviceTypes = new Dictionary<Type, Assembly>();
                foreach (var da in _assemblies.Keys)
                {
                    tlog.LogMessage("RegisterDrivers", $"Loading drivers from {da.FullName}");
                    foreach (var dt in da.GetTypes().Where(t => t.GetTypeInfo().ImplementedInterfaces.Contains(typeof(IAscomDriver))))
                    {
                        tlog.LogMessage("RegisterDrivers", $"Registering Type '{dt.FullName}'");
                        if (_deviceTypes.Keys.Contains(dt))
                            throw new AscomException($"Duplicate driver type registration '{dt.FullName}' in  assembly {da.FullName}. Previous registration is from assemply {_deviceTypes[dt].FullName}");

                        _deviceTypes.Add(dt,da);
                        _devInstances.Add(dt, null);
                    }
                }
                //_deviceTypes = _assemblies
                //    .SelectMany(t => t.GetTypes())
                //    )
                //    .ToList();
            }
            return _devInstances.Count;

        }
        private static void UnregisterDrivers()
        {
            if (_deviceTypes !=null)
            {
                _deviceTypes.Clear();
                _deviceTypes = null;
            }
            if (_devInstances != null)
            {
                foreach (var inst in _devInstances.Values)
                    inst?.Dispose();
                _devInstances.Clear();
                _devInstances = null;
            }
        }
        #endregion

        #region Assembly management
        /// <summary>
        /// Returns assemblies loaded from inside of DriverRoot
        /// </summary>
        /// <returns>A list of Assemblies found in the DriverRoot path</returns>
        private static int LoadAssemblies(bool forceReload = false)
        {
            if (forceReload)
                UnloadAssemblies();
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

        #region Instance management
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

        #region Device Implementation methods
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
        #endregion

        #region API instance methods
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
        #endregion

    }
}