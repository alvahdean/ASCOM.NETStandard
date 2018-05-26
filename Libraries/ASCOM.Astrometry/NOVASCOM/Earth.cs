// Decompiled with JetBrains decompiler
// Type: ASCOM.Astrometry.NOVASCOM.Earth
// Assembly: ASCOM.Astrometry, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 695EF5AA-123E-4689-AAF5-64FD1324F6BB
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.Astrometry\6.0.0.0__565de7938946fba7\ASCOM.Astrometry.dll

using ASCOM.Astrometry.Kepler;
using ASCOM.Astrometry.NOVAS;
using System;
using System.Runtime.InteropServices;

namespace ASCOM.Astrometry.NOVASCOM
{
  //[Guid("6BD93BA2-79C5-4077-9630-B7C6E30B2FDF")]
  [ClassInterface(ClassInterfaceType.None)]
  [Obsolete("This class will be withdrawn in the next major release, please use the SOFA or NOVAS31 classes instead")]
  //[ComVisible(true)]
  public class Earth : IEarth
  {
    private PositionVector m_BaryPos;
    private PositionVector m_HeliPos;
    private VelocityVector m_BaryVel;
    private VelocityVector m_HeliVel;
    private double m_BaryTime;
    private double m_MeanOb;
    private double m_EquOfEqu;
    private double m_NutLong;
    private double m_NutObl;
    private double m_TrueOb;
    private IEphemeris m_EarthEph;
    private bool m_Valid;

    public PositionVector BarycentricPosition
    {
      get
      {
        return this.m_BaryPos;
      }
    }

    public double BarycentricTime
    {
      get
      {
        return this.m_BaryTime;
      }
    }

    public VelocityVector BarycentricVelocity
    {
      get
      {
        return this.m_BaryVel;
      }
    }

    public IEphemeris EarthEphemeris
    {
      get
      {
        return this.m_EarthEph;
      }
      set
      {
        this.m_EarthEph = value;
      }
    }

    public double EquationOfEquinoxes
    {
      get
      {
        return this.m_EquOfEqu;
      }
    }

    public PositionVector HeliocentricPosition
    {
      get
      {
        return this.m_HeliPos;
      }
    }

    public VelocityVector HeliocentricVelocity
    {
      get
      {
        return this.m_HeliVel;
      }
    }

    public double MeanObliquity
    {
      get
      {
        return this.m_MeanOb;
      }
    }

    public double NutationInLongitude
    {
      get
      {
        return this.m_NutLong;
      }
    }

    public double NutationInObliquity
    {
      get
      {
        return this.m_NutObl;
      }
    }

    public double TrueObliquity
    {
      get
      {
        return this.m_TrueOb;
      }
    }

    public Earth()
    {
      this.m_BaryPos = new PositionVector();
      this.m_HeliPos = new PositionVector();
      this.m_BaryVel = new VelocityVector();
      this.m_HeliVel = new VelocityVector();
      this.m_EarthEph = (IEphemeris) new Ephemeris();
      this.m_EarthEph.BodyType = BodyType.Moon;
      this.m_EarthEph.Number = Body.Earth;
      this.m_EarthEph.Name = "Earth";
      this.m_Valid = false;
    }

    public bool SetForTime(double tjd)
    {
      double[] peb = new double[3];
      double[] veb = new double[3];
      double[] pes = new double[3];
      double[] ves = new double[3];
      EphemerisCode.get_earth_nov(ref this.m_EarthEph, tjd, ref this.m_BaryTime, ref peb, ref veb, ref pes, ref ves);
      NOVAS2.EarthTilt(tjd, ref this.m_MeanOb, ref this.m_TrueOb, ref this.m_EquOfEqu, ref this.m_NutLong, ref this.m_NutObl);
      this.m_BaryPos.x = peb[0];
      this.m_BaryPos.y = peb[1];
      this.m_BaryPos.z = peb[2];
      this.m_BaryVel.x = veb[0];
      this.m_BaryVel.y = veb[1];
      this.m_BaryVel.z = veb[2];
      this.m_HeliPos.x = pes[0];
      this.m_HeliPos.y = pes[1];
      this.m_HeliPos.z = pes[2];
      this.m_HeliVel.x = ves[0];
      this.m_HeliVel.y = ves[1];
      this.m_HeliVel.z = ves[2];
      this.m_Valid = true;
      return this.m_Valid;
    }
  }
}
