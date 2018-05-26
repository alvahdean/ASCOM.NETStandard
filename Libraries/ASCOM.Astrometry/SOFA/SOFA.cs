// Decompiled with JetBrains decompiler
// Type: ASCOM.Astrometry.SOFA.SOFA
// Assembly: ASCOM.Astrometry, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 695EF5AA-123E-4689-AAF5-64FD1324F6BB
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.Astrometry\6.0.0.0__565de7938946fba7\ASCOM.Astrometry.dll

using ASCOM.Utilities;
using ASCOM.Utilities.Exceptions;

using System;
using System.Runtime.InteropServices;
using System.Text;

namespace ASCOM.Astrometry.SOFA
{
    [ClassInterface(ClassInterfaceType.None)]
    //[Guid("DF65E97B-ED0E-4F48-BBC9-4A8854C0EF6E")]
    //[ComVisible(true)]
    public class SOFA : ISOFA, IDisposable
    {
        private const string SOFA32DLL = "SOFA11.dll";
        private const string SOFA64DLL = "SOFA11-64.dll";
        private const string SOFA_DLL_LOCATION = "\\ASCOM\\Astrometry\\";
        private const int SOFA_RELEASE_NUMBER = 11;
        private const string SOFA_ISSUE_DATE = "2015-02-09";
        private const int SOFA_REVISION_NUMBER = 1;
        private const string SOFA_REVISION_DATE = "2015-04-02";
        private TraceLogger TL;
        private Util Utl;
        private IntPtr SofaDllHandle;
        private bool disposedValue;
        private const int CSIDL_PROGRAM_FILES = 38;
        private const int CSIDL_PROGRAM_FILESX86 = 42;
        private const int CSIDL_WINDOWS = 36;
        private const int CSIDL_PROGRAM_FILES_COMMONX86 = 44;

        public SOFA()
        {
            this.disposedValue = false;
            StringBuilder lpszPath = new StringBuilder(260);
            this.TL = new TraceLogger("", "SOFA");
            this.TL.Enabled = RegistryCommonCode.GetBool("Trace NOVAS", false);
            this.Utl = new Util();
            string lpFileName;
            if (this.Is64Bit())
            {
                ASCOM.Astrometry.SOFA.SOFA.SHGetSpecialFolderPath(IntPtr.Zero, lpszPath, 44, false);
                lpFileName = lpszPath.ToString() + "\\ASCOM\\Astrometry\\SOFA11-64.dll";
            }
            else
                lpFileName = Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles) + "\\ASCOM\\Astrometry\\SOFA11.dll";
            this.TL.LogMessage("New", "Loading SOFA library DLL: " + lpFileName);
            this.SofaDllHandle = ASCOM.Astrometry.SOFA.SOFA.LoadLibrary(lpFileName);
            int lastWin32Error = Marshal.GetLastWin32Error();
            if (this.SofaDllHandle != IntPtr.Zero)
            {
                this.TL.LogMessage("New", "Loaded SOFA library OK");
                this.TL.LogMessage("New", "SOFA Initialised OK");
            }
            else
            {
                this.TL.LogMessage("New", "Error loading SOFA library: " + lastWin32Error.ToString("X8"));
                throw new HelperException("Error code returned from LoadLibrary when loading SOFA library: " + lastWin32Error.ToString("X8"));
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    if (this.Utl != null)
                    {
                        this.Utl.Dispose();
                        this.Utl = (Util)null;
                    }
                    if (this.TL != null)
                    {
                        this.TL.Enabled = false;
                        this.TL.Dispose();
                        this.TL = (TraceLogger)null;
                    }
                }
                try
                {
                    ASCOM.Astrometry.SOFA.SOFA.FreeLibrary(this.SofaDllHandle);
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
            GC.SuppressFinalize((object)this);
        }

        public int SofaReleaseNumber()
        {
            return 11;
        }

        public string SofaIssueDate()
        {
            return "2015-02-09";
        }

        public string SofaRevisionDate()
        {
            return "2015-04-02";
        }

