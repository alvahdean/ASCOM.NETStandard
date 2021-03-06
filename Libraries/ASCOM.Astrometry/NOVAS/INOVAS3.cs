﻿// Decompiled with JetBrains decompiler
// Type: ASCOM.Astrometry.NOVAS.INOVAS3
// Assembly: ASCOM.Astrometry, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 695EF5AA-123E-4689-AAF5-64FD1324F6BB
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.Astrometry\6.0.0.0__565de7938946fba7\ASCOM.Astrometry.dll

using System.Collections;
using System.Runtime.InteropServices;

namespace ASCOM.Astrometry.NOVAS
{
  //[ComVisible(true)]
  //[Guid("5EF15982-D79E-42f7-B20B-E83232E2B86B")]
  public interface INOVAS3
  {
    [DispId(1)]
    short PlanetEphemeris(ref double[] Tjd, Target Target, Target Center, ref double[] Position, ref double[] Velocity);

    [DispId(2)]
    double[] ReadEph(int Mp, string Name, double Jd, ref int Err);

    [DispId(3)]
    short SolarSystem(double Tjd, Body Body, Origin Origin, ref double[] Pos, ref double[] Vel);

    [DispId(4)]
    short State(ref double[] Jed, Target Target, ref double[] TargetPos, ref double[] TargetVel);

    [DispId(5)]
    void Aberration(double[] Pos, double[] Vel, double LightTime, ref double[] Pos2);

    [DispId(6)]
    short AppPlanet(double JdTt, Object3 SsBody, Accuracy Accuracy, ref double Ra, ref double Dec, ref double Dis);

    [DispId(7)]
    short AppStar(double JdTt, CatEntry3 Star, Accuracy Accuracy, ref double Ra, ref double Dec);

    [DispId(8)]
    short AstroPlanet(double JdTt, Object3 SsBody, Accuracy Accuracy, ref double Ra, ref double Dec, ref double Dis);

    [DispId(9)]
    short AstroStar(double JdTt, CatEntry3 Star, Accuracy Accuracy, ref double Ra, ref double Dec);

    [DispId(10)]
    void Bary2Obs(double[] Pos, double[] PosObs, ref double[] Pos2, ref double Lighttime);

    [DispId(11)]
    void CalDate(double Tjd, ref short Year, ref short Month, ref short Day, ref double Hour);

    [DispId(12)]
    short CelPole(double Tjd, PoleOffsetCorrection Type, double Dpole1, double Dpole2);

    [DispId(13)]
    short CioArray(double JdTdb, int NPts, ref ArrayList Cio);

    [DispId(14)]
    short CioBasis(double JdTdbEquionx, double RaCioEquionx, ReferenceSystem RefSys, Accuracy Accuracy, ref double x, ref double y, ref double z);

    [DispId(15)]
    short CioLocation(double JdTdb, Accuracy Accuracy, ref double RaCio, ref ReferenceSystem RefSys);

    [DispId(16)]
    short CioRa(double JdTt, Accuracy Accuracy, ref double RaCio);

    [DispId(17)]
    double DLight(double[] Pos1, double[] PosObs);

    [DispId(18)]
    short Ecl2EquVec(double JdTt, CoordSys CoordSys, Accuracy Accuracy, double[] Pos1, ref double[] Pos2);

    [DispId(19)]
    double EeCt(double JdHigh, double JdLow, Accuracy Accuracy);

    [DispId(20)]
    short Ephemeris(double[] Jd, Object3 CelObj, Origin Origin, Accuracy Accuracy, ref double[] Pos, ref double[] Vel);

    [DispId(21)]
    short Equ2Ecl(double JdTt, CoordSys CoordSys, Accuracy Accuracy, double Ra, double Dec, ref double ELon, ref double ELat);

    [DispId(22)]
    short Equ2EclVec(double JdTt, CoordSys CoordSys, Accuracy Accuracy, double[] Pos1, ref double[] Pos2);

    [DispId(23)]
    void Equ2Gal(double RaI, double DecI, ref double GLon, ref double GLat);

    [DispId(24)]
    void Equ2Hor(double Jd_Ut1, double DeltT, Accuracy Accuracy, double x, double y, OnSurface Location, double Ra, double Dec, RefractionOption RefOption, ref double Zd, ref double Az, ref double RaR, ref double DecR);

    [DispId(25)]
    double Era(double JdHigh, double JdLow);

    [DispId(26)]
    void ETilt(double JdTdb, Accuracy Accuracy, ref double Mobl, ref double Tobl, ref double Ee, ref double Dpsi, ref double Deps);

    [DispId(27)]
    void FrameTie(double[] Pos1, FrameConversionDirection Direction, ref double[] Pos2);

    [DispId(28)]
    void FundArgs(double t, ref double[] a);

    [DispId(29)]
    short Gcrs2Equ(double JdTt, CoordSys CoordSys, Accuracy Accuracy, double RaG, double DecG, ref double Ra, ref double Dec);

    [DispId(30)]
    short GeoPosVel(double JdTt, double DeltaT, Accuracy Accuracy, Observer Obs, ref double[] Pos, ref double[] Vel);

    [DispId(31)]
    short GravDef(double JdTdb, EarthDeflection LocCode, Accuracy Accuracy, double[] Pos1, double[] PosObs, ref double[] Pos2);

    [DispId(32)]
    void GravVec(double[] Pos1, double[] PosObs, double[] PosBody, double RMass, ref double[] Pos2);

    [DispId(33)]
    double IraEquinox(double JdTdb, EquinoxType Equinox, Accuracy Accuracy);

    [DispId(34)]
    double JulianDate(short Year, short Month, short Day, double Hour);

