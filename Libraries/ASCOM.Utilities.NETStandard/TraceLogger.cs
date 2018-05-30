// Decompiled with JetBrains decompiler
// Type: ASCOM.Utilities.TraceLogger
// Assembly: ASCOM.Utilities, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 06F2ED6E-C559-496D-B7FF-DCA800F2230D
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.Utilities\6.0.0.0__565de7938946fba7\ASCOM.Utilities.dll

using ASCOM.Utilities.Exceptions;
using ASCOM.Utilities.Interfaces;
using RACI.Data;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace ASCOM.Utilities
{
    [ClassInterface(ClassInterfaceType.None)]
    //[ComVisible(true)]
    //[Guid("A088DB9B-E081-4339-996E-191EB9A80844")]
    public class TraceLogger : ITraceLogger, ITraceLoggerExtra, IDisposable
    {
        private object syncLog = new object();
        private const String _defaultExt = "txt";
        private string g_LogName;
        private string g_LogFileName;
        private string g_LogFileType;
        private string g_LogDir;
        private string g_LogExt;
        private SystemHelper sys;
        private UserSettings user;
        public static TextWriter DebugWriter { get; set; } = Console.Out;
        private StreamWriter g_LogFile;
        private bool g_LineStarted;
        private bool _useMutext;
        private Mutex mut;
        private bool GotMutex;
        private bool disposedValue;
        public bool IsOpen => g_LogFile?.BaseStream?.CanWrite ?? false;
        public bool UseMutext
        {
            get => _useMutext;
            set
            {
                if (value != UseMutext)
                {
                    if (!value)
                        ReleaseLock();
                    else if (mut == null)
                        mut = new Mutex();  //TODO: Check original source for Mutex parameters
                    _useMutext = value;
                }
            }
        }
        public string LogExt
        {
            get => !String.IsNullOrWhiteSpace(g_LogExt)?g_LogExt:"txt";
            set
            {
                String newExt = value;
                if (String.IsNullOrWhiteSpace(newExt))
                    newExt = "";
                newExt=newExt.Trim();
                newExt = newExt.TrimStart('.').TrimStart();
                g_LogExt = newExt;
            }
        }
        public string LogName
        {
            get => !String.IsNullOrWhiteSpace(g_LogName) ? g_LogName : LogType??"Default";
            private set
            {
                string newName = value?.Trim();
                if (newName != LogName)
                    g_LogName = newName;
            }
        }
        public string LogType
        {
            get => !String.IsNullOrWhiteSpace(g_LogFileType)?g_LogFileType:(g_LogFileType="Default");
            protected set
            {
                string newType = value;
                if (String.IsNullOrWhiteSpace(newType))
                    newType = "Default";
                newType = newType.Trim();
                if(!LogType.Equals(newType, StringComparison.InvariantCultureIgnoreCase))
                {
                    g_LogFileType = value;
                }
            }
        }
        public string LogFileName
        {
            get
            {
                String dtime = (CreatedTime != default(DateTime)) ? $"_{CreatedTime.ToString("HHmm.ssfff")}" : "";

                return LogFileIdx>1 
                    ? $"{LogName}{dtime}_{LogFileIdx}.{LogExt}"
                    : $"{LogName}{dtime}.{LogExt}";
            }
        }
        public string LogDir
        {
            get
            {
                return !String.IsNullOrWhiteSpace(g_LogDir)
                    ? g_LogDir : $"{user.HomeDir}\\ASCOM\\Logs\\{DateTime.Now.ToString("yyyy-MM-dd")}";
            }
            private set
            {
                string newPath = value?.Trim() ?? "";
                if (newPath == "")
                    newPath = $"{user.HomeDir}\\ASCOM\\Logs\\{DateTime.Now.ToString("yyyy-MM-dd")}";
                if (!Path.IsPathRooted(newPath))
                    newPath = Path.GetFullPath(newPath);
                g_LogDir = newPath;
            }
        }
        public DateTime CreatedTime { get; private set; } = default(DateTime);
        public int LogFileIdx { get; private set; }
        public string FullPath
        {
            get => $"{LogDir}\\{LogFileName}";
        }

        public static bool EnableDebug { get; set; } = false;

        public bool Enabled { get; set; }

        public TraceLogger() : this("Default") { }
        public TraceLogger(string logFileType) : this(logFileType, logFileType) { }
        public TraceLogger(string logFileName, string logFileType)
        {
            System.Diagnostics.Debug.AutoFlush = true;
            UseMutext = false;
            mut = null;
            sys = new SystemHelper();
            user = sys.GetUser();
            disposedValue = false;
            Enabled = true;
            g_LogExt = _defaultExt;
            LogFileIdx = 1;
            SetLogFile(logFileName,LogType);
            LogDir = $"{user.HomeDir}\\ASCOM\\Logs\\{DateTime.Now.ToString("yyyy-MM-dd")}";
            CreateLogFile();
        }
        ~TraceLogger()
        {
            this.Dispose(false);
            // ISSUE: explicit finalizer call
            //base.Finalize();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue && disposing)
            {
                if (this.g_LogFile != null)
                {
                    try
                    {
                        this.g_LogFile.Flush();
                    }
                    catch (Exception ex)
                    {
                        //ProjectData.SetProjectError(ex);
                        //ProjectData.ClearProjectError();
                    }
                    try
                    {
                        this.g_LogFile.Close();
                    }
                    catch (Exception ex)
                    {
                        //ProjectData.SetProjectError(ex);
                        //ProjectData.ClearProjectError();
                    }
                    try
                    {
                        this.g_LogFile.Dispose();
                    }
                    catch (Exception ex)
                    {
                        //ProjectData.SetProjectError(ex);
                        //ProjectData.ClearProjectError();
                    }
                    this.g_LogFile = (StreamWriter)null;
                }
                if (this.mut != null)
                {
                    try
                    {
                        this.mut.Close();
                    }
                    catch (Exception ex)
                    {
                        //ProjectData.SetProjectError(ex);
                        //ProjectData.ClearProjectError();
                    }
                    this.mut = (Mutex)null;
                }
            }
            this.disposedValue = true;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize((object)this);
        }

        public void LogIssue(string Identifier, string Message)
        {
            try
            {
                this.GetTraceLoggerMutex("LogIssue", "\"" + Identifier + "\", \"" + Message + "\"");
                if (!this.Enabled)
                    return;
                if (this.g_LogFile == null)
                    this.CreateLogFile();
                if (this.g_LineStarted)
                    this.g_LogFile.WriteLine();
                this.LogMsgFormatter(Identifier, Message, true, false);
                if (!this.g_LineStarted)
                    return;
                this.LogMsgFormatter("Continuation", "", false, false);
            }
            finally
            {
                ReleaseLock();
            }
        }

        public static void Debug(string msg)
        {
            if (!EnableDebug)
                return;
            msg = msg?.TrimEnd() ?? "";
            if (DebugWriter != null)
            {
                DebugWriter.WriteLine($"TL_DEBUG: {msg}");
                DebugWriter.Flush();
            }
            System.Diagnostics.Debug.WriteLine($"TL_DEBUG: {msg}");
        }

        public void BlankLine()
        {
            this.LogMessage("", "", false);
        }

        public void LogMessage(string Identifier, string Message, bool HexDump)
        {
            string p_Msg = Message;
            try
            {
                this.GetTraceLoggerMutex("LogMessage", "\"" + Identifier + "\", \"" + Message + "\", " + HexDump.ToString() + "\"");
                if (this.g_LineStarted)
                    this.LogFinish(" ");
                if (!this.Enabled)
                    return;
                if (this.g_LogFile == null)
                    this.CreateLogFile();
                if (HexDump)
                    p_Msg = Message + "  (HEX" + this.MakeHex(Message) + ")";
                this.LogMsgFormatter(Identifier, p_Msg, true, false);
            }
            finally
            {
                ReleaseLock();
            }
        }

        public void LogMessageCrLf(string Identifier, string Message)
        {
            try
            {
                this.ObtainLock("LogMessage", "\"" + Identifier + "\", \"" + Message + "\"");
                if (this.g_LineStarted)
                    this.LogFinish(" ");
                if (!this.Enabled)
                    return;
                if (this.g_LogFile == null)
                    this.CreateLogFile();
                this.LogMsgFormatter(Identifier, Message, true, true);
            }
            finally
            {
                ReleaseLock();
            }
        }

        public void LogStart(string Identifier, string Message)
        {
            try
            {
                this.GetTraceLoggerMutex("LogStart", "\"" + Identifier + "\", \"" + Message + "\"");
                if (this.g_LineStarted)
                {
                    this.LogFinish("LOGISSUE: LogStart has been called before LogFinish. Parameters: " + Identifier + " " + Message);
                }
                else
                {
                    this.g_LineStarted = true;
                    if (!this.Enabled)
                        return;
                    if (this.g_LogFile == null)
                        this.CreateLogFile();
                    this.LogMsgFormatter(Identifier, Message, false, false);
                }
            }
            finally
            {
                ReleaseLock();
            }
        }

        public void LogContinue(string Message, bool HexDump)
        {
            string Message1 = Message;
            if (HexDump)
                Message1 = Message + "  (HEX" + this.MakeHex(Message) + ")";
            this.LogContinue(Message1);
        }

        public void LogFinish(string Message, bool HexDump)
        {
            string Message1 = Message;
            if (HexDump)
                Message1 = Message + "  (HEX" + this.MakeHex(Message) + ")";
            this.LogFinish(Message1);
        }

        public void SetLogFile(string logName, string logType)
        {
            this.LogType = !String.IsNullOrWhiteSpace(logType) ? logType.Trim() : "Default";
            this.g_LogFileName = !String.IsNullOrWhiteSpace(logName) ? logName.Trim() : LogType;
        }

        //[ComVisible(false)]
        public void LogMessage(string Identifier, string Message)
        {
            try
            {
                this.ObtainLock($"LogMessage\'{Identifier}\", \"{Message}\"",null);
                if (this.g_LineStarted)
                    this.LogFinish(" ");
                if (!this.Enabled)
                    return;
                if (this.g_LogFile == null)
                    this.CreateLogFile();
                this.LogMsgFormatter(Identifier, Message, true, false);
                Debug($"{Identifier}: {Message}");
            }
            finally
            {
                ReleaseLock();
            }
        }

        //[ComVisible(false)]
        public void LogContinue(string Message)
        {
            try
            {
                this.GetTraceLoggerMutex("LogContinue", "\"" + Message + "\"");
                if (!this.g_LineStarted)
                {
                    this.LogMessage("LOGISSUE", "LogContinue has been called before LogStart. Parameter: " + Message);
                }
                else
                {
                    if (!this.Enabled)
                        return;
                    if (this.g_LogFile == null)
                        this.CreateLogFile();
                    this.g_LogFile.Write(this.MakePrintable(Message, false));
                }
            }
            finally
            {
                ReleaseLock();
            }
        }

        //[ComVisible(false)]
        public void LogFinish(string Message)
        {
            try
            {
                this.GetTraceLoggerMutex("LogFinish", "\"" + Message + "\"");
                if (!this.g_LineStarted)
                {
                    this.LogMessage("LOGISSUE", "LogFinish has been called before LogStart. Parameter: " + Message);
                }
                else
                {
                    this.g_LineStarted = false;
                    if (!this.Enabled)
                        return;
                    if (this.g_LogFile == null)
                        this.CreateLogFile();
                    this.g_LogFile.WriteLine(this.MakePrintable(Message, false));
                }
            }
            finally
            {
                ReleaseLock();
            }
        }

        private void CreateLogFile()
        {
            if (IsOpen)
                return;
            if (CreatedTime == default(DateTime))
                CreatedTime = DateTime.Now;
            try
            {
                Directory.CreateDirectory(LogDir);
                while (File.Exists(FullPath)&&LogFileIdx<20)
                {
                    Debug($"Logfile already exists '{FullPath}', incrementing index");
                    checked { ++LogFileIdx; }
                }
                if(LogFileIdx>=20)
                    throw new Exception($"Unable to determine an acceptable log file, Index={LogFileIdx}");
                Debug($"Opening TraceLogger stream for '{FullPath}'");
                g_LogFile = new StreamWriter(FullPath, false);
                g_LogFile.AutoFlush = true;
            }
            catch (Exception ex)
            {
                throw new HelperException("TraceLogger:CreateLogFile - Unable to create log file", ex);
                //ProjectData.ClearProjectError();
            }
        }

        private string MakePrintable(string p_Msg, bool p_RespectCrLf)
        {
            string str = "";
            int num1 = 1;
            int num2 = Strings.Len(p_Msg);
            int Start = num1;
            while (Start <= num2)
            {
                int Number = Strings.Asc(Strings.Mid(p_Msg, Start, 1));
                int num3 = Number;
                switch (num3)
                {
                    case 10:
                    case 13:
                        str = !p_RespectCrLf ? str + "[" + Strings.Right("00" + Conversion.Hex(Number), 2) + "]" : str + Strings.Mid(p_Msg, Start, 1);
                        break;
                    case -9:
                    case 11:
                    case 12:
                    case -17:
                        str = str + "[" + Strings.Right("00" + Conversion.Hex(Number), 2) + "]";
                        break;
                    default:
                        if (num3 <= 126)
                        {
                            str += Strings.Mid(p_Msg, Start, 1);
                            break;
                        }
                        goto case -9;
                }
                if (!(Number < 32 | Number > 126))
                    ;
                checked { ++Start; }
            }
            return str;
        }

        private string MakeHex(string p_Msg)
        {
            string str = "";
            int num1 = 1;
            int num2 = Strings.Len(p_Msg);
            int Start = num1;
            while (Start <= num2)
            {
                int Number = Strings.Asc(Strings.Mid(p_Msg, Start, 1));
                str = str + "[" + Strings.Right("00" + Conversion.Hex(Number), 2) + "]";
                checked { ++Start; }
            }
            return str;
        }

        private void LogMsgFormatter(string p_Test, string p_Msg, bool p_NewLine, bool p_RespectCrLf)
        {
            try
            {
                p_Test = Strings.Left(p_Test + Strings.StrDup(30, " "), 25);
                string str = Strings.Format((object)DateTime.Now, "HH:mm:ss.fff") + " " + this.MakePrintable(p_Test, p_RespectCrLf) + " " + this.MakePrintable(p_Msg, p_RespectCrLf);
                if (this.g_LogFile == null)
                    return;
                if (p_NewLine)
                    this.g_LogFile.WriteLine(str);
                else
                    this.g_LogFile.Write(str);
                this.g_LogFile.Flush();
            }
            catch (Exception ex)
            {
                //ProjectData.SetProjectError(ex);
                EventLogCode.LogEvent("LogMsgFormatter", "Exception", EventLogEntryType.Error, GlobalConstants.EventLogErrors.TraceLoggerException, ex.ToString());
                //ProjectData.ClearProjectError();
            }
        }

        private bool ObtainLock(string Method, string Parameters)
        {
            if (GotMutex)
                return GotMutex;

            if (!UseMutext)
            {
                DateTime ti = DateTime.Now;
                lock (syncLog)
                {
                    GotMutex = true;
                }
            }
            else
            {
                try { GotMutex = this.mut.WaitOne(5000, false); }
                catch (AbandonedMutexException ex)
                {
                    //ProjectData.SetProjectError((Exception) ex);
                    AbandonedMutexException abandonedMutexException = ex;
                    Debug($"{EventLogEntryType.Error}[{GlobalConstants.EventLogErrors.TraceLoggerMutexAbandoned}]: AbandonedMutexException in {Method}, parameters: {Parameters}");
                    EventLogCode.LogEvent("TraceLogger", "AbandonedMutexException in " + Method + ", parameters: " + Parameters, EventLogEntryType.Error, GlobalConstants.EventLogErrors.TraceLoggerMutexAbandoned, abandonedMutexException.ToString());
                    if (RegistryCommonCode.GetBool("Trace Abandoned Mutexes", false))
                    {
                        EventLogCode.LogEvent("TraceLogger", "AbandonedMutexException in " + Method + ": Throwing exception to application", EventLogEntryType.Warning, GlobalConstants.EventLogErrors.TraceLoggerMutexAbandoned, (string)null);
                        throw;
                    }
                    else
                    {
                        EventLogCode.LogEvent("TraceLogger", "AbandonedMutexException in " + Method + ": Absorbing exception, continuing normal execution", EventLogEntryType.Warning, GlobalConstants.EventLogErrors.TraceLoggerMutexAbandoned, (string)null);
                        GotMutex = true;
                        //ProjectData.ClearProjectError();
                    }
                }
            }
            if (!this.GotMutex)
            {
                Debug($"{EventLogEntryType.Error}[{GlobalConstants.EventLogErrors.TraceLoggerMutexTimeOut}]: Timed out waiting for TraceLogger mutex in {Method}, parameters: {Parameters}");
                EventLogCode.LogEvent(Method, "Timed out waiting for TraceLogger mutex in " + Method + ", parameters: " + Parameters, EventLogEntryType.Error, GlobalConstants.EventLogErrors.TraceLoggerMutexTimeOut, (string)null);
                throw new ProfilePersistenceException("Timed out waiting for TraceLogger mutex in " + Method + ", parameters: " + Parameters);
            }
            return GotMutex;
        }
        private void ReleaseLock()
        {
            if (!GotMutex)
                return;
            if (UseMutext && mut != null)
            {
                //Debug("[TRACE]: Releasing TraceLogger mutext...");
                mut.ReleaseMutex();
            }
            else
            {
               // Debug("[TRACE]: Releasing TraceLogger syncLog lock...");
                lock (syncLog)
                {
                    //Debug("[TRACE]: Releasing TraceLogger syncLog lock...");
                    GotMutex = false;
                }
            }
        }

        //Backward compatibility for mutext calls
        [Obsolete("Use ObtainLock(...)")]
        private void GetTraceLoggerMutex(string Method, string Parameters) => ObtainLock(Method, Parameters);
    }
}