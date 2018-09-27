using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;

namespace ASCOM.Utilities
{
    public class TraceLogProvider : ILoggerProvider
    {
        public ILogger CreateLogger() => new AscomLogger("Default");
        public ILogger CreateLogger(string categoryName) => new AscomLogger(categoryName);
        public ILogger CreateLogger(string fileName, string categoryName) => new AscomLogger(fileName,categoryName);

        public void Dispose() { }

        private class AscomLogger : TraceLogger, ILogger
        {
            public AscomLogger() : this("Default") { }
            public AscomLogger(string logFileType) : this(logFileType, logFileType) { }
            public AscomLogger(string logFileName, string logFileType) : base(logFileName, logFileType) { }
            public bool IsEnabled(LogLevel logLevel) => base.Enabled;

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception,
                Func<TState, Exception, string> formatter)
            {
                var message = formatter(state, exception);
                LogMessage(message);
            }

            public IDisposable BeginScope<TState>(TState state) => null;
        }
    }

}
