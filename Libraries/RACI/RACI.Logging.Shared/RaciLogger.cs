using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace RACI.Logging
{
    public static class RaciLog
    { 
        static private LoggerFactory _factory;
        static private object _syncRoot;
        static private bool _enabled = false;

        #region Instance management
        static RaciLog()
        {
            _enabled = false;
            _syncRoot = new object();
            MinLevel = LogLevel.Debug;
            LogPublisher = new SubscriptionLogProvider();
            NullLogger = new LoggerFactory().AddNull().CreateLogger("Null");
            _factory = new LoggerFactory();
            _factory.AddConsole(EntryFilter, true);
            _factory.AddProvider(LogPublisher);
#if DEBUG
            _factory.AddDebug(LogLevel.Debug);
#endif
#if TRACE
            //_factory.AddTraceSource()
#endif
            Enabled = true;
            DefaultLogger = _factory.CreateLogger("RACI");
        }
        #endregion

        #region Static Properties
        static public SubscriptionLogProvider LogPublisher { get; }
        public static ILoggerFactory Factory => _factory;
        public static bool LevelFilter(LogLevel level)
        {
            return Enabled && MinLevel != LogLevel.None && level >= MinLevel;
        }
        public static bool EntryFilter(string category, LogLevel level)
        {
            bool catEnabled = true;
            return LevelFilter(level) && catEnabled;
        }
        public static LogLevel MinLevel { get; set; }
        public static bool Enabled
        {
            get => _enabled && _factory != null;
            set
            {
                if (_enabled != value)
                    _enabled = value;
            }
        }
        public static ILogger DefaultLogger { get; private set; }
        public static ILogger NullLogger { get; private set; }
        public static ILogger CreateLogger(string categoryName,bool userLog=false)
        {
            return Enabled 
                ? _factory.CreateLogger(categoryName) 
                : NullLogger;
        }
        #endregion
    }
}
