// Decompiled with JetBrains decompiler
// Type: ASCOM.Astrometry.NOVASCOM.IStar
// Assembly: ASCOM.Astrometry, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 695EF5AA-123E-4689-AAF5-64FD1324F6BB
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.Astrometry\6.0.0.0__565de7938946fba7\ASCOM.Astrometry.dll

using System.Runtime.InteropServices;

namespace ASCOM.Astrometry.NOVASCOM
{
  [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
  //[ComVisible(true)]
  //[Guid("89145C95-9B78-494e-99FE-BD2EF4386096")]
  public interface IStar
  {
    [DispId(8)]
    string Catalog { get; set; }

    [DispId(9)]
    double Declination { get; set; }

    [DispId(10)]
    double DeltaT { get; set; }

    [DispId(11)]
    object EarthEphemeris { get; set; }

    [DispId(12)]
    string Name { get; set; }

    [DispId(13)]
    int Number { get; set; }

    [DispId(14)]
    double Parallax { get; set; }

    [DispId(15)]
    double ProperMotionDec { get; set; }

    [DispId(16)]
    double ProperMotionRA { get; set; }

    [DispId(17)]
    double RadialVelocity { get; set; }

    [DispId(18)]
    double RightAscension { get; set; }

    [DispId(1)]
    void Set(double RA, double Dec, double ProMoRA, double ProMoDec, double Parallax, double RadVel);

    [DispId(2)]
    void SetHipparcos(double RA, double Dec, double ProMoRA, double ProMoDec, double Parallax, double RadVel);

    [DispId(3)]
    PositionVector GetApparentPosition(double tjd);

    [DispId(4)]
    PositionVector GetAstrometricPosition(double tjd);

    [DispId(5)]
    PositionVector GetLocalPosition(double tjd, [MarshalAs(UnmanagedType.IDispatch)] Site site);

    [DispId(6)]
    PositionVector GetTopocentricPosition(double tjd, [MarshalAs(UnmanagedType.IDispatch)] Site site, bool Refract);

    [DispId(7)]
    PositionVector GetVirtualPosition(double tjd);
  }
}
