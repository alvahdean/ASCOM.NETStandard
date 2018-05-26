// Decompiled with JetBrains decompiler
// Type: ASCOM.Astrometry.KeplerGlobalCode
// Assembly: ASCOM.Astrometry, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 695EF5AA-123E-4689-AAF5-64FD1324F6BB
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.Astrometry\6.0.0.0__565de7938946fba7\ASCOM.Astrometry.dll


using System;

namespace ASCOM.Astrometry
{
  
  internal sealed class KeplerGlobalCode
  {
    internal static double[] pAcof = new double[10]
    {
      -8.66E-10,
      -4.759E-08,
      2.424E-07,
      1.3095E-05,
      0.00017451,
      -0.0018055,
      -0.235316,
      0.076,
      110.5414,
      50287.91959
    };
    internal static double[] nodecof = new double[11]
    {
      6.6402E-16,
      -2.69151E-15,
      -1.547021E-12,
      7.521313E-12,
      1.9E-10,
      -3.54E-09,
      -1.8103E-07,
      1.26E-07,
      7.436169E-05,
      -0.04207794833,
      3.052115282424
    };
    internal static double[] inclcof = new double[11]
    {
      1.2147E-16,
      7.3759E-17,
      -8.26287E-14,
      2.50341E-13,
      2.4650839E-11,
      -5.4000441E-11,
      1.32115526E-09,
      -6.012E-07,
      -1.62442E-05,
      0.00227850649,
      0.0
    };
    internal static KeplerGlobalCode.orbit mercury = new KeplerGlobalCode.orbit("Mercury", 2446800.5, 4378.0 / 625.0, 48.177, 29.074, 0.387098, 4.09236, 0.205628, 198.7199, 2446800.5, -0.42, 3.36, Mer404Data.mer404, 0.0, 0.0, 0.0);
    internal static KeplerGlobalCode.orbit venus = new KeplerGlobalCode.orbit("Venus", 2446800.5, 3.3946, 76.561, 54.889, 0.723329, 1.60214, 0.006757, 9.0369, 2446800.5, -4.4, 8.34, Ven404Data.ven404, 0.0, 0.0, 0.0);
    internal static KeplerGlobalCode.orbit earthplanet = new KeplerGlobalCode.orbit("Earth", 2446800.5, 0.0, 0.0, 102.884, 0.999999, 0.985611, 0.016713, 1.1791, 2446800.5, -3.86, 0.0, Ear404Data.ear404, 0.0, 0.0, 0.0);
    internal static KeplerGlobalCode.orbit mars = new KeplerGlobalCode.orbit("Mars", 2446800.5, 1.8498, 49.457, 286.343, 1.52371, 0.524023, 0.093472, 53.1893, 2446800.5, -1.52, 4.68, Mar404Data.mar404, 0.0, 0.0, 0.0);
    internal static KeplerGlobalCode.orbit jupiter = new KeplerGlobalCode.orbit("Jupiter", 2446800.5, 1.3051, 100.358, 275.129, 5.20265, 0.0830948, 0.0481, 344.5086, 2446800.5, -9.4, 98.44, Jup404Data.jup404, 0.0, 0.0, 0.0);
    internal static KeplerGlobalCode.orbit saturn = new KeplerGlobalCode.orbit("Saturn", 2446800.5, 2.4858, 113.555, 337.969, 9.5405, 0.033451, 0.052786, 159.6327, 2446800.5, -8.88, 82.73, Sat404Data.sat404, 0.0, 0.0, 0.0);
    internal static KeplerGlobalCode.orbit uranus = new KeplerGlobalCode.orbit("Uranus", 2446800.5, 0.7738, 73.994, 98.746, 19.2233, 0.0116943, 0.045682, 84.8516, 2446800.5, -7.19, 35.02, Ura404Data.ura404, 0.0, 0.0, 0.0);
    internal static KeplerGlobalCode.orbit neptune = new KeplerGlobalCode.orbit("Neptune", 2446800.5, 1.7697, 131.677, 250.623, 30.1631, 0.00594978, 0.009019, 254.2568, 2446800.5, -6.87, 33.5, Nep404Data.nep404, 0.0, 0.0, 0.0);
    internal static KeplerGlobalCode.orbit pluto = new KeplerGlobalCode.orbit("Pluto", 2446640.5, 17.1346, 110.204, 114.21, 39.4633, 0.0039757, 0.248662, 355.0554, 2446640.5, -1.0, 2.07, Plu404Data.plu404, 0.0, 0.0, 0.0);
    private static double[,] ss = new double[19, 32];
    private static double[,] cc = new double[19, 32];
    private static double[] Args = new double[19];
    internal static double[] freqs = new double[9]
    {
      53810162868.8982,
      21066413643.3548,
      12959774228.3429,
      6890507749.3988,
      1092566037.7991,
      439960985.5372,
      154248119.3933,
      78655032.0744,
      52272245.1795
    };
    internal static double[] phases = new double[9]
    {
      908103.259872,
      655127.28306,
      361679.244588,
      1279558.798488,
      123665.467464,
      180278.79948,
      1130598.018396,
      1095655.195728,
      860492.1546
    };
    internal const int NARGS = 18;
    private const double DTR = 0.0174532925199433;
    private const double RTD = 57.2957795130823;
    private const double RTS = 206264.806247096;
    private const double STR = 4.84813681109536E-06;
    private const double PI = 3.14159265358979;
    private const double TPI = 6.28318530717959;
    private const double J2000 = 2451545.0;
    private const double B1950 = 2433282.423;
    private const double J1900 = 2415020.0;
    private const double aearth = 6378137.0;
    private const double au = 149597870.691;
    private const double emrat = 81.300585;
    private const double Clight = 299792.458;
    private const double Clightaud = 0.0;
    private static double LP_equinox;
    private static double NF_arcsec;
    private static double Ea_arcsec;
    private static double pA_precession;

