// Decompiled with JetBrains decompiler
// Type: ASCOM.Astrometry.Kepler.IEphemeris
// Assembly: ASCOM.Astrometry, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 695EF5AA-123E-4689-AAF5-64FD1324F6BB
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.Astrometry\6.0.0.0__565de7938946fba7\ASCOM.Astrometry.dll

using System.Runtime.InteropServices;

namespace ASCOM.Astrometry.Kepler
{
  //[ComVisible(true)]
  //[Guid("54A8F586-C7B7-4899-8AA1-6044BDE4ABFA")]
  public interface IEphemeris
  {
    [DispId(2)]
    double a { get; set; }

    [DispId(3)]
    BodyType BodyType { get; set; }

    [DispId(4)]
    double e { get; set; }

    [DispId(5)]
    double Epoch { get; set; }

    [DispId(6)]
    double G { get; set; }

    [DispId(7)]
    double H { get; set; }

    [DispId(8)]
    double Incl { get; set; }

    [DispId(9)]
    double M { get; set; }

    [DispId(10)]
    double n { get; set; }

    [DispId(11)]
    string Name { get; set; }

    [DispId(12)]
    double Node { get; set; }

    [DispId(13)]
    Body Number { get; set; }

    [DispId(14)]
    double P { get; set; }

    [DispId(15)]
    double Peri { get; set; }

    [DispId(16)]
    double q { get; set; }

    [DispId(17)]
    double z { get; set; }

    [DispId(1)]
    double[] GetPositionAndVelocity(double tjd);
  }
}
