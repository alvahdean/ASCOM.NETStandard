// Decompiled with JetBrains decompiler
// Type: ASCOM.DeviceInterface.IObservingConditions
// Assembly: ASCOM.DeviceInterfaces, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 1FB1AAA7-A5CF-493B-90DA-CE61713D2AB5
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.DeviceInterfaces\6.0.0.0__565de7938946fba7\ASCOM.DeviceInterfaces.dll

using System.Collections;
using System.Runtime.InteropServices;

namespace ASCOM.DeviceInterface
{
  //[ComVisible(true)]
  //[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
  //[Guid("06E9F8D9-E85C-4B2B-BC84-6F2EF6B3E779")]
  public interface IObservingConditions : IAscomDriver
    {

    double AveragePeriod { get; set; }

    double CloudCover { get; }

    double DewPoint { get; }

    double Humidity { get; }

    double Pressure { get; }

    double RainRate { get; }

    double SkyBrightness { get; }

    double SkyQuality { get; }

    double StarFWHM { get; }

    double SkyTemperature { get; }

    double Temperature { get; }

    double WindDirection { get; }

    double WindGust { get; }

    double WindSpeed { get; }

    double TimeSinceLastUpdate(string PropertyName);

    string SensorDescription(string PropertyName);

    void Refresh();
  }
}
