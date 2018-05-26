// Decompiled with JetBrains decompiler
// Type: ASCOM.DeviceInterface.ICameraV2
// Assembly: ASCOM.DeviceInterfaces, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 1FB1AAA7-A5CF-493B-90DA-CE61713D2AB5
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.DeviceInterfaces\6.0.0.0__565de7938946fba7\ASCOM.DeviceInterfaces.dll

using System.Collections;
using System.Runtime.InteropServices;

namespace ASCOM.DeviceInterface
{
  //[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
  //[ComVisible(true)]
  //[Guid("972CEBC6-0EBE-4efc-99DD-CC5293FDE77B")]
  public interface ICameraV2 : IAscomDriver
  {

    short BinX { get; set; }

    short BinY { get; set; }

    CameraStates CameraState { get; }

    int CameraXSize { get; }

    int CameraYSize { get; }

    bool CanAbortExposure { get; }

    bool CanAsymmetricBin { get; }

    bool CanGetCoolerPower { get; }

    bool CanPulseGuide { get; }

    bool CanSetCCDTemperature { get; }

    bool CanStopExposure { get; }

    double CCDTemperature { get; }

    bool CoolerOn { get; set; }

    double CoolerPower { get; }

    double ElectronsPerADU { get; }

    double FullWellCapacity { get; }

    bool HasShutter { get; }

    double HeatSinkTemperature { get; }

    object ImageArray { get; }

    object ImageArrayVariant { get; }

    bool ImageReady { get; }

    bool IsPulseGuiding { get; }

    double LastExposureDuration { get; }

    string LastExposureStartTime { get; }

    int MaxADU { get; }

    short MaxBinX { get; }

    short MaxBinY { get; }

    int NumX { get; set; }

    int NumY { get; set; }

    double PixelSizeX { get; }

    double PixelSizeY { get; }

    double SetCCDTemperature { get; set; }

    int StartX { get; set; }

    int StartY { get; set; }

    short BayerOffsetX { get; }

    short BayerOffsetY { get; }

    bool CanFastReadout { get; }

    double ExposureMax { get; }

    double ExposureMin { get; }

    double ExposureResolution { get; }

    bool FastReadout { get; set; }

    short Gain { get; set; }

    short GainMax { get; }

    short GainMin { get; }

    ArrayList Gains { get; }

    short PercentCompleted { get; }

    short ReadoutMode { get; set; }

    ArrayList ReadoutModes { get; }

    string SensorName { get; }

    SensorType SensorType { get; }

    void AbortExposure();

    void PulseGuide(GuideDirections Direction, int Duration);

    void StartExposure(double Duration, bool Light);

    void StopExposure();
  }
}
