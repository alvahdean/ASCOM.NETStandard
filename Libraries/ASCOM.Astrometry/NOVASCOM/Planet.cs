// Decompiled with JetBrains decompiler
// Type: ASCOM.Astrometry.NOVASCOM.Planet
// Assembly: ASCOM.Astrometry, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 695EF5AA-123E-4689-AAF5-64FD1324F6BB
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.Astrometry\6.0.0.0__565de7938946fba7\ASCOM.Astrometry.dll

using ASCOM.Astrometry.Exceptions;
using ASCOM.Astrometry.Kepler;
using ASCOM.Astrometry.NOVAS;
using ASCOM.Utilities.Exceptions;

using System;
using System.Runtime.InteropServices;

namespace ASCOM.Astrometry.NOVASCOM
{
  //[ComVisible(true)]
  [Obsolete("This class will be withdrawn in the next major release, please use the SOFA or NOVAS31 classes instead")]
  //[Guid("78F157E4-D03D-4efb-8248-745F9C63A850")]
  [ClassInterface(ClassInterfaceType.None)]
  public class Planet : IPlanet
  {
    private double m_deltat;
    private bool m_bDTValid;
    private BodyType m_type;
    private int m_number;
    private string m_name;
    private IEphemeris m_ephobj;
    private int[] m_ephdisps;
    private int[] m_earthephdisps;
    private IEphemeris m_earthephobj;
    private NOVAS31 Nov31;

    public double DeltaT
    {
      get
      {
        if (!this.m_bDTValid)
          throw new ValueNotAvailableException("Planet:DeltaT DeltaT is not available");
        return this.m_deltat;
      }
      set
      {
        this.m_deltat = value;
        this.m_bDTValid = true;
      }
    }

    public IEphemeris EarthEphemeris
    {
      get
      {
        return this.m_earthephobj;
      }
      set
      {
        this.m_earthephobj = value;
      }
    }

    public IEphemeris Ephemeris
    {
      get
      {
        return this.m_ephobj;
      }
      set
      {
        this.m_ephobj = value;
      }
    }

    public string Name
    {
      get
      {
        return this.m_name;
      }
      set
      {
        this.m_name = value;
      }
    }

    public int Number
    {
      get
      {
        return this.m_number;
      }
      set
      {
        if (this.m_type == BodyType.Moon & (value < 1 | value > 11))
          throw new ASCOM.Utilities.Exceptions.InvalidValueException($"Planet.Number MajorPlanet number is < 1 or > 11 - {value}");
        this.m_number = value;
      }
    }

    public BodyType Type
    {
      get
      {
        return this.m_type;
      }
      set
      {
        if (value < BodyType.Moon | value > BodyType.Comet)
          throw new ASCOM.Utilities.Exceptions.InvalidValueException($"Planet.Type BodyType is < 0 or > 2: {(int) value}");
        this.m_type = value;
      }
    }

    public Planet()
    {
      this.m_ephdisps = new int[5];
      this.m_earthephdisps = new int[5];
      this.m_name = (string) null;
      this.m_bDTValid = false;
      this.m_ephobj = (IEphemeris) new ASCOM.Astrometry.Kepler.Ephemeris();
      this.m_earthephobj = (IEphemeris) new ASCOM.Astrometry.Kepler.Ephemeris();
      this.m_earthephobj.BodyType = BodyType.Moon;
      this.m_earthephobj.Name = "Earth";
      this.m_earthephobj.Number = Body.Earth;
      this.Nov31 = new NOVAS31();
    }