    internal static void epsiln(double J, ref double eps, ref double coseps, ref double sineps)
    {
      double num = (J - 2451545.0) / 365250.0;
      eps = ((((((((((2.45E-10 * num + 5.79E-09) * num + 2.787E-07) * num + 7.12E-07) * num - 3.905E-05) * num - 0.0024967) * num - 0.005138) * num + 1.9989) * num - 7.0 / 400.0) * num - 468.3396) * num + 84381.406173) * 4.84813681109536E-06;
      coseps = Math.Cos(eps);
      sineps = Math.Sin(eps);
    }

    internal static void precess(ref double[] R, double J, int direction)
    {
      double[] numArray = new double[4];
      if (J == 2451545.0)
        return;
      double num1 = (J - 2451545.0) / 36525.0;
      double eps = 0;
            double coseps = 0;
            double sineps = 0;
            if (direction == 1)
        KeplerGlobalCode.epsiln(J, ref eps, ref coseps, ref sineps);
      else
        KeplerGlobalCode.epsiln(2451545.0, ref eps, ref coseps, ref sineps);
      numArray[0] = R[0];
      double num2 = coseps * R[1] + sineps * R[2];
      numArray[2] = -sineps * R[1] + coseps * R[2];
      numArray[1] = num2;
      double num3 = num1 / 10.0;
      double[] pAcof = KeplerGlobalCode.pAcof;
      double num4 = pAcof[0];
      int index1 = 1;
      do
      {
        num4 = num4 * num3 + pAcof[index1];
        checked { ++index1; }
      }
      while (index1 <= 9);
      double num5 = num4 * (4.84813681109536E-06 * num3);
      double[] nodecof = KeplerGlobalCode.nodecof;
      double num6 = nodecof[0];
      int index2 = 1;
      do
      {
        num6 = num6 * num3 + nodecof[index2];
        checked { ++index2; }
      }
      while (index2 <= 10);
      double num7 = direction != 1 ? num6 : num6 + num5;
      double num8 = Math.Cos(num7);
      double num9 = Math.Sin(num7);
      double num10 = num8 * numArray[0] + num9 * numArray[1];
      numArray[1] = -num9 * numArray[0] + num8 * numArray[1];
      numArray[0] = num10;
      double[] inclcof = KeplerGlobalCode.inclcof;
      double num11 = inclcof[0];
      int index3 = 1;
      do
      {
        num11 = num11 * num3 + inclcof[index3];
        checked { ++index3; }
      }
      while (index3 <= 10);
      if (direction == 1)
        num11 = -num11;
      double num12 = Math.Cos(num11);
      double num13 = Math.Sin(num11);
      double num14 = num12 * numArray[1] + num13 * numArray[2];
      numArray[2] = -num13 * numArray[1] + num12 * numArray[2];
      numArray[1] = num14;
      double num15 = direction != 1 ? -num6 - num5 : -num6;
      double num16 = Math.Cos(num15);
      double num17 = Math.Sin(num15);
      double num18 = num16 * numArray[0] + num17 * numArray[1];
      numArray[1] = -num17 * numArray[0] + num16 * numArray[1];
      numArray[0] = num18;
      if (direction == 1)
        KeplerGlobalCode.epsiln(2451545.0, ref eps, ref coseps, ref sineps);
      else
        KeplerGlobalCode.epsiln(J, ref eps, ref coseps, ref sineps);
      double num19 = coseps * numArray[1] - sineps * numArray[2];
      numArray[2] = sineps * numArray[1] + coseps * numArray[2];
      numArray[1] = num19;
      int index4 = 0;
      do
      {
        R[index4] = numArray[index4];
        checked { ++index4; }
      }
      while (index4 <= 2);
    }

    internal static double atan4(double x, double y)
    {
      int num1 = 0;
      if (x < 0.0)
        num1 = 2;
      if (y < 0.0)
        num1 |= 1;
      if (x == 0.0)
      {
        if ((num1 & 1) > 0)
          return 3.0 * Math.PI / 2.0;
        return y == 0.0 ? 0.0 : Math.PI / 2.0;
      }
      if (y == 0.0)
        return (num1 & 2) > 0 ? Math.PI : 0.0;
      double num2 = 0;
            switch (num1)
      {
        case 0:
          num2 = 0.0;
          break;
        case 1:
          num2 = 2.0 * Math.PI;
          break;
        case 3:
          num2 = Math.PI;
          break;
      }
      double num3 = Math.Atan(y / x);
      return num2 + num3;
    }

    internal static double modtp(double x)
    {
      double num1 = Math.Floor(x / (2.0 * Math.PI));
      double num2 = x - num1 * (2.0 * Math.PI);
      while (num2 < 0.0)
        num2 += 2.0 * Math.PI;
      while (num2 >= 2.0 * Math.PI)
        num2 -= 2.0 * Math.PI;
      return num2;
    }

    internal static double mod360(double x)
    {
      int num1 = checked ((int) Math.Round(unchecked (x / 360.0)));
      double num2 = x - (double) num1 * 360.0;
      while (num2 < 0.0)
        num2 += 360.0;
      while (num2 > 360.0)
        num2 -= 360.0;
      return num2;
    }

