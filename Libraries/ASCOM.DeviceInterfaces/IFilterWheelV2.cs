// Decompiled with JetBrains decompiler
// Type: ASCOM.DeviceInterface.IFilterWheelV2
// Assembly: ASCOM.DeviceInterfaces, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 1FB1AAA7-A5CF-493B-90DA-CE61713D2AB5
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.DeviceInterfaces\6.0.0.0__565de7938946fba7\ASCOM.DeviceInterfaces.dll

using System.Collections;
using System.Runtime.InteropServices;

namespace ASCOM.DeviceInterface
{
  //[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
  //[Guid("DCF3858D-D68E-45ed-8141-1C899B4B432A")]
  //[ComVisible(true)]
  public interface IFilterWheelV2 : IAscomDriver
    {

    int[] FocusOffsets { get; }

    string[] Names { get; }

    short Position { get; set; }

  }
}