    public PositionVector GetApparentPosition(double tjd)
    {
      double[] peb = new double[4];
      double[] veb = new double[4];
      double[] pes = new double[4];
      double[] ves = new double[4];
      double[] pos = new double[4];
      double[] vel = new double[4];
      double[] pos2_1 = new double[4];
      double[] pos2_2 = new double[4];
      double[] pos2_3 = new double[4];
      double[] pos2_4 = new double[4];
      double[] pos2_5 = new double[9];
      Object3 SsBody = new Object3();
      PositionVector positionVector;
      if (this.m_type == BodyType.Moon & (this.m_number == 10 | this.m_number == 11))
      {
        SsBody.Number = CommonCode.NumberToBody(this.m_number);
        SsBody.Type = ASCOM.Astrometry.ObjectType.MajorPlanetSunOrMoon;
        double Ra = 0;
        double Dec = 0;
        double Dis = 0;
        int num = (int) this.Nov31.AppPlanet(tjd, SsBody, Accuracy.Full, ref Ra, ref Dec, ref Dis);
        this.Nov31.RaDec2Vector(Ra, Dec, Dis, ref pos);
        positionVector = new PositionVector(pos[0], pos[1], pos[2], Ra, Dec, Dis, Dis / 173.14463348);
      }
      else
      {
        double tdb = 0;
        EphemerisCode.get_earth_nov(ref this.m_earthephobj, tjd, ref tdb, ref peb, ref veb, ref pes, ref ves);
        EphemerisCode.ephemeris_nov(ref this.m_ephobj, tdb, this.m_type, this.m_number, this.m_name, Origin.Barycentric, ref pos, ref vel);
        double lighttime = 0;
        NOVAS2.BaryToGeo(pos, peb, ref pos2_1, ref lighttime);
        double num1 = tdb - lighttime;
        int num2 = 0;
        double tjd1;
        do
        {
          tjd1 = num1;
          EphemerisCode.ephemeris_nov(ref this.m_ephobj, tjd1, this.m_type, this.m_number, this.m_name, Origin.Barycentric, ref pos, ref vel);
          NOVAS2.BaryToGeo(pos, peb, ref pos2_1, ref lighttime);
          num1 = tdb - lighttime;
          checked { ++num2; }
        }
        while (Math.Abs(num1 - tjd1) > 1E-06 & num2 < 100);
        int num3 = (int) NOVAS2.SunField(pos2_1, pes, ref pos2_2);
        int num4 = (int) NOVAS2.Aberration(pos2_2, veb, lighttime, ref pos2_3);
        NOVAS2.Precession(2451545.0, pos2_3, tdb, ref pos2_4);
        int num5 = (int) NOVAS2.Nutate(tdb, NutationDirection.MeanToTrue, pos2_4, ref pos2_5);
        positionVector = new PositionVector();
        positionVector.x = pos2_5[0];
        positionVector.y = pos2_5[1];
        positionVector.z = pos2_5[2];
      }
      return positionVector;
    }

    public PositionVector GetAstrometricPosition(double tjd)
    {
      double[] pos = new double[4];
      double[] vel = new double[4];
      double[] pos2 = new double[4];
      double[] peb = new double[4];
      double[] veb = new double[4];
      double[] pes = new double[4];
      double[] ves = new double[4];
      Object3 SsBody = new Object3();
      PositionVector positionVector;
      if (this.m_type == BodyType.Moon & (this.m_number == 10 | this.m_number == 11))
      {
        SsBody.Number = CommonCode.NumberToBody(this.m_number);
        SsBody.Type = ASCOM.Astrometry.ObjectType.MajorPlanetSunOrMoon;
        double Ra = 0;
        double Dec = 0;
        double Dis = 0;
        int num = (int) this.Nov31.AstroPlanet(tjd, SsBody, Accuracy.Full, ref Ra, ref Dec, ref Dis);
        this.Nov31.RaDec2Vector(Ra, Dec, Dis, ref pos);
        positionVector = new PositionVector(pos[0], pos[1], pos[2], Ra, Dec, Dis, Dis / 173.14463348);
      }
      else
      {
        double tdb = 0;
        EphemerisCode.get_earth_nov(ref this.m_earthephobj, tjd, ref tdb, ref peb, ref veb, ref pes, ref ves);
        EphemerisCode.ephemeris_nov(ref this.m_ephobj, tdb, this.m_type, this.m_number, this.m_name, Origin.Barycentric, ref pos, ref vel);
        double lighttime = 0;
        NOVAS2.BaryToGeo(pos, peb, ref pos2, ref lighttime);
        double num1 = tdb - lighttime;
        int num2 = 0;
        double tjd1;
        do
        {
          tjd1 = num1;
          EphemerisCode.ephemeris_nov(ref this.m_ephobj, tjd1, this.m_type, this.m_number, this.m_name, Origin.Barycentric, ref pos, ref vel);
          NOVAS2.BaryToGeo(pos, peb, ref pos2, ref lighttime);
          num1 = tdb - lighttime;
          checked { ++num2; }
        }
        while (Math.Abs(num1 - tjd1) > 1E-06 & num2 < 100);
        if (num2 >= 100)
          throw new HelperException("Planet:GetAstrometricPoition ephemeris_nov did not converge in 100 iterations");
        positionVector = new PositionVector();
        positionVector.x = pos2[0];
        positionVector.y = pos2[1];
        positionVector.z = pos2[2];
      }
      return positionVector;
    }

