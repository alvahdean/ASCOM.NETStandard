// Decompiled with JetBrains decompiler
// Type: ASCOM.Astrometry.Object3Internal
// Assembly: ASCOM.Astrometry, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 695EF5AA-123E-4689-AAF5-64FD1324F6BB
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.Astrometry\6.0.0.0__565de7938946fba7\ASCOM.Astrometry.dll

using System.Runtime.InteropServices;

namespace ASCOM.Astrometry
{
  internal struct Object3Internal
  {
    public ObjectType Type;
    public short Number;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 51)]
    public string Name;
    public CatEntry3 Star;
  }
}
