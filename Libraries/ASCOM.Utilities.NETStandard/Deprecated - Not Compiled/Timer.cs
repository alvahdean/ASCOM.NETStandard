// Decompiled with JetBrains decompiler
// Type: ASCOM.Utilities.Timer
// Assembly: ASCOM.Utilities, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 06F2ED6E-C559-496D-B7FF-DCA800F2230D
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.Utilities\6.0.0.0__565de7938946fba7\ASCOM.Utilities.dll

using ASCOM.Utilities.Interfaces;
using Microsoft.VisualBasic;
//using Microsoft.VisualBasic;
//using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Timers;

namespace ASCOM.Utilities
{
  [ClassInterface(ClassInterfaceType.None)]
  //[ComVisible(true)]
  //[Guid("64FEE414-176D-44d0-99DF-47621D9C377F")]
  //[ComSourceInterfaces(typeof (ITimerEvent))]
  [Obsolete("Please replace it with Systems.Timers.Timer, which is reliable in all console and non-windowed applications.", false)]
  public class Timer : ITimer, IDisposable
  {
    [AccessedThroughProperty("FormTimer")]
    private System.Windows.Forms.Timer _FormTimer;
    [AccessedThroughProperty("TimersTimer")]
    private System.Timers.Timer _TimersTimer;
    private bool IsForm;
    private bool TraceEnabled;
    private TraceLogger TL;
    private bool disposedValue;

    private System.Windows.Forms.Timer FormTimer
    {
      [DebuggerNonUserCode] get
      {
        return this._FormTimer;
      }
      [DebuggerNonUserCode, MethodImpl(MethodImplOptions.Synchronized)] set
      {
        EventHandler eventHandler = new EventHandler(this.OnTimedEvent);
        if (this._FormTimer != null)
          this._FormTimer.Tick -= eventHandler;
        this._FormTimer = value;
        if (this._FormTimer == null)
          return;
        this._FormTimer.Tick += eventHandler;
      }
    }

    private System.Timers.Timer TimersTimer
    {
      [DebuggerNonUserCode] get
      {
        return this._TimersTimer;
      }
      [DebuggerNonUserCode, MethodImpl(MethodImplOptions.Synchronized)] set
      {
        ElapsedEventHandler elapsedEventHandler = new ElapsedEventHandler(this.OnTimedEvent);
        if (this._TimersTimer != null)
          this._TimersTimer.Elapsed -= elapsedEventHandler;
        this._TimersTimer = value;
        if (this._TimersTimer == null)
          return;
        this._TimersTimer.Elapsed += elapsedEventHandler;
      }
    }

    public int Interval
    {
      get
      {
        if (this.IsForm)
        {
          this.TL.LogMessage("Interval FormTimer Get", this.FormTimer.Interval.ToString() + ", Thread: " + Thread.CurrentThread.ManagedThreadId.ToString());
          return this.FormTimer.Interval;
        }
        this.TL.LogMessage("Interval TimersTimer Get", this.TimersTimer.Interval.ToString() + ", Thread: " + Thread.CurrentThread.ManagedThreadId.ToString());
        return this.FormTimer.Interval;
      }
      set
      {
        if (this.IsForm)
        {
          this.TL.LogMessage("Interval FormTimer Set", value.ToString() + ", Thread: " + Thread.CurrentThread.ManagedThreadId.ToString());
          if (value > 0)
            this.FormTimer.Interval = value;
          else
            this.FormTimer.Enabled = false;
        }
        else
        {
          this.TL.LogMessage("Interval TimersTimer Set", value.ToString() + ", Thread: " + Thread.CurrentThread.ManagedThreadId.ToString());
          if (value > 0)
            this.TimersTimer.Interval = (double) value;
          else
            this.TimersTimer.Enabled = false;
        }
      }
    }

    public bool Enabled
    {
      get
      {
        if (this.IsForm)
        {
          this.TL.LogMessage("Enabled FormTimer Get", this.FormTimer.Enabled.ToString() + ", Thread: " + Thread.CurrentThread.ManagedThreadId.ToString());
          return this.FormTimer.Enabled;
        }
        this.TL.LogMessage("Enabled TimersTimer Get", this.TimersTimer.Enabled.ToString() + ", Thread: " + Thread.CurrentThread.ManagedThreadId.ToString());
        return this.TimersTimer.Enabled;
      }
      set
      {
        if (this.IsForm)
        {
          this.TL.LogMessage("Enabled FormTimer Set", value.ToString() + ", Thread: " + Thread.CurrentThread.ManagedThreadId.ToString());
          this.FormTimer.Enabled = value;
        }
        else
        {
          this.TL.LogMessage("Enabled TimersTimer Set", value.ToString() + ", Thread: " + Thread.CurrentThread.ManagedThreadId.ToString());
          this.TimersTimer.Enabled = value;
        }
      }
    }

    public event Timer.TickEventHandler Tick;

