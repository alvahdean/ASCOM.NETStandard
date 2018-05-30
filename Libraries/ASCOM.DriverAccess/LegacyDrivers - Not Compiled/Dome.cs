// Decompiled with JetBrains decompiler
// Type: ASCOM.DriverAccess.Dome
// Assembly: ASCOM.DriverAccess, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 4B15C8A3-EB2C-468B-8912-4BBDBC985936
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.DriverAccess\6.0.0.0__565de7938946fba7\ASCOM.DriverAccess.dll

using ASCOM.DeviceInterface;
using ASCOM.Utilities;
using System;

namespace ASCOM.DriverAccess.Legacy
{
  public class Dome : AscomDriver, IDomeV2
  {
    private MemberFactory memberFactory;

    public double Altitude
    {
      get
      {
        return Convert.ToDouble(this.memberFactory.CallMember(1, "Altitude", new Type[0]));
      }
    }

    public bool AtHome
    {
      get
      {
        return Convert.ToBoolean(this.memberFactory.CallMember(1, "AtHome", new Type[0]));
      }
    }

    public bool AtPark
    {
      get
      {
        return Convert.ToBoolean(this.memberFactory.CallMember(1, "AtPark", new Type[0]));
      }
    }

    public double Azimuth
    {
      get
      {
        return Convert.ToDouble(this.memberFactory.CallMember(1, "Azimuth", new Type[0]));
      }
    }

    public bool CanFindHome
    {
      get
      {
        return Convert.ToBoolean(this.memberFactory.CallMember(1, "CanFindHome", new Type[0]));
      }
    }

    public bool CanPark
    {
      get
      {
        return Convert.ToBoolean(this.memberFactory.CallMember(1, "CanPark", new Type[0]));
      }
    }

    public bool CanSetAltitude
    {
      get
      {
        return Convert.ToBoolean(this.memberFactory.CallMember(1, "CanSetAltitude", new Type[0]));
      }
    }

    public bool CanSetAzimuth
    {
      get
      {
        return Convert.ToBoolean(this.memberFactory.CallMember(1, "CanSetAzimuth", new Type[0]));
      }
    }

    public bool CanSetPark
    {
      get
      {
        return Convert.ToBoolean(this.memberFactory.CallMember(1, "CanSetPark", new Type[0]));
      }
    }

    public bool CanSetShutter
    {
      get
      {
        return Convert.ToBoolean(this.memberFactory.CallMember(1, "CanSetShutter", new Type[0]));
      }
    }

    public bool CanSlave
    {
      get
      {
        return Convert.ToBoolean(this.memberFactory.CallMember(1, "CanSlave", new Type[0]));
      }
    }

    public bool CanSyncAzimuth
    {
      get
      {
        return Convert.ToBoolean(this.memberFactory.CallMember(1, "CanSyncAzimuth", new Type[0]));
      }
    }

    public ShutterState ShutterStatus
    {
      get
      {
        return (ShutterState) this.memberFactory.CallMember(1, "ShutterStatus", new Type[0]);
      }
    }

    public bool Slaved
    {
      get
      {
        return Convert.ToBoolean(this.memberFactory.CallMember(1, "Slaved", new Type[0]));
      }
      set
      {
        this.memberFactory.CallMember(2, "Slaved", new Type[0], (object) value);
      }
    }

    public bool Slewing
    {
      get
      {
        return Convert.ToBoolean(this.memberFactory.CallMember(1, "Slewing", new Type[0]));
      }
    }

    public Dome(string domeId)
      : base(domeId)
    {
      this.memberFactory = this.MemberFactory;
    }

    public static string Choose(string domeId)
    {
      using (Chooser chooser = new Chooser())
      {
        chooser.DeviceType = "Dome";
        return chooser.Choose(domeId);
      }
    }

    public void AbortSlew()
    {
      this.memberFactory.CallMember(3, "AbortSlew", new Type[0]);
    }

    public void CloseShutter()
    {
      this.memberFactory.CallMember(3, "CloseShutter", new Type[0]);
    }

    public void FindHome()
    {
      this.memberFactory.CallMember(3, "FindHome", new Type[0]);
    }

    public void OpenShutter()
    {
      this.memberFactory.CallMember(3, "OpenShutter", new Type[0]);
    }

    public void Park()
    {
      this.memberFactory.CallMember(3, "Park", new Type[0]);
    }

    public void SetPark()
    {
      this.memberFactory.CallMember(3, "SetPark", new Type[0]);
    }

    public void SlewToAltitude(double Altitude)
    {
      this.memberFactory.CallMember(3, "SlewToAltitude", new Type[1]
      {
        typeof (double)
      }, (object) Altitude);
    }

    public void SlewToAzimuth(double Azimuth)
    {
      this.memberFactory.CallMember(3, "SlewToAzimuth", new Type[1]
      {
        typeof (double)
      }, (object) Azimuth);
    }

    public void SyncToAzimuth(double Azimuth)
    {
      this.memberFactory.CallMember(3, "SyncToAzimuth", new Type[1]
      {
        typeof (double)
      }, (object) Azimuth);
    }
  }
}
