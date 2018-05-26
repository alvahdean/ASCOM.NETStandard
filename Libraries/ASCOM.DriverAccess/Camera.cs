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
  public class Camera : AscomDriver, ICameraV2
  {
    private readonly MemberFactory _memberFactory;
    private readonly short _driverInterfaceVersion;

    public short BinX
    {
      get
      {
        return Convert.ToInt16(this._memberFactory.CallMember(1, "BinX", new Type[0]));
      }
      set
      {
        this._memberFactory.CallMember(2, "BinX", new Type[0], (object) value);
      }
    }

    public short BinY
    {
      get
      {
        return Convert.ToInt16(this._memberFactory.CallMember(1, "BinY", new Type[0]));
      }
      set
      {
        this._memberFactory.CallMember(2, "BinY", new Type[0], (object) value);
      }
    }

    public double CCDTemperature
    {
      get
      {
        return Convert.ToDouble(this._memberFactory.CallMember(1, "CCDTemperature", new Type[0]));
      }
    }

    public CameraStates CameraState
    {
      get
      {
        return (CameraStates) this._memberFactory.CallMember(1, "CameraState", new Type[0]);
      }
    }

    public int CameraXSize
    {
      get
      {
        return Convert.ToInt32(this._memberFactory.CallMember(1, "CameraXSize", new Type[0]));
      }
    }

    public int CameraYSize
    {
      get
      {
        return Convert.ToInt32(this._memberFactory.CallMember(1, "CameraYSize", new Type[0]));
      }
    }

    public bool CanAbortExposure
    {
      get
      {
        return Convert.ToBoolean(this._memberFactory.CallMember(1, "CanAbortExposure", new Type[0]));
      }
    }

    public bool CanAsymmetricBin
    {
      get
      {
        return Convert.ToBoolean(this._memberFactory.CallMember(1, "CanAsymmetricBin", new Type[0]));
      }
    }

    public bool CanGetCoolerPower
    {
      get
      {
        return Convert.ToBoolean(this._memberFactory.CallMember(1, "CanGetCoolerPower", new Type[0]));
      }
    }

    public bool CanPulseGuide
    {
      get
      {
        return Convert.ToBoolean(this._memberFactory.CallMember(1, "CanPulseGuide", new Type[0]));
      }
    }

    public bool CanSetCCDTemperature
    {
      get
      {
        return Convert.ToBoolean(this._memberFactory.CallMember(1, "CanSetCCDTemperature", new Type[0]));
      }
    }

    public bool CanStopExposure
    {
      get
      {
        return Convert.ToBoolean(this._memberFactory.CallMember(1, "CanStopExposure", new Type[0]));
      }
    }

    public bool CoolerOn
    {
      get
      {
        return Convert.ToBoolean(this._memberFactory.CallMember(1, "CoolerOn", new Type[0]));
      }
      set
      {
        this._memberFactory.CallMember(2, "CoolerOn", new Type[0], (object) value);
      }
    }

    public double CoolerPower
    {
      get
      {
        return Convert.ToDouble(this._memberFactory.CallMember(1, "CoolerPower", new Type[0]));
      }
    }

    public double ElectronsPerADU
    {
      get
      {
        return Convert.ToDouble(this._memberFactory.CallMember(1, "ElectronsPerADU", new Type[0]));
      }
    }

    public double FullWellCapacity
    {
      get
      {
        return Convert.ToDouble(this._memberFactory.CallMember(1, "FullWellCapacity", new Type[0]));
      }
    }

    public bool HasShutter
    {
      get
      {
        return Convert.ToBoolean(this._memberFactory.CallMember(1, "HasShutter", new Type[0]));
      }
    }

    public double HeatSinkTemperature
    {
      get
      {
        return Convert.ToDouble(this._memberFactory.CallMember(1, "HeatSinkTemperature", new Type[0]));
      }
    }

    public object ImageArray
    {
      get
      {
        return this._memberFactory.CallMember(1, "ImageArray", new Type[0]);
      }
    }

    public object ImageArrayVariant
    {
      get
      {
        return this._memberFactory.CallMember(1, "ImageArrayVariant", new Type[0]);
      }
    }

    public bool ImageReady
    {
      get
      {
        return Convert.ToBoolean(this._memberFactory.CallMember(1, "ImageReady", new Type[0]));
      }
    }

    public bool IsPulseGuiding
    {
      get
      {
        return Convert.ToBoolean(this._memberFactory.CallMember(1, "IsPulseGuiding", new Type[0]));
      }
    }

    public double LastExposureDuration
    {
      get
      {
        return Convert.ToDouble(this._memberFactory.CallMember(1, "LastExposureDuration", new Type[0]));
      }
    }

    public string LastExposureStartTime
    {
      get
      {
        return Convert.ToString(this._memberFactory.CallMember(1, "LastExposureStartTime", new Type[0]));
      }
    }

    public int MaxADU
    {
      get
      {
        return Convert.ToInt32(this._memberFactory.CallMember(1, "MaxADU", new Type[0]));
      }
    }

    public short MaxBinX
    {
      get
      {
        return Convert.ToInt16(this._memberFactory.CallMember(1, "MaxBinX", new Type[0]));
      }
    }

    public short MaxBinY
    {
      get
      {
        return Convert.ToInt16(this._memberFactory.CallMember(1, "MaxBinY", new Type[0]));
      }
    }

    public int NumX
    {
      get
      {
        return Convert.ToInt32(this._memberFactory.CallMember(1, "NumX", new Type[0]));
      }
      set
      {
        this._memberFactory.CallMember(2, "NumX", new Type[0], (object) value);
      }
    }

    public int NumY
    {
      get
      {
        return Convert.ToInt32(this._memberFactory.CallMember(1, "NumY", new Type[0]));
      }
      set
      {
        this._memberFactory.CallMember(2, "NumY", new Type[0], (object) value);
      }
    }

    public double PixelSizeX
    {
      get
      {
        return Convert.ToDouble(this._memberFactory.CallMember(1, "PixelSizeX", new Type[0]));
      }
    }

    public double PixelSizeY
    {
      get
      {
        return Convert.ToDouble(this._memberFactory.CallMember(1, "PixelSizeY", new Type[0]));
      }
    }

    public double SetCCDTemperature
    {
      get
      {
        return Convert.ToDouble(this._memberFactory.CallMember(1, "SetCCDTemperature", new Type[0]));
      }
      set
      {
        this._memberFactory.CallMember(2, "SetCCDTemperature", new Type[0], (object) value);
      }
    }

    public int StartX
    {
      get
      {
        return Convert.ToInt32(this._memberFactory.CallMember(1, "StartX", new Type[0]));
      }
      set
      {
        this._memberFactory.CallMember(2, "StartX", new Type[0], (object) value);
      }
    }

    public int StartY
    {
      get
      {
        return Convert.ToInt32(this._memberFactory.CallMember(1, "StartY", new Type[0]));
      }
      set
      {
        this._memberFactory.CallMember(2, "StartY", new Type[0], (object) value);
      }
    }

    public short BayerOffsetX
    {
      get
      {
        return Convert.ToInt16(this._memberFactory.CallMember(1, "BayerOffsetX", new Type[0]));
      }
    }

    public short BayerOffsetY
    {
      get
      {
        return Convert.ToInt16(this._memberFactory.CallMember(1, "BayerOffsetY", new Type[0]));
      }
    }

    public bool CanFastReadout
    {
      get
      {
        if ((int) this._driverInterfaceVersion > 1)
          return Convert.ToBoolean(this._memberFactory.CallMember(1, "CanFastReadout", new Type[0]));
        return false;
      }
    }

    public double ExposureMax
    {
      get
      {
        return Convert.ToDouble(this._memberFactory.CallMember(1, "ExposureMax", new Type[0]));
      }
    }

    public double ExposureMin
    {
      get
      {
        return Convert.ToDouble(this._memberFactory.CallMember(1, "ExposureMin", new Type[0]));
      }
    }

    public double ExposureResolution
    {
      get
      {
        return Convert.ToDouble(this._memberFactory.CallMember(1, "ExposureResolution", new Type[0]));
      }
    }

    public bool FastReadout
    {
      get
      {
        return Convert.ToBoolean(this._memberFactory.CallMember(1, "FastReadout", new Type[0]));
      }
      set
      {
        this._memberFactory.CallMember(2, "FastReadout", new Type[0], (object) value);
      }
    }

    public short Gain
    {
      get
      {
        return Convert.ToInt16(this._memberFactory.CallMember(1, "Gain", new Type[0]));
      }
      set
      {
        this._memberFactory.CallMember(2, "Gain", new Type[0], (object) value);
      }
    }

    public short GainMax
    {
      get
      {
        return Convert.ToInt16(this._memberFactory.CallMember(1, "GainMax", new Type[0]));
      }
    }

    public short GainMin
    {
      get
      {
        return Convert.ToInt16(this._memberFactory.CallMember(1, "GainMin", new Type[0]));
      }
    }

    public ArrayList Gains
    {
      get
      {
        return (ArrayList) this._memberFactory.CallMember(1, "Gains", new Type[0]);
      }
    }

    public short PercentCompleted
    {
      get
      {
        return Convert.ToInt16(this._memberFactory.CallMember(1, "PercentCompleted", new Type[0]));
      }
    }

    public short ReadoutMode
    {
      get
      {
        return Convert.ToInt16(this._memberFactory.CallMember(1, "ReadoutMode", new Type[0]));
      }
      set
      {
        this._memberFactory.CallMember(2, "ReadoutMode", new Type[0], (object) value);
      }
    }

    public ArrayList ReadoutModes
    {
      get
      {
        return (ArrayList) this._memberFactory.CallMember(1, "ReadoutModes", new Type[0]);
      }
    }

    public string SensorName
    {
      get
      {
        return Convert.ToString(this._memberFactory.CallMember(1, "SensorName", new Type[0]));
      }
    }

    public SensorType SensorType
    {
      get
      {
        return (SensorType) this._memberFactory.CallMember(1, "SensorType", new Type[0]);
      }
    }

    public Camera(string cameraId)
      : base(cameraId)
    {
      this._memberFactory = this.MemberFactory;
      try
      {
        this._driverInterfaceVersion = this.InterfaceVersion;
      }
      catch (PropertyNotImplementedException ex)
      {
        this._driverInterfaceVersion = (short) 1;
      }
    }

    public static string Choose(string cameraId)
    {
      using (Chooser chooser = new Chooser())
      {
        chooser.DeviceType = "Camera";
        return chooser.Choose(cameraId);
      }
    }

    public void AbortExposure()
    {
      this._memberFactory.CallMember(3, "AbortExposure", new Type[0]);
    }

    public void PulseGuide(GuideDirections Direction, int Duration)
    {
      this._memberFactory.CallMember(3, "PulseGuide", new Type[2]
      {
        typeof (GuideDirections),
        typeof (int)
      }, (object) Direction, (object) Duration);
    }

    public void StartExposure(double Duration, bool Light)
    {
      this._memberFactory.CallMember(3, "StartExposure", new Type[2]
      {
        typeof (double),
        typeof (bool)
      }, (object) Duration, (object) Light);
    }

    public void StopExposure()
    {
      this._memberFactory.CallMember(3, "StopExposure", new Type[0]);
    }
  }
}
