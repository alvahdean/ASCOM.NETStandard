// Decompiled with JetBrains decompiler
// Type: ASCOM.Astrometry.NOVAS.INOVAS2
// Assembly: ASCOM.Astrometry, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 695EF5AA-123E-4689-AAF5-64FD1324F6BB
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.Astrometry\6.0.0.0__565de7938946fba7\ASCOM.Astrometry.dll

using System.Runtime.InteropServices;

namespace ASCOM.Astrometry.NOVAS
{
  //[ComVisible(true)]
  //[Guid("3D201554-007C-47e6-805D-F66D1CA35543")]
  public interface INOVAS2
  {
    [DispId(1)]
    short AppStar(double tjd, ref BodyDescription earth, ref CatEntry star, ref double ra, ref double dec);

    [DispId(2)]
    short TopoStar(double tjd, ref BodyDescription earth, double deltat, ref CatEntry star, ref SiteInfo location, ref double ra, ref double dec);

    [DispId(3)]
    short AppPlanet(double tjd, ref BodyDescription ss_object, ref BodyDescription earth, ref double ra, ref double dec, ref double dis);

    [DispId(4)]
    short TopoPlanet(double tjd, ref BodyDescription ss_object, ref BodyDescription earth, double deltat, ref SiteInfo location, ref double ra, ref double dec, ref double dis);

    [DispId(5)]
    short VirtualStar(double tjd, ref BodyDescription earth, ref CatEntry star, ref double ra, ref double dec);

    [DispId(6)]
    short LocalStar(double tjd, ref BodyDescription earth, double deltat, ref CatEntry star, ref SiteInfo location, ref double ra, ref double dec);

    [DispId(7)]
    short VirtualPlanet(double tjd, ref BodyDescription ss_object, ref BodyDescription earth, ref double ra, ref double dec, ref double dis);

    [DispId(8)]
    short LocalPlanet(double tjd, ref BodyDescription ss_object, ref BodyDescription earth, double deltat, ref SiteInfo location, ref double ra, ref double dec, ref double dis);

    [DispId(9)]
    short AstroStar(double tjd, ref BodyDescription earth, ref CatEntry star, ref double ra, ref double dec);

    [DispId(10)]
    short AstroPlanet(double tjd, ref BodyDescription ss_object, ref BodyDescription earth, ref double ra, ref double dec, ref double dis);

    [DispId(11)]
    void Equ2Hor(double tjd, double deltat, double x, double y, ref SiteInfo location, double ra, double dec, RefractionOption ref_option, ref double zd, ref double az, ref double rar, ref double decr);

    [DispId(12)]
    void TransformHip(ref CatEntry hipparcos, ref CatEntry fk5);

    [DispId(13)]
    void TransformCat(TransformationOption option, double date_incat, ref CatEntry incat, double date_newcat, ref byte[] newcat_id, ref CatEntry newcat);

    [DispId(14)]
    void SiderealTime(double jd_high, double jd_low, double ee, ref double gst);

    [DispId(15)]
    void Precession(double tjd1, double[] pos, double tjd2, ref double[] pos2);

    [DispId(16)]
    void EarthTilt(double tjd, ref double mobl, ref double tobl, ref double eq, ref double dpsi, ref double deps);

    [DispId(17)]
    void CelPole(double del_dpsi, double del_deps);

    [DispId(18)]
    short Ephemeris(double tjd, ref BodyDescription cel_obj, Origin origin, ref double[] pos, ref double[] vel);

    [DispId(19)]
    short SolarSystem(double tjd, Body body, Origin origin, ref double[] pos, ref double[] vel);

    [DispId(20)]
    short Vector2RADec(double[] pos, ref double ra, ref double dec);

    [DispId(21)]
    void StarVectors(CatEntry star, ref double[] pos, ref double[] vel);

    [DispId(22)]
    void RADec2Vector(double ra, double dec, double dist, ref double[] pos);

    [DispId(23)]
    short GetEarth(double tjd, ref BodyDescription earth, ref double tdb, ref double[] bary_earthp, ref double[] bary_earthv, ref double[] helio_earthp, ref double[] helio_earthv);

    [DispId(24)]
    short MeanStar(double tjd, ref BodyDescription earth, double ra, double dec, ref double mra, ref double mdec);

    [DispId(25)]
    void Pnsw(double tjd, double gast, double x, double y, double[] vece, ref double[] vecs);

    [DispId(26)]
    void Spin(double st, double[] pos1, ref double[] pos2);

    [DispId(27)]
    void Wobble(double x, double y, double[] pos1, ref double[] pos2);

    [DispId(28)]
    void Terra(ref SiteInfo locale, double st, ref double[] pos, ref double[] vel);

    [DispId(29)]
    void ProperMotion(double tjd1, double[] pos, double[] vel, double tjd2, ref double[] pos2);

    [DispId(30)]
    void BaryToGeo(double[] pos, double[] earthvector, ref double[] pos2, ref double lighttime);

    [DispId(31)]
    short SunField(double[] pos, double[] earthvector, ref double[] pos2);

    [DispId(32)]
    short Aberration(double[] pos, double[] vel, double lighttime, ref double[] pos2);

    [DispId(33)]
    short Nutate(double tjd, NutationDirection fn, double[] pos, ref double[] pos2);

    [DispId(34)]
    short NutationAngles(double tdbtime, ref double longnutation, ref double obliqnutation);

    [DispId(35)]
    void FundArgs(double t, ref double[] a);

    [DispId(36)]
    void Tdb2Tdt(double tdb, ref double tdtjd, ref double secdiff);

    [DispId(37)]
    short SetBody(BodyType type, Body number, string name, ref BodyDescription cel_obj);

    [DispId(38)]
    void MakeCatEntry(string catalog, string star_name, int star_num, double ra, double dec, double pm_ra, double pm_dec, double parallax, double rad_vel, ref CatEntry star);

    [DispId(39)]
    double Refract(ref SiteInfo location, short ref_option, double zd_obs);

    [DispId(40)]
    double JulianDate(short year, short month, short day, double hour);

    [DispId(41)]
    void CalDate(double tjd, ref short year, ref short month, ref short day, ref double hour);

    [DispId(42)]
    void SunEph(double jd, ref double ra, ref double dec, ref double dis);

    [DispId(43)]
    double DeltaT(double Tjd);
  }
}
