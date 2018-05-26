// Decompiled with JetBrains decompiler
// Type: ASCOM.Astrometry.NOVAS.NOVAS31
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
using System.Threading;

namespace ASCOM.Astrometry.NOVAS
{
  //[ComVisible(true)]
  [ClassInterface(ClassInterfaceType.None)]
  //[Guid("B7203C35-B113-472D-9E5D-0602883AC835")]
  public class NOVAS31 : INOVAS31, IDisposable
  {
    private const string NOVAS32DLL = "NOVAS31.dll";
    private const string NOVAS64DLL = "NOVAS31-64.dll";
    private const string JPL_EPHEM_FILE_NAME = "JPLEPH";
    private const double JPL_EPHEM_START_DATE = 2305424.5;
    private const double JPL_EPHEM_END_DATE = 2525008.5;
    private const string NOVAS_DLL_LOCATION = "\\ASCOM\\Astrometry\\";
    private const string RACIO_FILE = "cio_ra.bin";
    private const string NOVAS31_MUTEX_NAME = "ASCOMNovas31Mutex";
    private TraceLogger TL;
    private Util Utl;
    private IntPtr Novas31DllHandle;
    private bool disposedValue;
    private const int CSIDL_PROGRAM_FILES = 38;
    private const int CSIDL_PROGRAM_FILESX86 = 42;
    private const int CSIDL_WINDOWS = 36;
    private const int CSIDL_PROGRAM_FILES_COMMONX86 = 44;

