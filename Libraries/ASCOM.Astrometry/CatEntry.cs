// Decompiled with JetBrains decompiler
// Type: ASCOM.Astrometry.CatEntry
// Assembly: ASCOM.Astrometry, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 695EF5AA-123E-4689-AAF5-64FD1324F6BB
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.Astrometry\6.0.0.0__565de7938946fba7\ASCOM.Astrometry.dll

using System.Runtime.InteropServices;

namespace ASCOM.Astrometry
{
  //[ComVisible(true)]
  //[Guid("6320FEDA-8582-4048-988A-7D4DE7978C71")]
  public struct CatEntry
  {
    [MarshalAs(UnmanagedType.BStr)]
    public string Catalog;
    [MarshalAs(UnmanagedType.BStr)]
    public string StarName;
    public int StarNumber;
    public double RA;
    public double Dec;
    public double ProMoRA;
    public double ProMoDec;
    public double Parallax;
    public double RadialVelocity;
  }
}