    internal static void KeplerCalc(double J, ref KeplerGlobalCode.orbit e, ref double[] rect)
    {
      double[] pobj = new double[4];
      double num1;
      double num2;
      double pr;
      if (e.ptable.lon_tbl[0] != 0.0)
      {
        if (e.obname=="Earth")
          KeplerGlobalCode.g3plan(J, ref e.ptable, ref pobj, 3);
        else
          KeplerGlobalCode.gplan(J, ref e.ptable, ref pobj);
        num1 = pobj[0];
        e.L = num1;
        num2 = pobj[1];
        pr = pobj[2];
        e.r = pr;
        e.epoch = J;
        e.equinox = 2451545.0;
      }
      else
      {
        e.equinox = 2451545.0;
        double epoch = e.epoch;
        double i = e.i;
        double num3 = e.W * (Math.PI / 180.0);
        double w = e.W;
        double a = e.a;
        double num4 = e.dm;
        double ecc = e.ecc;
        double m = e.M;
        double num5;
        if (ecc == 1.0)
        {
          double num6 = a * Math.Sqrt(a);
          double num7 = (J - epoch) * 0.0364911624 / num6;
          double d = 0.0;
          double num8 = 1.0;
          while (Math.Abs(num8) > 1E-11)
          {
            double num9 = d * d;
            double num10 = (2.0 * d * num9 + num7) / (3.0 * (1.0 + num9));
            num8 = num10 - d;
            if (num10 != 0.0)
              num8 /= num10;
            d = num10;
          }
          pr = a * (1.0 + d * d);
          num5 = 2.0 * Math.Atan(d) + Math.PI / 180.0 * w;
        }
        else if (ecc > 1.0)
        {
          double d = a / (ecc - 1.0);
          double num6 = d * Math.Sqrt(d);
          double num7 = (J - epoch) * 0.01720209895 / num6;
          double num8 = num7 / (ecc - 1.0);
          double num9 = 1.0;
          while (Math.Abs(num9) > 1E-11)
          {
            num9 = -num8 + ecc * Math.Sinh(num8) - num7;
            num8 += num9 / (1.0 - ecc * Math.Cosh(num8));
          }
          pr = d * (ecc * Math.Cosh(num8) - 1.0);
          num5 = 2.0 * Math.Atan(Math.Sqrt((ecc + 1.0) / (ecc - 1.0)) * Math.Tanh(0.5 * num8)) + Math.PI / 180.0 * w;
        }
        else
        {
          if (num4 == 0.0)
            num4 = 0.9856076686 / (e.a * Math.Sqrt(e.a));
          double num6 = num4 * (J - epoch);
          double num7 = KeplerGlobalCode.modtp(Math.PI / 180.0 * (m + num6));
          if (e.L != 0.0)
          {
            e.L = e.L + num6;
            e.L = KeplerGlobalCode.mod360(e.L);
          }
          double num8 = num7;
          double num9;
          do
          {
            num9 = num8 - ecc * Math.Sin(num8) - num7;
            num8 -= num9 / (1.0 - ecc * Math.Cos(num8));
          }
          while (Math.Abs(num9) > 1E-11);
          double d = KeplerGlobalCode.modtp(2.0 * Math.Atan(Math.Sqrt((1.0 + ecc) / (1.0 - ecc)) * Math.Tan(0.5 * num8)));
          double num10 = m * (Math.PI / 180.0);
          num5 = e.L == 0.0 ? d + Math.PI / 180.0 * w : e.L * (Math.PI / 180.0) + d - num10 - num3;
          pr = a * (1.0 - ecc * ecc) / (1.0 + ecc * Math.Cos(d));
        }
        double num11 = i * (Math.PI / 180.0);
        double x = Math.Cos(num5);
        double num12 = Math.Sin(num5);
        double y = num12 * Math.Cos(num11);
        num1 = KeplerGlobalCode.atan4(x, y) + num3;
        num2 = Math.Asin(num12 * Math.Sin(num11));
      }
      rect[2] = pr * Math.Sin(num2);
      double num13 = Math.Cos(num2);
      rect[1] = pr * num13 * Math.Sin(num1);
      rect[0] = pr * num13 * Math.Cos(num1);
      double eps = 0;
            double coseps = 0;
            double sineps = 0;
            KeplerGlobalCode.epsiln(e.equinox, ref eps, ref coseps, ref sineps);
      double num14 = coseps * rect[1] - sineps * rect[2];
      double num15 = sineps * rect[1] + coseps * rect[2];
      rect[1] = num14;
      rect[2] = num15;
      KeplerGlobalCode.precess(ref rect, e.equinox, 1);
      if (e.obname!="Earth")
        return;
      KeplerGlobalCode.embofs(J, ref rect, ref pr);
    }

    internal static void embofs(double J, ref double[] ea, ref double pr)
    {
      double[] numArray = new double[4];
      double[] pol = new double[4];
      KeplerGlobalCode.gmoon(J, ref numArray, ref pol);
      KeplerGlobalCode.precess(ref numArray, J, 1);
      double num = 0.0121505819187069;
      double d = 0.0;
      int index = 0;
      do
      {
        ea[index] = ea[index] - num * numArray[index];
        d += ea[index] * ea[index];
        checked { ++index; }
      }
      while (index <= 2);
      pr = Math.Sqrt(d);
    }

    internal static double mods3600(double x)
    {
      return x - 1296000.0 * Math.Floor(x / 1296000.0);
    }

