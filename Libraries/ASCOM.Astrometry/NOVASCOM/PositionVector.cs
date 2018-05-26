// Decompiled with JetBrains decompiler
// Type: ASCOM.Astrometry.NOVASCOM.PositionVector
// Assembly: ASCOM.Astrometry, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 695EF5AA-123E-4689-AAF5-64FD1324F6BB
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.Astrometry\6.0.0.0__565de7938946fba7\ASCOM.Astrometry.dll

using ASCOM.Astrometry.Exceptions;
using ASCOM.Astrometry.NOVAS;

using System;
using System.Runtime.InteropServices;

namespace ASCOM.Astrometry.NOVASCOM
{
  [Obsolete("This class will be withdrawn in the next major release, please use the SOFA or NOVAS31 classes instead")]
  //[Guid("8D8B7043-49AA-40be-881F-0EC5D8E2213D")]
  //[ComVisible(true)]
  [ClassInterface(ClassInterfaceType.None)]
  public class PositionVector : IPositionVector, IPositionVectorExtra
  {
    private bool xOk;
    private bool yOk;
    private bool zOk;
    private bool RADecOk;
    private bool AzElOk;
    private double[] PosVec;
    private double m_RA;
    private double m_DEC;
    private double m_Dist;
    private double m_Light;
    private double m_Alt;
    private double m_Az;

    public double Azimuth
    {
      get
      {
        if (!this.AzElOk)
          throw new ValueNotAvailableException("PositionVector:Azimuth Azimuth is not available");
        return this.m_Az;
      }
    }

    public double Declination
    {
      get
      {
        if (!(this.xOk & this.yOk & this.zOk))
          throw new ASCOM.Astrometry.Exceptions.ValueNotSetException("PositionVector:Declination x, y or z has not been set");
        this.CheckEq();
        return this.m_DEC;
      }
    }

    public double Distance
    {
      get
      {
        if (!(this.xOk & this.yOk & this.zOk))
          throw new ASCOM.Astrometry.Exceptions.ValueNotSetException("PositionVector:Distance x, y or z has not been set");
        this.CheckEq();
        return this.m_Dist;
      }
    }

    public double Elevation
    {
      get
      {
        if (!this.AzElOk)
          throw new ValueNotAvailableException("PositionVector:Elevation Elevation is not available");
        return this.m_Alt;
      }
    }

    public double LightTime
    {
      get
      {
        if (!(this.xOk & this.yOk & this.zOk))
          throw new ASCOM.Astrometry.Exceptions.ValueNotSetException("PositionVector:LightTime x, y or z has not been set");
        this.CheckEq();
        return this.m_Light;
      }
    }

    public double RightAscension
    {
      get
      {
        if (!(this.xOk & this.yOk & this.zOk))
          throw new ASCOM.Astrometry.Exceptions.ValueNotSetException("PositionVector:RA x, y or z has not been set");
        this.CheckEq();
        return this.m_RA;
      }
    }

    public double x
    {
      get
      {
        if (!this.xOk)
          throw new ASCOM.Astrometry.Exceptions.ValueNotSetException("PositionVector:x has not been set");
        return this.PosVec[0];
      }
      set
      {
        this.PosVec[0] = value;
        this.xOk = true;
        this.RADecOk = false;
        this.AzElOk = false;
      }
    }

    public double y
    {
      get
      {
        if (!this.yOk)
          throw new ASCOM.Astrometry.Exceptions.ValueNotSetException("PositionVector:y has not been set");
        return this.PosVec[1];
      }
      set
      {
        this.PosVec[1] = value;
        this.yOk = true;
        this.RADecOk = false;
        this.AzElOk = false;
      }
    }

    public double z
    {
      get
      {
        if (!this.zOk)
          throw new ASCOM.Astrometry.Exceptions.ValueNotSetException("PositionVector:z has not been set");
        return this.PosVec[2];
      }
      set
      {
        this.PosVec[2] = value;
        this.zOk = true;
        this.RADecOk = false;
        this.AzElOk = false;
      }
    }

    public PositionVector()
    {
      this.PosVec = new double[3];
      this.xOk = false;
      this.yOk = false;
      this.zOk = false;
      this.RADecOk = false;
      this.AzElOk = false;
    }

    public PositionVector(double x, double y, double z, double RA, double DEC, double Distance, double Light, double Azimuth, double Altitude)
    {
      this.PosVec = new double[3];
      this.PosVec[0] = x;
      this.xOk = true;
      this.PosVec[1] = y;
      this.yOk = true;
      this.PosVec[2] = z;
      this.zOk = true;
      this.m_RA = RA;
      this.m_DEC = DEC;
      this.RADecOk = true;
      this.m_Dist = Distance;
      this.m_Light = Light;
      this.m_Az = Azimuth;
      this.m_Alt = Altitude;
      this.AzElOk = true;
    }

