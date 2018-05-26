// Decompiled with JetBrains decompiler
// Type: ASCOM.Astrometry.NOVASCOM.Site
// Assembly: ASCOM.Astrometry, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 695EF5AA-123E-4689-AAF5-64FD1324F6BB
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.Astrometry\6.0.0.0__565de7938946fba7\ASCOM.Astrometry.dll

using System;
using System.Runtime.InteropServices;

namespace ASCOM.Astrometry.NOVASCOM
{
  [ClassInterface(ClassInterfaceType.None)]
  //[Guid("46ACFBCE-4EEE-496d-A4B6-7A5FDDD8F969")]
  [Obsolete("This class will be withdrawn in the next major release, please use the SOFA or NOVAS31 classes instead")]
  //[ComVisible(true)]
  public class Site : ISite
  {
    private double vHeight;
    private double vLatitude;
    private double vLongitude;
    private double vPressure;
    private double vTemperature;
    private bool HeightValid;
    private bool LatitudeValid;
    private bool LongitudeValid;
    private bool PressureValid;
    private bool TemperatureValid;

    public double Height
    {
      get
      {
        if (!this.HeightValid)
          throw new ASCOM.Astrometry.Exceptions.ValueNotSetException("Height has not yet been set");
        return this.vHeight;
      }
      set
      {
        this.vHeight = value;
        this.HeightValid = true;
      }
    }

    public double Latitude
    {
      get
      {
        if (!this.LatitudeValid)
          throw new ASCOM.Astrometry.Exceptions.ValueNotSetException("Latitude has not yet been set");
        return this.vLatitude;
      }
      set
      {
        this.vLatitude = value;
        this.LatitudeValid = true;
      }
    }

    public double Longitude
    {
      get
      {
        if (!this.LongitudeValid)
          throw new ASCOM.Astrometry.Exceptions.ValueNotSetException("Longitude has not yet been set");
        return this.vLongitude;
      }
      set
      {
        this.vLongitude = value;
        this.LongitudeValid = true;
      }
    }

    public double Pressure
    {
      get
      {
        if (!this.PressureValid)
          throw new ASCOM.Astrometry.Exceptions.ValueNotSetException("Pressure has not yet been set");
        return this.vPressure;
      }
      set
      {
        this.vPressure = value;
        this.PressureValid = true;
      }
    }

    public double Temperature
    {
      get
      {
        if (!this.TemperatureValid)
          throw new ASCOM.Astrometry.Exceptions.ValueNotSetException("Temperature has not yet been set");
        return this.vTemperature;
      }
      set
      {
        this.vTemperature = value;
        this.TemperatureValid = true;
      }
    }

    public Site()
    {
      this.HeightValid = false;
      this.LatitudeValid = false;
      this.LongitudeValid = false;
      this.PressureValid = false;
      this.TemperatureValid = false;
    }

    public void Set(double Latitude, double Longitude, double Height)
    {
      this.vLatitude = Latitude;
      this.vLongitude = Longitude;
      this.vHeight = Height;
      this.LatitudeValid = true;
      this.LongitudeValid = true;
      this.HeightValid = true;
    }
  }
}
