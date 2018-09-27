// Decompiled with JetBrains decompiler
// Type: ASCOM.Astrometry.Transform.Transform
// Assembly: ASCOM.Astrometry, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 695EF5AA-123E-4689-AAF5-64FD1324F6BB
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.Astrometry\6.0.0.0__565de7938946fba7\ASCOM.Astrometry.dll

using ASCOM.Astrometry.Exceptions;
using ASCOM.Utilities;
//using Microsoft.VisualBasic;
//
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace ASCOM.Astrometry.Transform
{
  //[Guid("779CD957-5502-4939-A661-EBEE9E1F485E")]
  //[ComVisible(true)]
  //[ClassInterface(ClassInterfaceType.None)]
  public class Transform : ITransform, IDisposable
  {
    private bool disposedValue;
    private Util Utl;
    private ASCOM.Astrometry.AstroUtils.AstroUtils AstroUtl;
    private ASCOM.Astrometry.SOFA.SOFA SOFA;
    private double RAJ2000Value;
    private double RATopoValue;
    private double DECJ2000Value;
    private double DECTopoValue;
    private double SiteElevValue;
    private double SiteLatValue;
    private double SiteLongValue;
    private double SiteTempValue;
    private double RAApparentValue;
    private double DECApparentValue;
    private double AzimuthTopoValue;
    private double ElevationTopoValue;
    private double JulianDateTTValue;
    private double JulianDateUTCValue;
    private bool RefracValue;
    private bool RequiresRecalculate;
    private ASCOM.Astrometry.Transform.Transform.SetBy LastSetBy;
    private TraceLogger TL;
    private Stopwatch Sw;
    private Stopwatch SwRecalculate;
    private const double HOURS2RADIANS = 0.261799387799149;
    private const double DEGREES2RADIANS = 0.0174532925199433;
    private const double RADIANS2HOURS = 3.81971863420549;
    private const double RADIANS2DEGREES = 57.2957795130823;
    private const double TWOPI = 6.28318530717959;
    private const string DATE_FORMAT = "dd/MM/yyyy HH:mm:ss.fff";

    public double SiteLatitude
    {
      get
      {
        this.CheckSet("SiteLatitude", this.SiteLatValue, "Site latitude has not been set");
        this.TL.LogMessage("SiteLatitude Get", this.FormatDec(this.SiteLatValue));
        return this.SiteLatValue;
      }
      set
      {
        if (this.SiteLatValue != value)
          this.RequiresRecalculate = true;
        this.SiteLatValue = value;
        this.TL.LogMessage("SiteLatitude Set", this.FormatDec(value));
      }
    }

    public double SiteLongitude
    {
      get
      {
        this.CheckSet("SiteLongitude", this.SiteLongValue, "Site longitude has not been set");
        this.TL.LogMessage("SiteLongitude Get", this.FormatDec(this.SiteLongValue));
        return this.SiteLongValue;
      }
      set
      {
        if (this.SiteLongValue != value)
          this.RequiresRecalculate = true;
        this.SiteLongValue = value;
        this.TL.LogMessage("SiteLongitude Set", this.FormatDec(value));
      }
    }

    public double SiteElevation
    {
      get
      {
        this.CheckSet("SiteElevation", this.SiteElevValue, "Site elevation has not been set");
        this.TL.LogMessage("SiteElevation Get", this.SiteElevValue.ToString());
        return this.SiteElevValue;
      }
      set
      {
        if (this.SiteElevValue != value)
          this.RequiresRecalculate = true;
        this.SiteElevValue = value;
        this.TL.LogMessage("SiteElevation Set", value.ToString());
      }
    }

    public double SiteTemperature
    {
      get
      {
        this.CheckSet("SiteTemperature", this.SiteTempValue, "Site temperature has not been set");
        this.TL.LogMessage("SiteTemperature Get", this.SiteTempValue.ToString());
        return this.SiteTempValue;
      }
      set
      {
        if (this.SiteTempValue != value)
          this.RequiresRecalculate = true;
        this.SiteTempValue = value;
        this.TL.LogMessage("SiteTemperature Set", value.ToString());
      }
    }

    public bool Refraction
    {
      get
      {
        this.TL.LogMessage("Refraction Get", this.RefracValue.ToString());
        return this.RefracValue;
      }
      set
      {
        if (this.RefracValue != value)
          this.RequiresRecalculate = true;
        this.RefracValue = value;
        this.TL.LogMessage("Refraction Set", value.ToString());
      }
    }

    public double RAJ2000
    {
      get
      {
        if (this.LastSetBy == ASCOM.Astrometry.Transform.Transform.SetBy.Never)
          throw new TransformUninitialisedException("Attempt to read RAJ2000 before a SetXX method has been called");
        this.Recalculate();
        this.CheckSet("RAJ2000", this.RAJ2000Value, "RA J2000 can not be derived from the information provided. Are site parameters set?");
        this.TL.LogMessage("RAJ2000 Get", this.FormatRA(this.RAJ2000Value));
        return this.RAJ2000Value;
      }
    }

    public double DECJ2000
    {
      get
      {
        if (this.LastSetBy == ASCOM.Astrometry.Transform.Transform.SetBy.Never)
          throw new TransformUninitialisedException("Attempt to read DECJ2000 before a SetXX method has been called");
        this.Recalculate();
        this.CheckSet("DecJ2000", this.DECJ2000Value, "DEC J2000 can not be derived from the information provided. Are site parameters set?");
        this.TL.LogMessage("DecJ2000 Get", this.FormatDec(this.DECJ2000Value));
        return this.DECJ2000Value;
      }
    }

    public double RATopocentric
    {
      get
      {
        if (this.LastSetBy == ASCOM.Astrometry.Transform.Transform.SetBy.Never)
          throw new TransformUninitialisedException("Attempt to read RATopocentric before a SetXX method  has been called");
        this.Recalculate();
        this.CheckSet("RATopocentric", this.RATopoValue, "RA topocentric can not be derived from the information provided. Are site parameters set?");
        this.TL.LogMessage("RATopocentric Get", this.FormatRA(this.RATopoValue));
        return this.RATopoValue;
      }
    }

    public double DECTopocentric
    {
      get
      {
        if (this.LastSetBy == ASCOM.Astrometry.Transform.Transform.SetBy.Never)
          throw new TransformUninitialisedException("Attempt to read DECTopocentric before a SetXX method has been called");
        this.Recalculate();
        this.CheckSet("DECTopocentric", this.DECTopoValue, "DEC topocentric can not be derived from the information provided. Are site parameters set?");
        this.TL.LogMessage("DECTopocentric Get", this.FormatDec(this.DECTopoValue));
        return this.DECTopoValue;
      }
    }

    public double RAApparent
    {
      get
      {
        if (this.LastSetBy == ASCOM.Astrometry.Transform.Transform.SetBy.Never)
          throw new TransformUninitialisedException("Attempt to read DECApparent before a SetXX method has been called");
        this.Recalculate();
        this.TL.LogMessage("RAApparent Get", this.FormatRA(this.RAApparentValue));
        return this.RAApparentValue;
      }
    }

    public double DECApparent
    {
      get
      {
        if (this.LastSetBy == ASCOM.Astrometry.Transform.Transform.SetBy.Never)
          throw new TransformUninitialisedException("Attempt to read DECApparent before a SetXX method has been called");
        this.Recalculate();
        this.TL.LogMessage("DECApparent Get", this.FormatDec(this.DECApparentValue));
        return this.DECApparentValue;
      }
    }

    public double AzimuthTopocentric
    {
      get
      {
        if (this.LastSetBy == ASCOM.Astrometry.Transform.Transform.SetBy.Never)
          throw new TransformUninitialisedException("Attempt to read AzimuthTopocentric before a SetXX method has been called");
        this.RequiresRecalculate = true;
        this.Recalculate();
        this.CheckSet("AzimuthTopocentric", this.AzimuthTopoValue, "Azimuth topocentric can not be derived from the information provided. Are site parameters set?");
        this.TL.LogMessage("AzimuthTopocentric Get", this.FormatDec(this.AzimuthTopoValue));
        return this.AzimuthTopoValue;
      }
    }

    public double ElevationTopocentric
    {
      get
      {
        if (this.LastSetBy == ASCOM.Astrometry.Transform.Transform.SetBy.Never)
          throw new TransformUninitialisedException("Attempt to read ElevationTopocentric before a SetXX method has been called");
        this.RequiresRecalculate = true;
        this.Recalculate();
        this.CheckSet("ElevationTopocentric", this.ElevationTopoValue, "Elevation topocentric can not be derived from the information provided. Are site parameters set?");
        this.TL.LogMessage("ElevationTopocentric Get", this.FormatDec(this.ElevationTopoValue));
        return this.ElevationTopoValue;
      }
    }

    public double JulianDateTT
    {
      get
      {
        return this.JulianDateTTValue;
      }
      set
      {
        this.JulianDateTTValue = value;
        this.RequiresRecalculate = true;
        if (this.JulianDateTTValue != 0.0)
        {
          double tai1=0;
          double tai2=0;
          if (this.SOFA.TtTai(this.JulianDateTTValue, 0.0, ref tai1, ref tai2) != 0)
            this.TL.LogMessage("JulianDateUTC Set", "Utctai - Bad return code");
          double utc1=0;
          double utc2=0;
          if (this.SOFA.TaiUtc(tai1, tai2, ref utc1, ref utc2) != 0)
            this.TL.LogMessage("JulianDateUTC Set", "Taitt - Bad return code");
          this.JulianDateUTCValue = utc1 + utc2;
          this.TL.LogMessage("JulianDateTT Set", this.JulianDateTTValue.ToString() + " " + this.Julian2DateTime(this.JulianDateTTValue).ToString("dd/MM/yyyy HH:mm:ss.fff") + ", JDUTC: " + this.Julian2DateTime(this.JulianDateUTCValue).ToString("dd/MM/yyyy HH:mm:ss.fff"));
        }
        else
        {
          this.JulianDateUTCValue = 0.0;
          this.TL.LogMessage("JulianDateTT Set", "Calculations will now be based on PC the DateTime");
        }
      }
    }

    public double JulianDateUTC
    {
      get
      {
        return this.JulianDateUTCValue;
      }
      set
      {
        this.JulianDateUTCValue = value;
        this.RequiresRecalculate = true;
        if (this.JulianDateUTCValue != 0.0)
        {
          double tai1 = 0;
          double tai2 = 0;
          if (this.SOFA.UtcTai(this.JulianDateUTCValue, 0.0, ref tai1, ref tai2) != 0)
            this.TL.LogMessage("JulianDateUTC Set", "Utctai - Bad return code");
          double tt1=0;
          double tt2 = 0;
          if (this.SOFA.TaiTt(tai1, tai2, ref tt1, ref tt2) != 0)
            this.TL.LogMessage("JulianDateUTC Set", "Taitt - Bad return code");
          this.JulianDateTTValue = tt1 + tt2;
          this.TL.LogMessage("JulianDateUTC Set", this.JulianDateTTValue.ToString() + " " + this.Julian2DateTime(this.JulianDateUTCValue).ToString("dd/MM/yyyy HH:mm:ss.fff") + ", JDTT: " + this.Julian2DateTime(this.JulianDateTTValue).ToString("dd/MM/yyyy HH:mm:ss.fff"));
        }
        else
        {
          this.JulianDateTTValue = 0.0;
          this.TL.LogMessage("JulianDateUTC Set", "Calculations will now be based on PC the DateTime");
        }
      }
    }

    public Transform()
    {
      this.disposedValue = false;
      this.TL = new TraceLogger("", "Transform");
      this.TL.Enabled = RegistryCommonCode.GetBool("Trace Transform", false);
      this.TL.LogMessage("New", "Trace logger created OK");
      this.Utl = new Util();
      this.Sw = new Stopwatch();
      this.SwRecalculate = new Stopwatch();
      this.AstroUtl = new ASCOM.Astrometry.AstroUtils.AstroUtils();
      this.SOFA = new ASCOM.Astrometry.SOFA.SOFA();
      this.RAJ2000Value = double.NaN;
      this.DECJ2000Value = double.NaN;
      this.RATopoValue = double.NaN;
      this.DECTopoValue = double.NaN;
      this.SiteElevValue = double.NaN;
      this.SiteLatValue = double.NaN;
      this.SiteLongValue = double.NaN;
      this.RefracValue = false;
      this.LastSetBy = ASCOM.Astrometry.Transform.Transform.SetBy.Never;
      this.RequiresRecalculate = true;
      this.JulianDateTTValue = 0.0;
      this.CheckGAC();
      this.TL.LogMessage("New", "NOVAS initialised OK");
    }

    protected virtual void Dispose(bool disposing)
    {
      if (!this.disposedValue)
      {
        if (this.Utl != null)
        {
          this.Utl.Dispose();
          this.Utl = (Util) null;
        }
        if (this.AstroUtl != null)
        {
          this.AstroUtl.Dispose();
          this.AstroUtl = (ASCOM.Astrometry.AstroUtils.AstroUtils) null;
        }
        if (this.Sw != null)
        {
          this.Sw.Stop();
          this.Sw = (Stopwatch) null;
        }
        if (this.SwRecalculate != null)
        {
          this.SwRecalculate.Stop();
          this.SwRecalculate = (Stopwatch) null;
        }
      }
      this.disposedValue = true;
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    public void Refresh()
    {
      this.TL.LogMessage("Refresh", "");
      this.Recalculate();
    }

    public void SetJ2000(double RA, double DEC)
    {
      this.LastSetBy = ASCOM.Astrometry.Transform.Transform.SetBy.J2000;
      if (RA != this.RAJ2000Value | DEC != this.DECJ2000Value)
        this.RequiresRecalculate = true;
      this.RAJ2000Value = this.ValidateRA("SetJ2000", RA);
      this.DECJ2000Value = this.ValidateDec("SetJ2000", DEC);
      this.TL.LogMessage("SetJ2000", "RA: " + Strings.Format((object) RA, "") + ", DEC: " + this.FormatDec(DEC));
    }

    public void SetApparent(double RA, double DEC)
    {
      this.LastSetBy = ASCOM.Astrometry.Transform.Transform.SetBy.Apparent;
      if (RA != this.RAApparentValue | DEC != this.DECApparentValue)
        this.RequiresRecalculate = true;
      this.RAApparentValue = this.ValidateRA("SetApparent", RA);
      this.DECApparentValue = this.ValidateDec("SetApparent", DEC);
      this.TL.LogMessage("SetApparent", "RA: " + Strings.Format((object) RA, "") + ", DEC: " + this.FormatDec(DEC));
    }

    public void SetTopocentric(double RA, double DEC)
    {
      this.LastSetBy = ASCOM.Astrometry.Transform.Transform.SetBy.Topocentric;
      if (RA != this.RATopoValue | DEC != this.DECTopoValue)
        this.RequiresRecalculate = true;
      this.RATopoValue = this.ValidateRA("SetTopocentric", RA);
      this.DECTopoValue = this.ValidateDec("SetTopocentric", DEC);
      this.TL.LogMessage("SetTopocentric", "RA: " + Strings.Format((object) RA, "") + ", DEC: " + this.FormatDec(DEC));
    }

    public void SetAzimuthElevation(double Azimuth, double Elevation)
    {
      this.LastSetBy = ASCOM.Astrometry.Transform.Transform.SetBy.AzimuthElevation;
      this.RequiresRecalculate = true;
      this.AzimuthTopoValue = Azimuth;
      this.ElevationTopoValue = Elevation;
      this.TL.LogMessage("SetAzimuthElevation", "Azimuth: " + this.FormatDec(Azimuth) + ", Elevation: " + this.FormatDec(Elevation));
    }

    private void CheckSet(string Caller, double Value, string ErrMsg)
    {
      if (double.IsNaN(Value))
      {
        this.TL.LogMessage(Caller, "Throwing TransformUninitialisedException: " + ErrMsg);
        throw new TransformUninitialisedException(ErrMsg);
      }
    }

    private void J2000ToTopo()
    {
      if (double.IsNaN(this.SiteElevValue))
        throw new TransformUninitialisedException("Site elevation has not been set");
      if (double.IsNaN(this.SiteLatValue))
        throw new TransformUninitialisedException("Site latitude has not been set");
      if (double.IsNaN(this.SiteLongValue))
        throw new TransformUninitialisedException("Site longitude has not been set");
      if (double.IsNaN(this.SiteTempValue))
        throw new TransformUninitialisedException("Site temperature has not been set");
      this.Sw.Reset();
      this.Sw.Start();
      double jdutcSofa = this.GetJDUTCSofa();
      DeltatCode.DeltaTCalc(jdutcSofa);
      double dut1 = this.AstroUtl.DeltaUT(jdutcSofa);
      this.Julian2DateTime(jdutcSofa);
      this.Sw.Reset();
      this.Sw.Start();
      double aob = 0;
      double zob = 0;
      double hob = 0;
      double dob = 0;
      double rob = 0;
      double eo = 0;
      if (this.RefracValue)
        this.SOFA.CelestialToObserved(this.RAJ2000Value * (Math.PI / 12.0), this.DECJ2000Value * (Math.PI / 180.0), 0.0, 0.0, 0.0, 0.0, jdutcSofa, 0.0, dut1, this.SiteLongValue * (Math.PI / 180.0), this.SiteLatValue * (Math.PI / 180.0), this.SiteElevValue, 0.0, 0.0, 1000.0, this.SiteTempValue, 0.8, 0.57, ref aob, ref zob, ref hob, ref dob, ref rob, ref eo);
      else
        this.SOFA.CelestialToObserved(this.RAJ2000Value * (Math.PI / 12.0), this.DECJ2000Value * (Math.PI / 180.0), 0.0, 0.0, 0.0, 0.0, jdutcSofa, 0.0, dut1, this.SiteLongValue * (Math.PI / 180.0), this.SiteLatValue * (Math.PI / 180.0), this.SiteElevValue, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, ref aob, ref zob, ref hob, ref dob, ref rob, ref eo);
      this.RATopoValue = this.SOFA.Anp(rob - eo) * (12.0 / Math.PI);
      this.DECTopoValue = dob * (180.0 / Math.PI);
      this.AzimuthTopoValue = aob * (180.0 / Math.PI);
      this.ElevationTopoValue = 90.0 - zob * (180.0 / Math.PI);
      this.TL.LogMessage("  J2000 To Topo", "  Topocentric RA/DEC (including refraction if specified):  " + this.FormatRA(this.RATopoValue) + " " + this.FormatDec(this.DECTopoValue) + " Refraction: " + this.RefracValue.ToString() + ", " + Strings.FormatNumber((object) this.Sw.Elapsed.TotalMilliseconds, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) + "ms");
      this.TL.LogMessage("  J2000 To Topo", "  Azimuth/Elevation: " + this.FormatDec(this.AzimuthTopoValue) + " " + this.FormatDec(this.ElevationTopoValue) + ", " + Strings.FormatNumber((object) this.Sw.Elapsed.TotalMilliseconds, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) + "ms");
      this.TL.LogMessage("  J2000 To Topo", "  Completed");
      this.TL.BlankLine();
    }

    private void J2000ToApparent()
    {
      this.Sw.Reset();
      this.Sw.Start();
      double ri = 0;
      double di = 0;
      double eo = 0;
      this.SOFA.CelestialToIntermediate(this.RAJ2000Value * (Math.PI / 12.0), this.DECJ2000Value * (Math.PI / 180.0), 0.0, 0.0, 0.0, 0.0, this.GetJDTTSofa(), 0.0, ref ri, ref di, ref eo);
      this.RAApparentValue = this.SOFA.Anp(ri - eo) * (12.0 / Math.PI);
      this.DECApparentValue = di * (180.0 / Math.PI);
      this.TL.LogMessage("  J2000 To Apparent", "  Apparent RA/Dec:   " + this.FormatRA(this.RAApparentValue) + " " + this.FormatDec(this.DECApparentValue) + ", " + Strings.FormatNumber((object) this.Sw.Elapsed.TotalMilliseconds, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) + "ms");
    }

    private void TopoToJ2000()
    {
      if (double.IsNaN(this.SiteElevValue))
        throw new TransformUninitialisedException("Site elevation has not been set");
      if (double.IsNaN(this.SiteLatValue))
        throw new TransformUninitialisedException("Site latitude has not been set");
      if (double.IsNaN(this.SiteLongValue))
        throw new TransformUninitialisedException("Site longitude has not been set");
      if (double.IsNaN(this.SiteTempValue))
        throw new TransformUninitialisedException("Site temperature has not been set");
      this.Sw.Reset();
      this.Sw.Start();
      double jdutcSofa = this.GetJDUTCSofa();
      double jdttSofa = this.GetJDTTSofa();
      double dut1 = this.AstroUtl.DeltaUT(jdutcSofa);
      this.Sw.Reset();
      this.Sw.Start();
      double rc = 0;
            double dc = 0;
            int num = !this.RefracValue ? this.SOFA.ObservedToCelestial("R", this.SOFA.Anp(this.RATopoValue * (Math.PI / 12.0) + this.SOFA.Eo06a(jdttSofa, 0.0)), this.DECTopoValue * (Math.PI / 180.0), jdutcSofa, 0.0, dut1, this.SiteLongValue * (Math.PI / 180.0), this.SiteLatValue * (Math.PI / 180.0), 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, ref rc, ref dc) : this.SOFA.ObservedToCelestial("R", this.SOFA.Anp(this.RATopoValue * (Math.PI / 12.0) + this.SOFA.Eo06a(jdttSofa, 0.0)), this.DECTopoValue * (Math.PI / 180.0), jdutcSofa, 0.0, dut1, this.SiteLongValue * (Math.PI / 180.0), this.SiteLatValue * (Math.PI / 180.0), this.SiteElevValue, 0.0, 0.0, 1000.0, this.SiteTempValue, 0.85, 0.57, ref rc, ref dc);
      this.RAJ2000Value = rc * (12.0 / Math.PI);
      this.DECJ2000Value = dc * (180.0 / Math.PI);
      this.TL.LogMessage("  Topo To J2000", "  J2000 RA/Dec:" + this.FormatRA(this.RAJ2000Value) + " " + this.FormatDec(this.DECJ2000Value) + ", " + Strings.FormatNumber((object) this.Sw.Elapsed.TotalMilliseconds, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) + "ms");
      this.Sw.Reset();
      this.Sw.Start();
      double aob=0;
      double zob = 0;
      double hob = 0;
      double dob = 0;
      double rob = 0;
      double eo = 0;
      if (this.RefracValue)
        this.SOFA.CelestialToObserved(this.RAJ2000Value * (Math.PI / 12.0), this.DECJ2000Value * (Math.PI / 180.0), 0.0, 0.0, 0.0, 0.0, jdutcSofa, 0.0, dut1, this.SiteLongValue * (Math.PI / 180.0), this.SiteLatValue * (Math.PI / 180.0), this.SiteElevValue, 0.0, 0.0, 1000.0, this.SiteTempValue, 0.8, 0.57, ref aob, ref zob, ref hob, ref dob, ref rob, ref eo);
      else
        this.SOFA.CelestialToObserved(this.RAJ2000Value * (Math.PI / 12.0), this.DECJ2000Value * (Math.PI / 180.0), 0.0, 0.0, 0.0, 0.0, jdutcSofa, 0.0, dut1, this.SiteLongValue * (Math.PI / 180.0), this.SiteLatValue * (Math.PI / 180.0), this.SiteElevValue, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, ref aob, ref zob, ref hob, ref dob, ref rob, ref eo);
      this.AzimuthTopoValue = aob * (180.0 / Math.PI);
      this.ElevationTopoValue = 90.0 - zob * (180.0 / Math.PI);
      this.TL.LogMessage("  Topo To J2000", "  Azimuth/Elevation: " + this.FormatDec(this.AzimuthTopoValue) + " " + this.FormatDec(this.ElevationTopoValue) + ", " + Strings.FormatNumber((object) this.Sw.Elapsed.TotalMilliseconds, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) + "ms");
    }

    private void ApparentToJ2000()
    {
      this.Sw.Reset();
      this.Sw.Start();
      double rc = 0;
            double dc = 0;
            double eo = 0;
            this.SOFA.IntermediateToCelestial(this.SOFA.Anp(this.RAApparentValue * (Math.PI / 12.0) + this.SOFA.Eo06a(this.GetJDUTCSofa(), 0.0)), this.DECApparentValue * (Math.PI / 180.0), this.GetJDTTSofa(), 0.0, ref rc, ref dc, ref eo);
      this.RAJ2000Value = rc * (12.0 / Math.PI);
      this.DECJ2000Value = dc * (180.0 / Math.PI);
      this.TL.LogMessage("  Apparent To J2000", "  J2000 RA/Dec" + this.FormatRA(this.RAJ2000Value) + " " + this.FormatDec(this.DECJ2000Value) + ", " + Strings.FormatNumber((object) this.Sw.Elapsed.TotalMilliseconds, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) + "ms");
    }

    private void Recalculate()
    {
      this.SwRecalculate.Reset();
      this.SwRecalculate.Start();
      if (this.RequiresRecalculate | this.RefracValue)
      {
        this.TL.LogMessage("Recalculate", "Requires Recalculate: " + this.RequiresRecalculate.ToString() + ", Refraction: " + this.RefracValue.ToString());
        switch (this.LastSetBy)
        {
          case ASCOM.Astrometry.Transform.Transform.SetBy.J2000:
            this.TL.LogMessage("  Recalculate", "  Values last set by SetJ2000");
            if (!double.IsNaN(this.SiteLatValue) & !double.IsNaN(this.SiteLongValue) & !double.IsNaN(this.SiteElevValue) & !double.IsNaN(this.SiteTempValue))
            {
              this.J2000ToTopo();
            }
            else
            {
              this.RATopoValue = double.NaN;
              this.DECTopoValue = double.NaN;
              this.AzimuthTopoValue = double.NaN;
              this.ElevationTopoValue = double.NaN;
            }
            this.J2000ToApparent();
            break;
          case ASCOM.Astrometry.Transform.Transform.SetBy.Apparent:
            this.TL.LogMessage("  Recalculate", "  Values last set by SetApparent");
            this.ApparentToJ2000();
            if (!double.IsNaN(this.SiteLatValue) & !double.IsNaN(this.SiteLongValue) & !double.IsNaN(this.SiteElevValue) & !double.IsNaN(this.SiteTempValue))
            {
              this.J2000ToTopo();
              break;
            }
            this.RATopoValue = double.NaN;
            this.DECTopoValue = double.NaN;
            this.AzimuthTopoValue = double.NaN;
            this.ElevationTopoValue = double.NaN;
            break;
          case ASCOM.Astrometry.Transform.Transform.SetBy.Topocentric:
            this.TL.LogMessage("  Recalculate", "  Values last set by SetTopocentric");
            if (!double.IsNaN(this.SiteLatValue) & !double.IsNaN(this.SiteLongValue) & !double.IsNaN(this.SiteElevValue) & !double.IsNaN(this.SiteTempValue))
            {
              this.TopoToJ2000();
              this.J2000ToApparent();
              break;
            }
            this.RAJ2000Value = double.NaN;
            this.DECJ2000Value = double.NaN;
            this.RAApparentValue = double.NaN;
            this.DECApparentValue = double.NaN;
            this.AzimuthTopoValue = double.NaN;
            this.ElevationTopoValue = double.NaN;
            break;
          case ASCOM.Astrometry.Transform.Transform.SetBy.AzimuthElevation:
            this.TL.LogMessage("  Recalculate", "  Values last set by AzimuthElevation");
            if (!double.IsNaN(this.SiteLatValue) & !double.IsNaN(this.SiteLongValue) & !double.IsNaN(this.SiteElevValue) & !double.IsNaN(this.SiteTempValue))
            {
              this.AzElToJ2000();
              this.J2000ToTopo();
              this.J2000ToApparent();
              break;
            }
            this.RAJ2000Value = double.NaN;
            this.DECJ2000Value = double.NaN;
            this.RAApparentValue = double.NaN;
            this.DECApparentValue = double.NaN;
            this.RATopoValue = double.NaN;
            this.DECTopoValue = double.NaN;
            break;
          default:
            this.TL.LogMessage("Recalculate", "Neither SetJ2000 nor SetTopocentric nor SetApparent have been called. Throwing TransforUninitialisedException");
            throw new TransformUninitialisedException("Can't recalculate Transform object values because neither SetJ2000 nor SetTopocentric nor SetApparent have been called");
        }
        this.TL.LogMessage("  Recalculate", "  Completed in " + Strings.FormatNumber((object) this.SwRecalculate.Elapsed.TotalMilliseconds, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) + "ms");
        this.RequiresRecalculate = false;
      }
      else
        this.TL.LogMessage("  Recalculate", "No parameters have changed, refraction is " + Conversions.ToString(this.RefracValue) + ", recalculation not required");
      this.SwRecalculate.Stop();
    }

    private void AzElToJ2000()
    {
      this.Sw.Reset();
      this.Sw.Start();
      if (double.IsNaN(this.SiteElevValue))
        throw new TransformUninitialisedException("Site elevation has not been set");
      if (double.IsNaN(this.SiteLatValue))
        throw new TransformUninitialisedException("Site latitude has not been set");
      if (double.IsNaN(this.SiteLongValue))
        throw new TransformUninitialisedException("Site longitude has not been set");
      if (double.IsNaN(this.SiteTempValue))
        throw new TransformUninitialisedException("Site temperature has not been set");
      double jdutcSofa = this.GetJDUTCSofa();
      this.GetJDTTSofa();
      double dut1 = this.AstroUtl.DeltaUT(jdutcSofa);
      double rc = 0;
            double dc = 0;
            int num = !this.RefracValue ? this.SOFA.ObservedToCelestial("A", this.AzimuthTopoValue * (Math.PI / 180.0), (90.0 - this.ElevationTopoValue) * (Math.PI / 180.0), jdutcSofa, 0.0, dut1, this.SiteLongValue * (Math.PI / 180.0), this.SiteLatValue * (Math.PI / 180.0), 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, ref rc, ref dc) : this.SOFA.ObservedToCelestial("A", this.AzimuthTopoValue * (Math.PI / 180.0), (90.0 - this.ElevationTopoValue) * (Math.PI / 180.0), jdutcSofa, 0.0, dut1, this.SiteLongValue * (Math.PI / 180.0), this.SiteLatValue * (Math.PI / 180.0), this.SiteElevValue, 0.0, 0.0, 1000.0, this.SiteTempValue, 0.85, 0.57, ref rc, ref dc);
      this.RAJ2000Value = rc * (12.0 / Math.PI);
      this.DECJ2000Value = dc * (180.0 / Math.PI);
      this.TL.LogMessage("  AzEl To J2000", "  SOFA RA: " + this.FormatRA(this.RAJ2000Value) + ", Declination: " + this.FormatDec(this.DECJ2000Value));
      this.Sw.Stop();
      this.TL.BlankLine();
    }

    private double GetJDUTCSofa()
    {
      double m_JulianDate = 0;
            if (this.JulianDateUTCValue == 0.0)
      {
        DateTime utcNow = DateTime.UtcNow;
        double d1 = 0;
                double d2 = 0;
                if (this.SOFA.Dtf2d("", utcNow.Year, utcNow.Month, utcNow.Day, utcNow.Hour, utcNow.Minute, (double) utcNow.Second + (double) utcNow.Millisecond / 1000.0, ref d1, ref d2) != 0)
          this.TL.LogMessage("Dtf2d", "Bad return code");
        m_JulianDate = d1 + d2;
      }
      else
        m_JulianDate = this.JulianDateUTCValue;
      this.TL.LogMessage("  GetJDUTCSofa", "  " + m_JulianDate.ToString() + " " + this.Julian2DateTime(m_JulianDate).ToString("dd/MM/yyyy HH:mm:ss.fff"));
      return m_JulianDate;
    }

    private double GetJDTTSofa()
    {
      double m_JulianDate;
      if (this.JulianDateTTValue == 0.0)
      {
        DateTime utcNow = DateTime.UtcNow;
        double d1 = 0;
                double d2 = 0;
                if (this.SOFA.Dtf2d("", utcNow.Year, utcNow.Month, utcNow.Day, utcNow.Hour, utcNow.Minute, (double) utcNow.Second + (double) utcNow.Millisecond / 1000.0, ref d1, ref d2) != 0)
          this.TL.LogMessage("Dtf2d", "Bad return code");
        double tai1 = 0;
                double tai2 = 0;
                if (this.SOFA.UtcTai(d1, d2, ref tai1, ref tai2) != 0)
          this.TL.LogMessage("GetJDTTSofa", "Utctai - Bad return code");
        double tt1 = 0;
                double tt2 = 0;
                if (this.SOFA.TaiTt(tai1, tai2, ref tt1, ref tt2) != 0)
          this.TL.LogMessage("GetJDTTSofa", "Taitt - Bad return code");
        m_JulianDate = tt1 + tt2;
      }
      else
        m_JulianDate = this.JulianDateTTValue;
      this.TL.LogMessage("  GetJDTTSofa", "  " + m_JulianDate.ToString() + " " + this.Julian2DateTime(m_JulianDate).ToString("dd/MM/yyyy HH:mm:ss.fff"));
      return m_JulianDate;
    }

    private void CheckGAC()
    {
      this.TL.LogMessage("CheckGAC", "Started");
      this.TL.LogMessage("CheckGAC", "Assembly path: " + Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
    }

    private double ValidateRA(string Caller, double RA)
    {
      if (RA < 0.0 | RA >= 24.0)
        throw new InvalidValueException(Caller, RA.ToString(), "0 to 23.9999");
      return RA;
    }

    private double ValidateDec(string Caller, double Dec)
    {
      if (Dec < -90.0 | Dec > 90.0)
        throw new InvalidValueException(Caller, Dec.ToString(), "-90.0 to 90.0");
      return Dec;
    }

    private DateTime Julian2DateTime(double m_JulianDate)
    {
      bool flag = false;
      DateTime dateTime;
      try
      {
        if (m_JulianDate > 2378507.5)
        {
          long num1 = checked ((long) Math.Round(Math.Floor(m_JulianDate)));
          double num2 = m_JulianDate - Math.Floor(m_JulianDate);
          if (flag)
            this.TL.LogMessage("ConvertFromJulian", "Initial: " + Conversions.ToString(num1) + " " + Conversions.ToString(num2));
          long num3 = checked (num1 + 68569L);
          long num4 = checked (4L * num3) / 146097L;
          long num5 = checked (num3 - unchecked (checked (146097L * num4 + 3L) / 4L));
          long num6 = checked (4000L * num5 + 1L) / 1461001L;
          long num7 = checked (num5 - unchecked (checked (1461L * num6) / 4L) + 31L);
          long num8 = checked (80L * num7) / 2447L;
          int day = checked ((int) (num7 - unchecked (checked (2447L * num8) / 80L)));
          long num9 = num8 / 11L;
          int month = checked ((int) (num8 + 2L - 12L * num9));
          int year = checked ((int) (100L * (num4 - 49L) + num6 + num9));
          if (flag)
            this.TL.LogMessage("ConvertFromJulian", "DMY: " + Conversions.ToString(day) + " " + Conversions.ToString(month) + " " + Conversions.ToString(year));
          double num10 = num2 + 5.78703703703704E-09;
          double num11;
          if (num10 >= 0.5)
          {
            if (flag)
              this.TL.LogMessage("ConvertFromJulian", "JDFraction >= 0.5: " + Conversions.ToString(num10));
            checked { ++day; }
            num11 = num10 - 0.5;
            if (flag)
              this.TL.LogMessage("ConvertFromJulian", "DMY: " + Conversions.ToString(day) + " " + Conversions.ToString(num11));
          }
          else
            num11 = num10 + 0.5;
          int hour = checked ((int) Math.Round(Conversion.Int(unchecked (num11 * 24.0))));
          double num12 = num11 * 24.0 - (double) hour;
          if (flag)
            this.TL.LogMessage("ConvertFromJulian", "Hours: " + Conversions.ToString(hour) + " " + Conversions.ToString(num12));
          int minute = checked ((int) Math.Round(Conversion.Int(unchecked (num12 * 60.0))));
          double num13 = num12 * 60.0 - (double) minute;
          if (flag)
            this.TL.LogMessage("ConvertFromJulian", "Minutes: " + Conversions.ToString(minute) + " " + Conversions.ToString(num13));
          int second = checked ((int) Math.Round(Conversion.Int(unchecked (num13 * 60.0))));
          double num14 = num13 * 60.0 - (double) second;
          if (flag)
            this.TL.LogMessage("ConvertFromJulian", "Seconds: " + Conversions.ToString(second) + " " + Conversions.ToString(num14));
          int millisecond = checked ((int) Math.Round(Conversion.Int(unchecked (num14 * 1000.0))));
          if (flag)
            this.TL.LogMessage("ConvertFromJulian", Conversions.ToString(num1) + " " + Conversions.ToString(num11) + " " + Conversions.ToString(day) + " " + Conversions.ToString(hour) + " " + Conversions.ToString(minute) + " " + Conversions.ToString(second) + " " + Conversions.ToString(millisecond));
          dateTime = new DateTime(year, month, day, hour, minute, second, millisecond);
        }
        else
          dateTime = new DateTime(1800, 1, 10);
      }
      catch (Exception ex)
      {
        //ProjectData.SetProjectError(ex);
        this.TL.LogMessageCrLf("", "Exception: " + ex.ToString());
        dateTime = new DateTime(1900, 1, 10);
        //ProjectData.ClearProjectError();
      }
      return dateTime;
    }

    private string FormatRA(double RA)
    {
      return this.Utl.HoursToHMS(RA, ":", ":", "", 3);
    }

    private string FormatDec(double Dec)
    {
      return this.Utl.DegreesToDMS(Dec, ":", ":", "", 3);
    }

    private enum SetBy
    {
      Never,
      J2000,
      Apparent,
      Topocentric,
      AzimuthElevation,
      Refresh,
    }
  }
}
