// Decompiled with JetBrains decompiler
// Type: ASCOM.DeviceInterface.IFocuserV2
// Assembly: ASCOM.DeviceInterfaces, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 1FB1AAA7-A5CF-493B-90DA-CE61713D2AB5
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.DeviceInterfaces\6.0.0.0__565de7938946fba7\ASCOM.DeviceInterfaces.dll

using System.Collections;
using System.Runtime.InteropServices;

namespace ASCOM.DeviceInterface
{
    //[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    //[ComVisible(true)]
    //[Guid("E430C8A8-539E-4558-895D-A2F293D946E7")]
    public interface IFocuserV2 : IAscomDriver
    {
        bool Absolute { get; }

        bool IsMoving { get; }

        bool Link { get; set; }

        int MaxIncrement { get; }

        int MaxStep { get; }

        int Position { get; }

        double StepSize { get; }

        bool TempComp { get; set; }

        bool TempCompAvailable { get; }

        double Temperature { get; }

        void Halt();

        void Move(int position);
    }
}