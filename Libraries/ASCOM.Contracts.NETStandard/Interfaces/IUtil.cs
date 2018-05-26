// Decompiled with JetBrains decompiler
// Type: ASCOM.Utilities.Interfaces.IUtil
// Assembly: ASCOM.Utilities, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 06F2ED6E-C559-496D-B7FF-DCA800F2230D
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.Utilities\6.0.0.0__565de7938946fba7\ASCOM.Utilities.dll

using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace ASCOM.Utilities.Interfaces
{
  //[ComVisible(true)]
  //[Guid("DF41946E-EE14-40f7-AA66-DD8A92E36EF2")]
  public interface IUtil
  {
    [DispId(11)]
    string PlatformVersion { get; }

    [DispId(12)]
    string SerialTraceFile { get; set; }

    [DispId(13)]
    bool SerialTrace { get; set; }

    [DispId(14)]
    string TimeZoneName { get; }

    [DispId(15)]
    double TimeZoneOffset { get; }

    [DispId(16)]
    DateTime UTCDate { get; }

    [DispId(17)]
    double JulianDate { get; }

    [DispId(28)]
    int MajorVersion { get; }

    [DispId(29)]
    int MinorVersion { get; }

    [DispId(30)]
    int ServicePack { get; }

    [DispId(31)]
    int BuildNumber { get; }

    [DispId(1)]
    void WaitForMilliseconds(int Milliseconds);

    [DispId(2)]
    double DMSToDegrees(string DMS);

    [DispId(3)]
    double HMSToHours(string HMS);

    [DispId(4)]
    double HMSToDegrees(string HMS);

    [DispId(5)]
    string DegreesToDMS(double Degrees, string DegDelim, string MinDelim, string SecDelim, int SecDecimalDigits);

    [DispId(6)]
    string HoursToHMS(double Hours, string HrsDelim, string MinDelim, string SecDelim, int SecDecimalDigits);

    [DispId(7)]
    string DegreesToDM(double Degrees, string DegDelim, string MinDelim, int MinDecimalDigits);

    [DispId(8)]
    string HoursToHM(double Hours, string HrsDelim, string MinDelim, int MinDecimalDigits);

    [DispId(9)]
    string DegreesToHMS(double Degrees, string HrsDelim, string MinDelim, string SecDelim, int SecDecimalDigits);

    [DispId(10)]
    string DegreesToHM(double Degrees, string HrsDelim, string MinDelim, int MinDecimalDigits);

    [DispId(18)]
    double DateLocalToJulian(DateTime LocalDate);

    [DispId(19)]
    DateTime DateJulianToLocal(double JD);

    [DispId(20)]
    double DateUTCToJulian(DateTime UTCDate);

    [DispId(21)]
    DateTime DateJulianToUTC(double JD);

    [DispId(22)]
    DateTime DateUTCToLocal(DateTime UTCDate);

    [DispId(23)]
    DateTime DateLocalToUTC(DateTime LocalDate);

    [DispId(24)]
    bool IsMinimumRequiredVersion(int RequiredMajorVersion, int RequiredMinorVersion);

    [DispId(25)]
    ArrayList ToStringCollection(string[] stringArray);

    [DispId(26)]
    ArrayList ToIntegerCollection(int[] integerArray);

    [DispId(27)]
    [return: MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_VARIANT)]
    object ArrayToVariantArray(object SuppliedObject);

    [DispId(32)]
    double ConvertUnits(double InputValue, Units FromUnits, Units ToUnits);

    [DispId(33)]
    double Humidity2DewPoint(double Humidity, double AmbientTemperature);

    [DispId(34)]
    double DewPoint2Humidity(double DewPoint, double AmbientTemperature);

    [DispId(35)]
    double ConvertPressure(double Pressure, double FromAltitudeAboveMeanSeaLevel, double ToAltitudeAboveMeanSeaLevel);
  }
}
