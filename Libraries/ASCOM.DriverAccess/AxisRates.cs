// Decompiled with JetBrains decompiler
// Type: ASCOM.DriverAccess.AxisRates
// Assembly: ASCOM.DriverAccess, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 4B15C8A3-EB2C-468B-8912-4BBDBC985936
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.DriverAccess\6.0.0.0__565de7938946fba7\ASCOM.DriverAccess.dll

using ASCOM.Utilities;
using System;
using System.Collections;
using ASCOM.DeviceInterface;

namespace ASCOM.DriverAccess
{
  public class AxisRates : IAxisRates, IEnumerator
  {
    private TraceLogger TL;
    private int CurrentPosition;
    private IAxisRates AxisRatesP5;

    public object Current
    {
      get
      {
        return (object) this[this.CurrentPosition];
      }
    }

    public int Count
    {
      get
      {
        if (this.TL != null)
          this.TL.LogMessage("AxisRates Class P5", "Count: " + (object) this.AxisRatesP5.Count);
        return this.AxisRatesP5.Count;
      }
    }

    public ASCOM.DeviceInterface.IRate this[int index]
    {
      get
      {
        if (this.TL != null)
          this.TL.LogMessage("AxisRates Class P5", "Get IRate - Index: " + (object) index);
        try
        {
          if (this.TL != null)
            this.TL.LogMessage("AxisRates Class P5", "Get IRate - Minimum: " + (object) this.AxisRatesP5[index].Minimum + ", Maximum: " + (object) this.AxisRatesP5[index].Maximum);
        }
        catch (Exception ex)
        {
          if (this.TL != null)
            this.TL.LogMessage("AxisRates Class P5", "Exception: " + ex.ToString());
        }
        return (ASCOM.DeviceInterface.IRate) new Rate(this.AxisRatesP5[index].Minimum, this.AxisRatesP5[index].Maximum);
      }
    }

    public AxisRates()
    {
      this.Reset();
      this.TL = (TraceLogger) null;
    }

    internal AxisRates(IAxisRates AxisRates, TraceLogger traceLogger)
    {
      this.TL = traceLogger;
      this.AxisRatesP5 = AxisRates;
      this.Reset();
      foreach (IRate axisRate in AxisRates)
      {
        if (this.TL != null)
          this.TL.LogMessage("AxisRates Class P5 New", "Adding rate: - Minimum: " + (object) axisRate.Minimum + ", Maximum: " + (object) axisRate.Maximum);
      }
    }

    public void Add(double Minimum, double Maximum)
    {
    }

    public bool MoveNext()
    {
      if (this.CurrentPosition <= this.Count)
        ++this.CurrentPosition;
      bool flag = this.CurrentPosition <= this.Count;
      if (this.TL != null)
        this.TL.LogMessage("AxisRates Class P5", "MoveNext - Position: " + (object) this.CurrentPosition + ", Return value: " + (object) flag);
      return flag;
    }

    public void Reset()
    {
      if (this.TL != null)
        this.TL.LogMessage("AxisRates Class P5", "Reset enumerator position to 0");
      this.CurrentPosition = 0;
    }

    public void Dispose()
    {
    }

    public IEnumerator GetEnumerator()
    {
      if (this.TL != null)
        this.TL.LogMessage("AxisRates Class P5", "Returning Enumerator - New object");
      return (IEnumerator) this;
    }
  }
}
