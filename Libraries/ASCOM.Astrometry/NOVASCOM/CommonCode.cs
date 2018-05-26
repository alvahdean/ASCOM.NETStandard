// Decompiled with JetBrains decompiler
// Type: ASCOM.Astrometry.NOVASCOM.CommonCode
// Assembly: ASCOM.Astrometry, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 695EF5AA-123E-4689-AAF5-64FD1324F6BB
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.Astrometry\6.0.0.0__565de7938946fba7\ASCOM.Astrometry.dll



namespace ASCOM.Astrometry.NOVASCOM
{
  
  internal sealed class CommonCode
  {
    internal static Body NumberToBody(int Number)
    {
      switch (Number)
      {
        case 1:
          return Body.Mercury;
        case 2:
          return Body.Venus;
        case 3:
          return Body.Earth;
        case 4:
          return Body.Mars;
        case 5:
          return Body.Jupiter;
        case 6:
          return Body.Saturn;
        case 7:
          return Body.Uranus;
        case 8:
          return Body.Neptune;
        case 9:
          return Body.Pluto;
        case 10:
          return Body.Sun;
        case 11:
          return Body.Moon;
        default:
          throw new InvalidValueException("PlanetNumberToBody", Number.ToString(), "1 to 11");
      }
    }
  }
}
