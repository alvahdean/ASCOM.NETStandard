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
        public AscomLoaderTest()
        {

        }

        [TestMethod]
        public void CleanDriverSection()
        {
            Console.WriteLine("CleanDriverSection");
            SystemHelper sh = new SystemHelper();
            AscomPlatformNode ascom = sh.Ascom;
            foreach (var node in sh.Nodes<ProfileNode>(ascom.ProfileNodeId))
            {
                if(node.Name!="Utilities")
                {
                    Console.WriteLine($"Removing AscomPlatform subnode: {node.Name}");
                    sh.DeleteNode(node.ProfileNodeId);
                }
                else
                    Console.WriteLine($"Skipping removal of AscomPlatform subnode: {node.Name}");
            }
        }

        [TestMethod]
        public void GetDriverRoot()
        {
            Console.WriteLine("GetDriverRootDefault");
            Console.WriteLine($"Loader DriverRoot: = '{DriverLoader.DriverRoot}'");
        }

        [TestMethod]
        public void DriverLoaderInit()
        {
            Console.WriteLine("DriverLoaderInit");
            CleanDriverSection();
            DriverLoader.Init();
        }

        [TestMethod]
        public void GetDriverAssemblies()
        {
            Console.WriteLine("GetDriverAssemblies");
            Console.WriteLine($"Loader DriverRoot: = '{DriverLoader.DriverRoot}'");
            Console.WriteLine($"[Loader Device GetDriverAssemblies]");
            foreach (var v in DriverLoader.Assemblies)
            {
                AssemblyName aName = v.GetName();
                Console.WriteLine($"[{aName.FullName}]: ({aName.CodeBase})");
            }
        }

        [TestMethod]
        public void GetDriverTypes()
        {
            Console.WriteLine("GetDriverTypes");
            Console.WriteLine($"[Loader Device Types]");
            foreach (var v in DriverLoader.DriverTypes)
            {
                Assert.IsNotNull(v, "Check DriverType not null");
                Console.WriteLine($"Driver: {v.FullName}'");

                Console.WriteLine($"IsInterface: {v.IsInterface}'");
                Assert.IsFalse(v.IsInterface, "Check driver type is not interface");

                Type ifType = DriverLoader.InterfaceFor(v);
                Console.WriteLine($"Interface: {ifType?.FullName}'");
                Assert.IsNotNull(ifType, "Check InterfaceType");

                Type apiType = DriverLoader.ApiTypeFor(v);
                Console.WriteLine($"API Type: {apiType?.FullName}'");
                Assert.IsNotNull(apiType, "Validate APIType");

                AssemblyName aName = v.Assembly?.GetName();
                Assert.IsNotNull(aName, "Validate DriverType Assembly");
                Console.WriteLine($"Assembly: {aName.FullName}'");
                Console.WriteLine($"Codebase: {aName.CodeBase}'");
                Console.WriteLine($"--------------------------");
                Console.WriteLine();

            }
        }

        [TestMethod]
        public void DriverImplementationInstanceTests()
        {
            Console.WriteLine("DriverImplementationInstanceTests");
            DriverLoaderInit();
            foreach(var dt in DriverLoader.DriverTypes)
            {
                Console.WriteLine($"Creating instance of '{dt.FullName}'");
                var iInst = DriverLoader.GetImplementation(dt);
                Assert.IsNotNull(iInst, "Validate IAscomDriver reference");
                var cInst = Convert.ChangeType(iInst, dt);
                Assert.IsNotNull(cInst, $"Validate converted reference ({cInst})");
            }
        }

        [TestMethod]
        public void DriverApiInstanceTests()
        {
            Console.WriteLine("DriverApiInstanceTests");
            DriverLoaderInit();
            foreach (var dt in DriverLoader.DriverTypes)
            {
                Type apiType = DriverLoader.ApiTypeFor(dt);
                Console.WriteLine($"Creating instance of '{apiType.FullName}'");
                var iInst = DriverLoader.GetApiDriver(dt);
                Assert.IsNotNull(iInst, "Validate IAscomDriver reference");
                var cInst = Convert.ChangeType(iInst, apiType);
                Assert.IsNotNull(cInst, $"Validate converted reference ({cInst})");
            }
        }
    }

}
