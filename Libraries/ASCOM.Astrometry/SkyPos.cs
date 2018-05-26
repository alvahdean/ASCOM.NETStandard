// Decompiled with JetBrains decompiler
// Type: ASCOM.Astrometry.SkyPos
// Assembly: ASCOM.Astrometry, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 695EF5AA-123E-4689-AAF5-64FD1324F6BB
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.Astrometry\6.0.0.0__565de7938946fba7\ASCOM.Astrometry.dll

using System.Runtime.InteropServices;

namespace ASCOM.Astrometry
{
  //[ComVisible(true)]
  //[Guid("9AD852C3-A895-4f69-AEC0-C9CA44283FA0")]
  public struct SkyPos
  {
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.R8)]
    public double[] RHat;
    public double RA;
    public double Dec;
    public double Dis;
    public double RV;
  }
}