    internal static int gplan(double JD, ref KeplerGlobalCode.plantbl plan, ref double[] pobj)
    {
      double num1 = (JD - 2451545.0) / plan.timescale;
      int maxargs = plan.maxargs;
      int num2 = 0;
      int num3 = checked (maxargs - 1);
      int k = num2;
      while (k <= num3)
      {
        int n = plan.max_harmonic[k];
        if (n > 0)
        {
          double num4 = (KeplerGlobalCode.mods3600(KeplerGlobalCode.freqs[k] * num1) + KeplerGlobalCode.phases[k]) * 4.84813681109536E-06;
          KeplerGlobalCode.sscc(k, num4, n);
        }
        checked { ++k; }
      }
      int index1 = 0;
      int index2 = 0;
      int index3 = 0;
      int index4 = 0;
      double num5 = 0.0;
      double num6 = 0.0;
      double num7 = 0.0;
      while (true)
      {
        int num4 = plan.arg_tbl[index1];
        int index5 = checked (index1 + 1);
        if (num4 >= 0)
        {
          if (num4 == 0)
          {
            int num8 = plan.arg_tbl[index5];
            index1 = checked (index5 + 1);
            double x = plan.lon_tbl[index2];
            checked { ++index2; }
            int num9 = 0;
            int num10 = checked (num8 - 1);
            int num11 = num9;
            while (num11 <= num10)
            {
              x = x * num1 + plan.lon_tbl[index2];
              checked { ++index2; }
              checked { ++num11; }
            }
            num5 += KeplerGlobalCode.mods3600(x);
            double num12 = plan.lat_tbl[index3];
            checked { ++index3; }
            int num13 = 0;
            int num14 = checked (num8 - 1);
            int num15 = num13;
            while (num15 <= num14)
            {
              num12 = num12 * num1 + plan.lat_tbl[index3];
              checked { ++index3; }
              checked { ++num15; }
            }
            num6 += num12;
            double num16 = plan.rad_tbl[index4];
            checked { ++index4; }
            int num17 = 0;
            int num18 = checked (num8 - 1);
            int num19 = num17;
            while (num19 <= num18)
            {
              num16 = num16 * num1 + plan.rad_tbl[index4];
              checked { ++index4; }
              checked { ++num19; }
            }
            num7 += num16;
          }
          else
          {
            int num8 = 0;
            double num9 = 0.0;
            double num10 = 0.0;
            int num11 = 0;
            int num12 = checked (num4 - 1);
            int num13 = num11;
            while (num13 <= num12)
            {
              int num14 = plan.arg_tbl[index5];
              int index6 = checked (index5 + 1);
              int index7 = checked (plan.arg_tbl[index6] - 1);
              index5 = checked (index6 + 1);
              if (num14 != 0)
              {
                int num15 = num14;
                if (num14 < 0)
                  num15 = checked (-num15);
                int index8 = checked (num15 - 1);
                double num16 = KeplerGlobalCode.ss[index7, index8];
                if (num14 < 0)
                  num16 = -num16;
                double num17 = KeplerGlobalCode.cc[index7, index8];
                if (num8 == 0)
                {
                  num10 = num16;
                  num9 = num17;
                  num8 = 1;
                }
                else
                {
                  double num18 = num16 * num9 + num17 * num10;
                  num9 = num17 * num9 - num16 * num10;
                  num10 = num18;
                }
              }
              checked { ++num13; }
            }
            int num19 = plan.arg_tbl[index5];
            index1 = checked (index5 + 1);
            double num20 = plan.lon_tbl[index2];
            int index9 = checked (index2 + 1);
            double num21 = plan.lon_tbl[index9];
            index2 = checked (index9 + 1);
            int num22 = 0;
            int num23 = checked (num19 - 1);
            int num24 = num22;
            while (num24 <= num23)
            {
              num20 = num20 * num1 + plan.lon_tbl[index2];
              int index6 = checked (index2 + 1);
              num21 = num21 * num1 + plan.lon_tbl[index6];
              index2 = checked (index6 + 1);
              checked { ++num24; }
            }
            num5 += num20 * num9 + num21 * num10;
            double num25 = plan.lat_tbl[index3];
            int index10 = checked (index3 + 1);
            double num26 = plan.lat_tbl[index10];
            index3 = checked (index10 + 1);
            int num27 = 1;
            int num28 = num19;
            int num29 = num27;
            while (num29 <= num28)
            {
              num25 = num25 * num1 + plan.lat_tbl[index3];
              int index6 = checked (index3 + 1);
              num26 = num26 * num1 + plan.lat_tbl[index6];
              index3 = checked (index6 + 1);
              checked { ++num29; }
            }
            num6 += num25 * num9 + num26 * num10;
            double num30 = plan.rad_tbl[index4];
            int index11 = checked (index4 + 1);
            double num31 = plan.rad_tbl[index11];
            index4 = checked (index11 + 1);
            int num32 = 1;
            int num33 = num19;
            int num34 = num32;
            while (num34 <= num33)
            {
              num30 = num30 * num1 + plan.rad_tbl[index4];
              int index6 = checked (index4 + 1);
              num31 = num31 * num1 + plan.rad_tbl[index6];
              index4 = checked (index6 + 1);
              checked { ++num34; }
            }
            num7 += num30 * num9 + num31 * num10;
          }
        }
        else
          break;
      }
      pobj[0] = 4.84813681109536E-06 * num5;
      pobj[1] = 4.84813681109536E-06 * num6;
      pobj[2] = 4.84813681109536E-06 * plan.distance * num7 + plan.distance;
      return 0;
    }

    internal static int sscc(int k, double arg, int n)
    {
      double num1 = Math.Sin(arg);
      double num2 = Math.Cos(arg);
      KeplerGlobalCode.ss[k, 0] = num1;
      KeplerGlobalCode.cc[k, 0] = num2;
      double num3 = 2.0 * num1 * num2;
      double num4 = num2 * num2 - num1 * num1;
      KeplerGlobalCode.ss[k, 1] = num3;
      KeplerGlobalCode.cc[k, 1] = num4;
      int num5 = 2;
      int num6 = checked (n - 1);
      int index = num5;
      while (index <= num6)
      {
        double num7 = num1 * num4 + num2 * num3;
        num4 = num2 * num4 - num1 * num3;
        num3 = num7;
        KeplerGlobalCode.ss[k, index] = num3;
        KeplerGlobalCode.cc[k, index] = num4;
        checked { ++index; }
      }
      return 0;
    }

