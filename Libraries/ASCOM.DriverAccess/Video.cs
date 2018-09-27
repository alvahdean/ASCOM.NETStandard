// Decompiled with JetBrains decompiler
// Type: ASCOM.DriverAccess.Video
// Assembly: ASCOM.DriverAccess, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 4B15C8A3-EB2C-468B-8912-4BBDBC985936
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.DriverAccess\6.0.0.0__565de7938946fba7\ASCOM.DriverAccess.dll

using ASCOM.DeviceInterface;
using ASCOM.Utilities;
using System;
using System.Collections;

namespace ASCOM.DriverAccess
{
    public class Video : AscomDriver<IVideo>, IVideo
    {
        public Video(string videoId) : base(videoId) { }

        public string VideoCaptureDeviceName { get => Impl.VideoCaptureDeviceName; }

        public double ExposureMax { get => Impl.ExposureMax; }

        public double ExposureMin { get => Impl.ExposureMin; }

        public VideoCameraFrameRate FrameRate { get => Impl.FrameRate; }

        public ArrayList SupportedIntegrationRates { get => Impl.SupportedIntegrationRates; }

        public int IntegrationRate
        {
            get => Impl.IntegrationRate;
            set
            {
                if (IntegrationRate != value)
                {
                    Impl.IntegrationRate = value;
                    profile.SetValue(nameof(IntegrationRate), Impl.IntegrationRate.ToString());
                    RaisePropertyChanged(nameof(IntegrationRate));
                }
            }
        }

        public IVideoFrame LastVideoFrame { get => Impl.LastVideoFrame; }

        public string SensorName { get => Impl.SensorName; }

        public SensorType SensorType { get => Impl.SensorType; }

        public int Width { get => Impl.Width; }

        public int Height { get => Impl.Height; }

        public double PixelSizeX { get => Impl.PixelSizeX; }

        public double PixelSizeY { get => Impl.PixelSizeY; }

        public int BitDepth { get => Impl.BitDepth; }

        public string VideoCodec { get => Impl.VideoCodec; }

        public string VideoFileFormat { get => Impl.VideoFileFormat; }

        public int VideoFramesBufferSize { get => Impl.VideoFramesBufferSize; }

        public VideoCameraState CameraState { get => Impl.CameraState; }

        public short GainMax { get => Impl.GainMax; }

        public short GainMin { get => Impl.GainMin; }

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

        public ArrayList Gains { get => Impl.Gains; }

        public short GammaMax { get => Impl.GammaMax; }

        public short GammaMin { get => Impl.GammaMin; }

        public short Gamma
        {
            get => Impl.Gamma;
            set
            {
                if (Gamma != value)
                {
                    Impl.Gamma = value;
                    profile.SetValue(nameof(Gamma), Impl.Gamma.ToString());
                    RaisePropertyChanged(nameof(Gamma));
                }
            }
        }

        public ArrayList Gammas { get => Impl.Gammas; }

        public bool CanConfigureDeviceProperties { get => Impl.CanConfigureDeviceProperties; }

        public string StartRecordingVideoFile(string PreferredFileName)
        {
            TL.LogMessage($"Begin StartRecordingVideoFile(azimuth={PreferredFileName})", $"");
            var result = Impl.StartRecordingVideoFile(PreferredFileName);
            TL.LogMessage($"End StartRecordingVideoFile", $"Result: { result}");
            return result;
        }

        public void StopRecordingVideoFile()
        {
            TL.LogMessage($"Begin StopRecordingVideoFile()", $"");
            Impl.StopRecordingVideoFile();
            TL.LogMessage($"End StopRecordingVideoFile", $"");
        }

        public void ConfigureDeviceProperties()
        {
            TL.LogMessage($"Begin ConfigureDeviceProperties()", $"");
            Impl.ConfigureDeviceProperties();
            TL.LogMessage($"End ConfigureDeviceProperties", $"");
        }
    }
}
