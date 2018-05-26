// Decompiled with JetBrains decompiler
// Type: ASCOM.Astrometry.Observer
// Assembly: ASCOM.Astrometry, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 695EF5AA-123E-4689-AAF5-64FD1324F6BB
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.Astrometry\6.0.0.0__565de7938946fba7\ASCOM.Astrometry.dll

using System.Runtime.InteropServices;

namespace ASCOM.Astrometry
{
  //[ComVisible(true)]
  //[Guid("64A25FDD-3687-45e0-BEAF-18C361E5E340")]
  public struct Observer
  {
    public ObserverLocation Where;
    public OnSurface OnSurf;
    public InSpace NearEarth;
  }
}
