using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Win32;
using System.Net;
using Microsoft.Extensions.Configuration;
using RACI.Settings;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MSTest.ASCOMTests.AppConfig
{
    [TestClass]
    public class AppSettingsTest
    {
        [TestMethod]
        public void AppSettings_Basic()
        {
            Console.WriteLine($"Running {nameof(AppSettings_Basic)}...");
            AppSettings settings = new AppSettings();
            Assert.IsFalse(String.IsNullOrWhiteSpace(settings.AppName), "AppName not specified");
            Assert.IsFalse(String.IsNullOrWhiteSpace(settings.AscomDataStore), "ASCOM DataStore not specified");
            
            Assert.IsNotNull(settings, "Settings object is null");
            Assert.IsNotNull(settings.RACI, "RACI Section cannot be found");
            Assert.IsNotNull(settings.PathSettings, "Path Section cannot be found");
            Console.WriteLine("AppSettings object dump:");
            Console.WriteLine(settings.ToString());
        }
    }
}
