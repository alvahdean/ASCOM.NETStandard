// Decompiled with JetBrains decompiler
// Type: ASCOM.DriverAccess.Telescope
// Assembly: ASCOM.DriverAccess, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 4B15C8A3-EB2C-468B-8912-4BBDBC985936
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.DriverAccess\6.0.0.0__565de7938946fba7\ASCOM.DriverAccess.dll

using ASCOM.DeviceInterface;
using ASCOM.Utilities;
using System;

namespace ASCOM.DriverAccess
{
  public class Telescope : AscomDriver, ITelescopeV3
  {
    private MemberFactory memberFactory;
    private bool isPlatform6Telescope;
    private bool isPlatform5Telescope;

    public ASCOM.DeviceInterface.AlignmentModes AlignmentMode
    {
      get
      {
        return (ASCOM.DeviceInterface.AlignmentModes) this.memberFactory.CallMember(1, "AlignmentMode", new Type[0]);
      }
    }

    public double Altitude
    {
      get
      {
        return Convert.ToDouble(this.memberFactory.CallMember(1, "Altitude", new Type[0]));
      }
    }

    public double ApertureArea
    {
      get
      {
        return Convert.ToDouble(this.memberFactory.CallMember(1, "ApertureArea", new Type[0]));
      }
    }

    public double ApertureDiameter
    {
      get
      {
        return Convert.ToDouble(this.memberFactory.CallMember(1, "ApertureDiameter", new Type[0]));
      }
    }

    public bool AtHome
    {
      get
      {
        return (bool) this.memberFactory.CallMember(1, "AtHome", new Type[0]);
      }
    }

    public bool AtPark
    {
      get
      {
        return (bool) this.memberFactory.CallMember(1, "AtPark", new Type[0]);
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
        return (bool) this.memberFactory.CallMember(1, "CanFindHome", new Type[0]);
      }
    }

    public bool CanPark
    {
      get
      {
        return (bool) this.memberFactory.CallMember(1, "CanPark", new Type[0]);
      }
    }

    public bool CanPulseGuide
    {
      get
      {
        return (bool) this.memberFactory.CallMember(1, "CanPulseGuide", new Type[0]);
      }
    }

    public bool CanSetDeclinationRate
    {
      get
      {
        return (bool) this.memberFactory.CallMember(1, "CanSetDeclinationRate", new Type[0]);
      }
    }

    public bool CanSetGuideRates
    {
      get
      {
        return (bool) this.memberFactory.CallMember(1, "CanSetGuideRates", new Type[0]);
      }
    }

    public bool CanSetPark
    {
      get
      {
        return (bool) this.memberFactory.CallMember(1, "CanSetPark", new Type[0]);
      }
    }

    public bool CanSetPierSide
    {
      get
      {
        return (bool) this.memberFactory.CallMember(1, "CanSetPierSide", new Type[0]);
      }
    }

    public bool CanSetRightAscensionRate
    {
      get
      {
        return (bool) this.memberFactory.CallMember(1, "CanSetRightAscensionRate", new Type[0]);
      }
    }

    public bool CanSetTracking
    {
      get
      {
        return (bool) this.memberFactory.CallMember(1, "CanSetTracking", new Type[0]);
      }
    }

    public bool CanSlew
    {
      get
      {
        return (bool) this.memberFactory.CallMember(1, "CanSlew", new Type[0]);
      }
    }

    public bool CanSlewAltAz
    {
      get
      {
        return (bool) this.memberFactory.CallMember(1, "CanSlewAltAz", new Type[0]);
      }
    }

    public bool CanSlewAltAzAsync
    {
      get
      {
        return (bool) this.memberFactory.CallMember(1, "CanSlewAltAzAsync", new Type[0]);
      }
    }

    public bool CanSlewAsync
    {
      get
      {
        return (bool) this.memberFactory.CallMember(1, "CanSlewAsync", new Type[0]);
      }
    }

    public bool CanSync
    {
      get
      {
        return (bool) this.memberFactory.CallMember(1, "CanSync", new Type[0]);
      }
    }

    public bool CanSyncAltAz
    {
      get
      {
        return (bool) this.memberFactory.CallMember(1, "CanSyncAltAz", new Type[0]);
      }
    }

    public bool CanUnpark
    {
      get
      {
        return (bool) this.memberFactory.CallMember(1, "CanUnpark", new Type[0]);
      }
    }

    public double Declination
    {
      get
      {
        return Convert.ToDouble(this.memberFactory.CallMember(1, "Declination", new Type[0]));
      }
    }

    public double DeclinationRate
    {
      get
      {
        return Convert.ToDouble(this.memberFactory.CallMember(1, "DeclinationRate", new Type[0]));
      }
      set
      {
        this.memberFactory.CallMember(2, "DeclinationRate", new Type[0], (object) value);
      }
    }

