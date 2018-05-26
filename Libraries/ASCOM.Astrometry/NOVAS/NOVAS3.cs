// Decompiled with JetBrains decompiler
// Type: ASCOM.Astrometry.NOVAS.NOVAS3
// Assembly: ASCOM.Astrometry, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 695EF5AA-123E-4689-AAF5-64FD1324F6BB
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.Astrometry\6.0.0.0__565de7938946fba7\ASCOM.Astrometry.dll

using ASCOM.Utilities;
using ASCOM.Utilities.Exceptions;
//
using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Text;

namespace ASCOM.Astrometry.NOVAS
{
  [ClassInterface(ClassInterfaceType.None)]
  //[ComVisible(true)]
  [Obsolete("This class will be withdrawn in the next major release, please use the SOFA or NOVAS31 classes instead")]
  //[Guid("74F604BD-6106-40ac-A821-B32F80BF3FED")]
  public class NOVAS3 : INOVAS3, IDisposable
  {
    private const string NOVAS32DLL = "NOVAS3.dll";
    private const string NOVAS64DLL = "NOVAS3-64.dll";
    private const string JPL_EPHEM_FILE_NAME = "JPLEPH";
    private const double JPL_EPHEM_START_DATE = 2305424.5;
    private const double JPL_EPHEM_END_DATE = 2525008.5;
    private const string NOVAS_DLL_LOCATION = "\\ASCOM\\Astrometry\\";
    private const string RACIO_FILE = "cio_ra.bin";
    private IntPtr Novas3DllHandle;
    private TraceLogger TL;
    private bool disposedValue;
    private const int CSIDL_PROGRAM_FILES = 38;
    private const int CSIDL_PROGRAM_FILESX86 = 42;
    private const int CSIDL_WINDOWS = 36;
    private const int CSIDL_PROGRAM_FILES_COMMONX86 = 44;

