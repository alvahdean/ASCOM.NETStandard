// Decompiled with JetBrains decompiler
// Type: ASCOM.Astrometry.DeltatCode
// Assembly: ASCOM.Astrometry, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 695EF5AA-123E-4689-AAF5-64FD1324F6BB
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.Astrometry\6.0.0.0__565de7938946fba7\ASCOM.Astrometry.dll

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;

namespace ASCOM.Astrometry
{
  //
  internal sealed class DeltatCode
  {
    [SpecialName]
    private static double _DeltaTCalc__01DD__ans= double.MinValue;
    [SpecialName]
    private static double _DeltaTCalc__01DD__lasttjd= double.MinValue;

    [DebuggerNonUserCode]
    static DeltatCode()
    {
    }

    public static double DeltaTCalc(double tjd)
    {
      if (tjd == DeltatCode._DeltaTCalc__01DD__lasttjd)
        return DeltatCode._DeltaTCalc__01DD__ans;
      DeltatCode._DeltaTCalc__01DD__lasttjd = tjd;
      double d = 2000.0 + (tjd - 2451545.0) / 365.25;
      if (d >= 2015.75 & d < double.MaxValue)
      {
        DeltatCode._DeltaTCalc__01DD__ans = 0.02002376 * d * d + -80.27921003 * d + 80529.32;
        return DeltatCode._DeltaTCalc__01DD__ans;
      }
      if (d >= 2011.75 & d < 2015.75)
      {
        DeltatCode._DeltaTCalc__01DD__ans = 0.00231189 * d * d + -8.85231952 * d + 8518.54;
        return DeltatCode._DeltaTCalc__01DD__ans;
      }
      if (d >= 2011.0 & d < 2011.75)
      {
        double num = d - 2000.0;
        DeltatCode._DeltaTCalc__01DD__ans = 62.92 + num * (0.32217 + num * 0.005589);
        return DeltatCode._DeltaTCalc__01DD__ans;
      }
      short[] numArray1 = new short[392]
      {
        (short) 12400,
        (short) 11900,
        (short) 11500,
        (short) 11000,
        (short) 10600,
        (short) 10200,
        (short) 9800,
        (short) 9500,
        (short) 9100,
        (short) 8800,
        (short) 8500,
        (short) 8200,
        (short) 7900,
        (short) 7700,
        (short) 7400,
        (short) 7200,
        (short) 7000,
        (short) 6700,
        (short) 6500,
        (short) 6300,
        (short) 6200,
        (short) 6000,
        (short) 5800,
        (short) 5700,
        (short) 5500,
        (short) 5400,
        (short) 5300,
        (short) 5100,
        (short) 5000,
        (short) 4900,
        (short) 4800,
        (short) 4700,
        (short) 4600,
        (short) 4500,
        (short) 4400,
        (short) 4300,
        (short) 4200,
        (short) 4100,
        (short) 4000,
        (short) 3800,
        (short) 3700,
        (short) 3600,
        (short) 3500,
        (short) 3400,
        (short) 3300,
        (short) 3200,
        (short) 3100,
        (short) 3000,
        (short) 2800,
        (short) 2700,
        (short) 2600,
        (short) 2500,
        (short) 2400,
        (short) 2300,
        (short) 2200,
        (short) 2100,
        (short) 2000,
        (short) 1900,
        (short) 1800,
        (short) 1700,
        (short) 1600,
        (short) 1500,
        (short) 1400,
        (short) 1400,
        (short) 1300,
        (short) 1200,
        (short) 1200,
        (short) 1100,
        (short) 1100,
        (short) 1000,
        (short) 1000,
        (short) 1000,
        (short) 900,
        (short) 900,
        (short) 900,
        (short) 900,
        (short) 900,
        (short) 900,
        (short) 900,
        (short) 900,
        (short) 900,
        (short) 900,
        (short) 900,
        (short) 900,
        (short) 900,
        (short) 900,
        (short) 900,
        (short) 900,
        (short) 1000,
        (short) 1000,
        (short) 1000,
        (short) 1000,
        (short) 1000,
        (short) 1000,
        (short) 1000,
        (short) 1000,
        (short) 1000,
        (short) 1100,
        (short) 1100,
        (short) 1100,
        (short) 1100,
        (short) 1100,
        (short) 1100,
        (short) 1100,
        (short) 1100,
        (short) 1100,
        (short) 1100,
        (short) 1100,
        (short) 1100,
        (short) 1100,
        (short) 1100,
        (short) 1100,
        (short) 1100,
        (short) 1100,
        (short) 1200,
        (short) 1200,
        (short) 1200,
        (short) 1200,
        (short) 1200,
        (short) 1200,
        (short) 1200,
        (short) 1200,
        (short) 1200,
        (short) 1200,
        (short) 1300,
        (short) 1300,
        (short) 1300,
        (short) 1300,
        (short) 1300,
        (short) 1300,
        (short) 1300,
        (short) 1400,
        (short) 1400,
        (short) 1400,
        (short) 1400,
        (short) 1400,
        (short) 1400,
        (short) 1400,
        (short) 1500,
        (short) 1500,
        (short) 1500,
        (short) 1500,
        (short) 1500,
        (short) 1500,
        (short) 1500,
        (short) 1600,
        (short) 1600,
        (short) 1600,
        (short) 1600,
        (short) 1600,
        (short) 1600,
        (short) 1600,
        (short) 1600,
        (short) 1600,
        (short) 1600,
        (short) 1700,
        (short) 1700,
        (short) 1700,
        (short) 1700,
        (short) 1700,
        (short) 1700,
        (short) 1700,
        (short) 1700,
        (short) 1700,
        (short) 1700,
        (short) 1700,
        (short) 1700,
        (short) 1700,
        (short) 1700,
        (short) 1700,
        (short) 1700,
        (short) 1700,
        (short) 1600,
        (short) 1600,
        (short) 1600,
        (short) 1600,
        (short) 1500,
        (short) 1500,
        (short) 1400,
        (short) 1400,
        (short) 1370,
        (short) 1340,
        (short) 1310,
        (short) 1290,
        (short) 1270,
        (short) 1260,
        (short) 1250,
        (short) 1250,
        (short) 1250,
        (short) 1250,
        (short) 1250,
        (short) 1250,
        (short) 1250,
        (short) 1250,
        (short) 1250,
        (short) 1250,
        (short) 1250,
        (short) 1240,
        (short) 1230,
        (short) 1220,
        (short) 1200,
        (short) 1170,
        (short) 1140,
        (short) 1110,
        (short) 1060,
        (short) 1020,
        (short) 960,
        (short) 910,
        (short) 860,
        (short) 800,
        (short) 750,
        (short) 700,
        (short) 660,
        (short) 630,
        (short) 600,
        (short) 580,
        (short) 570,
        (short) 560,
        (short) 560,
        (short) 560,
        (short) 570,
        (short) 580,
        (short) 590,
        (short) 610,
        (short) 620,
        (short) 630,
        (short) 650,
        (short) 660,
        (short) 680,
        (short) 690,
        (short) 710,
        (short) 720,
        (short) 730,
        (short) 740,
        (short) 750,
        (short) 760,
        (short) 770,
        (short) 770,
        (short) 780,
        (short) 780,
        (short) 788,
        (short) 782,
        (short) 754,
        (short) 697,
        (short) 640,
        (short) 602,
        (short) 541,
        (short) 410,
        (short) 292,
        (short) 182,
        (short) 161,
        (short) 10,
        (short) -102,
        (short) sbyte.MinValue,
        (short) -269,
        (short) -324,
        (short) -364,
        (short) -454,
        (short) -471,
        (short) -511,
        (short) -540,
        (short) -542,
        (short) -520,
        (short) -546,
        (short) -546,
        (short) -579,
        (short) -563,
        (short) -564,
        (short) -580,
        (short) -566,
        (short) -587,
        (short) -601,
        (short) -619,
        (short) -664,
        (short) -644,
        (short) -647,
        (short) -609,
        (short) -576,
        (short) -466,
        (short) -374,
        (short) -272,
        (short) -154,
        (short) -2,
        (short) 124,
        (short) 264,
        (short) 386,
        (short) 537,
        (short) 614,
        (short) 775,
        (short) 913,
        (short) 1046,
        (short) 1153,
        (short) 1336,
        (short) 1465,
        (short) 1601,
        (short) 1720,
        (short) 1824,
        (short) 1906,
        (short) 2025,
        (short) 2095,
        (short) 2116,
        (short) 2225,
        (short) 2241,
        (short) 2303,
        (short) 2349,
        (short) 2362,
        (short) 2386,
        (short) 2449,
        (short) 2434,
        (short) 2408,
        (short) 2402,
        (short) 2400,
        (short) 2387,
        (short) 2395,
        (short) 2386,
        (short) 2393,
        (short) 2373,
        (short) 2392,
        (short) 2396,
        (short) 2402,
        (short) 2433,
        (short) 2483,
        (short) 2530,
        (short) 2570,
        (short) 2624,
        (short) 2677,
        (short) 2728,
        (short) 2778,
        (short) 2825,
        (short) 2871,
        (short) 2915,
        (short) 2957,
        (short) 2997,
        (short) 3036,
        (short) 3072,
        (short) 3107,
        (short) 3135,
        (short) 3168,
        (short) 3218,
        (short) 3268,
        (short) 3315,
        (short) 3359,
        (short) 3400,
        (short) 3447,
        (short) 3503,
        (short) 3573,
        (short) 3654,
        (short) 3743,
        (short) 3829,
        (short) 3920,
        (short) 4018,
        (short) 4117,
        (short) 4223,
        (short) 4337,
        (short) 4449,
        (short) 4548,
        (short) 4646,
        (short) 4752,
        (short) 4853,
        (short) 4959,
        (short) 5054,
        (short) 5138,
        (short) 5217,
        (short) 5296,
        (short) 5379,
        (short) 5434,
        (short) 5487,
        (short) 5532,
        (short) 5582,
        (short) 5630,
        (short) 5686,
        (short) 5757,
        (short) 5831,
        (short) 5912,
        (short) 5998,
        (short) 6078,
        (short) 6163,
        (short) 6230,
        (short) 6296,
        (short) 6347,
        (short) 6383,
        (short) 6409,
        (short) 6430,
        (short) 6447,
        (short) 6457,
        (short) 6469,
        (short) 6485,
        (short) 6515,
        (short) 6546,
        (short) 6570,
        (short) 6650,
        (short) 6710
      };
      int[] numArray2 = new int[7];
      if (d < 1620.0)
      {
        if (d >= 948.0)
        {
          double num = 0.01 * (d - 2000.0);
          DeltatCode._DeltaTCalc__01DD__ans = (23.58 * num + 100.3) * num + 101.6;
        }
        else
        {
          double num = 0.01 * (d - 2000.0) + 3.75;
          DeltatCode._DeltaTCalc__01DD__ans = 35.0 * num * num + 40.0;
        }
        return DeltatCode._DeltaTCalc__01DD__ans;
      }
      double num1 = Math.Floor(d);
      int index1 = checked ((int) Math.Round(unchecked (num1 - 1620.0)));
      DeltatCode._DeltaTCalc__01DD__ans = (double) numArray1[index1];
      int index2 = checked (index1 + 1);
      if (index2 < 392)
      {
        double num2 = d - num1;
        DeltatCode._DeltaTCalc__01DD__ans += num2 * (double) checked ((short) unchecked ((int) numArray1[index2] - (int) numArray1[index1]));
        if (!(checked (index1 - 1) < 0 | checked (index1 + 2) >= 392))
        {
          int index3 = checked (index1 - 2);
          int index4 = 0;
          do
          {
            numArray2[index4] = !(index3 < 0 | checked (index3 + 1) >= 392) ? (int) checked ((short) unchecked ((int) numArray1[checked (index3 + 1)] - (int) numArray1[index3])) : 0;
            checked { ++index3; }
            checked { ++index4; }
          }
          while (index4 <= 4);
          int index5 = 0;
          do
          {
            numArray2[index5] = checked (numArray2[index5 + 1] - numArray2[index5]);
            checked { ++index5; }
          }
          while (index5 <= 3);
          double num3 = 0.25 * num2 * (num2 - 1.0);
          DeltatCode._DeltaTCalc__01DD__ans += num3 * (double) checked (numArray2[1] + numArray2[2]);
          if (checked (index1 + 2) < 392)
          {
            int index6 = 0;
            do
            {
              numArray2[index6] = checked (numArray2[index6 + 1] - numArray2[index6]);
              checked { ++index6; }
            }
            while (index6 <= 2);
            double num4 = 2.0 * num3 / 3.0;
            DeltatCode._DeltaTCalc__01DD__ans += (num2 - 0.5) * num4 * (double) numArray2[1];
            if (!(checked (index1 - 2) < 0 | checked (index1 + 3) > 392))
            {
              int index7 = 0;
              do
              {
                numArray2[index7] = checked (numArray2[index7 + 1] - numArray2[index7]);
                checked { ++index7; }
              }
              while (index7 <= 1);
              double num5 = 0.125 * num4 * (num2 + 1.0) * (num2 - 2.0);
              DeltatCode._DeltaTCalc__01DD__ans += num5 * (double) checked (numArray2[0] + numArray2[1]);
            }
          }
        }
      }
      DeltatCode._DeltaTCalc__01DD__ans *= 0.01;
      if (d < 1955.0)
      {
        double num2 = d - 1955.0;
        DeltatCode._DeltaTCalc__01DD__ans += -1.81999999999999E-05 * num2 * num2;
      }
      return DeltatCode._DeltaTCalc__01DD__ans;
    }
  }
}
