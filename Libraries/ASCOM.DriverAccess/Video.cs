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
  public class Video : AscomDriver, IVideo
  {
    private MemberFactory memberFactory;

    public string VideoCaptureDeviceName
    {
      get
      {
        return (string) this.memberFactory.CallMember(1, "VideoCaptureDeviceName", new Type[0]);
      }
    }

    public double ExposureMax
    {
      get
      {
        return (double) this.memberFactory.CallMember(1, "ExposureMax", new Type[0]);
      }
    }

    public double ExposureMin
    {
      get
      {
        return (double) this.memberFactory.CallMember(1, "ExposureMin", new Type[0]);
      }
    }

    public VideoCameraFrameRate FrameRate
    {
      get
      {
        return (VideoCameraFrameRate) this.memberFactory.CallMember(1, "FrameRate", new Type[0]);
      }
    }

    public ArrayList SupportedIntegrationRates
    {
      get
      {
        return (ArrayList) this.memberFactory.CallMember(1, "SupportedIntegrationRates", new Type[0]);
      }
    }

    public int IntegrationRate
    {
      get
      {
        return (int) this.memberFactory.CallMember(1, "IntegrationRate", new Type[0]);
      }
      set
      {
        this.memberFactory.CallMember(2, "IntegrationRate", new Type[0], (object) value);
      }
    }

    public IVideoFrame LastVideoFrame
    {
      get
      {
        return (IVideoFrame) this.memberFactory.CallMember(1, "LastVideoFrame", new Type[0]);
      }
    }

    public string SensorName
    {
      get
      {
        return (string) this.memberFactory.CallMember(1, "SensorName", new Type[0]);
      }
    }

    public SensorType SensorType
    {
      get
      {
        return (SensorType) this.memberFactory.CallMember(1, "SensorType", new Type[0]);
      }
    }

    public int Width
    {
      get
      {
        return (int) this.memberFactory.CallMember(1, "Width", new Type[0]);
      }
    }

    public int Height
    {
      get
      {
        return (int) this.memberFactory.CallMember(1, "Height", new Type[0]);
      }
    }

    public double PixelSizeX
    {
      get
      {
        return Convert.ToDouble(this.memberFactory.CallMember(1, "PixelSizeX", new Type[0]));
      }
    }

    public double PixelSizeY
    {
      get
      {
        return Convert.ToDouble(this.memberFactory.CallMember(1, "PixelSizeY", new Type[0]));
      }
    }

    public int BitDepth
    {
      get
      {
        return (int) this.memberFactory.CallMember(1, "BitDepth", new Type[0]);
      }
    }

    public string VideoCodec
    {
      get
      {
        return (string) this.memberFactory.CallMember(1, "VideoCodec", new Type[0]);
      }
    }

    public string VideoFileFormat
    {
      get
      {
        return (string) this.memberFactory.CallMember(1, "VideoFileFormat", new Type[0]);
      }
    }

    public int VideoFramesBufferSize
    {
      get
      {
        return (int) this.memberFactory.CallMember(1, "VideoFramesBufferSize", new Type[0]);
      }
    }

    public VideoCameraState CameraState
    {
      get
      {
        return (VideoCameraState) this.memberFactory.CallMember(1, "CameraState", new Type[0]);
      }
    }

    public short GainMax
    {
      get
      {
        return (short) this.memberFactory.CallMember(1, "GainMax", new Type[0]);
      }
    }

    public short GainMin
    {
      get
      {
        return (short) this.memberFactory.CallMember(1, "GainMin", new Type[0]);
      }
    }

    public short Gain
    {
      get
      {
        return (short) this.memberFactory.CallMember(1, "Gain", new Type[0]);
      }
      set
      {
        this.memberFactory.CallMember(2, "Gain", new Type[0], (object) value);
      }
    }

    public ArrayList Gains
    {
      get
      {
        return (ArrayList) this.memberFactory.CallMember(1, "Gains", new Type[0]);
      }
    }

    public short GammaMax
    {
      get
      {
        return (short) this.memberFactory.CallMember(1, "GammaMax", new Type[0]);
      }
    }

    public short GammaMin
    {
      get
      {
        return (short) this.memberFactory.CallMember(1, "GammaMin", new Type[0]);
      }
    }

    public short Gamma
    {
      get
      {
        return (short) this.memberFactory.CallMember(1, "Gamma", new Type[0]);
      }
      set
      {
        this.memberFactory.CallMember(2, "Gamma", new Type[0], (object) value);
      }
    }

    public ArrayList Gammas
    {
      get
      {
        return (ArrayList) this.memberFactory.CallMember(1, "Gammas", new Type[0]);
      }
    }

    public bool CanConfigureDeviceProperties
    {
      get
      {
        return (bool) this.memberFactory.CallMember(1, "CanConfigureDeviceProperties", new Type[0]);
      }
    }

    public Video(string videoId)
      : base(videoId)
    {
      this.memberFactory = this.MemberFactory;
    }

    public static string Choose(string videoId)
    {
      using (Chooser chooser = new Chooser())
      {
        chooser.DeviceType = "Video";
        return chooser.Choose(videoId);
      }
    }

    public string StartRecordingVideoFile(string PreferredFileName)
    {
      return (string) this.memberFactory.CallMember(3, "StartRecordingVideoFile", new Type[1]
      {
        typeof (string)
      }, (object) PreferredFileName);
    }

    public void StopRecordingVideoFile()
    {
      this.memberFactory.CallMember(3, "StopRecordingVideoFile", new Type[0]);
    }

    public void ConfigureDeviceProperties()
    {
      this.memberFactory.CallMember(3, "ConfigureDeviceProperties", new Type[0]);
    }
  }
}
