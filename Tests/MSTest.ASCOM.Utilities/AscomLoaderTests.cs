using System;
using System.Collections.Generic;
using System.Linq;
using RACI.Data;
using Microsoft.Win32;
using System.Net;
using ASCOM.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ASCOM.DriverAccess;
using System.Reflection;

namespace MSTest.ASCOMTests.DriverAccess
{
    [TestClass]
    public class AscomLoaderTest
    {
        [TestMethod]
        public void GetDriverRoot()
        {
            Console.WriteLine("GetDriverRootDefault");
            Console.WriteLine($"Loader DriverRoot: = '{AscomDriverLoader.DriverRoot}'");
            Console.WriteLine("Loader initialized");
        }

        [TestMethod]
        public void GetDriverAssemblies()
        {
            Console.WriteLine("GetDriverAssemblies");
            Console.WriteLine($"Loader DriverRoot: = '{AscomDriverLoader.DriverRoot}'");
            Console.WriteLine($"[Loader Driver Assemblies]");
            foreach (var v in AscomDriverLoader.GetDriverAssemblies())
            {
                AssemblyName aName = v.GetName();
                Console.WriteLine($"\t[{aName.FullName}] ({aName.CodeBase})");
            }
        }
        [TestMethod]
        public void GetDriverTypes()
        {
            Console.WriteLine("GetDriverTypes");
            Console.WriteLine($"Loader DriverRoot: = '{AscomDriverLoader.DriverRoot}'");
            Console.WriteLine($"[Loader Device Types]");
            foreach (var v in AscomDriverLoader.DriverTypes)
            {
                AssemblyName aName = v.Assembly.GetName();
                Console.WriteLine($"[{v.Name}].[{aName.FullName}]: ({aName.CodeBase})");
            }
        }

    }
}
