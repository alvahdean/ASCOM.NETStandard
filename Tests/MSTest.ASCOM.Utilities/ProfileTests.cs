using System;
using System.Diagnostics;
using ASCOM;
using ASCOM.Utilities;
using ASCOM.Internal;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;
using ASCOM.Utilities.Interfaces;

namespace MSTest.ASCOMTests.Profiles
{
    [TestClass]
    public class ProfileTests
    {
        private TextWriter debug = Console.Out;
        Profile _prf;
        public ProfileTests()
        {
            _prf = new Profile();
        }
        [TestMethod]
        public void RegisteredDeviceTypes()
        {
            debug.WriteLine("RegisteredDeviceTypes:");
            var drvList = _prf.RegisteredDeviceTypes.ToList<string>();
            Assert.IsTrue(drvList.Contains("Telescope"), "Telescope exists check");
            foreach (var drv in drvList)
            {
                debug.WriteLine($"\t{drv}");
            }
        }
        [TestMethod]
        public void RegisteredDevices()
        {
            debug.WriteLine("RegisteredDevices:");
            var drvList = _prf.RegisteredDeviceTypes.ToList<string>();
            Assert.IsTrue(drvList.Contains("Telescope"), "Telescope exists check");
            foreach (var drv in drvList)
            {
                debug.WriteLine($"[{drv} Devices]");
                
                var devList = _prf.RegisteredDevices(drv).ToList<IKeyValuePair>();
                if (drv == "Focuser")
                    Assert.IsTrue(devList.Any(t => t.Key == $"ASCOM.Simulator.{drv}"), "Simulator check");
                foreach(var dev in devList)
                    debug.WriteLine($"\t{dev.Key}: {dev.Value}");
                debug.WriteLine();
            }
        }

        [TestMethod]
        public void IsRegistered()
        {
            debug.WriteLine("IsRegistered:");

            _prf.DeviceType = "Focuser";
            Assert.IsFalse(_prf.IsRegistered("ASCOM.XXXX.Focuser"), "Device doesn't exists check");
            Assert.IsTrue(_prf.IsRegistered("ASCOM.Simulator.Focuser"), "Focuser exists check");
        }

        [TestMethod]
        public void RegisterDriver()
        {
            debug.WriteLine("RegisterDriver:");
            string drvType = "Focuser";
            string drvName = $"ASCOM.UnitTestReg.{drvType}";
            string drvDesc = $"UnitTestReg {drvType} Driver";
            _prf.DeviceType = drvType;
            if (_prf.IsRegistered(drvName))
                _prf.Unregister(drvName);

            Assert.IsFalse(_prf.IsRegistered(drvName), "Device is not registered check");
            _prf.Register(drvName,"");
            Assert.IsTrue(_prf.IsRegistered(drvName), "Device is registered check");
            
            Assert.IsTrue(String.IsNullOrWhiteSpace(_prf.GetValue(drvName, "")), "Device description is initially empty");
            _prf.Register(drvName, drvDesc);
            Assert.IsTrue(_prf.GetValue(drvName, "")==drvDesc, "Device description is initially empty");
            _prf.Unregister(drvName);
            Assert.IsFalse(_prf.IsRegistered(drvName), "Device is unregistered");
        }

        [TestMethod]
        public void AscomProfileTest()
        {
            string driverType = "Telescope";
            string devName = "ASCOM.Simulator.Telescope";
            debug.WriteLine("AscomProfileTest:");
            debug.WriteLine($"\tDriver Type: {driverType}");
            debug.WriteLine($"\tDevice Name: {devName}");
            Profile prf = new Profile() { DeviceType = driverType };
            IASCOMProfile aprf = prf.GetProfile(devName);
            debug.WriteLine($"\t[Values]");
            foreach (var kvp in aprf.ProfileValues)
            {
                string key = kvp.Key;
                foreach(var skvp in kvp.Value)
                {
                    string skey = skvp.Key;
                    string sval = skvp.Value;
                    debug.WriteLine($"\t[{key}]\\[{skey}]:{sval}");
                }
            }
        }
    }
}
