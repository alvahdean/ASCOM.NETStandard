// Decompiled with JetBrains decompiler
// Type: ASCOM.Utilities.Interfaces.ITimerEvent
// Assembly: ASCOM.Utilities, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 06F2ED6E-C559-496D-B7FF-DCA800F2230D
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.Utilities\6.0.0.0__565de7938946fba7\ASCOM.Utilities.dll

using System.Runtime.InteropServices;

namespace ASCOM.Utilities.Interfaces
{
  //[Guid("BDDA4DFD-77F8-4bd2-ACC0-AF32B4F8B9C2")]
  //[ComVisible(true)]
  [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
  public interface ITimerEvent
  {
    [DispId(1)]
    void Tick();
  }
}