    public static void mean_elements(double J)
    {
      double num1 = (J - 2451545.0) / 36525.0;
      double num2 = num1 * num1;
      double num3 = KeplerGlobalCode.mods3600(538101628.688982 * num1 + 908103.213) + (6.39E-06 * num1 - 0.0192789) * num2;
      KeplerGlobalCode.Args[0] = 4.84813681109536E-06 * num3;
      double num4 = KeplerGlobalCode.mods3600(210664136.433548 * num1 + 655127.236) + (-6.27E-06 * num1 + 0.0059381) * num2;
      KeplerGlobalCode.Args[1] = 4.84813681109536E-06 * num4;
      double num5 = KeplerGlobalCode.mods3600(129597742.283429 * num1 + 361679.198) + (-5.23E-06 * num1 - 0.0204411) * num2;
      KeplerGlobalCode.Ea_arcsec = num5;
      KeplerGlobalCode.Args[2] = 4.84813681109536E-06 * num5;
      double num6 = KeplerGlobalCode.mods3600(68905077.493988 * num1 + 1279558.751) + (-1.043E-05 * num1 + 0.0094264) * num2;
      KeplerGlobalCode.Args[3] = 4.84813681109536E-06 * num6;
      double num7 = KeplerGlobalCode.mods3600(10925660.377991 * num1 + 123665.42) + ((((-3.4E-10 * num1 + 5.91E-08) * num1 + 4.667E-06) * num1 + 5.706E-05) * num1 - 0.3060378) * num2;
      KeplerGlobalCode.Args[4] = 4.84813681109536E-06 * num7;
      double num8 = KeplerGlobalCode.mods3600(4399609.855372 * num1 + 180278.752) + ((((8.3E-10 * num1 - 1.452E-07) * num1 - 1.1484E-05) * num1 - 0.00016618) * num1 + 0.7561614) * num2;
      KeplerGlobalCode.Args[5] = 4.84813681109536E-06 * num8;
      double num9 = KeplerGlobalCode.mods3600(1542481.193933 * num1 + 1130597.971) + (2.156E-05 * num1 - 0.0175083) * num2;
      KeplerGlobalCode.Args[6] = 4.84813681109536E-06 * num9;
      double num10 = KeplerGlobalCode.mods3600(786550.320744 * num1 + 1095655.149) + (-8.95E-06 * num1 + 0.0021103) * num2;
      KeplerGlobalCode.Args[7] = 4.84813681109536E-06 * num10;
      double num11 = KeplerGlobalCode.mods3600(1602961600.99397 * num1 + 1072261.22024451) + (((((-3.207663637426E-13 * num1 + 2.555243317839E-11) * num1 + 2.560078201452E-09) * num1 - 3.702060118571E-05) * num1 + 0.00694927468360584) * num1 - 6.73522023744575) * num2;
      KeplerGlobalCode.Args[9] = 4.84813681109536E-06 * num11;
      double num12 = KeplerGlobalCode.mods3600(1739527262.84377 * num1 + 335779.514128847) + (((((4.474984866301E-13 * num1 + 4.189032191814E-11) * num1 - 2.790392351314E-09) * num1 - 2.165750777942E-06) * num1 - 0.00075311878482338) * num1 - 13.1178097896501) * num2;
      KeplerGlobalCode.NF_arcsec = num12;
      KeplerGlobalCode.Args[10] = 4.84813681109536E-06 * num12;
      double num13 = KeplerGlobalCode.mods3600(129596581.023043 * num1 + 1287102.74074415) + ((((((((1.62E-20 * num1 - 1.039E-17) * num1 - 3.83508E-15) * num1 + 4.237343E-13) * num1 + 8.8555011E-11) * num1 - 4.77258489E-08) * num1 - 1.1297037031E-05) * num1 + 8.74737173673247E-05) * num1 - 0.552813064217831) * num2;
      KeplerGlobalCode.Args[11] = 4.84813681109536E-06 * num13;
      double num14 = KeplerGlobalCode.mods3600(1717915922.88468 * num1 + 485868.174658253) + ((((-1.755312760154E-12 * num1 + 3.452144225877E-11 * num1 - 2.506365935364E-08) * num1 - 0.0002536291235258) * num1 + 0.0520996413027358) * num1 + 31.5013590718941) * num2;
      KeplerGlobalCode.Args[12] = 4.84813681109536E-06 * num14;
      double num15 = KeplerGlobalCode.mods3600(1732564372.04423 * num1 + 785939.809210524) + (((((7.200592540556E-14 * num1 + 2.235210987108E-10) * num1 - 1.024222633731E-08) * num1 - 6.073960534117E-05) * num1 + 0.00690172485283805) * num1 - 5.65504600274714) * num2;
      KeplerGlobalCode.LP_equinox = num15;
      KeplerGlobalCode.Args[13] = 4.84813681109536E-06 * num15;
      KeplerGlobalCode.pA_precession = 4.84813681109536E-06 * ((((((((((-8.66E-20 * num1 - 4.759E-17) * num1 + 2.424E-15) * num1 + 1.3095E-12) * num1 + 1.7451E-10) * num1 - 1.8055E-08) * num1 - 2.35316E-05) * num1 + 7.6E-05) * num1 + 1.105414) * num1 + 5028.791959) * num1);
      double num16 = KeplerGlobalCode.mods3600(44817540.9 * num1 + 806045.7);
      KeplerGlobalCode.Args[14] = 4.84813681109536E-06 * num16;
      double num17 = KeplerGlobalCode.mods3600(5364867.87 * num1 - 391702.8);
      KeplerGlobalCode.Args[15] = 4.84813681109536E-06 * num17;
      double num18 = KeplerGlobalCode.mods3600(1735730.0 * num1);
      KeplerGlobalCode.Args[17] = 4.84813681109536E-06 * num18;
    }

