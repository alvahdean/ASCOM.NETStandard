// Decompiled with JetBrains decompiler
// Type: ASCOM.DriverAccess.Camera
// Assembly: ASCOM.DriverAccess, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 4B15C8A3-EB2C-468B-8912-4BBDBC985936
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.DriverAccess\6.0.0.0__565de7938946fba7\ASCOM.DriverAccess.dll

using ASCOM.DeviceInterface;
using ASCOM.Utilities;
using System;
using System.Collections;

namespace ASCOM.DriverAccess
{
    public class Camera : AscomDriver<ICameraV2>, ICameraV2
    {
        private readonly short _driverInterfaceVersion;

        public Camera(string cameraId) : base(cameraId)
        {
        }

        public short BinX
        {
            get => Impl.BinX;
            set
            {
                if (BinX != value)
                {
                    Impl.BinX = value;
                    profile.SetValue(nameof(BinX), Impl.BinX.ToString());
                    RaisePropertyChanged(nameof(BinX));
                }
            }
        }

        public short BinY
        {
            get => Impl.BinY;
            set
            {
                if (BinY != value)
                {
                    Impl.BinY = value;
                    profile.SetValue(nameof(BinY), Impl.BinY.ToString());
                    RaisePropertyChanged(nameof(BinY));
                }
            }
        }

        public double CCDTemperature { get => Impl.CCDTemperature; }

        public CameraStates CameraState { get => Impl.CameraState; }

        public int CameraXSize { get => Impl.CameraXSize; }

        public int CameraYSize { get => Impl.CameraYSize; }

        public bool CanAbortExposure { get => Impl.CanAbortExposure; }

        public bool CanAsymmetricBin { get => Impl.CanAsymmetricBin; }

        public bool CanGetCoolerPower { get => Impl.CanGetCoolerPower; }

        public bool CanPulseGuide { get => Impl.CanPulseGuide; }

        public bool CanSetCCDTemperature { get => Impl.CanSetCCDTemperature; }

        public bool CanStopExposure { get => Impl.CanStopExposure; }

        public bool CoolerOn
        {
            get => Impl.CoolerOn;
            set
            {
                if (CoolerOn != value)
                {
                    Impl.CoolerOn = value;
                    profile.SetValue(nameof(CoolerOn), Impl.CoolerOn.ToString());
                    RaisePropertyChanged(nameof(CoolerOn));
                }
            }
        }

        //TODO: return not implemented if !CanGetCoolerPower
        public double CoolerPower { get => Impl.CoolerPower; }

        public double ElectronsPerADU { get => Impl.ElectronsPerADU; }

        public double FullWellCapacity { get => Impl.FullWellCapacity; }

        public bool HasShutter { get => Impl.HasShutter; }

        public double HeatSinkTemperature { get => Impl.HeatSinkTemperature; }

        public object ImageArray { get => Impl.ImageArray; }

        public object ImageArrayVariant { get => Impl.ImageArrayVariant; }

        public bool ImageReady { get => Impl.ImageReady; }

        public bool IsPulseGuiding { get => Impl.IsPulseGuiding; }

        public double LastExposureDuration { get => Impl.LastExposureDuration; }

        public string LastExposureStartTime { get => Impl.LastExposureStartTime; }
  
        public int MaxADU { get => Impl.MaxADU; }

        public short MaxBinX { get => Impl.MaxBinX; }

        public short MaxBinY { get => Impl.MaxBinY; }

        public int NumX
        {
            get => Impl.NumX;
            set
            {
                if (NumX != value)
                {
                    Impl.NumX = value;
                    profile.SetValue(nameof(NumX), Impl.NumX.ToString());
                    RaisePropertyChanged(nameof(NumX));
                }
            }
        }


        public int NumY
        {
            get => Impl.NumY;
            set
            {
                if (NumY != value)
                {
                    Impl.NumY = value;
                    profile.SetValue(nameof(NumY), Impl.NumY.ToString());
                    RaisePropertyChanged(nameof(NumY));
                }
            }
        }

