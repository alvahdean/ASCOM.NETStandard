using System;
using System.Diagnostics;
using ASCOM;
using ASCOM.Utilities;
using ASCOM.Internal;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace MSTest.ASCOMTests.Utilities
{
    [TestClass]
    public class UtilTest
    {
        private TextWriter debug = Console.Out;

        [TestMethod]
        public void Util_Instance()
        {
            debug.WriteLine("Starting Test: Util_Properties");
            Util u = new Util();
            Assert.IsNotNull(u, "Util is null");
            debug.WriteLine("\tTest Complete");
            debug.WriteLine();
        }

        [TestMethod]
        public void Util_Properties()
        {
            string sval = "";
            string vName = "";
            debug.WriteLine("Starting Test: Util_Properties");
            Util u = new Util();
            TraceLogger.EnableDebug = true;

            sval = u.Hostname;
            vName = nameof(u.Hostname);
            TraceLogger.Debug($"\t{vName}: {sval}");
            Assert.IsFalse(sval== "", $"{vName} check");

            sval = u.CurrentUser.Name;
            vName = nameof(u.CurrentUser);
            TraceLogger.Debug($"\t{vName}: {sval}");
            Assert.IsFalse(sval == "", $"{vName} check");

            sval = u.CurrentUserHome;
            vName = nameof(u.CurrentUserHome);
            TraceLogger.Debug($"\t{vName}: {sval}");
            Assert.IsFalse(sval == "", $"{vName} check");

            sval = u.JulianDate.ToString();
            vName = nameof(u.JulianDate);
            TraceLogger.Debug($"\t{vName}: {sval}");
            Assert.IsFalse(sval == "", $"{vName} check");

            sval = u.UTCDate.ToString();
            vName = nameof(u.UTCDate);
            TraceLogger.Debug($"\t{vName}: {sval}");
            Assert.IsFalse(sval == "", $"{vName} check");

            sval = u.TimeZoneName.ToString();
            vName = nameof(u.TimeZoneName);
            TraceLogger.Debug($"\t{vName}: {sval}");
            Assert.IsFalse(sval == "", $"{vName} check");

            sval = u.TimeZoneOffset.ToString();
            vName = nameof(u.TimeZoneOffset);
            TraceLogger.Debug($"\t{vName}: {sval}");
            Assert.IsFalse(sval == "", $"{vName} check");

            sval = u.PlatformVersion.ToString();
            vName = nameof(u.PlatformVersion);
            TraceLogger.Debug($"\t{vName}: {sval}");
            Assert.IsFalse(sval == "", $"{vName} check");

            sval = u.BuildNumber.ToString();
            vName = nameof(u.BuildNumber);
            TraceLogger.Debug($"\t{vName}: {sval}");
            Assert.IsFalse(sval == "", $"{vName} check");

            sval = u.MajorVersion.ToString();
            vName = nameof(u.MajorVersion);
            TraceLogger.Debug($"\t{vName}: {sval}");
            Assert.IsFalse(sval == "", $"{vName} check");

            sval = u.MinorVersion.ToString();
            vName = nameof(u.MinorVersion);
            TraceLogger.Debug($"\t{vName}: {sval}");
            Assert.IsFalse(sval == "", $"{vName} check");

            sval = u.ServicePack.ToString();
            vName = nameof(u.ServicePack);
            TraceLogger.Debug($"\t{vName}: {sval}");
            Assert.IsFalse(sval == "", $"{vName} check");

            sval = u.SerialTraceFile.ToString();
            vName = nameof(u.SerialTraceFile);
            TraceLogger.Debug($"\t{vName}: {sval}");
            Assert.IsFalse(sval == "", $"{vName} check");

            sval = u.SerialTrace.ToString();
            vName = nameof(u.SerialTrace);
            TraceLogger.Debug($"\t{vName}: {sval}");
            Assert.IsFalse(sval == "", $"{vName} check");

            TraceLogger.Debug("\tTest Complete");
            TraceLogger.Debug("");
        }
    }
}
