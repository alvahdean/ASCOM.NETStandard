// Decompiled with JetBrains decompiler
// Type: ASCOM.Astrometry.NOVASCOM.ISite
// Assembly: ASCOM.Astrometry, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 695EF5AA-123E-4689-AAF5-64FD1324F6BB
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.Astrometry\6.0.0.0__565de7938946fba7\ASCOM.Astrometry.dll

using System.Runtime.InteropServices;

namespace ASCOM.Astrometry.NOVASCOM
{
  //[Guid("2414C071-8A5B-4d53-89BC-CAF30BA7123B")]
  //[ComVisible(true)]
  [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
  public interface ISite
  {
    [DispId(2)]
    double Height { get; set; }

    [DispId(3)]
    double Latitude { get; set; }

    [DispId(4)]
    double Longitude { get; set; }

    [DispId(5)]
    double Pressure { get; set; }

    [DispId(6)]
    double Temperature { get; set; }

    [DispId(1)]
    void Set(double Latitude, double Longitude, double Height);
  }
}
