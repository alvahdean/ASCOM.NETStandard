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
using RACI.Data;

namespace MSTest.ASCOMTests.Profiles
{
    [TestClass]
    public class ProfileComparisonTests
    {
        TextWriter debug = Console.Out;

        [TestMethod]
        public void ProfileValueEquality()
        {
            string msg;
            debug.WriteLine("ProfileNodeCompare:");
            ProfileValue p1 = new ProfileValue("ptest ") { ParentProfileNodeId = 13 };
            ProfileValue p2 = new ProfileValue("PTest") { ParentProfileNodeId = p1.ParentProfileNodeId };
            msg = $"[TEST Keys Compare]: '[{p1?.ParentProfileNodeId}]{p1?.Key}' == '[{p2?.ParentProfileNodeId}]{p2?.Key}'";
            debug.WriteLine(msg);
            Assert.IsTrue(p1.Equals(p2), msg);
            p2.ParentProfileNodeId++;
            msg = $"[TEST Keys Compare]: '[{p1?.ParentProfileNodeId}]{p1?.Key}' == '[{p2?.ParentProfileNodeId}]{p2?.Key}'";
            debug.WriteLine(msg);
            Assert.IsTrue(!p1.Equals(p2), msg);
            p2.ParentProfileNodeId--;
            p2.Key = "not same ";
            msg = $"[TEST Keys Compare]: '[{p1?.ParentProfileNodeId}]{p1?.Key}' == '[{p2?.ParentProfileNodeId}]{p2?.Key}'";
            debug.WriteLine(msg);
            Assert.IsTrue(!p1.Equals(p2), msg);
        }

        [TestMethod]
        public void ProfileNodeEquality()
        {
            string msg;
            debug.WriteLine("ProfileNodeCompare:");
            ProfileNode p1 = new ProfileNode("ptest ") { ParentProfileNodeId = 13 };
            ProfileNode p2 = new ProfileNode("PTest") { ParentProfileNodeId = p1.ParentProfileNodeId };
            msg = $"[TEST Keys Compare]: '[{p1?.ParentProfileNodeId}]{p1?.Name}' == '[{p2?.ParentProfileNodeId}]{p2?.Name}'";
            debug.WriteLine(msg);
            Assert.IsTrue(p1.Equals(p2), msg);
            p2.ParentProfileNodeId++;
            msg = $"[TEST Keys Compare]: '[{p1?.ParentProfileNodeId}]{p1?.Name}' == '[{p2?.ParentProfileNodeId}]{p2?.Name}'";
            debug.WriteLine(msg);
            Assert.IsTrue(!p1.Equals(p2), msg);
            p2.ParentProfileNodeId--;
            p2.Name = "not same ";
            msg = $"[TEST Keys Compare]: '[{p1?.ParentProfileNodeId}]{p1?.Name}' == '[{p2?.ParentProfileNodeId}]{p2?.Name}'";
            debug.WriteLine(msg);
            Assert.IsTrue(!p1.Equals(p2), msg);
        }

        [TestMethod]
        public void ProfileValueCompare()
        {
            string msg;
            debug.WriteLine("ProfileValueCompare:");
            ProfileValue p1 = new ProfileValue("ptest ");
            ProfileValue p2 = new ProfileValue("PTest");
            msg = $"[TEST Keys Compare]: '{p1?.Key}' < '{p2?.Key}'";
            debug.WriteLine(msg);
            Assert.IsTrue(p1 < p2, msg);
        }

        [TestMethod]
        public void ProfileNodeCompare()
        {
            string msg;
            debug.WriteLine("ProfileNodeCompare:");
            ProfileNode p1 = new ProfileNode("ptest ") { ParentProfileNodeId = 13 };
            ProfileNode p2 = new ProfileNode("PTest") { ParentProfileNodeId = p1.ParentProfileNodeId };
            msg = $"[TEST Keys Compare]: '{p1?.Name}' == '{p2?.Name}'";
            debug.WriteLine(msg);
            Assert.IsTrue(p1.Equals(p2), msg);
            p2.ParentProfileNodeId++;
            msg = $"[TEST Keys Compare (Parent different)]: '{p1?.Name}' != '{p2?.Name}'";
            debug.WriteLine(msg);
            Assert.IsTrue(!p1.Equals(p2), msg);
            p2.ParentProfileNodeId--;
            p2.Name = "not same ";
            msg = $"[TEST Keys Compare (Parent different)]: '{p1?.Name}' != '{p2?.Name}'";
            debug.WriteLine(msg);
            Assert.IsTrue(!p1.Equals(p2), msg);

        }

    }
}
