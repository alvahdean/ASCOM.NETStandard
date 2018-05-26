// Decompiled with JetBrains decompiler
// Type: ASCOM.Astrometry.NOVASCOM.IPlanet
// Assembly: ASCOM.Astrometry, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 695EF5AA-123E-4689-AAF5-64FD1324F6BB
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.Astrometry\6.0.0.0__565de7938946fba7\ASCOM.Astrometry.dll

using ASCOM.Astrometry.Kepler;
using System.Runtime.InteropServices;

namespace ASCOM.Astrometry.NOVASCOM
{
  [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
  //[Guid("CAE65556-EA7A-4252-BF28-D0E967AEF04D")]
  //[ComVisible(true)]
  public interface IPlanet
  {
    [DispId(6)]
    double DeltaT { get; set; }

    [DispId(7)]
    IEphemeris EarthEphemeris { get; set; }

    [DispId(8)]
    IEphemeris Ephemeris { get; set; }

    [DispId(9)]
    string Name { get; set; }

    [DispId(10)]
    int Number { get; set; }

    [DispId(11)]
    BodyType Type { get; set; }

    [DispId(1)]
    PositionVector GetApparentPosition(double tjd);

    [DispId(2)]
    PositionVector GetAstrometricPosition(double tjd);

    [DispId(3)]
    PositionVector GetLocalPosition(double tjd, [MarshalAs(UnmanagedType.IDispatch)] Site site);

    [DispId(4)]
    PositionVector GetTopocentricPosition(double tjd, [MarshalAs(UnmanagedType.IDispatch)] Site site, bool Refract);

    [DispId(5)]
    PositionVector GetVirtualPosition(double tjd);
  }
}
