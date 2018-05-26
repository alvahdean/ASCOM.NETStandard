// Decompiled with JetBrains decompiler
// Type: ASCOM.DriverAccess._Rate
// Assembly: ASCOM.DriverAccess, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 4B15C8A3-EB2C-468B-8912-4BBDBC985936
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.DriverAccess\6.0.0.0__565de7938946fba7\ASCOM.DriverAccess.dll

using ASCOM.DeviceInterface;
using System;
using System.Reflection;

namespace ASCOM.DriverAccess
{
  internal class _Rate : IRate
  {
    private Type objTypeRate;
    private object objRateLateBound;

    public double Maximum
    {
      get
      {
        return (double) this.objTypeRate.InvokeMember("Maximum", BindingFlags.GetProperty, (Binder) null, this.objRateLateBound, new object[0]);
      }
      set
      {
        this.objTypeRate.InvokeMember("Maximum", BindingFlags.SetProperty, (Binder) null, this.objRateLateBound, new object[1]
        {
          (object) value
        });
      }
    }

    public double Minimum
    {
      get
      {
        return (double) this.objTypeRate.InvokeMember("Minimum", BindingFlags.GetProperty, (Binder) null, this.objRateLateBound, new object[0]);
      }
      set
      {
        this.objTypeRate.InvokeMember("Minimum", BindingFlags.SetProperty, (Binder) null, this.objRateLateBound, new object[1]
        {
          (object) value
        });
      }
    }

    public _Rate(int index, Type objTypeAxisRates, object objAxisRatesLateBound)
    {
      this.objRateLateBound = objTypeAxisRates.InvokeMember("Item", BindingFlags.GetProperty, (Binder) null, objAxisRatesLateBound, new object[1]
      {
        (object) index
      });
      if (this.objRateLateBound == null)
        throw new NullReferenceException("Driver returned a null reference instead of a Rate object");
      this.objTypeRate = this.objRateLateBound.GetType();
    }

    public _Rate(object objRateLateBound)
    {
      this.objRateLateBound = objRateLateBound;
      this.objTypeRate = objRateLateBound.GetType();
    }

    public void Dispose()
    {
      if (this.objRateLateBound == null)
        return;
      this.objRateLateBound = (object) null;
    }
  }
}