        public int Af2a(string s, int ideg, int iamin, double asec, ref double rad)
        {
            if (string.IsNullOrEmpty(s))
                s = " ";
            if (this.Is64Bit())
            {
                int num1 = (int)ASCOM.Astrometry.SOFA.SOFA.Af2a64(s.ToCharArray()[0], Convert.ToInt16(ideg), Convert.ToInt16(iamin), asec, ref rad);
            }
            else
            {
                int num2 = (int)ASCOM.Astrometry.SOFA.SOFA.Af2a32(s.ToCharArray()[0], Convert.ToInt16(ideg), Convert.ToInt16(iamin), asec, ref rad);
            }
            short num3=0;
            return Convert.ToInt32(num3);
        }

        public double Anp(double a)
        {
            return !this.Is64Bit() ? ASCOM.Astrometry.SOFA.SOFA.Anp32(a) : ASCOM.Astrometry.SOFA.SOFA.Anp64(a);
        }

        public void CelestialToIntermediate(double rc, double dc, double pr, double pd, double px, double rv, double date1, double date2, ref double ri, ref double di, ref double eo)
        {
            if (this.Is64Bit())
                ASCOM.Astrometry.SOFA.SOFA.Atci1364(rc, dc, pr, pd, px, rv, date1, date2, ref ri, ref di, ref eo);
            else
                ASCOM.Astrometry.SOFA.SOFA.Atci1332(rc, dc, pr, pd, px, rv, date1, date2, ref ri, ref di, ref eo);
        }

        public int CelestialToObserved(double rc, double dc, double pr, double pd, double px, double rv, double utc1, double utc2, double dut1, double elong, double phi, double hm, double xp, double yp, double phpa, double tc, double rh, double wl, ref double aob, ref double zob, ref double hob, ref double dob, ref double rob, ref double eo)
        {
            return Convert.ToInt32(!this.Is64Bit() ? ASCOM.Astrometry.SOFA.SOFA.Atco1332(rc, dc, pr, pd, px, rv, utc1, utc2, dut1, elong, phi, hm, xp, yp, phpa, tc, rh, wl, ref aob, ref zob, ref hob, ref dob, ref rob, ref eo) : ASCOM.Astrometry.SOFA.SOFA.Atco1364(rc, dc, pr, pd, px, rv, utc1, utc2, dut1, elong, phi, hm, xp, yp, phpa, tc, rh, wl, ref aob, ref zob, ref hob, ref dob, ref rob, ref eo));
        }

        public int Dtf2d(string scale, int iy, int im, int id, int ihr, int imn, double sec, ref double d1, ref double d2)
        {
            return Convert.ToInt32(!this.Is64Bit() ? ASCOM.Astrometry.SOFA.SOFA.Dtf2d32(scale, iy, im, id, ihr, imn, sec, ref d1, ref d2) : ASCOM.Astrometry.SOFA.SOFA.Dtf2d64(scale, iy, im, id, ihr, imn, sec, ref d1, ref d2));
        }

        public double Eo06a(double date1, double date2)
        {
            return !this.Is64Bit() ? ASCOM.Astrometry.SOFA.SOFA.Eo06a32(date1, date2) : ASCOM.Astrometry.SOFA.SOFA.Eo06a64(date1, date2);
        }

        public void IntermediateToCelestial(double ri, double di, double date1, double date2, ref double rc, ref double dc, ref double eo)
        {
            if (this.Is64Bit())
                ASCOM.Astrometry.SOFA.SOFA.Atic1364(ri, di, date1, date2, ref rc, ref dc, ref eo);
            else
                ASCOM.Astrometry.SOFA.SOFA.Atic1332(ri, di, date1, date2, ref rc, ref dc, ref eo);
        }

