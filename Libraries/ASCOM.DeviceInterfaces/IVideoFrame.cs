// Decompiled with JetBrains decompiler
// Type: ASCOM.DeviceInterface.IVideoFrame
// Assembly: ASCOM.DeviceInterfaces, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 1FB1AAA7-A5CF-493B-90DA-CE61713D2AB5
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.DeviceInterfaces\6.0.0.0__565de7938946fba7\ASCOM.DeviceInterfaces.dll

using System.Collections;
using System.Runtime.InteropServices;

namespace ASCOM.DeviceInterface
{
  //[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
  //[ComVisible(true)]
  //[Guid("EA1D5478-7263-43F8-B708-78783A48158C")]
  public interface IVideoFrame
  {
    object ImageArray { get; }

    byte[] PreviewBitmap { get; }

    long FrameNumber { get; }

    double ExposureDuration { get; }

    string ExposureStartTime { get; }

    ArrayList ImageMetadata { get; }
  }
}
