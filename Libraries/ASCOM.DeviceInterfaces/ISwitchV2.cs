// Decompiled with JetBrains decompiler
// Type: ASCOM.DeviceInterface.ISwitchV2
// Assembly: ASCOM.DeviceInterfaces, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 1FB1AAA7-A5CF-493B-90DA-CE61713D2AB5
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.DeviceInterfaces\6.0.0.0__565de7938946fba7\ASCOM.DeviceInterfaces.dll

using System.Collections;
using System.Runtime.InteropServices;

namespace ASCOM.DeviceInterface
{
  //[Guid("71A6CA6B-A86B-4EBB-8DA3-6D91705177A3")]
  //[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
  //[ComVisible(true)]
  public interface ISwitchV2 : IAscomDriver
    {
    short MaxSwitch { get; }

    string GetSwitchName(short id);

    void SetSwitchName(short id, string name);

    string GetSwitchDescription(short id);

    bool CanWrite(short id);

    bool GetSwitch(short id);

    void SetSwitch(short id, bool state);

    double MaxSwitchValue(short id);

    double MinSwitchValue(short id);

    double SwitchStep(short id);

    double GetSwitchValue(short id);

    void SetSwitchValue(short id, double value);
  }
}
