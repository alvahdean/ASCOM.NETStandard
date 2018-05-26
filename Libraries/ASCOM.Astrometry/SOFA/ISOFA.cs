// Decompiled with JetBrains decompiler
// Type: ASCOM.Astrometry.SOFA.ISOFA
// Assembly: ASCOM.Astrometry, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 695EF5AA-123E-4689-AAF5-64FD1324F6BB
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.Astrometry\6.0.0.0__565de7938946fba7\ASCOM.Astrometry.dll

using System.Runtime.InteropServices;

namespace ASCOM.Astrometry.SOFA
{
  [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
  //[ComVisible(true)]
  //[Guid("8E322A40-8E75-49FC-B75B-984A45D35C0A")]
  public interface ISOFA
  {
    int Af2a(string s, int ideg, int iamin, double asec, ref double rad);

    double Anp(double a);

    void Atci13(double rc, double dc, double pr, double pd, double px, double rv, double date1, double date2, ref double ri, ref double di, ref double eo);

    void Atic13(double ri, double di, double date1, double date2, ref double rc, ref double dc, ref double eo);

    int Atco13(double rc, double dc, double pr, double pd, double px, double rv, double utc1, double utc2, double dut1, double elong, double phi, double hm, double xp, double yp, double phpa, double tc, double rh, double wl, ref double aob, ref double zob, ref double hob, ref double dob, ref double rob, ref double eo);

    int Atio13(double ri, double di, double utc1, double utc2, double dut1, double elong, double phi, double hm, double xp, double yp, double phpa, double tc, double rh, double wl, ref double aob, ref double zob, ref double hob, ref double dob, ref double rob);

    int Atoc13(string type, double ob1, double ob2, double utc1, double utc2, double dut1, double elong, double phi, double hm, double xp, double yp, double phpa, double tc, double rh, double wl, ref double rc, ref double dc);

    int Atoi13(string type, double ob1, double ob2, double utc1, double utc2, double dut1, double elong, double phi, double hm, double xp, double yp, double phpa, double tc, double rh, double wl, ref double ri, ref double di);

    int Dtf2d(string scale, int iy, int im, int id, int ihr, int imn, double sec, ref double d1, ref double d2);

    double Eo06a(double date1, double date2);

    int SofaReleaseNumber();

    string SofaIssueDate();

    string SofaRevisionDate();

    int TaiTt(double tai1, double tai2, ref double tt1, ref double tt2);

    int TaiUtc(double tai1, double tai2, ref double utc1, ref double utc2);

    int Tf2a(string s, int ihour, int imin, double sec, ref double rad);

    int TtTai(double tt1, double tt2, ref double tai1, ref double tai2);

    int UtcTai(double utc1, double utc2, ref double tai1, ref double tai2);
  }
}
