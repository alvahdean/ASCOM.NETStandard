using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Reflection;

namespace AscomReg
{
    class Program
    {
        static void Main(string[] args)
        {
            bool unregister = false;
            string libPath = "";
            string driverName = "";
            foreach(var arg in args)
            {
                string larg = arg.ToLower();
                if (larg == "-u")
                    unregister = true;
                else if (larg.EndsWith("dll"))
                    libPath = arg;
                else
                    driverName = arg;
            }
            if(string.IsNullOrWhiteSpace(libPath))
            {
                Console.WriteLine("No library specified.");
                return;
            }
            //if(!File.Exists(libPath))
            //{
            //    Console.WriteLine($"Library file not found: {libPath}");
            //    return;
            //}
            Console.WriteLine($"Loading DLL '{libPath}'...");
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.AssemblyResolve += new ResolveEventHandler(LoadFromSameFolder);

            Assembly asm =Assembly.Load(libPath);
            if (asm==null)
            {
                Console.WriteLine($"DLL load failed.");
                return;
            }
            Console.WriteLine($"DLL loaded successfully...");

            Console.WriteLine($"Creating driver instance: '{driverName}'");
            object driver =asm.CreateInstance(driverName);
            if (driver == null)
            {
                Console.WriteLine($"Unable to create driver instance.");
                return;
            }
            Console.WriteLine($"Driver instantiated successfully...");

            Console.WriteLine($"Getting method handle");
            Type dType = driver.GetType();
            Type[] argSig = new Type[] { typeof(bool) };
            var regMeth = dType.GetRuntimeMethod("RegUnregASCOM", argSig);
            Console.WriteLine($"Doing registration (Register={!unregister})");
            regMeth.Invoke(driver, new object[] { !unregister });
            Console.WriteLine($"Driver registration completed...");
        }

        static Assembly LoadFromSameFolder(object sender, ResolveEventArgs args)
        {
            string folderPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string assemblyPath = Path.Combine(folderPath, new AssemblyName(args.Name).Name + ".dll");
            if (!File.Exists(assemblyPath)) return null;
            Assembly assembly = Assembly.LoadFrom(assemblyPath);
            return assembly;
        }
    }
}