    public PositionVector GetLocalPosition(double tjd, Site site)
    {
      double[] pos2_1 = new double[4];
      double[] pos2_2 = new double[4];
      double[] earthvector1 = new double[4];
      double[] vel1 = new double[4];
      double[] earthvector2 = new double[4];
      double[] numArray1 = new double[4];
      double[] pos = new double[4];
      double[] vel2 = new double[4];
      double[] pos2_3 = new double[4];
      double[] pos2_4 = new double[4];
      double[] pos2_5 = new double[4];
      double[] numArray2 = new double[4];
      double[] peb = new double[4];
      double[] veb = new double[4];
      double[] pes = new double[4];
      double[] ves = new double[4];
      Object3 SsBody = new Object3();
      OnSurface onSurface = new OnSurface();
      if (!this.m_bDTValid)
        this.m_deltat = DeltatCode.DeltaTCalc(tjd);
      double num1 = tjd - this.m_deltat / 86400.0;
      SiteInfo locale=new SiteInfo();
      try
      {
        locale.Latitude = site.Latitude;
      }
      catch (Exception ex)
      {
        //ProjectData.SetProjectError(ex);
        throw new ValueNotAvailableException("Star:GetTopocentricPosition Site.Latitude is not available");
      }
      try
      {
        locale.Longitude = site.Longitude;
      }
      catch (Exception ex)
      {
        //ProjectData.SetProjectError(ex);
        throw new ValueNotAvailableException("Star:GetTopocentricPosition Site.Longitude is not available");
      }
      try
      {
        locale.Height = site.Height;
      }
      catch (Exception ex)
      {
        //ProjectData.SetProjectError(ex);
        throw new ValueNotAvailableException("Star:GetTopocentricPosition Site.Height is not available");
      }
      PositionVector positionVector;
      if (this.m_type == BodyType.Moon & (this.m_number == 10 | this.m_number == 11))
      {
        onSurface.Height = site.Height;
        onSurface.Latitude = site.Latitude;
        onSurface.Longitude = site.Longitude;
        onSurface.Pressure = site.Pressure;
        onSurface.Temperature = site.Temperature;
        SsBody.Number = CommonCode.NumberToBody(this.m_number);
        SsBody.Type = ASCOM.Astrometry.ObjectType.MajorPlanetSunOrMoon;
        RefractionOption RefOption = RefractionOption.NoRefraction;
        double Ra = 0;
        double Dec = 0;
        double Dis = 0;
        this.Nov31.LocalPlanet(tjd, SsBody, this.m_deltat, onSurface, Accuracy.Full, ref Ra, ref Dec, ref Dis);
        double Zd = 0;
        double Az = 0;
        double RaR = 0;
        double DecR = 0;
        this.Nov31.Equ2Hor(num1, this.m_deltat, Accuracy.Full, 0.0, 0.0, onSurface, Ra, Dec, RefOption, ref Zd, ref Az, ref RaR, ref DecR);
        this.Nov31.RaDec2Vector(RaR, DecR, Dis, ref numArray2);
        positionVector = new PositionVector(numArray2[0], numArray2[1], numArray2[2], RaR, DecR, Dis, Dis / 173.14463348, Az, 90.0 - Zd);
      }
      else
      {
        double tdb=0;
        EphemerisCode.get_earth_nov(ref this.m_earthephobj, tjd, ref tdb, ref peb, ref veb, ref pes, ref ves);
        double mobl = 0;
        double tobl = 0;
        double eq = 0;
        double dpsi = 0;
        double deps = 0;
        NOVAS2.EarthTilt(tdb, ref mobl, ref tobl, ref eq, ref dpsi, ref deps);
        double gst = 0;
        NOVAS2.SiderealTime(num1, 0.0, eq, ref gst);
        NOVAS2.Terra(ref locale, gst, ref pos, ref vel2);
        int num2 = (int) NOVAS2.Nutate(tdb, NutationDirection.TrueToMean, pos, ref pos2_3);
        NOVAS2.Precession(tdb, pos2_3, 2451545.0, ref pos2_1);
        int num3 = (int) NOVAS2.Nutate(tdb, NutationDirection.TrueToMean, vel2, ref pos2_4);
        NOVAS2.Precession(tdb, pos2_4, 2451545.0, ref pos2_2);
        int index = 0;
        do
        {
          earthvector1[index] = peb[index] + pos2_1[index];
          vel1[index] = veb[index] + pos2_2[index];
          earthvector2[index] = pes[index] + pos2_1[index];
          numArray1[index] = ves[index] + pos2_2[index];
          checked { ++index; }
        }
        while (index <= 2);
        EphemerisCode.ephemeris_nov(ref this.m_ephobj, tdb, this.m_type, this.m_number, this.m_name, Origin.Barycentric, ref pos, ref vel2);
        double lighttime = 0;
        NOVAS2.BaryToGeo(pos, earthvector1, ref pos2_3, ref lighttime);
        double num4 = tdb - lighttime;
        int num5 = 0;
        double tjd1;
        do
        {
          tjd1 = num4;
          EphemerisCode.ephemeris_nov(ref this.m_ephobj, tjd1, this.m_type, this.m_number, this.m_name, Origin.Barycentric, ref pos, ref vel2);
          NOVAS2.BaryToGeo(pos, earthvector1, ref pos2_3, ref lighttime);
          num4 = tdb - lighttime;
          checked { ++num5; }
        }
        while (Math.Abs(num4 - tjd1) > 1E-06 & num5 < 100);
        if (num5 >= 100)
          throw new HelperException("Planet:GetLocalPoition ephemeris_nov did not converge in 100 iterations");
        int num6 = (int) NOVAS2.SunField(pos2_3, earthvector2, ref pos2_5);
        int num7 = (int) NOVAS2.Aberration(pos2_5, vel1, lighttime, ref numArray2);
        positionVector = new PositionVector();
        positionVector.x = numArray2[0];
        positionVector.y = numArray2[1];
        positionVector.z = numArray2[2];
      }
      return positionVector;
    }