    public NOVAS3()
    {
      this.disposedValue = false;
      StringBuilder lpszPath = new StringBuilder(260);
      this.TL = new TraceLogger("", "NOVAS3");
      this.TL.Enabled = RegistryCommonCode.GetBool("Trace NOVAS", false);
      string lpFileName;
      string FName;
      string str;
      if (NOVAS3.Is64Bit())
      {
        NOVAS3.SHGetSpecialFolderPath(IntPtr.Zero, lpszPath, 44, false);
        lpFileName = lpszPath.ToString() + "\\ASCOM\\Astrometry\\NOVAS3-64.dll";
        FName = lpszPath.ToString() + "\\ASCOM\\Astrometry\\cio_ra.bin";
        str = lpszPath.ToString() + "\\ASCOM\\Astrometry\\JPLEPH";
      }
      else
      {
        lpFileName = Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles) + "\\ASCOM\\Astrometry\\NOVAS3.dll";
        FName = Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles) + "\\ASCOM\\Astrometry\\cio_ra.bin";
        str = Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles) + "\\ASCOM\\Astrometry\\JPLEPH";
      }
      this.TL.LogMessage("New", "Loading NOVAS3 library DLL: " + lpFileName);
      this.Novas3DllHandle = NOVAS3.LoadLibrary(lpFileName);
      int lastWin32Error = Marshal.GetLastWin32Error();
      if (this.Novas3DllHandle != IntPtr.Zero)
      {
        this.TL.LogMessage("New", "Loaded NOVAS3 library OK");
        this.SetRACIOFile(FName);
        string Ephem_Name = str;
        double JD_Begin = 2305424.5;
        double JD_End = 2525008.5;
        short num = this.Ephem_Open(Ephem_Name, ref JD_Begin, ref JD_End);
        if ((int) num > 0)
          throw new HelperException("Unable to open ephemeris file: " + str + ", RC: " + Conversions.ToString((int) num));
        this.TL.LogMessage("New", "NOVAS3 initialised OK");
      }
      else
      {
        this.TL.LogMessage("New", "Error loading NOVAS3 library: " + lastWin32Error.ToString("X8"));
        throw new HelperException("Error code returned from LoadLibrary when loading NOVAS3 library: " + lastWin32Error.ToString("X8"));
      }
    }

    protected virtual void Dispose(bool disposing)
    {
      if (!this.disposedValue)
      {
        int num1 = disposing ? 1 : 0;
        try
        {
          NOVAS3.FreeLibrary(this.Novas3DllHandle);
        }
        catch (Exception ex)
        {
          //ProjectData.SetProjectError(ex);
          //ProjectData.ClearProjectError();
        }
        try
        {
          int num2 = (int) this.Ephem_Close();
        }
        catch (Exception ex)
        {
          //ProjectData.SetProjectError(ex);
          //ProjectData.ClearProjectError();
        }
      }
      this.disposedValue = true;
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    public short PlanetEphemeris(ref double[] Tjd, Target Target, Target Center, ref double[] Position, ref double[] Velocity)
    {
      JDHighPrecision Tjd1 = new JDHighPrecision();
      PosVector Position1 = new PosVector();
      VelVector Velocity1 = new VelVector();
      Tjd1.JDPart1 = Tjd[0];
      Tjd1.JDPart2 = Tjd[1];
      short num = !NOVAS3.Is64Bit() ? NOVAS3.PlanetEphemeris32(ref Tjd1, Target, Center, ref Position1, ref Velocity1) : NOVAS3.PlanetEphemeris64(ref Tjd1, Target, Center, ref Position1, ref Velocity1);
      NOVAS3.PosVecToArr(Position1, ref Position);
      NOVAS3.VelVecToArr(Velocity1, ref Velocity);
      return num;
    }

    public double[] ReadEph(int Mp, string Name, double Jd, ref int Err)
    {
      double[] numArray = new double[6];
      byte[] destination = new byte[49];
      IntPtr source = !NOVAS3.Is64Bit() ? NOVAS3.ReadEph32(Mp, Name, Jd, ref Err) : NOVAS3.ReadEph64(Mp, Name, Jd, ref Err);
      if (Err == 0)
      {
        if (source != IntPtr.Zero)
        {
          Marshal.Copy(source, destination, 0, 48);
          int index = 0;
          do
          {
            numArray[index] = BitConverter.ToDouble(destination, checked (index * 8));
            checked { ++index; }
          }
          while (index <= 5);
        }
        else
        {
          int index = 0;
          do
          {
            numArray[index] = double.NaN;
            checked { ++index; }
          }
          while (index <= 5);
        }
      }
      return numArray;
    }

    public short SolarSystem(double Tjd, Body Body, Origin Origin, ref double[] Pos, ref double[] Vel)
    {
      PosVector pos = new PosVector();
      VelVector vel = new VelVector();
      short num = !NOVAS3.Is64Bit() ? NOVAS3.SolarSystem32(Tjd, checked ((short) Body), checked ((short) Origin), ref pos, ref vel) : NOVAS3.SolarSystem64(Tjd, checked ((short) Body), checked ((short) Origin), ref pos, ref vel);
      NOVAS3.PosVecToArr(pos, ref Pos);
      NOVAS3.VelVecToArr(vel, ref Vel);
      return num;
    }

    public short State(ref double[] Jed, Target Target, ref double[] TargetPos, ref double[] TargetVel)
    {
      JDHighPrecision Jed1 = new JDHighPrecision();
      PosVector TargetPos1 = new PosVector();
      VelVector TargetVel1 = new VelVector();
      Jed1.JDPart1 = Jed[0];
      Jed1.JDPart2 = Jed[1];
      short num = !NOVAS3.Is64Bit() ? NOVAS3.State32(ref Jed1, Target, ref TargetPos1, ref TargetVel1) : NOVAS3.State64(ref Jed1, Target, ref TargetPos1, ref TargetVel1);
      NOVAS3.PosVecToArr(TargetPos1, ref TargetPos);
      NOVAS3.VelVecToArr(TargetVel1, ref TargetVel);
      return num;
    }

    public void Aberration(double[] Pos, double[] Vel, double LightTime, ref double[] Pos2)
    {
      PosVector Pos2_1=new PosVector();
      if (NOVAS3.Is64Bit())
      {
        PosVector posVec = NOVAS3.ArrToPosVec(Pos);
        VelVector velVec = NOVAS3.ArrToVelVec(Vel);
        double LightTime1 = LightTime;
        NOVAS3.Aberration64(ref posVec, ref velVec, LightTime1, ref Pos2_1);
      }
      else
      {
        PosVector posVec = NOVAS3.ArrToPosVec(Pos);
        VelVector velVec = NOVAS3.ArrToVelVec(Vel);
        double LightTime1 = LightTime;
        NOVAS3.Aberration32(ref posVec, ref velVec, LightTime1, ref Pos2_1);
      }
      NOVAS3.PosVecToArr(Pos2_1, ref Pos2);
    }

    public short AppPlanet(double JdTt, Object3 SsBody, Accuracy Accuracy, ref double Ra, ref double Dec, ref double Dis)
    {
      if (NOVAS3.Is64Bit())
      {
        double JdTt1 = JdTt;
        Object3Internal SsBody1 = this.O3IFromObject3(SsBody);
        int num = (int) Accuracy;
        return NOVAS3.AppPlanet64(JdTt1, ref SsBody1, (Accuracy) num, ref Ra, ref Dec, ref Dis);
      }
      double JdTt2 = JdTt;
      Object3Internal SsBody2 = this.O3IFromObject3(SsBody);
      int num1 = (int) Accuracy;
      return NOVAS3.AppPlanet32(JdTt2, ref SsBody2, (Accuracy) num1, ref Ra, ref Dec, ref Dis);
    }

    public short AppStar(double JdTt, CatEntry3 Star, Accuracy Accuracy, ref double Ra, ref double Dec)
    {
      if (NOVAS3.Is64Bit())
        return NOVAS3.AppStar64(JdTt, ref Star, Accuracy, ref Ra, ref Dec);
      return NOVAS3.AppStar32(JdTt, ref Star, Accuracy, ref Ra, ref Dec);
    }

    public short AstroPlanet(double JdTt, Object3 SsBody, Accuracy Accuracy, ref double Ra, ref double Dec, ref double Dis)
    {
      if (NOVAS3.Is64Bit())
      {
        double JdTt1 = JdTt;
        Object3Internal SsBody1 = this.O3IFromObject3(SsBody);
        int num = (int) Accuracy;
        return NOVAS3.AstroPlanet64(JdTt1, ref SsBody1, (Accuracy) num, ref Ra, ref Dec, ref Dis);
      }
      double JdTt2 = JdTt;
      Object3Internal SsBody2 = this.O3IFromObject3(SsBody);
      int num1 = (int) Accuracy;
      return NOVAS3.AstroPlanet32(JdTt2, ref SsBody2, (Accuracy) num1, ref Ra, ref Dec, ref Dis);
    }

    public short AstroStar(double JdTt, CatEntry3 Star, Accuracy Accuracy, ref double Ra, ref double Dec)
    {
      if (NOVAS3.Is64Bit())
        return NOVAS3.AstroStar64(JdTt, ref Star, Accuracy, ref Ra, ref Dec);
      return NOVAS3.AstroStar32(JdTt, ref Star, Accuracy, ref Ra, ref Dec);
    }

    public void Bary2Obs(double[] Pos, double[] PosObs, ref double[] Pos2, ref double Lighttime)
    {
      PosVector Pos2_1 = new PosVector();
      if (NOVAS3.Is64Bit())
      {
        PosVector posVec1 = NOVAS3.ArrToPosVec(Pos);
        PosVector posVec2 = NOVAS3.ArrToPosVec(PosObs);
        NOVAS3.Bary2Obs64(ref posVec1, ref posVec2, ref Pos2_1, ref Lighttime);
        NOVAS3.PosVecToArr(Pos2_1, ref Pos2);
      }
      else
      {
        PosVector posVec1 = NOVAS3.ArrToPosVec(Pos);
        PosVector posVec2 = NOVAS3.ArrToPosVec(PosObs);
        NOVAS3.Bary2Obs32(ref posVec1, ref posVec2, ref Pos2_1, ref Lighttime);
        NOVAS3.PosVecToArr(Pos2_1, ref Pos2);
      }
    }

    public void CalDate(double Tjd, ref short Year, ref short Month, ref short Day, ref double Hour)
    {
      if (NOVAS3.Is64Bit())
        NOVAS3.CalDate64(Tjd, ref Year, ref Month, ref Day, ref Hour);
      else
        NOVAS3.CalDate32(Tjd, ref Year, ref Month, ref Day, ref Hour);
    }

    public short CelPole(double Tjd, PoleOffsetCorrection Type, double Dpole1, double Dpole2)
    {
      if (NOVAS3.Is64Bit())
        return NOVAS3.CelPole64(Tjd, Type, Dpole1, Dpole2);
      return NOVAS3.CelPole32(Tjd, Type, Dpole1, Dpole2);
    }

    public short CioArray(double JdTdb, int NPts, ref ArrayList Cio)
    {
      RAOfCioArray Cio1 = new RAOfCioArray();
      Cio1.Initialise();
      short num = !NOVAS3.Is64Bit() ? NOVAS3.CioArray32(JdTdb, NPts, ref Cio1) : NOVAS3.CioArray64(JdTdb, NPts, ref Cio1);
      NOVAS3.RACioArrayStructureToArr(Cio1, ref Cio);
      return num;
    }

    public short CioBasis(double JdTdbEquionx, double RaCioEquionx, ReferenceSystem RefSys, Accuracy Accuracy, ref double x, ref double y, ref double z)
    {
      if (NOVAS3.Is64Bit())
        return NOVAS3.CioBasis64(JdTdbEquionx, RaCioEquionx, RefSys, Accuracy, ref x, ref y, ref z);
      return NOVAS3.CioBasis32(JdTdbEquionx, RaCioEquionx, RefSys, Accuracy, ref x, ref y, ref z);
    }

    public short CioLocation(double JdTdb, Accuracy Accuracy, ref double RaCio, ref ReferenceSystem RefSys)
    {
      if (NOVAS3.Is64Bit())
        return NOVAS3.CioLocation64(JdTdb, Accuracy, ref RaCio, ref RefSys);
      return NOVAS3.CioLocation32(JdTdb, Accuracy, ref RaCio, ref RefSys);
    }

    public short CioRa(double JdTt, Accuracy Accuracy, ref double RaCio)
    {
      if (NOVAS3.Is64Bit())
        return NOVAS3.CioRa64(JdTt, Accuracy, ref RaCio);
      return NOVAS3.CioRa32(JdTt, Accuracy, ref RaCio);
    }

    public double DLight(double[] Pos1, double[] PosObs)
    {
      if (NOVAS3.Is64Bit())
      {
        PosVector posVec1 = NOVAS3.ArrToPosVec(Pos1);
        PosVector posVec2 = NOVAS3.ArrToPosVec(PosObs);
        return NOVAS3.DLight64(ref posVec1, ref posVec2);
      }
      PosVector posVec3 = NOVAS3.ArrToPosVec(Pos1);
      PosVector posVec4 = NOVAS3.ArrToPosVec(PosObs);
      return NOVAS3.DLight32(ref posVec3, ref posVec4);
    }

    public short Ecl2EquVec(double JdTt, CoordSys CoordSys, Accuracy Accuracy, double[] Pos1, ref double[] Pos2)
    {
      PosVector Pos2_1 = new PosVector();
      short num1;
      if (NOVAS3.Is64Bit())
      {
        double JdTt1 = JdTt;
        int num2 = (int) CoordSys;
        int num3 = (int) Accuracy;
        PosVector posVec = NOVAS3.ArrToPosVec(Pos1);
        num1 = NOVAS3.Ecl2EquVec64(JdTt1, (CoordSys) num2, (Accuracy) num3, ref posVec, ref Pos2_1);
      }
      else
      {
        double JdTt1 = JdTt;
        int num2 = (int) CoordSys;
        int num3 = (int) Accuracy;
        PosVector posVec = NOVAS3.ArrToPosVec(Pos1);
        num1 = NOVAS3.Ecl2EquVec32(JdTt1, (CoordSys) num2, (Accuracy) num3, ref posVec, ref Pos2_1);
      }
      NOVAS3.PosVecToArr(Pos2_1, ref Pos2);
      return num1;
    }

    public double EeCt(double JdHigh, double JdLow, Accuracy Accuracy)
    {
      if (NOVAS3.Is64Bit())
        return NOVAS3.EeCt64(JdHigh, JdLow, Accuracy);
      return NOVAS3.EeCt32(JdHigh, JdLow, Accuracy);
    }

    public short Ephemeris(double[] Jd, Object3 CelObj, Origin Origin, Accuracy Accuracy, ref double[] Pos, ref double[] Vel)
    {
      PosVector Pos1 = new PosVector();
      VelVector Vel1 = new VelVector();
      JDHighPrecision Jd1;
      Jd1.JDPart1 = Jd[0];
      Jd1.JDPart2 = Jd[1];
      short num1;
      if (NOVAS3.Is64Bit())
      {
        Object3Internal CelObj1 = this.O3IFromObject3(CelObj);
        int num2 = (int) Origin;
        int num3 = (int) Accuracy;
        num1 = NOVAS3.Ephemeris64(ref Jd1, ref CelObj1, (Origin) num2, (Accuracy) num3, ref Pos1, ref Vel1);
      }
      else
      {
        Object3Internal CelObj1 = this.O3IFromObject3(CelObj);
        int num2 = (int) Origin;
        int num3 = (int) Accuracy;
        num1 = NOVAS3.Ephemeris32(ref Jd1, ref CelObj1, (Origin) num2, (Accuracy) num3, ref Pos1, ref Vel1);
      }
      NOVAS3.PosVecToArr(Pos1, ref Pos);
      NOVAS3.VelVecToArr(Vel1, ref Vel);
      return num1;
    }

    public short Equ2Ecl(double JdTt, CoordSys CoordSys, Accuracy Accuracy, double Ra, double Dec, ref double ELon, ref double ELat)
    {
      if (NOVAS3.Is64Bit())
        return NOVAS3.Equ2Ecl64(JdTt, CoordSys, Accuracy, Ra, Dec, ref ELon, ref ELat);
      return NOVAS3.Equ2Ecl32(JdTt, CoordSys, Accuracy, Ra, Dec, ref ELon, ref ELat);
    }

    public short Equ2EclVec(double JdTt, CoordSys CoordSys, Accuracy Accuracy, double[] Pos1, ref double[] Pos2)
    {
      PosVector Pos2_1 = new PosVector();
      short num1;
      if (NOVAS3.Is64Bit())
      {
        double JdTt1 = JdTt;
        int num2 = (int) CoordSys;
        int num3 = (int) Accuracy;
        PosVector posVec = NOVAS3.ArrToPosVec(Pos1);
        num1 = NOVAS3.Equ2EclVec64(JdTt1, (CoordSys) num2, (Accuracy) num3, ref posVec, ref Pos2_1);
      }
      else
      {
        double JdTt1 = JdTt;
        int num2 = (int) CoordSys;
        int num3 = (int) Accuracy;
        PosVector posVec = NOVAS3.ArrToPosVec(Pos1);
        num1 = NOVAS3.Equ2EclVec32(JdTt1, (CoordSys) num2, (Accuracy) num3, ref posVec, ref Pos2_1);
      }
      NOVAS3.PosVecToArr(Pos2_1, ref Pos2);
      return num1;
    }

    public void Equ2Gal(double RaI, double DecI, ref double GLon, ref double GLat)
    {
      if (NOVAS3.Is64Bit())
        NOVAS3.Equ2Gal64(RaI, DecI, ref GLon, ref GLat);
      else
        NOVAS3.Equ2Gal32(RaI, DecI, ref GLon, ref GLat);
    }

    public void Equ2Hor(double Jd_Ut1, double DeltT, Accuracy Accuracy, double x, double y, OnSurface Location, double Ra, double Dec, RefractionOption RefOption, ref double Zd, ref double Az, ref double RaR, ref double DecR)
    {
      if (NOVAS3.Is64Bit())
        NOVAS3.Equ2Hor64(Jd_Ut1, DeltT, Accuracy, x, y, ref Location, Ra, Dec, RefOption, ref Zd, ref Az, ref RaR, ref DecR);
      else
        NOVAS3.Equ2Hor32(Jd_Ut1, DeltT, Accuracy, x, y, ref Location, Ra, Dec, RefOption, ref Zd, ref Az, ref RaR, ref DecR);
    }

    public double Era(double JdHigh, double JdLow)
    {
      if (NOVAS3.Is64Bit())
        return NOVAS3.Era64(JdHigh, JdLow);
      return NOVAS3.Era32(JdHigh, JdLow);
    }

    public void ETilt(double JdTdb, Accuracy Accuracy, ref double Mobl, ref double Tobl, ref double Ee, ref double Dpsi, ref double Deps)
    {
      if (NOVAS3.Is64Bit())
        NOVAS3.ETilt64(JdTdb, Accuracy, ref Mobl, ref Tobl, ref Ee, ref Dpsi, ref Deps);
      else
        NOVAS3.ETilt32(JdTdb, Accuracy, ref Mobl, ref Tobl, ref Ee, ref Dpsi, ref Deps);
    }

    public void FrameTie(double[] Pos1, FrameConversionDirection Direction, ref double[] Pos2)
    {
      PosVector Pos2_1 = new PosVector();
      if (NOVAS3.Is64Bit())
      {
        PosVector posVec = NOVAS3.ArrToPosVec(Pos1);
        NOVAS3.FrameTie64(ref posVec, Direction, ref Pos2_1);
      }
      else
      {
        PosVector posVec = NOVAS3.ArrToPosVec(Pos1);
        NOVAS3.FrameTie32(ref posVec, Direction, ref Pos2_1);
      }
      NOVAS3.PosVecToArr(Pos2_1, ref Pos2);
    }

    public void FundArgs(double t, ref double[] a)
    {
      FundamentalArgs a1 = new FundamentalArgs();
      if (NOVAS3.Is64Bit())
        NOVAS3.FundArgs64(t, ref a1);
      else
        NOVAS3.FundArgs32(t, ref a1);
      a[0] = a1.l;
      a[1] = a1.ldash;
      a[2] = a1.F;
      a[3] = a1.D;
      a[4] = a1.Omega;
    }

    public short Gcrs2Equ(double JdTt, CoordSys CoordSys, Accuracy Accuracy, double RaG, double DecG, ref double Ra, ref double Dec)
    {
      if (NOVAS3.Is64Bit())
        return NOVAS3.Gcrs2Equ64(JdTt, CoordSys, Accuracy, RaG, DecG, ref Ra, ref Dec);
      return NOVAS3.Gcrs2Equ32(JdTt, CoordSys, Accuracy, RaG, DecG, ref Ra, ref Dec);
    }

    public short GeoPosVel(double JdTt, double DeltaT, Accuracy Accuracy, Observer Obs, ref double[] Pos, ref double[] Vel)
    {
      PosVector Pos1 = new PosVector();
      VelVector Vel1 = new VelVector();
      short num = !NOVAS3.Is64Bit() ? NOVAS3.GeoPosVel32(JdTt, DeltaT, Accuracy, ref Obs, ref Pos1, ref Vel1) : NOVAS3.GeoPosVel64(JdTt, DeltaT, Accuracy, ref Obs, ref Pos1, ref Vel1);
      NOVAS3.PosVecToArr(Pos1, ref Pos);
      NOVAS3.VelVecToArr(Vel1, ref Vel);
      return num;
    }

    public short GravDef(double JdTdb, EarthDeflection LocCode, Accuracy Accuracy, double[] Pos1, double[] PosObs, ref double[] Pos2)
    {
      PosVector Pos2_1 = new PosVector();
      short num1;
      if (NOVAS3.Is64Bit())
      {
        double JdTdb1 = JdTdb;
        int num2 = (int) LocCode;
        int num3 = (int) Accuracy;
        PosVector posVec1 = NOVAS3.ArrToPosVec(Pos1);
        PosVector posVec2 = NOVAS3.ArrToPosVec(PosObs);
        num1 = NOVAS3.GravDef64(JdTdb1, (EarthDeflection) num2, (Accuracy) num3, ref posVec1, ref posVec2, ref Pos2_1);
      }
      else
      {
        double JdTdb1 = JdTdb;
        int num2 = (int) LocCode;
        int num3 = (int) Accuracy;
        PosVector posVec1 = NOVAS3.ArrToPosVec(Pos1);
        PosVector posVec2 = NOVAS3.ArrToPosVec(PosObs);
        num1 = NOVAS3.GravDef32(JdTdb1, (EarthDeflection) num2, (Accuracy) num3, ref posVec1, ref posVec2, ref Pos2_1);
      }
      NOVAS3.PosVecToArr(Pos2_1, ref Pos2);
      return num1;
    }

    public void GravVec(double[] Pos1, double[] PosObs, double[] PosBody, double RMass, ref double[] Pos2)
    {
      PosVector Pos2_1 = new PosVector();
      if (NOVAS3.Is64Bit())
      {
        PosVector posVec1 = NOVAS3.ArrToPosVec(Pos1);
        PosVector posVec2 = NOVAS3.ArrToPosVec(PosObs);
        PosVector posVec3 = NOVAS3.ArrToPosVec(PosBody);
        double RMass1 = RMass;
        NOVAS3.GravVec64(ref posVec1, ref posVec2, ref posVec3, RMass1, ref Pos2_1);
      }
      else
      {
        PosVector posVec1 = NOVAS3.ArrToPosVec(Pos1);
        PosVector posVec2 = NOVAS3.ArrToPosVec(PosObs);
        PosVector posVec3 = NOVAS3.ArrToPosVec(PosBody);
        double RMass1 = RMass;
        NOVAS3.GravVec32(ref posVec1, ref posVec2, ref posVec3, RMass1, ref Pos2_1);
      }
      NOVAS3.PosVecToArr(Pos2_1, ref Pos2);
    }

    public double IraEquinox(double JdTdb, EquinoxType Equinox, Accuracy Accuracy)
    {
      if (NOVAS3.Is64Bit())
        return NOVAS3.IraEquinox64(JdTdb, Equinox, Accuracy);
      return NOVAS3.IraEquinox32(JdTdb, Equinox, Accuracy);
    }

    public double JulianDate(short Year, short Month, short Day, double Hour)
    {
      if (NOVAS3.Is64Bit())
        return NOVAS3.JulianDate64(Year, Month, Day, Hour);
      return NOVAS3.JulianDate32(Year, Month, Day, Hour);
    }

    public short LightTime(double JdTdb, Object3 SsObject, double[] PosObs, double TLight0, Accuracy Accuracy, ref double[] Pos, ref double TLight)
    {
      PosVector Pos1 = new PosVector();
      short num1;
      if (NOVAS3.Is64Bit())
      {
        double JdTdb1 = JdTdb;
        Object3Internal SsObject1 = this.O3IFromObject3(SsObject);
        PosVector posVec = NOVAS3.ArrToPosVec(PosObs);
        double TLight0_1 = TLight0;
        int num2 = (int) Accuracy;
        num1 = NOVAS3.LightTime64(JdTdb1, ref SsObject1, ref posVec, TLight0_1, (Accuracy) num2, ref Pos1, ref TLight);
      }
      else
      {
        double JdTdb1 = JdTdb;
        Object3Internal SsObject1 = this.O3IFromObject3(SsObject);
        PosVector posVec = NOVAS3.ArrToPosVec(PosObs);
        double TLight0_1 = TLight0;
        int num2 = (int) Accuracy;
        num1 = NOVAS3.LightTime32(JdTdb1, ref SsObject1, ref posVec, TLight0_1, (Accuracy) num2, ref Pos1, ref TLight);
      }
      NOVAS3.PosVecToArr(Pos1, ref Pos);
      return num1;
    }

    public void LimbAngle(double[] PosObj, double[] PosObs, ref double LimbAng, ref double NadirAng)
    {
      if (NOVAS3.Is64Bit())
      {
        PosVector posVec1 = NOVAS3.ArrToPosVec(PosObj);
        PosVector posVec2 = NOVAS3.ArrToPosVec(PosObs);
        NOVAS3.LimbAngle64(ref posVec1, ref posVec2, ref LimbAng, ref NadirAng);
      }
      else
      {
        PosVector posVec1 = NOVAS3.ArrToPosVec(PosObj);
        PosVector posVec2 = NOVAS3.ArrToPosVec(PosObs);
        NOVAS3.LimbAngle32(ref posVec1, ref posVec2, ref LimbAng, ref NadirAng);
      }
    }

    public short LocalPlanet(double JdTt, Object3 SsBody, double DeltaT, OnSurface Position, Accuracy Accuracy, ref double Ra, ref double Dec, ref double Dis)
    {
      if (NOVAS3.Is64Bit())
      {
        double JdTt1 = JdTt;
        Object3Internal SsBody1 = this.O3IFromObject3(SsBody);
        double DeltaT1 = DeltaT;
        int num = (int) Accuracy;
        return NOVAS3.LocalPlanet64(JdTt1, ref SsBody1, DeltaT1, ref Position, (Accuracy) num, ref Ra, ref Dec, ref Dis);
      }
      double JdTt2 = JdTt;
      Object3Internal SsBody2 = this.O3IFromObject3(SsBody);
      double DeltaT2 = DeltaT;
      int num1 = (int) Accuracy;
      return NOVAS3.LocalPlanet32(JdTt2, ref SsBody2, DeltaT2, ref Position, (Accuracy) num1, ref Ra, ref Dec, ref Dis);
    }

    public short LocalStar(double JdTt, double DeltaT, CatEntry3 Star, OnSurface Position, Accuracy Accuracy, ref double Ra, ref double Dec)
    {
      if (NOVAS3.Is64Bit())
        return NOVAS3.LocalStar64(JdTt, DeltaT, ref Star, ref Position, Accuracy, ref Ra, ref Dec);
      return NOVAS3.LocalStar32(JdTt, DeltaT, ref Star, ref Position, Accuracy, ref Ra, ref Dec);
    }

    public void MakeCatEntry(string StarName, string Catalog, int StarNum, double Ra, double Dec, double PmRa, double PmDec, double Parallax, double RadVel, ref CatEntry3 Star)
    {
      if (NOVAS3.Is64Bit())
        NOVAS3.MakeCatEntry64(StarName, Catalog, StarNum, Ra, Dec, PmRa, PmDec, Parallax, RadVel, ref Star);
      else
        NOVAS3.MakeCatEntry32(StarName, Catalog, StarNum, Ra, Dec, PmRa, PmDec, Parallax, RadVel, ref Star);
    }

    public void MakeInSpace(double[] ScPos, double[] ScVel, ref InSpace ObsSpace)
    {
      if (NOVAS3.Is64Bit())
      {
        PosVector posVec = NOVAS3.ArrToPosVec(ScPos);
        VelVector velVec = NOVAS3.ArrToVelVec(ScVel);
        NOVAS3.MakeInSpace64(ref posVec, ref velVec, ref ObsSpace);
      }
      else
      {
        PosVector posVec = NOVAS3.ArrToPosVec(ScPos);
        VelVector velVec = NOVAS3.ArrToVelVec(ScVel);
        NOVAS3.MakeInSpace32(ref posVec, ref velVec, ref ObsSpace);
      }
    }

    public short MakeObject(ASCOM.Astrometry.ObjectType Type, short Number, string Name, CatEntry3 StarData, ref Object3 CelObj)
    {
      Object3Internal CelObj1 = new Object3Internal();
      short num = !NOVAS3.Is64Bit() ? NOVAS3.MakeObject32(Type, Number, Name, ref StarData, ref CelObj1) : NOVAS3.MakeObject64(Type, Number, Name, ref StarData, ref CelObj1);
      this.O3FromO3Internal(CelObj1, ref CelObj);
      return num;
    }

    public short MakeObserver(ObserverLocation Where, OnSurface ObsSurface, InSpace ObsSpace, ref Observer Obs)
    {
      if (NOVAS3.Is64Bit())
        return NOVAS3.MakeObserver64(Where, ref ObsSurface, ref ObsSpace, ref Obs);
      return NOVAS3.MakeObserver32(Where, ref ObsSurface, ref ObsSpace, ref Obs);
    }

    public void MakeObserverAtGeocenter(ref Observer ObsAtGeocenter)
    {
      if (NOVAS3.Is64Bit())
        NOVAS3.MakeObserverAtGeocenter64(ref ObsAtGeocenter);
      else
        NOVAS3.MakeObserverAtGeocenter32(ref ObsAtGeocenter);
    }

    public void MakeObserverInSpace(double[] ScPos, double[] ScVel, ref Observer ObsInSpace)
    {
      if (NOVAS3.Is64Bit())
      {
        PosVector posVec = NOVAS3.ArrToPosVec(ScPos);
        VelVector velVec = NOVAS3.ArrToVelVec(ScVel);
        NOVAS3.MakeObserverInSpace64(ref posVec, ref velVec, ref ObsInSpace);
      }
      else
      {
        PosVector posVec = NOVAS3.ArrToPosVec(ScPos);
        VelVector velVec = NOVAS3.ArrToVelVec(ScVel);
        NOVAS3.MakeObserverInSpace32(ref posVec, ref velVec, ref ObsInSpace);
      }
    }

    public void MakeObserverOnSurface(double Latitude, double Longitude, double Height, double Temperature, double Pressure, ref Observer ObsOnSurface)
    {
      if (NOVAS3.Is64Bit())
        NOVAS3.MakeObserverOnSurface64(Latitude, Longitude, Height, Temperature, Pressure, ref ObsOnSurface);
      else
        NOVAS3.MakeObserverOnSurface32(Latitude, Longitude, Height, Temperature, Pressure, ref ObsOnSurface);
    }

    public void MakeOnSurface(double Latitude, double Longitude, double Height, double Temperature, double Pressure, ref OnSurface ObsSurface)
    {
      if (NOVAS3.Is64Bit())
        NOVAS3.MakeOnSurface64(Latitude, Longitude, Height, Temperature, Pressure, ref ObsSurface);
      else
        NOVAS3.MakeOnSurface32(Latitude, Longitude, Height, Temperature, Pressure, ref ObsSurface);
    }

    public double MeanObliq(double JdTdb)
    {
      if (NOVAS3.Is64Bit())
        return NOVAS3.MeanObliq64(JdTdb);
      return NOVAS3.MeanObliq32(JdTdb);
    }

    public short MeanStar(double JdTt, double Ra, double Dec, Accuracy Accuracy, ref double IRa, ref double IDec)
    {
      if (NOVAS3.Is64Bit())
        return NOVAS3.MeanStar64(JdTt, Ra, Dec, Accuracy, ref IRa, ref IDec);
      return NOVAS3.MeanStar32(JdTt, Ra, Dec, Accuracy, ref IRa, ref IDec);
    }

    public double NormAng(double Angle)
    {
      if (NOVAS3.Is64Bit())
        return NOVAS3.NormAng64(Angle);
      return NOVAS3.NormAng32(Angle);
    }

    public void Nutation(double JdTdb, NutationDirection Direction, Accuracy Accuracy, double[] Pos, ref double[] Pos2)
    {
      PosVector Pos2_1 = new PosVector();
      if (NOVAS3.Is64Bit())
      {
        double JdTdb1 = JdTdb;
        int num1 = (int) Direction;
        int num2 = (int) Accuracy;
        PosVector posVec = NOVAS3.ArrToPosVec(Pos);
        NOVAS3.Nutation64(JdTdb1, (NutationDirection) num1, (Accuracy) num2, ref posVec, ref Pos2_1);
      }
      else
      {
        double JdTdb1 = JdTdb;
        int num1 = (int) Direction;
        int num2 = (int) Accuracy;
        PosVector posVec = NOVAS3.ArrToPosVec(Pos);
        NOVAS3.Nutation32(JdTdb1, (NutationDirection) num1, (Accuracy) num2, ref posVec, ref Pos2_1);
      }
      NOVAS3.PosVecToArr(Pos2_1, ref Pos2);
    }

    public void NutationAngles(double t, Accuracy Accuracy, ref double DPsi, ref double DEps)
    {
      if (NOVAS3.Is64Bit())
        NOVAS3.NutationAngles64(t, Accuracy, ref DPsi, ref DEps);
      else
        NOVAS3.NutationAngles32(t, Accuracy, ref DPsi, ref DEps);
    }

    public short Place(double JdTt, Object3 CelObject, Observer Location, double DeltaT, CoordSys CoordSys, Accuracy Accuracy, ref SkyPos Output)
    {
      if (NOVAS3.Is64Bit())
      {
        double JdTt1 = JdTt;
        Object3Internal CelObject1 = this.O3IFromObject3(CelObject);
        double DeltaT1 = DeltaT;
        int num1 = (int) CoordSys;
        int num2 = (int) Accuracy;
        return NOVAS3.Place64(JdTt1, ref CelObject1, ref Location, DeltaT1, (CoordSys) num1, (Accuracy) num2, ref Output);
      }
      double JdTt2 = JdTt;
      Object3Internal CelObject2 = this.O3IFromObject3(CelObject);
      double DeltaT2 = DeltaT;
      int num3 = (int) CoordSys;
      int num4 = (int) Accuracy;
      return NOVAS3.Place32(JdTt2, ref CelObject2, ref Location, DeltaT2, (CoordSys) num3, (Accuracy) num4, ref Output);
    }

    public short Precession(double JdTdb1, double[] Pos1, double JdTdb2, ref double[] Pos2)
    {
      PosVector Pos2_1 = new PosVector();
      short num;
      if (NOVAS3.Is64Bit())
      {
        double JdTdb1_1 = JdTdb1;
        PosVector posVec = NOVAS3.ArrToPosVec(Pos1);
        double JdTdb2_1 = JdTdb2;
        num = NOVAS3.Precession64(JdTdb1_1, ref posVec, JdTdb2_1, ref Pos2_1);
      }
      else
      {
        double JdTdb1_1 = JdTdb1;
        PosVector posVec = NOVAS3.ArrToPosVec(Pos1);
        double JdTdb2_1 = JdTdb2;
        num = NOVAS3.Precession32(JdTdb1_1, ref posVec, JdTdb2_1, ref Pos2_1);
      }
      NOVAS3.PosVecToArr(Pos2_1, ref Pos2);
      return num;
    }

    public void ProperMotion(double JdTdb1, double[] Pos, double[] Vel, double JdTdb2, ref double[] Pos2)
    {
      PosVector Pos2_1 = new PosVector();
      if (NOVAS3.Is64Bit())
      {
        double JdTdb1_1 = JdTdb1;
        PosVector posVec = NOVAS3.ArrToPosVec(Pos);
        VelVector velVec = NOVAS3.ArrToVelVec(Vel);
        double JdTdb2_1 = JdTdb2;
        NOVAS3.ProperMotion64(JdTdb1_1, ref posVec, ref velVec, JdTdb2_1, ref Pos2_1);
      }
      else
      {
        double JdTdb1_1 = JdTdb1;
        PosVector posVec = NOVAS3.ArrToPosVec(Pos);
        VelVector velVec = NOVAS3.ArrToVelVec(Vel);
        double JdTdb2_1 = JdTdb2;
        NOVAS3.ProperMotion32(JdTdb1_1, ref posVec, ref velVec, JdTdb2_1, ref Pos2_1);
      }
      NOVAS3.PosVecToArr(Pos2_1, ref Pos2);
    }

    public void RaDec2Vector(double Ra, double Dec, double Dist, ref double[] Vector)
    {
      PosVector Vector1 = new PosVector();
      if (NOVAS3.Is64Bit())
        NOVAS3.RaDec2Vector64(Ra, Dec, Dist, ref Vector1);
      else
        NOVAS3.RaDec2Vector32(Ra, Dec, Dist, ref Vector1);
      NOVAS3.PosVecToArr(Vector1, ref Vector);
    }

    public void RadVel(Object3 CelObject, double[] Pos, double[] Vel, double[] VelObs, double DObsGeo, double DObsSun, double DObjSun, ref double Rv)
    {
      if (NOVAS3.Is64Bit())
      {
        Object3Internal CelObject1 = this.O3IFromObject3(CelObject);
        PosVector posVec = NOVAS3.ArrToPosVec(Pos);
        VelVector velVec1 = NOVAS3.ArrToVelVec(Vel);
        VelVector velVec2 = NOVAS3.ArrToVelVec(VelObs);
        double DObsGeo1 = DObsGeo;
        double DObsSun1 = DObsSun;
        double DObjSun1 = DObjSun;
        NOVAS3.RadVel64(ref CelObject1, ref posVec, ref velVec1, ref velVec2, DObsGeo1, DObsSun1, DObjSun1, ref Rv);
      }
      else
      {
        Object3Internal CelObject1 = this.O3IFromObject3(CelObject);
        PosVector posVec = NOVAS3.ArrToPosVec(Pos);
        VelVector velVec1 = NOVAS3.ArrToVelVec(Vel);
        VelVector velVec2 = NOVAS3.ArrToVelVec(VelObs);
        double DObsGeo1 = DObsGeo;
        double DObsSun1 = DObsSun;
        double DObjSun1 = DObjSun;
        NOVAS3.RadVel32(ref CelObject1, ref posVec, ref velVec1, ref velVec2, DObsGeo1, DObsSun1, DObjSun1, ref Rv);
      }
    }

    public double Refract(OnSurface Location, RefractionOption RefOption, double ZdObs)
    {
      if (NOVAS3.Is64Bit())
        return NOVAS3.Refract64(ref Location, RefOption, ZdObs);
      return NOVAS3.Refract32(ref Location, RefOption, ZdObs);
    }

    public short SiderealTime(double JdHigh, double JdLow, double DeltaT, GstType GstType, ASCOM.Astrometry.Method Method, Accuracy Accuracy, ref double Gst)
    {
      if (NOVAS3.Is64Bit())
        return NOVAS3.SiderealTime64(JdHigh, JdLow, DeltaT, GstType, Method, Accuracy, ref Gst);
      return NOVAS3.SiderealTime32(JdHigh, JdLow, DeltaT, GstType, Method, Accuracy, ref Gst);
    }

    public void Spin(double Angle, double[] Pos1, ref double[] Pos2)
    {
      PosVector Pos2_1 = new PosVector();
      if (NOVAS3.Is64Bit())
      {
        double Angle1 = Angle;
        PosVector posVec = NOVAS3.ArrToPosVec(Pos1);
        NOVAS3.Spin64(Angle1, ref posVec, ref Pos2_1);
      }
      else
      {
        double Angle1 = Angle;
        PosVector posVec = NOVAS3.ArrToPosVec(Pos1);
        NOVAS3.Spin32(Angle1, ref posVec, ref Pos2_1);
      }
      NOVAS3.PosVecToArr(Pos2_1, ref Pos2);
    }

    public void StarVectors(CatEntry3 Star, ref double[] Pos, ref double[] Vel)
    {
      PosVector Pos1 = new PosVector();
      VelVector Vel1 = new VelVector();
      if (NOVAS3.Is64Bit())
        NOVAS3.StarVectors64(ref Star, ref Pos1, ref Vel1);
      else
        NOVAS3.StarVectors32(ref Star, ref Pos1, ref Vel1);
      NOVAS3.PosVecToArr(Pos1, ref Pos);
      NOVAS3.VelVecToArr(Vel1, ref Vel);
    }

    public void Tdb2Tt(double TdbJd, ref double TtJd, ref double SecDiff)
    {
      if (NOVAS3.Is64Bit())
        NOVAS3.Tdb2Tt64(TdbJd, ref TtJd, ref SecDiff);
      else
        NOVAS3.Tdb2Tt32(TdbJd, ref TtJd, ref SecDiff);
    }

    public short Ter2Cel(double JdHigh, double JdLow, double DeltaT, ASCOM.Astrometry.Method Method, Accuracy Accuracy, OutputVectorOption OutputOption, double x, double y, double[] VecT, ref double[] VecC)
    {
      PosVector VecC1 = new PosVector();
      short num1;
      if (NOVAS3.Is64Bit())
      {
        double JdHigh1 = JdHigh;
        double JdLow1 = JdLow;
        double DeltaT1 = DeltaT;
        int num2 = (int) Method;
        int num3 = (int) Accuracy;
        int num4 = (int) OutputOption;
        double x1 = x;
        double y1 = y;
        PosVector posVec = NOVAS3.ArrToPosVec(VecT);
        num1 = NOVAS3.Ter2Cel64(JdHigh1, JdLow1, DeltaT1, (ASCOM.Astrometry.Method) num2, (Accuracy) num3, (OutputVectorOption) num4, x1, y1, ref posVec, ref VecC1);
      }
      else
      {
        double JdHigh1 = JdHigh;
        double JdLow1 = JdLow;
        double DeltaT1 = DeltaT;
        int num2 = (int) Method;
        int num3 = (int) Accuracy;
        int num4 = (int) OutputOption;
        double x1 = x;
        double y1 = y;
        PosVector posVec = NOVAS3.ArrToPosVec(VecT);
        num1 = NOVAS3.Ter2Cel32(JdHigh1, JdLow1, DeltaT1, (ASCOM.Astrometry.Method) num2, (Accuracy) num3, (OutputVectorOption) num4, x1, y1, ref posVec, ref VecC1);
      }
      NOVAS3.PosVecToArr(VecC1, ref VecC);
      return num1;
    }

    public void Terra(OnSurface Location, double St, ref double[] Pos, ref double[] Vel)
    {
      PosVector Pos1 = new PosVector();
      VelVector Vel1 = new VelVector();
      if (NOVAS3.Is64Bit())
        NOVAS3.Terra64(ref Location, St, ref Pos1, ref Vel1);
      else
        NOVAS3.Terra32(ref Location, St, ref Pos1, ref Vel1);
      NOVAS3.PosVecToArr(Pos1, ref Pos);
      NOVAS3.VelVecToArr(Vel1, ref Vel);
    }

    public short TopoPlanet(double JdTt, Object3 SsBody, double DeltaT, OnSurface Position, Accuracy Accuracy, ref double Ra, ref double Dec, ref double Dis)
    {
      if (NOVAS3.Is64Bit())
      {
        double JdTt1 = JdTt;
        Object3Internal SsBody1 = this.O3IFromObject3(SsBody);
        double DeltaT1 = DeltaT;
        int num = (int) Accuracy;
        return NOVAS3.TopoPlanet64(JdTt1, ref SsBody1, DeltaT1, ref Position, (Accuracy) num, ref Ra, ref Dec, ref Dis);
      }
      double JdTt2 = JdTt;
      Object3Internal SsBody2 = this.O3IFromObject3(SsBody);
      double DeltaT2 = DeltaT;
      int num1 = (int) Accuracy;
      return NOVAS3.TopoPlanet32(JdTt2, ref SsBody2, DeltaT2, ref Position, (Accuracy) num1, ref Ra, ref Dec, ref Dis);
    }

    public short TopoStar(double JdTt, double DeltaT, CatEntry3 Star, OnSurface Position, Accuracy Accuracy, ref double Ra, ref double Dec)
    {
      if (NOVAS3.Is64Bit())
        return NOVAS3.TopoStar64(JdTt, DeltaT, ref Star, ref Position, Accuracy, ref Ra, ref Dec);
      return NOVAS3.TopoStar32(JdTt, DeltaT, ref Star, ref Position, Accuracy, ref Ra, ref Dec);
    }

    public short TransformCat(TransformationOption3 TransformOption, double DateInCat, CatEntry3 InCat, double DateNewCat, string NewCatId, ref CatEntry3 NewCat)
    {
      if (NOVAS3.Is64Bit())
        return NOVAS3.TransformCat64(TransformOption, DateInCat, ref InCat, DateNewCat, NewCatId, ref NewCat);
      return NOVAS3.TransformCat32(TransformOption, DateInCat, ref InCat, DateNewCat, NewCatId, ref NewCat);
    }

    public void TransformHip(CatEntry3 Hipparcos, ref CatEntry3 Hip2000)
    {
      if (NOVAS3.Is64Bit())
        NOVAS3.TransformHip64(ref Hipparcos, ref Hip2000);
      else
        NOVAS3.TransformHip32(ref Hipparcos, ref Hip2000);
    }

    public short Vector2RaDec(double[] Pos, ref double Ra, ref double Dec)
    {
      if (NOVAS3.Is64Bit())
      {
        PosVector posVec = NOVAS3.ArrToPosVec(Pos);
        return NOVAS3.Vector2RaDec64(ref posVec, ref Ra, ref Dec);
      }
      PosVector posVec1 = NOVAS3.ArrToPosVec(Pos);
      return NOVAS3.Vector2RaDec32(ref posVec1, ref Ra, ref Dec);
    }

    public short VirtualPlanet(double JdTt, Object3 SsBody, Accuracy Accuracy, ref double Ra, ref double Dec, ref double Dis)
    {
      if (NOVAS3.Is64Bit())
      {
        double JdTt1 = JdTt;
        Object3Internal SsBody1 = this.O3IFromObject3(SsBody);
        int num = (int) Accuracy;
        return NOVAS3.VirtualPlanet64(JdTt1, ref SsBody1, (Accuracy) num, ref Ra, ref Dec, ref Dis);
      }
      double JdTt2 = JdTt;
      Object3Internal SsBody2 = this.O3IFromObject3(SsBody);
      int num1 = (int) Accuracy;
      return NOVAS3.VirtualPlanet32(JdTt2, ref SsBody2, (Accuracy) num1, ref Ra, ref Dec, ref Dis);
    }

    public short VirtualStar(double JdTt, CatEntry3 Star, Accuracy Accuracy, ref double Ra, ref double Dec)
    {
      if (NOVAS3.Is64Bit())
        return NOVAS3.VirtualStar64(JdTt, ref Star, Accuracy, ref Ra, ref Dec);
      return NOVAS3.VirtualStar32(JdTt, ref Star, Accuracy, ref Ra, ref Dec);
    }

    public void Wobble(double Tjd, double x, double y, double[] Pos1, ref double[] Pos2)
    {
      PosVector Pos2_1 = new PosVector();
      if (NOVAS3.Is64Bit())
      {
        double Tjd1 = Tjd;
        double x1 = x;
        double y1 = y;
        PosVector posVec = NOVAS3.ArrToPosVec(Pos1);
        NOVAS3.Wobble64(Tjd1, x1, y1, ref posVec, ref Pos2_1);
      }
      else
      {
        double Tjd1 = Tjd;
        double x1 = x;
        double y1 = y;
        PosVector posVec = NOVAS3.ArrToPosVec(Pos1);
        NOVAS3.Wobble32(Tjd1, x1, y1, ref posVec, ref Pos2_1);
      }
      NOVAS3.PosVecToArr(Pos2_1, ref Pos2);
    }

    private short Ephem_Open(string Ephem_Name, ref double JD_Begin, ref double JD_End)
    {
      return !NOVAS3.Is64Bit() ? NOVAS3.EphemOpen32(Ephem_Name, ref JD_Begin, ref JD_End) : NOVAS3.EphemOpen64(Ephem_Name, ref JD_Begin, ref JD_End);
    }

    private short Ephem_Close()
    {
      if (NOVAS3.Is64Bit())
        return NOVAS3.EphemClose64();
      return NOVAS3.EphemClose32();
    }

    private void SetRACIOFile(string FName)
    {
      if (NOVAS3.Is64Bit())
        NOVAS3.SetRACIOFile64(FName);
      else
        NOVAS3.SetRACIOFile32(FName);
    }

    [DllImport("NOVAS3.dll", EntryPoint = "set_racio_file")]
    private static extern void SetRACIOFile32([MarshalAs(UnmanagedType.LPStr)] string FName);

    [DllImport("NOVAS3.dll", EntryPoint = "Ephem_Close")]
    private static extern short EphemClose32();

    [DllImport("NOVAS3.dll", EntryPoint = "Ephem_Open")]
    private static extern short EphemOpen32([MarshalAs(UnmanagedType.LPStr)] string Ephem_Name, ref double JD_Begin, ref double JD_End);

    [DllImport("NOVAS3.dll", EntryPoint = "Planet_Ephemeris")]
    private static extern short PlanetEphemeris32(ref JDHighPrecision Tjd, Target Target, Target Center, ref PosVector Position, ref VelVector Velocity);

    [DllImport("NOVAS3.dll", EntryPoint = "readeph")]
    private static extern IntPtr ReadEph32(int Mp, [MarshalAs(UnmanagedType.LPStr)] string Name, double Jd, ref int Err);

    [DllImport("NOVAS3.dll", EntryPoint = "cleaneph")]
    private static extern void CleanEph32();

    [DllImport("NOVAS3.dll", EntryPoint = "solarsystem")]
    private static extern short SolarSystem32(double tjd, short body, short origin, ref PosVector pos, ref VelVector vel);

    [DllImport("NOVAS3.dll", EntryPoint = "State")]
    private static extern short State32(ref JDHighPrecision Jed, Target Target, ref PosVector TargetPos, ref VelVector TargetVel);

    [DllImport("NOVAS3.dll", EntryPoint = "aberration")]
    private static extern void Aberration32(ref PosVector Pos, ref VelVector Vel, double LightTime, ref PosVector Pos2);

    [DllImport("NOVAS3.dll", EntryPoint = "app_planet")]
    private static extern short AppPlanet32(double JdTt, ref Object3Internal SsBody, Accuracy Accuracy, ref double Ra, ref double Dec, ref double Dis);

    [DllImport("NOVAS3.dll", EntryPoint = "app_star")]
    private static extern short AppStar32(double JdTt, ref CatEntry3 Star, Accuracy Accuracy, ref double Ra, ref double Dec);

    [DllImport("NOVAS3.dll", EntryPoint = "astro_planet")]
    private static extern short AstroPlanet32(double JdTt, ref Object3Internal SsBody, Accuracy Accuracy, ref double Ra, ref double Dec, ref double Dis);

    [DllImport("NOVAS3.dll", EntryPoint = "astro_star")]
    private static extern short AstroStar32(double JdTt, ref CatEntry3 Star, Accuracy Accuracy, ref double Ra, ref double Dec);

    [DllImport("NOVAS3.dll", EntryPoint = "bary2obs")]
    private static extern void Bary2Obs32(ref PosVector Pos, ref PosVector PosObs, ref PosVector Pos2, ref double Lighttime);

    [DllImport("NOVAS3.dll", EntryPoint = "cal_date")]
    private static extern void CalDate32(double Tjd, ref short Year, ref short Month, ref short Day, ref double Hour);

    [DllImport("NOVAS3.dll", EntryPoint = "cel_pole")]
    private static extern short CelPole32(double Tjd, PoleOffsetCorrection Type, double Dpole1, double Dpole2);

    [DllImport("NOVAS3.dll", EntryPoint = "cio_array")]
    private static extern short CioArray32(double JdTdb, int NPts, ref RAOfCioArray Cio);

    [DllImport("NOVAS3.dll", EntryPoint = "cio_basis")]
    private static extern short CioBasis32(double JdTdbEquionx, double RaCioEquionx, ReferenceSystem RefSys, Accuracy Accuracy, ref double x, ref double y, ref double z);

    [DllImport("NOVAS3.dll", EntryPoint = "cio_location")]
    private static extern short CioLocation32(double JdTdb, Accuracy Accuracy, ref double RaCio, ref ReferenceSystem RefSys);

    [DllImport("NOVAS3.dll", EntryPoint = "cio_ra")]
    private static extern short CioRa32(double JdTt, Accuracy Accuracy, ref double RaCio);

    [DllImport("NOVAS3.dll", EntryPoint = "d_light")]
    private static extern double DLight32(ref PosVector Pos1, ref PosVector PosObs);

    [DllImport("NOVAS3.dll", EntryPoint = "e_tilt")]
    private static extern void ETilt32(double JdTdb, Accuracy Accuracy, ref double Mobl, ref double Tobl, ref double Ee, ref double Dpsi, ref double Deps);

    [DllImport("NOVAS3.dll", EntryPoint = "ecl2equ_vec")]
    private static extern short Ecl2EquVec32(double JdTt, CoordSys CoordSys, Accuracy Accuracy, ref PosVector Pos1, ref PosVector Pos2);

    [DllImport("NOVAS3.dll", EntryPoint = "ee_ct")]
    private static extern double EeCt32(double JdHigh, double JdLow, Accuracy Accuracy);

    [DllImport("NOVAS3.dll", EntryPoint = "ephemeris")]
    private static extern short Ephemeris32(ref JDHighPrecision Jd, ref Object3Internal CelObj, Origin Origin, Accuracy Accuracy, ref PosVector Pos, ref VelVector Vel);

    [DllImport("NOVAS3.dll", EntryPoint = "equ2ecl")]
    private static extern short Equ2Ecl32(double JdTt, CoordSys CoordSys, Accuracy Accuracy, double Ra, double Dec, ref double ELon, ref double ELat);

    [DllImport("NOVAS3.dll", EntryPoint = "equ2ecl_vec")]
    private static extern short Equ2EclVec32(double JdTt, CoordSys CoordSys, Accuracy Accuracy, ref PosVector Pos1, ref PosVector Pos2);

    [DllImport("NOVAS3.dll", EntryPoint = "equ2gal")]
    private static extern void Equ2Gal32(double RaI, double DecI, ref double GLon, ref double GLat);

    [DllImport("NOVAS3.dll", EntryPoint = "equ2hor")]
    private static extern void Equ2Hor32(double Jd_Ut1, double DeltT, Accuracy Accuracy, double x, double y, ref OnSurface Location, double Ra, double Dec, RefractionOption RefOption, ref double Zd, ref double Az, ref double RaR, ref double DecR);

    [DllImport("NOVAS3.dll", EntryPoint = "era")]
    private static extern double Era32(double JdHigh, double JdLow);

    [DllImport("NOVAS3.dll", EntryPoint = "frame_tie")]
    private static extern void FrameTie32(ref PosVector Pos1, FrameConversionDirection Direction, ref PosVector Pos2);

    [DllImport("NOVAS3.dll", EntryPoint = "fund_args")]
    private static extern void FundArgs32(double t, ref FundamentalArgs a);

    [DllImport("NOVAS3.dll", EntryPoint = "gcrs2equ")]
    private static extern short Gcrs2Equ32(double JdTt, CoordSys CoordSys, Accuracy Accuracy, double RaG, double DecG, ref double Ra, ref double Dec);

    [DllImport("NOVAS3.dll", EntryPoint = "geo_posvel")]
    private static extern short GeoPosVel32(double JdTt, double DeltaT, Accuracy Accuracy, ref Observer Obs, ref PosVector Pos, ref VelVector Vel);

    [DllImport("NOVAS3.dll", EntryPoint = "grav_def")]
    private static extern short GravDef32(double JdTdb, EarthDeflection LocCode, Accuracy Accuracy, ref PosVector Pos1, ref PosVector PosObs, ref PosVector Pos2);

    [DllImport("NOVAS3.dll", EntryPoint = "grav_vec")]
    private static extern void GravVec32(ref PosVector Pos1, ref PosVector PosObs, ref PosVector PosBody, double RMass, ref PosVector Pos2);

    [DllImport("NOVAS3.dll", EntryPoint = "ira_equinox")]
    private static extern double IraEquinox32(double JdTdb, EquinoxType Equinox, Accuracy Accuracy);

    [DllImport("NOVAS3.dll", EntryPoint = "julian_date")]
    private static extern double JulianDate32(short Year, short Month, short Day, double Hour);

    [DllImport("NOVAS3.dll", EntryPoint = "light_time")]
    private static extern short LightTime32(double JdTdb, ref Object3Internal SsObject, ref PosVector PosObs, double TLight0, Accuracy Accuracy, ref PosVector Pos, ref double TLight);

    [DllImport("NOVAS3.dll", EntryPoint = "limb_angle")]
    private static extern void LimbAngle32(ref PosVector PosObj, ref PosVector PosObs, ref double LimbAng, ref double NadirAng);

    [DllImport("NOVAS3.dll", EntryPoint = "local_planet")]
    private static extern short LocalPlanet32(double JdTt, ref Object3Internal SsBody, double DeltaT, ref OnSurface Position, Accuracy Accuracy, ref double Ra, ref double Dec, ref double Dis);

    [DllImport("NOVAS3.dll", EntryPoint = "local_star")]
    private static extern short LocalStar32(double JdTt, double DeltaT, ref CatEntry3 Star, ref OnSurface Position, Accuracy Accuracy, ref double Ra, ref double Dec);

    [DllImport("NOVAS3.dll", EntryPoint = "make_cat_entry")]
    private static extern void MakeCatEntry32([MarshalAs(UnmanagedType.LPStr)] string StarName, [MarshalAs(UnmanagedType.LPStr)] string Catalog, int StarNum, double Ra, double Dec, double PmRa, double PmDec, double Parallax, double RadVel, ref CatEntry3 Star);

    [DllImport("NOVAS3.dll", EntryPoint = "make_in_space")]
    private static extern void MakeInSpace32(ref PosVector ScPos, ref VelVector ScVel, ref InSpace ObsSpace);

    [DllImport("NOVAS3.dll", EntryPoint = "make_object")]
    private static extern short MakeObject32(ASCOM.Astrometry.ObjectType Type, short Number, [MarshalAs(UnmanagedType.LPStr)] string Name, ref CatEntry3 StarData, ref Object3Internal CelObj);

    [DllImport("NOVAS3.dll", EntryPoint = "make_observer")]
    private static extern short MakeObserver32(ObserverLocation Where, ref OnSurface ObsSurface, ref InSpace ObsSpace, ref Observer Obs);

    [DllImport("NOVAS3.dll", EntryPoint = "make_observer_at_geocenter")]
    private static extern void MakeObserverAtGeocenter32(ref Observer ObsAtGeocenter);

    [DllImport("NOVAS3.dll", EntryPoint = "make_observer_in_space")]
    private static extern void MakeObserverInSpace32(ref PosVector ScPos, ref VelVector ScVel, ref Observer ObsInSpace);

    [DllImport("NOVAS3.dll", EntryPoint = "make_observer_on_surface")]
    private static extern void MakeObserverOnSurface32(double Latitude, double Longitude, double Height, double Temperature, double Pressure, ref Observer ObsOnSurface);

    [DllImport("NOVAS3.dll", EntryPoint = "make_on_surface")]
    private static extern void MakeOnSurface32(double Latitude, double Longitude, double Height, double Temperature, double Pressure, ref OnSurface ObsSurface);

    [DllImport("NOVAS3.dll", EntryPoint = "mean_obliq")]
    private static extern double MeanObliq32(double JdTdb);

    [DllImport("NOVAS3.dll", EntryPoint = "mean_star")]
    private static extern short MeanStar32(double JdTt, double Ra, double Dec, Accuracy Accuracy, ref double IRa, ref double IDec);

    [DllImport("NOVAS3.dll", EntryPoint = "norm_ang")]
    private static extern double NormAng32(double Angle);

    [DllImport("NOVAS3.dll", EntryPoint = "nutation")]
    private static extern void Nutation32(double JdTdb, NutationDirection Direction, Accuracy Accuracy, ref PosVector Pos, ref PosVector Pos2);

    [DllImport("NOVAS3.dll", EntryPoint = "nutation_angles")]
    private static extern void NutationAngles32(double t, Accuracy Accuracy, ref double DPsi, ref double DEps);

    [DllImport("NOVAS3.dll", EntryPoint = "place")]
    private static extern short Place32(double JdTt, ref Object3Internal CelObject, ref Observer Location, double DeltaT, CoordSys CoordSys, Accuracy Accuracy, ref SkyPos Output);

    [DllImport("NOVAS3.dll", EntryPoint = "precession")]
    private static extern short Precession32(double JdTdb1, ref PosVector Pos1, double JdTdb2, ref PosVector Pos2);

    [DllImport("NOVAS3.dll", EntryPoint = "proper_motion")]
    private static extern void ProperMotion32(double JdTdb1, ref PosVector Pos, ref VelVector Vel, double JdTdb2, ref PosVector Pos2);

    [DllImport("NOVAS3.dll", EntryPoint = "rad_vel")]
    private static extern void RadVel32(ref Object3Internal CelObject, ref PosVector Pos, ref VelVector Vel, ref VelVector VelObs, double DObsGeo, double DObsSun, double DObjSun, ref double Rv);

    [DllImport("NOVAS3.dll", EntryPoint = "radec2vector")]
    private static extern void RaDec2Vector32(double Ra, double Dec, double Dist, ref PosVector Vector);

    [DllImport("NOVAS3.dll", EntryPoint = "refract")]
    private static extern double Refract32(ref OnSurface Location, RefractionOption RefOption, double ZdObs);

    [DllImport("NOVAS3.dll", EntryPoint = "sidereal_time")]
    private static extern short SiderealTime32(double JdHigh, double JdLow, double DeltaT, GstType GstType, ASCOM.Astrometry.Method Method, Accuracy Accuracy, ref double Gst);

    [DllImport("NOVAS3.dll", EntryPoint = "spin")]
    private static extern void Spin32(double Angle, ref PosVector Pos1, ref PosVector Pos2);

    [DllImport("NOVAS3.dll", EntryPoint = "starvectors")]
    private static extern void StarVectors32(ref CatEntry3 Star, ref PosVector Pos, ref VelVector Vel);

    [DllImport("NOVAS3.dll", EntryPoint = "tdb2tt")]
    private static extern void Tdb2Tt32(double TdbJd, ref double TtJd, ref double SecDiff);

    [DllImport("NOVAS3.dll", EntryPoint = "ter2cel")]
    private static extern short Ter2Cel32(double JdHigh, double JdLow, double DeltaT, ASCOM.Astrometry.Method Method, Accuracy Accuracy, OutputVectorOption OutputOption, double x, double y, ref PosVector VecT, ref PosVector VecC);

    [DllImport("NOVAS3.dll", EntryPoint = "terra")]
    private static extern void Terra32(ref OnSurface Location, double St, ref PosVector Pos, ref VelVector Vel);

    [DllImport("NOVAS3.dll", EntryPoint = "topo_planet")]
    private static extern short TopoPlanet32(double JdTt, ref Object3Internal SsBody, double DeltaT, ref OnSurface Position, Accuracy Accuracy, ref double Ra, ref double Dec, ref double Dis);

    [DllImport("NOVAS3.dll", EntryPoint = "topo_star")]
    private static extern short TopoStar32(double JdTt, double DeltaT, ref CatEntry3 Star, ref OnSurface Position, Accuracy Accuracy, ref double Ra, ref double Dec);

    [DllImport("NOVAS3.dll", EntryPoint = "transform_cat")]
    private static extern short TransformCat32(TransformationOption3 TransformOption, double DateInCat, ref CatEntry3 InCat, double DateNewCat, [MarshalAs(UnmanagedType.LPStr)] string NewCatId, ref CatEntry3 NewCat);

    [DllImport("NOVAS3.dll", EntryPoint = "transform_hip")]
    private static extern void TransformHip32(ref CatEntry3 Hipparcos, ref CatEntry3 Hip2000);

    [DllImport("NOVAS3.dll", EntryPoint = "vector2radec")]
    private static extern short Vector2RaDec32(ref PosVector Pos, ref double Ra, ref double Dec);

    [DllImport("NOVAS3.dll", EntryPoint = "virtual_planet")]
    private static extern short VirtualPlanet32(double JdTt, ref Object3Internal SsBody, Accuracy Accuracy, ref double Ra, ref double Dec, ref double Dis);

    [DllImport("NOVAS3.dll", EntryPoint = "virtual_star")]
    private static extern short VirtualStar32(double JdTt, ref CatEntry3 Star, Accuracy Accuracy, ref double Ra, ref double Dec);

    [DllImport("NOVAS3.dll", EntryPoint = "wobble")]
    private static extern void Wobble32(double Tjd, double x, double y, ref PosVector Pos1, ref PosVector Pos2);

    [DllImport("NOVAS3-64.dll", EntryPoint = "set_racio_file")]
    private static extern void SetRACIOFile64([MarshalAs(UnmanagedType.LPStr)] string Name);

    [DllImport("NOVAS3-64.dll", EntryPoint = "Ephem_Close")]
    private static extern short EphemClose64();

    [DllImport("NOVAS3-64.dll", EntryPoint = "Ephem_Open")]
    private static extern short EphemOpen64([MarshalAs(UnmanagedType.LPStr)] string Ephem_Name, ref double JD_Begin, ref double JD_End);

    [DllImport("NOVAS3-64.dll", EntryPoint = "Planet_Ephemeris")]
    private static extern short PlanetEphemeris64(ref JDHighPrecision Tjd, Target Target, Target Center, ref PosVector Position, ref VelVector Velocity);

    [DllImport("NOVAS3-64.dll", EntryPoint = "readeph")]
    private static extern IntPtr ReadEph64(int Mp, [MarshalAs(UnmanagedType.LPStr)] string Name, double Jd, ref int Err);

    [DllImport("NOVAS3-64.dll", EntryPoint = "cleaneph")]
    private static extern void CleanEph64();

    [DllImport("NOVAS3-64.dll", EntryPoint = "solarsystem")]
    private static extern short SolarSystem64(double tjd, short body, short origin, ref PosVector pos, ref VelVector vel);

    [DllImport("NOVAS3-64.dll", EntryPoint = "State")]
    private static extern short State64(ref JDHighPrecision Jed, Target Target, ref PosVector TargetPos, ref VelVector TargetVel);

    [DllImport("NOVAS3-64.dll", EntryPoint = "aberration")]
    private static extern void Aberration64(ref PosVector Pos, ref VelVector Vel, double LightTime, ref PosVector Pos2);

    [DllImport("NOVAS3-64.dll", EntryPoint = "app_planet")]
    private static extern short AppPlanet64(double JdTt, ref Object3Internal SsBody, Accuracy Accuracy, ref double Ra, ref double Dec, ref double Dis);

    [DllImport("NOVAS3-64.dll", EntryPoint = "app_star")]
    private static extern short AppStar64(double JdTt, ref CatEntry3 Star, Accuracy Accuracy, ref double Ra, ref double Dec);

    [DllImport("NOVAS3-64.dll", EntryPoint = "astro_planet")]
    private static extern short AstroPlanet64(double JdTt, ref Object3Internal SsBody, Accuracy Accuracy, ref double Ra, ref double Dec, ref double Dis);

    [DllImport("NOVAS3-64.dll", EntryPoint = "astro_star")]
    private static extern short AstroStar64(double JdTt, ref CatEntry3 Star, Accuracy Accuracy, ref double Ra, ref double Dec);

    [DllImport("NOVAS3-64.dll", EntryPoint = "bary2obs")]
    private static extern void Bary2Obs64(ref PosVector Pos, ref PosVector PosObs, ref PosVector Pos2, ref double Lighttime);

    [DllImport("NOVAS3-64.dll", EntryPoint = "cal_date")]
    private static extern void CalDate64(double Tjd, ref short Year, ref short Month, ref short Day, ref double Hour);

    [DllImport("NOVAS3-64.dll", EntryPoint = "cel_pole")]
    private static extern short CelPole64(double Tjd, PoleOffsetCorrection Type, double Dpole1, double Dpole2);

    [DllImport("NOVAS3-64.dll", EntryPoint = "cio_array")]
    private static extern short CioArray64(double JdTdb, int NPts, ref RAOfCioArray Cio);

    [DllImport("NOVAS3-64.dll", EntryPoint = "cio_basis")]
    private static extern short CioBasis64(double JdTdbEquionx, double RaCioEquionx, ReferenceSystem RefSys, Accuracy Accuracy, ref double x, ref double y, ref double z);

    [DllImport("NOVAS3-64.dll", EntryPoint = "cio_location")]
    private static extern short CioLocation64(double JdTdb, Accuracy Accuracy, ref double RaCio, ref ReferenceSystem RefSys);

    [DllImport("NOVAS3-64.dll", EntryPoint = "cio_ra")]
    private static extern short CioRa64(double JdTt, Accuracy Accuracy, ref double RaCio);

    [DllImport("NOVAS3-64.dll", EntryPoint = "d_light")]
    private static extern double DLight64(ref PosVector Pos1, ref PosVector PosObs);

    [DllImport("NOVAS3-64.dll", EntryPoint = "e_tilt")]
    private static extern void ETilt64(double JdTdb, Accuracy Accuracy, ref double Mobl, ref double Tobl, ref double Ee, ref double Dpsi, ref double Deps);

    [DllImport("NOVAS3-64.dll", EntryPoint = "ecl2equ_vec")]
    private static extern short Ecl2EquVec64(double JdTt, CoordSys CoordSys, Accuracy Accuracy, ref PosVector Pos1, ref PosVector Pos2);

    [DllImport("NOVAS3-64.dll", EntryPoint = "ee_ct")]
    private static extern double EeCt64(double JdHigh, double JdLow, Accuracy Accuracy);

    [DllImport("NOVAS3-64.dll", EntryPoint = "ephemeris")]
    private static extern short Ephemeris64(ref JDHighPrecision Jd, ref Object3Internal CelObj, Origin Origin, Accuracy Accuracy, ref PosVector Pos, ref VelVector Vel);

    [DllImport("NOVAS3-64.dll", EntryPoint = "equ2ecl")]
    private static extern short Equ2Ecl64(double JdTt, CoordSys CoordSys, Accuracy Accuracy, double Ra, double Dec, ref double ELon, ref double ELat);

    [DllImport("NOVAS3-64.dll", EntryPoint = "equ2ecl_vec")]
    private static extern short Equ2EclVec64(double JdTt, CoordSys CoordSys, Accuracy Accuracy, ref PosVector Pos1, ref PosVector Pos2);

    [DllImport("NOVAS3-64.dll", EntryPoint = "equ2gal")]
    private static extern void Equ2Gal64(double RaI, double DecI, ref double GLon, ref double GLat);

    [DllImport("NOVAS3-64.dll", EntryPoint = "equ2hor")]
    private static extern void Equ2Hor64(double Jd_Ut1, double DeltT, Accuracy Accuracy, double x, double y, ref OnSurface Location, double Ra, double Dec, RefractionOption RefOption, ref double Zd, ref double Az, ref double RaR, ref double DecR);

    [DllImport("NOVAS3-64.dll", EntryPoint = "era")]
    private static extern double Era64(double JdHigh, double JdLow);

    [DllImport("NOVAS3-64.dll", EntryPoint = "frame_tie")]
    private static extern void FrameTie64(ref PosVector Pos1, FrameConversionDirection Direction, ref PosVector Pos2);

    [DllImport("NOVAS3-64.dll", EntryPoint = "fund_args")]
    private static extern void FundArgs64(double t, ref FundamentalArgs a);

    [DllImport("NOVAS3-64.dll", EntryPoint = "gcrs2equ")]
    private static extern short Gcrs2Equ64(double JdTt, CoordSys CoordSys, Accuracy Accuracy, double RaG, double DecG, ref double Ra, ref double Dec);

    [DllImport("NOVAS3-64.dll", EntryPoint = "geo_posvel")]
    private static extern short GeoPosVel64(double JdTt, double DeltaT, Accuracy Accuracy, ref Observer Obs, ref PosVector Pos, ref VelVector Vel);

    [DllImport("NOVAS3-64.dll", EntryPoint = "grav_def")]
    private static extern short GravDef64(double JdTdb, EarthDeflection LocCode, Accuracy Accuracy, ref PosVector Pos1, ref PosVector PosObs, ref PosVector Pos2);

    [DllImport("NOVAS3-64.dll", EntryPoint = "grav_vec")]
    private static extern void GravVec64(ref PosVector Pos1, ref PosVector PosObs, ref PosVector PosBody, double RMass, ref PosVector Pos2);

    [DllImport("NOVAS3-64.dll", EntryPoint = "ira_equinox")]
    private static extern double IraEquinox64(double JdTdb, EquinoxType Equinox, Accuracy Accuracy);

    [DllImport("NOVAS3-64.dll", EntryPoint = "julian_date")]
    private static extern double JulianDate64(short Year, short Month, short Day, double Hour);

    [DllImport("NOVAS3-64.dll", EntryPoint = "light_time")]
    private static extern short LightTime64(double JdTdb, ref Object3Internal SsObject, ref PosVector PosObs, double TLight0, Accuracy Accuracy, ref PosVector Pos, ref double TLight);

    [DllImport("NOVAS3-64.dll", EntryPoint = "limb_angle")]
    private static extern void LimbAngle64(ref PosVector PosObj, ref PosVector PosObs, ref double LimbAng, ref double NadirAng);

    [DllImport("NOVAS3-64.dll", EntryPoint = "local_planet")]
    private static extern short LocalPlanet64(double JdTt, ref Object3Internal SsBody, double DeltaT, ref OnSurface Position, Accuracy Accuracy, ref double Ra, ref double Dec, ref double Dis);

    [DllImport("NOVAS3-64.dll", EntryPoint = "local_star")]
    private static extern short LocalStar64(double JdTt, double DeltaT, ref CatEntry3 Star, ref OnSurface Position, Accuracy Accuracy, ref double Ra, ref double Dec);

    [DllImport("NOVAS3-64.dll", EntryPoint = "make_cat_entry")]
    private static extern void MakeCatEntry64([MarshalAs(UnmanagedType.LPStr)] string StarName, [MarshalAs(UnmanagedType.LPStr)] string Catalog, int StarNum, double Ra, double Dec, double PmRa, double PmDec, double Parallax, double RadVel, ref CatEntry3 Star);

    [DllImport("NOVAS3-64.dll", EntryPoint = "make_in_space")]
    private static extern void MakeInSpace64(ref PosVector ScPos, ref VelVector ScVel, ref InSpace ObsSpace);

    [DllImport("NOVAS3-64.dll", EntryPoint = "make_object")]
    private static extern short MakeObject64(ASCOM.Astrometry.ObjectType Type, short Number, [MarshalAs(UnmanagedType.LPStr)] string Name, ref CatEntry3 StarData, ref Object3Internal CelObj);

    [DllImport("NOVAS3-64.dll", EntryPoint = "make_observer")]
    private static extern short MakeObserver64(ObserverLocation Where, ref OnSurface ObsSurface, ref InSpace ObsSpace, ref Observer Obs);

    [DllImport("NOVAS3-64.dll", EntryPoint = "make_observer_at_geocenter")]
    private static extern void MakeObserverAtGeocenter64(ref Observer ObsAtGeocenter);

    [DllImport("NOVAS3-64.dll", EntryPoint = "make_observer_in_space")]
    private static extern void MakeObserverInSpace64(ref PosVector ScPos, ref VelVector ScVel, ref Observer ObsInSpace);

    [DllImport("NOVAS3-64.dll", EntryPoint = "make_observer_on_surface")]
    private static extern void MakeObserverOnSurface64(double Latitude, double Longitude, double Height, double Temperature, double Pressure, ref Observer ObsOnSurface);

    [DllImport("NOVAS3-64.dll", EntryPoint = "make_on_surface")]
    private static extern void MakeOnSurface64(double Latitude, double Longitude, double Height, double Temperature, double Pressure, ref OnSurface ObsSurface);

    [DllImport("NOVAS3-64.dll", EntryPoint = "mean_obliq")]
    private static extern double MeanObliq64(double JdTdb);

    [DllImport("NOVAS3-64.dll", EntryPoint = "mean_star")]
    private static extern short MeanStar64(double JdTt, double Ra, double Dec, Accuracy Accuracy, ref double IRa, ref double IDec);

    [DllImport("NOVAS3-64.dll", EntryPoint = "norm_ang")]
    private static extern double NormAng64(double Angle);

    [DllImport("NOVAS3-64.dll", EntryPoint = "nutation")]
    private static extern void Nutation64(double JdTdb, NutationDirection Direction, Accuracy Accuracy, ref PosVector Pos, ref PosVector Pos2);

    [DllImport("NOVAS3-64.dll", EntryPoint = "nutation_angles")]
    private static extern void NutationAngles64(double t, Accuracy Accuracy, ref double DPsi, ref double DEps);

    [DllImport("NOVAS3-64.dll", EntryPoint = "place")]
    private static extern short Place64(double JdTt, ref Object3Internal CelObject, ref Observer Location, double DeltaT, CoordSys CoordSys, Accuracy Accuracy, ref SkyPos Output);

    [DllImport("NOVAS3-64.dll", EntryPoint = "precession")]
    private static extern short Precession64(double JdTdb1, ref PosVector Pos1, double JdTdb2, ref PosVector Pos2);

    [DllImport("NOVAS3-64.dll", EntryPoint = "proper_motion")]
    private static extern void ProperMotion64(double JdTdb1, ref PosVector Pos, ref VelVector Vel, double JdTdb2, ref PosVector Pos2);

    [DllImport("NOVAS3-64.dll", EntryPoint = "rad_vel")]
    private static extern void RadVel64(ref Object3Internal CelObject, ref PosVector Pos, ref VelVector Vel, ref VelVector VelObs, double DObsGeo, double DObsSun, double DObjSun, ref double Rv);

    [DllImport("NOVAS3-64.dll", EntryPoint = "radec2vector")]
    private static extern void RaDec2Vector64(double Ra, double Dec, double Dist, ref PosVector Vector);

    [DllImport("NOVAS3-64.dll", EntryPoint = "refract")]
    private static extern double Refract64(ref OnSurface Location, RefractionOption RefOption, double ZdObs);

    [DllImport("NOVAS3-64.dll", EntryPoint = "sidereal_time")]
    private static extern short SiderealTime64(double JdHigh, double JdLow, double DeltaT, GstType GstType, ASCOM.Astrometry.Method Method, Accuracy Accuracy, ref double Gst);

    [DllImport("NOVAS3-64.dll", EntryPoint = "spin")]
    private static extern void Spin64(double Angle, ref PosVector Pos1, ref PosVector Pos2);

    [DllImport("NOVAS3-64.dll", EntryPoint = "starvectors")]
    private static extern void StarVectors64(ref CatEntry3 Star, ref PosVector Pos, ref VelVector Vel);

    [DllImport("NOVAS3-64.dll", EntryPoint = "tdb2tt")]
    private static extern void Tdb2Tt64(double TdbJd, ref double TtJd, ref double SecDiff);

    [DllImport("NOVAS3-64.dll", EntryPoint = "ter2cel")]
    private static extern short Ter2Cel64(double JdHigh, double JdLow, double DeltaT, ASCOM.Astrometry.Method Method, Accuracy Accuracy, OutputVectorOption OutputOption, double x, double y, ref PosVector VecT, ref PosVector VecC);

    [DllImport("NOVAS3-64.dll", EntryPoint = "terra")]
    private static extern void Terra64(ref OnSurface Location, double St, ref PosVector Pos, ref VelVector Vel);

    [DllImport("NOVAS3-64.dll", EntryPoint = "topo_planet")]
    private static extern short TopoPlanet64(double JdTt, ref Object3Internal SsBody, double DeltaT, ref OnSurface Position, Accuracy Accuracy, ref double Ra, ref double Dec, ref double Dis);

    [DllImport("NOVAS3-64.dll", EntryPoint = "topo_star")]
    private static extern short TopoStar64(double JdTt, double DeltaT, ref CatEntry3 Star, ref OnSurface Position, Accuracy Accuracy, ref double Ra, ref double Dec);

    [DllImport("NOVAS3-64.dll", EntryPoint = "transform_cat")]
    private static extern short TransformCat64(TransformationOption3 TransformOption, double DateInCat, ref CatEntry3 InCat, double DateNewCat, [MarshalAs(UnmanagedType.LPStr)] string NewCatId, ref CatEntry3 NewCat);

    [DllImport("NOVAS3-64.dll", EntryPoint = "transform_hip")]
    private static extern void TransformHip64(ref CatEntry3 Hipparcos, ref CatEntry3 Hip2000);

    [DllImport("NOVAS3-64.dll", EntryPoint = "vector2radec")]
    private static extern short Vector2RaDec64(ref PosVector Pos, ref double Ra, ref double Dec);

    [DllImport("NOVAS3-64.dll", EntryPoint = "virtual_planet")]
    private static extern short VirtualPlanet64(double JdTt, ref Object3Internal SsBody, Accuracy Accuracy, ref double Ra, ref double Dec, ref double Dis);

    [DllImport("NOVAS3-64.dll", EntryPoint = "virtual_star")]
    private static extern short VirtualStar64(double JdTt, ref CatEntry3 Star, Accuracy Accuracy, ref double Ra, ref double Dec);

    [DllImport("NOVAS3-64.dll", EntryPoint = "wobble")]
    private static extern void Wobble64(double Tjd, double x, double y, ref PosVector Pos1, ref PosVector Pos2);

    [DllImport("shell32.dll")]
    public static extern bool SHGetSpecialFolderPath(IntPtr hwndOwner, [Out] StringBuilder lpszPath, int nFolder, bool fCreate);

    [DllImport("kernel32.dll", EntryPoint = "LoadLibraryA", SetLastError = true)]
    public static extern IntPtr LoadLibrary(string lpFileName);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool FreeLibrary(IntPtr hModule);

    private static bool Is64Bit()
    {
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

    private static void RACioArrayStructureToArr(RAOfCioArray C, ref ArrayList Ar)
    {
      if (C.Value1.RACio != double.NaN)
        Ar.Add((object) C.Value1);
      if (C.Value2.RACio != double.NaN)
        Ar.Add((object) C.Value2);
      if (C.Value3.RACio != double.NaN)
        Ar.Add((object) C.Value3);
      if (C.Value4.RACio != double.NaN)
        Ar.Add((object) C.Value4);
      if (C.Value5.RACio != double.NaN)
        Ar.Add((object) C.Value5);
      if (C.Value6.RACio != double.NaN)
        Ar.Add((object) C.Value6);
      if (C.Value7.RACio != double.NaN)
        Ar.Add((object) C.Value7);
      if (C.Value8.RACio != double.NaN)
        Ar.Add((object) C.Value8);
      if (C.Value9.RACio != double.NaN)
        Ar.Add((object) C.Value9);
      if (C.Value10.RACio != double.NaN)
        Ar.Add((object) C.Value10);
      if (C.Value11.RACio != double.NaN)
        Ar.Add((object) C.Value11);
      if (C.Value12.RACio != double.NaN)
        Ar.Add((object) C.Value12);
      if (C.Value13.RACio != double.NaN)
        Ar.Add((object) C.Value13);
      if (C.Value14.RACio != double.NaN)
        Ar.Add((object) C.Value14);
      if (C.Value15.RACio != double.NaN)
        Ar.Add((object) C.Value15);
      if (C.Value16.RACio != double.NaN)
        Ar.Add((object) C.Value16);
      if (C.Value17.RACio != double.NaN)
        Ar.Add((object) C.Value17);
      if (C.Value18.RACio != double.NaN)
        Ar.Add((object) C.Value18);
      if (C.Value19.RACio != double.NaN)
        Ar.Add((object) C.Value19);
      if (C.Value20.RACio == double.NaN)
        return;
      Ar.Add((object) C.Value20);
    }

    private void O3FromO3Internal(Object3Internal O3I, ref Object3 O3)
    {
      O3.Name = O3I.Name;
      O3.Number = (Body) O3I.Number;
      O3.Star = O3I.Star;
      O3.Type = O3I.Type;
    }

    private Object3Internal O3IFromObject3(Object3 O3)
    {
      return new Object3Internal()
      {
        Name = O3.Name,
        Number = checked ((short) O3.Number),
        Star = O3.Star,
        Type = O3.Type
      };
    }

    public double DeltaT(double Tjd)
    {
      return DeltatCode.DeltaTCalc(Tjd);
    }
  }
}
