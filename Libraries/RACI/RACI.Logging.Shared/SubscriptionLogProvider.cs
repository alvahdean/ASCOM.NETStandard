using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace RACI.Logging
{

    public interface ILogPublisher : ILogger, IObservable<SubscriberLogEntry> { }

    public class SubscriberLogEntry
    {
        private SubscriberLogEntry()
        {
            Message = "";
            Level = LogLevel.None;
            Event = 0;
            Error = null;
            Timestamp = DateTime.Now;
        }
        public SubscriberLogEntry(string msg, LogLevel level,EventId evId,Exception ex,DateTime? tstamp=null)
            :this()
        {
            Message = msg ?? "";
            Level = level;
            Event = evId;
            Error = ex;
            if(tstamp.HasValue)
                Timestamp = tstamp.Value;
        }

        public DateTime Timestamp { get; set; }
        public LogLevel Level { get; set; }
        public EventId Event { get; set; }
        public Exception Error { get; set; }
        public String Message { get; set; }
    }

    public class SubscriptionLogProvider : ILoggerProvider, IObservable<SubscriberLogEntry>
    {
        private List<IObserver<SubscriberLogEntry>> Subscribers { get; }
        private List<ILogger> Logs { get; }
        public SubscriptionLogProvider()
        {
            Logs = new List<ILogger>();
            Subscribers = new List<IObserver<SubscriberLogEntry>>();
        }
        public ILogger CreateLogger(string categoryName)
        {
            var logger = new SubscriberLog(this);
            Logs.Add(logger);
            return logger;
        }
        public void Dispose() { }
        public IDisposable Subscribe(IObserver<SubscriberLogEntry> subscriber)
        {
            SubscriberLog.Unsubscriber unsub = null;
            if (!Subscribers.Contains(subscriber))
            {
                Subscribers.Add(subscriber);
                unsub = new SubscriberLog.Unsubscriber(subscriber, Subscribers);
            }
            return unsub;
        } 
        private class SubscriberLog : ILogger
        {
            #region Instance
            private SubscriberLog()
            {
                Subscribers = new List<IObserver<SubscriberLogEntry>>();
#if TRACE
                MinLevel = LogLevel.Trace;
#elif DEBUG
                MinLevel = LogLevel.Debug;
#else
                MinLevel = LogLevel.Information;
#endif
            }
            public SubscriberLog(SubscriptionLogProvider prov)
                : this()
            {
                Subscribers = new List<IObserver<SubscriberLogEntry>>();
            }
            public SubscriberLog(SubscriptionLogProvider prov, LogLevel minLevel)
                : this(prov)
            {
                MinLevel = minLevel;
            }
            #endregion

            #region Private
            private bool HaveSubs => (Subscribers?.Count ?? 0) > 0;
            #endregion

            #region Properties
            public List<IObserver<SubscriberLogEntry>> Subscribers;
            public LogLevel MinLevel;
            #endregion

            #region ILogger implementation
            public bool IsEnabled(LogLevel logLevel) => logLevel>=MinLevel;
            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception,Func<TState, Exception, string> formatter)
            {
                if (MinLevel!=LogLevel.None && logLevel >= MinLevel && HaveSubs)
                {
                    var msg = formatter(state, exception);
                    var entry = new SubscriberLogEntry(msg, logLevel, eventId, exception);
                    foreach (var sub in Subscribers)
                        sub.OnNext(entry);
                }
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
            public class Unsubscriber : IDisposable
            {
                private IObserver<SubscriberLogEntry> Sub;
                private ICollection<IObserver<SubscriberLogEntry>> Repo;
                public Unsubscriber()
                {
                    Sub = null;
                    Repo = null;
                }
                public Unsubscriber(IObserver<SubscriberLogEntry> sub, ICollection<IObserver<SubscriberLogEntry>> repo)
                    :this()
                {
                    Sub = sub;
                    Repo = repo;
                }

                public void Dispose()
                {
                    if (Repo.Contains(Sub))
                        Repo.Remove(Sub);
                }
            }
            #endregion
        }
    }

    public static class SubscriptionLogProviderExt
    {
        public static ILoggingBuilder AddSubscription(this ILoggingBuilder builder, SubscriptionLogProvider provider)
        {
            return builder.AddProvider(provider);
        }
        public static ILoggingBuilder AddSubscription(this ILoggingBuilder builder, IObserver<SubscriberLogEntry> subscriber)
        {
            SubscriptionLogProvider prov = new SubscriptionLogProvider();
            prov.Subscribe(subscriber);
            return builder.AddProvider(prov);
        }
        public static ILoggerFactory AddSubscription(this ILoggerFactory factory, SubscriptionLogProvider provider)
        {
            factory.AddProvider(provider);
            return factory;
        }
        public static ILoggerFactory AddSubscription(this ILoggerFactory factory, IObserver<SubscriberLogEntry> subscriber)
        {
            SubscriptionLogProvider prov = new SubscriptionLogProvider();
            prov.Subscribe(subscriber);
            factory.AddProvider(prov);
            return factory;
        }

    }
}
