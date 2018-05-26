// Decompiled with JetBrains decompiler
// Type: ASCOM.DeviceInterface.ITelescopeV3
// Assembly: ASCOM.DeviceInterfaces, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 1FB1AAA7-A5CF-493B-90DA-CE61713D2AB5
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.DeviceInterfaces\6.0.0.0__565de7938946fba7\ASCOM.DeviceInterfaces.dll

using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace ASCOM.DeviceInterface
{
  //[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
  //[Guid("A007D146-AE3D-4754-98CA-199FEC03CF68")]
  //[ComVisible(true)]
  public interface ITelescopeV3 : IAscomDriver
    {

    AlignmentModes AlignmentMode { get; }

    double Altitude { get; }

    double ApertureArea { get; }

    double ApertureDiameter { get; }

    bool AtHome { get; }

    bool AtPark { get; }

    double Azimuth { get; }

    bool CanFindHome { get; }

    bool CanPark { get; }

    bool CanPulseGuide { get; }

    bool CanSetDeclinationRate { get; }

    bool CanSetGuideRates { get; }

    bool CanSetPark { get; }

    bool CanSetPierSide { get; }

    bool CanSetRightAscensionRate { get; }

    bool CanSetTracking { get; }

    bool CanSlew { get; }

    bool CanSlewAltAz { get; }

    bool CanSlewAltAzAsync { get; }

    bool CanSlewAsync { get; }

    bool CanSync { get; }

    bool CanSyncAltAz { get; }

    bool CanUnpark { get; }

    double Declination { get; }

    double DeclinationRate { get; set; }

    bool DoesRefraction { get; set; }

    EquatorialCoordinateType EquatorialSystem { get; }

    double FocalLength { get; }

    double GuideRateDeclination { get; set; }

    double GuideRateRightAscension { get; set; }

    bool IsPulseGuiding { get; }

    double RightAscension { get; }

    double RightAscensionRate { get; set; }

    PierSide SideOfPier { get; set; }

    double SiderealTime { get; }

    double SiteElevation { get; set; }

    double SiteLatitude { get; set; }

    double SiteLongitude { get; set; }

    bool Slewing { get; }

    short SlewSettleTime { get; set; }

    double TargetDeclination { get; set; }

    double TargetRightAscension { get; set; }

    bool Tracking { get; set; }

    DriveRates TrackingRate { get; set; }

    ITrackingRates TrackingRates { get; }

    DateTime UTCDate { get; set; }

    void AbortSlew();

    IAxisRates AxisRates(TelescopeAxes Axis);

    bool CanMoveAxis(TelescopeAxes Axis);

    PierSide DestinationSideOfPier(double RightAscension, double Declination);

    void FindHome();

    void MoveAxis(TelescopeAxes Axis, double Rate);

    void Park();

    void PulseGuide(GuideDirections Direction, int Duration);

    void SetPark();

    void SlewToAltAz(double Azimuth, double Altitude);

    void SlewToAltAzAsync(double Azimuth, double Altitude);

    void SlewToCoordinates(double RightAscension, double Declination);

    void SlewToCoordinatesAsync(double RightAscension, double Declination);

    void SlewToTarget();

    void SlewToTargetAsync();

    void SyncToAltAz(double Azimuth, double Altitude);

    void SyncToCoordinates(double RightAscension, double Declination);

    void SyncToTarget();

    void Unpark();
  }
}