        public int IntermediateToObserved(double ri, double di, double utc1, double utc2, double dut1, double elong, double phi, double hm, double xp, double yp, double phpa, double tc, double rh, double wl, ref double aob, ref double zob, ref double hob, ref double dob, ref double rob)
        {
            return Convert.ToInt32(!this.Is64Bit() ? ASCOM.Astrometry.SOFA.SOFA.Atio1332(ri, di, utc1, utc2, dut1, elong, phi, hm, xp, yp, phpa, tc, rh, wl, ref aob, ref zob, ref hob, ref dob, ref rob) : ASCOM.Astrometry.SOFA.SOFA.Atio1364(ri, di, utc1, utc2, dut1, elong, phi, hm, xp, yp, phpa, tc, rh, wl, ref aob, ref zob, ref hob, ref dob, ref rob));
        }

        public int ObservedToCelestial(string type, double ob1, double ob2, double utc1, double utc2, double dut1, double elong, double phi, double hm, double xp, double yp, double phpa, double tc, double rh, double wl, ref double rc, ref double dc)
        {
            return Convert.ToInt32(!this.Is64Bit() ? ASCOM.Astrometry.SOFA.SOFA.Atoc1332(type, ob1, ob2, utc1, utc2, dut1, elong, phi, hm, xp, yp, phpa, tc, rh, wl, ref rc, ref dc) : ASCOM.Astrometry.SOFA.SOFA.Atoc1364(type, ob1, ob2, utc1, utc2, dut1, elong, phi, hm, xp, yp, phpa, tc, rh, wl, ref rc, ref dc));
        }

        public int ObservedToIntermediate(string type, double ob1, double ob2, double utc1, double utc2, double dut1, double elong, double phi, double hm, double xp, double yp, double phpa, double tc, double rh, double wl, ref double ri, ref double di)
        {
            return Convert.ToInt32(!this.Is64Bit() ? ASCOM.Astrometry.SOFA.SOFA.Atoi1332(type, ob1, ob2, utc1, utc2, dut1, elong, phi, hm, xp, yp, phpa, tc, rh, wl, ref ri, ref di) : ASCOM.Astrometry.SOFA.SOFA.Atoi1364(type, ob1, ob2, utc1, utc2, dut1, elong, phi, hm, xp, yp, phpa, tc, rh, wl, ref ri, ref di));
        }

        public int TaiUtc(double tai1, double tai2, ref double utc1, ref double utc2)
        {
            return Convert.ToInt32(!this.Is64Bit() ? ASCOM.Astrometry.SOFA.SOFA.Taiutc32(tai1, tai2, ref utc1, ref utc2) : ASCOM.Astrometry.SOFA.SOFA.Taiutc64(tai1, tai2, ref utc1, ref utc2));
        }

        public int TaiTt(double tai1, double tai2, ref double tt1, ref double tt2)
        {
            return Convert.ToInt32(!this.Is64Bit() ? ASCOM.Astrometry.SOFA.SOFA.Taitt32(tai1, tai2, ref tt1, ref tt2) : ASCOM.Astrometry.SOFA.SOFA.Taitt64(tai1, tai2, ref tt1, ref tt2));
        }

        public int TtTai(double tt1, double tt2, ref double tai1, ref double tai2)
        {
            return Convert.ToInt32(!this.Is64Bit() ? ASCOM.Astrometry.SOFA.SOFA.Tttai32(tt1, tt2, ref tai1, ref tai2) : ASCOM.Astrometry.SOFA.SOFA.Tttai64(tt1, tt2, ref tai1, ref tai2));
        }

        public int Tf2a(string s, int ihour, int imin, double sec, ref double rad)
        {
            if (string.IsNullOrEmpty(s))
                s = " ";
            if (this.Is64Bit())
            {
                int num1 = (int)ASCOM.Astrometry.SOFA.SOFA.Tf2a64(s.ToCharArray()[0], Convert.ToInt16(ihour), Convert.ToInt16(imin), sec, ref rad);
            }
            else
            {
                int num2 = (int)ASCOM.Astrometry.SOFA.SOFA.Tf2a32(s.ToCharArray()[0], Convert.ToInt16(ihour), Convert.ToInt16(imin), sec, ref rad);
            }
            short num3=0;
            return Convert.ToInt32(num3);
        }

