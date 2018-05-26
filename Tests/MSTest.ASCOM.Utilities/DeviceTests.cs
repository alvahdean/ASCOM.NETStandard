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
using ASCOM.DriverAccess;
using RACI.Data;
using ASCOM.Utilities.Exceptions;

namespace MSTest.ASCOMTests.Devices
{
    [TestClass]
    public class DeviceTests
    {
        private TextWriter debug = Console.Out;
        protected SystemHelper hlp = new SystemHelper();
        protected string DriverType { get; set; } = "Focuser";
        protected string DeviceName { get; set; } = "ASCOM.Simulator.Focuser";

        public DeviceTests()
        {
        }

        protected TDriver CreateDevice<TDriver>(string driverType, string devName)
        {

            //This is a brute force creation method but should be done via MemberFactory
            Type drvType = typeof(TDriver);
            devName = devName?.Trim() ?? "{Unspecified}";
            Profile prf = new Profile() { DeviceType = driverType };
            if (!prf.IsRegistered(devName))
                throw new DriverNotRegisteredException($"Device '{devName}' of type '{driverType}' not registered");
            TDriver inst = (TDriver)Activator.CreateInstance(drvType, devName);
            Assert.IsNotNull(inst,$"Error creating instance of '{driverType}.{devName}'");
            return inst;
        }

        [TestMethod]
        public void Connect()
        {
            debug.WriteLine($"Connect => Device[{DriverType}][{DeviceName}]");
            Focuser device = CreateDevice<Focuser>(DriverType, DeviceName);
            if(device.Connected)
            {
                debug.WriteLine($"Device[{DriverType}][{DeviceName}]: already connected, disconnecting");
                device.Connected = false;
                Assert.IsFalse(device.Connected, "Verify disconnect completed");
            }
            device.Connected = true;
            Assert.IsTrue(device.Connected, "Verify connect completed");
        }
        [TestMethod]
        public void Disconnect()
        {
            debug.WriteLine($"Disconnect => Device[{DriverType}][{DeviceName}]");
            Focuser device = CreateDevice<Focuser>(DriverType, DeviceName);
            if (!device.Connected)
            {
                debug.WriteLine($"Device[{DriverType}][{DeviceName}]: not connected, Connecting");
                device.Connected = true;
                Assert.IsTrue(device.Connected, "Verify connect completed");
            }
            device.Connected = false;
            Assert.IsFalse(device.Connected, "Verify connect completed");
        }

        [TestMethod]
        public void Description()
        {
            debug.WriteLine($"Description => Device[{DriverType}][{DeviceName}]");
            Focuser device = CreateDevice<Focuser>(DriverType, DeviceName);
            var info = device.Description;
            Assert.IsNotNull(info, "Verify data returned");
            debug.WriteLine($"\t{info}");
        }


        [TestMethod]
        public void DriverInfo()
        {
            debug.WriteLine($"DriverInfo => Device[{DriverType}][{DeviceName}]");
            Focuser device = CreateDevice<Focuser>(DriverType, DeviceName);
            var info = device.DriverInfo;
            Assert.IsNotNull(info, "Verify data returned");
            debug.WriteLine($"\t{info}");
        }


        [TestMethod]
        public void DriverVersion()
        {
            debug.WriteLine($"DriverVersion => Device[{DriverType}][{DeviceName}]");
            Focuser device = CreateDevice<Focuser>(DriverType, DeviceName);
            var info = device.DriverVersion;
            Assert.IsNotNull(info, "Verify data returned");
            debug.WriteLine($"\t{info}");
        }

        [TestMethod]
        public void InterfaceVersion()
        {
            debug.WriteLine($"InterfaceVersion => Device[{DriverType}][{DeviceName}]");
            Focuser device = CreateDevice<Focuser>(DriverType, DeviceName);
            var info = device.InterfaceVersion;
            Assert.IsNotNull(info, "Verify data returned");
            debug.WriteLine($"\t{info}");
        }

        [TestMethod]
        public void Name()
        {
            debug.WriteLine($"Name => Device[{DriverType}][{DeviceName}]");
            Focuser device = CreateDevice<Focuser>(DriverType, DeviceName);
            var info = device.Name;
            Assert.IsNotNull(info, "Verify data returned");
                debug.WriteLine($"\t{info}");
        }

        [TestMethod]
        public void SupportedActions()
        {
            debug.WriteLine($"SupportedActions => Device[{DriverType}][{DeviceName}]");
            Focuser device = CreateDevice<Focuser>(DriverType, DeviceName);
            var info=device.SupportedActions.ToStrings();
            Assert.IsNotNull(info, "Verify data returned");
            foreach(var item in info)
                debug.WriteLine($"\t{item}");
        }
    }
}
