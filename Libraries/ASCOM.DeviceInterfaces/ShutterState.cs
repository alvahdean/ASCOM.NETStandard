// Decompiled with JetBrains decompiler
// Type: ASCOM.DeviceInterface.ShutterState
// Assembly: ASCOM.DeviceInterfaces, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 1FB1AAA7-A5CF-493B-90DA-CE61713D2AB5
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.DeviceInterfaces\6.0.0.0__565de7938946fba7\ASCOM.DeviceInterfaces.dll

using System.Runtime.InteropServices;

namespace ASCOM.DeviceInterface
{
  //[Guid("DA182F18-4133-4d6f-A533-67306F48AC5C")]
  //[ComVisible(true)]
  public enum ShutterState
  {
    shutterOpen,
    shutterClosed,
    shutterOpening,
    shutterClosing,
    shutterError,
  }
}
