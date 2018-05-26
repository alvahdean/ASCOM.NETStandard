// Decompiled with JetBrains decompiler
// Type: ASCOM.Astrometry.Kepler.Ephemeris
// Assembly: ASCOM.Astrometry, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 695EF5AA-123E-4689-AAF5-64FD1324F6BB
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.Astrometry\6.0.0.0__565de7938946fba7\ASCOM.Astrometry.dll

using ASCOM.Astrometry.Exceptions;

using System;
using System.Runtime.InteropServices;

namespace ASCOM.Astrometry.Kepler
{
  [Obsolete("This class will be withdrawn in the next major release, please use the SOFA or NOVAS31 classes instead")]
  //[ComVisible(true)]
  [ClassInterface(ClassInterfaceType.None)]
  //[Guid("2F2B0413-1F83-4777-B3B4-38DE3C32DC6B")]
  public class Ephemeris : IEphemeris
  {
    private const double DTVEL = 0.01;
    private string m_Name;
    private Body m_Number;
    private bool m_bNumberValid;
    private BodyType m_Type;
    private bool m_bTypeValid;
    private KeplerGlobalCode.orbit m_e;
    private double[,] ss;
    private double[,] cc;
    private double[] Args;
    private double LP_equinox;
    private double NF_arcsec;
    private double Ea_arcsec;
    private double pA_precession;

    public double a
    {
      get
      {
        return this.m_e.a;
      }
      set
      {
        this.m_e.a = value;
      }
    }

    public BodyType BodyType
    {
      get
      {
        if (!this.m_bTypeValid)
          throw new ASCOM.Astrometry.Exceptions.ValueNotSetException("KEPLER:BodyType BodyType has not been set");
        return this.m_Type;
      }
      set
      {
        this.m_Type = value;
        this.m_bTypeValid = true;
      }
    }

    public double e
    {
      get
      {
        return this.m_e.ecc;
      }
      set
      {
        this.m_e.ecc = value;
      }
    }

    public double Epoch
    {
      get
      {
        return this.m_e.epoch;
      }
      set
      {
        this.m_e.epoch = value;
      }
    }

    public double G
    {
      get
      {
        throw new ValueNotAvailableException("Kepler:G Read - Magnitude slope parameter calculation not implemented");
      }
      set
      {
        throw new ValueNotAvailableException("Kepler:G Write - Magnitude slope parameter calculation not implemented");
      }
    }

    public double H
    {
      get
      {
        throw new ValueNotAvailableException("Kepler:H Read - Visual magnitude calculation not implemented");
      }
      set
      {
        throw new ValueNotAvailableException("Kepler:H Write - Visual magnitude calculation not implemented");
      }
    }

    public double Incl
    {
      get
      {
        return this.m_e.i;
      }
      set
      {
        this.m_e.i = value;
      }
    }

    public double M
    {
      get
      {
        return this.m_e.M;
      }
      set
      {
        this.m_e.M = value;
      }
    }

    public double n
    {
      get
      {
        return this.m_e.dm;
      }
      set
      {
        this.m_e.dm = value;
      }
    }

    public string Name
    {
      get
      {
        if (this.m_Name=="")
          throw new ASCOM.Astrometry.Exceptions.ValueNotSetException("KEPLER:Name Name has not been set");
        return this.m_Name;
      }
      set
      {
        this.m_Name = value;
      }
    }

    public double Node
    {
      get
      {
        return this.m_e.W;
      }
      set
      {
        this.m_e.W = value;
      }
    }

    public Body Number
    {
      get
      {
        if (!this.m_bNumberValid)
          throw new ASCOM.Astrometry.Exceptions.ValueNotSetException("KEPLER:Number Planet number has not been set");
        return this.m_Number;
      }
      set
      {
        this.m_Number = value;
      }
    }

    public double P
    {
      get
      {
        throw new ValueNotAvailableException("Kepler:P Read - Orbital period calculation not implemented");
      }
      set
      {
        throw new ValueNotAvailableException("Kepler:P Write - Orbital period calculation not implemented");
      }
    }

    public double Peri
    {
      get
      {
        return this.m_e.wp;
      }
      set
      {
        this.m_e.wp = value;
      }
    }

    public double q
    {
      get
      {
        return this.m_e.a;
      }
      set
      {
        this.m_e.a = value;
      }
    }

    public double z
    {
      get
      {
        return 1.0 / this.m_e.a;
      }
      set
      {
        this.m_e.a = 1.0 / value;
      }
    }

    public Ephemeris()
    {
      this.ss = new double[19, 32];
      this.cc = new double[19, 32];
      this.Args = new double[19];
      this.m_bTypeValid = false;
      this.m_Name = "";
      this.m_Type = BodyType.Moon;
      this.m_e.ptable.lon_tbl = new double[1]{ 0.0 };
      this.m_e.ptable.lat_tbl = new double[1]{ 0.0 };
    }

    public double[] GetPositionAndVelocity(double tjd)
    {
      double[] numArray1 = new double[6];
      int[] numArray2 = new int[2];
      double[,] numArray3 = new double[4, 4];
      KeplerGlobalCode.orbit e = new KeplerGlobalCode.orbit();
      double[] rect = new double[3];
      if (!this.m_bTypeValid)
        throw new ASCOM.Astrometry.Exceptions.ValueNotSetException("Kepler:GetPositionAndVelocity Body type has not been set");
      switch (this.m_Type)
      {
        case BodyType.Moon:
          switch (this.m_Number)
          {
            case Body.Mercury:
              e = KeplerGlobalCode.mercury;
              break;
            case Body.Venus:
              e = KeplerGlobalCode.venus;
              break;
            case Body.Earth:
              e = KeplerGlobalCode.earthplanet;
              break;
            case Body.Mars:
              e = KeplerGlobalCode.mars;
              break;
            case Body.Jupiter:
              e = KeplerGlobalCode.jupiter;
              break;
            case Body.Saturn:
              e = KeplerGlobalCode.saturn;
              break;
            case Body.Uranus:
              e = KeplerGlobalCode.uranus;
              break;
            case Body.Neptune:
              e = KeplerGlobalCode.neptune;
              break;
            case Body.Pluto:
              e = KeplerGlobalCode.pluto;
              break;
            default:
              throw new ASCOM.Utilities.Exceptions.InvalidValueException($"Kepler:GetPositionAndVelocity Invalid value for planet number: {(int)this.m_Number}");
          }
                    break;
        case BodyType.MinorPlanet:
          e = this.m_e;
          break;
        case BodyType.Comet:
          e = this.m_e;
          break;
      }
      int index1 = 0;
      do
      {
        KeplerGlobalCode.KeplerCalc(tjd + (double) checked (index1 - 1) * 0.01, ref e, ref rect);
        numArray3[index1, 0] = rect[0];
        numArray3[index1, 1] = rect[1];
        numArray3[index1, 2] = rect[2];
        checked { ++index1; }
      }
      while (index1 <= 2);
      int index2 = 0;
      do
      {
        numArray1[index2] = numArray3[1, index2];
        numArray1[checked (3 + index2)] = (numArray3[2, index2] - numArray3[0, index2]) / 0.02;
        checked { ++index2; }
      }
      while (index2 <= 2);
      return numArray1;
    }
  }
}
