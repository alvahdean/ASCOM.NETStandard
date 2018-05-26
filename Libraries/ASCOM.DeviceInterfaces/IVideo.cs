// Decompiled with JetBrains decompiler
// Type: ASCOM.DeviceInterface.IVideo
// Assembly: ASCOM.DeviceInterfaces, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 1FB1AAA7-A5CF-493B-90DA-CE61713D2AB5
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.DeviceInterfaces\6.0.0.0__565de7938946fba7\ASCOM.DeviceInterfaces.dll

using System.Collections;
using System.Runtime.InteropServices;

namespace ASCOM.DeviceInterface
{
  //[Guid("00A394A5-BCB0-449D-A46B-81A02824ADC5")]
  //[ComVisible(true)]
  //[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
  public interface IVideo : IAscomDriver
    {
    string VideoCaptureDeviceName { get; }

    double ExposureMax { get; }

    double ExposureMin { get; }

    VideoCameraFrameRate FrameRate { get; }

    ArrayList SupportedIntegrationRates { get; }

    int IntegrationRate { get; set; }

    IVideoFrame LastVideoFrame { get; }

    string SensorName { get; }

    SensorType SensorType { get; }

    int Width { get; }

    int Height { get; }

    double PixelSizeX { get; }

    double PixelSizeY { get; }

    int BitDepth { get; }

    string VideoCodec { get; }

    string VideoFileFormat { get; }

    int VideoFramesBufferSize { get; }

    VideoCameraState CameraState { get; }

    short GainMax { get; }

    short GainMin { get; }

    short Gain { get; set; }

    ArrayList Gains { get; }

    short GammaMax { get; }

    short GammaMin { get; }

    short Gamma { get; set; }

    ArrayList Gammas { get; }

    bool CanConfigureDeviceProperties { get; }

    string StartRecordingVideoFile(string PreferredFileName);

    void StopRecordingVideoFile();

    void ConfigureDeviceProperties();
  }
}