    public NOVAS31()
    {
      this.disposedValue = false;
      StringBuilder lpszPath = new StringBuilder(260);
      this.TL = new TraceLogger("", "NOVAS31");
      this.TL.Enabled = RegistryCommonCode.GetBool("Trace NOVAS", false);
      Mutex mutex = new Mutex(false, "ASCOMNovas31Mutex");
      string str;
      short num;
      try
      {
        this.TL.LogMessage("New", "Waiting for mutex");
        mutex.WaitOne(10000);
        this.TL.LogMessage("New", "Got mutex");
        this.Utl = new Util();
        string lpFileName;
        string FName;
        if (NOVAS31.Is64Bit())
        {
          NOVAS31.SHGetSpecialFolderPath(IntPtr.Zero, lpszPath, 44, false);
          lpFileName = lpszPath.ToString() + "\\ASCOM\\Astrometry\\NOVAS31-64.dll";
          FName = lpszPath.ToString() + "\\ASCOM\\Astrometry\\cio_ra.bin";
          str = lpszPath.ToString() + "\\ASCOM\\Astrometry\\JPLEPH";
        }
        else
        {
          lpFileName = Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles) + "\\ASCOM\\Astrometry\\NOVAS31.dll";
          FName = Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles) + "\\ASCOM\\Astrometry\\cio_ra.bin";
          str = Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles) + "\\ASCOM\\Astrometry\\JPLEPH";
        }
        this.TL.LogMessage("New", "Loading NOVAS31 library DLL: " + lpFileName);
        this.Novas31DllHandle = NOVAS31.LoadLibrary(lpFileName);
        int lastWin32Error = Marshal.GetLastWin32Error();
        if (this.Novas31DllHandle != IntPtr.Zero)
        {
          this.TL.LogMessage("New", "Loaded NOVAS31 library OK");
          this.SetRACIOFile(FName);
          string Ephem_Name = str;
          double JD_Begin = 2305424.5;
          double JD_End = 2525008.5;
          short DENumber = 0;
                    num = this.Ephem_Open(Ephem_Name, ref JD_Begin, ref JD_End, ref DENumber);
        }
        else
        {
          this.TL.LogMessage("New", "Error loading NOVAS31 library: " + lastWin32Error.ToString("X8"));
          throw new Exception("Error code returned from LoadLibrary when loading NOVAS31 library: " + lastWin32Error.ToString("X8"));
        }
      }
      finally
      {
        mutex.ReleaseMutex();
      }
      if ((int) num > 0)
      {
        this.TL.LogMessage("New", "Unable to open ephemeris file: " + str + ", RC: " + Conversions.ToString((int) num));
        throw new HelperException("Unable to open ephemeris file: " + str + ", RC: " + Conversions.ToString((int) num));
      }
      this.TL.LogMessage("New", "NOVAS31 initialised OK");
    }

    protected virtual void Dispose(bool disposing)
    {
      Mutex mutex = (Mutex) null;
      try
      {
        mutex = new Mutex(false, "ASCOMNovas31Mutex");
        mutex.WaitOne(10000);
        if (!this.disposedValue)
        {
          if (disposing)
          {
            if (this.Utl != null)
            {
              this.Utl.Dispose();
              this.Utl = (Util) null;
            }
            if (this.TL != null)
            {
              this.TL.Enabled = false;
              this.TL.Dispose();
              this.TL = (TraceLogger) null;
            }
          }
          try
          {
            int num = (int) this.Ephem_Close();
          }
          catch (Exception ex)
          {
            //ProjectData.SetProjectError(ex);
            //ProjectData.ClearProjectError();
          }
          try
          {
            NOVAS31.FreeLibrary(this.Novas31DllHandle);
          }
          catch (Exception ex)
          {
            //ProjectData.SetProjectError(ex);
            //ProjectData.ClearProjectError();
          }
        }
        this.disposedValue = true;
      }
      finally
      {
        if (mutex != null)
          mutex.ReleaseMutex();
      }
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
      short num = !NOVAS31.Is64Bit() ? NOVAS31.PlanetEphemeris32(ref Tjd1, Target, Center, ref Position1, ref Velocity1) : NOVAS31.PlanetEphemeris64(ref Tjd1, Target, Center, ref Position1, ref Velocity1);
      NOVAS31.PosVecToArr(Position1, ref Position);
      NOVAS31.VelVecToArr(Velocity1, ref Velocity);
      return num;
    }

    public double[] ReadEph(int Mp, string Name, double Jd, ref int Err)
    {
      double[] numArray = new double[6];
      byte[] destination = new byte[49];
      IntPtr source = !NOVAS31.Is64Bit() ? NOVAS31.ReadEph32(Mp, Name, Jd, ref Err) : NOVAS31.ReadEph64(Mp, Name, Jd, ref Err);
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
      short num = !NOVAS31.Is64Bit() ? NOVAS31.SolarSystem32(Tjd, checked ((short) Body), checked ((short) Origin), ref pos, ref vel) : NOVAS31.SolarSystem64(Tjd, checked ((short) Body), checked ((short) Origin), ref pos, ref vel);
      NOVAS31.PosVecToArr(pos, ref Pos);
      NOVAS31.VelVecToArr(vel, ref Vel);
      return num;
    }

    public short State(ref double[] Jed, Target Target, ref double[] TargetPos, ref double[] TargetVel)
    {
      JDHighPrecision Jed1 = new JDHighPrecision();
      PosVector TargetPos1 = new PosVector();
      VelVector TargetVel1 = new VelVector();
      Jed1.JDPart1 = Jed[0];
      Jed1.JDPart2 = Jed[1];
      short num = !NOVAS31.Is64Bit() ? NOVAS31.State32(ref Jed1, Target, ref TargetPos1, ref TargetVel1) : NOVAS31.State64(ref Jed1, Target, ref TargetPos1, ref TargetVel1);
      NOVAS31.PosVecToArr(TargetPos1, ref TargetPos);
      NOVAS31.VelVecToArr(TargetVel1, ref TargetVel);
      return num;
    }

    public void Aberration(double[] Pos, double[] Vel, double LightTime, ref double[] Pos2)
    {
      PosVector Pos2_1 = new PosVector();
            if (NOVAS31.Is64Bit())
      {
        PosVector posVec = NOVAS31.ArrToPosVec(Pos);
        VelVector velVec = NOVAS31.ArrToVelVec(Vel);
        double LightTime1 = LightTime;
        NOVAS31.Aberration64(ref posVec, ref velVec, LightTime1, ref Pos2_1);
      }
      else
      {
        PosVector posVec = NOVAS31.ArrToPosVec(Pos);
        VelVector velVec = NOVAS31.ArrToVelVec(Vel);
        double LightTime1 = LightTime;
        NOVAS31.Aberration32(ref posVec, ref velVec, LightTime1, ref Pos2_1);
      }
      NOVAS31.PosVecToArr(Pos2_1, ref Pos2);
    }

    public short AppPlanet(double JdTt, Object3 SsBody, Accuracy Accuracy, ref double Ra, ref double Dec, ref double Dis)
    {
      if (NOVAS31.Is64Bit())
      {
        double JdTt1 = JdTt;
        Object3Internal SsBody1 = this.O3IFromObject3(SsBody);
        int num = (int) Accuracy;
        return NOVAS31.AppPlanet64(JdTt1, ref SsBody1, (Accuracy) num, ref Ra, ref Dec, ref Dis);
      }
      double JdTt2 = JdTt;
      Object3Internal SsBody2 = this.O3IFromObject3(SsBody);
      int num1 = (int) Accuracy;
      return NOVAS31.AppPlanet32(JdTt2, ref SsBody2, (Accuracy) num1, ref Ra, ref Dec, ref Dis);
    }

    public short AppStar(double JdTt, CatEntry3 Star, Accuracy Accuracy, ref double Ra, ref double Dec)
    {
      try
      {
        this.TL.LogMessage("AppStar", "JD Accuracy:        " + Conversions.ToString(JdTt) + " " + Accuracy.ToString());
        this.TL.LogMessage("AppStar", "  Star.RA:          " + this.Utl.HoursToHMS(Star.RA, ":", ":", "", 3));
        this.TL.LogMessage("AppStar", "  Dec:              " + this.Utl.DegreesToDMS(Star.Dec, ":", ":", "", 3));
        this.TL.LogMessage("AppStar", "  Catalog:          " + Star.Catalog);
        this.TL.LogMessage("AppStar", "  Parallax:         " + Conversions.ToString(Star.Parallax));
        this.TL.LogMessage("AppStar", "  ProMoDec:         " + Conversions.ToString(Star.ProMoDec));
        this.TL.LogMessage("AppStar", "  ProMoRA:          " + Conversions.ToString(Star.ProMoRA));
        this.TL.LogMessage("AppStar", "  RadialVelocity:   " + Conversions.ToString(Star.RadialVelocity));
        this.TL.LogMessage("AppStar", "  StarName:         " + Star.StarName);
        this.TL.LogMessage("AppStar", "  StarNumber:       " + Conversions.ToString(Star.StarNumber));
      }
      catch (Exception ex)
      {
        //ProjectData.SetProjectError(ex);
        this.TL.LogMessageCrLf("AppStar", "Exception: " + ex.ToString());
        //ProjectData.ClearProjectError();
      }
      if (NOVAS31.Is64Bit())
      {
        short num = NOVAS31.AppStar64(JdTt, ref Star, Accuracy, ref Ra, ref Dec);
        this.TL.LogMessage("AppStar", "  64bit - Return Code: " + Conversions.ToString((int) num) + ", RA Dec: " + this.Utl.HoursToHMS(Ra, ":", ":", "", 3) + " " + this.Utl.DegreesToDMS(Dec, ":", ":", "", 3));
        return num;
      }
      short num1 = NOVAS31.AppStar32(JdTt, ref Star, Accuracy, ref Ra, ref Dec);
      this.TL.LogMessage("AppStar", "  32bit - Return Code: " + Conversions.ToString((int) num1) + ", RA Dec: " + this.Utl.HoursToHMS(Ra, ":", ":", "", 3) + " " + this.Utl.DegreesToDMS(Dec, ":", ":", "", 3));
      return num1;
    }

    public short AstroPlanet(double JdTt, Object3 SsBody, Accuracy Accuracy, ref double Ra, ref double Dec, ref double Dis)
    {
      if (NOVAS31.Is64Bit())
      {
        double JdTt1 = JdTt;
        Object3Internal SsBody1 = this.O3IFromObject3(SsBody);
        int num = (int) Accuracy;
        return NOVAS31.AstroPlanet64(JdTt1, ref SsBody1, (Accuracy) num, ref Ra, ref Dec, ref Dis);
      }
      double JdTt2 = JdTt;
      Object3Internal SsBody2 = this.O3IFromObject3(SsBody);
      int num1 = (int) Accuracy;
      return NOVAS31.AstroPlanet32(JdTt2, ref SsBody2, (Accuracy) num1, ref Ra, ref Dec, ref Dis);
    }

    public short AstroStar(double JdTt, CatEntry3 Star, Accuracy Accuracy, ref double Ra, ref double Dec)
    {
      try
      {
        this.TL.LogMessage("AstroStar", "JD Accuracy:        " + Conversions.ToString(JdTt) + " " + Accuracy.ToString());
        this.TL.LogMessage("AstroStar", "  Star.RA:          " + this.Utl.HoursToHMS(Star.RA, ":", ":", "", 3));
        this.TL.LogMessage("AstroStar", "  Dec:              " + this.Utl.DegreesToDMS(Star.Dec, ":", ":", "", 3));
        this.TL.LogMessage("AstroStar", "  Catalog:          " + Star.Catalog);
        this.TL.LogMessage("AstroStar", "  Parallax:         " + Conversions.ToString(Star.Parallax));
        this.TL.LogMessage("AstroStar", "  ProMoDec:         " + Conversions.ToString(Star.ProMoDec));
        this.TL.LogMessage("AstroStar", "  ProMoRA:          " + Conversions.ToString(Star.ProMoRA));
        this.TL.LogMessage("AstroStar", "  RadialVelocity:   " + Conversions.ToString(Star.RadialVelocity));
        this.TL.LogMessage("AstroStar", "  StarName:         " + Star.StarName);
        this.TL.LogMessage("AstroStar", "  StarNumber:       " + Conversions.ToString(Star.StarNumber));
      }
      catch (Exception ex)
      {
        //ProjectData.SetProjectError(ex);
        this.TL.LogMessageCrLf("AstroStar", "Exception: " + ex.ToString());
        //ProjectData.ClearProjectError();
      }
      if (NOVAS31.Is64Bit())
      {
        short num = NOVAS31.AstroStar64(JdTt, ref Star, Accuracy, ref Ra, ref Dec);
        this.TL.LogMessage("AstroStar", "  64bit - Return Code: " + Conversions.ToString((int) num) + ", RA Dec: " + this.Utl.HoursToHMS(Ra, ":", ":", "", 3) + " " + this.Utl.DegreesToDMS(Dec, ":", ":", "", 3));
        return num;
      }
      short num1 = NOVAS31.AstroStar32(JdTt, ref Star, Accuracy, ref Ra, ref Dec);
      this.TL.LogMessage("AstroStar", "  32bit - Return Code: " + Conversions.ToString((int) num1) + ", RA Dec: " + this.Utl.HoursToHMS(Ra, ":", ":", "", 3) + " " + this.Utl.DegreesToDMS(Dec, ":", ":", "", 3));
      return num1;
    }

    public void Bary2Obs(double[] Pos, double[] PosObs, ref double[] Pos2, ref double Lighttime)
    {
      PosVector Pos2_1 = new PosVector();
      if (NOVAS31.Is64Bit())
      {
        PosVector posVec1 = NOVAS31.ArrToPosVec(Pos);
        PosVector posVec2 = NOVAS31.ArrToPosVec(PosObs);
        NOVAS31.Bary2Obs64(ref posVec1, ref posVec2, ref Pos2_1, ref Lighttime);
        NOVAS31.PosVecToArr(Pos2_1, ref Pos2);
      }
      else
      {
        PosVector posVec1 = NOVAS31.ArrToPosVec(Pos);
        PosVector posVec2 = NOVAS31.ArrToPosVec(PosObs);
        NOVAS31.Bary2Obs32(ref posVec1, ref posVec2, ref Pos2_1, ref Lighttime);
        NOVAS31.PosVecToArr(Pos2_1, ref Pos2);
      }
    }

    public void CalDate(double Tjd, ref short Year, ref short Month, ref short Day, ref double Hour)
    {
      if (NOVAS31.Is64Bit())
        NOVAS31.CalDate64(Tjd, ref Year, ref Month, ref Day, ref Hour);
      else
        NOVAS31.CalDate32(Tjd, ref Year, ref Month, ref Day, ref Hour);
    }

    public short Cel2Ter(double JdHigh, double JdLow, double DeltaT, ASCOM.Astrometry.Method Method, Accuracy Accuracy, OutputVectorOption OutputOption, double xp, double yp, double[] VecT, ref double[] VecC)
    {
      PosVector VecC1 = new PosVector();
      short num1;
      if (NOVAS31.Is64Bit())
      {
        double JdHigh1 = JdHigh;
        double JdLow1 = JdLow;
        double DeltaT1 = DeltaT;
        int num2 = (int) Method;
        int num3 = (int) Accuracy;
        int num4 = (int) OutputOption;
        double x = xp;
        double y = yp;
        PosVector posVec = NOVAS31.ArrToPosVec(VecT);
        num1 = NOVAS31.Cel2Ter64(JdHigh1, JdLow1, DeltaT1, (ASCOM.Astrometry.Method) num2, (Accuracy) num3, (OutputVectorOption) num4, x, y, ref posVec, ref VecC1);
      }
      else
      {
        double JdHigh1 = JdHigh;
        double JdLow1 = JdLow;
        double DeltaT1 = DeltaT;
        int num2 = (int) Method;
        int num3 = (int) Accuracy;
        int num4 = (int) OutputOption;
        double x = xp;
        double y = yp;
        PosVector posVec = NOVAS31.ArrToPosVec(VecT);
        num1 = NOVAS31.Cel2Ter32(JdHigh1, JdLow1, DeltaT1, (ASCOM.Astrometry.Method) num2, (Accuracy) num3, (OutputVectorOption) num4, x, y, ref posVec, ref VecC1);
      }
      NOVAS31.PosVecToArr(VecC1, ref VecC);
      return num1;
    }

    public short CelPole(double Tjd, PoleOffsetCorrection Type, double Dpole1, double Dpole2)
    {
      if (NOVAS31.Is64Bit())
        return NOVAS31.CelPole64(Tjd, Type, Dpole1, Dpole2);
      return NOVAS31.CelPole32(Tjd, Type, Dpole1, Dpole2);
    }

    public short CioArray(double JdTdb, int NPts, ref ArrayList Cio)
    {
      RAOfCioArray Cio1 = new RAOfCioArray();
      Cio1.Initialise();
      short num = !NOVAS31.Is64Bit() ? NOVAS31.CioArray32(JdTdb, NPts, ref Cio1) : NOVAS31.CioArray64(JdTdb, NPts, ref Cio1);
      NOVAS31.RACioArrayStructureToArr(Cio1, ref Cio);
      return num;
    }

    public short CioBasis(double JdTdbEquionx, double RaCioEquionx, ReferenceSystem RefSys, Accuracy Accuracy, ref double x, ref double y, ref double z)
    {
      if (NOVAS31.Is64Bit())
        return NOVAS31.CioBasis64(JdTdbEquionx, RaCioEquionx, RefSys, Accuracy, ref x, ref y, ref z);
      return NOVAS31.CioBasis32(JdTdbEquionx, RaCioEquionx, RefSys, Accuracy, ref x, ref y, ref z);
    }

    public short CioLocation(double JdTdb, Accuracy Accuracy, ref double RaCio, ref ReferenceSystem RefSys)
    {
      if (NOVAS31.Is64Bit())
        return NOVAS31.CioLocation64(JdTdb, Accuracy, ref RaCio, ref RefSys);
      return NOVAS31.CioLocation32(JdTdb, Accuracy, ref RaCio, ref RefSys);
    }

    public short CioRa(double JdTt, Accuracy Accuracy, ref double RaCio)
    {
      if (NOVAS31.Is64Bit())
        return NOVAS31.CioRa64(JdTt, Accuracy, ref RaCio);
      return NOVAS31.CioRa32(JdTt, Accuracy, ref RaCio);
    }

    public double DLight(double[] Pos1, double[] PosObs)
    {
      if (NOVAS31.Is64Bit())
      {
        PosVector posVec1 = NOVAS31.ArrToPosVec(Pos1);
        PosVector posVec2 = NOVAS31.ArrToPosVec(PosObs);
        return NOVAS31.DLight64(ref posVec1, ref posVec2);
      }
      PosVector posVec3 = NOVAS31.ArrToPosVec(Pos1);
      PosVector posVec4 = NOVAS31.ArrToPosVec(PosObs);
      return NOVAS31.DLight32(ref posVec3, ref posVec4);
    }

    public short Ecl2EquVec(double JdTt, CoordSys CoordSys, Accuracy Accuracy, double[] Pos1, ref double[] Pos2)
    {
      PosVector Pos2_1 = new PosVector();
      short num1;
      if (NOVAS31.Is64Bit())
      {
        double JdTt1 = JdTt;
        int num2 = (int) CoordSys;
        int num3 = (int) Accuracy;
        PosVector posVec = NOVAS31.ArrToPosVec(Pos1);
        num1 = NOVAS31.Ecl2EquVec64(JdTt1, (CoordSys) num2, (Accuracy) num3, ref posVec, ref Pos2_1);
      }
      else
      {
        double JdTt1 = JdTt;
        int num2 = (int) CoordSys;
        int num3 = (int) Accuracy;
        PosVector posVec = NOVAS31.ArrToPosVec(Pos1);
        num1 = NOVAS31.Ecl2EquVec32(JdTt1, (CoordSys) num2, (Accuracy) num3, ref posVec, ref Pos2_1);
      }
      NOVAS31.PosVecToArr(Pos2_1, ref Pos2);
      return num1;
    }

    public double EeCt(double JdHigh, double JdLow, Accuracy Accuracy)
    {
      if (NOVAS31.Is64Bit())
        return NOVAS31.EeCt64(JdHigh, JdLow, Accuracy);
      return NOVAS31.EeCt32(JdHigh, JdLow, Accuracy);
    }

    public short Ephemeris(double[] Jd, Object3 CelObj, Origin Origin, Accuracy Accuracy, ref double[] Pos, ref double[] Vel)
    {
      PosVector Pos1 = new PosVector();
      VelVector Vel1 = new VelVector();
      JDHighPrecision Jd1;
      Jd1.JDPart1 = Jd[0];
      Jd1.JDPart2 = Jd[1];
      short num1;
      if (NOVAS31.Is64Bit())
      {
        Object3Internal CelObj1 = this.O3IFromObject3(CelObj);
        int num2 = (int) Origin;
        int num3 = (int) Accuracy;
        num1 = NOVAS31.Ephemeris64(ref Jd1, ref CelObj1, (Origin) num2, (Accuracy) num3, ref Pos1, ref Vel1);
      }
      else
      {
        Object3Internal CelObj1 = this.O3IFromObject3(CelObj);
        int num2 = (int) Origin;
        int num3 = (int) Accuracy;
        num1 = NOVAS31.Ephemeris32(ref Jd1, ref CelObj1, (Origin) num2, (Accuracy) num3, ref Pos1, ref Vel1);
      }
      NOVAS31.PosVecToArr(Pos1, ref Pos);
      NOVAS31.VelVecToArr(Vel1, ref Vel);
      return num1;
    }

    public short Equ2Ecl(double JdTt, CoordSys CoordSys, Accuracy Accuracy, double Ra, double Dec, ref double ELon, ref double ELat)
    {
      if (NOVAS31.Is64Bit())
        return NOVAS31.Equ2Ecl64(JdTt, CoordSys, Accuracy, Ra, Dec, ref ELon, ref ELat);
      return NOVAS31.Equ2Ecl32(JdTt, CoordSys, Accuracy, Ra, Dec, ref ELon, ref ELat);
    }

    public short Equ2EclVec(double JdTt, CoordSys CoordSys, Accuracy Accuracy, double[] Pos1, ref double[] Pos2)
    {
      PosVector Pos2_1 = new PosVector();
      short num1;
      if (NOVAS31.Is64Bit())
      {
        double JdTt1 = JdTt;
        int num2 = (int) CoordSys;
        int num3 = (int) Accuracy;
        PosVector posVec = NOVAS31.ArrToPosVec(Pos1);
        num1 = NOVAS31.Equ2EclVec64(JdTt1, (CoordSys) num2, (Accuracy) num3, ref posVec, ref Pos2_1);
      }
      else
      {
        double JdTt1 = JdTt;
        int num2 = (int) CoordSys;
        int num3 = (int) Accuracy;
        PosVector posVec = NOVAS31.ArrToPosVec(Pos1);
        num1 = NOVAS31.Equ2EclVec32(JdTt1, (CoordSys) num2, (Accuracy) num3, ref posVec, ref Pos2_1);
      }
      NOVAS31.PosVecToArr(Pos2_1, ref Pos2);
      return num1;
    }

    public void Equ2Gal(double RaI, double DecI, ref double GLon, ref double GLat)
    {
      if (NOVAS31.Is64Bit())
        NOVAS31.Equ2Gal64(RaI, DecI, ref GLon, ref GLat);
      else
        NOVAS31.Equ2Gal32(RaI, DecI, ref GLon, ref GLat);
    }

    public void Equ2Hor(double Jd_Ut1, double DeltT, Accuracy Accuracy, double xp, double yp, OnSurface Location, double Ra, double Dec, RefractionOption RefOption, ref double Zd, ref double Az, ref double RaR, ref double DecR)
    {
      try
      {
        this.TL.LogMessage("Equ2Hor", "JD Accuracy RA DEC:     " + Conversions.ToString(Jd_Ut1) + " " + Accuracy.ToString() + " " + this.Utl.HoursToHMS(Ra, ":", ":", "", 3) + " " + this.Utl.DegreesToDMS(Dec, ":", ":", "", 3));
        this.TL.LogMessage("Equ2Hor", "  DeltaT:               " + Conversions.ToString(DeltT));
        this.TL.LogMessage("Equ2Hor", "  xp:                   " + Conversions.ToString(xp));
        this.TL.LogMessage("Equ2Hor", "  yp:                   " + Conversions.ToString(yp));
        this.TL.LogMessage("Equ2Hor", "  Refraction:           " + RefOption.ToString());
        this.TL.LogMessage("Equ2Hor", "  Location.Height:      " + Conversions.ToString(Location.Height));
        this.TL.LogMessage("Equ2Hor", "  Location.Latitude:    " + Conversions.ToString(Location.Latitude));
        this.TL.LogMessage("Equ2Hor", "  Location.Longitude:   " + Conversions.ToString(Location.Longitude));
        this.TL.LogMessage("Equ2Hor", "  Location.Pressure:    " + Conversions.ToString(Location.Pressure));
        this.TL.LogMessage("Equ2Hor", "  Location.Temperature: " + Conversions.ToString(Location.Temperature));
      }
      catch (Exception ex)
      {
        //ProjectData.SetProjectError(ex);
        this.TL.LogMessageCrLf("Equ2Hor", "Exception: " + ex.ToString());
        //ProjectData.ClearProjectError();
      }
      if (NOVAS31.Is64Bit())
      {
        NOVAS31.Equ2Hor64(Jd_Ut1, DeltT, Accuracy, xp, yp, ref Location, Ra, Dec, RefOption, ref Zd, ref Az, ref RaR, ref DecR);
        this.TL.LogMessage("Equ2Hor", "  64bit - RA Dec: " + this.Utl.HoursToHMS(RaR, ":", ":", "", 3) + " " + this.Utl.DegreesToDMS(DecR, ":", ":", "", 3));
      }
      else
      {
        NOVAS31.Equ2Hor32(Jd_Ut1, DeltT, Accuracy, xp, yp, ref Location, Ra, Dec, RefOption, ref Zd, ref Az, ref RaR, ref DecR);
        this.TL.LogMessage("Equ2Hor", "  32bit - RA Dec: " + this.Utl.HoursToHMS(RaR, ":", ":", "", 3) + " " + this.Utl.DegreesToDMS(DecR, ":", ":", "", 3));
      }
    }

    public double Era(double JdHigh, double JdLow)
    {
      if (NOVAS31.Is64Bit())
        return NOVAS31.Era64(JdHigh, JdLow);
      return NOVAS31.Era32(JdHigh, JdLow);
    }

    public void ETilt(double JdTdb, Accuracy Accuracy, ref double Mobl, ref double Tobl, ref double Ee, ref double Dpsi, ref double Deps)
    {
      if (NOVAS31.Is64Bit())
        NOVAS31.ETilt64(JdTdb, Accuracy, ref Mobl, ref Tobl, ref Ee, ref Dpsi, ref Deps);
      else
        NOVAS31.ETilt32(JdTdb, Accuracy, ref Mobl, ref Tobl, ref Ee, ref Dpsi, ref Deps);
    }

    public void FrameTie(double[] Pos1, FrameConversionDirection Direction, ref double[] Pos2)
    {
      PosVector Pos2_1 = new PosVector();
      if (NOVAS31.Is64Bit())
      {
        PosVector posVec = NOVAS31.ArrToPosVec(Pos1);
        NOVAS31.FrameTie64(ref posVec, Direction, ref Pos2_1);
      }
      else
      {
        PosVector posVec = NOVAS31.ArrToPosVec(Pos1);
        NOVAS31.FrameTie32(ref posVec, Direction, ref Pos2_1);
      }
      NOVAS31.PosVecToArr(Pos2_1, ref Pos2);
    }

    public void FundArgs(double t, ref double[] a)
    {
      FundamentalArgs a1 = new FundamentalArgs();
      if (NOVAS31.Is64Bit())
        NOVAS31.FundArgs64(t, ref a1);
      else
        NOVAS31.FundArgs32(t, ref a1);
      a[0] = a1.l;
      a[1] = a1.ldash;
      a[2] = a1.F;
      a[3] = a1.D;
      a[4] = a1.Omega;
    }

    public short Gcrs2Equ(double JdTt, CoordSys CoordSys, Accuracy Accuracy, double RaG, double DecG, ref double Ra, ref double Dec)
    {
      if (NOVAS31.Is64Bit())
        return NOVAS31.Gcrs2Equ64(JdTt, CoordSys, Accuracy, RaG, DecG, ref Ra, ref Dec);
      return NOVAS31.Gcrs2Equ32(JdTt, CoordSys, Accuracy, RaG, DecG, ref Ra, ref Dec);
    }

    public short GeoPosVel(double JdTt, double DeltaT, Accuracy Accuracy, Observer Obs, ref double[] Pos, ref double[] Vel)
    {
      PosVector Pos1 = new PosVector();
      VelVector Vel1 = new VelVector();
      short num = !NOVAS31.Is64Bit() ? NOVAS31.GeoPosVel32(JdTt, DeltaT, Accuracy, ref Obs, ref Pos1, ref Vel1) : NOVAS31.GeoPosVel64(JdTt, DeltaT, Accuracy, ref Obs, ref Pos1, ref Vel1);
      NOVAS31.PosVecToArr(Pos1, ref Pos);
      NOVAS31.VelVecToArr(Vel1, ref Vel);
      return num;
    }

    public short GravDef(double JdTdb, EarthDeflection LocCode, Accuracy Accuracy, double[] Pos1, double[] PosObs, ref double[] Pos2)
    {
      PosVector Pos2_1 = new PosVector();
      short num1;
      if (NOVAS31.Is64Bit())
      {
        double JdTdb1 = JdTdb;
        int num2 = (int) LocCode;
        int num3 = (int) Accuracy;
        PosVector posVec1 = NOVAS31.ArrToPosVec(Pos1);
        PosVector posVec2 = NOVAS31.ArrToPosVec(PosObs);
        num1 = NOVAS31.GravDef64(JdTdb1, (EarthDeflection) num2, (Accuracy) num3, ref posVec1, ref posVec2, ref Pos2_1);
      }
      else
      {
        double JdTdb1 = JdTdb;
        int num2 = (int) LocCode;
        int num3 = (int) Accuracy;
        PosVector posVec1 = NOVAS31.ArrToPosVec(Pos1);
        PosVector posVec2 = NOVAS31.ArrToPosVec(PosObs);
        num1 = NOVAS31.GravDef32(JdTdb1, (EarthDeflection) num2, (Accuracy) num3, ref posVec1, ref posVec2, ref Pos2_1);
      }
      NOVAS31.PosVecToArr(Pos2_1, ref Pos2);
      return num1;
    }

    public void GravVec(double[] Pos1, double[] PosObs, double[] PosBody, double RMass, ref double[] Pos2)
    {
      PosVector Pos2_1 = new PosVector();
      if (NOVAS31.Is64Bit())
      {
        PosVector posVec1 = NOVAS31.ArrToPosVec(Pos1);
        PosVector posVec2 = NOVAS31.ArrToPosVec(PosObs);
        PosVector posVec3 = NOVAS31.ArrToPosVec(PosBody);
        double RMass1 = RMass;
        NOVAS31.GravVec64(ref posVec1, ref posVec2, ref posVec3, RMass1, ref Pos2_1);
      }
      else
      {
        PosVector posVec1 = NOVAS31.ArrToPosVec(Pos1);
        PosVector posVec2 = NOVAS31.ArrToPosVec(PosObs);
        PosVector posVec3 = NOVAS31.ArrToPosVec(PosBody);
        double RMass1 = RMass;
        NOVAS31.GravVec32(ref posVec1, ref posVec2, ref posVec3, RMass1, ref Pos2_1);
      }
      NOVAS31.PosVecToArr(Pos2_1, ref Pos2);
    }

    public double IraEquinox(double JdTdb, EquinoxType Equinox, Accuracy Accuracy)
    {
      if (NOVAS31.Is64Bit())
        return NOVAS31.IraEquinox64(JdTdb, Equinox, Accuracy);
      return NOVAS31.IraEquinox32(JdTdb, Equinox, Accuracy);
    }

    public double JulianDate(short Year, short Month, short Day, double Hour)
    {
      if (NOVAS31.Is64Bit())
        return NOVAS31.JulianDate64(Year, Month, Day, Hour);
      return NOVAS31.JulianDate32(Year, Month, Day, Hour);
    }

    public short LightTime(double JdTdb, Object3 SsObject, double[] PosObs, double TLight0, Accuracy Accuracy, ref double[] Pos, ref double TLight)
    {
      PosVector Pos1 = new PosVector();
      short num1;
      if (NOVAS31.Is64Bit())
      {
        double JdTdb1 = JdTdb;
        Object3Internal SsObject1 = this.O3IFromObject3(SsObject);
        PosVector posVec = NOVAS31.ArrToPosVec(PosObs);
        double TLight0_1 = TLight0;
        int num2 = (int) Accuracy;
        num1 = NOVAS31.LightTime64(JdTdb1, ref SsObject1, ref posVec, TLight0_1, (Accuracy) num2, ref Pos1, ref TLight);
      }
      else
      {
        double JdTdb1 = JdTdb;
        Object3Internal SsObject1 = this.O3IFromObject3(SsObject);
        PosVector posVec = NOVAS31.ArrToPosVec(PosObs);
        double TLight0_1 = TLight0;
        int num2 = (int) Accuracy;
        num1 = NOVAS31.LightTime32(JdTdb1, ref SsObject1, ref posVec, TLight0_1, (Accuracy) num2, ref Pos1, ref TLight);
      }
      NOVAS31.PosVecToArr(Pos1, ref Pos);
      return num1;
    }

    public void LimbAngle(double[] PosObj, double[] PosObs, ref double LimbAng, ref double NadirAng)
    {
      if (NOVAS31.Is64Bit())
      {
        PosVector posVec1 = NOVAS31.ArrToPosVec(PosObj);
        PosVector posVec2 = NOVAS31.ArrToPosVec(PosObs);
        NOVAS31.LimbAngle64(ref posVec1, ref posVec2, ref LimbAng, ref NadirAng);
      }
      else
      {
        PosVector posVec1 = NOVAS31.ArrToPosVec(PosObj);
        PosVector posVec2 = NOVAS31.ArrToPosVec(PosObs);
        NOVAS31.LimbAngle32(ref posVec1, ref posVec2, ref LimbAng, ref NadirAng);
      }
    }

    public short LocalPlanet(double JdTt, Object3 SsBody, double DeltaT, OnSurface Position, Accuracy Accuracy, ref double Ra, ref double Dec, ref double Dis)
    {
      if (NOVAS31.Is64Bit())
      {
        double JdTt1 = JdTt;
        Object3Internal SsBody1 = this.O3IFromObject3(SsBody);
        double DeltaT1 = DeltaT;
        int num = (int) Accuracy;
        return NOVAS31.LocalPlanet64(JdTt1, ref SsBody1, DeltaT1, ref Position, (Accuracy) num, ref Ra, ref Dec, ref Dis);
      }
      double JdTt2 = JdTt;
      Object3Internal SsBody2 = this.O3IFromObject3(SsBody);
      double DeltaT2 = DeltaT;
      int num1 = (int) Accuracy;
      return NOVAS31.LocalPlanet32(JdTt2, ref SsBody2, DeltaT2, ref Position, (Accuracy) num1, ref Ra, ref Dec, ref Dis);
    }

    public short LocalStar(double JdTt, double DeltaT, CatEntry3 Star, OnSurface Position, Accuracy Accuracy, ref double Ra, ref double Dec)
    {
      if (NOVAS31.Is64Bit())
        return NOVAS31.LocalStar64(JdTt, DeltaT, ref Star, ref Position, Accuracy, ref Ra, ref Dec);
      return NOVAS31.LocalStar32(JdTt, DeltaT, ref Star, ref Position, Accuracy, ref Ra, ref Dec);
    }

    public void MakeCatEntry(string StarName, string Catalog, int StarNum, double Ra, double Dec, double PmRa, double PmDec, double Parallax, double RadVel, ref CatEntry3 Star)
    {
      if (NOVAS31.Is64Bit())
        NOVAS31.MakeCatEntry64(StarName, Catalog, StarNum, Ra, Dec, PmRa, PmDec, Parallax, RadVel, ref Star);
      else
        NOVAS31.MakeCatEntry32(StarName, Catalog, StarNum, Ra, Dec, PmRa, PmDec, Parallax, RadVel, ref Star);
    }

    public void MakeInSpace(double[] ScPos, double[] ScVel, ref InSpace ObsSpace)
    {
      if (NOVAS31.Is64Bit())
      {
        PosVector posVec = NOVAS31.ArrToPosVec(ScPos);
        VelVector velVec = NOVAS31.ArrToVelVec(ScVel);
        NOVAS31.MakeInSpace64(ref posVec, ref velVec, ref ObsSpace);
      }
      else
      {
        PosVector posVec = NOVAS31.ArrToPosVec(ScPos);
        VelVector velVec = NOVAS31.ArrToVelVec(ScVel);
        NOVAS31.MakeInSpace32(ref posVec, ref velVec, ref ObsSpace);
      }
    }

    public short MakeObject(ASCOM.Astrometry.ObjectType Type, short Number, string Name, CatEntry3 StarData, ref Object3 CelObj)
    {
      Object3Internal CelObj1 = new Object3Internal();
      short num = !NOVAS31.Is64Bit() ? NOVAS31.MakeObject32(Type, Number, Name, ref StarData, ref CelObj1) : NOVAS31.MakeObject64(Type, Number, Name, ref StarData, ref CelObj1);
      this.O3FromO3Internal(CelObj1, ref CelObj);
      return num;
    }

    public short MakeObserver(ObserverLocation Where, OnSurface ObsSurface, InSpace ObsSpace, ref Observer Obs)
    {
      if (NOVAS31.Is64Bit())
        return NOVAS31.MakeObserver64(Where, ref ObsSurface, ref ObsSpace, ref Obs);
      return NOVAS31.MakeObserver32(Where, ref ObsSurface, ref ObsSpace, ref Obs);
    }

    public void MakeObserverAtGeocenter(ref Observer ObsAtGeocenter)
    {
      if (NOVAS31.Is64Bit())
        NOVAS31.MakeObserverAtGeocenter64(ref ObsAtGeocenter);
      else
        NOVAS31.MakeObserverAtGeocenter32(ref ObsAtGeocenter);
    }

    public void MakeObserverInSpace(double[] ScPos, double[] ScVel, ref Observer ObsInSpace)
    {
      if (NOVAS31.Is64Bit())
      {
        PosVector posVec = NOVAS31.ArrToPosVec(ScPos);
        VelVector velVec = NOVAS31.ArrToVelVec(ScVel);
        NOVAS31.MakeObserverInSpace64(ref posVec, ref velVec, ref ObsInSpace);
      }
      else
      {
        PosVector posVec = NOVAS31.ArrToPosVec(ScPos);
        VelVector velVec = NOVAS31.ArrToVelVec(ScVel);
        NOVAS31.MakeObserverInSpace32(ref posVec, ref velVec, ref ObsInSpace);
      }
    }

    public void MakeObserverOnSurface(double Latitude, double Longitude, double Height, double Temperature, double Pressure, ref Observer ObsOnSurface)
    {
      if (NOVAS31.Is64Bit())
        NOVAS31.MakeObserverOnSurface64(Latitude, Longitude, Height, Temperature, Pressure, ref ObsOnSurface);
      else
        NOVAS31.MakeObserverOnSurface32(Latitude, Longitude, Height, Temperature, Pressure, ref ObsOnSurface);
    }

    public void MakeOnSurface(double Latitude, double Longitude, double Height, double Temperature, double Pressure, ref OnSurface ObsSurface)
    {
      if (NOVAS31.Is64Bit())
        NOVAS31.MakeOnSurface64(Latitude, Longitude, Height, Temperature, Pressure, ref ObsSurface);
      else
        NOVAS31.MakeOnSurface32(Latitude, Longitude, Height, Temperature, Pressure, ref ObsSurface);
    }

    public double MeanObliq(double JdTdb)
    {
      if (NOVAS31.Is64Bit())
        return NOVAS31.MeanObliq64(JdTdb);
      return NOVAS31.MeanObliq32(JdTdb);
    }

    public short MeanStar(double JdTt, double Ra, double Dec, Accuracy Accuracy, ref double IRa, ref double IDec)
    {
      if (NOVAS31.Is64Bit())
        return NOVAS31.MeanStar64(JdTt, Ra, Dec, Accuracy, ref IRa, ref IDec);
      return NOVAS31.MeanStar32(JdTt, Ra, Dec, Accuracy, ref IRa, ref IDec);
    }

    public double NormAng(double Angle)
    {
      if (NOVAS31.Is64Bit())
        return NOVAS31.NormAng64(Angle);
      return NOVAS31.NormAng32(Angle);
    }

    public void Nutation(double JdTdb, NutationDirection Direction, Accuracy Accuracy, double[] Pos, ref double[] Pos2)
    {
      PosVector Pos2_1 = new PosVector();
      if (NOVAS31.Is64Bit())
      {
        double JdTdb1 = JdTdb;
        int num1 = (int) Direction;
        int num2 = (int) Accuracy;
        PosVector posVec = NOVAS31.ArrToPosVec(Pos);
        NOVAS31.Nutation64(JdTdb1, (NutationDirection) num1, (Accuracy) num2, ref posVec, ref Pos2_1);
      }
      else
      {
        double JdTdb1 = JdTdb;
        int num1 = (int) Direction;
        int num2 = (int) Accuracy;
        PosVector posVec = NOVAS31.ArrToPosVec(Pos);
        NOVAS31.Nutation32(JdTdb1, (NutationDirection) num1, (Accuracy) num2, ref posVec, ref Pos2_1);
      }
      NOVAS31.PosVecToArr(Pos2_1, ref Pos2);
    }

    public void NutationAngles(double t, Accuracy Accuracy, ref double DPsi, ref double DEps)
    {
      if (NOVAS31.Is64Bit())
        NOVAS31.NutationAngles64(t, Accuracy, ref DPsi, ref DEps);
      else
        NOVAS31.NutationAngles32(t, Accuracy, ref DPsi, ref DEps);
    }

    public short Place(double JdTt, Object3 CelObject, Observer Location, double DeltaT, CoordSys CoordSys, Accuracy Accuracy, ref SkyPos Output)
    {
      if (NOVAS31.Is64Bit())
      {
        double JdTt1 = JdTt;
        Object3Internal CelObject1 = this.O3IFromObject3(CelObject);
        double DeltaT1 = DeltaT;
        int num1 = (int) CoordSys;
        int num2 = (int) Accuracy;
        return NOVAS31.Place64(JdTt1, ref CelObject1, ref Location, DeltaT1, (CoordSys) num1, (Accuracy) num2, ref Output);
      }
      double JdTt2 = JdTt;
      Object3Internal CelObject2 = this.O3IFromObject3(CelObject);
      double DeltaT2 = DeltaT;
      int num3 = (int) CoordSys;
      int num4 = (int) Accuracy;
      return NOVAS31.Place32(JdTt2, ref CelObject2, ref Location, DeltaT2, (CoordSys) num3, (Accuracy) num4, ref Output);
    }

    public short Precession(double JdTdb1, double[] Pos1, double JdTdb2, ref double[] Pos2)
    {
      PosVector Pos2_1 = new PosVector();
      short num;
      if (NOVAS31.Is64Bit())
      {
        double JdTdb1_1 = JdTdb1;
        PosVector posVec = NOVAS31.ArrToPosVec(Pos1);
        double JdTdb2_1 = JdTdb2;
        num = NOVAS31.Precession64(JdTdb1_1, ref posVec, JdTdb2_1, ref Pos2_1);
      }
      else
      {
        double JdTdb1_1 = JdTdb1;
        PosVector posVec = NOVAS31.ArrToPosVec(Pos1);
        double JdTdb2_1 = JdTdb2;
        num = NOVAS31.Precession32(JdTdb1_1, ref posVec, JdTdb2_1, ref Pos2_1);
      }
      NOVAS31.PosVecToArr(Pos2_1, ref Pos2);
      return num;
    }

    public void ProperMotion(double JdTdb1, double[] Pos, double[] Vel, double JdTdb2, ref double[] Pos2)
    {
      PosVector Pos2_1 = new PosVector();
      if (NOVAS31.Is64Bit())
      {
        double JdTdb1_1 = JdTdb1;
        PosVector posVec = NOVAS31.ArrToPosVec(Pos);
        VelVector velVec = NOVAS31.ArrToVelVec(Vel);
        double JdTdb2_1 = JdTdb2;
        NOVAS31.ProperMotion64(JdTdb1_1, ref posVec, ref velVec, JdTdb2_1, ref Pos2_1);
      }
      else
      {
        double JdTdb1_1 = JdTdb1;
        PosVector posVec = NOVAS31.ArrToPosVec(Pos);
        VelVector velVec = NOVAS31.ArrToVelVec(Vel);
        double JdTdb2_1 = JdTdb2;
        NOVAS31.ProperMotion32(JdTdb1_1, ref posVec, ref velVec, JdTdb2_1, ref Pos2_1);
      }
      NOVAS31.PosVecToArr(Pos2_1, ref Pos2);
    }

    public void RaDec2Vector(double Ra, double Dec, double Dist, ref double[] Vector)
    {
      PosVector Vector1 = new PosVector();
      if (NOVAS31.Is64Bit())
        NOVAS31.RaDec2Vector64(Ra, Dec, Dist, ref Vector1);
      else
        NOVAS31.RaDec2Vector32(Ra, Dec, Dist, ref Vector1);
      NOVAS31.PosVecToArr(Vector1, ref Vector);
    }

    public void RadVel(Object3 CelObject, double[] Pos, double[] Vel, double[] VelObs, double DObsGeo, double DObsSun, double DObjSun, ref double Rv)
    {
      if (NOVAS31.Is64Bit())
      {
        Object3Internal CelObject1 = this.O3IFromObject3(CelObject);
        PosVector posVec = NOVAS31.ArrToPosVec(Pos);
        VelVector velVec1 = NOVAS31.ArrToVelVec(Vel);
        VelVector velVec2 = NOVAS31.ArrToVelVec(VelObs);
        double DObsGeo1 = DObsGeo;
        double DObsSun1 = DObsSun;
        double DObjSun1 = DObjSun;
        NOVAS31.RadVel64(ref CelObject1, ref posVec, ref velVec1, ref velVec2, DObsGeo1, DObsSun1, DObjSun1, ref Rv);
      }
      else
      {
        Object3Internal CelObject1 = this.O3IFromObject3(CelObject);
        PosVector posVec = NOVAS31.ArrToPosVec(Pos);
        VelVector velVec1 = NOVAS31.ArrToVelVec(Vel);
        VelVector velVec2 = NOVAS31.ArrToVelVec(VelObs);
        double DObsGeo1 = DObsGeo;
        double DObsSun1 = DObsSun;
        double DObjSun1 = DObjSun;
        NOVAS31.RadVel32(ref CelObject1, ref posVec, ref velVec1, ref velVec2, DObsGeo1, DObsSun1, DObjSun1, ref Rv);
      }
    }

    public double Refract(OnSurface Location, RefractionOption RefOption, double ZdObs)
    {
      if (NOVAS31.Is64Bit())
        return NOVAS31.Refract64(ref Location, RefOption, ZdObs);
      return NOVAS31.Refract32(ref Location, RefOption, ZdObs);
    }

    public short SiderealTime(double JdHigh, double JdLow, double DeltaT, GstType GstType, ASCOM.Astrometry.Method Method, Accuracy Accuracy, ref double Gst)
    {
      if (NOVAS31.Is64Bit())
        return NOVAS31.SiderealTime64(JdHigh, JdLow, DeltaT, GstType, Method, Accuracy, ref Gst);
      return NOVAS31.SiderealTime32(JdHigh, JdLow, DeltaT, GstType, Method, Accuracy, ref Gst);
    }

    public void Spin(double Angle, double[] Pos1, ref double[] Pos2)
    {
      PosVector Pos2_1 = new PosVector();
      if (NOVAS31.Is64Bit())
      {
        double Angle1 = Angle;
        PosVector posVec = NOVAS31.ArrToPosVec(Pos1);
        NOVAS31.Spin64(Angle1, ref posVec, ref Pos2_1);
      }
      else
      {
        double Angle1 = Angle;
        PosVector posVec = NOVAS31.ArrToPosVec(Pos1);
        NOVAS31.Spin32(Angle1, ref posVec, ref Pos2_1);
      }
      NOVAS31.PosVecToArr(Pos2_1, ref Pos2);
    }

    public void StarVectors(CatEntry3 Star, ref double[] Pos, ref double[] Vel)
    {
      PosVector Pos1 = new PosVector();
      VelVector Vel1 = new VelVector();
      if (NOVAS31.Is64Bit())
        NOVAS31.StarVectors64(ref Star, ref Pos1, ref Vel1);
      else
        NOVAS31.StarVectors32(ref Star, ref Pos1, ref Vel1);
      NOVAS31.PosVecToArr(Pos1, ref Pos);
      NOVAS31.VelVecToArr(Vel1, ref Vel);
    }

    public void Tdb2Tt(double TdbJd, ref double TtJd, ref double SecDiff)
    {
      if (NOVAS31.Is64Bit())
        NOVAS31.Tdb2Tt64(TdbJd, ref TtJd, ref SecDiff);
      else
        NOVAS31.Tdb2Tt32(TdbJd, ref TtJd, ref SecDiff);
    }

    public short Ter2Cel(double JdHigh, double JdLow, double DeltaT, ASCOM.Astrometry.Method Method, Accuracy Accuracy, OutputVectorOption OutputOption, double xp, double yp, double[] VecT, ref double[] VecC)
    {
      PosVector VecC1 = new PosVector();
      short num1;
      if (NOVAS31.Is64Bit())
      {
        double JdHigh1 = JdHigh;
        double JdLow1 = JdLow;
        double DeltaT1 = DeltaT;
        int num2 = (int) Method;
        int num3 = (int) Accuracy;
        int num4 = (int) OutputOption;
        double x = xp;
        double y = yp;
        PosVector posVec = NOVAS31.ArrToPosVec(VecT);
        num1 = NOVAS31.Ter2Cel64(JdHigh1, JdLow1, DeltaT1, (ASCOM.Astrometry.Method) num2, (Accuracy) num3, (OutputVectorOption) num4, x, y, ref posVec, ref VecC1);
      }
      else
      {
        double JdHigh1 = JdHigh;
        double JdLow1 = JdLow;
        double DeltaT1 = DeltaT;
        int num2 = (int) Method;
        int num3 = (int) Accuracy;
        int num4 = (int) OutputOption;
        double x = xp;
        double y = yp;
        PosVector posVec = NOVAS31.ArrToPosVec(VecT);
        num1 = NOVAS31.Ter2Cel32(JdHigh1, JdLow1, DeltaT1, (ASCOM.Astrometry.Method) num2, (Accuracy) num3, (OutputVectorOption) num4, x, y, ref posVec, ref VecC1);
      }
      NOVAS31.PosVecToArr(VecC1, ref VecC);
      return num1;
    }

    public void Terra(OnSurface Location, double St, ref double[] Pos, ref double[] Vel)
    {
      PosVector Pos1 = new PosVector();
      VelVector Vel1 = new VelVector();
      if (NOVAS31.Is64Bit())
        NOVAS31.Terra64(ref Location, St, ref Pos1, ref Vel1);
      else
        NOVAS31.Terra32(ref Location, St, ref Pos1, ref Vel1);
      NOVAS31.PosVecToArr(Pos1, ref Pos);
      NOVAS31.VelVecToArr(Vel1, ref Vel);
    }

    public short TopoPlanet(double JdTt, Object3 SsBody, double DeltaT, OnSurface Position, Accuracy Accuracy, ref double Ra, ref double Dec, ref double Dis)
    {
      if (NOVAS31.Is64Bit())
      {
        double JdTt1 = JdTt;
        Object3Internal SsBody1 = this.O3IFromObject3(SsBody);
        double DeltaT1 = DeltaT;
        int num = (int) Accuracy;
        return NOVAS31.TopoPlanet64(JdTt1, ref SsBody1, DeltaT1, ref Position, (Accuracy) num, ref Ra, ref Dec, ref Dis);
      }
      double JdTt2 = JdTt;
      Object3Internal SsBody2 = this.O3IFromObject3(SsBody);
      double DeltaT2 = DeltaT;
      int num1 = (int) Accuracy;
      return NOVAS31.TopoPlanet32(JdTt2, ref SsBody2, DeltaT2, ref Position, (Accuracy) num1, ref Ra, ref Dec, ref Dis);
    }

    public short TopoStar(double JdTt, double DeltaT, CatEntry3 Star, OnSurface Position, Accuracy Accuracy, ref double Ra, ref double Dec)
    {
      short num1 = 0;
            try
      {
        this.TL.LogMessage("TopoStar", "JD Accuracy:            " + Conversions.ToString(JdTt) + " " + Accuracy.ToString());
        this.TL.LogMessage("TopoStar", "  Star.RA:              " + this.Utl.HoursToHMS(Star.RA, ":", ":", "", 3));
        this.TL.LogMessage("TopoStar", "  Dec:                  " + this.Utl.DegreesToDMS(Star.Dec, ":", ":", "", 3));
        this.TL.LogMessage("TopoStar", "  Catalog:              " + Star.Catalog);
        this.TL.LogMessage("TopoStar", "  Parallax:             " + Conversions.ToString(Star.Parallax));
        this.TL.LogMessage("TopoStar", "  ProMoDec:             " + Conversions.ToString(Star.ProMoDec));
        this.TL.LogMessage("TopoStar", "  ProMoRA:              " + Conversions.ToString(Star.ProMoRA));
        this.TL.LogMessage("TopoStar", "  RadialVelocity:       " + Conversions.ToString(Star.RadialVelocity));
        this.TL.LogMessage("TopoStar", "  StarName:             " + Star.StarName);
        this.TL.LogMessage("TopoStar", "  StarNumber:           " + Conversions.ToString(Star.StarNumber));
        this.TL.LogMessage("TopoStar", "  Position.Height:      " + Conversions.ToString(Position.Height));
        this.TL.LogMessage("TopoStar", "  Position.Latitude:    " + Conversions.ToString(Position.Latitude));
        this.TL.LogMessage("TopoStar", "  Position.Longitude:   " + Conversions.ToString(Position.Longitude));
        this.TL.LogMessage("TopoStar", "  Position.Pressure:    " + Conversions.ToString(Position.Pressure));
        this.TL.LogMessage("TopoStar", "  Position.Temperature: " + Conversions.ToString(Position.Temperature));
        if (NOVAS31.Is64Bit())
        {
          short num2 = NOVAS31.TopoStar64(JdTt, DeltaT, ref Star, ref Position, Accuracy, ref Ra, ref Dec);
          this.TL.LogMessage("TopoStar", "  64bit - Return Code: " + Conversions.ToString((int) num2) + ", RA Dec: " + this.Utl.HoursToHMS(Ra, ":", ":", "", 3) + " " + this.Utl.DegreesToDMS(Dec, ":", ":", "", 3));
          num1 = num2;
        }
        else
        {
          short num2 = NOVAS31.TopoStar32(JdTt, DeltaT, ref Star, ref Position, Accuracy, ref Ra, ref Dec);
          this.TL.LogMessage("TopoStar", "  32bit - Return Code: " + Conversions.ToString((int) num2) + ", RA Dec: " + this.Utl.HoursToHMS(Ra, ":", ":", "", 3) + " " + this.Utl.DegreesToDMS(Dec, ":", ":", "", 3));
          num1 = num2;
        }
      }
      catch (Exception ex)
      {
        //ProjectData.SetProjectError(ex);
        this.TL.LogMessageCrLf("TopoStar", "Exception: " + ex.ToString());
        //ProjectData.ClearProjectError();
      }
      return num1;
    }

    public short TransformCat(TransformationOption3 TransformOption, double DateInCat, CatEntry3 InCat, double DateNewCat, string NewCatId, ref CatEntry3 NewCat)
    {
      if (NOVAS31.Is64Bit())
        return NOVAS31.TransformCat64(TransformOption, DateInCat, ref InCat, DateNewCat, NewCatId, ref NewCat);
      return NOVAS31.TransformCat32(TransformOption, DateInCat, ref InCat, DateNewCat, NewCatId, ref NewCat);
    }

    public void TransformHip(CatEntry3 Hipparcos, ref CatEntry3 Hip2000)
    {
      if (NOVAS31.Is64Bit())
        NOVAS31.TransformHip64(ref Hipparcos, ref Hip2000);
      else
        NOVAS31.TransformHip32(ref Hipparcos, ref Hip2000);
    }

    public short Vector2RaDec(double[] Pos, ref double Ra, ref double Dec)
    {
      if (NOVAS31.Is64Bit())
      {
        PosVector posVec = NOVAS31.ArrToPosVec(Pos);
        return NOVAS31.Vector2RaDec64(ref posVec, ref Ra, ref Dec);
      }
      PosVector posVec1 = NOVAS31.ArrToPosVec(Pos);
      return NOVAS31.Vector2RaDec32(ref posVec1, ref Ra, ref Dec);
    }

    public short VirtualPlanet(double JdTt, Object3 SsBody, Accuracy Accuracy, ref double Ra, ref double Dec, ref double Dis)
    {
      if (NOVAS31.Is64Bit())
      {
        double JdTt1 = JdTt;
        Object3Internal SsBody1 = this.O3IFromObject3(SsBody);
        int num = (int) Accuracy;
        return NOVAS31.VirtualPlanet64(JdTt1, ref SsBody1, (Accuracy) num, ref Ra, ref Dec, ref Dis);
      }
      double JdTt2 = JdTt;
      Object3Internal SsBody2 = this.O3IFromObject3(SsBody);
      int num1 = (int) Accuracy;
      return NOVAS31.VirtualPlanet32(JdTt2, ref SsBody2, (Accuracy) num1, ref Ra, ref Dec, ref Dis);
    }

    public short VirtualStar(double JdTt, CatEntry3 Star, Accuracy Accuracy, ref double Ra, ref double Dec)
    {
      if (NOVAS31.Is64Bit())
        return NOVAS31.VirtualStar64(JdTt, ref Star, Accuracy, ref Ra, ref Dec);
      return NOVAS31.VirtualStar32(JdTt, ref Star, Accuracy, ref Ra, ref Dec);
    }

    public void Wobble(double Tjd, TransformationDirection Direction, double xp, double yp, double[] Pos1, ref double[] Pos2)
    {
      PosVector Pos2_1 = new PosVector();
      if (NOVAS31.Is64Bit())
      {
        double Tjd1 = Tjd;
        int num = (int) Direction;
        double x = xp;
        double y = yp;
        PosVector posVec = NOVAS31.ArrToPosVec(Pos1);
        NOVAS31.Wobble64(Tjd1, (short) num, x, y, ref posVec, ref Pos2_1);
      }
      else
      {
        double Tjd1 = Tjd;
        int num = (int) Direction;
        double x = xp;
        double y = yp;
        PosVector posVec = NOVAS31.ArrToPosVec(Pos1);
        NOVAS31.Wobble32(Tjd1, (short) num, x, y, ref posVec, ref Pos2_1);
      }
      NOVAS31.PosVecToArr(Pos2_1, ref Pos2);
    }

    private short Ephem_Open(string Ephem_Name, ref double JD_Begin, ref double JD_End, ref short DENumber)
    {
      return !NOVAS31.Is64Bit() ? NOVAS31.EphemOpen32(Ephem_Name, ref JD_Begin, ref JD_End, ref DENumber) : NOVAS31.EphemOpen64(Ephem_Name, ref JD_Begin, ref JD_End, ref DENumber);
    }

    private short Ephem_Close()
    {
      if (NOVAS31.Is64Bit())
        return NOVAS31.EphemClose64();
      return NOVAS31.EphemClose32();
    }

    private void SetRACIOFile(string FName)
    {
      if (NOVAS31.Is64Bit())
        NOVAS31.SetRACIOFile64(FName);
      else
        NOVAS31.SetRACIOFile32(FName);
    }

    [DllImport("NOVAS31.dll", EntryPoint = "set_racio_file")]
    private static extern void SetRACIOFile32([MarshalAs(UnmanagedType.LPStr)] string FName);

    [DllImport("NOVAS31.dll", EntryPoint = "ephem_close")]
    private static extern short EphemClose32();

    [DllImport("NOVAS31.dll", EntryPoint = "ephem_open")]
    private static extern short EphemOpen32([MarshalAs(UnmanagedType.LPStr)] string Ephem_Name, ref double JD_Begin, ref double JD_End, ref short DENumber);

    [DllImport("NOVAS31.dll", EntryPoint = "planet_ephemeris")]
    private static extern short PlanetEphemeris32(ref JDHighPrecision Tjd, Target Target, Target Center, ref PosVector Position, ref VelVector Velocity);

    [DllImport("NOVAS31.dll", EntryPoint = "readeph")]
    private static extern IntPtr ReadEph32(int Mp, [MarshalAs(UnmanagedType.LPStr)] string Name, double Jd, ref int Err);

    [DllImport("NOVAS31.dll", EntryPoint = "cleaneph")]
    private static extern void CleanEph32();

    [DllImport("NOVAS31.dll", EntryPoint = "solarsystem")]
    private static extern short SolarSystem32(double tjd, short body, short origin, ref PosVector pos, ref VelVector vel);

    [DllImport("NOVAS31.dll", EntryPoint = "state")]
    private static extern short State32(ref JDHighPrecision Jed, Target Target, ref PosVector TargetPos, ref VelVector TargetVel);

    [DllImport("NOVAS31.dll", EntryPoint = "aberration")]
    private static extern void Aberration32(ref PosVector Pos, ref VelVector Vel, double LightTime, ref PosVector Pos2);

    [DllImport("NOVAS31.dll", EntryPoint = "app_planet")]
    private static extern short AppPlanet32(double JdTt, ref Object3Internal SsBody, Accuracy Accuracy, ref double Ra, ref double Dec, ref double Dis);

    [DllImport("NOVAS31.dll", EntryPoint = "app_star")]
    private static extern short AppStar32(double JdTt, ref CatEntry3 Star, Accuracy Accuracy, ref double Ra, ref double Dec);

    [DllImport("NOVAS31.dll", EntryPoint = "astro_planet")]
    private static extern short AstroPlanet32(double JdTt, ref Object3Internal SsBody, Accuracy Accuracy, ref double Ra, ref double Dec, ref double Dis);

    [DllImport("NOVAS31.dll", EntryPoint = "astro_star")]
    private static extern short AstroStar32(double JdTt, ref CatEntry3 Star, Accuracy Accuracy, ref double Ra, ref double Dec);

    [DllImport("NOVAS31.dll", EntryPoint = "bary2obs")]
    private static extern void Bary2Obs32(ref PosVector Pos, ref PosVector PosObs, ref PosVector Pos2, ref double Lighttime);

    [DllImport("NOVAS31.dll", EntryPoint = "cal_date")]
    private static extern void CalDate32(double Tjd, ref short Year, ref short Month, ref short Day, ref double Hour);

    [DllImport("NOVAS31.dll", EntryPoint = "cel2ter")]
    private static extern short Cel2Ter32(double JdHigh, double JdLow, double DeltaT, ASCOM.Astrometry.Method Method, Accuracy Accuracy, OutputVectorOption OutputOption, double x, double y, ref PosVector VecT, ref PosVector VecC);

    [DllImport("NOVAS31.dll", EntryPoint = "cel_pole")]
    private static extern short CelPole32(double Tjd, PoleOffsetCorrection Type, double Dpole1, double Dpole2);

    [DllImport("NOVAS31.dll", EntryPoint = "cio_array")]
    private static extern short CioArray32(double JdTdb, int NPts, ref RAOfCioArray Cio);

    [DllImport("NOVAS31.dll", EntryPoint = "cio_basis")]
    private static extern short CioBasis32(double JdTdbEquionx, double RaCioEquionx, ReferenceSystem RefSys, Accuracy Accuracy, ref double x, ref double y, ref double z);

    [DllImport("NOVAS31.dll", EntryPoint = "cio_location")]
    private static extern short CioLocation32(double JdTdb, Accuracy Accuracy, ref double RaCio, ref ReferenceSystem RefSys);

    [DllImport("NOVAS31.dll", EntryPoint = "cio_ra")]
    private static extern short CioRa32(double JdTt, Accuracy Accuracy, ref double RaCio);

    [DllImport("NOVAS31.dll", EntryPoint = "d_light")]
    private static extern double DLight32(ref PosVector Pos1, ref PosVector PosObs);

    [DllImport("NOVAS31.dll", EntryPoint = "e_tilt")]
    private static extern void ETilt32(double JdTdb, Accuracy Accuracy, ref double Mobl, ref double Tobl, ref double Ee, ref double Dpsi, ref double Deps);

    [DllImport("NOVAS31.dll", EntryPoint = "ecl2equ_vec")]
    private static extern short Ecl2EquVec32(double JdTt, CoordSys CoordSys, Accuracy Accuracy, ref PosVector Pos1, ref PosVector Pos2);

    [DllImport("NOVAS31.dll", EntryPoint = "ee_ct")]
    private static extern double EeCt32(double JdHigh, double JdLow, Accuracy Accuracy);

    [DllImport("NOVAS31.dll", EntryPoint = "ephemeris")]
    private static extern short Ephemeris32(ref JDHighPrecision Jd, ref Object3Internal CelObj, Origin Origin, Accuracy Accuracy, ref PosVector Pos, ref VelVector Vel);

    [DllImport("NOVAS31.dll", EntryPoint = "equ2ecl")]
    private static extern short Equ2Ecl32(double JdTt, CoordSys CoordSys, Accuracy Accuracy, double Ra, double Dec, ref double ELon, ref double ELat);

    [DllImport("NOVAS31.dll", EntryPoint = "equ2ecl_vec")]
    private static extern short Equ2EclVec32(double JdTt, CoordSys CoordSys, Accuracy Accuracy, ref PosVector Pos1, ref PosVector Pos2);

    [DllImport("NOVAS31.dll", EntryPoint = "equ2gal")]
    private static extern void Equ2Gal32(double RaI, double DecI, ref double GLon, ref double GLat);

    [DllImport("NOVAS31.dll", EntryPoint = "equ2hor")]
    private static extern void Equ2Hor32(double Jd_Ut1, double DeltT, Accuracy Accuracy, double x, double y, ref OnSurface Location, double Ra, double Dec, RefractionOption RefOption, ref double Zd, ref double Az, ref double RaR, ref double DecR);

    [DllImport("NOVAS31.dll", EntryPoint = "era")]
    private static extern double Era32(double JdHigh, double JdLow);

    [DllImport("NOVAS31.dll", EntryPoint = "frame_tie")]
    private static extern void FrameTie32(ref PosVector Pos1, FrameConversionDirection Direction, ref PosVector Pos2);

    [DllImport("NOVAS31.dll", EntryPoint = "fund_args")]
    private static extern void FundArgs32(double t, ref FundamentalArgs a);

    [DllImport("NOVAS31.dll", EntryPoint = "gcrs2equ")]
    private static extern short Gcrs2Equ32(double JdTt, CoordSys CoordSys, Accuracy Accuracy, double RaG, double DecG, ref double Ra, ref double Dec);

    [DllImport("NOVAS31.dll", EntryPoint = "geo_posvel")]
    private static extern short GeoPosVel32(double JdTt, double DeltaT, Accuracy Accuracy, ref Observer Obs, ref PosVector Pos, ref VelVector Vel);

    [DllImport("NOVAS31.dll", EntryPoint = "grav_def")]
    private static extern short GravDef32(double JdTdb, EarthDeflection LocCode, Accuracy Accuracy, ref PosVector Pos1, ref PosVector PosObs, ref PosVector Pos2);

    [DllImport("NOVAS31.dll", EntryPoint = "grav_vec")]
    private static extern void GravVec32(ref PosVector Pos1, ref PosVector PosObs, ref PosVector PosBody, double RMass, ref PosVector Pos2);

    [DllImport("NOVAS31.dll", EntryPoint = "ira_equinox")]
    private static extern double IraEquinox32(double JdTdb, EquinoxType Equinox, Accuracy Accuracy);

    [DllImport("NOVAS31.dll", EntryPoint = "julian_date")]
    private static extern double JulianDate32(short Year, short Month, short Day, double Hour);

    [DllImport("NOVAS31.dll", EntryPoint = "light_time")]
    private static extern short LightTime32(double JdTdb, ref Object3Internal SsObject, ref PosVector PosObs, double TLight0, Accuracy Accuracy, ref PosVector Pos, ref double TLight);

    [DllImport("NOVAS31.dll", EntryPoint = "limb_angle")]
    private static extern void LimbAngle32(ref PosVector PosObj, ref PosVector PosObs, ref double LimbAng, ref double NadirAng);

    [DllImport("NOVAS31.dll", EntryPoint = "local_planet")]
    private static extern short LocalPlanet32(double JdTt, ref Object3Internal SsBody, double DeltaT, ref OnSurface Position, Accuracy Accuracy, ref double Ra, ref double Dec, ref double Dis);

    [DllImport("NOVAS31.dll", EntryPoint = "local_star")]
    private static extern short LocalStar32(double JdTt, double DeltaT, ref CatEntry3 Star, ref OnSurface Position, Accuracy Accuracy, ref double Ra, ref double Dec);

    [DllImport("NOVAS31.dll", EntryPoint = "make_cat_entry")]
    private static extern void MakeCatEntry32([MarshalAs(UnmanagedType.LPStr)] string StarName, [MarshalAs(UnmanagedType.LPStr)] string Catalog, int StarNum, double Ra, double Dec, double PmRa, double PmDec, double Parallax, double RadVel, ref CatEntry3 Star);

    [DllImport("NOVAS31.dll", EntryPoint = "make_in_space")]
    private static extern void MakeInSpace32(ref PosVector ScPos, ref VelVector ScVel, ref InSpace ObsSpace);

    [DllImport("NOVAS31.dll", EntryPoint = "make_object")]
    private static extern short MakeObject32(ASCOM.Astrometry.ObjectType Type, short Number, [MarshalAs(UnmanagedType.LPStr)] string Name, ref CatEntry3 StarData, ref Object3Internal CelObj);

    [DllImport("NOVAS31.dll", EntryPoint = "make_observer")]
    private static extern short MakeObserver32(ObserverLocation Where, ref OnSurface ObsSurface, ref InSpace ObsSpace, ref Observer Obs);

    [DllImport("NOVAS31.dll", EntryPoint = "make_observer_at_geocenter")]
    private static extern void MakeObserverAtGeocenter32(ref Observer ObsAtGeocenter);

    [DllImport("NOVAS31.dll", EntryPoint = "make_observer_in_space")]
    private static extern void MakeObserverInSpace32(ref PosVector ScPos, ref VelVector ScVel, ref Observer ObsInSpace);

    [DllImport("NOVAS31.dll", EntryPoint = "make_observer_on_surface")]
    private static extern void MakeObserverOnSurface32(double Latitude, double Longitude, double Height, double Temperature, double Pressure, ref Observer ObsOnSurface);

    [DllImport("NOVAS31.dll", EntryPoint = "make_on_surface")]
    private static extern void MakeOnSurface32(double Latitude, double Longitude, double Height, double Temperature, double Pressure, ref OnSurface ObsSurface);

    [DllImport("NOVAS31.dll", EntryPoint = "mean_obliq")]
    private static extern double MeanObliq32(double JdTdb);

    [DllImport("NOVAS31.dll", EntryPoint = "mean_star")]
    private static extern short MeanStar32(double JdTt, double Ra, double Dec, Accuracy Accuracy, ref double IRa, ref double IDec);

    [DllImport("NOVAS31.dll", EntryPoint = "norm_ang")]
    private static extern double NormAng32(double Angle);

    [DllImport("NOVAS31.dll", EntryPoint = "nutation")]
    private static extern void Nutation32(double JdTdb, NutationDirection Direction, Accuracy Accuracy, ref PosVector Pos, ref PosVector Pos2);

    [DllImport("NOVAS31.dll", EntryPoint = "nutation_angles")]
    private static extern void NutationAngles32(double t, Accuracy Accuracy, ref double DPsi, ref double DEps);

    [DllImport("NOVAS31.dll", EntryPoint = "place")]
    private static extern short Place32(double JdTt, ref Object3Internal CelObject, ref Observer Location, double DeltaT, CoordSys CoordSys, Accuracy Accuracy, ref SkyPos Output);

    [DllImport("NOVAS31.dll", EntryPoint = "precession")]
    private static extern short Precession32(double JdTdb1, ref PosVector Pos1, double JdTdb2, ref PosVector Pos2);

    [DllImport("NOVAS31.dll", EntryPoint = "proper_motion")]
    private static extern void ProperMotion32(double JdTdb1, ref PosVector Pos, ref VelVector Vel, double JdTdb2, ref PosVector Pos2);

    [DllImport("NOVAS31.dll", EntryPoint = "rad_vel")]
    private static extern void RadVel32(ref Object3Internal CelObject, ref PosVector Pos, ref VelVector Vel, ref VelVector VelObs, double DObsGeo, double DObsSun, double DObjSun, ref double Rv);

    [DllImport("NOVAS31.dll", EntryPoint = "radec2vector")]
    private static extern void RaDec2Vector32(double Ra, double Dec, double Dist, ref PosVector Vector);

    [DllImport("NOVAS31.dll", EntryPoint = "refract")]
    private static extern double Refract32(ref OnSurface Location, RefractionOption RefOption, double ZdObs);

    [DllImport("NOVAS31.dll", EntryPoint = "sidereal_time")]
    private static extern short SiderealTime32(double JdHigh, double JdLow, double DeltaT, GstType GstType, ASCOM.Astrometry.Method Method, Accuracy Accuracy, ref double Gst);

    [DllImport("NOVAS31.dll", EntryPoint = "spin")]
    private static extern void Spin32(double Angle, ref PosVector Pos1, ref PosVector Pos2);

    [DllImport("NOVAS31.dll", EntryPoint = "starvectors")]
    private static extern void StarVectors32(ref CatEntry3 Star, ref PosVector Pos, ref VelVector Vel);

    [DllImport("NOVAS31.dll", EntryPoint = "tdb2tt")]
    private static extern void Tdb2Tt32(double TdbJd, ref double TtJd, ref double SecDiff);

    [DllImport("NOVAS31.dll", EntryPoint = "ter2cel")]
    private static extern short Ter2Cel32(double JdHigh, double JdLow, double DeltaT, ASCOM.Astrometry.Method Method, Accuracy Accuracy, OutputVectorOption OutputOption, double x, double y, ref PosVector VecT, ref PosVector VecC);

    [DllImport("NOVAS31.dll", EntryPoint = "terra")]
    private static extern void Terra32(ref OnSurface Location, double St, ref PosVector Pos, ref VelVector Vel);

    [DllImport("NOVAS31.dll", EntryPoint = "topo_planet")]
    private static extern short TopoPlanet32(double JdTt, ref Object3Internal SsBody, double DeltaT, ref OnSurface Position, Accuracy Accuracy, ref double Ra, ref double Dec, ref double Dis);

    [DllImport("NOVAS31.dll", EntryPoint = "topo_star")]
    private static extern short TopoStar32(double JdTt, double DeltaT, ref CatEntry3 Star, ref OnSurface Position, Accuracy Accuracy, ref double Ra, ref double Dec);

    [DllImport("NOVAS31.dll", EntryPoint = "transform_cat")]
    private static extern short TransformCat32(TransformationOption3 TransformOption, double DateInCat, ref CatEntry3 InCat, double DateNewCat, [MarshalAs(UnmanagedType.LPStr)] string NewCatId, ref CatEntry3 NewCat);

    [DllImport("NOVAS31.dll", EntryPoint = "transform_hip")]
    private static extern void TransformHip32(ref CatEntry3 Hipparcos, ref CatEntry3 Hip2000);

    [DllImport("NOVAS31.dll", EntryPoint = "vector2radec")]
    private static extern short Vector2RaDec32(ref PosVector Pos, ref double Ra, ref double Dec);

    [DllImport("NOVAS31.dll", EntryPoint = "virtual_planet")]
    private static extern short VirtualPlanet32(double JdTt, ref Object3Internal SsBody, Accuracy Accuracy, ref double Ra, ref double Dec, ref double Dis);

    [DllImport("NOVAS31.dll", EntryPoint = "virtual_star")]
    private static extern short VirtualStar32(double JdTt, ref CatEntry3 Star, Accuracy Accuracy, ref double Ra, ref double Dec);

    [DllImport("NOVAS31.dll", EntryPoint = "wobble")]
    private static extern void Wobble32(double Tjd, short Direction, double x, double y, ref PosVector Pos1, ref PosVector Pos2);

    [DllImport("NOVAS31-64.dll", EntryPoint = "set_racio_file")]
    private static extern void SetRACIOFile64([MarshalAs(UnmanagedType.LPStr)] string Name);

    [DllImport("NOVAS31-64.dll", EntryPoint = "ephem_close")]
    private static extern short EphemClose64();

    [DllImport("NOVAS31-64.dll", EntryPoint = "ephem_open")]
    private static extern short EphemOpen64([MarshalAs(UnmanagedType.LPStr)] string Ephem_Name, ref double JD_Begin, ref double JD_End, ref short DENumber);

    [DllImport("NOVAS31-64.dll", EntryPoint = "planet_ephemeris")]
    private static extern short PlanetEphemeris64(ref JDHighPrecision Tjd, Target Target, Target Center, ref PosVector Position, ref VelVector Velocity);

    [DllImport("NOVAS31-64.dll", EntryPoint = "readeph")]
    private static extern IntPtr ReadEph64(int Mp, [MarshalAs(UnmanagedType.LPStr)] string Name, double Jd, ref int Err);

    [DllImport("NOVAS31-64.dll", EntryPoint = "cleaneph")]
    private static extern void CleanEph64();

    [DllImport("NOVAS31-64.dll", EntryPoint = "solarsystem")]
    private static extern short SolarSystem64(double tjd, short body, short origin, ref PosVector pos, ref VelVector vel);

    [DllImport("NOVAS31-64.dll", EntryPoint = "state")]
    private static extern short State64(ref JDHighPrecision Jed, Target Target, ref PosVector TargetPos, ref VelVector TargetVel);

    [DllImport("NOVAS31-64.dll", EntryPoint = "aberration")]
    private static extern void Aberration64(ref PosVector Pos, ref VelVector Vel, double LightTime, ref PosVector Pos2);

    [DllImport("NOVAS31-64.dll", EntryPoint = "app_planet")]
    private static extern short AppPlanet64(double JdTt, ref Object3Internal SsBody, Accuracy Accuracy, ref double Ra, ref double Dec, ref double Dis);

    [DllImport("NOVAS31-64.dll", EntryPoint = "app_star")]
    private static extern short AppStar64(double JdTt, ref CatEntry3 Star, Accuracy Accuracy, ref double Ra, ref double Dec);

    [DllImport("NOVAS31-64.dll", EntryPoint = "astro_planet")]
    private static extern short AstroPlanet64(double JdTt, ref Object3Internal SsBody, Accuracy Accuracy, ref double Ra, ref double Dec, ref double Dis);

    [DllImport("NOVAS31-64.dll", EntryPoint = "astro_star")]
    private static extern short AstroStar64(double JdTt, ref CatEntry3 Star, Accuracy Accuracy, ref double Ra, ref double Dec);

    [DllImport("NOVAS31-64.dll", EntryPoint = "bary2obs")]
    private static extern void Bary2Obs64(ref PosVector Pos, ref PosVector PosObs, ref PosVector Pos2, ref double Lighttime);

    [DllImport("NOVAS31-64.dll", EntryPoint = "cal_date")]
    private static extern void CalDate64(double Tjd, ref short Year, ref short Month, ref short Day, ref double Hour);

    [DllImport("NOVAS31-64.dll", EntryPoint = "cel2ter")]
    private static extern short Cel2Ter64(double JdHigh, double JdLow, double DeltaT, ASCOM.Astrometry.Method Method, Accuracy Accuracy, OutputVectorOption OutputOption, double x, double y, ref PosVector VecT, ref PosVector VecC);

    [DllImport("NOVAS31-64.dll", EntryPoint = "cel_pole")]
    private static extern short CelPole64(double Tjd, PoleOffsetCorrection Type, double Dpole1, double Dpole2);

    [DllImport("NOVAS31-64.dll", EntryPoint = "cio_array")]
    private static extern short CioArray64(double JdTdb, int NPts, ref RAOfCioArray Cio);

    [DllImport("NOVAS31-64.dll", EntryPoint = "cio_basis")]
    private static extern short CioBasis64(double JdTdbEquionx, double RaCioEquionx, ReferenceSystem RefSys, Accuracy Accuracy, ref double x, ref double y, ref double z);

    [DllImport("NOVAS31-64.dll", EntryPoint = "cio_location")]
    private static extern short CioLocation64(double JdTdb, Accuracy Accuracy, ref double RaCio, ref ReferenceSystem RefSys);

    [DllImport("NOVAS31-64.dll", EntryPoint = "cio_ra")]
    private static extern short CioRa64(double JdTt, Accuracy Accuracy, ref double RaCio);

    [DllImport("NOVAS31-64.dll", EntryPoint = "d_light")]
    private static extern double DLight64(ref PosVector Pos1, ref PosVector PosObs);

    [DllImport("NOVAS31-64.dll", EntryPoint = "e_tilt")]
    private static extern void ETilt64(double JdTdb, Accuracy Accuracy, ref double Mobl, ref double Tobl, ref double Ee, ref double Dpsi, ref double Deps);

    [DllImport("NOVAS31-64.dll", EntryPoint = "ecl2equ_vec")]
    private static extern short Ecl2EquVec64(double JdTt, CoordSys CoordSys, Accuracy Accuracy, ref PosVector Pos1, ref PosVector Pos2);

    [DllImport("NOVAS31-64.dll", EntryPoint = "ee_ct")]
    private static extern double EeCt64(double JdHigh, double JdLow, Accuracy Accuracy);

    [DllImport("NOVAS31-64.dll", EntryPoint = "ephemeris")]
    private static extern short Ephemeris64(ref JDHighPrecision Jd, ref Object3Internal CelObj, Origin Origin, Accuracy Accuracy, ref PosVector Pos, ref VelVector Vel);

    [DllImport("NOVAS31-64.dll", EntryPoint = "equ2ecl")]
    private static extern short Equ2Ecl64(double JdTt, CoordSys CoordSys, Accuracy Accuracy, double Ra, double Dec, ref double ELon, ref double ELat);

    [DllImport("NOVAS31-64.dll", EntryPoint = "equ2ecl_vec")]
    private static extern short Equ2EclVec64(double JdTt, CoordSys CoordSys, Accuracy Accuracy, ref PosVector Pos1, ref PosVector Pos2);

    [DllImport("NOVAS31-64.dll", EntryPoint = "equ2gal")]
    private static extern void Equ2Gal64(double RaI, double DecI, ref double GLon, ref double GLat);

    [DllImport("NOVAS31-64.dll", EntryPoint = "equ2hor")]
    private static extern void Equ2Hor64(double Jd_Ut1, double DeltT, Accuracy Accuracy, double x, double y, ref OnSurface Location, double Ra, double Dec, RefractionOption RefOption, ref double Zd, ref double Az, ref double RaR, ref double DecR);

    [DllImport("NOVAS31-64.dll", EntryPoint = "era")]
    private static extern double Era64(double JdHigh, double JdLow);

    [DllImport("NOVAS31-64.dll", EntryPoint = "frame_tie")]
    private static extern void FrameTie64(ref PosVector Pos1, FrameConversionDirection Direction, ref PosVector Pos2);

    [DllImport("NOVAS31-64.dll", EntryPoint = "fund_args")]
    private static extern void FundArgs64(double t, ref FundamentalArgs a);

    [DllImport("NOVAS31-64.dll", EntryPoint = "gcrs2equ")]
    private static extern short Gcrs2Equ64(double JdTt, CoordSys CoordSys, Accuracy Accuracy, double RaG, double DecG, ref double Ra, ref double Dec);

    [DllImport("NOVAS31-64.dll", EntryPoint = "geo_posvel")]
    private static extern short GeoPosVel64(double JdTt, double DeltaT, Accuracy Accuracy, ref Observer Obs, ref PosVector Pos, ref VelVector Vel);

    [DllImport("NOVAS31-64.dll", EntryPoint = "grav_def")]
    private static extern short GravDef64(double JdTdb, EarthDeflection LocCode, Accuracy Accuracy, ref PosVector Pos1, ref PosVector PosObs, ref PosVector Pos2);

    [DllImport("NOVAS31-64.dll", EntryPoint = "grav_vec")]
    private static extern void GravVec64(ref PosVector Pos1, ref PosVector PosObs, ref PosVector PosBody, double RMass, ref PosVector Pos2);

    [DllImport("NOVAS31-64.dll", EntryPoint = "ira_equinox")]
    private static extern double IraEquinox64(double JdTdb, EquinoxType Equinox, Accuracy Accuracy);

    [DllImport("NOVAS31-64.dll", EntryPoint = "julian_date")]
    private static extern double JulianDate64(short Year, short Month, short Day, double Hour);

    [DllImport("NOVAS31-64.dll", EntryPoint = "light_time")]
    private static extern short LightTime64(double JdTdb, ref Object3Internal SsObject, ref PosVector PosObs, double TLight0, Accuracy Accuracy, ref PosVector Pos, ref double TLight);

    [DllImport("NOVAS31-64.dll", EntryPoint = "limb_angle")]
    private static extern void LimbAngle64(ref PosVector PosObj, ref PosVector PosObs, ref double LimbAng, ref double NadirAng);

    [DllImport("NOVAS31-64.dll", EntryPoint = "local_planet")]
    private static extern short LocalPlanet64(double JdTt, ref Object3Internal SsBody, double DeltaT, ref OnSurface Position, Accuracy Accuracy, ref double Ra, ref double Dec, ref double Dis);

    [DllImport("NOVAS31-64.dll", EntryPoint = "local_star")]
    private static extern short LocalStar64(double JdTt, double DeltaT, ref CatEntry3 Star, ref OnSurface Position, Accuracy Accuracy, ref double Ra, ref double Dec);

    [DllImport("NOVAS31-64.dll", EntryPoint = "make_cat_entry")]
    private static extern void MakeCatEntry64([MarshalAs(UnmanagedType.LPStr)] string StarName, [MarshalAs(UnmanagedType.LPStr)] string Catalog, int StarNum, double Ra, double Dec, double PmRa, double PmDec, double Parallax, double RadVel, ref CatEntry3 Star);

    [DllImport("NOVAS31-64.dll", EntryPoint = "make_in_space")]
    private static extern void MakeInSpace64(ref PosVector ScPos, ref VelVector ScVel, ref InSpace ObsSpace);

    [DllImport("NOVAS31-64.dll", EntryPoint = "make_object")]
    private static extern short MakeObject64(ASCOM.Astrometry.ObjectType Type, short Number, [MarshalAs(UnmanagedType.LPStr)] string Name, ref CatEntry3 StarData, ref Object3Internal CelObj);

    [DllImport("NOVAS31-64.dll", EntryPoint = "make_observer")]
    private static extern short MakeObserver64(ObserverLocation Where, ref OnSurface ObsSurface, ref InSpace ObsSpace, ref Observer Obs);

    [DllImport("NOVAS31-64.dll", EntryPoint = "make_observer_at_geocenter")]
    private static extern void MakeObserverAtGeocenter64(ref Observer ObsAtGeocenter);

    [DllImport("NOVAS31-64.dll", EntryPoint = "make_observer_in_space")]
    private static extern void MakeObserverInSpace64(ref PosVector ScPos, ref VelVector ScVel, ref Observer ObsInSpace);

    [DllImport("NOVAS31-64.dll", EntryPoint = "make_observer_on_surface")]
    private static extern void MakeObserverOnSurface64(double Latitude, double Longitude, double Height, double Temperature, double Pressure, ref Observer ObsOnSurface);

    [DllImport("NOVAS31-64.dll", EntryPoint = "make_on_surface")]
    private static extern void MakeOnSurface64(double Latitude, double Longitude, double Height, double Temperature, double Pressure, ref OnSurface ObsSurface);

    [DllImport("NOVAS31-64.dll", EntryPoint = "mean_obliq")]
    private static extern double MeanObliq64(double JdTdb);

    [DllImport("NOVAS31-64.dll", EntryPoint = "mean_star")]
    private static extern short MeanStar64(double JdTt, double Ra, double Dec, Accuracy Accuracy, ref double IRa, ref double IDec);

    [DllImport("NOVAS31-64.dll", EntryPoint = "norm_ang")]
    private static extern double NormAng64(double Angle);

    [DllImport("NOVAS31-64.dll", EntryPoint = "nutation")]
    private static extern void Nutation64(double JdTdb, NutationDirection Direction, Accuracy Accuracy, ref PosVector Pos, ref PosVector Pos2);

    [DllImport("NOVAS31-64.dll", EntryPoint = "nutation_angles")]
    private static extern void NutationAngles64(double t, Accuracy Accuracy, ref double DPsi, ref double DEps);

    [DllImport("NOVAS31-64.dll", EntryPoint = "place")]
    private static extern short Place64(double JdTt, ref Object3Internal CelObject, ref Observer Location, double DeltaT, CoordSys CoordSys, Accuracy Accuracy, ref SkyPos Output);

    [DllImport("NOVAS31-64.dll", EntryPoint = "precession")]
    private static extern short Precession64(double JdTdb1, ref PosVector Pos1, double JdTdb2, ref PosVector Pos2);

    [DllImport("NOVAS31-64.dll", EntryPoint = "proper_motion")]
    private static extern void ProperMotion64(double JdTdb1, ref PosVector Pos, ref VelVector Vel, double JdTdb2, ref PosVector Pos2);

    [DllImport("NOVAS31-64.dll", EntryPoint = "rad_vel")]
    private static extern void RadVel64(ref Object3Internal CelObject, ref PosVector Pos, ref VelVector Vel, ref VelVector VelObs, double DObsGeo, double DObsSun, double DObjSun, ref double Rv);

    [DllImport("NOVAS31-64.dll", EntryPoint = "radec2vector")]
    private static extern void RaDec2Vector64(double Ra, double Dec, double Dist, ref PosVector Vector);

    [DllImport("NOVAS31-64.dll", EntryPoint = "refract")]
    private static extern double Refract64(ref OnSurface Location, RefractionOption RefOption, double ZdObs);

    [DllImport("NOVAS31-64.dll", EntryPoint = "sidereal_time")]
    private static extern short SiderealTime64(double JdHigh, double JdLow, double DeltaT, GstType GstType, ASCOM.Astrometry.Method Method, Accuracy Accuracy, ref double Gst);

    [DllImport("NOVAS31-64.dll", EntryPoint = "spin")]
    private static extern void Spin64(double Angle, ref PosVector Pos1, ref PosVector Pos2);

    [DllImport("NOVAS31-64.dll", EntryPoint = "starvectors")]
    private static extern void StarVectors64(ref CatEntry3 Star, ref PosVector Pos, ref VelVector Vel);

    [DllImport("NOVAS31-64.dll", EntryPoint = "tdb2tt")]
    private static extern void Tdb2Tt64(double TdbJd, ref double TtJd, ref double SecDiff);

    [DllImport("NOVAS31-64.dll", EntryPoint = "ter2cel")]
    private static extern short Ter2Cel64(double JdHigh, double JdLow, double DeltaT, ASCOM.Astrometry.Method Method, Accuracy Accuracy, OutputVectorOption OutputOption, double x, double y, ref PosVector VecT, ref PosVector VecC);

    [DllImport("NOVAS31-64.dll", EntryPoint = "terra")]
    private static extern void Terra64(ref OnSurface Location, double St, ref PosVector Pos, ref VelVector Vel);

    [DllImport("NOVAS31-64.dll", EntryPoint = "topo_planet")]
    private static extern short TopoPlanet64(double JdTt, ref Object3Internal SsBody, double DeltaT, ref OnSurface Position, Accuracy Accuracy, ref double Ra, ref double Dec, ref double Dis);

    [DllImport("NOVAS31-64.dll", EntryPoint = "topo_star")]
    private static extern short TopoStar64(double JdTt, double DeltaT, ref CatEntry3 Star, ref OnSurface Position, Accuracy Accuracy, ref double Ra, ref double Dec);

    [DllImport("NOVAS31-64.dll", EntryPoint = "transform_cat")]
    private static extern short TransformCat64(TransformationOption3 TransformOption, double DateInCat, ref CatEntry3 InCat, double DateNewCat, [MarshalAs(UnmanagedType.LPStr)] string NewCatId, ref CatEntry3 NewCat);

    [DllImport("NOVAS31-64.dll", EntryPoint = "transform_hip")]
    private static extern void TransformHip64(ref CatEntry3 Hipparcos, ref CatEntry3 Hip2000);

    [DllImport("NOVAS31-64.dll", EntryPoint = "vector2radec")]
    private static extern short Vector2RaDec64(ref PosVector Pos, ref double Ra, ref double Dec);

    [DllImport("NOVAS31-64.dll", EntryPoint = "virtual_planet")]
    private static extern short VirtualPlanet64(double JdTt, ref Object3Internal SsBody, Accuracy Accuracy, ref double Ra, ref double Dec, ref double Dis);

    [DllImport("NOVAS31-64.dll", EntryPoint = "virtual_star")]
    private static extern short VirtualStar64(double JdTt, ref CatEntry3 Star, Accuracy Accuracy, ref double Ra, ref double Dec);

    [DllImport("NOVAS31-64.dll", EntryPoint = "wobble")]
    private static extern void Wobble64(double Tjd, short Direction, double x, double y, ref PosVector Pos1, ref PosVector Pos2);

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
