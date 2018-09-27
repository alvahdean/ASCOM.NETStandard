// Decompiled with JetBrains decompiler
// Type: ASCOM.DriverAccess.Telescope
// Assembly: ASCOM.DriverAccess, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 4B15C8A3-EB2C-468B-8912-4BBDBC985936
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.DriverAccess\6.0.0.0__565de7938946fba7\ASCOM.DriverAccess.dll

using ASCOM.DeviceInterface;
using ASCOM.Utilities;
using System;
using System.Linq;

namespace ASCOM.DriverAccess
{
    public class Telescope : AscomDriver<ITelescopeV3>, ITelescopeV3
    {
        private MemberFactory memberFactory;
        private readonly bool isPlatform6Telescope;
        private bool isPlatform5Telescope;

        public Telescope(string telescopeId) : base(telescopeId)
        {
            if (DriverLoader.AscomInterfaces(Impl.GetType()).Contains(typeof(ITelescopeV3)))
                isPlatform6Telescope = true;
            TL.LogMessage("Telescope", "Platform 5 Telescope: " + isPlatform5Telescope.ToString() + " Platform 6 Telescope: " + isPlatform6Telescope.ToString());
        }

        public ASCOM.DeviceInterface.AlignmentModes AlignmentMode { get => Impl.AlignmentMode; }

        public double Altitude { get => Impl.Altitude; }

        public double ApertureArea { get => Impl.ApertureArea; }

        public double ApertureDiameter { get => Impl.ApertureDiameter; }

        public bool AtHome { get => Impl.AtHome; }

        public bool AtPark { get => Impl.AtPark; }

        public double Azimuth { get => Impl.Azimuth; }

        public bool CanFindHome { get => Impl.CanFindHome; }

        public bool CanPark { get => Impl.CanPark; }

        public bool CanPulseGuide { get => Impl.CanPulseGuide; }

        public bool CanSetDeclinationRate { get => Impl.CanSetDeclinationRate; }

        public bool CanSetGuideRates { get => Impl.CanSetGuideRates; }

        public bool CanSetPark { get => Impl.CanSetPark; }

        public bool CanSetPierSide { get => Impl.CanSetPierSide; }

        public bool CanSetRightAscensionRate { get => Impl.CanSetRightAscensionRate; }

        public bool CanSetTracking { get => Impl.CanSetTracking; }

        public bool CanSlew { get => Impl.CanSlew; }

        public bool CanSlewAltAz { get => Impl.CanSlewAltAz; }

        public bool CanSlewAltAzAsync { get => Impl.CanSlewAltAzAsync; }

        public bool CanSlewAsync { get => Impl.CanSlewAsync; }

        public bool CanSync { get => Impl.CanSync; }

        public bool CanSyncAltAz { get => Impl.CanSyncAltAz; }

        public bool CanUnpark { get => Impl.CanUnpark; }

        public double Declination { get => Impl.Declination; }

        public double DeclinationRate
        {
            get => Impl.DeclinationRate;
            set
            {
                if (DeclinationRate != value)
                {
                    Impl.DeclinationRate = value;
                    profile.SetValue(nameof(DeclinationRate), Impl.DeclinationRate.ToString());
                    RaisePropertyChanged(nameof(DeclinationRate));
                }
            }
        }

        public bool DoesRefraction
        {
            get => Impl.DoesRefraction;
            set
            {
                if (DoesRefraction != value)
                {
                    Impl.DoesRefraction = value;
                    profile.SetValue(nameof(DoesRefraction), Impl.DoesRefraction.ToString());
                    RaisePropertyChanged(nameof(DoesRefraction));
                }
            }
        }

        public EquatorialCoordinateType EquatorialSystem { get => Impl.EquatorialSystem; }

        public double FocalLength { get => Impl.FocalLength; }

        public double GuideRateDeclination
        {
            get => Impl.GuideRateDeclination;
            set
            {
                if (GuideRateDeclination != value)
                {
                    Impl.GuideRateDeclination = value;
                    profile.SetValue(nameof(GuideRateDeclination), Impl.GuideRateDeclination.ToString());
                    RaisePropertyChanged(nameof(GuideRateDeclination));
                }
            }
        }

        public double GuideRateRightAscension
        {
            get => Impl.GuideRateRightAscension;
            set
            {
                if (GuideRateRightAscension != value)
                {
                    Impl.GuideRateRightAscension = value;
                    profile.SetValue(nameof(GuideRateRightAscension), Impl.GuideRateRightAscension.ToString());
                    RaisePropertyChanged(nameof(GuideRateRightAscension));
                }
            }
        }

