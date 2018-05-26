using System;
using System.Collections.Generic;
using System.Linq;
using RACI.Data;
using Microsoft.Win32;
using System.Net;
using ASCOM.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MSTest.ASCOMTests.Data
{
    [TestClass]
    public class SystemHelperTest
    {
        [TestMethod]
        public void RepoNodePaths()
        {
            Console.WriteLine($"Creating repo object platform SubKeys");
            SystemHelper sys = new SystemHelper();
            RaciSystem root = sys.System;
            Console.WriteLine($"[{sys.System.Name}]");
            foreach (var path in sys.SubKeys(root.ProfileNodeId))
                Console.WriteLine($"\t{path}");
        }

        [TestMethod]
        public void AscomSubKeys()
        {
            SystemHelper sys = new SystemHelper();
            Console.WriteLine($"ASCOM platform SubKeys");
            ProfileNode ascom = sys.Ascom;
            Console.WriteLine($"ASCOM System Node: {sys.System}");
            Console.WriteLine($"ASCOM Root Node: {ascom.ProfileNodeId}");
            IOrderedEnumerable<string> vals = sys.SubKeys(ascom.ProfileNodeId, true);
            foreach (var path in vals)
            {
                Console.Write($"[{path}]\t");
                string val = sys.ValueByPath(sys.Ascom, path);
                Console.WriteLine($"'{val}'");
            }
        }

        [TestMethod]
        public void AscomEnumKeyValues()
        {
            SystemHelper sys = new SystemHelper();
            Console.WriteLine($"ASCOM platform SubKeys");
            ProfileNode ascom = sys.Ascom;
            IOrderedEnumerable<ProfileValue> items = sys
                .EnumKeyValues(ascom.ProfileNodeId, true)
                .OrderBy(t => t.ParentProfileNodeId)
                .ThenBy(t => t.Key);
            foreach (var item in items)
            {
                Console.WriteLine($"[{item.Key}]\t{item.Value}");
            }
        }

        [TestMethod]
        public void AscomSubValues()
        {
            SystemHelper sys = new SystemHelper();
            Console.WriteLine($"ASCOM platform SubKeys");
            ProfileNode ascom = sys.Ascom;
            var items = sys.SubValues(ascom.ProfileNodeId, true);
            foreach (var item in items)
            {
                Console.WriteLine($"[{item.Key}]\t{item.Value.Value}");
            }
        }

        [TestMethod]
        public void ConvertBoolValue()
        {
            string nodeName = "Chooser";
            string valKey = "ASCOM.Simulator.Dome Init";
            Console.WriteLine($"Getting Value for {nodeName}/{valKey}");
            SystemHelper sys = new SystemHelper();
            ProfileNode ascom = sys.Ascom;
            ProfileNode chooser = sys.SubNode(ascom.ProfileNodeId, nodeName, true);

            string valString = sys.ValueOrDefault(chooser.ProfileNodeId, valKey, null);
            Console.WriteLine($"String Value: {valString}");
            bool tval = bool.Parse(valString);
            Console.WriteLine($"Parsed String Value: {tval}");
            bool val = sys.ValueOrDefault(chooser.ProfileNodeId, valKey, false);
            Console.WriteLine($"Converted Value: {val}");

            Assert.AreEqual(val, tval, "Parsed value does not match converted result");

        }
        [TestMethod]
        public void ConvertDoubleValue()
        {
            string driverType = "Telescope";
            string deviceName = "ASCOM.Simulator.Telescope";
            string valKey = "Latitude";
            Console.WriteLine($"Getting Value for {deviceName}.{valKey}");
            SystemHelper sys = new SystemHelper();
            AscomDeviceNode node = sys.AscomDevice(driverType, deviceName);
            int parentId = node.ProfileNodeId;
            string valString = sys.ValueOrDefault(parentId, valKey, null);
            Console.WriteLine($"String Value: {valString}");
            double tval = double.Parse(valString);
            Console.WriteLine($"Parsed String Value: {tval}");
            double val = sys.ValueOrDefault(parentId, valKey, double.NaN);
            Console.WriteLine($"Converted Value: {val}");

            Assert.AreEqual(val, tval, "Parsed value does not match converted result");

        }
    }
}

