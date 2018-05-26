// Decompiled with JetBrains decompiler
// Type: ASCOM.DriverAccess.ObservingConditions
// Assembly: ASCOM.DriverAccess, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 4B15C8A3-EB2C-468B-8912-4BBDBC985936
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.DriverAccess\6.0.0.0__565de7938946fba7\ASCOM.DriverAccess.dll

using ASCOM.DeviceInterface;
using ASCOM.Utilities;
using System;

namespace ASCOM.DriverAccess
{
  public class ObservingConditions : AscomDriver, IObservingConditions
  {
    private readonly MemberFactory _memberFactory;

    public double AveragePeriod
    {
      get
      {
        return (double) this._memberFactory.CallMember(1, "AveragePeriod", new Type[0]);
      }
      set
      {
        this._memberFactory.CallMember(2, "AveragePeriod", new Type[0], (object) value);
      }
    }

    public double CloudCover
    {
      get
      {
        return (double) this._memberFactory.CallMember(1, "CloudCover", new Type[0]);
      }
    }

    public double DewPoint
    {
      get
      {
        return (double) this._memberFactory.CallMember(1, "DewPoint", new Type[0]);
      }
    }

    public double Humidity
    {
      get
      {
        return (double) this._memberFactory.CallMember(1, "Humidity", new Type[0]);
      }
    }

    public double Pressure
    {
      get
      {
        return (double) this._memberFactory.CallMember(1, "Pressure", new Type[0]);
      }
    }

    public double RainRate
    {
      get
      {
        return (double) this._memberFactory.CallMember(1, "RainRate", new Type[0]);
      }
    }

    public double SkyBrightness
    {
      get
      {
        return (double) this._memberFactory.CallMember(1, "SkyBrightness", new Type[0]);
      }
    }

    public double SkyQuality
    {
      get
      {
        return (double) this._memberFactory.CallMember(1, "SkyQuality", new Type[0]);
      }
    }

    public double StarFWHM
    {
      get
      {
        return (double) this._memberFactory.CallMember(1, "StarFWHM", new Type[0]);
      }
    }

    public double SkyTemperature
    {
      get
      {
        return (double) this._memberFactory.CallMember(1, "SkyTemperature", new Type[0]);
      }
    }

    public double Temperature
    {
      get
      {
        return (double) this._memberFactory.CallMember(1, "Temperature", new Type[0]);
      }
    }

    public double WindDirection
    {
      get
      {
        return (double) this._memberFactory.CallMember(1, "WindDirection", new Type[0]);
      }
    }

    public double WindGust
    {
      get
      {
        return (double) this._memberFactory.CallMember(1, "WindGust", new Type[0]);
      }
    }

    public double WindSpeed
    {
      get
      {
        return (double) this._memberFactory.CallMember(1, "WindSpeed", new Type[0]);
      }
    }

    public ObservingConditions(string observingConditionsId)
      : base(observingConditionsId)
    {
      this._memberFactory = this.MemberFactory;
    }

    public static string Choose(string observingConditionsId)
    {
      using (Chooser chooser = new Chooser())
      {
        chooser.DeviceType = "ObservingConditions";
        return chooser.Choose(observingConditionsId);
      }
    }

    public double TimeSinceLastUpdate(string PropertyName)
    {
      return (double) this._memberFactory.CallMember(3, "TimeSinceLastUpdate", new Type[1]
      {
        typeof (string)
      }, (object) PropertyName);
    }

    public string SensorDescription(string PropertyName)
    {
      return (string) this._memberFactory.CallMember(3, "SensorDescription", new Type[1]
      {
        typeof (string)
      }, (object) PropertyName);
    }

    public void Refresh()
    {
      this._memberFactory.CallMember(3, "Refresh", new Type[0]);
    }
  }
}
