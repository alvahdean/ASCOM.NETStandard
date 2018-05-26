// Decompiled with JetBrains decompiler
// Type: ASCOM.Utilities.Interfaces.ITimer
// Assembly: ASCOM.Utilities, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 06F2ED6E-C559-496D-B7FF-DCA800F2230D
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.Utilities\6.0.0.0__565de7938946fba7\ASCOM.Utilities.dll

using System.Runtime.InteropServices;

namespace ASCOM.Utilities.Interfaces
{
  //[ComVisible(true)]
  //[Guid("23A8A279-FB8E-4b3c-8F2E-010AC0F98588")]
  public interface ITimer
  {
    [DispId(1)]
    int Interval { get; set; }

    [DispId(2)]
    bool Enabled { get; set; }
  }
}