        public bool IsPulseGuiding { get => Impl.IsPulseGuiding; }

        public double RightAscension { get => Impl.RightAscension; }

        public double RightAscensionRate
        {
            get => Impl.RightAscensionRate;
            set
            {
                if (RightAscensionRate != value)
                {
                    Impl.RightAscensionRate = value;
                    profile.SetValue(nameof(RightAscensionRate), Impl.RightAscensionRate.ToString());
                    RaisePropertyChanged(nameof(RightAscensionRate));
                }
            }
        }

        public PierSide SideOfPier
        {
            get => Impl.SideOfPier;
            set
            {
                if (SideOfPier != value)
                {
                    Impl.SideOfPier = value;
                    profile.SetValue(nameof(SideOfPier), Impl.SideOfPier.ToString());
                    RaisePropertyChanged(nameof(SideOfPier));
                }
            }
        }

        public double SiderealTime { get => Impl.SiderealTime; }

        public double SiteElevation
        {
            get => Impl.SiteElevation;
            set
            {
                if (SiteElevation != value)
                {
                    Impl.SiteElevation = value;
                    profile.SetValue(nameof(SiteElevation), Impl.SiteElevation.ToString());
                    RaisePropertyChanged(nameof(SiteElevation));
                }
            }
        }

        public double SiteLatitude
        {
            get => Impl.SiteLatitude;
            set
            {
                if (SiteLatitude != value)
                {
                    Impl.SiteLatitude = value;
                    profile.SetValue(nameof(SiteLatitude), Impl.SiteLatitude.ToString());
                    RaisePropertyChanged(nameof(SiteLatitude));
                }
            }
        }

        public double SiteLongitude
        {
            get => Impl.SiteLongitude;
            set
            {
                if (SiteLongitude != value)
                {
                    Impl.SiteLongitude = value;
                    profile.SetValue(nameof(SiteLongitude), Impl.SiteLongitude.ToString());
                    RaisePropertyChanged(nameof(SiteLongitude));
                }
            }
        }

        public short SlewSettleTime
        {
            get => Impl.SlewSettleTime;
            set
            {
                if (SlewSettleTime != value)
                {
                    Impl.SlewSettleTime = value;
                    profile.SetValue(nameof(SlewSettleTime), Impl.SlewSettleTime.ToString());
                    RaisePropertyChanged(nameof(SlewSettleTime));
                }
            }
        }

        public bool Slewing { get => Impl.Slewing; }

        public double TargetDeclination
        {
            get => Impl.TargetDeclination;
            set
            {
                if (TargetDeclination != value)
                {
                    Impl.TargetDeclination = value;
                    profile.SetValue(nameof(TargetDeclination), Impl.TargetDeclination.ToString());
                    RaisePropertyChanged(nameof(TargetDeclination));
                }
            }
        }

        public double TargetRightAscension
        {
            get => Impl.TargetRightAscension;
            set
            {
                if (TargetRightAscension != value)
                {
                    Impl.TargetRightAscension = value;
                    profile.SetValue(nameof(TargetRightAscension), Impl.TargetRightAscension.ToString());
                    RaisePropertyChanged(nameof(TargetRightAscension));
                }
            }
        }

        public bool Tracking
        {
            get => Impl.Tracking;
            set
            {
                if (Tracking != value)
                {
                    Impl.Tracking = value;
                    profile.SetValue(nameof(Tracking), Impl.Tracking.ToString());
                    RaisePropertyChanged(nameof(Tracking));
                }
            }
        }

        public DriveRates TrackingRate
        {
            get => Impl.TrackingRate;
            set
            {
                if (TrackingRate != value)
                {
                    Impl.TrackingRate = value;
                    profile.SetValue(nameof(TrackingRate), Impl.TrackingRate.ToString());
                    RaisePropertyChanged(nameof(TrackingRate));
                }
            }
        }

        public ASCOM.DeviceInterface.ITrackingRates TrackingRates
        {
            get => new ASCOM.DriverAccess.TrackingRates(Impl.GetType(), Impl, TL);
        }

        public DateTime UTCDate
        {
            get => Impl.UTCDate;
            set
            {
                if (UTCDate != value)
                {
                    Impl.UTCDate = value;
                    profile.SetValue(nameof(UTCDate), Impl.UTCDate.ToString());
                    RaisePropertyChanged(nameof(UTCDate));
                }
            }
        }

