// Decompiled with JetBrains decompiler
// Type: ASCOM.Astrometry.AstroUtils.IAstroUtils
// Assembly: ASCOM.Astrometry, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 695EF5AA-123E-4689-AAF5-64FD1324F6BB
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.Astrometry\6.0.0.0__565de7938946fba7\ASCOM.Astrometry.dll

using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace ASCOM.Astrometry.AstroUtils
{
  [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
  //[Guid("143068F6-ADC9-4751-AC39-924111396F0F")]
  //[ComVisible(true)]
  public interface IAstroUtils
  {
    [DispId(5)]
    double JulianDateUtc { get; }

    [DispId(15)]
    int LeapSeconds { get; set; }

    [DispId(1)]
    double ConditionRA(double RA);

    [DispId(2)]
    double ConditionHA(double HA);

    [DispId(3)]
    double DeltaT();

    [DispId(4)]
    double UnRefract(OnSurface Location, RefractionOption RefOption, double ZdObs);

    [DispId(6)]
    double JulianDateTT(double DeltaUT1);

    [DispId(7)]
    double JulianDateUT1(double DeltaUT1);

    [DispId(8)]
    double Range(double Value, double LowerBound, bool LowerEqual, double UpperBound, bool UpperEqual);

    [DispId(9)]
    double CalendarToMJD(int Day, int Month, int Year);

    [DispId(10)]
    double MJDToOADate(double MJD);

    [DispId(11)]
    DateTime MJDToDate(double MJD);

    [DispId(12)]
    string FormatMJD(double MJD, string PresentationFormat);

    [DispId(13)]
    double DeltaUT(double JulianDate);

    [DispId(14)]
    string FormatJD(double JD, string PresentationFormat);

    [DispId(16)]
    ArrayList EventTimes(EventType TypeofEvent, int Day, int Month, int Year, double SiteLatitude, double SiteLongitude, double SiteTimeZone);

    [DispId(17)]
    double MoonIllumination(double JD);

    [DispId(18)]
    double MoonPhase(double JD);
  }
}