    public bool DoesRefraction
    {
      get
      {
        return (bool) this.memberFactory.CallMember(1, "DoesRefraction", new Type[0]);
      }
      set
      {
        this.memberFactory.CallMember(2, "DoesRefraction", new Type[0], (object) value);
      }
    }

    public ASCOM.DeviceInterface.EquatorialCoordinateType EquatorialSystem
    {
      get
      {
        return (ASCOM.DeviceInterface.EquatorialCoordinateType) this.memberFactory.CallMember(1, "EquatorialSystem", new Type[0]);
      }
    }

    public double FocalLength
    {
      get
      {
        return Convert.ToDouble(this.memberFactory.CallMember(1, "FocalLength", new Type[0]));
      }
    }

    public double GuideRateDeclination
    {
      get
      {
        return Convert.ToDouble(this.memberFactory.CallMember(1, "GuideRateDeclination", new Type[0]));
      }
      set
      {
        this.memberFactory.CallMember(2, "GuideRateDeclination", new Type[0], (object) value);
      }
    }

    public double GuideRateRightAscension
    {
      get
      {
        return Convert.ToDouble(this.memberFactory.CallMember(1, "GuideRateRightAscension", new Type[0]));
      }
      set
      {
        this.memberFactory.CallMember(2, "GuideRateRightAscension", new Type[0], (object) value);
      }
    }

    public bool IsPulseGuiding
    {
      get
      {
        return (bool) this.memberFactory.CallMember(1, "IsPulseGuiding", new Type[0]);
      }
    }

    public double RightAscension
    {
      get
      {
        return Convert.ToDouble(this.memberFactory.CallMember(1, "RightAscension", new Type[0]));
      }
    }

    public double RightAscensionRate
    {
      get
      {
        return Convert.ToDouble(this.memberFactory.CallMember(1, "RightAscensionRate", new Type[0]));
      }
      set
      {
        this.memberFactory.CallMember(2, "RightAscensionRate", new Type[0], (object) value);
      }
    }

    public ASCOM.DeviceInterface.PierSide SideOfPier
    {
      get
      {
        return (ASCOM.DeviceInterface.PierSide) this.memberFactory.CallMember(1, "SideOfPier", new Type[0]);
      }
      set
      {
        this.memberFactory.CallMember(2, "SideOfPier", new Type[0], (object) value);
      }
    }

    public double SiderealTime
    {
      get
      {
        return Convert.ToDouble(this.memberFactory.CallMember(1, "SiderealTime", new Type[0]));
      }
    }

    public double SiteElevation
    {
      get
      {
        return Convert.ToDouble(this.memberFactory.CallMember(1, "SiteElevation", new Type[0]));
      }
      set
      {
        this.memberFactory.CallMember(2, "SiteElevation", new Type[0], (object) value);
      }
    }

    public double SiteLatitude
    {
      get
      {
        return Convert.ToDouble(this.memberFactory.CallMember(1, "SiteLatitude", new Type[0]));
      }
      set
      {
        this.memberFactory.CallMember(2, "SiteLatitude", new Type[0], (object) value);
      }
    }

    public double SiteLongitude
    {
      get
      {
        return Convert.ToDouble(this.memberFactory.CallMember(1, "SiteLongitude", new Type[0]));
      }
      set
      {
        this.memberFactory.CallMember(2, "SiteLongitude", new Type[0], (object) value);
      }
    }

    public short SlewSettleTime
    {
      get
      {
        return Convert.ToInt16(this.memberFactory.CallMember(1, "SlewSettleTime", new Type[0]));
      }
      set
      {
        this.memberFactory.CallMember(2, "SlewSettleTime", new Type[0], (object) value);
      }
    }

    public bool Slewing
    {
      get
      {
        return (bool) this.memberFactory.CallMember(1, "Slewing", new Type[0]);
      }
    }

    public double TargetDeclination
    {
      get
      {
        return Convert.ToDouble(this.memberFactory.CallMember(1, "TargetDeclination", new Type[0]));
      }
      set
      {
        this.memberFactory.CallMember(2, "TargetDeclination", new Type[0], (object) value);
      }
    }

    public double TargetRightAscension
    {
      get
      {
        return Convert.ToDouble(this.memberFactory.CallMember(1, "TargetRightAscension", new Type[0]));
      }
      set
      {
        this.memberFactory.CallMember(2, "TargetRightAscension", new Type[0], (object) value);
      }
    }

    public bool Tracking
    {
      get
      {
        return (bool) this.memberFactory.CallMember(1, "Tracking", new Type[0]);
      }
      set
      {
        this.memberFactory.CallMember(2, "Tracking", new Type[0], (object) value);
      }
    }

