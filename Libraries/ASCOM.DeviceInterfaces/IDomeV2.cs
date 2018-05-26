// Decompiled with JetBrains decompiler
// Type: ASCOM.DeviceInterface.IDomeV2
// Assembly: ASCOM.DeviceInterfaces, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 1FB1AAA7-A5CF-493B-90DA-CE61713D2AB5
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.DeviceInterfaces\6.0.0.0__565de7938946fba7\ASCOM.DeviceInterfaces.dll

using System.Collections;
using System.Runtime.InteropServices;

namespace ASCOM.DeviceInterface
{
  //[Guid("88CFA00C-DDD3-4b42-A1F0-9387E6823832")]
  //[ComVisible(true)]
  //[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
  public interface IDomeV2 : IAscomDriver
  {
    double Altitude { get; }

    bool AtHome { get; }

    bool AtPark { get; }

    double Azimuth { get; }

    bool CanFindHome { get; }

    bool CanPark { get; }

    bool CanSetAltitude { get; }

    bool CanSetAzimuth { get; }

    bool CanSetPark { get; }

    bool CanSetShutter { get; }

    bool CanSlave { get; }

    bool CanSyncAzimuth { get; }

    ShutterState ShutterStatus { get; }

    bool Slaved { get; set; }

    bool Slewing { get; }

    void AbortSlew();

    void CloseShutter();

    void FindHome();

    void OpenShutter();

    void Park();

    void SetPark();

    void SlewToAltitude(double Altitude);

    void SlewToAzimuth(double Azimuth);

    void SyncToAzimuth(double Azimuth);
  }
}