        public void AbortSlew()
        {
            TL.LogMessage($"Begin {nameof(Unpark)}()", $"");
            Impl.Unpark();
            TL.LogMessage($"End {nameof(Unpark)}", $"Result: ");
        }

        public ASCOM.DeviceInterface.IAxisRates AxisRates(ASCOM.DeviceInterface.TelescopeAxes Axis)
        {
            TL.LogMessage("AxisRates", "");
            TL.LogMessage("AxisRates", Axis.ToString());
            if (!memberFactory.IsComObject)
            {
                if (isPlatform6Telescope)
                {
                    TL.LogMessage("AxisRates", "Platform 6 .NET Telescope");
                    object obj = memberFactory.CallMember(3, "AxisRates", new Type[1]
                    {
            typeof (ASCOM.DeviceInterface.TelescopeAxes)
                    }, (object)Axis);
                    try
                    {
                        ASCOM.DeviceInterface.IAxisRates axisRates = (ASCOM.DeviceInterface.IAxisRates)obj;
                        TL.LogMessage("AxisRates", "Number of returned AxisRates: " + (object)axisRates.Count);
                        if (axisRates.Count > 0)
                        {
                            for (int index = 1; index <= axisRates.Count; ++index)
                                TL.LogMessage("AxisRates", "Found Minimim: " + (object)axisRates[index].Minimum + ", Maximum: " + (object)axisRates[index].Maximum);
                        }
                    }
                    catch (Exception ex)
                    {
                        TL.LogMessageCrLf("AxisRates", ex.ToString());
                    }
                    return (ASCOM.DeviceInterface.IAxisRates)obj;
                }
                if (isPlatform5Telescope)
                {
                    TL.LogMessage("AxisRates", "Platform 5 .NET Telescope");
                    object obj = memberFactory.CallMember(3, "AxisRates", new Type[1]
                    {
            typeof (ASCOM.DeviceInterface.TelescopeAxes)
                    }, (object)Axis);
                    ASCOM.DeviceInterface.IAxisRates axisRates = (IAxisRates)new AxisRates();
                    try
                    {
                        IAxisRates AxisRates = (IAxisRates)obj;
                        axisRates = (IAxisRates)new AxisRates(AxisRates, TL);
                        TL.LogMessage("AxisRates", "Number of returned AxisRates: " + (object)AxisRates.Count);
                        if (AxisRates.Count > 0)
                        {
                            for (int index = 1; index <= AxisRates.Count; ++index)
                                TL.LogMessage("AxisRates", "Found Minimim: " + (object)AxisRates[index].Minimum + ", Maximum: " + (object)AxisRates[index].Maximum);
                        }
                    }
                    catch (Exception ex)
                    {
                        TL.LogMessageCrLf("AxisRates", ex.ToString());
                    }
                    return axisRates;
                }
                TL.LogMessage("AxisRates", "Neither Platform 5 nor Platform 6 .NET Telescope");
                return (ASCOM.DeviceInterface.IAxisRates)new AxisRates();
            }
            TL.LogMessage("AxisRates", "Platform 5/6 COM Telescope");
            _AxisRates axisRates1 = new _AxisRates(Axis, memberFactory.GetObjType, memberFactory.GetLateBoundObject, TL);
            try
            {
                if (axisRates1.Count > 0)
                {
                    for (int index = 1; index <= axisRates1.Count; ++index)
                        TL.LogMessage("AxisRates", "Found Minimim: " + (object)axisRates1[index].Minimum + ", Maximum: " + (object)axisRates1[index].Maximum);
                }
            }
            catch (Exception ex)
            {
                TL.LogMessageCrLf("AxisRates", ex.ToString());
            }
            return (ASCOM.DeviceInterface.IAxisRates)axisRates1;
        }

        public bool CanMoveAxis(ASCOM.DeviceInterface.TelescopeAxes Axis)
        {
            TL.LogMessage($"Begin {nameof(CanMoveAxis)}({Axis})", $"");
            var result = Impl.CanMoveAxis(Axis);
            TL.LogMessage($"End {nameof(CanMoveAxis)}", $"Result: { result}");
            return result;
        }

        public PierSide DestinationSideOfPier(double RightAscension, double Declination)
        {
            TL.LogMessage($"Begin {nameof(DestinationSideOfPier)}({RightAscension}),{Declination}", $"");
            var result = Impl.DestinationSideOfPier(RightAscension,Declination);
            TL.LogMessage($"End {nameof(DestinationSideOfPier)}", $"Result: { result}");
            return result;
        }

