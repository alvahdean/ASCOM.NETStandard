// Decompiled with JetBrains decompiler
// Type: ASCOM.Astrometry.NOVASCOM.IEarth
// Assembly: ASCOM.Astrometry, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 695EF5AA-123E-4689-AAF5-64FD1324F6BB
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.Astrometry\6.0.0.0__565de7938946fba7\ASCOM.Astrometry.dll

using ASCOM.Astrometry.Kepler;
using System.Runtime.InteropServices;

namespace ASCOM.Astrometry.NOVASCOM
{
  //[ComVisible(true)]
  [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
  //[Guid("FF6DA248-BA2A-4a62-BA0A-AAD433EAAC85")]
  public interface IEarth
  {
    [DispId(2)]
    PositionVector BarycentricPosition { get; }

    [DispId(3)]
    double BarycentricTime { get; }

    [DispId(4)]
    VelocityVector BarycentricVelocity { get; }

    [DispId(5)]
    IEphemeris EarthEphemeris { get; set; }

    [DispId(6)]
    double EquationOfEquinoxes { get; }

    [DispId(7)]
    PositionVector HeliocentricPosition { get; }

    [DispId(8)]
    VelocityVector HeliocentricVelocity { get; }

    [DispId(9)]
    double MeanObliquity { get; }

    [DispId(10)]
    double NutationInLongitude { get; }

    [DispId(11)]
    double NutationInObliquity { get; }

    [DispId(12)]
    double TrueObliquity { get; }

    [DispId(1)]
    bool SetForTime(double tjd);
  }
}
