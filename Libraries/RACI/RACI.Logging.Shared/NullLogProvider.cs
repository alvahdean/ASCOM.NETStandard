using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace RACI.Logging
{
    public class NullLogProvider : ILoggerProvider
    {
        public NullLogProvider() { }
        public ILogger CreateLogger(string categoryName) => new NullLog(categoryName);
        private class NullLog : ILogger
        {

            #region Instance
            public NullLog() : this("") { }
            public NullLog(string categoryName)
            {
                Category = categoryName?.Trim() ?? "";
            }
            #endregion

            #region Properties
            public string Category { get; }
            #endregion

            #region ILogger implementation            
            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception,Func<TState, Exception, string> formatter)
            {
            }
            public IDisposable BeginScope<TState>(TState state) => null;
            public void Dispose() { }
            #endregion

            #region Internal classes
            public class LogScope : IDisposable
            {
                #region Internal 
                private static int nextId = 1;
                private StringBuilder msg { get; }
                #endregion

                #region Properties
                public int Id { get; }
                public bool Complete { get; private set; }
                public string Message { get; private set; }
                public string Finish()
                {
                    Complete = true;
                    Message = msg?.ToString() ?? "";
                    return Message;
                }
                #endregion

                #region Instance management
                public LogScope()
                {
                    Id = nextId++;
                    Complete = false;
                    Message = null;
                    msg = new StringBuilder();
                }
                #endregion

                #region Overrides
                public override string ToString() => Message;
                #endregion

                #region IDisposable Support
                private bool disposedValue = false;
                public void Dispose() => Dispose(true);
                protected virtual void Dispose(bool disposing)
                {
                    if (!disposedValue)
                    {
                        if (disposing)
                            Finish();
                        disposedValue = true;
                    }
                }
                #endregion
            }

            public bool IsEnabled(LogLevel logLevel) => false;
            #endregion
        }
        public void Dispose() { }
    }

    public static class NullLogProviderExt
    {
        public static ILoggingBuilder AddNull(this ILoggingBuilder builder, NullLogProvider provider) =>
            builder.AddProvider(provider);
        public static ILoggingBuilder AddNull(this ILoggingBuilder builder) =>
            builder.AddProvider(new NullLogProvider());
        public static ILoggerFactory AddNull(this ILoggerFactory factory, NullLogProvider provider)
        {
            factory.AddProvider(provider);
            return factory;
        }
        public static ILoggerFactory AddNull(this ILoggerFactory factory)
        {
            factory.AddProvider(new NullLogProvider());
            return factory;
        }
    }
}
