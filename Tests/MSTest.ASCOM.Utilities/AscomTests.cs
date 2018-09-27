using System;
using System.Collections.Generic;
using System.Linq;
using RACI.Data;
using Microsoft.Win32;
using System.Net;
using ASCOM.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MSTest.ASCOMTests.Utilities
{
    [TestClass]
    public class AscomTest
    {
        [TestMethod]
        public void PlatformSettings()
        {
            Console.WriteLine($"TEST: {nameof(PlatformSettings)}");
            SystemHelper sys = new SystemHelper();
            AscomPlatformNode ascom = sys.Ascom;
            Assert.IsNotNull(ascom, "ASCOM node is null");
            IProfileNode platform = sys.SubNode(ascom, "Platform",true);
            Assert.IsNotNull(platform, "ASCOM Platform settings not found");
            IEnumerable<string> skeys = sys.SubKeys(platform.ProfileNodeId);
            Assert.IsTrue((skeys?.Count()??0)>0,"No ASCOM platform settings found");
            Assert.IsTrue(skeys?.Contains("Platform Installation Date") ??false, "Platform Installation Date setting not found");
            foreach (var key in skeys.OrderBy(t => t))
            {
                string value = sys.KeyValueByPath(platform, key)?.Value;
                Console.WriteLine($"\t{key}: {value}");
            }
        }

        [TestMethod]
        public void ConvertPlatformDate()
        {
            Console.WriteLine($"TEST: {nameof(ConvertPlatformDate)}");
            string skey = "Platform Installation Date";
            SystemHelper sys = new SystemHelper();
            AscomPlatformNode platform = sys.SubNode<AscomPlatformNode>(sys.Ascom,"Platform",true);            
            Assert.IsNotNull(platform, "ASCOM Platform settings not found");
            bool hasKey=sys.HasValue(platform.ProfileNodeId, skey);
            Assert.IsTrue(hasKey, $"ASCOM Platform setting not found: '{skey}'");
            String sval = sys.ValueOrDefault(platform.ProfileNodeId, skey,"");
            Assert.IsFalse(String.IsNullOrWhiteSpace(sval), $"Setting '{skey}' is empty");
            Console.WriteLine($"\tString value: {sval}");
            DateTime parsed = DateTime.Parse(sval);
            Console.WriteLine($"\tParsed value: {parsed}");
            DateTime instDate = sys.ValueOrDefault<DateTime>(platform.ProfileNodeId, skey,default(DateTime));
            Assert.AreEqual(parsed,instDate, "Converted value does not match parsed value");
            Console.WriteLine($"\tDateTime Value: {instDate}");
        }

        [TestMethod]
        public void ListDriverTypes()
        {
            Console.WriteLine($"TEST: {nameof(ListDriverTypes)}");
            //RaciRepository repo = new RaciRepository();
            SystemHelper sys = new SystemHelper();
            ProfileNode ascom = sys.Ascom;
            List<string> driverNames = sys.AscomDrivers?.Select(t => t.Name).ToList();
            Assert.IsTrue((driverNames?.Count ?? 0) > 0, "No drivers found");
            Assert.IsTrue(driverNames.Any(t => t == "Telescope Drivers"), "Telescope driver not found");
            foreach (var drv in driverNames.OrderBy(t => t))
                Console.WriteLine($"\t{drv}");
        }
        [TestMethod]
        public void ListDevices()
        {
            string[] drvTypes = new string[] { "Telescope","Camera" };
            Console.WriteLine($"TEST: {nameof(ListDevices)}");
            //RaciRepository repo = new RaciRepository();
            SystemHelper sys = new SystemHelper();
            foreach(string drv in sys.AscomDrivers.Select(t=>t.Name.Replace(" Drivers","")))
            {
                Console.WriteLine($"Driver: {drv}");
                ProfileNode driverNode = sys.AscomDriver(drv);
                Assert.IsNotNull(driverNode, $"Driver '{drv}' is not registered");
                List<AscomDeviceNode> devices = sys.AscomDevices(drv).ToList();
                Assert.IsNotNull(devices, $"Driver '{drv}' returned no devices");
                Assert.IsTrue(devices.Count>0, $"Driver '{drv}' returned no devices");
                foreach (string devName in devices.Select(t=>t.Name))
                    Console.WriteLine($"\t{devName}");
                Console.WriteLine();
            }
        }

    }
}
