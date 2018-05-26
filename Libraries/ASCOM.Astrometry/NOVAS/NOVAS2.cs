// Decompiled with JetBrains decompiler
// Type: ASCOM.Astrometry.NOVAS.NOVAS2
// Assembly: ASCOM.Astrometry, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 695EF5AA-123E-4689-AAF5-64FD1324F6BB
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.Astrometry\6.0.0.0__565de7938946fba7\ASCOM.Astrometry.dll


using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace ASCOM.Astrometry.NOVAS
{
  //[ComVisible(false)]
  [Obsolete("This class will be withdrawn in the next major release, please use the SOFA or NOVAS31 classes instead")]
  public class NOVAS2
  {
    private const string NOVAS32Dll = "NOVAS-C.dll";
    private const string NOVAS64Dll = "NOVAS-C64.dll";
    private const string NOVAS_DLL_LOCATION = "\\ASCOM\\Astrometry";
    private const int CSIDL_PROGRAM_FILES = 38;
    private const int CSIDL_PROGRAM_FILESX86 = 42;
    private const int CSIDL_WINDOWS = 36;
    private const int CSIDL_PROGRAM_FILES_COMMONX86 = 44;
    [SpecialName]
    private static string _Is64Bit__002__StatPath;

    [DebuggerNonUserCode]
    public NOVAS2()
    {
    }

    public static short AppStar(double tjd, ref BodyDescription earth, ref CatEntry star, ref double ra, ref double dec)
    {
      if (NOVAS2.Is64Bit())
      {
        double tjd1 = tjd;
        NOVAS2.BodyDescriptionShort earth1 = NOVAS2.BodyDescToShort(earth);
        NOVAS2.CatEntryNOVAS2 catEntryNovaS2 = NOVAS2.CatEntryToCatEntryNOVAS2(star);
        return NOVAS2.app_star64(tjd1, ref earth1, ref catEntryNovaS2, ref ra, ref dec);
      }
      double tjd2 = tjd;
      NOVAS2.BodyDescriptionShort earth2 = NOVAS2.BodyDescToShort(earth);
      NOVAS2.CatEntryNOVAS2 catEntryNovaS2_1 = NOVAS2.CatEntryToCatEntryNOVAS2(star);
      return NOVAS2.app_star32(tjd2, ref earth2, ref catEntryNovaS2_1, ref ra, ref dec);
    }

    public static short TopoStar(double tjd, ref BodyDescription earth, double deltat, ref CatEntry star, ref SiteInfo location, ref double ra, ref double dec)
    {
      if (NOVAS2.Is64Bit())
      {
        double tjd1 = tjd;
        NOVAS2.BodyDescriptionShort earth1 = NOVAS2.BodyDescToShort(earth);
        double deltat1 = deltat;
        NOVAS2.CatEntryNOVAS2 catEntryNovaS2 = NOVAS2.CatEntryToCatEntryNOVAS2(star);
        return NOVAS2.topo_star64(tjd1, ref earth1, deltat1, ref catEntryNovaS2, ref location, ref ra, ref dec);
      }
      double tjd2 = tjd;
      NOVAS2.BodyDescriptionShort earth2 = NOVAS2.BodyDescToShort(earth);
      double deltat2 = deltat;
      NOVAS2.CatEntryNOVAS2 catEntryNovaS2_1 = NOVAS2.CatEntryToCatEntryNOVAS2(star);
      return NOVAS2.topo_star32(tjd2, ref earth2, deltat2, ref catEntryNovaS2_1, ref location, ref ra, ref dec);
    }

    public static short AppPlanet(double tjd, ref BodyDescription ss_object, ref BodyDescription earth, ref double ra, ref double dec, ref double dis)
    {
      if (NOVAS2.Is64Bit())
      {
        double tjd1 = tjd;
        NOVAS2.BodyDescriptionShort ss_object1 = NOVAS2.BodyDescToShort(ss_object);
        NOVAS2.BodyDescriptionShort earth1 = NOVAS2.BodyDescToShort(earth);
        return NOVAS2.app_planet64(tjd1, ref ss_object1, ref earth1, ref ra, ref dec, ref dis);
      }
      double tjd2 = tjd;
      NOVAS2.BodyDescriptionShort ss_object2 = NOVAS2.BodyDescToShort(ss_object);
      NOVAS2.BodyDescriptionShort earth2 = NOVAS2.BodyDescToShort(earth);
      return NOVAS2.app_planet32(tjd2, ref ss_object2, ref earth2, ref ra, ref dec, ref dis);
    }

    public static short TopoPlanet(double tjd, ref BodyDescription ss_object, ref BodyDescription earth, double deltat, ref SiteInfo location, ref double ra, ref double dec, ref double dis)
    {
      if (NOVAS2.Is64Bit())
      {
        double tjd1 = tjd;
        NOVAS2.BodyDescriptionShort ss_object1 = NOVAS2.BodyDescToShort(ss_object);
        NOVAS2.BodyDescriptionShort earth1 = NOVAS2.BodyDescToShort(earth);
        double deltat1 = deltat;
        return NOVAS2.topo_planet64(tjd1, ref ss_object1, ref earth1, deltat1, ref location, ref ra, ref dec, ref dis);
      }
      double tjd2 = tjd;
      NOVAS2.BodyDescriptionShort ss_object2 = NOVAS2.BodyDescToShort(ss_object);
      NOVAS2.BodyDescriptionShort earth2 = NOVAS2.BodyDescToShort(earth);
      double deltat2 = deltat;
      return NOVAS2.topo_planet32(tjd2, ref ss_object2, ref earth2, deltat2, ref location, ref ra, ref dec, ref dis);
    }

    public static short VirtualStar(double tjd, ref BodyDescription earth, ref CatEntry star, ref double ra, ref double dec)
    {
      if (NOVAS2.Is64Bit())
      {
        double tjd1 = tjd;
        NOVAS2.BodyDescriptionShort earth1 = NOVAS2.BodyDescToShort(earth);
        NOVAS2.CatEntryNOVAS2 catEntryNovaS2 = NOVAS2.CatEntryToCatEntryNOVAS2(star);
        return NOVAS2.virtual_star64(tjd1, ref earth1, ref catEntryNovaS2, ref ra, ref dec);
      }
      double tjd2 = tjd;
      NOVAS2.BodyDescriptionShort earth2 = NOVAS2.BodyDescToShort(earth);
      NOVAS2.CatEntryNOVAS2 catEntryNovaS2_1 = NOVAS2.CatEntryToCatEntryNOVAS2(star);
      return NOVAS2.virtual_star32(tjd2, ref earth2, ref catEntryNovaS2_1, ref ra, ref dec);
    }

    public static short LocalStar(double tjd, ref BodyDescription earth, double deltat, ref CatEntry star, ref SiteInfo location, ref double ra, ref double dec)
    {
      if (NOVAS2.Is64Bit())
      {
        double tjd1 = tjd;
        NOVAS2.BodyDescriptionShort earth1 = NOVAS2.BodyDescToShort(earth);
        double deltat1 = deltat;
        NOVAS2.CatEntryNOVAS2 catEntryNovaS2 = NOVAS2.CatEntryToCatEntryNOVAS2(star);
        return NOVAS2.local_star64(tjd1, ref earth1, deltat1, ref catEntryNovaS2, ref location, ref ra, ref dec);
      }
      double tjd2 = tjd;
      NOVAS2.BodyDescriptionShort earth2 = NOVAS2.BodyDescToShort(earth);
      double deltat2 = deltat;
      NOVAS2.CatEntryNOVAS2 catEntryNovaS2_1 = NOVAS2.CatEntryToCatEntryNOVAS2(star);
      return NOVAS2.local_star32(tjd2, ref earth2, deltat2, ref catEntryNovaS2_1, ref location, ref ra, ref dec);
    }

    public static short VirtualPlanet(double tjd, ref BodyDescription ss_object, ref BodyDescription earth, ref double ra, ref double dec, ref double dis)
    {
      if (NOVAS2.Is64Bit())
      {
        double tjd1 = tjd;
        NOVAS2.BodyDescriptionShort ss_object1 = NOVAS2.BodyDescToShort(ss_object);
        NOVAS2.BodyDescriptionShort earth1 = NOVAS2.BodyDescToShort(earth);
        return NOVAS2.virtual_planet64(tjd1, ref ss_object1, ref earth1, ref ra, ref dec, ref dis);
      }
      double tjd2 = tjd;
      NOVAS2.BodyDescriptionShort ss_object2 = NOVAS2.BodyDescToShort(ss_object);
      NOVAS2.BodyDescriptionShort earth2 = NOVAS2.BodyDescToShort(earth);
      return NOVAS2.virtual_planet32(tjd2, ref ss_object2, ref earth2, ref ra, ref dec, ref dis);
    }

    public static short LocalPlanet(double tjd, ref BodyDescription ss_object, ref BodyDescription earth, double deltat, ref SiteInfo location, ref double ra, ref double dec, ref double dis)
    {
      if (NOVAS2.Is64Bit())
      {
        double tjd1 = tjd;
        NOVAS2.BodyDescriptionShort ss_object1 = NOVAS2.BodyDescToShort(ss_object);
        NOVAS2.BodyDescriptionShort earth1 = NOVAS2.BodyDescToShort(earth);
        double deltat1 = deltat;
        return NOVAS2.local_planet64(tjd1, ref ss_object1, ref earth1, deltat1, ref location, ref ra, ref dec, ref dis);
      }
      double tjd2 = tjd;
      NOVAS2.BodyDescriptionShort ss_object2 = NOVAS2.BodyDescToShort(ss_object);
      NOVAS2.BodyDescriptionShort earth2 = NOVAS2.BodyDescToShort(earth);
      double deltat2 = deltat;
      return NOVAS2.local_planet32(tjd2, ref ss_object2, ref earth2, deltat2, ref location, ref ra, ref dec, ref dis);
    }

    public static short AstroStar(double tjd, ref BodyDescription earth, ref CatEntry star, ref double ra, ref double dec)
    {
      if (NOVAS2.Is64Bit())
      {
        double tjd1 = tjd;
        NOVAS2.BodyDescriptionShort earth1 = NOVAS2.BodyDescToShort(earth);
        NOVAS2.CatEntryNOVAS2 catEntryNovaS2 = NOVAS2.CatEntryToCatEntryNOVAS2(star);
        return NOVAS2.astro_star64(tjd1, ref earth1, ref catEntryNovaS2, ref ra, ref dec);
      }
      double tjd2 = tjd;
      NOVAS2.BodyDescriptionShort earth2 = NOVAS2.BodyDescToShort(earth);
      NOVAS2.CatEntryNOVAS2 catEntryNovaS2_1 = NOVAS2.CatEntryToCatEntryNOVAS2(star);
      return NOVAS2.astro_star32(tjd2, ref earth2, ref catEntryNovaS2_1, ref ra, ref dec);
    }

    public static short AstroPlanet(double tjd, ref BodyDescription ss_object, ref BodyDescription earth, ref double ra, ref double dec, ref double dis)
    {
      if (NOVAS2.Is64Bit())
      {
        double tjd1 = tjd;
        NOVAS2.BodyDescriptionShort ss_object1 = NOVAS2.BodyDescToShort(ss_object);
        NOVAS2.BodyDescriptionShort earth1 = NOVAS2.BodyDescToShort(earth);
        return NOVAS2.astro_planet64(tjd1, ref ss_object1, ref earth1, ref ra, ref dec, ref dis);
      }
      double tjd2 = tjd;
      NOVAS2.BodyDescriptionShort ss_object2 = NOVAS2.BodyDescToShort(ss_object);
      NOVAS2.BodyDescriptionShort earth2 = NOVAS2.BodyDescToShort(earth);
      return NOVAS2.astro_planet32(tjd2, ref ss_object2, ref earth2, ref ra, ref dec, ref dis);
    }

    public static void Equ2Hor(double tjd, double deltat, double x, double y, ref SiteInfo location, double ra, double dec, RefractionOption ref_option, ref double zd, ref double az, ref double rar, ref double decr)
    {
      if (NOVAS2.Is64Bit())
        NOVAS2.equ2hor64(tjd, deltat, x, y, ref location, ra, dec, checked ((short) ref_option), ref zd, ref az, ref rar, ref decr);
      else
        NOVAS2.equ2hor32(tjd, deltat, x, y, ref location, ra, dec, checked ((short) ref_option), ref zd, ref az, ref rar, ref decr);
    }

    public static void TransformHip(ref CatEntry hipparcos, ref CatEntry fk5)
    {
      NOVAS2.CatEntryNOVAS2 fk5_1 = new NOVAS2.CatEntryNOVAS2();
      if (NOVAS2.Is64Bit())
      {
        NOVAS2.CatEntryNOVAS2 catEntryNovaS2 = NOVAS2.CatEntryToCatEntryNOVAS2(hipparcos);
        NOVAS2.transform_hip64(ref catEntryNovaS2, ref fk5_1);
      }
      else
      {
        NOVAS2.CatEntryNOVAS2 catEntryNovaS2 = NOVAS2.CatEntryToCatEntryNOVAS2(hipparcos);
        NOVAS2.transform_hip32(ref catEntryNovaS2, ref fk5_1);
      }
      NOVAS2.CatEntryNOVAS2ToCatEntry(fk5_1, ref fk5);
    }

    public static void TransformCat(TransformationOption option, double date_incat, ref CatEntry incat, double date_newcat, ref byte[] newcat_id, ref CatEntry newcat)
    {
      NOVAS2.CatEntryNOVAS2 newcat1 = new NOVAS2.CatEntryNOVAS2();
      if (NOVAS2.Is64Bit())
      {
        int num = (int) checked ((short) option);
        double date_incat1 = date_incat;
        NOVAS2.CatEntryNOVAS2 catEntryNovaS2 = NOVAS2.CatEntryToCatEntryNOVAS2(incat);
        double date_newcat1 = date_newcat;
        NOVAS2.transform_cat64((short) num, date_incat1, ref catEntryNovaS2, date_newcat1, ref newcat_id, ref newcat1);
      }
      else
      {
        int num = (int) checked ((short) option);
        double date_incat1 = date_incat;
        NOVAS2.CatEntryNOVAS2 catEntryNovaS2 = NOVAS2.CatEntryToCatEntryNOVAS2(incat);
        double date_newcat1 = date_newcat;
        NOVAS2.transform_cat32((short) num, date_incat1, ref catEntryNovaS2, date_newcat1, ref newcat_id, ref newcat1);
      }
      NOVAS2.CatEntryNOVAS2ToCatEntry(newcat1, ref newcat);
    }

    public static void SiderealTime(double jd_high, double jd_low, double ee, ref double gst)
    {
      if (NOVAS2.Is64Bit())
        NOVAS2.sidereal_time64(jd_high, jd_low, ee, ref gst);
      else
        NOVAS2.sidereal_time32(jd_high, jd_low, ee, ref gst);
    }

    public static void Precession(double tjd1, double[] pos, double tjd2, ref double[] pos2)
    {
      PosVector pos2_1 = new PosVector();
      if (NOVAS2.Is64Bit())
      {
        double tjd1_1 = tjd1;
        PosVector posVec = NOVAS2.ArrToPosVec(pos);
        double tjd2_1 = tjd2;
        NOVAS2.precession64(tjd1_1, ref posVec, tjd2_1, ref pos2_1);
      }
      else
      {
        double tjd1_1 = tjd1;
        PosVector posVec = NOVAS2.ArrToPosVec(pos);
        double tjd2_1 = tjd2;
        NOVAS2.precession32(tjd1_1, ref posVec, tjd2_1, ref pos2_1);
      }
      NOVAS2.PosVecToArr(pos2_1, ref pos2);
    }

    public static void EarthTilt(double tjd, ref double mobl, ref double tobl, ref double eq, ref double dpsi, ref double deps)
    {
      if (NOVAS2.Is64Bit())
        NOVAS2.earthtilt64(tjd, ref mobl, ref tobl, ref eq, ref dpsi, ref deps);
      else
        NOVAS2.earthtilt32(tjd, ref mobl, ref tobl, ref eq, ref dpsi, ref deps);
    }

    public static void CelPole(double del_dpsi, double del_deps)
    {
      if (NOVAS2.Is64Bit())
        NOVAS2.cel_pole64(del_dpsi, del_deps);
      else
        NOVAS2.cel_pole32(del_dpsi, del_deps);
    }

    public static short Ephemeris(double tjd, ref BodyDescription cel_obj, Origin origin, ref double[] pos, ref double[] vel)
    {
      PosVector pos1 = new PosVector();
      VelVector vel1 = new VelVector();
      short num1;
      if (NOVAS2.Is64Bit())
      {
        double tjd1 = tjd;
        NOVAS2.BodyDescriptionShort cel_obj1 = NOVAS2.BodyDescToShort(cel_obj);
        int num2 = (int) checked ((short) origin);
        num1 = NOVAS2.ephemeris64(tjd1, ref cel_obj1, (short) num2, ref pos1, ref vel1);
      }
      else
      {
        double tjd1 = tjd;
        NOVAS2.BodyDescriptionShort cel_obj1 = NOVAS2.BodyDescToShort(cel_obj);
        int num2 = (int) checked ((short) origin);
        num1 = NOVAS2.ephemeris32(tjd1, ref cel_obj1, (short) num2, ref pos1, ref vel1);
      }
      NOVAS2.PosVecToArr(pos1, ref pos);
      NOVAS2.VelVecToArr(vel1, ref vel);
      return num1;
    }

    public static short SolarSystem(double tjd, Body body, Origin origin, ref double[] pos, ref double[] vel)
    {
      PosVector pos1 = new PosVector();
      VelVector vel1 = new VelVector();
      short num = !NOVAS2.Is64Bit() ? NOVAS2.solarsystem32(tjd, checked ((short) body), checked ((short) origin), ref pos1, ref vel1) : NOVAS2.solarsystem64(tjd, checked ((short) body), checked ((short) origin), ref pos1, ref vel1);
      NOVAS2.PosVecToArr(pos1, ref pos);
      NOVAS2.VelVecToArr(vel1, ref vel);
      return num;
    }

    public static short Vector2RADec(double[] pos, ref double ra, ref double dec)
    {
      short num;
      if (NOVAS2.Is64Bit())
      {
        PosVector posVec = NOVAS2.ArrToPosVec(pos);
        num = NOVAS2.vector2radec64(ref posVec, ref ra, ref dec);
      }
      else
      {
        PosVector posVec = NOVAS2.ArrToPosVec(pos);
        num = NOVAS2.vector2radec32(ref posVec, ref ra, ref dec);
      }
      return num;
    }

    public static void StarVectors(CatEntry star, ref double[] pos, ref double[] vel)
    {
      PosVector pos1 = new PosVector();
      VelVector vel1 = new VelVector();
      if (NOVAS2.Is64Bit())
      {
        NOVAS2.CatEntryNOVAS2 catEntryNovaS2 = NOVAS2.CatEntryToCatEntryNOVAS2(star);
        NOVAS2.starvectors64(ref catEntryNovaS2, ref pos1, ref vel1);
      }
      else
      {
        NOVAS2.CatEntryNOVAS2 catEntryNovaS2 = NOVAS2.CatEntryToCatEntryNOVAS2(star);
        NOVAS2.starvectors32(ref catEntryNovaS2, ref pos1, ref vel1);
      }
      NOVAS2.PosVecToArr(pos1, ref pos);
      NOVAS2.VelVecToArr(vel1, ref vel);
    }

    public static void RADec2Vector(double ra, double dec, double dist, ref double[] pos)
    {
      PosVector pos1 = new PosVector();
      if (NOVAS2.Is64Bit())
        NOVAS2.radec2vector64(ra, dec, dist, ref pos1);
      else
        NOVAS2.radec2vector32(ra, dec, dist, ref pos1);
      NOVAS2.PosVecToArr(pos1, ref pos);
    }

    public static short GetEarth(double tjd, ref BodyDescription earth, ref double tdb, ref double[] bary_earthp, ref double[] bary_earthv, ref double[] helio_earthp, ref double[] helio_earthv)
    {
      PosVector bary_earthp1 = new PosVector();
      PosVector helio_earthp1 = new PosVector();
      VelVector bary_earthv1 = new VelVector();
      VelVector helio_earthv1 = new VelVector();
      short num;
      if (NOVAS2.Is64Bit())
      {
        double tjd1 = tjd;
        NOVAS2.BodyDescriptionShort earth1 = NOVAS2.BodyDescToShort(earth);
        num = NOVAS2.get_earth64(tjd1, ref earth1, ref tdb, ref bary_earthp1, ref bary_earthv1, ref helio_earthp1, ref helio_earthv1);
      }
      else
      {
        double tjd1 = tjd;
        NOVAS2.BodyDescriptionShort earth1 = NOVAS2.BodyDescToShort(earth);
        num = NOVAS2.get_earth32(tjd1, ref earth1, ref tdb, ref bary_earthp1, ref bary_earthv1, ref helio_earthp1, ref helio_earthv1);
      }
      NOVAS2.PosVecToArr(bary_earthp1, ref bary_earthp);
      NOVAS2.VelVecToArr(bary_earthv1, ref bary_earthv);
      NOVAS2.PosVecToArr(helio_earthp1, ref helio_earthp);
      NOVAS2.VelVecToArr(helio_earthv1, ref helio_earthv);
      return num;
    }

    public static short MeanStar(double tjd, ref BodyDescription earth, double ra, double dec, ref double mra, ref double mdec)
    {
      short num;
      if (NOVAS2.Is64Bit())
      {
        double tjd1 = tjd;
        NOVAS2.BodyDescriptionShort earth1 = NOVAS2.BodyDescToShort(earth);
        double ra1 = ra;
        double dec1 = dec;
        num = NOVAS2.mean_star64(tjd1, ref earth1, ra1, dec1, ref mra, ref mdec);
      }
      else
      {
        double tjd1 = tjd;
        NOVAS2.BodyDescriptionShort earth1 = NOVAS2.BodyDescToShort(earth);
        double ra1 = ra;
        double dec1 = dec;
        num = NOVAS2.mean_star32(tjd1, ref earth1, ra1, dec1, ref mra, ref mdec);
      }
      return num;
    }

    public static void Pnsw(double tjd, double gast, double x, double y, double[] vece, ref double[] vecs)
    {
      PosVector vecs1 = new PosVector();
      if (NOVAS2.Is64Bit())
      {
        double tjd1 = tjd;
        double gast1 = gast;
        double x1 = x;
        double y1 = y;
        PosVector posVec = NOVAS2.ArrToPosVec(vece);
        NOVAS2.pnsw64(tjd1, gast1, x1, y1, ref posVec, ref vecs1);
      }
      else
      {
        double tjd1 = tjd;
        double gast1 = gast;
        double x1 = x;
        double y1 = y;
        PosVector posVec = NOVAS2.ArrToPosVec(vece);
        NOVAS2.pnsw32(tjd1, gast1, x1, y1, ref posVec, ref vecs1);
      }
      NOVAS2.PosVecToArr(vecs1, ref vecs);
    }

    public static void Spin(double st, double[] pos1, ref double[] pos2)
    {
      PosVector pos2_1 = new PosVector();
      if (NOVAS2.Is64Bit())
      {
        double st1 = st;
        PosVector posVec = NOVAS2.ArrToPosVec(pos1);
        NOVAS2.spin64(st1, ref posVec, ref pos2_1);
      }
      else
      {
        double st1 = st;
        PosVector posVec = NOVAS2.ArrToPosVec(pos1);
        NOVAS2.spin32(st1, ref posVec, ref pos2_1);
      }
      NOVAS2.PosVecToArr(pos2_1, ref pos2);
    }

    public static void Wobble(double x, double y, double[] pos1, ref double[] pos2)
    {
      PosVector pos2_1 = new PosVector();
      if (NOVAS2.Is64Bit())
      {
        double x1 = x;
        double y1 = y;
        PosVector posVec = NOVAS2.ArrToPosVec(pos1);
        NOVAS2.wobble64(x1, y1, ref posVec, ref pos2_1);
      }
      else
      {
        double x1 = x;
        double y1 = y;
        PosVector posVec = NOVAS2.ArrToPosVec(pos1);
        NOVAS2.wobble32(x1, y1, ref posVec, ref pos2_1);
      }
      NOVAS2.PosVecToArr(pos2_1, ref pos2);
    }

    public static void Terra(ref SiteInfo locale, double st, ref double[] pos, ref double[] vel)
    {
      PosVector pos1 = new PosVector();
      VelVector vel1 = new VelVector();
      if (NOVAS2.Is64Bit())
        NOVAS2.terra64(ref locale, st, ref pos1, ref vel1);
      else
        NOVAS2.terra32(ref locale, st, ref pos1, ref vel1);
      NOVAS2.PosVecToArr(pos1, ref pos);
      NOVAS2.VelVecToArr(vel1, ref vel);
    }

    public static void ProperMotion(double tjd1, double[] pos, double[] vel, double tjd2, ref double[] pos2)
    {
      PosVector pos2_1 = new PosVector();
      if (NOVAS2.Is64Bit())
      {
        double tjd1_1 = tjd1;
        PosVector posVec = NOVAS2.ArrToPosVec(pos);
        VelVector velVec = NOVAS2.ArrToVelVec(vel);
        double tjd2_1 = tjd2;
        NOVAS2.proper_motion64(tjd1_1, ref posVec, ref velVec, tjd2_1, ref pos2_1);
      }
      else
      {
        double tjd1_1 = tjd1;
        PosVector posVec = NOVAS2.ArrToPosVec(pos);
        VelVector velVec = NOVAS2.ArrToVelVec(vel);
        double tjd2_1 = tjd2;
        NOVAS2.proper_motion32(tjd1_1, ref posVec, ref velVec, tjd2_1, ref pos2_1);
      }
      NOVAS2.PosVecToArr(pos2_1, ref pos2);
    }

    public static void BaryToGeo(double[] pos, double[] earthvector, ref double[] pos2, ref double lighttime)
    {
            PosVector pos2_1 = new PosVector();
            if (NOVAS2.Is64Bit())
      {
        PosVector posVec1 = NOVAS2.ArrToPosVec(pos);
        PosVector posVec2 = NOVAS2.ArrToPosVec(earthvector);
        NOVAS2.bary_to_geo64(ref posVec1, ref posVec2, ref pos2_1, ref lighttime);
      }
      else
      {
        PosVector posVec1 = NOVAS2.ArrToPosVec(pos);
        PosVector posVec2 = NOVAS2.ArrToPosVec(earthvector);
        NOVAS2.bary_to_geo32(ref posVec1, ref posVec2, ref pos2_1, ref lighttime);
      }
      NOVAS2.PosVecToArr(pos2_1, ref pos2);
    }

    public static short SunField(double[] pos, double[] earthvector, ref double[] pos2)
    {
            PosVector pos2_1 = new PosVector();
      short num;
      if (NOVAS2.Is64Bit())
      {
        PosVector posVec1 = NOVAS2.ArrToPosVec(pos);
        PosVector posVec2 = NOVAS2.ArrToPosVec(earthvector);
        num = NOVAS2.sun_field64(ref posVec1, ref posVec2, ref pos2_1);
      }
      else
      {
        PosVector posVec1 = NOVAS2.ArrToPosVec(pos);
        PosVector posVec2 = NOVAS2.ArrToPosVec(earthvector);
        num = NOVAS2.sun_field32(ref posVec1, ref posVec2, ref pos2_1);
      }
      NOVAS2.PosVecToArr(pos2_1, ref pos2);
      return num;
    }

    public static short Aberration(double[] pos, double[] vel, double lighttime, ref double[] pos2)
    {
      PosVector pos2_1 = new PosVector();
            short num;
      if (NOVAS2.Is64Bit())
      {
        PosVector posVec = NOVAS2.ArrToPosVec(pos);
        VelVector velVec = NOVAS2.ArrToVelVec(vel);
        double lighttime1 = lighttime;
        num = NOVAS2.aberration64(ref posVec, ref velVec, lighttime1, ref pos2_1);
      }
      else
      {
        PosVector posVec = NOVAS2.ArrToPosVec(pos);
        VelVector velVec = NOVAS2.ArrToVelVec(vel);
        double lighttime1 = lighttime;
        num = NOVAS2.aberration32(ref posVec, ref velVec, lighttime1, ref pos2_1);
      }
      NOVAS2.PosVecToArr(pos2_1, ref pos2);
      return num;
    }

    public static short Nutate(double tjd, NutationDirection fn, double[] pos, ref double[] pos2)
    {
      PosVector pos2_1 = new PosVector();
            short num1;
      if (NOVAS2.Is64Bit())
      {
        double tjd1 = tjd;
        int num2 = (int) checked ((short) fn);
        PosVector posVec = NOVAS2.ArrToPosVec(pos);
        num1 = NOVAS2.nutate64(tjd1, (short) num2, ref posVec, ref pos2_1);
      }
      else
      {
        double tjd1 = tjd;
        int num2 = (int) checked ((short) fn);
        PosVector posVec = NOVAS2.ArrToPosVec(pos);
        num1 = NOVAS2.nutate32(tjd1, (short) num2, ref posVec, ref pos2_1);
      }
      NOVAS2.PosVecToArr(pos2_1, ref pos2);
      return num1;
    }

    public static short NutationAngles(double tdbtime, ref double longnutation, ref double obliqnutation)
    {
      if (NOVAS2.Is64Bit())
        return NOVAS2.nutation_angles64(tdbtime, ref longnutation, ref obliqnutation);
      return NOVAS2.nutation_angles32(tdbtime, ref longnutation, ref obliqnutation);
    }

    public static void FundArgs(double t, ref double[] a)
    {
      FundamentalArgs a1 = new FundamentalArgs();
      if (NOVAS2.Is64Bit())
        NOVAS2.fund_args64(t, ref a1);
      else
        NOVAS2.fund_args32(t, ref a1);
      a[0] = a1.l;
      a[1] = a1.ldash;
      a[2] = a1.F;
      a[3] = a1.D;
      a[4] = a1.Omega;
    }

    public static void Tdb2Tdt(double tdb, ref double tdtjd, ref double secdiff)
    {
      if (NOVAS2.Is64Bit())
        NOVAS2.tdb2tdt64(tdb, ref tdtjd, ref secdiff);
      else
        NOVAS2.tdb2tdt32(tdb, ref tdtjd, ref secdiff);
    }

    public static short SetBody(BodyType type, Body number, string name, ref BodyDescription cel_obj)
    {
      NOVAS2.BodyDescriptionShort cel_obj1 = new NOVAS2.BodyDescriptionShort();
      if (NOVAS2.Is64Bit())
        return NOVAS2.set_body64(checked ((short) type), checked ((short) number), name, ref cel_obj1);
      return NOVAS2.set_body32(checked ((short) type), checked ((short) number), name, ref cel_obj1);
    }

    public static void MakeCatEntry(string catalog, string star_name, int star_num, double ra, double dec, double pm_ra, double pm_dec, double parallax, double rad_vel, ref CatEntry star)
    {
      NOVAS2.CatEntryNOVAS2 star1 = new NOVAS2.CatEntryNOVAS2();
      if (NOVAS2.Is64Bit())
        NOVAS2.make_cat_entry64(catalog, star_name, star_num, ra, dec, pm_ra, pm_dec, parallax, rad_vel, ref star1);
      else
        NOVAS2.make_cat_entry32(catalog, star_name, star_num, ra, dec, pm_ra, pm_dec, parallax, rad_vel, ref star1);
      NOVAS2.CatEntryNOVAS2ToCatEntry(star1, ref star);
    }

    public static double Refract(ref SiteInfo location, short ref_option, double zd_obs)
    {
      if (NOVAS2.Is64Bit())
        return NOVAS2.refract64(ref location, ref_option, zd_obs);
      return NOVAS2.refract32(ref location, ref_option, zd_obs);
    }

    public static double JulianDate(short year, short month, short day, double hour)
    {
      if (NOVAS2.Is64Bit())
        return NOVAS2.julian_date64(year, month, day, hour);
      return NOVAS2.julian_date32(year, month, day, hour);
    }

    public static void CalDate(double tjd, ref short year, ref short month, ref short day, ref double hour)
    {
      if (NOVAS2.Is64Bit())
        NOVAS2.cal_date64(tjd, ref year, ref month, ref day, ref hour);
      else
        NOVAS2.cal_date32(tjd, ref year, ref month, ref day, ref hour);
    }

    public static void SunEph(double jd, ref double ra, ref double dec, ref double dis)
    {
      if (NOVAS2.Is64Bit())
        NOVAS2.sun_eph64(jd, ref ra, ref dec, ref dis);
      else
        NOVAS2.sun_eph32(jd, ref ra, ref dec, ref dis);
    }

    public static double DeltaT(double Tjd)
    {
      return DeltatCode.DeltaTCalc(Tjd);
    }

    [DllImport("NOVAS-C.dll", EntryPoint = "app_star")]
    private static extern short app_star32(double tjd, ref NOVAS2.BodyDescriptionShort earth, ref NOVAS2.CatEntryNOVAS2 star, ref double ra, ref double dec);

    [DllImport("NOVAS-C.dll", EntryPoint = "topo_star")]
    private static extern short topo_star32(double tjd, ref NOVAS2.BodyDescriptionShort earth, double deltat, ref NOVAS2.CatEntryNOVAS2 star, ref SiteInfo location, ref double ra, ref double dec);

    [DllImport("NOVAS-C.dll", EntryPoint = "app_planet")]
    private static extern short app_planet32(double tjd, ref NOVAS2.BodyDescriptionShort ss_object, ref NOVAS2.BodyDescriptionShort earth, ref double ra, ref double dec, ref double dis);

    [DllImport("NOVAS-C.dll", EntryPoint = "topo_planet")]
    private static extern short topo_planet32(double tjd, ref NOVAS2.BodyDescriptionShort ss_object, ref NOVAS2.BodyDescriptionShort earth, double deltat, ref SiteInfo location, ref double ra, ref double dec, ref double dis);

    [DllImport("NOVAS-C.dll", EntryPoint = "virtual_star")]
    private static extern short virtual_star32(double tjd, ref NOVAS2.BodyDescriptionShort earth, ref NOVAS2.CatEntryNOVAS2 star, ref double ra, ref double dec);

    [DllImport("NOVAS-C.dll", EntryPoint = "local_star")]
    private static extern short local_star32(double tjd, ref NOVAS2.BodyDescriptionShort earth, double deltat, ref NOVAS2.CatEntryNOVAS2 star, ref SiteInfo location, ref double ra, ref double dec);

    [DllImport("NOVAS-C.dll", EntryPoint = "virtual_planet")]
    private static extern short virtual_planet32(double tjd, ref NOVAS2.BodyDescriptionShort ss_object, ref NOVAS2.BodyDescriptionShort earth, ref double ra, ref double dec, ref double dis);

    [DllImport("NOVAS-C.dll", EntryPoint = "local_planet")]
    private static extern short local_planet32(double tjd, ref NOVAS2.BodyDescriptionShort ss_object, ref NOVAS2.BodyDescriptionShort earth, double deltat, ref SiteInfo location, ref double ra, ref double dec, ref double dis);

    [DllImport("NOVAS-C.dll", EntryPoint = "astro_star")]
    private static extern short astro_star32(double tjd, ref NOVAS2.BodyDescriptionShort earth, ref NOVAS2.CatEntryNOVAS2 star, ref double ra, ref double dec);

    [DllImport("NOVAS-C.dll", EntryPoint = "astro_planet")]
    private static extern short astro_planet32(double tjd, ref NOVAS2.BodyDescriptionShort ss_object, ref NOVAS2.BodyDescriptionShort earth, ref double ra, ref double dec, ref double dis);

    [DllImport("NOVAS-C.dll", EntryPoint = "equ2hor")]
    private static extern void equ2hor32(double tjd, double deltat, double x, double y, ref SiteInfo location, double ra, double dec, short ref_option, ref double zd, ref double az, ref double rar, ref double decr);

    [DllImport("NOVAS-C.dll", EntryPoint = "transform_hip")]
    private static extern void transform_hip32(ref NOVAS2.CatEntryNOVAS2 hipparcos, ref NOVAS2.CatEntryNOVAS2 fk5);

    [DllImport("NOVAS-C.dll", EntryPoint = "transform_cat")]
    private static extern void transform_cat32(short option, double date_incat, ref NOVAS2.CatEntryNOVAS2 incat, double date_newcat, ref byte[] newcat_id, ref NOVAS2.CatEntryNOVAS2 newcat);

    [DllImport("NOVAS-C.dll", EntryPoint = "sidereal_time")]
    private static extern void sidereal_time32(double jd_high, double jd_low, double ee, ref double gst);

    [DllImport("NOVAS-C.dll", EntryPoint = "precession")]
    private static extern void precession32(double tjd1, ref PosVector pos, double tjd2, ref PosVector pos2);

    [DllImport("NOVAS-C.dll", EntryPoint = "earthtilt")]
    private static extern void earthtilt32(double tjd, ref double mobl, ref double tobl, ref double eq, ref double dpsi, ref double deps);

    [DllImport("NOVAS-C.dll", EntryPoint = "cel_pole")]
    private static extern void cel_pole32(double del_dpsi, double del_deps);

    [DllImport("NOVAS-C.dll", EntryPoint = "ephemeris")]
    private static extern short ephemeris32(double tjd, ref NOVAS2.BodyDescriptionShort cel_obj, short origin, ref PosVector pos, ref VelVector vel);

    [DllImport("NOVAS-C.dll", EntryPoint = "solarsystem")]
    private static extern short solarsystem32(double tjd, short body, short origin, ref PosVector pos, ref VelVector vel);

    [DllImport("NOVAS-C.dll", EntryPoint = "vector2radec")]
    private static extern short vector2radec32(ref PosVector pos, ref double ra, ref double dec);

    [DllImport("NOVAS-C.dll", EntryPoint = "starvectors")]
    private static extern void starvectors32(ref NOVAS2.CatEntryNOVAS2 star, ref PosVector pos, ref VelVector vel);

    [DllImport("NOVAS-C.dll", EntryPoint = "radec2vector")]
    private static extern void radec2vector32(double ra, double dec, double dist, ref PosVector pos);

    [DllImport("NOVAS-C.dll", EntryPoint = "get_earth")]
    private static extern short get_earth32(double tjd, ref NOVAS2.BodyDescriptionShort earth, ref double tdb, ref PosVector bary_earthp, ref VelVector bary_earthv, ref PosVector helio_earthp, ref VelVector helio_earthv);

    [DllImport("NOVAS-C.dll", EntryPoint = "mean_star")]
    private static extern short mean_star32(double tjd, ref NOVAS2.BodyDescriptionShort earth, double ra, double dec, ref double mra, ref double mdec);

    [DllImport("NOVAS-C.dll", EntryPoint = "pnsw")]
    private static extern void pnsw32(double tjd, double gast, double x, double y, ref PosVector vece, ref PosVector vecs);

    [DllImport("NOVAS-C.dll", EntryPoint = "spin")]
    private static extern void spin32(double st, ref PosVector pos1, ref PosVector pos2);

    [DllImport("NOVAS-C.dll", EntryPoint = "wobble")]
    private static extern void wobble32(double x, double y, ref PosVector pos1, ref PosVector pos2);

    [DllImport("NOVAS-C.dll", EntryPoint = "terra")]
    private static extern void terra32(ref SiteInfo locale, double st, ref PosVector pos, ref VelVector vel);

    [DllImport("NOVAS-C.dll", EntryPoint = "proper_motion")]
    private static extern void proper_motion32(double tjd1, ref PosVector pos, ref VelVector vel, double tjd2, ref PosVector pos2);

    [DllImport("NOVAS-C.dll", EntryPoint = "bary_to_geo")]
    private static extern void bary_to_geo32(ref PosVector pos, ref PosVector earthvector, ref PosVector pos2, ref double lighttime);

    [DllImport("NOVAS-C.dll", EntryPoint = "sun_field")]
    private static extern short sun_field32(ref PosVector pos, ref PosVector earthvector, ref PosVector pos2);

    [DllImport("NOVAS-C.dll", EntryPoint = "aberration")]
    private static extern short aberration32(ref PosVector pos, ref VelVector vel, double lighttime, ref PosVector pos2);

    [DllImport("NOVAS-C.dll", EntryPoint = "nutate")]
    private static extern short nutate32(double tjd, short fn, ref PosVector pos, ref PosVector pos2);

    [DllImport("NOVAS-C.dll", EntryPoint = "nutation_angles")]
    private static extern short nutation_angles32(double tdbtime, ref double longnutation, ref double obliqnutation);

    [DllImport("NOVAS-C.dll", EntryPoint = "fund_args")]
    private static extern void fund_args32(double t, ref FundamentalArgs a);

    [DllImport("NOVAS-C.dll", EntryPoint = "tdb2tdt")]
    private static extern void tdb2tdt32(double tdb, ref double tdtjd, ref double secdiff);

    [DllImport("NOVAS-C.dll", EntryPoint = "set_body")]
    private static extern short set_body32(short type, short number, [MarshalAs(UnmanagedType.LPStr)] string name, ref NOVAS2.BodyDescriptionShort cel_obj);

    [DllImport("NOVAS-C.dll", EntryPoint = "readeph")]
    private static extern PosVector readeph32(int mp, IntPtr name, double jd, ref int err);

    [DllImport("NOVAS-C.dll", EntryPoint = "make_cat_entry")]
    private static extern void make_cat_entry32([MarshalAs(UnmanagedType.LPStr)] string catalog, [MarshalAs(UnmanagedType.LPStr)] string star_name, int star_num, double ra, double dec, double pm_ra, double pm_dec, double parallax, double rad_vel, ref NOVAS2.CatEntryNOVAS2 star);

    [DllImport("NOVAS-C.dll", EntryPoint = "refract")]
    private static extern double refract32(ref SiteInfo location, short ref_option, double zd_obs);

    [DllImport("NOVAS-C.dll", EntryPoint = "julian_date")]
    private static extern double julian_date32(short year, short month, short day, double hour);

    [DllImport("NOVAS-C.dll", EntryPoint = "cal_date")]
    private static extern void cal_date32(double tjd, ref short year, ref short month, ref short day, ref double hour);

    [DllImport("NOVAS-C.dll", EntryPoint = "sun_eph")]
    private static extern void sun_eph32(double jd, ref double ra, ref double dec, ref double dis);

    [DllImport("NOVAS-C64.dll", EntryPoint = "app_star")]
    private static extern short app_star64(double tjd, ref NOVAS2.BodyDescriptionShort earth, ref NOVAS2.CatEntryNOVAS2 star, ref double ra, ref double dec);

    [DllImport("NOVAS-C64.dll", EntryPoint = "topo_star")]
    private static extern short topo_star64(double tjd, ref NOVAS2.BodyDescriptionShort earth, double deltat, ref NOVAS2.CatEntryNOVAS2 star, ref SiteInfo location, ref double ra, ref double dec);

    [DllImport("NOVAS-C64.dll", EntryPoint = "app_planet")]
    private static extern short app_planet64(double tjd, ref NOVAS2.BodyDescriptionShort ss_object, ref NOVAS2.BodyDescriptionShort earth, ref double ra, ref double dec, ref double dis);

    [DllImport("NOVAS-C64.dll", EntryPoint = "topo_planet")]
    private static extern short topo_planet64(double tjd, ref NOVAS2.BodyDescriptionShort ss_object, ref NOVAS2.BodyDescriptionShort earth, double deltat, ref SiteInfo location, ref double ra, ref double dec, ref double dis);

    [DllImport("NOVAS-C64.dll", EntryPoint = "virtual_star")]
    private static extern short virtual_star64(double tjd, ref NOVAS2.BodyDescriptionShort earth, ref NOVAS2.CatEntryNOVAS2 star, ref double ra, ref double dec);

    [DllImport("NOVAS-C64.dll", EntryPoint = "local_star")]
    private static extern short local_star64(double tjd, ref NOVAS2.BodyDescriptionShort earth, double deltat, ref NOVAS2.CatEntryNOVAS2 star, ref SiteInfo location, ref double ra, ref double dec);

    [DllImport("NOVAS-C64.dll", EntryPoint = "virtual_planet")]
    private static extern short virtual_planet64(double tjd, ref NOVAS2.BodyDescriptionShort ss_object, ref NOVAS2.BodyDescriptionShort earth, ref double ra, ref double dec, ref double dis);

    [DllImport("NOVAS-C64.dll", EntryPoint = "local_planet")]
    private static extern short local_planet64(double tjd, ref NOVAS2.BodyDescriptionShort ss_object, ref NOVAS2.BodyDescriptionShort earth, double deltat, ref SiteInfo location, ref double ra, ref double dec, ref double dis);

    [DllImport("NOVAS-C64.dll", EntryPoint = "astro_star")]
    private static extern short astro_star64(double tjd, ref NOVAS2.BodyDescriptionShort earth, ref NOVAS2.CatEntryNOVAS2 star, ref double ra, ref double dec);

    [DllImport("NOVAS-C64.dll", EntryPoint = "astro_planet")]
    private static extern short astro_planet64(double tjd, ref NOVAS2.BodyDescriptionShort ss_object, ref NOVAS2.BodyDescriptionShort earth, ref double ra, ref double dec, ref double dis);

    [DllImport("NOVAS-C64.dll", EntryPoint = "equ2hor")]
    private static extern void equ2hor64(double tjd, double deltat, double x, double y, ref SiteInfo location, double ra, double dec, short ref_option, ref double zd, ref double az, ref double rar, ref double decr);

    [DllImport("NOVAS-C64.dll", EntryPoint = "transform_hip")]
    private static extern void transform_hip64(ref NOVAS2.CatEntryNOVAS2 hipparcos, ref NOVAS2.CatEntryNOVAS2 fk5);

    [DllImport("NOVAS-C64.dll", EntryPoint = "transform_cat")]
    private static extern void transform_cat64(short option, double date_incat, ref NOVAS2.CatEntryNOVAS2 incat, double date_newcat, ref byte[] newcat_id, ref NOVAS2.CatEntryNOVAS2 newcat);

    [DllImport("NOVAS-C64.dll", EntryPoint = "sidereal_time")]
    private static extern void sidereal_time64(double jd_high, double jd_low, double ee, ref double gst);

    [DllImport("NOVAS-C64.dll", EntryPoint = "precession")]
    private static extern void precession64(double tjd1, ref PosVector pos, double tjd2, ref PosVector pos2);

    [DllImport("NOVAS-C64.dll", EntryPoint = "earthtilt")]
    private static extern void earthtilt64(double tjd, ref double mobl, ref double tobl, ref double eq, ref double dpsi, ref double deps);

    [DllImport("NOVAS-C64.dll", EntryPoint = "cel_pole")]
    private static extern void cel_pole64(double del_dpsi, double del_deps);

    [DllImport("NOVAS-C64.dll", EntryPoint = "ephemeris")]
    private static extern short ephemeris64(double tjd, ref NOVAS2.BodyDescriptionShort cel_obj, short origin, ref PosVector pos, ref VelVector vel);

    [DllImport("NOVAS-C64.dll", EntryPoint = "solarsystem")]
    private static extern short solarsystem64(double tjd, short body, short origin, ref PosVector pos, ref VelVector vel);

    [DllImport("NOVAS-C64.dll", EntryPoint = "vector2radec")]
    private static extern short vector2radec64(ref PosVector pos, ref double ra, ref double dec);

    [DllImport("NOVAS-C64.dll", EntryPoint = "starvectors")]
    private static extern void starvectors64(ref NOVAS2.CatEntryNOVAS2 star, ref PosVector pos, ref VelVector vel);

    [DllImport("NOVAS-C64.dll", EntryPoint = "radec2vector")]
    private static extern void radec2vector64(double ra, double dec, double dist, ref PosVector pos);

    [DllImport("NOVAS-C64.dll", EntryPoint = "get_earth")]
    private static extern short get_earth64(double tjd, ref NOVAS2.BodyDescriptionShort earth, ref double tdb, ref PosVector bary_earthp, ref VelVector bary_earthv, ref PosVector helio_earthp, ref VelVector helio_earthv);

    [DllImport("NOVAS-C64.dll", EntryPoint = "mean_star")]
    private static extern short mean_star64(double tjd, ref NOVAS2.BodyDescriptionShort earth, double ra, double dec, ref double mra, ref double mdec);

    [DllImport("NOVAS-C64.dll", EntryPoint = "pnsw")]
    private static extern void pnsw64(double tjd, double gast, double x, double y, ref PosVector vece, ref PosVector vecs);

    [DllImport("NOVAS-C64.dll", EntryPoint = "spin")]
    private static extern void spin64(double st, ref PosVector pos1, ref PosVector pos2);

    [DllImport("NOVAS-C64.dll", EntryPoint = "wobble")]
    private static extern void wobble64(double x, double y, ref PosVector pos1, ref PosVector pos2);

    [DllImport("NOVAS-C64.dll", EntryPoint = "terra")]
    private static extern void terra64(ref SiteInfo locale, double st, ref PosVector pos, ref VelVector vel);

    [DllImport("NOVAS-C64.dll", EntryPoint = "proper_motion")]
    private static extern void proper_motion64(double tjd1, ref PosVector pos, ref VelVector vel, double tjd2, ref PosVector pos2);

    [DllImport("NOVAS-C64.dll", EntryPoint = "bary_to_geo")]
    private static extern void bary_to_geo64(ref PosVector pos, ref PosVector earthvector, ref PosVector pos2, ref double lighttime);

    [DllImport("NOVAS-C64.dll", EntryPoint = "sun_field")]
    private static extern short sun_field64(ref PosVector pos, ref PosVector earthvector, ref PosVector pos2);

    [DllImport("NOVAS-C64.dll", EntryPoint = "aberration")]
    private static extern short aberration64(ref PosVector pos, ref VelVector vel, double lighttime, ref PosVector pos2);

    [DllImport("NOVAS-C64.dll", EntryPoint = "nutate")]
    private static extern short nutate64(double tjd, short fn, ref PosVector pos, ref PosVector pos2);

    [DllImport("NOVAS-C64.dll", EntryPoint = "nutation_angles")]
    private static extern short nutation_angles64(double tdbtime, ref double longnutation, ref double obliqnutation);

    [DllImport("NOVAS-C64.dll", EntryPoint = "fund_args")]
    private static extern void fund_args64(double t, ref FundamentalArgs a);

    [DllImport("NOVAS-C64.dll", EntryPoint = "tdb2tdt")]
    private static extern void tdb2tdt64(double tdb, ref double tdtjd, ref double secdiff);

    [DllImport("NOVAS-C64.dll", EntryPoint = "set_body")]
    private static extern short set_body64(short type, short number, [MarshalAs(UnmanagedType.LPStr)] string name, ref NOVAS2.BodyDescriptionShort cel_obj);

    [DllImport("NOVAS-C64.dll", EntryPoint = "readeph")]
    private static extern PosVector readeph64(int mp, IntPtr name, double jd, ref int err);

    [DllImport("NOVAS-C64.dll", EntryPoint = "make_cat_entry")]
    private static extern void make_cat_entry64([MarshalAs(UnmanagedType.LPStr)] string catalog, [MarshalAs(UnmanagedType.LPStr)] string star_name, int star_num, double ra, double dec, double pm_ra, double pm_dec, double parallax, double rad_vel, ref NOVAS2.CatEntryNOVAS2 star);

    [DllImport("NOVAS-C64.dll", EntryPoint = "refract")]
    private static extern double refract64(ref SiteInfo location, short ref_option, double zd_obs);

    [DllImport("NOVAS-C64.dll", EntryPoint = "julian_date")]
    private static extern double julian_date64(short year, short month, short day, double hour);

    [DllImport("NOVAS-C64.dll", EntryPoint = "cal_date")]
    private static extern void cal_date64(double tjd, ref short year, ref short month, ref short day, ref double hour);

    [DllImport("NOVAS-C64.dll", EntryPoint = "sun_eph")]
    private static extern void sun_eph64(double jd, ref double ra, ref double dec, ref double dis);

    [DllImport("NOVAS-C64.dll", EntryPoint = "solarsystem")]
    private static extern short solarsystem64(double tjd, short body, ref int origin, ref PosVector pos, ref VelVector vel);

    [DllImport("shell32.dll")]
    public static extern bool SHGetSpecialFolderPath(IntPtr hwndOwner, [Out] StringBuilder lpszPath, int nFolder, bool fCreate);

    [DllImport("kernel32.dll")]
    private static extern bool SetDllDirectory(string lpPathName);

    private static bool Is64Bit()
    {
      if (NOVAS2._Is64Bit__002__StatPath=="")
      {
        StringBuilder lpszPath = new StringBuilder(260);
        bool flag;
        string folderPath;
        if (IntPtr.Size == 8)
        {
          flag = NOVAS2.SHGetSpecialFolderPath(IntPtr.Zero, lpszPath, 44, false);
          folderPath = lpszPath.ToString();
        }
        else
          folderPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles);
        NOVAS2._Is64Bit__002__StatPath = folderPath + "\\ASCOM\\Astrometry";
        flag = NOVAS2.SetDllDirectory(NOVAS2._Is64Bit__002__StatPath);
      }
      return IntPtr.Size == 8;
    }

    private static PosVector ArrToPosVec(double[] Arr)
    {
      return new PosVector()
      {
        x = Arr[0],
        y = Arr[1],
        z = Arr[2]
      };
    }

    private static void PosVecToArr(PosVector V, ref double[] Ar)
    {
      Ar[0] = V.x;
      Ar[1] = V.y;
      Ar[2] = V.z;
    }

    private static VelVector ArrToVelVec(double[] Arr)
    {
      return new VelVector()
      {
        x = Arr[0],
        y = Arr[1],
        z = Arr[2]
      };
    }

    private static void VelVecToArr(VelVector V, ref double[] Ar)
    {
      Ar[0] = V.x;
      Ar[1] = V.y;
      Ar[2] = V.z;
    }

    private static NOVAS2.BodyDescriptionShort BodyDescToShort(BodyDescription BD)
    {
      return new NOVAS2.BodyDescriptionShort()
      {
        Name = BD.Name,
        Number = checked ((short) BD.Number),
        Type = checked ((short) BD.Type)
      };
    }

    private static NOVAS2.CatEntryNOVAS2 CatEntryToCatEntryNOVAS2(CatEntry CE)
    {
      return new NOVAS2.CatEntryNOVAS2()
      {
        Catalog = CE.Catalog,
        Dec = CE.Dec,
        Parallax = CE.Parallax,
        ProMoDec = CE.ProMoDec,
        ProMoRA = CE.ProMoRA,
        RA = CE.RA,
        RadialVelocity = CE.RadialVelocity,
        StarName = CE.StarName,
        StarNumber = CE.StarNumber
      };
    }

    private static void CatEntryNOVAS2ToCatEntry(NOVAS2.CatEntryNOVAS2 CEN2, ref CatEntry CE)
    {
      CE.Catalog = CEN2.Catalog;
      CE.Dec = CEN2.Dec;
      CE.Parallax = CEN2.Parallax;
      CE.ProMoDec = CEN2.ProMoDec;
      CE.ProMoRA = CEN2.ProMoRA;
      CE.RA = CEN2.RA;
      CE.RadialVelocity = CEN2.RadialVelocity;
      CE.StarName = CEN2.StarName;
      CE.StarNumber = CEN2.StarNumber;
    }

    private struct BodyDescriptionShort
    {
      public short Type;
      public short Number;
      [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
      public string Name;
    }

    private struct CatEntryNOVAS2
    {
      [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
      public string Catalog;
      [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 51)]
      public string StarName;
      public int StarNumber;
      public double RA;
      public double Dec;
      public double ProMoRA;
      public double ProMoDec;
      public double Parallax;
      public double RadialVelocity;
    }
  }
}
