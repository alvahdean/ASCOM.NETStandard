// Decompiled with JetBrains decompiler
// Type: ASCOM.Astrometry.NOVAS.NOVAS2COM
// Assembly: ASCOM.Astrometry, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 695EF5AA-123E-4689-AAF5-64FD1324F6BB
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.Astrometry\6.0.0.0__565de7938946fba7\ASCOM.Astrometry.dll

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ASCOM.Astrometry.NOVAS
{
  [ClassInterface(ClassInterfaceType.None)]
  //[ComVisible(true)]
  [Obsolete("This class will be withdrawn in the next major release, please use the SOFA or NOVAS31 classes instead")]
  //[Guid("C3F04186-CD53-40fb-8B2A-B52BE955956D")]
  public class NOVAS2COM : INOVAS2
  {
    [DebuggerNonUserCode]
    public NOVAS2COM()
    {
    }

    public short Aberration(double[] pos, double[] vel, double lighttime, ref double[] pos2)
    {
      return NOVAS2.Aberration(pos, vel, lighttime, ref pos2);
    }

    public short AppPlanet(double tjd, ref BodyDescription ss_object, ref BodyDescription earth, ref double ra, ref double dec, ref double dis)
    {
      return NOVAS2.AppPlanet(tjd, ref ss_object, ref earth, ref ra, ref dec, ref dis);
    }

    public short AppStar(double tjd, ref BodyDescription earth, ref CatEntry star, ref double ra, ref double dec)
    {
      return NOVAS2.AppStar(tjd, ref earth, ref star, ref ra, ref dec);
    }

    public short AstroPlanet(double tjd, ref BodyDescription ss_object, ref BodyDescription earth, ref double ra, ref double dec, ref double dis)
    {
      return NOVAS2.AstroPlanet(tjd, ref ss_object, ref earth, ref ra, ref dec, ref dis);
    }

    public short AstroStar(double tjd, ref BodyDescription earth, ref CatEntry star, ref double ra, ref double dec)
    {
      return NOVAS2.AstroStar(tjd, ref earth, ref star, ref ra, ref dec);
    }

    public void BaryToGeo(double[] pos, double[] earthvector, ref double[] pos2, ref double lighttime)
    {
      NOVAS2.BaryToGeo(pos, earthvector, ref pos2, ref lighttime);
    }

    public void CalDate(double tjd, ref short year, ref short month, ref short day, ref double hour)
    {
      NOVAS2.CalDate(tjd, ref year, ref month, ref day, ref hour);
    }

    public void CelPole(double del_dpsi, double del_deps)
    {
      NOVAS2.CelPole(del_dpsi, del_deps);
    }

    public void EarthTilt(double tjd, ref double mobl, ref double tobl, ref double eq, ref double dpsi, ref double deps)
    {
      NOVAS2.EarthTilt(tjd, ref mobl, ref tobl, ref eq, ref dpsi, ref deps);
    }

    public short Ephemeris(double tjd, ref BodyDescription cel_obj, Origin origin, ref double[] pos, ref double[] vel)
    {
      return NOVAS2.Ephemeris(tjd, ref cel_obj, origin, ref pos, ref vel);
    }

    public void Equ2Hor(double tjd, double deltat, double x, double y, ref SiteInfo location, double ra, double dec, RefractionOption ref_option, ref double zd, ref double az, ref double rar, ref double decr)
    {
      NOVAS2.Equ2Hor(tjd, deltat, x, y, ref location, ra, dec, ref_option, ref zd, ref az, ref rar, ref decr);
    }

    public void FundArgs(double t, ref double[] a)
    {
      NOVAS2.FundArgs(t, ref a);
    }

    public short GetEarth(double tjd, ref BodyDescription earth, ref double tdb, ref double[] bary_earthp, ref double[] bary_earthv, ref double[] helio_earthp, ref double[] helio_earthv)
    {
      return NOVAS2.GetEarth(tjd, ref earth, ref tdb, ref bary_earthp, ref bary_earthv, ref helio_earthp, ref helio_earthv);
    }

    public double JulianDate(short year, short month, short day, double hour)
    {
      return NOVAS2.JulianDate(year, month, day, hour);
    }

    public short LocalPlanet(double tjd, ref BodyDescription ss_object, ref BodyDescription earth, double deltat, ref SiteInfo location, ref double ra, ref double dec, ref double dis)
    {
      return NOVAS2.LocalPlanet(tjd, ref ss_object, ref earth, deltat, ref location, ref ra, ref dec, ref dis);
    }

    public short LocalStar(double tjd, ref BodyDescription earth, double deltat, ref CatEntry star, ref SiteInfo location, ref double ra, ref double dec)
    {
      return NOVAS2.LocalStar(tjd, ref earth, deltat, ref star, ref location, ref ra, ref dec);
    }

    public void MakeCatEntry(string catalog, string star_name, int star_num, double ra, double dec, double pm_ra, double pm_dec, double parallax, double rad_vel, ref CatEntry star)
    {
      NOVAS2.MakeCatEntry(catalog, star_name, star_num, ra, dec, pm_ra, pm_dec, parallax, rad_vel, ref star);
    }

    public short MeanStar(double tjd, ref BodyDescription earth, double ra, double dec, ref double mra, ref double mdec)
    {
      return NOVAS2.MeanStar(tjd, ref earth, ra, dec, ref mra, ref mdec);
    }

    public short Nutate(double tjd, NutationDirection fn, double[] pos, ref double[] pos2)
    {
      return NOVAS2.Nutate(tjd, fn, pos, ref pos2);
    }

    public short NutationAngles(double tdbtime, ref double longnutation, ref double obliqnutation)
    {
      return NOVAS2.NutationAngles(tdbtime, ref longnutation, ref obliqnutation);
    }

    public void Pnsw(double tjd, double gast, double x, double y, double[] vece, ref double[] vecs)
    {
      NOVAS2.Pnsw(tjd, gast, x, y, vece, ref vecs);
    }

    public void Precession(double tjd1, double[] pos, double tjd2, ref double[] pos2)
    {
      NOVAS2.Precession(tjd1, pos, tjd2, ref pos2);
    }

    public void ProperMotion(double tjd1, double[] pos, double[] vel, double tjd2, ref double[] pos2)
    {
      NOVAS2.ProperMotion(tjd1, pos, vel, tjd2, ref pos2);
    }

    public void RADec2Vector(double ra, double dec, double dist, ref double[] pos)
    {
      NOVAS2.RADec2Vector(ra, dec, dist, ref pos);
    }

    public double Refract(ref SiteInfo location, short ref_option, double zd_obs)
    {
      return NOVAS2.Refract(ref location, ref_option, zd_obs);
    }

    public short SetBody(BodyType type, Body number, string name, ref BodyDescription cel_obj)
    {
      return NOVAS2.SetBody(type, number, name, ref cel_obj);
    }

    public void SiderealTime(double jd_high, double jd_low, double ee, ref double gst)
    {
      NOVAS2.SiderealTime(jd_high, jd_low, ee, ref gst);
    }

    public short SolarSystem(double tjd, Body body, Origin origin, ref double[] pos, ref double[] vel)
    {
      return NOVAS2.SolarSystem(tjd, body, origin, ref pos, ref vel);
    }

    public void Spin(double st, double[] pos1, ref double[] pos2)
    {
      NOVAS2.Spin(st, pos1, ref pos2);
    }

    public void StarVectors(CatEntry star, ref double[] pos, ref double[] vel)
    {
      NOVAS2.StarVectors(star, ref pos, ref vel);
    }

    public void SunEph(double jd, ref double ra, ref double dec, ref double dis)
    {
      NOVAS2.SunEph(jd, ref ra, ref dec, ref dis);
    }

    public short SunField(double[] pos, double[] earthvector, ref double[] pos2)
    {
      return NOVAS2.SunField(pos, earthvector, ref pos2);
    }

    public void Tdb2Tdt(double tdb, ref double tdtjd, ref double secdiff)
    {
      NOVAS2.Tdb2Tdt(tdb, ref tdtjd, ref secdiff);
    }

    public void Terra(ref SiteInfo locale, double st, ref double[] pos, ref double[] vel)
    {
      NOVAS2.Terra(ref locale, st, ref pos, ref vel);
    }

    public short TopoPlanet(double tjd, ref BodyDescription ss_object, ref BodyDescription earth, double deltat, ref SiteInfo location, ref double ra, ref double dec, ref double dis)
    {
      return NOVAS2.TopoPlanet(tjd, ref ss_object, ref earth, deltat, ref location, ref ra, ref dec, ref dis);
    }

    public short TopoStar(double tjd, ref BodyDescription earth, double deltat, ref CatEntry star, ref SiteInfo location, ref double ra, ref double dec)
    {
      return NOVAS2.TopoStar(tjd, ref earth, deltat, ref star, ref location, ref ra, ref dec);
    }

    public void TransformCat(TransformationOption option, double date_incat, ref CatEntry incat, double date_newcat, ref byte[] newcat_id, ref CatEntry newcat)
    {
      NOVAS2.TransformCat(option, date_incat, ref incat, date_newcat, ref newcat_id, ref newcat);
    }

    public void TransformHip(ref CatEntry hipparcos, ref CatEntry fk5)
    {
      NOVAS2.TransformHip(ref hipparcos, ref fk5);
    }

    public short Vector2RADec(double[] pos, ref double ra, ref double dec)
    {
      return NOVAS2.Vector2RADec(pos, ref ra, ref dec);
    }

    public short VirtualPlanet(double tjd, ref BodyDescription ss_object, ref BodyDescription earth, ref double ra, ref double dec, ref double dis)
    {
      return NOVAS2.VirtualPlanet(tjd, ref ss_object, ref earth, ref ra, ref dec, ref dis);
    }

    public short VirtualStar(double tjd, ref BodyDescription earth, ref CatEntry star, ref double ra, ref double dec)
    {
      return NOVAS2.VirtualStar(tjd, ref earth, ref star, ref ra, ref dec);
    }

    public void Wobble(double x, double y, double[] pos1, ref double[] pos2)
    {
      NOVAS2.Wobble(x, y, pos1, ref pos2);
    }

    public double DeltaT(double Tjd)
    {
      return DeltatCode.DeltaTCalc(Tjd);
    }
  }
}