    internal static int g3plan(double JD, ref KeplerGlobalCode.plantbl plan, ref double[] pobj, int objnum)
    {
      KeplerGlobalCode.mean_elements(JD);
      double num1 = (JD - 2451545.0) / plan.timescale;
      int maxargs = plan.maxargs;
      int num2 = 0;
      int num3 = checked (maxargs - 1);
      int k = num2;
      while (k <= num3)
      {
        int n = plan.max_harmonic[k];
        if (n > 0)
          KeplerGlobalCode.sscc(k, KeplerGlobalCode.Args[k], n);
        checked { ++k; }
      }
      int index1 = 0;
      int index2 = 0;
      int index3 = 0;
      int index4 = 0;
      double num4 = 0.0;
      double num5 = 0.0;
      double num6 = 0.0;
      while (true)
      {
        int num7 = plan.arg_tbl[index1];
        int index5 = checked (index1 + 1);
        if (num7 >= 0)
        {
          if (num7 == 0)
          {
            int num8 = plan.arg_tbl[index5];
            index1 = checked (index5 + 1);
            double num9 = plan.lon_tbl[index2];
            checked { ++index2; }
            int num10 = 0;
            int num11 = checked (num8 - 1);
            int num12 = num10;
            while (num12 <= num11)
            {
              num9 = num9 * num1 + plan.lon_tbl[index2];
              checked { ++index2; }
              checked { ++num12; }
            }
            num4 += num9;
            double num13 = plan.lat_tbl[index3];
            checked { ++index3; }
            int num14 = 0;
            int num15 = checked (num8 - 1);
            int num16 = num14;
            while (num16 <= num15)
            {
              num13 = num13 * num1 + plan.lat_tbl[index3];
              checked { ++index3; }
              checked { ++num16; }
            }
            num5 += num13;
            double num17 = plan.rad_tbl[index4];
            checked { ++index4; }
            int num18 = 0;
            int num19 = checked (num8 - 1);
            int num20 = num18;
            while (num20 <= num19)
            {
              num17 = num17 * num1 + plan.rad_tbl[index4];
              checked { ++index4; }
              checked { ++num20; }
            }
            num6 += num17;
          }
          else
          {
            int num8 = 0;
            double num9 = 0.0;
            double num10 = 0.0;
            int num11 = 0;
            int num12 = checked (num7 - 1);
            int num13 = num11;
            while (num13 <= num12)
            {
              int num14 = plan.arg_tbl[index5];
              int index6 = checked (index5 + 1);
              int index7 = checked (plan.arg_tbl[index6] - 1);
              index5 = checked (index6 + 1);
              if (num14 != 0)
              {
                int index8 = checked (unchecked (num14 >= 0) ? num14 : -num14 - 1);
                double num15 = KeplerGlobalCode.ss[index7, index8];
                if (num14 < 0)
                  num15 = -num15;
                double num16 = KeplerGlobalCode.cc[index7, index8];
                if (num8 == 0)
                {
                  num10 = num15;
                  num9 = num16;
                  num8 = 1;
                }
                else
                {
                  double num17 = num15 * num9 + num16 * num10;
                  num9 = num16 * num9 - num15 * num10;
                  num10 = num17;
                }
              }
              checked { ++num13; }
            }
            int num18 = plan.arg_tbl[index5];
            index1 = checked (index5 + 1);
            double num19 = plan.lon_tbl[index2];
            int index9 = checked (index2 + 1);
            double num20 = plan.lon_tbl[index9];
            index2 = checked (index9 + 1);
            int num21 = 0;
            int num22 = checked (num18 - 1);
            int num23 = num21;
            while (num23 <= num22)
            {
              num19 = num19 * num1 + plan.lon_tbl[index2];
              int index6 = checked (index2 + 1);
              num20 = num20 * num1 + plan.lon_tbl[index6];
              index2 = checked (index6 + 1);
              checked { ++num23; }
            }
            num4 += num19 * num9 + num20 * num10;
            double num24 = plan.lat_tbl[index3];
            int index10 = checked (index3 + 1);
            double num25 = plan.lat_tbl[index10];
            index3 = checked (index10 + 1);
            int num26 = 0;
            int num27 = checked (num18 - 1);
            int num28 = num26;
            while (num28 <= num27)
            {
              num24 = num24 * num1 + plan.lat_tbl[index3];
              int index6 = checked (index3 + 1);
              num25 = num25 * num1 + plan.lat_tbl[index6];
              index3 = checked (index6 + 1);
              checked { ++num28; }
            }
            num5 += num24 * num9 + num25 * num10;
            double num29 = plan.rad_tbl[index4];
            int index11 = checked (index4 + 1);
            double num30 = plan.rad_tbl[index11];
            index4 = checked (index11 + 1);
            int num31 = 0;
            int num32 = checked (num18 - 1);
            int num33 = num31;
            while (num33 <= num32)
            {
              num29 = num29 * num1 + plan.rad_tbl[index4];
              int index6 = checked (index4 + 1);
              num30 = num30 * num1 + plan.rad_tbl[index6];
              index4 = checked (index6 + 1);
              checked { ++num33; }
            }
            num6 += num29 * num9 + num30 * num10;
          }
        }
        else
          break;
      }
      double trunclvl = plan.trunclvl;
      pobj[0] = KeplerGlobalCode.Args[checked (objnum - 1)] + 4.84813681109536E-06 * trunclvl * num4;
      pobj[1] = 4.84813681109536E-06 * trunclvl * num5;
      pobj[2] = plan.distance * (1.0 + 4.84813681109536E-06 * trunclvl * num6);
      return 0;
    }

