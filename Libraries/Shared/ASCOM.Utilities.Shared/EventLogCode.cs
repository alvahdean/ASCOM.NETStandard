// Decompiled with JetBrains decompiler
// Type: ASCOM.Utilities.EventLogCode
// Assembly: ASCOM.Utilities, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 06F2ED6E-C559-496D-B7FF-DCA800F2230D
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.Utilities\6.0.0.0__565de7938946fba7\ASCOM.Utilities.dll

//using Microsoft.VisualBasic;
//using Microsoft.VisualBasic.CompilerServices;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.IO;
using System.Threading;


#if NETCOREAPP2_0 || NETSTANDARD2_0
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
#endif



namespace ASCOM.Utilities
{
#warning Internal class exposed as public during porting: EventLogCode

    //[StandardModule]
    public sealed class EventLogCode 
    //internal sealed class EventLogCode
    {

    public static void LogEvent(string Caller, string Msg, EventLogEntryType Severity, GlobalConstants.EventLogErrors Id, string Except)
    {
      try
      {
                TraceLogger.Debug($"EventLog[{Severity}][{Id}]:[{Caller}]: {Msg}");
                if (!String.IsNullOrEmpty(Except))
                    TraceLogger.Debug($"Exception: {Except}");
        if (!EventLog.SourceExists("ASCOM Platform"))
        {
          EventLog.CreateEventSource("ASCOM Platform", "ASCOM");
          EventLog eventLog1 = new EventLog("ASCOM", ".", "ASCOM Platform");
          eventLog1.ModifyOverflowPolicy(OverflowAction.OverwriteAsNeeded, 0);
          eventLog1.MaximumKilobytes = 1024L;
          eventLog1.Close();
          eventLog1.Dispose();
          Thread.Sleep(3000);
          EventLog eventLog2 = new EventLog("ASCOM", ".", "ASCOM Platform");
          eventLog2.WriteEntry("Successfully created event log - Policy: " + eventLog2.OverflowAction.ToString() + ", Size: " + Conversions.ToString(eventLog2.MaximumKilobytes) + "kb", EventLogEntryType.Information, 0);
          eventLog2.Close();
          eventLog2.Dispose();
        }
        EventLog eventLog = new EventLog("ASCOM", ".", "ASCOM Platform");
        string message = Caller + " - " + Msg;
        if (Except != null)
          message = message + "\r\n" + Except;
        eventLog.WriteEntry(message, Severity, (int) Id);
        eventLog.Close();
        eventLog.Dispose();
      }
      catch (Win32Exception ex1)
      {
        //ProjectData.SetProjectError((Exception) ex1);
        Win32Exception win32Exception = ex1;
        try
        {
          string str = Strings.Format((object)DateTime.Now, "dd MMMM yyyy HH:mm:ss.fff");
          string path1 = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\ASCOM\\Logs\\EventLog.Errors.txt";
          string path2 = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\ASCOM\\Logs\\EventLog.Messages.txt";
          File.AppendAllText(path1, str + " ErrorCode: 0x" + Conversion.Hex(win32Exception.ErrorCode) + " NativeErrorCode: 0x" + Conversion.Hex(win32Exception.NativeErrorCode) + " " + win32Exception.ToString() + "\r\n");
          File.AppendAllText(path2, str + " " + Caller + " " + Msg + " " + Severity.ToString() + " " + Id.ToString() + " " + Except + "\r\n");
        }
        catch (Exception ex2)
        {
          //ProjectData.SetProjectError(ex2);
          //ProjectData.ClearProjectError();
        }
        //ProjectData.ClearProjectError();
      }
      catch (Exception ex1)
      {
        //ProjectData.SetProjectError(ex1);
        Exception exception = ex1;
        try
        {
          string str = Strings.Format((object) DateTime.Now, "dd MMMM yyyy HH:mm:ss.fff");
          string path1 = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\ASCOM\\Logs\\EventLogErrors.txt";
          string path2 = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\ASCOM\\Logs\\EventLogMessages.txt";
          File.AppendAllText(path1, str + " " + exception.ToString() + "\r\n");
          File.AppendAllText(path2, str + " " + Caller + " " + Msg + " " + Severity.ToString() + " " + Id.ToString() + " " + Except + "\r\n");
        }
        catch (Exception ex2)
        {
          //ProjectData.SetProjectError(ex2);
          //ProjectData.ClearProjectError();
        }
        //ProjectData.ClearProjectError();
      }
    }


    }
}
