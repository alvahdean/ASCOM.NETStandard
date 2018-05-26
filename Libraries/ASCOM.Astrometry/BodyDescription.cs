// Decompiled with JetBrains decompiler
// Type: ASCOM.Astrometry.BodyDescription
// Assembly: ASCOM.Astrometry, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 695EF5AA-123E-4689-AAF5-64FD1324F6BB
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.Astrometry\6.0.0.0__565de7938946fba7\ASCOM.Astrometry.dll

using System.Runtime.InteropServices;

namespace ASCOM.Astrometry
{
  //[Guid("558F644F-E112-4e88-9D79-20063BB25C3E")]
  //[ComVisible(true)]
  public struct BodyDescription
  {
    public BodyType Type;
    public Body Number;
    [MarshalAs(UnmanagedType.BStr)]
    public string Name;
  }
}
