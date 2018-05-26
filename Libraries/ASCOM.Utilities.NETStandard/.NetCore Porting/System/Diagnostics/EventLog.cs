// Decompiled with JetBrains decompiler
// Type: ASCOM.Utilities.EventLogCode
// Assembly: ASCOM.Utilities, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 06F2ED6E-C559-496D-B7FF-DCA800F2230D
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.Utilities\6.0.0.0__565de7938946fba7\ASCOM.Utilities.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace ASCOM.Utilities
{
    internal class EventLog : IDisposable
    {
        private static TextWriter _errStream;
        private static Dictionary<String, TraceLogger> _logs;
        private static List<String> _sources;
        private static string _logFile;
        private string _logName;
        private string _logSource;
        private string _machineName;

        static EventLog()
        {
            _errStream = Console.Error;
            _sources = new List<string>();
            _logs = new Dictionary<string, TraceLogger>();
            _logFile = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "/ASCOM/Logs";
            if (!Directory.Exists(_logFile))
                Directory.CreateDirectory(_logFile);
        }

        public EventLog() : this("", ".", "") { }

        public EventLog(string logName)
            : this(logName, ".", "") { }

        public EventLog(string logName, string machineName, string logSource)
        {
            _logName = logName?.Trim() ?? "";
            _machineName = machineName?.Trim() ?? ".";
            _logSource = logSource?.Trim() ?? "";
            CreateEventSource(_logSource, _logName);
        }

        public String LogFile { get { return LogExists(_logName) ? _logs[_logName].LogFileName : ""; } }
        public long MaximumKilobytes { get; internal set; }
        public OverflowAction OverflowAction { get; internal set; }
        public int RetentionDays { get; internal set; }
        public bool IsValid
        {
            get { return SourceExists(_logSource) && LogExists(_logName); }
        }

        protected static string GetFQPath(String logName)
        {
            logName = logName.Trim();
            if (!String.IsNullOrWhiteSpace(logName))
                logName = "ASCOM";
            return $"{_logFile}/EventLog.{logName}";
        }
        public static bool SourceExists(string logSource)
        {
            return !String.IsNullOrWhiteSpace(logSource) && _sources.Contains(logSource);
        }

        public static bool LogExists(string logName)
        {
            return !String.IsNullOrWhiteSpace(logName) && _logs.ContainsKey(logName) && _logs[logName]!=null;
        }

        //
        // Summary:
        //     Establishes the specified source name as a valid event source for writing entries
        //     to a log on the local computer. This method can also create a new custom log
        //     on the local computer.
        //
        // Parameters:
        //   source:
        //     The source name by which the application is registered on the local computer.
        //
        //   logName:
        //     The name of the log the source's entries are written to. Possible values include
        //     Application, System, or a custom event log.
        //
        // Exceptions:
        //   T:System.ArgumentException:
        //     source is an empty string ("") or null.- or - logName is not a valid event log
        //     name. Event log names must consist of printable characters, and cannot include
        //     the characters '*', '?', or '\'.- or - logName is not valid for user log creation.
        //     The event log names AppEvent, SysEvent, and SecEvent are reserved for system
        //     use.- or - The log name matches an existing event source name.- or - The source
        //     name results in a registry key path longer than 254 characters.- or - The first
        //     8 characters of logName match the first 8 characters of an existing event log
        //     name.- or - The source cannot be registered because it already exists on the
        //     local computer.- or - The source name matches an existing event log name.
        //
        //   T:System.InvalidOperationException:
        //     The registry key for the event log could not be opened on the local computer.
        public static void CreateEventSource(string source, string logName="")
        {
            source = source.Trim();
            if (!String.IsNullOrWhiteSpace(source))
                source = "ASCOM";

            if (!SourceExists(source))
                _sources.Add(source);
            CreateEventLog(logName);
        }

        public static void CreateEventLog(string logName)
        {
            if (!LogExists(logName))
            {
                string logFile = GetFQPath(logName);
                _logs.Add(logName, new TraceLogger(logFile, logName));
            }
        }

        public void ModifyOverflowPolicy(OverflowAction overflowAction, int retentionDays)
        {
            OverflowAction = overflowAction;
            RetentionDays = retentionDays;
        }

        public void Close()
        {
            //TODO: implement Close???
            //throw new System.NotImplementedException();
        }

        public void WriteEntry(string message, EventLogEntryType type, int eventID)
        {
            if(IsValid)
            {
                _logs[_logName].LogMessageCrLf(_logSource, $"{type.ToString()}[{eventID}]: {message}");
            }
        }

        protected void HandleOverflow()
        {
            //TODO: Implement Overflow handling
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~EventLog() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}