    public PositionVector(double x, double y, double z, double RA, double DEC, double Distance, double Light)
    {
      this.PosVec = new double[3];
      this.PosVec[0] = x;
      this.xOk = true;
      this.PosVec[1] = y;
      this.yOk = true;
      this.PosVec[2] = z;
      this.zOk = true;
      this.m_RA = RA;
      this.m_DEC = DEC;
      this.RADecOk = true;
      this.m_Dist = Distance;
      this.m_Light = Light;
      this.AzElOk = false;
    }

    public void Aberration(VelocityVector vel)
    {
      double[] pos = new double[3];
      double[] vel1 = new double[3];
      if (!(this.xOk & this.yOk & this.zOk))
        throw new ASCOM.Astrometry.Exceptions.ValueNotSetException("PositionVector:ProperMotion x, y or z has not been set");
      this.CheckEq();
      pos[0] = this.PosVec[0];
      pos[1] = this.PosVec[1];
      pos[2] = this.PosVec[2];
      try
      {
        vel1[0] = vel.x;
      }
      catch (Exception ex)
      {
        //ProjectData.SetProjectError(ex);
        throw new ValueNotAvailableException("PositionVector:Aberration VelocityVector.x is not available");
      }
      try
      {
        vel1[1] = vel.y;
      }
      catch (Exception ex)
      {
        //ProjectData.SetProjectError(ex);
        throw new ValueNotAvailableException("PositionVector:Aberration VelocityVector.y is not available");
      }
      try
      {
        vel1[2] = vel.z;
      }
      catch (Exception ex)
      {
        //ProjectData.SetProjectError(ex);
        throw new ValueNotAvailableException("PositionVector:Aberration VelocityVector.z is not available");
      }
      int num = (int) NOVAS2.Aberration(pos, vel1, this.m_Light, ref this.PosVec);
      this.RADecOk = false;
      this.AzElOk = false;
    }

    public void Precess(double tjd, double tjd2)
    {
      double[] pos = new double[3];
      if (!(this.xOk & this.yOk & this.zOk))
        throw new ASCOM.Astrometry.Exceptions.ValueNotSetException("PositionVector:Precess x, y or z has not been set");
      pos[0] = this.PosVec[0];
      pos[1] = this.PosVec[1];
      pos[2] = this.PosVec[2];
      NOVAS2.Precession(tjd, pos, tjd2, ref this.PosVec);
      this.RADecOk = false;
      this.AzElOk = false;
    }

    public bool ProperMotion(VelocityVector vel, double tjd1, double tjd2)
    {
      double[] pos = new double[3];
      double[] vel1 = new double[3];
      if (!(this.xOk & this.yOk & this.zOk))
        throw new ASCOM.Astrometry.Exceptions.ValueNotSetException("PositionVector:ProperMotion x, y or z has not been set");
      pos[0] = this.PosVec[0];
      pos[1] = this.PosVec[1];
      pos[2] = this.PosVec[2];
      try
      {
        vel1[0] = vel.x;
      }
      catch (Exception ex)
      {
        //ProjectData.SetProjectError(ex);
        throw new ValueNotAvailableException("PositionVector:ProperMotion VelocityVector.x is not available");
      }
      try
      {
        vel1[1] = vel.y;
      }
      catch (Exception ex)
      {
        //ProjectData.SetProjectError(ex);
        throw new ValueNotAvailableException("PositionVector:ProperMotion VelocityVector.y is not available");
      }
      try
      {
        vel1[2] = vel.z;
      }
      catch (Exception ex)
      {
        //ProjectData.SetProjectError(ex);
        throw new ValueNotAvailableException("PositionVector:ProperMotion VelocityVector.z is not available");
      }
      NOVAS2.ProperMotion(tjd1, pos, vel1, tjd2, ref this.PosVec);
      this.RADecOk = false;
      this.AzElOk = false;
      bool flag = false;
            return flag;
    }

