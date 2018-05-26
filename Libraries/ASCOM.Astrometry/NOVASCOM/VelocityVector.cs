// Decompiled with JetBrains decompiler
// Type: ASCOM.Astrometry.NOVASCOM.VelocityVector
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
  //[Guid("25F2ED0A-D0C1-403d-86B9-5F7CEBE97D87")]
  //[ComVisible(true)]
  [ClassInterface(ClassInterfaceType.None)]
  public class VelocityVector : IVelocityVector, IVelocityVectorExtra
  {
    private bool m_xv;
    private bool m_yv;
    private bool m_zv;
    private bool m_cv;
    private double[] m_v;
    private double m_VRA;
    private double m_RadVel;
    private double m_VDec;

    public double DecVelocity
    {
      get
      {
        if (!(this.m_xv & this.m_yv & this.m_zv))
          throw new ASCOM.Astrometry.Exceptions.ValueNotSetException("VelocityVector:DecVelocity x, y or z has not been set");
        this.CheckEq();
        return this.m_VDec;
      }
    }

    public double RadialVelocity
    {
      get
      {
        if (!(this.m_xv & this.m_yv & this.m_zv))
          throw new ASCOM.Astrometry.Exceptions.ValueNotSetException("VelocityVector:RadialVelocity x, y or z has not been set");
        this.CheckEq();
        return this.m_RadVel;
      }
    }

    public double RAVelocity
    {
      get
      {
        if (!(this.m_xv & this.m_yv & this.m_zv))
          throw new ASCOM.Astrometry.Exceptions.ValueNotSetException("VelocityVector:RAVelocity x, y or z has not been set");
        this.CheckEq();
        return this.m_VRA;
      }
    }

    public double x
    {
      get
      {
        if (!this.m_xv)
          throw new ASCOM.Astrometry.Exceptions.ValueNotSetException("VelocityVector:x x value has not been set");
        return this.m_v[0];
      }
      set
      {
        this.m_v[0] = value;
        this.m_xv = true;
      }
    }

    public double y
    {
      get
      {
        if (!this.m_yv)
          throw new ASCOM.Astrometry.Exceptions.ValueNotSetException("VelocityVector:y y value has not been set");
        return this.m_v[1];
      }
      set
      {
        this.m_v[1] = value;
        this.m_yv = true;
      }
    }

    public double z
    {
      get
      {
        if (!this.m_zv)
          throw new ASCOM.Astrometry.Exceptions.ValueNotSetException("VelocityVector:z z value has not been set");
        return this.m_v[2];
      }
      set
      {
        this.m_v[2] = value;
        this.m_zv = true;
      }
    }

    public VelocityVector()
    {
      this.m_v = new double[3];
      this.m_xv = false;
      this.m_yv = false;
      this.m_zv = false;
      this.m_cv = false;
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
        throw new ValueNotAvailableException("VelocityVector:SetFromSite Site.Latitude is not available");
      }
      double num2 = latitude * (Math.PI / 180.0);
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
        throw new ValueNotAvailableException("VelocityVector:SetFromSite Site.Height is not available");
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
        throw new ValueNotAvailableException("VelocityVector:SetFromSite Site.Longitude is not available");
      }
      double num8 = (gast * 15.0 + longitude) * (Math.PI / 180.0);
      double num9 = Math.Sin(num8);
      double num10 = Math.Cos(num8);
      this.m_v[0] = -7.2921151467E-05 * num6 * x2 * num9 * 86400.0 / 149597870.0;
      this.m_v[1] = 7.2921151467E-05 * num6 * x2 * num10 * 86400.0 / 149597870.0;
      this.m_v[2] = 0.0;
      this.m_xv = true;
      this.m_yv = true;
      this.m_zv = true;
      this.m_cv = false;
      return true;
    }

    //[ComVisible(false)]
    public bool SetFromSiteJD(Site site, double ujd)
    {
      return this.SetFromSiteJD(site, ujd, 0.0);
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
      return true;
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
        throw new ValueNotAvailableException("VelocityVector:SetFromStar Star.Parallax is not available");
      }
      if (num1 <= 0.0)
        num1 = 1E-07;
      double rightAscension;
      try
      {
        rightAscension = star.RightAscension;
      }
      catch (Exception ex)
      {
        //ProjectData.SetProjectError(ex);
        throw new ValueNotAvailableException("VelocityVector:SetFromStar Star.RightAscension is not available");
      }
      double declination;
      try
      {
        declination = star.Declination;
      }
      catch (Exception ex)
      {
        //ProjectData.SetProjectError(ex);
        throw new ValueNotAvailableException("VelocityVector:SetFromStar Star.Declination is not available");
      }
      double num2 = declination * (Math.PI / 180.0);
      double num3 = Math.Cos(rightAscension);
      double num4 = Math.Sin(rightAscension);
      double num5 = Math.Cos(num2);
      double num6 = Math.Sin(num2);
      double properMotionRa;
      try
      {
        properMotionRa = star.ProperMotionRA;
      }
      catch (Exception ex)
      {
        //ProjectData.SetProjectError(ex);
        throw new ValueNotAvailableException("VelocityVector:SetFromStar Star.ProperMotionRA is not available");
      }
      this.m_VRA = properMotionRa * 15.0 * num5 / (num1 * 36525.0);
      double properMotionDec;
      try
      {
        properMotionDec = star.ProperMotionDec;
      }
      catch (Exception ex)
      {
        //ProjectData.SetProjectError(ex);
        throw new ValueNotAvailableException("VelocityVector:SetFromStar Star.ProperMotionDec is not available");
      }
      this.m_VDec = properMotionDec / (num1 * 36525.0);
      double radialVelocity;
      try
      {
        radialVelocity = star.RadialVelocity;
      }
      catch (Exception ex)
      {
        //ProjectData.SetProjectError(ex);
        throw new ValueNotAvailableException("VelocityVector:SetFromStar Star.RadialVelocity is not available");
      }
      this.m_RadVel = radialVelocity * 86400.0 / 149597870.0;
      this.m_v[0] = -this.m_VRA * num4 - this.m_VDec * num6 * num3 + this.m_RadVel * num5 * num3;
      this.m_v[1] = this.m_VRA * num3 - this.m_VDec * num6 * num4 + this.m_RadVel * num5 * num4;
      this.m_v[2] = this.m_VDec * num5 + this.m_RadVel * num6;
      this.m_xv = true;
      this.m_yv = true;
      this.m_zv = true;
      this.m_cv = true;
      return true;
    }

    private void CheckEq()
    {
      if (this.m_cv)
        return;
      int num = (int) NOVAS2.Vector2RADec(this.m_v, ref this.m_VRA, ref this.m_VDec);
      this.m_RadVel = Math.Sqrt(Math.Pow(this.m_v[0], 2.0) + Math.Pow(this.m_v[1], 2.0) + Math.Pow(this.m_v[2], 2.0));
      this.m_cv = true;
    }
  }
}