        public int UtcTai(double utc1, double utc2, ref double tai1, ref double tai2)
        {
            if (this.Is64Bit())
            {
                int num1 = (int)ASCOM.Astrometry.SOFA.SOFA.Utctai64(utc1, utc2, ref tai1, ref tai2);
            }
            else
            {
                int num2 = (int)ASCOM.Astrometry.SOFA.SOFA.Utctai32(utc1, utc2, ref tai1, ref tai2);
            }
            short num3=0;
            return Convert.ToInt32(num3);
        }

        [DllImport("SOFA11.dll", EntryPoint = "iauAf2a")]
        private static extern short Af2a32(char s, short ideg, short iamin, double asec, ref double rad);

        [DllImport("SOFA11.dll", EntryPoint = "iauAnp")]
        private static extern double Anp32(double a);

        [DllImport("SOFA11.dll", EntryPoint = "iauAtci13")]
        private static extern void Atci1332(double rc, double dc, double pr, double pd, double px, double rv, double date1, double date2, ref double ri, ref double di, ref double eo);

        [DllImport("SOFA11.dll", EntryPoint = "iauAtco13")]
        private static extern short Atco1332(double rc, double dc, double pr, double pd, double px, double rv, double utc1, double utc2, double dut1, double elong, double phi, double hm, double xp, double yp, double phpa, double tc, double rh, double wl, ref double aob, ref double zob, ref double hob, ref double dob, ref double rob, ref double eo);

        [DllImport("SOFA11.dll", EntryPoint = "iauAtic13")]
        private static extern void Atic1332(double ri, double di, double date1, double date2, ref double rc, ref double dc, ref double eo);

        [DllImport("SOFA11.dll", EntryPoint = "iauAtoc13")]
        private static extern short Atoc1332(string type, double ob1, double ob2, double utc1, double utc2, double dut1, double elong, double phi, double hm, double xp, double yp, double phpa, double tc, double rh, double wl, ref double rc, ref double dc);

        [DllImport("SOFA11.dll", EntryPoint = "iauAtio13")]
        private static extern short Atio1332(double ri, double di, double utc1, double utc2, double dut1, double elong, double phi, double hm, double xp, double yp, double phpa, double tc, double rh, double wl, ref double aob, ref double zob, ref double hob, ref double dob, ref double rob);

        [DllImport("SOFA11.dll", EntryPoint = "iauAtoi13")]
        private static extern short Atoi1332(string type, double ob1, double ob2, double utc1, double utc2, double dut1, double elong, double phi, double hm, double xp, double yp, double phpa, double tc, double rh, double wl, ref double ri, ref double di);

        [DllImport("SOFA11.dll", EntryPoint = "iauDtf2d")]
        private static extern short Dtf2d32(string scale, int iy, int im, int id, int ihr, int imn, double sec, ref double d1, ref double d2);

        [DllImport("SOFA11.dll", EntryPoint = "iauEo06a")]
        private static extern double Eo06a32(double date1, double date2);

        [DllImport("SOFA11.dll", EntryPoint = "iauTaitt")]
        private static extern short Taitt32(double tai1, double tai2, ref double tt1, ref double tt2);

        [DllImport("SOFA11.dll", EntryPoint = "iauTttai")]
        private static extern short Tttai32(double tt1, double tt2, ref double tai1, ref double tai2);

        [DllImport("SOFA11.dll", EntryPoint = "iauTf2a")]
        private static extern short Tf2a32(char s, short ihour, short imin, double sec, ref double rad);

        [DllImport("SOFA11.dll", EntryPoint = "iauUtctai")]
        private static extern short Utctai32(double utc1, double utc2, ref double tai1, ref double tai2);

        [DllImport("SOFA11.dll", EntryPoint = "iauTaiutc")]
        private static extern short Taiutc32(double tai1, double tai2, ref double utc1, ref double utc2);

        [DllImport("SOFA11-64.dll", EntryPoint = "iauAf2a")]
        private static extern short Af2a64(char s, short ideg, short iamin, double asec, ref double rad);

