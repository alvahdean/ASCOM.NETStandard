// Decompiled with JetBrains decompiler
// Type: ASCOM.Astrometry.Transform.ITransform
// Assembly: ASCOM.Astrometry, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 695EF5AA-123E-4689-AAF5-64FD1324F6BB
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.Astrometry\6.0.0.0__565de7938946fba7\ASCOM.Astrometry.dll

using System.Runtime.InteropServices;

namespace ASCOM.Astrometry.Transform
{
  //[Guid("6F38768E-C52D-41c7-9E0F-B8E4AFE341A7")]
  //[ComVisible(true)]
  public interface ITransform
  {
    [DispId(1)]
    double SiteLatitude { get; set; }

    [DispId(2)]
    double SiteLongitude { get; set; }

    [DispId(3)]
    double SiteElevation { get; set; }

    [DispId(4)]
    double SiteTemperature { get; set; }

    [DispId(5)]
    bool Refraction { get; set; }

    [DispId(10)]
    double RAJ2000 { get; }

    [DispId(11)]
    double DECJ2000 { get; }

    [DispId(12)]
    double RATopocentric { get; }

    [DispId(13)]
    double DECTopocentric { get; }

    [DispId(14)]
    double RAApparent { get; }

    [DispId(15)]
    double DECApparent { get; }

    [DispId(16)]
    double AzimuthTopocentric { get; }

    [DispId(17)]
    double ElevationTopocentric { get; }

    [DispId(19)]
    double JulianDateTT { get; set; }

    [DispId(20)]
    double JulianDateUTC { get; set; }

    [DispId(6)]
    void Refresh();

    [DispId(7)]
    void SetJ2000(double RA, double DEC);

    [DispId(8)]
    void SetApparent(double RA, double DEC);

    [DispId(9)]
    void SetTopocentric(double RA, double DEC);

    [DispId(18)]
    void SetAzimuthElevation(double Azimuth, double Elevation);
  }
}
