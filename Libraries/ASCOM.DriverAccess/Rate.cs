// Decompiled with JetBrains decompiler
// Type: ASCOM.DriverAccess.Rate
// Assembly: ASCOM.DriverAccess, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 4B15C8A3-EB2C-468B-8912-4BBDBC985936
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.DriverAccess\6.0.0.0__565de7938946fba7\ASCOM.DriverAccess.dll

using ASCOM.DeviceInterface;

namespace ASCOM.DriverAccess
{
  public class Rate : IRate
  {
    private double m_dMaximumR;
    private double m_dMinimumR;

    public double Maximum
    {
      get
      {
        return this.m_dMaximumR;
      }
      set
      {
        this.m_dMaximumR = value;
      }
    }

    public double Minimum
    {
      get
      {
        return this.m_dMinimumR;
      }
      set
      {
        this.m_dMinimumR = value;
      }
    }

    internal Rate(double Minimum, double Maximum)
    {
      this.m_dMaximumR = Maximum;
      this.m_dMinimumR = Minimum;
    }

    public void Dispose()
    {
    }
  }
}