    internal static int g2plan(double JD, ref KeplerGlobalCode.plantbl plan, ref double[] pobj)
    {
      KeplerGlobalCode.mean_elements(JD);
      double num1 = (JD - 2451545.0) / plan.timescale;
      int maxargs = plan.maxargs;
      int num2 = 0;
      int num3 = checked (maxargs - 1);
      int k = num2;
      while (k <= num3)
      {
        int n = plan.max_harmonic[k];
        if (n > 0)
          KeplerGlobalCode.sscc(k, KeplerGlobalCode.Args[k], n);
        checked { ++k; }
      }
      int index1 = 0;
      int index2 = 0;
      int index3 = 0;
      double num4 = 0.0;
      double num5 = 0.0;
      while (true)
      {
        int num6 = plan.arg_tbl[index1];
        int index4 = checked (index1 + 1);
        if (num6 >= 0)
        {
          if (num6 == 0)
          {
            int num7 = plan.arg_tbl[index4];
            index1 = checked (index4 + 1);
            double num8 = plan.lon_tbl[index2];
            checked { ++index2; }
            int num9 = 0;
            int num10 = checked (num7 - 1);
            int num11 = num9;
            while (num11 <= num10)
            {
              num8 = num8 * num1 + plan.lon_tbl[index2];
              checked { ++index2; }
              checked { ++num11; }
            }
            num4 += num8;
            double num12 = plan.rad_tbl[index3];
            checked { ++index3; }
            int num13 = 0;
            int num14 = checked (num7 - 1);
            int num15 = num13;
            while (num15 <= num14)
            {
              num12 = num12 * num1 + plan.rad_tbl[index3];
              checked { ++index3; }
              checked { ++num15; }
            }
            num5 += num12;
          }
          else
          {
            int num7 = 0;
            double num8 = 0.0;
            double num9 = 0.0;
            int num10 = 0;
            int num11 = checked (num6 - 1);
            int num12 = num10;
            while (num12 <= num11)
            {
              int num13 = plan.arg_tbl[index4];
              int index5 = checked (index4 + 1);
              int index6 = checked (plan.arg_tbl[index5] - 1);
              index4 = checked (index5 + 1);
              if (num13 != 0)
              {
                int index7 = checked (unchecked (num13 >= 0) ? num13 : -num13 - 1);
                double num14 = KeplerGlobalCode.ss[index6, index7];
                if (num13 < 0)
                  num14 = -num14;
                double num15 = KeplerGlobalCode.cc[index6, index7];
                if (num7 == 0)
                {
                  num9 = num14;
                  num8 = num15;
                  num7 = 1;
                }
                else
                {
                  double num16 = num14 * num8 + num15 * num9;
                  num8 = num15 * num8 - num14 * num9;
                  num9 = num16;
                }
              }
              checked { ++num12; }
            }
            int num17 = plan.arg_tbl[index4];
            index1 = checked (index4 + 1);
            double num18 = plan.lon_tbl[index2];
            int index8 = checked (index2 + 1);
            double num19 = plan.lon_tbl[index8];
            index2 = checked (index8 + 1);
            int num20 = 0;
            int num21 = checked (num17 - 1);
            int num22 = num20;
            while (num22 <= num21)
            {
              num18 = num18 * num1 + plan.lon_tbl[index2];
              int index5 = checked (index2 + 1);
              num19 = num19 * num1 + plan.lon_tbl[index5];
              index2 = checked (index5 + 1);
              checked { ++num22; }
            }
            num4 += num18 * num8 + num19 * num9;
            double num23 = plan.rad_tbl[index3];
            int index9 = checked (index3 + 1);
            double num24 = plan.rad_tbl[index9];
            index3 = checked (index9 + 1);
            int num25 = 0;
            int num26 = checked (num17 - 1);
            int num27 = num25;
            while (num27 <= num26)
            {
              num23 = num23 * num1 + plan.rad_tbl[index3];
              int index5 = checked (index3 + 1);
              num24 = num24 * num1 + plan.rad_tbl[index5];
              index3 = checked (index5 + 1);
              checked { ++num27; }
            }
            num5 += num23 * num8 + num24 * num9;
          }
        }
        else
          break;
      }
      double trunclvl = plan.trunclvl;
      pobj[0] = trunclvl * num4;
      pobj[2] = trunclvl * num5;
      return 0;
    }