    public PositionVector GetTopocentricPosition(double tjd, Site site, bool Refract)
    {
      double[] pos1 = new double[4];
      double[] pos2_1 = new double[4];
      double[] pos2_2 = new double[4];
      double[] pos2_3 = new double[4];
      double[] pos2_4 = new double[4];
      double[] vel1 = new double[4];
      double[] pos2_5 = new double[4];
      double[] pos2_6 = new double[4];
      double[] pos2_7 = new double[4];
      double[] earthvector1 = new double[4];
      double[] pos2 = new double[4];
      double[] vel2 = new double[4];
      double[] earthvector2 = new double[4];
      double[] peb = new double[4];
      double[] veb = new double[4];
      double[] pes = new double[4];
      double[] ves = new double[4];
      Object3 SsBody = new Object3();
      OnSurface onSurface = new OnSurface();
      if (!this.m_bDTValid)
        this.m_deltat = DeltatCode.DeltaTCalc(tjd);
      double num1 = tjd - this.m_deltat / 86400.0;
      SiteInfo siteInfo=new SiteInfo();
      try
      {
        siteInfo.Latitude = site.Latitude;
      }
      catch (Exception ex)
      {
        //ProjectData.SetProjectError(ex);
        throw new ValueNotAvailableException("Star:GetTopocentricPosition Site.Latitude is not available");
      }
      try
      {
        siteInfo.Longitude = site.Longitude;
      }
      catch (Exception ex)
      {
        //ProjectData.SetProjectError(ex);
        throw new ValueNotAvailableException("Star:GetTopocentricPosition Site.Longitude is not available");
      }
      try
      {
        siteInfo.Height = site.Height;
      }
      catch (Exception ex)
      {
        //ProjectData.SetProjectError(ex);
        throw new ValueNotAvailableException("Star:GetTopocentricPosition Site.Height is not available");
      }
      PositionVector positionVector;
      if (this.m_type == BodyType.Moon & (this.m_number == 10 | this.m_number == 11))
      {
        onSurface.Height = site.Height;
        onSurface.Latitude = site.Latitude;
        onSurface.Longitude = site.Longitude;
        onSurface.Pressure = site.Pressure;
        onSurface.Temperature = site.Temperature;
        SsBody.Number = CommonCode.NumberToBody(this.m_number);
        SsBody.Type = ASCOM.Astrometry.ObjectType.MajorPlanetSunOrMoon;
        RefractionOption RefOption = !Refract ? RefractionOption.NoRefraction : RefractionOption.LocationRefraction;
        double Ra = 0;
        double Dec = 0;
        double Dis = 0;
        this.Nov31.TopoPlanet(tjd, SsBody, this.m_deltat, onSurface, Accuracy.Full, ref Ra, ref Dec, ref Dis);
        double Zd = 0;
        double Az = 0;
        double RaR = 0;
        double DecR = 0;
        this.Nov31.Equ2Hor(num1, this.m_deltat, Accuracy.Full, 0.0, 0.0, onSurface, Ra, Dec, RefOption, ref Zd, ref Az, ref RaR, ref DecR);
        this.Nov31.RaDec2Vector(RaR, DecR, Dis, ref pos2);
        positionVector = new PositionVector(pos2[0], pos2[1], pos2[2], RaR, DecR, Dis, Dis / 173.14463348, Az, 90.0 - Zd);
      }
      else
      {
        double tdb = 0;
        EphemerisCode.get_earth_nov(ref this.m_earthephobj, tjd, ref tdb, ref peb, ref veb, ref pes, ref ves);
        double mobl = 0;
        double tobl = 0;
        double eq = 0;
        double dpsi = 0;
        double deps = 0;
        NOVAS2.EarthTilt(tdb, ref mobl, ref tobl, ref eq, ref dpsi, ref deps);
        double gst=0;
        NOVAS2.SiderealTime(num1, 0.0, eq, ref gst);
        NOVAS2.Terra(ref siteInfo, gst, ref pos1, ref vel1);
        int num2 = (int) NOVAS2.Nutate(tdb, NutationDirection.TrueToMean, pos1, ref pos2_1);
        NOVAS2.Precession(tdb, pos2_1, 2451545.0, ref pos2_6);
        int num3 = (int) NOVAS2.Nutate(tdb, NutationDirection.TrueToMean, vel1, ref pos2_5);
        NOVAS2.Precession(tdb, pos2_5, 2451545.0, ref pos2_7);
        short num4 = 0;
        do
        {
          earthvector1[(int) num4] = peb[(int) num4] + pos2_6[(int) num4];
          vel2[(int) num4] = veb[(int) num4] + pos2_7[(int) num4];
          earthvector2[(int) num4] = pes[(int) num4] + pos2_6[(int) num4];
          ++num4;
        }
        while ((int) num4 <= 2);
        EphemerisCode.ephemeris_nov(ref this.m_ephobj, tdb, this.m_type, this.m_number, this.m_name, Origin.Barycentric, ref pos1, ref vel1);
        double lighttime=0;
        NOVAS2.BaryToGeo(pos1, earthvector1, ref pos2_1, ref lighttime);
        double num5 = tdb - lighttime;
        int num6 = 0;
        double tjd1;
        do
        {
          tjd1 = num5;
          EphemerisCode.ephemeris_nov(ref this.m_ephobj, tjd1, this.m_type, this.m_number, this.m_name, Origin.Barycentric, ref pos1, ref vel1);
          NOVAS2.BaryToGeo(pos1, earthvector1, ref pos2_1, ref lighttime);
          num5 = tdb - lighttime;
          checked { ++num6; }
        }
        while (Math.Abs(num5 - tjd1) > 1E-06 & num6 < 100);
        if (num6 >= 100)
          throw new HelperException("Planet:GetTopocentricPoition ephemeris_nov did not converge in 100 iterations");
        int num7 = (int) NOVAS2.SunField(pos2_1, earthvector2, ref pos2_2);
        int num8 = (int) NOVAS2.Aberration(pos2_2, vel2, lighttime, ref pos2_3);
        NOVAS2.Precession(2451545.0, pos2_3, tdb, ref pos2_4);
        int num9 = (int) NOVAS2.Nutate(tdb, NutationDirection.MeanToTrue, pos2_4, ref pos2);
        double ra = 0;
        double dec = 0;
        int num10 = (int) NOVAS2.Vector2RADec(pos2, ref ra, ref dec);
        double num11 = Math.Sqrt(Math.Pow(pos2[0], 2.0) + Math.Pow(pos2[1], 2.0) + Math.Pow(pos2[2], 2.0));
        RefractionOption ref_option = RefractionOption.NoRefraction;
        if (Refract)
        {
          bool flag = true;
          try
          {
            siteInfo.Temperature = site.Temperature;
          }
          catch (Exception ex)
          {
            //ProjectData.SetProjectError(ex);
            flag = false;
            //ProjectData.ClearProjectError();
          }
          try
          {
            siteInfo.Pressure = site.Pressure;
          }
          catch (Exception ex)
          {
            //ProjectData.SetProjectError(ex);
            flag = false;
            //ProjectData.ClearProjectError();
          }
          ref_option = !flag ? RefractionOption.StandardRefraction : RefractionOption.LocationRefraction;
        }
        double zd = 0;
        double az = 0;
        double rar = 0;
        double decr = 0;
        if (this.m_bDTValid)
          NOVAS2.Equ2Hor(tjd, this.m_deltat, 0.0, 0.0, ref siteInfo, ra, dec, ref_option, ref zd, ref az, ref rar, ref decr);
        else
          NOVAS2.Equ2Hor(tjd, DeltatCode.DeltaTCalc(tjd), 0.0, 0.0, ref siteInfo, ra, dec, ref_option, ref zd, ref az, ref rar, ref decr);
        if (ref_option != RefractionOption.NoRefraction)
          NOVAS2.RADec2Vector(rar, decr, num11, ref pos2);
        positionVector = new PositionVector(pos2[0], pos2[1], pos2[2], rar, decr, num11, num11 / 173.14463348, az, 90.0 - zd);
      }
      return positionVector;
    }

