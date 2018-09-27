using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace RACI.Settings
{

    public class PathSettings
    {
        private string _log;
        private string _application;
        private string _appData;
        private string _userData;
        private string _driverRoot;
        private string _temp;

        public String UserHome
        {
            get
            {
                string path= RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "C:\\Users" : "/users";
                return $"{path}{Path.DirectorySeparatorChar}{Environment.UserName}";
            }
        }
        public String Log
        {
            get
            {
                String path = _log;
                if (String.IsNullOrWhiteSpace(path))
                    path = "Logs";
                if (!Path.IsPathRooted(path))
                    path = $"{Application}{Path.DirectorySeparatorChar}{path}";
                return path;
            }
            set
            {
                _log = value;
            }
        }
        public String Application
        {
            get
            {
                String path = _application;
                if (String.IsNullOrWhiteSpace(path))
                    path = "RACI";
                if (!Path.IsPathRooted(path))
                    path = $"{UserHome}{Path.DirectorySeparatorChar}{path}";
                return path;
            }
            set
            {
                _application = value;
            }

        }
        public String AppData
        {
            get
            {
                String path = _appData;
                if (String.IsNullOrWhiteSpace(path))
                    path = "Data";
                if (!Path.IsPathRooted(path))
                    path = $"{Application}{Path.DirectorySeparatorChar}{path}";
                return path;
            }
            set
            {
                _appData = value;
            }
        }
        public String UserData
        {
            get
            {
                String path = _userData;
                if (String.IsNullOrWhiteSpace(path))
                    path = "UserData";
                if (!Path.IsPathRooted(path))
                    path = $"{UserHome}{Path.DirectorySeparatorChar}AppData{Path.DirectorySeparatorChar}RACI{Path.DirectorySeparatorChar}{path}";
                return path;
            }
            set
            {                
                _userData = value;
            }
        }
        public String DriverRoot
        {
            get
            {
                String path = _driverRoot;
                if (String.IsNullOrWhiteSpace(path))
                    path = "Drivers";
                if (!Path.IsPathRooted(path))
                    path = $"{Application}{Path.DirectorySeparatorChar}{path}";
                return path;
            }
            set
            {
                _driverRoot = value;
            }
        }
        public String Temp
        {
            get
            {
                String path = _temp;
                if (String.IsNullOrWhiteSpace(path))
                    path = "Temp";
                if (!Path.IsPathRooted(path))
                    path = $"{UserHome}{Path.DirectorySeparatorChar}AppData{Path.DirectorySeparatorChar}RACI{Path.DirectorySeparatorChar}{path}";
                return path;
            }
            set
            {
                _temp = value;
            }
        }
    }

    public class RACISettings
    {
        public String Logfile { get; set; }
        public String ProductVersion { get; set; }
        public String RestApiVersion { get; set; }
        public bool EnableInternalLog { get; set; }
        public String UrlRoot { get; set; }
    }

    public class AppSettings
    {

        private String fullPath(string path)
        {
            char sep = Path.DirectorySeparatorChar;
            path = path.Trim().Replace('\\', sep).Replace('/', sep).Replace($"{sep}{sep}", sep.ToString());
            return Path.GetFullPath(path);
        }
        private String makeDir(string dirName, string parent = null)
        {
            if (String.IsNullOrWhiteSpace(parent))
                parent = ".";
            parent = parent.Trim();
            if (String.IsNullOrWhiteSpace(dirName))
                throw new ArgumentException($"Argument '{nameof(dirName)}' cannot be empty ('{dirName}')");

            return makeDir($"{parent}{Path.DirectorySeparatorChar}{dirName}");
        }
        private String makeDir(string path)
        {
            path = fullPath(path);
            Directory.CreateDirectory(path);
            return new DirectoryInfo(path).FullName;
        }

        public AppSettings() : this(null) { }
        public AppSettings(IConfiguration config)
        {
            Configuration = config?? FileConfig();
            Configuration.Bind(this);
        }

        public IConfiguration Configuration
        {
            get;
            protected set;
        }
        /// <summary>
        /// Loads a set of files into the current AppSettings instance
        /// </summary>
        /// <param name="asOptional">
        /// True if the files are optional (if not present on the filesystem)
        /// False if they are required to exist</param>
        /// <param name="loadFiles">Array of file paths relative to 'baseDir' (default='.', currentDirectory)</param>
        /// <param name="baseDir">The directory to which the item paths are relative</param>
        /// <remarks>
        /// Absolute paths are honored.
        /// If no files are specified (default) then only ./appsettings.json file is loaded.
        /// </remarks>
        /// <returns>True on successful load of required items with no parsing errors for required or optional files</returns>
        protected IConfiguration FileConfig(bool asOptional = false, String[] loadFiles = null, String baseDir = null)
        {

            if ((loadFiles?.Count() ?? 0) == 0)
                loadFiles = new String[] { "appsettings.json" };
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory());

            foreach (string cfg in loadFiles)
            {
                if (!Path.HasExtension(cfg) && !asOptional)
                    throw new KeyNotFoundException($"Required config file does not have a known extension and cannot be loaded ('{cfg}')");
                if (cfg.EndsWith("json", StringComparison.InvariantCultureIgnoreCase))
                    builder.AddJsonFile(cfg, asOptional);
                if (cfg.EndsWith("xml", StringComparison.InvariantCultureIgnoreCase))
                    builder.AddXmlFile(cfg, asOptional);
                if (cfg.EndsWith("ini", StringComparison.InvariantCultureIgnoreCase))
                    builder.AddIniFile(cfg, asOptional);
            }
            return builder.Build();
        }
        //protected IConfiguration LoadEnvironmentConfig(String sectionKey, String prefix = null)
        //{
        //    IConfigurationBuilder bldr = new ConfigurationBuilder();
        //    if (!String.IsNullOrWhiteSpace(prefix))
        //        bldr.AddEnvironmentVariables(prefix);
        //    else
        //        bldr.AddEnvironmentVariables();

        //    return bldr.Build();
        //}
        protected IConfiguration LoadCmdLineConfig(string[] cmdLineArgs)
        {
            return new ConfigurationBuilder().AddCommandLine(cmdLineArgs).Build();
        }
        protected IConfiguration LoadAppContextConfig()
        {
            Dictionary<String, String> appCtx = new Dictionary<string, string>();
            appCtx.Add("AppContext:BaseDirectory", AppContext.BaseDirectory);
            appCtx.Add("AppContext:FrameworkDescription", RuntimeInformation.FrameworkDescription);
            appCtx.Add("AppContext:OSArchitecture", RuntimeInformation.OSArchitecture.ToString());
            IConfigurationBuilder cfg = new ConfigurationBuilder().AddInMemoryCollection(appCtx);
            return cfg.Build();
        }

        public String AppName { get; set; } = "RASCOM";
        public bool IsDevelopment { get; set; } = false;
        public String AscomDataStore { get; set; } = "EntityStore";
        public Dictionary<string, string> ConnectionStrings { get; set; }
        public PathSettings PathSettings { get; set; }
        public RACISettings RACI { get; set; }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb
                .AppendLine($"*** AppSettings ***")
                .AppendLine($"AppName: {AppName}")
                .AppendLine($"IsDevelopment: {IsDevelopment}")
                .AppendLine($"AscomDataStore: {AscomDataStore}")
                .AppendLine($"PathSettings:")
                .AppendLine($"\tUserHome: {PathSettings.UserHome}")
                .AppendLine($"\tApplication: {PathSettings.Application}")
                .AppendLine($"\tAppData: {PathSettings.AppData}")
                .AppendLine($"\tUserData: {PathSettings.UserData}")
                .AppendLine($"\tDriverRoot: {PathSettings.DriverRoot}")
                .AppendLine($"\tLog: {PathSettings.Log}")
                .AppendLine($"\tTemp: {PathSettings.Temp}")
                .AppendLine($"RACI:")
                .AppendLine($"\tLogfile: {RACI.Logfile}")
                .AppendLine($"Repository:")
                .AppendLine($"\tConnectionStrings: ");
            //foreach (var cs in Repository.ConnectionStrings.AsEnumerable())
            foreach(var cs in ConnectionStrings)
                sb.AppendLine($"\t\t{cs.Key}: {cs.Value}");
            return sb.ToString();
        }
    }

}