    internal static double g1plan(double JD, ref KeplerGlobalCode.plantbl plan)
    {
      double num1 = (JD - 2451545.0) / plan.timescale;
      KeplerGlobalCode.mean_elements(JD);
      int k = 0;
      do
      {
        int n = plan.max_harmonic[k];
        if (n > 0)
          KeplerGlobalCode.sscc(k, KeplerGlobalCode.Args[k], n);
        checked { ++k; }
      }
      while (k <= 17);
      int index1 = 0;
      int index2 = 0;
      double num2 = 0.0;
      while (true)
      {
        int num3 = plan.arg_tbl[index1];
        int index3 = checked (index1 + 1);
        if (num3 >= 0)
        {
          if (num3 == 0)
          {
            int num4 = plan.arg_tbl[index3];
            index1 = checked (index3 + 1);
            double num5 = plan.lon_tbl[index2];
            checked { ++index2; }
            int num6 = 0;
            int num7 = checked (num4 - 1);
            int num8 = num6;
            while (num8 <= num7)
            {
              num5 = num5 * num1 + plan.lon_tbl[index2];
              checked { ++index2; }
              checked { ++num8; }
            }
            num2 += num5;
          }
          else
          {
            int num4 = 0;
            double num5 = 0.0;
            double num6 = 0.0;
            int num7 = 0;
            int num8 = checked (num3 - 1);
            int num9 = num7;
            while (num9 <= num8)
            {
              int num10 = plan.arg_tbl[index3];
              int index4 = checked (index3 + 1);
              int index5 = checked (plan.arg_tbl[index4] - 1);
              index3 = checked (index4 + 1);
              if (num10 != 0)
              {
                int index6 = checked (unchecked (num10 >= 0) ? num10 : -num10 - 1);
                double num11 = KeplerGlobalCode.ss[index5, index6];
                if (num10 < 0)
                  num11 = -num11;
                double num12 = KeplerGlobalCode.cc[index5, index6];
                if (num4 == 0)
                {
                  num6 = num11;
                  num5 = num12;
                  num4 = 1;
                }
                else
                {
                  double num13 = num11 * num5 + num12 * num6;
                  num5 = num12 * num5 - num11 * num6;
                  num6 = num13;
                }
              }
              checked { ++num9; }
            }
            int num14 = plan.arg_tbl[index3];
            index1 = checked (index3 + 1);
            double num15 = plan.lon_tbl[index2];
            int index7 = checked (index2 + 1);
            double num16 = plan.lon_tbl[index7];
            index2 = checked (index7 + 1);
            int num17 = 0;
            int num18 = checked (num14 - 1);
            int num19 = num17;
            while (num19 <= num18)
            {
              num15 = num15 * num1 + plan.lon_tbl[index2];
              int index4 = checked (index2 + 1);
              num16 = num16 * num1 + plan.lon_tbl[index4];
              index2 = checked (index4 + 1);
              checked { ++num19; }
            }
            num2 += num15 * num5 + num16 * num6;
          }
        }
        else
          break;
      }
      return plan.trunclvl * num2;
    }

    internal static int gmoon(double J, ref double[] rect, ref double[] pol)
    {
      KeplerGlobalCode.g2plan(J, ref Mlr404Data.moonlr, ref pol);
      double num1 = pol[0] + KeplerGlobalCode.LP_equinox;
      if (num1 < -645000.0)
        num1 += 1296000.0;
      if (num1 > 645000.0)
        num1 -= 1296000.0;
      pol[0] = 4.84813681109536E-06 * num1;
      double num2 = KeplerGlobalCode.g1plan(J, ref Mlat404Data.moonlat);
      pol[1] = 4.84813681109536E-06 * num2;
      double num3 = (1.0 + 4.84813681109536E-06 * pol[2]) * Mlr404Data.moonlr.distance;
      pol[2] = num3;
      double eps = 0;
            double coseps = 0;
            double sineps = 0;
            KeplerGlobalCode.epsiln(J, ref eps, ref coseps, ref sineps);
      double num4 = Math.Cos(pol[1]);
      double num5 = Math.Sin(pol[1]);
      double num6 = Math.Cos(pol[0]);
      double num7 = Math.Sin(pol[0]);
      rect[0] = num4 * num6 * num3;
      rect[1] = (coseps * num4 * num7 - sineps * num5) * num3;
      rect[2] = (sineps * num4 * num7 + coseps * num5) * num3;
      return 0;
    }

    internal struct plantbl
    {
      internal int maxargs;
      internal int[] max_harmonic;
      internal int max_power_of_t;
      internal int[] arg_tbl;
      internal double[] lon_tbl;
      internal double[] lat_tbl;
      internal double[] rad_tbl;
      internal double distance;
      internal double timescale;
      internal double trunclvl;

      internal plantbl(int ma, int[] mh, int mpt, int[] at, double[] lot, double[] lat, double[] rat, double dis, double ts, double tl)
      {
        this = new KeplerGlobalCode.plantbl();
        this.maxargs = ma;
        this.max_harmonic = mh;
        this.max_power_of_t = mpt;
        this.arg_tbl = at;
        this.lon_tbl = lot;
        this.lat_tbl = lat;
        this.rad_tbl = rat;
        this.distance = dis;
        this.timescale = ts;
        this.trunclvl = tl;
      }
    }

    internal struct orbit
    {
      internal string obname;
      internal double epoch;
      internal double i;
      internal double W;
      internal double wp;
      internal double a;
      internal double dm;
      internal double ecc;
      internal double M;
      internal double equinox;
      internal double mag;
      internal double sdiam;
      internal KeplerGlobalCode.plantbl ptable;
      internal double L;
      internal double r;
      internal double plat;

      internal orbit(string obn, double ep, double i_p, double W_p, double wp_p, double a_p, double dm_p, double ecc_p, double M_p, double eq, double mg, double sd, KeplerGlobalCode.plantbl pt, double L_p, double r_p, double pl)
      {
        this = new KeplerGlobalCode.orbit();
        this.obname = obn;
        this.epoch = ep;
        this.i = i_p;
        this.W = W_p;
        this.wp = wp_p;
        this.a = a_p;
        this.dm = dm_p;
        this.ecc = ecc_p;
        this.M = M_p;
        this.equinox = eq;
        this.mag = mg;
        this.sdiam = sd;
        this.ptable = pt;
        this.L = L_p;
        this.r = r_p;
        this.plat = pl;
      }
    }
  }
}
