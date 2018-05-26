// Decompiled with JetBrains decompiler
// Type: ASCOM.Astrometry.NOVASCOM.IVelocityVector
// Assembly: ASCOM.Astrometry, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 695EF5AA-123E-4689-AAF5-64FD1324F6BB
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.Astrometry\6.0.0.0__565de7938946fba7\ASCOM.Astrometry.dll

using System.Runtime.InteropServices;

namespace ASCOM.Astrometry.NOVASCOM
{
  //[ComVisible(true)]
  //[Guid("8DD80835-29C6-49d6-8E4D-8887B20E707E")]
  [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
  public interface IVelocityVector
  {
    [DispId(4)]
    double DecVelocity { get; }

    [DispId(5)]
    double RadialVelocity { get; }

    [DispId(6)]
    double RAVelocity { get; }

    [DispId(7)]
    double x { get; set; }

    [DispId(8)]
    double y { get; set; }

    [DispId(9)]
    double z { get; set; }

    [DispId(1)]
    bool SetFromSite([MarshalAs(UnmanagedType.IDispatch)] Site site, double gast);

    [DispId(2)]
    bool SetFromSiteJD([MarshalAs(UnmanagedType.IDispatch)] Site site, double ujd, double delta_t);

    [DispId(3)]
    bool SetFromStar([MarshalAs(UnmanagedType.IDispatch)] Star star);
  }
}
