// Decompiled with JetBrains decompiler
// Type: ASCOM.DriverAccess._AxisRates
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
  internal class _AxisRates : IAxisRates
  {
    private Type objTypeAxisRates;
    private object objAxisRatesLateBound;
    private TraceLogger TL;
    private TelescopeAxes CurrentAxis;

    public IRate this[int index]
    {
      get
      {
        _Rate rate = new _Rate(index, this.objTypeAxisRates, this.objAxisRatesLateBound);
        this.TL.LogMessage("AxisRates Class", "Axis: " + this.CurrentAxis.ToString() + "- returned rate " + index.ToString() + " = Minimum: " + rate.Minimum.ToString() + ", Maximum: " + rate.Maximum.ToString());
        return (IRate) rate;
      }
    }

    public int Count
    {
      get
      {
        int num = (int) this.objTypeAxisRates.InvokeMember("Count", BindingFlags.GetProperty, (Binder) null, this.objAxisRatesLateBound, new object[0]);
        this.TL.LogMessage("AxisRates Class", "Count(" + this.CurrentAxis.ToString() + ") = " + (object) num);
        return num;
      }
    }

    public _AxisRates(TelescopeAxes Axis, Type objTypeScope, object objScopeLateBound, TraceLogger TraceLog)
    {
      this.objAxisRatesLateBound = objTypeScope.InvokeMember("AxisRates", BindingFlags.InvokeMethod, (Binder) null, objScopeLateBound, new object[1]
      {
        (object) Axis
      });
      if (this.objAxisRatesLateBound == null)
        throw new NullReferenceException("Driver returned a null reference instead of an AxisRates object for axis " + Axis.ToString());
      this.objTypeAxisRates = this.objAxisRatesLateBound.GetType();
      this.CurrentAxis = Axis;
      this.TL = TraceLog;
      this.TL.LogMessage("AxisRates Class", "Created object: " + this.objTypeAxisRates.FullName + " for axis: " + this.CurrentAxis.ToString());
    }

    public IEnumerator GetEnumerator()
    {
      this.TL.LogMessage("AxisRates Class", "GetEnumerator(" + this.CurrentAxis.ToString() + "): Returning rate enumerator");
      return (IEnumerator) new _RateEnumerator(this.objTypeAxisRates, this.objAxisRatesLateBound);
    }

    public void Dispose()
    {
      if (this.objAxisRatesLateBound == null)
        return;
      this.objAxisRatesLateBound = (object) null;
    }
  }
}