        public void FindHome()
        {
            TL.LogMessage($"Begin {nameof(FindHome)}()", $"");
            Impl.FindHome();
            TL.LogMessage($"End {nameof(FindHome)}", $"Result: ");
        }

        public void MoveAxis(ASCOM.DeviceInterface.TelescopeAxes Axis, double Rate)
        {
            TL.LogMessage($"Begin {nameof(MoveAxis)}({Axis},{Rate})", $"");
            Impl.MoveAxis(Axis,Rate);
            TL.LogMessage($"End {nameof(MoveAxis)}", $"Result: ");
        }

        public void Park()
        {
            TL.LogMessage($"Begin {nameof(Unpark)}()", $"");
            Impl.Unpark();
            TL.LogMessage($"End {nameof(Unpark)}", $"Result: ");
        }

        public void PulseGuide(ASCOM.DeviceInterface.GuideDirections Direction, int Duration)
        {
            TL.LogMessage($"Begin {nameof(PulseGuide)}({Direction},{Duration})", $"");
            Impl.PulseGuide(Direction, Duration);
            TL.LogMessage($"End {nameof(PulseGuide)}", $"Result: ");
        }

        public void SetPark()
        {
            TL.LogMessage($"Begin {nameof(SetPark)}()", $"");
            Impl.SetPark();
            TL.LogMessage($"End {nameof(SetPark)}", $"Result: ");
        }

        public void SlewToAltAz(double Azimuth, double Altitude)
        {
            TL.LogMessage($"Begin {nameof(SlewToAltAz)}({Azimuth},{Altitude})", $"");
            Impl.SlewToAltAz(Azimuth, Altitude);
            TL.LogMessage($"End {nameof(SlewToAltAz)}", $"Result: ");
        }

        public void SlewToAltAzAsync(double Azimuth, double Altitude)
        {
            TL.LogMessage($"Begin {nameof(SlewToAltAzAsync)}({Azimuth},{Altitude})", $"");
            Impl.SlewToAltAzAsync(Azimuth, Altitude);
            TL.LogMessage($"End {nameof(SlewToAltAzAsync)}", $"Result: ");
        }

        public void SlewToCoordinates(double RightAscension, double Declination)
        {
            TL.LogMessage($"Begin {nameof(SlewToCoordinates)}({RightAscension}, {Declination})", $"");
            Impl.SlewToCoordinates(RightAscension, Declination);
            TL.LogMessage($"End {nameof(SlewToCoordinates)}", $"Result: ");
        }

        public void SlewToCoordinatesAsync(double RightAscension, double Declination)
        {
            TL.LogMessage($"Begin {nameof(SlewToCoordinatesAsync)}({RightAscension}, {Declination})", $"");
            Impl.SlewToCoordinatesAsync(RightAscension, Declination);
            TL.LogMessage($"End {nameof(SlewToCoordinatesAsync)}", $"Result: ");
        }

        public void SlewToTarget()
        {
            TL.LogMessage($"Begin {nameof(SlewToTarget)}()", $"");
            Impl.SlewToTarget();
            TL.LogMessage($"End {nameof(SlewToTarget)}", $"Result: ");
        }

        public void SlewToTargetAsync()
        {
            TL.LogMessage($"Begin {nameof(SlewToTargetAsync)}()", $"");
            Impl.SlewToTargetAsync();
            TL.LogMessage($"End {nameof(SlewToTargetAsync)}", $"Result: ");
        }

        public void SyncToAltAz(double Azimuth, double Altitude)
        {
            TL.LogMessage($"Begin {nameof(SyncToAltAz)}({Azimuth},{Altitude})", $"");
            Impl.SyncToAltAz(Azimuth, Altitude);
            TL.LogMessage($"End {nameof(SyncToAltAz)}", $"Result: ");
        }

        public void SyncToCoordinates(double RightAscension, double Declination)
        {
            TL.LogMessage($"Begin {nameof(SyncToCoordinates)}({RightAscension},{Declination})", $"");
            Impl.SyncToCoordinates(RightAscension,Declination);
            TL.LogMessage($"End {nameof(SyncToCoordinates)}", $"Result: ");
        }

        public void SyncToTarget()
        {
            TL.LogMessage($"Begin {nameof(SyncToTarget)}()", $"");
            Impl.SyncToTarget();
            TL.LogMessage($"End {nameof(SyncToTarget)}", $"Result: ");
        }

        public void Unpark()
        {
            TL.LogMessage($"Begin {nameof(Unpark)}()", $"");
            Impl.Unpark();
            TL.LogMessage($"End {nameof(Unpark)}", $"Result: ");
        }
    }
}