    public Timer()
    {
      this.Tick += new Timer.TickEventHandler(this.Timer_Tick);
      this.disposedValue = false;
      this.TL = new TraceLogger("", "Timer");
      this.TraceEnabled = RegistryCommonCode.GetBool("Trace Timer", false);
      this.TL.Enabled = this.TraceEnabled;
      this.TL.LogMessage("New", "Started on thread: " + Thread.CurrentThread.ManagedThreadId.ToString());
      this.FormTimer = new System.Windows.Forms.Timer();
      this.TL.LogMessage("New", "Created FormTimer");
      this.FormTimer.Enabled = false;
      this.FormTimer.Interval = 1000;
      this.TL.LogMessage("New", "Set FormTimer interval");
      this.TimersTimer = new System.Timers.Timer();
      this.TL.LogMessage("New", "Created TimersTimer");
      this.TimersTimer.Enabled = false;
      this.TimersTimer.Interval = 1000.0;
      this.TL.LogMessage("New", "Set TimersTimer interval");
      try
      {
        this.TL.LogMessage("New", "Process FileName \"" + Process.GetCurrentProcess().MainModule.FileName + "\"");
        PEReader peReader = new PEReader(Process.GetCurrentProcess().MainModule.FileName, this.TL);
        this.TL.LogMessage("New", "SubSystem " + peReader.SubSystem().ToString());
        switch (peReader.SubSystem())
        {
          case PEReader.SubSystemType.WINDOWS_GUI:
            this.IsForm = true;
            break;
          case PEReader.SubSystemType.WINDOWS_CUI:
            this.IsForm = false;
            break;
          default:
            this.IsForm = false;
            break;
        }
        this.IsForm = !this.ForceTimer(this.IsForm);
        this.TL.LogMessage("New", "IsForm: " + Conversions.ToString(this.IsForm));
      }
      catch (Exception ex)
      {
        //ProjectData.SetProjectError(ex);
        Exception exception = ex;
        this.TL.LogMessageCrLf("New Exception", exception.ToString());
        EventLogCode.LogEvent("Timer:New", "Exception", EventLogEntryType.Error, GlobalConstants.EventLogErrors.TimerSetupException, exception.ToString());
        //ProjectData.ClearProjectError();
      }
    }

    ~Timer()
    {
      this.Dispose(false);
      // ISSUE: explicit finalizer call
      //base.Finalize();
    }

    private bool ForceTimer(bool CurrentIsForm)
    {
      RegistryAccess registryAccess = new RegistryAccess();
      bool flag1 = !CurrentIsForm;
      this.TL.LogMessage("ForceTimer", "Current IsForm: " + CurrentIsForm.ToString() + ", this makes the default ForceTimer value: " + Conversions.ToString(flag1));
      string upper = Process.GetCurrentProcess().MainModule.FileName.ToUpper();
      this.TL.LogMessage("ForceTimer", "Main process file name: " + upper);
      bool flag2 = false;
      SortedList<string, string> sortedList = registryAccess.EnumProfile("ForceSystemTimer");
      IEnumerator<System.Collections.Generic.KeyValuePair<string, string>> enumerator=null;
      try
      {
        enumerator = sortedList.GetEnumerator();
        while (enumerator.MoveNext())
        {
          System.Collections.Generic.KeyValuePair<string, string> current = enumerator.Current;
          if (upper.Contains(Strings.Trim(current.Key.ToUpper())))
          {
            this.TL.LogMessage("ForceTimer", "  Found: \"" + current.Key + "\" = \"" + current.Value + "\"");
            flag2 = true;
            bool result;
            if (bool.TryParse(current.Value, out result))
            {
              flag1 = result;
              this.TL.LogMessage("ForceTimer", "    Parsed OK: " + flag1.ToString() + ", ForceTimer set to: " + Conversions.ToString(flag1));
            }
            else
              this.TL.LogMessage("ForceTimer", "    ***** Error - Value is not boolean!");
          }
          else
            this.TL.LogMessage("ForceTimer", "  Tried: \"" + current.Key + "\" = \"" + current.Value + "\"");
        }
      }
      finally
      {
        if (enumerator != null)
          enumerator.Dispose();
      }
      if (!flag2)
        this.TL.LogMessage("ForceTimer", "  Didn't match any force timer application names");
      this.TL.LogMessage("ForceTimer", "Returning: " + flag1.ToString());
      return flag1;
    }

    protected virtual void Dispose(bool disposing)
    {
      if (!this.disposedValue)
      {
        if (disposing)
        {
          this.TL.Enabled = false;
          this.TL.Dispose();
        }
        if (this.FormTimer != null)
        {
          if (this.FormTimer != null)
            this.FormTimer.Enabled = false;
          this.FormTimer.Dispose();
          this.FormTimer = (System.Windows.Forms.Timer) null;
        }
      }
      this.disposedValue = true;
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    private void OnTimedEvent(object sender, object e)
    {
      if (!this.IsForm)
      {
        ElapsedEventArgs elapsedEventArgs = (ElapsedEventArgs) e;
        if (this.TraceEnabled)
          this.TL.LogMessage("OnTimedEvent", "SignalTime: " + elapsedEventArgs.SignalTime.ToString());
      }
      if (this.TraceEnabled)
        this.TL.LogMessage("OnTimedEvent", "Raising Tick, Thread: " + Thread.CurrentThread.ManagedThreadId.ToString());
      Timer.TickEventHandler tickEvent = this.TickEvent;
      if (tickEvent != null)
        tickEvent();
      if (!this.TraceEnabled)
        return;
      this.TL.LogMessage("OnTimedEvent", "Raised Tick, Thread: " + Thread.CurrentThread.ManagedThreadId.ToString());
    }

        private void TickEvent()
        {
            throw new System.NotImplementedException();
        }

        private void Timer_Tick()
    {
    }

    //[ComVisible(false)]
    public delegate void TickEventHandler();
  }
}