    public bool SetFromSite(Site site, double gast)
    {
      double num1 = Math.Pow(0.99664719, 2.0);
      double latitude;
      try
      {
        latitude = site.Latitude;
      }
      catch (Exception ex)
      {
        //ProjectData.SetProjectError(ex);
        throw new ValueNotAvailableException("PositionVector:SetFromSite Site.Latitude is not available");
      }
      double num2 = Math.PI / 180.0 * latitude;
      double x1 = Math.Sin(num2);
      double x2 = Math.Cos(num2);
      double num3 = 1.0 / Math.Sqrt(Math.Pow(x2, 2.0) + num1 * Math.Pow(x1, 2.0));
      double num4 = num1 * num3;
      double height;
      try
      {
        height = site.Height;
      }
      catch (Exception ex)
      {
        //ProjectData.SetProjectError(ex);
        throw new ValueNotAvailableException("PositionVector:SetFromSite Site.Height is not available");
      }
      double num5 = height / 1000.0;
      double num6 = 6378.14 * num3 + num5;
      double num7 = 6378.14 * num4 + num5;
      double longitude;
      try
      {
        longitude = site.Longitude;
      }
      catch (Exception ex)
      {
        //ProjectData.SetProjectError(ex);
        throw new ValueNotAvailableException("PositionVector:SetFromSite Site.Height is not available");
      }
      double num8 = (gast * 15.0 + longitude) * (Math.PI / 180.0);
      double num9 = Math.Sin(num8);
      double num10 = Math.Cos(num8);
      this.PosVec[0] = num6 * x2 * num10 / 149597870.0;
      this.PosVec[1] = num6 * x2 * num9 / 149597870.0;
      this.PosVec[2] = num7 * x1 / 149597870.0;
      this.RADecOk = false;
      this.AzElOk = false;
      this.xOk = true;
      this.yOk = true;
      this.zOk = true;
      bool flag=false;
      return flag;
    }

    //[ComVisible(false)]
    public bool SetFromSiteJD(Site site, double ujd)
    {
      this.SetFromSiteJD(site, ujd, 0.0);
      bool flag=false;
      return flag;
    }

    public bool SetFromSiteJD(Site site, double ujd, double delta_t)
    {
      double tdb = delta_t == 0.0 ? ujd + DeltatCode.DeltaTCalc(ujd) : ujd + delta_t;
      double tdtjd = 0;
            double secdiff = 0;
            NOVAS2.Tdb2Tdt(tdb, ref tdtjd, ref secdiff);
      double mobl = 0;
            double tobl = 0;
            double eq = 0;
            double dpsi = 0;
            double deps = 0;
            NOVAS2.EarthTilt(tdb + secdiff / 86400.0, ref mobl, ref tobl, ref eq, ref dpsi, ref deps);
      double gst = 0;
            NOVAS2.SiderealTime(ujd, 0.0, eq, ref gst);
      this.SetFromSite(site, gst);
      bool flag = false;
            return flag;
    }

    public bool SetFromStar(Star star)
    {
      double num1;
      try
      {
        num1 = star.Parallax;
      }
      catch (Exception ex)
      {
        //ProjectData.SetProjectError(ex);
        throw new ValueNotAvailableException("PositionVector:SetFromStar Star.Parallax is not available");
      }
      if (num1 <= 0.0)
        num1 = 1E-07;
      this.m_Dist = 206264.806247096 / num1;
      try
      {
        this.m_RA = star.RightAscension;
      }
      catch (Exception ex)
      {
        //ProjectData.SetProjectError(ex);
        throw new ValueNotAvailableException("PositionVector:SetFromStar Star.RightAscension is not available");
      }
      double num2 = this.m_RA * 15.0 * (Math.PI / 180.0);
      try
      {
        this.m_DEC = star.Declination;
      }
      catch (Exception ex)
      {
        //ProjectData.SetProjectError(ex);
        throw new ValueNotAvailableException("PositionVector:SetFromStar Star.Declination is not available");
      }
      double num3 = this.m_DEC * (Math.PI / 180.0);
      double num4 = Math.Cos(num2);
      double num5 = Math.Sin(num2);
      double num6 = Math.Cos(num3);
      double num7 = Math.Sin(num3);
      this.PosVec[0] = this.m_Dist * num6 * num4;
      this.PosVec[1] = this.m_Dist * num6 * num5;
      this.PosVec[2] = this.m_Dist * num7;
      this.RADecOk = true;
      this.xOk = true;
      this.yOk = true;
      this.zOk = true;
      bool flag=false;
      return flag;
    }

    private void CheckEq()
    {
      if (this.RADecOk)
        return;
      int num = (int) NOVAS2.Vector2RADec(this.PosVec, ref this.m_RA, ref this.m_DEC);
      this.m_Dist = Math.Sqrt(Math.Pow(this.PosVec[0], 2.0) + Math.Pow(this.PosVec[1], 2.0) + Math.Pow(this.PosVec[2], 2.0));
      this.m_Light = this.m_Dist / 173.14463348;
      this.RADecOk = true;
    }
  }
}