        [DllImport("SOFA11-64.dll", EntryPoint = "iauAnp")]
        private static extern double Anp64(double a);

        [DllImport("SOFA11-64.dll", EntryPoint = "iauAtci13")]
        private static extern void Atci1364(double rc, double dc, double pr, double pd, double px, double rv, double date1, double date2, ref double ri, ref double di, ref double eo);

        [DllImport("SOFA11-64.dll", EntryPoint = "iauAtco13")]
        private static extern short Atco1364(double rc, double dc, double pr, double pd, double px, double rv, double utc1, double utc2, double dut1, double elong, double phi, double hm, double xp, double yp, double phpa, double tc, double rh, double wl, ref double aob, ref double zob, ref double hob, ref double dob, ref double rob, ref double eo);

        [DllImport("SOFA11-64.dll", EntryPoint = "iauAtic13")]
        private static extern void Atic1364(double ri, double di, double date1, double date2, ref double rc, ref double dc, ref double eo);

        [DllImport("SOFA11-64.dll", EntryPoint = "iauAtoc13")]
        private static extern short Atoc1364(string type, double ob1, double ob2, double utc1, double utc2, double dut1, double elong, double phi, double hm, double xp, double yp, double phpa, double tc, double rh, double wl, ref double rc, ref double dc);

        [DllImport("SOFA11-64.dll", EntryPoint = "iauAtio13")]
        private static extern short Atio1364(double ri, double di, double utc1, double utc2, double dut1, double elong, double phi, double hm, double xp, double yp, double phpa, double tc, double rh, double wl, ref double aob, ref double zob, ref double hob, ref double dob, ref double rob);

        [DllImport("SOFA11-64.dll", EntryPoint = "iauAtoi13")]
        private static extern short Atoi1364(string type, double ob1, double ob2, double utc1, double utc2, double dut1, double elong, double phi, double hm, double xp, double yp, double phpa, double tc, double rh, double wl, ref double ri, ref double di);

        [DllImport("SOFA11-64.dll", EntryPoint = "iauDtf2d")]
        private static extern short Dtf2d64(string scale, int iy, int im, int id, int ihr, int imn, double sec, ref double d1, ref double d2);

        [DllImport("SOFA11-64.dll", EntryPoint = "iauEo06a")]
        private static extern double Eo06a64(double date1, double date2);

        [DllImport("SOFA11-64.dll", EntryPoint = "iauTaitt")]
        private static extern short Taitt64(double tai1, double tai2, ref double tt1, ref double tt2);

        [DllImport("SOFA11-64.dll", EntryPoint = "iauTttai")]
        private static extern short Tttai64(double tt1, double tt2, ref double tai1, ref double tai2);

        [DllImport("SOFA11-64.dll", EntryPoint = "iauTf2a")]
        private static extern short Tf2a64(char s, short ihour, short imin, double sec, ref double rad);

        [DllImport("SOFA11-64.dll", EntryPoint = "iauUtctai")]
        private static extern short Utctai64(double utc1, double utc2, ref double tai1, ref double tai2);

        [DllImport("SOFA11-64.dll", EntryPoint = "iauTaiutc")]
        private static extern short Taiutc64(double tai1, double tai2, ref double utc1, ref double utc2);

        [DllImport("shell32.dll")]
        private static extern bool SHGetSpecialFolderPath(IntPtr hwndOwner, [Out] StringBuilder lpszPath, int nFolder, bool fCreate);

        [DllImport("kernel32.dll", EntryPoint = "LoadLibraryA", SetLastError = true)]
        public static extern IntPtr LoadLibrary(string lpFileName);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool FreeLibrary(IntPtr hModule);

        private bool Is64Bit()
        {
            return IntPtr.Size == 8;
        }

