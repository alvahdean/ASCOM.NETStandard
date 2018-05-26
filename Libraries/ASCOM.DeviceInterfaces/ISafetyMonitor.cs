// Decompiled with JetBrains decompiler
// Type: ASCOM.DeviceInterface.ISafetyMonitor
// Assembly: ASCOM.DeviceInterfaces, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 1FB1AAA7-A5CF-493B-90DA-CE61713D2AB5
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.DeviceInterfaces\6.0.0.0__565de7938946fba7\ASCOM.DeviceInterfaces.dll

using System.Collections;
using System.Runtime.InteropServices;

namespace ASCOM.DeviceInterface
{
  //[Guid("90F92092-DD68-4AA5-845C-7061F328B73E")]
  //[ComVisible(true)]
  //[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
  public interface ISafetyMonitor : IAscomDriver
    {
    bool IsSafe { get; }

  }
}