        public double PixelSizeX { get => Impl.PixelSizeX; }

        public double PixelSizeY { get => Impl.PixelSizeY; }

        public double SetCCDTemperature
        {
            get => Impl.SetCCDTemperature;
            set
            {
                if (SetCCDTemperature != value)
                {
                    Impl.SetCCDTemperature = value;
                    profile.SetValue(nameof(SetCCDTemperature), Impl.SetCCDTemperature.ToString());
                    RaisePropertyChanged(nameof(SetCCDTemperature));
                }
            }
        }

        public int StartX
        {
            get => Impl.StartX;
            set
            {
                if (StartX != value)
                {
                    Impl.StartX = value;
                    profile.SetValue(nameof(StartX), Impl.StartX.ToString());
                    RaisePropertyChanged(nameof(StartX));
                }
            }
        }

        public int StartY
        {
            get => Impl.StartY;
            set
            {
                if (StartY != value)
                {
                    Impl.StartY = value;
                    profile.SetValue(nameof(StartY), Impl.StartY.ToString());
                    RaisePropertyChanged(nameof(StartY));
                }
            }
        }

        public short BayerOffsetX { get => Impl.BayerOffsetX; }

        public short BayerOffsetY { get => Impl.BayerOffsetY; }

        public bool CanFastReadout { get => Impl.CanFastReadout; }

        public double ExposureMax { get => Impl.ExposureMax; }

        public double ExposureMin { get => Impl.ExposureMin; }

        public double ExposureResolution { get => Impl.ExposureResolution; }
        
        public bool FastReadout
        {
            get => Impl.FastReadout;
            set
            {
                if (FastReadout != value)
                {
                    Impl.FastReadout = value;
                    profile.SetValue(nameof(FastReadout), Impl.FastReadout.ToString());
                    RaisePropertyChanged(nameof(FastReadout));
                }
            }
        }

        public short Gain
        {
            get => Impl.Gain;
            set
            {
                if (Gain != value)
                {
                    Impl.Gain = value;
                    profile.SetValue(nameof(Gain), Impl.Gain.ToString());
                    RaisePropertyChanged(nameof(Gain));
                }
            }
        }

        public short GainMax { get => Impl.GainMax; }

        public short GainMin { get => Impl.GainMin; }

        public ArrayList Gains { get => Impl.Gains; }

        public short PercentCompleted { get => Impl.PercentCompleted; }

        public short ReadoutMode
        {
            get => Impl.ReadoutMode;
            set
            {
                if (ReadoutMode != value)
                {
                    Impl.ReadoutMode = value;
                    profile.SetValue(nameof(ReadoutMode), Impl.ReadoutMode.ToString());
                    RaisePropertyChanged(nameof(ReadoutMode));
                }
            }
        }

        public ArrayList ReadoutModes { get => Impl.ReadoutModes; }

        public string SensorName { get => Impl.SensorName; }

        public SensorType SensorType { get => Impl.SensorType; }

        public void AbortExposure()
        {
            TL.LogMessage($"Begin {nameof(AbortExposure)}()", $"");
            Impl.AbortExposure();
            TL.LogMessage($"End {nameof(AbortExposure)}", $"Result: ");
        }

        public void PulseGuide(GuideDirections Direction, int Duration)
        {
            TL.LogMessage($"Begin {nameof(PulseGuide)}({Direction},{Duration})", $"");
            Impl.PulseGuide(Direction,Duration);
            TL.LogMessage($"End {nameof(PulseGuide)}", $"Result: ");
        }

        public void StartExposure(double Duration, bool Light)
        {
            TL.LogMessage($"Begin {nameof(StartExposure)}({Duration},{Light})", $"");
            Impl.StartExposure(Duration,Light);
            TL.LogMessage($"End {nameof(StartExposure)}", $"Result: ");

        }

        public void StopExposure()
        {
            TL.LogMessage($"Begin {nameof(StopExposure)}()", $"");
            Impl.StopExposure();
            TL.LogMessage($"End {nameof(StopExposure)}", $"Result: ");
        }
    }
}