        public void Atci13(double rc, double dc, double pr, double pd, double px, double rv, double date1, double date2, ref double ri, ref double di, ref double eo)
        {
            if (!this.Is64Bit())
                ASCOM.Astrometry.SOFA.SOFA.Atci1332(rc, dc, pr, pd, px, rv, date1, date2, ref ri, ref di, ref eo);
            else
                ASCOM.Astrometry.SOFA.SOFA.Atci1364(rc, dc, pr, pd, px, rv, date1, date2, ref ri, ref di, ref eo);
        }

        public void Atic13(double ri, double di, double date1, double date2, ref double rc, ref double dc, ref double eo)
        {
            if(!this.Is64Bit())
                ASCOM.Astrometry.SOFA.SOFA.Atic1332(ri, di, date1, date2, ref rc, ref dc, ref eo);
            else
                ASCOM.Astrometry.SOFA.SOFA.Atic1364(ri, di, date1, date2, ref rc, ref dc, ref eo);
        }

        public int Atco13(double rc, double dc, double pr, double pd, double px, double rv, double utc1, double utc2, double dut1, double elong, double phi, double hm, double xp, double yp, double phpa, double tc, double rh, double wl, ref double aob, ref double zob, ref double hob, ref double dob, ref double rob, ref double eo)
        {
            return !this.Is64Bit()
                ? ASCOM.Astrometry.SOFA.SOFA.Atco1332(rc, dc, pr, pd, px, rv, utc1, utc2, dut1, elong, phi, hm, xp, yp, phpa, tc, rh, wl, ref aob, ref  zob, ref hob, ref dob, ref rob, ref eo)
                : ASCOM.Astrometry.SOFA.SOFA.Atco1364(rc, dc, pr, pd, px, rv, utc1, utc2, dut1, elong, phi, hm, xp, yp, phpa, tc, rh, wl, ref aob, ref zob, ref hob, ref dob, ref rob, ref eo);
        }

        public int Atio13(double ri, double di, double utc1, double utc2, double dut1, double elong, double phi, double hm, double xp, double yp, double phpa, double tc, double rh, double wl, ref double aob, ref double zob, ref double hob, ref double dob, ref double rob)
        {
            return !this.Is64Bit()
                ? ASCOM.Astrometry.SOFA.SOFA.Atio1332(ri, di, utc1, utc2, dut1, elong, phi, hm, xp, yp, phpa, tc, rh, wl, ref aob, ref zob, ref hob, ref dob, ref rob)
                : ASCOM.Astrometry.SOFA.SOFA.Atio1364(ri, di, utc1, utc2, dut1, elong, phi, hm, xp, yp, phpa, tc, rh, wl, ref aob, ref zob, ref hob, ref dob, ref rob);
        }

        public int Atoc13(string type, double ob1, double ob2, double utc1, double utc2, double dut1, double elong, double phi, double hm, double xp, double yp, double phpa, double tc, double rh, double wl, ref double rc, ref double dc)
        {
            return !this.Is64Bit()
                ? ASCOM.Astrometry.SOFA.SOFA.Atoc1332(type, ob1, ob2, utc1, utc2, dut1, elong, phi, hm, xp, yp, phpa, tc, rh, wl, ref rc, ref dc)
                : ASCOM.Astrometry.SOFA.SOFA.Atoc1364(type, ob1, ob2, utc1, utc2, dut1, elong, phi, hm, xp, yp, phpa, tc, rh, wl, ref rc, ref dc);
        }

        public int Atoi13(string type, double ob1, double ob2, double utc1, double utc2, double dut1, double elong, double phi, double hm, double xp, double yp, double phpa, double tc, double rh, double wl, ref double ri, ref double di)
        {
            return !this.Is64Bit()
                ? ASCOM.Astrometry.SOFA.SOFA.Atoi1332(type, ob1, ob2, utc1, utc2, dut1, elong, phi, hm, xp, yp, phpa, tc, rh, wl, ref ri, ref di)
                : ASCOM.Astrometry.SOFA.SOFA.Atoi1364(type, ob1, ob2, utc1, utc2, dut1, elong, phi, hm, xp, yp, phpa, tc, rh, wl, ref ri, ref di);
        }
    }
}