    [DispId(35)]
    short LightTime(double JdTdb, Object3 SsObject, double[] PosObs, double TLight0, Accuracy Accuracy, ref double[] Pos, ref double TLight);

    [DispId(36)]
    void LimbAngle(double[] PosObj, double[] PosObs, ref double LimbAng, ref double NadirAng);

    [DispId(37)]
    short LocalPlanet(double JdTt, Object3 SsBody, double DeltaT, OnSurface Position, Accuracy Accuracy, ref double Ra, ref double Dec, ref double Dis);

    [DispId(38)]
    short LocalStar(double JdTt, double DeltaT, CatEntry3 Star, OnSurface Position, Accuracy Accuracy, ref double Ra, ref double Dec);

    [DispId(39)]
    void MakeCatEntry(string StarName, string Catalog, int StarNum, double Ra, double Dec, double PmRa, double PmDec, double Parallax, double RadVel, ref CatEntry3 Star);

    [DispId(40)]
    void MakeInSpace(double[] ScPos, double[] ScVel, ref InSpace ObsSpace);

    [DispId(41)]
    short MakeObject(ObjectType Type, short Number, string Name, CatEntry3 StarData, ref Object3 CelObj);

    [DispId(42)]
    short MakeObserver(ObserverLocation Where, OnSurface ObsSurface, InSpace ObsSpace, ref Observer Obs);

    [DispId(43)]
    void MakeObserverAtGeocenter(ref Observer ObsAtGeocenter);

    [DispId(44)]
    void MakeObserverInSpace(double[] ScPos, double[] ScVel, ref Observer ObsInSpace);

    [DispId(45)]
    void MakeObserverOnSurface(double Latitude, double Longitude, double Height, double Temperature, double Pressure, ref Observer ObsOnSurface);

    [DispId(46)]
    void MakeOnSurface(double Latitude, double Longitude, double Height, double Temperature, double Pressure, ref OnSurface ObsSurface);

    [DispId(47)]
    double MeanObliq(double JdTdb);

    [DispId(48)]
    short MeanStar(double JdTt, double Ra, double Dec, Accuracy Accuracy, ref double IRa, ref double IDec);

    [DispId(49)]
    double NormAng(double Angle);

    [DispId(50)]
    void Nutation(double JdTdb, NutationDirection Direction, Accuracy Accuracy, double[] Pos, ref double[] Pos2);

    [DispId(51)]
    void NutationAngles(double t, Accuracy Accuracy, ref double DPsi, ref double DEps);

    [DispId(52)]
    short Place(double JdTt, Object3 CelObject, Observer Location, double DeltaT, CoordSys CoordSys, Accuracy Accuracy, ref SkyPos Output);

    [DispId(53)]
    short Precession(double JdTdb1, double[] Pos1, double JdTdb2, ref double[] Pos2);

    [DispId(54)]
    void ProperMotion(double JdTdb1, double[] Pos, double[] Vel, double JdTdb2, ref double[] Pos2);

    [DispId(55)]
    void RaDec2Vector(double Ra, double Dec, double Dist, ref double[] Vector);

    [DispId(56)]
    void RadVel(Object3 CelObject, double[] Pos, double[] Vel, double[] VelObs, double DObsGeo, double DObsSun, double DObjSun, ref double Rv);

    [DispId(57)]
    double Refract(OnSurface Location, RefractionOption RefOption, double ZdObs);

    [DispId(58)]
    short SiderealTime(double JdHigh, double JdLow, double DeltaT, GstType GstType, Method Method, Accuracy Accuracy, ref double Gst);

    [DispId(59)]
    void Spin(double Angle, double[] Pos1, ref double[] Pos2);

    [DispId(60)]
    void StarVectors(CatEntry3 Star, ref double[] Pos, ref double[] Vel);

    [DispId(61)]
    void Tdb2Tt(double TdbJd, ref double TtJd, ref double SecDiff);

    [DispId(62)]
    short Ter2Cel(double JdHigh, double JdLow, double DeltaT, Method Method, Accuracy Accuracy, OutputVectorOption OutputOption, double x, double y, double[] VecT, ref double[] VecC);

    [DispId(63)]
    void Terra(OnSurface Location, double St, ref double[] Pos, ref double[] Vel);

    [DispId(64)]
    short TopoPlanet(double JdTt, Object3 SsBody, double DeltaT, OnSurface Position, Accuracy Accuracy, ref double Ra, ref double Dec, ref double Dis);

    [DispId(65)]
    short TopoStar(double JdTt, double DeltaT, CatEntry3 Star, OnSurface Position, Accuracy Accuracy, ref double Ra, ref double Dec);

    [DispId(66)]
    short TransformCat(TransformationOption3 TransformOption, double DateInCat, CatEntry3 InCat, double DateNewCat, string NewCatId, ref CatEntry3 NewCat);

    [DispId(67)]
    void TransformHip(CatEntry3 Hipparcos, ref CatEntry3 Hip2000);

    [DispId(68)]
    short Vector2RaDec(double[] Pos, ref double Ra, ref double Dec);

    [DispId(69)]
    short VirtualPlanet(double JdTt, Object3 SsBody, Accuracy Accuracy, ref double Ra, ref double Dec, ref double Dis);

    [DispId(70)]
    short VirtualStar(double JdTt, CatEntry3 Star, Accuracy Accuracy, ref double Ra, ref double Dec);

    [DispId(71)]
    void Wobble(double Tjd, double x, double y, double[] Pos1, ref double[] Pos2);

    [DispId(72)]
    double DeltaT(double Tjd);
  }
}
