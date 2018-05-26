// Decompiled with JetBrains decompiler
// Type: ASCOM.Astrometry.CatEntry3
// Assembly: ASCOM.Astrometry, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 695EF5AA-123E-4689-AAF5-64FD1324F6BB
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.Astrometry\6.0.0.0__565de7938946fba7\ASCOM.Astrometry.dll

using System.Runtime.InteropServices;

namespace ASCOM.Astrometry
{
  //[ComVisible(true)]
  //[Guid("5325E96C-BD24-4470-A0F6-E917B05805E1")]
  public struct CatEntry3
  {
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 51)]
    public string StarName;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
    public string Catalog;
    public int StarNumber;
    public double RA;
    public double Dec;
    public double ProMoRA;
    public double ProMoDec;
    public double Parallax;
    public double RadialVelocity;
  }
}
