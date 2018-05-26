using ASCOM.DeviceInterface;
using Microsoft.Extensions.DependencyModel;
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
    public static class AscomDriverLoader
    {
        private static Dictionary<Type, IAscomDriver> _devInstances;
        private static IList<Type> _deviceTypes;
        private static TraceLogger tlog;

        public static string DriverRoot { get; private set; }

        static AscomDriverLoader()
        {
            tlog = new TraceLogger("AscomDriverLoader");
            AppSettings appSettings = new AppSettings();
            _devInstances = new Dictionary<Type, IAscomDriver>();
            tlog.LogMessage("AscomDriverLoader", $"Creating AscomDriverLoader instance");
            DriverRoot = appSettings.PathSettings.DriverRoot?.Trim()??"";
            if (String.IsNullOrWhiteSpace(DriverRoot))
                DriverRoot = "Drivers";
            if (!Path.IsPathRooted(DriverRoot))
                DriverRoot = $"{appSettings.PathSettings.Application}\\{DriverRoot}";
            tlog.LogMessage("AscomDriverLoader", $"Driver Root: {DriverRoot}");
        }

        private static void LoadDeviceTypes()
        {
            if (_deviceTypes != null)
            {
                return;
            }

            var devs = from a in GetDriverAssemblies()
                       from t in a.GetTypes()
                       where t.GetTypeInfo().ImplementedInterfaces.Contains(typeof(IAscomDriver))
                       select t;

            _deviceTypes = devs.ToList();
        }

        /// <summary>
        /// Returns assemblies loaded from inside of DriverRoot
        /// </summary>
        /// <returns>A list of Assemblies found in the DriverRoot path</returns>
        private static IEnumerable<Assembly> GetDriverAssemblies()
        {
            List<Assembly> assList = new List<Assembly>();
            DirectoryInfo binPath = new DirectoryInfo(DriverRoot);
            foreach (var fileSystemInfo in binPath.GetFileSystemInfos("*.dll"))
            {
                tlog.LogMessage($"AscomDriverLoader.GetDriverAssemblies()", $"Loading assembly: '{fileSystemInfo.FullName}'");
                Console.WriteLine($"AscomDriverLoader.GetDriverAssemblies(): Loading assembly: '{fileSystemInfo.FullName}'");
                Console.Out.Flush();
                var assembly2 = Assembly.LoadFile(fileSystemInfo.FullName);
                //Assembly.Load(AssemblyName.GetAssemblyName(fileSystemInfo.FullName));
                //yield return assembly2;
                assList.Add(assembly2);
            }
            return assList;
        }
        public static bool DriverExists(String driverName) =>
            DriverTypes.Any(t => t.FullName == driverName);
        public static bool DriverExists(Type driverType) =>
            DriverExists(driverType.FullName);
        public static bool DriverExists<TDriver>() where TDriver: IAscomDriver =>
            DriverExists(typeof(TDriver));
        
        public static TDriver GetInstance<TDriver>() where TDriver : class, IAscomDriver
        {
            TDriver driver = null;
            Type driverType = typeof(TDriver);
            if (!DriverExists<TDriver>())
                throw new AscomException($"An assembly providing Driver '{driverType.Name}' not found under '{DriverRoot}'");

            if (driver==null)
                tlog.LogMessage($"AscomDriverLoader.GetInstance<{driverType.Name}>()", $"Failed to create an instance of '{driverType.Name}'");

            return driver;
        }
            
        public static IList<Type> DriverTypes
        {
            get
            {
                if (_deviceTypes == null)
                {
                    LoadDeviceTypes();
                }

                return _deviceTypes.ToList();
            }
        }

       
    }
}