    public PositionVector GetVirtualPosition(double tjd)
    {
      double[] pos = new double[4];
      double[] vel = new double[4];
      double[] pos2_1 = new double[4];
      double[] pos2_2 = new double[4];
      double[] pos2_3 = new double[4];
      double[] peb = new double[4];
      double[] veb = new double[4];
      double[] pes = new double[4];
      double[] ves = new double[4];
      PositionVector positionVector = new PositionVector();
      Object3 SsBody = new Object3();
      if (this.m_type == BodyType.Moon & (this.m_number == 10 | this.m_number == 11))
      {
        SsBody.Number = CommonCode.NumberToBody(this.m_number);
        SsBody.Type = ASCOM.Astrometry.ObjectType.MajorPlanetSunOrMoon;
        double Ra=0;
        double Dec = 0;
        double Dis = 0;
        int num = (int) this.Nov31.VirtualPlanet(tjd, SsBody, Accuracy.Full, ref Ra, ref Dec, ref Dis);
        this.Nov31.RaDec2Vector(Ra, Dec, Dis, ref pos);
        positionVector = new PositionVector(pos[0], pos[1], pos[2], Ra, Dec, Dis, Dis / 173.14463348);
      }
      else
      {
        double tdb = 0;
        EphemerisCode.get_earth_nov(ref this.m_earthephobj, tjd, ref tdb, ref peb, ref veb, ref pes, ref ves);
        double mobl = 0;
        double tobl = 0;
        double eq = 0;
        double dpsi = 0;
        double deps = 0;
        NOVAS2.EarthTilt(tdb, ref mobl, ref tobl, ref eq, ref dpsi, ref deps);
        BodyType btype=BodyType.MajorPlanet;
        switch (this.m_type)
        {
          case BodyType.Moon:
            btype = BodyType.Moon;
            break;
          case BodyType.MinorPlanet:
            btype = BodyType.MinorPlanet;
            break;
          case BodyType.Comet:
            btype = BodyType.Comet;
            break;
        }
        EphemerisCode.ephemeris_nov(ref this.m_ephobj, tdb, btype, this.m_number, this.m_name, Origin.Barycentric, ref pos, ref vel);
        double lighttime=0;
        NOVAS2.BaryToGeo(pos, peb, ref pos2_1, ref lighttime);
        double num1 = tdb - lighttime;
        int num2 = 0;
        double tjd1;
        do
        {
          tjd1 = num1;
          EphemerisCode.ephemeris_nov(ref this.m_ephobj, tjd1, btype, this.m_number, this.m_name, Origin.Barycentric, ref pos, ref vel);
          NOVAS2.BaryToGeo(pos, peb, ref pos2_1, ref lighttime);
          num1 = tdb - lighttime;
          checked { ++num2; }
        }
        while (Math.Abs(num1 - tjd1) > 1E-06 & num2 < 100);
        if (num2 >= 100)
          throw new HelperException("Planet:GetVirtualPoition ephemeris_nov did not converge in 100 iterations");
        int num3 = (int) NOVAS2.SunField(pos2_1, pes, ref pos2_2);
        int num4 = (int) NOVAS2.Aberration(pos2_2, veb, lighttime, ref pos2_3);
        positionVector.x = pos2_3[0];
        positionVector.y = pos2_3[1];
        positionVector.z = pos2_3[2];
      }
      return positionVector;
    }
  }
}