    public ASCOM.DeviceInterface.DriveRates TrackingRate
    {
      get
      {
        return (ASCOM.DeviceInterface.DriveRates) this.memberFactory.CallMember(1, "TrackingRate", new Type[0]);
      }
      set
      {
        this.memberFactory.CallMember(2, "TrackingRate", new Type[0], (object) value);
      }
    }

    public ASCOM.DeviceInterface.ITrackingRates TrackingRates
    {
      get
      {
        this.TL.LogMessage("TrackingRates", "");
        this.TL.LogMessage("TrackingRates", "Creating TrackingRates object");
        ASCOM.DriverAccess.TrackingRates trackingRates = new ASCOM.DriverAccess.TrackingRates(this.memberFactory.GetObjType, this.memberFactory.GetLateBoundObject, this.TL);
        this.TL.LogMessage("TrackingRates", "Returning TrackingRates object");
        return (ASCOM.DeviceInterface.ITrackingRates) trackingRates;
      }
    }

    public DateTime UTCDate
    {
      get
      {
        return (DateTime) this.memberFactory.CallMember(1, "UTCDate", new Type[0]);
      }
      set
      {
        this.memberFactory.CallMember(2, "UTCDate", new Type[0], (object) value);
      }
    }

    public Telescope(string telescopeId)
      : base(telescopeId)
    {
      this.memberFactory = this.MemberFactory;
      foreach (Type getInterface in this.memberFactory.GetInterfaces)
      {
        this.TL.LogMessage("Telescope", "Found interface name: " + getInterface.Name);
        if (getInterface.Equals(typeof (ITelescopeV3)))
          this.isPlatform6Telescope = true;
        //if (getInterface.Equals(typeof (ITelescope)))
        //  this.isPlatform5Telescope = true;
      }
      this.TL.LogMessage("Telescope", "Platform 5 Telescope: " + this.isPlatform5Telescope.ToString() + " Platform 6 Telescope: " + this.isPlatform6Telescope.ToString());
    }

    public static string Choose(string telescopeId)
    {
      using (Chooser chooser = new Chooser())
      {
        chooser.DeviceType = "Telescope";
        return chooser.Choose(telescopeId);
      }
    }

    public void AbortSlew()
    {
      this.TL.LogMessage("AbortSlew", "Calling method");
      this.memberFactory.CallMember(3, "AbortSlew", new Type[0]);
      this.TL.LogMessage("AbortSlew", "Finished");
    }

    public ASCOM.DeviceInterface.IAxisRates AxisRates(ASCOM.DeviceInterface.TelescopeAxes Axis)
    {
      this.TL.LogMessage("AxisRates", "");
      this.TL.LogMessage("AxisRates", Axis.ToString());
      if (!this.memberFactory.IsComObject)
      {
        if (this.isPlatform6Telescope)
        {
          this.TL.LogMessage("AxisRates", "Platform 6 .NET Telescope");
          object obj = this.memberFactory.CallMember(3, "AxisRates", new Type[1]
          {
            typeof (ASCOM.DeviceInterface.TelescopeAxes)
          }, (object) Axis);
          try
          {
            ASCOM.DeviceInterface.IAxisRates axisRates = (ASCOM.DeviceInterface.IAxisRates) obj;
            this.TL.LogMessage("AxisRates", "Number of returned AxisRates: " + (object) axisRates.Count);
            if (axisRates.Count > 0)
            {
              for (int index = 1; index <= axisRates.Count; ++index)
                this.TL.LogMessage("AxisRates", "Found Minimim: " + (object) axisRates[index].Minimum + ", Maximum: " + (object) axisRates[index].Maximum);
            }
          }
          catch (Exception ex)
          {
            this.TL.LogMessageCrLf("AxisRates", ex.ToString());
          }
          return (ASCOM.DeviceInterface.IAxisRates) obj;
        }
        if (this.isPlatform5Telescope)
        {
          this.TL.LogMessage("AxisRates", "Platform 5 .NET Telescope");
          object obj = this.memberFactory.CallMember(3, "AxisRates", new Type[1]
          {
            typeof (ASCOM.DeviceInterface.TelescopeAxes)
          }, (object) Axis);
          ASCOM.DeviceInterface.IAxisRates axisRates = (IAxisRates) new AxisRates();
          try
          {
            IAxisRates AxisRates = (IAxisRates) obj;
            axisRates = (IAxisRates) new AxisRates(AxisRates, this.TL);
            this.TL.LogMessage("AxisRates", "Number of returned AxisRates: " + (object) AxisRates.Count);
            if (AxisRates.Count > 0)
            {
              for (int index = 1; index <= AxisRates.Count; ++index)
                this.TL.LogMessage("AxisRates", "Found Minimim: " + (object) AxisRates[index].Minimum + ", Maximum: " + (object) AxisRates[index].Maximum);
            }
          }
          catch (Exception ex)
          {
            this.TL.LogMessageCrLf("AxisRates", ex.ToString());
          }
          return axisRates;
        }
        this.TL.LogMessage("AxisRates", "Neither Platform 5 nor Platform 6 .NET Telescope");
        return (ASCOM.DeviceInterface.IAxisRates) new AxisRates();
      }
      this.TL.LogMessage("AxisRates", "Platform 5/6 COM Telescope");
      _AxisRates axisRates1 = new _AxisRates(Axis, this.memberFactory.GetObjType, this.memberFactory.GetLateBoundObject, this.TL);
      try
      {
        if (axisRates1.Count > 0)
        {
          for (int index = 1; index <= axisRates1.Count; ++index)
            this.TL.LogMessage("AxisRates", "Found Minimim: " + (object) axisRates1[index].Minimum + ", Maximum: " + (object) axisRates1[index].Maximum);
        }
      }
      catch (Exception ex)
      {
        this.TL.LogMessageCrLf("AxisRates", ex.ToString());
      }
      return (ASCOM.DeviceInterface.IAxisRates) axisRates1;
    }

