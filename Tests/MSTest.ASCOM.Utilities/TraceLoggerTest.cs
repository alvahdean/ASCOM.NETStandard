using System;
using System.Diagnostics;
using ASCOM;
using ASCOM.Utilities;
using ASCOM.Internal;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MSTest.ASCOMTests.TraceLog
{
    [TestClass]
    public class TraceLoggerTest
    {
        public void RunAll()
        {
            TraceLogger_InstanceTests();
        }
        [TestMethod]
        public void TraceLogger_InstanceTests()
        {
            String logType = $"{GetType().Name}";
            Console.WriteLine($"Starting Test: {logType}");
            TraceLogger tl = new TraceLogger(logType);
            tl.Enabled = true;
            
            TraceLogger.Debug($"\tPath: {tl.FullPath}");
            TraceLogger.Debug($"\tEnabled: {tl.Enabled}");
            tl.LogMessage(nameof(TraceLogger_InstanceTests), $"\tPath: {tl.FullPath}");
            tl.LogMessageCrLf(nameof(TraceLogger_InstanceTests), $"\tEnabled: {tl.Enabled}");
        }
    }
}
