// Decompiled with JetBrains decompiler
// Type: ASCOM.DeviceInterface.CameraStates
// Assembly: ASCOM.DeviceInterfaces, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 1FB1AAA7-A5CF-493B-90DA-CE61713D2AB5
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.DeviceInterfaces\6.0.0.0__565de7938946fba7\ASCOM.DeviceInterfaces.dll

using System.Runtime.InteropServices;

namespace ASCOM.DeviceInterface
{
  //[ComVisible(true)]
  //[Guid("BBD1CD3C-5983-4584-96F9-E22AB0F8BB31")]
  public enum CameraStates
  {
    cameraIdle,
    cameraWaiting,
    cameraExposing,
    cameraReading,
    cameraDownload,
    cameraError,
  }
}