    public bool CanMoveAxis(ASCOM.DeviceInterface.TelescopeAxes Axis)
    {
      return (bool) this.memberFactory.CallMember(3, "CanMoveAxis", new Type[1]
      {
        typeof (ASCOM.DeviceInterface.TelescopeAxes)
      }, (object) Axis);
    }

    public ASCOM.DeviceInterface.PierSide DestinationSideOfPier(double RightAscension, double Declination)
    {
      return (ASCOM.DeviceInterface.PierSide) this.memberFactory.CallMember(3, "DestinationSideOfPier", new Type[2]
      {
        typeof (double),
        typeof (double)
      }, (object) RightAscension, (object) Declination);
    }

    public void FindHome()
    {
      this.memberFactory.CallMember(3, "FindHome", new Type[0]);
    }

    public void MoveAxis(ASCOM.DeviceInterface.TelescopeAxes Axis, double Rate)
    {
      this.memberFactory.CallMember(3, "MoveAxis", new Type[2]
      {
        typeof (ASCOM.DeviceInterface.TelescopeAxes),
        typeof (double)
      }, (object) Axis, (object) Rate);
    }

    public void Park()
    {
      this.memberFactory.CallMember(3, "Park", new Type[0]);
    }

    public void PulseGuide(ASCOM.DeviceInterface.GuideDirections Direction, int Duration)
    {
      this.memberFactory.CallMember(3, "PulseGuide", new Type[2]
      {
        typeof (ASCOM.DeviceInterface.GuideDirections),
        typeof (int)
      }, (object) Direction, (object) Duration);
    }

    public void SetPark()
    {
      this.memberFactory.CallMember(3, "SetPark", new Type[0]);
    }

    public void SlewToAltAz(double Azimuth, double Altitude)
    {
      this.memberFactory.CallMember(3, "SlewToAltAz", new Type[2]
      {
        typeof (double),
        typeof (double)
      }, (object) Azimuth, (object) Altitude);
    }

    public void SlewToAltAzAsync(double Azimuth, double Altitude)
    {
      this.memberFactory.CallMember(3, "SlewToAltAzAsync", new Type[2]
      {
        typeof (double),
        typeof (double)
      }, (object) Azimuth, (object) Altitude);
    }

    public void SlewToCoordinates(double RightAscension, double Declination)
    {
      this.memberFactory.CallMember(3, "SlewToCoordinates", new Type[2]
      {
        typeof (double),
        typeof (double)
      }, (object) RightAscension, (object) Declination);
    }

    public void SlewToCoordinatesAsync(double RightAscension, double Declination)
    {
      this.memberFactory.CallMember(3, "SlewToCoordinatesAsync", new Type[2]
      {
        typeof (double),
        typeof (double)
      }, (object) RightAscension, (object) Declination);
    }

    public void SlewToTarget()
    {
      this.memberFactory.CallMember(3, "SlewToTarget", new Type[0]);
    }

    public void SlewToTargetAsync()
    {
      this.memberFactory.CallMember(3, "SlewToTargetAsync", new Type[0]);
    }

    public void SyncToAltAz(double Azimuth, double Altitude)
    {
      this.memberFactory.CallMember(3, "SyncToAltAz", new Type[2]
      {
        typeof (double),
        typeof (double)
      }, (object) Azimuth, (object) Altitude);
    }

    public void SyncToCoordinates(double RightAscension, double Declination)
    {
      this.memberFactory.CallMember(3, "SyncToCoordinates", new Type[2]
      {
        typeof (double),
        typeof (double)
      }, (object) RightAscension, (object) Declination);
    }

    public void SyncToTarget()
    {
      this.memberFactory.CallMember(3, "SyncToTarget", new Type[0]);
    }

    public void Unpark()
    {
      this.memberFactory.CallMember(3, "Unpark", new Type[0]);
    }
  }
}
