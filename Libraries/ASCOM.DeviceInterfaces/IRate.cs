// Decompiled with JetBrains decompiler
// Type: ASCOM.DeviceInterface.IRate
// Assembly: ASCOM.DeviceInterfaces, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 1FB1AAA7-A5CF-493B-90DA-CE61713D2AB5
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.DeviceInterfaces\6.0.0.0__565de7938946fba7\ASCOM.DeviceInterfaces.dll

using System.Runtime.InteropServices;

namespace ASCOM.DeviceInterface
{
  //[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
  //[Guid("2E7CEEE4-B5C6-4e9a-87F4-80445700D123")]
  //[ComVisible(true)]
  public interface IRate
  {
    double Maximum { get; set; }

    double Minimum { get; set; }

    void Dispose();
  }
}
