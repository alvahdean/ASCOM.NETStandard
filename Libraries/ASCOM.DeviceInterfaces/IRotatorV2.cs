// Decompiled with JetBrains decompiler
// Type: ASCOM.DeviceInterface.IRotatorV2
// Assembly: ASCOM.DeviceInterfaces, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 1FB1AAA7-A5CF-493B-90DA-CE61713D2AB5
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.DeviceInterfaces\6.0.0.0__565de7938946fba7\ASCOM.DeviceInterfaces.dll

using System.Collections;
using System.Runtime.InteropServices;

namespace ASCOM.DeviceInterface
{
  //[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
  //[ComVisible(true)]
  //[Guid("692FA48C-4A30-4543-8681-DA0733758F11")]
  public interface IRotatorV2: IAscomDriver
  {
    bool CanReverse { get; }

    bool IsMoving { get; }

    float Position { get; }

    bool Reverse { get; set; }

    float StepSize { get; }

    float TargetPosition { get; }

    void Halt();

    void Move(float Position);

    void MoveAbsolute(float Position);
  }
}
