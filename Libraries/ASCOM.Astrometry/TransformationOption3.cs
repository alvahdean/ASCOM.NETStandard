// Decompiled with JetBrains decompiler
// Type: ASCOM.Astrometry.TransformationOption3
// Assembly: ASCOM.Astrometry, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 695EF5AA-123E-4689-AAF5-64FD1324F6BB
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.Astrometry\6.0.0.0__565de7938946fba7\ASCOM.Astrometry.dll

using System.Runtime.InteropServices;

namespace ASCOM.Astrometry
{
  //[Guid("8BBA934E-D874-48a2-A3E2-C842A7FFFB35")]
  //[ComVisible(true)]
  public enum TransformationOption3 : short
  {
    ChangeEpoch = 1,
    ChangeEquatorAndEquinox = 2,
    ChangeEquatorAndEquinoxAndEpoch = 3,
    ChangeEquatorAndEquinoxJ2000ToICRS = 4,
    ChangeICRSToEquatorAndEquinoxOfJ2000 = 5,
  }
}
