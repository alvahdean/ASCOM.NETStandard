// Decompiled with JetBrains decompiler
// Type: ASCOM.Astrometry.NOVASCOM.IPositionVector
// Assembly: ASCOM.Astrometry, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 695EF5AA-123E-4689-AAF5-64FD1324F6BB
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.Astrometry\6.0.0.0__565de7938946fba7\ASCOM.Astrometry.dll

using System.Runtime.InteropServices;

namespace ASCOM.Astrometry.NOVASCOM
{
  //[Guid("A3B6F9AA-B331-47c7-B8F0-4FBECF0638AA")]
  //[ComVisible(true)]
  [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
  public interface IPositionVector
  {
    [DispId(7)]
    double Azimuth { get; }

    [DispId(8)]
    double Declination { get; }

    [DispId(9)]
    double Distance { get; }

    [DispId(10)]
    double Elevation { get; }

    [DispId(11)]
    double LightTime { get; }

    [DispId(12)]
    double RightAscension { get; }

    [DispId(13)]
    double x { get; set; }

    [DispId(14)]
    double y { get; set; }

    [DispId(15)]
    double z { get; set; }

    [DispId(1)]
    void Aberration([MarshalAs(UnmanagedType.IDispatch)] VelocityVector vel);

    [DispId(2)]
    void Precess(double tjd, double tjd2);

    [DispId(3)]
    bool ProperMotion([MarshalAs(UnmanagedType.IDispatch)] VelocityVector vel, double tjd1, double tjd2);

    [DispId(4)]
    bool SetFromSite([MarshalAs(UnmanagedType.IDispatch)] Site site, double gast);

    [DispId(5)]
    bool SetFromSiteJD([MarshalAs(UnmanagedType.IDispatch)] Site site, double ujd, double delta_t);

    [DispId(6)]
    bool SetFromStar([MarshalAs(UnmanagedType.IDispatch)] Star star);
  }
}
