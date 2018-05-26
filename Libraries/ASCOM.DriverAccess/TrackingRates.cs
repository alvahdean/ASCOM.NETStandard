// Decompiled with JetBrains decompiler
// Type: ASCOM.DriverAccess.TrackingRates
// Assembly: ASCOM.DriverAccess, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 4B15C8A3-EB2C-468B-8912-4BBDBC985936
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.DriverAccess\6.0.0.0__565de7938946fba7\ASCOM.DriverAccess.dll

using ASCOM.DeviceInterface;
using ASCOM.Utilities;
using System;
using System.Collections;
using System.Reflection;

namespace ASCOM.DriverAccess
{
  public class TrackingRates : ITrackingRates, IEnumerable
  {
    private Type objTypeTrackingRates;
    private object objTrackingRatesLateBound;
    private TraceLogger TL;

    public DriveRates this[int index]
    {
      get
      {
        DriveRates driveRates = (DriveRates) this.objTypeTrackingRates.InvokeMember("Item", BindingFlags.GetProperty, (Binder) null, this.objTrackingRatesLateBound, new object[1]
        {
          (object) index
        });
        this.TL.LogMessage("TrackingRates Class", "DriveRates[" + (object) index + "] " + driveRates.ToString());
        return driveRates;
      }
    }

    public int Count
    {
      get
      {
        int num = (int) this.objTypeTrackingRates.InvokeMember("Count", BindingFlags.GetProperty, (Binder) null, this.objTrackingRatesLateBound, new object[0]);
        this.TL.LogMessage("TrackingRates Class", "Count: " + (object) num);
        return num;
      }
    }

    public TrackingRates(Type objTypeScope, object objScopeLateBound, TraceLogger TraceLog)
    {
      this.objTrackingRatesLateBound = objTypeScope.InvokeMember("TrackingRates", BindingFlags.GetProperty, (Binder) null, objScopeLateBound, new object[0]);
      if (this.objTrackingRatesLateBound == null)
        throw new NullReferenceException("Driver returned a null reference instead of an TrackingRates object");
      this.objTypeTrackingRates = this.objTrackingRatesLateBound.GetType();
      this.TL = TraceLog;
      this.TL.LogMessage("TrackingRates Class", "Created object: " + this.objTypeTrackingRates.FullName);
    }

    public IEnumerator GetEnumerator()
    {
      IEnumerator enumerator = (IEnumerator) this.objTypeTrackingRates.InvokeMember("GetEnumerator", BindingFlags.InvokeMethod, (Binder) null, this.objTrackingRatesLateBound, new object[0]);
      this.TL.LogMessage("TrackingRates Class", "Enumerator: " + enumerator.ToString());
      return enumerator;
    }

    public void Dispose()
    {
      if (this.objTrackingRatesLateBound == null)
        return;
      this.objTrackingRatesLateBound = (object) null;
    }
  }
}
