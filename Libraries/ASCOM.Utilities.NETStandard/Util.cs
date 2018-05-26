// Decompiled with JetBrains decompiler
// Type: ASCOM.Utilities.Util
// Assembly: ASCOM.Utilities, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 06F2ED6E-C559-496D-B7FF-DCA800F2230D
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.Utilities\6.0.0.0__565de7938946fba7\ASCOM.Utilities.dll


using ASCOM.Utilities.Interfaces;
using RACI.Data;
//using Microsoft.VisualBasic;
//using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections;
using System.Diagnostics;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace ASCOM.Utilities
{
    //[Guid("E861C6D8-B55B-494a-BC59-0F27F981CA98")]
    [ClassInterface(ClassInterfaceType.None)]
    //[ComVisible(true)]
    public class Util : IUtil, IUtilExtra, IDisposable
    {
        private Stopwatch m_StopWatch;
        private string m_SerTraceFile;
        private SystemHelper sys;
        private IAscomDataStore dStore;
        private bool disposedValue;
        private Version ver=null;
        private Version Version
        {
            get
            {
                //TODO: Implement thread-safe locking
                if (ver == null)
                {
                    string fullVersion = dStore.GetProfile("Platform", "Platform Version");
                    ver = new Version(fullVersion ?? "0.0.0.0");
                }
                return ver;
            }
        }

        public string PlatformVersion
        {
            get
            {
                string vString = dStore.GetProfile("", "PlatformVersion");
                //string Message = AscomSharedCode.ConditionPlatformVersion(vString, dStore, TL);
                string Message = AscomSharedCode.ConditionPlatformVersion(vString, null, TL);
                TL.LogMessage("PlatformVersion Get", Message);
                return Message;
            }
        }

        public string SerialTraceFile
        {
            get
            {
                return m_SerTraceFile;
            }
            set
            {
                m_SerTraceFile = value;
            }
        }

        public bool SerialTrace
        {
            get
            {
                return SerialTraceFile!="";
            }
            set
            {
                if (value)
                    dStore.WriteProfile("", "SerTraceFile", m_SerTraceFile);
                else
                    dStore.WriteProfile("", "SerTraceFile", "");
            }
        }

        public string TimeZoneName
        {
            get
            {
                return GetTimeZoneName();
            }
        }

        public double TimeZoneOffset
        {
            get
            {
                return GetTimeZoneOffset();
            }
        }

        public DateTime UTCDate
        {
            get
            {
                return DateTime.UtcNow;
            }
        }

        public double JulianDate
        {
            get
            {
                return DateUTCToJulian(DateTime.UtcNow);
            }
        }

        public int MajorVersion
        {
            get => Version.Major;
        }

        public int MinorVersion
        {
            get => Version.Minor;
        }

        public int ServicePack
        {
            get => Version.Build;
        }

        public int BuildNumber
        {
            get => Version.Revision;
        }

        #region Util Extensions
        public RaciSystem LocalSystem => sys.System;
        public RaciUser CurrentUser => sys.GetUser();
        public String Hostname => LocalSystem.Hostname;
        public String CurrentUserHome => CurrentUser.HomeDir;
        public TraceLogger TL { get; private set; }
        #endregion

        public Util()
        {
            m_StopWatch = new Stopwatch();
            m_SerTraceFile = "C:\\SerialTrace.txt";
            disposedValue = false;
            sys = new SystemHelper();
            bool enableTL= RegistryCommonCode.GetBool("Trace Util", false);
            TL = new TraceLogger("", "Util");
            TL.Enabled = enableTL;
            TL.LogMessage("New", "Trace logger created OK");
            dStore = new RegistryAccess();
            WaitForMilliseconds(1);
        }

        ~Util()
        {
            Dispose(false);
            // ISSUE: explicit finalizer call
            //base.Finalize();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                int num = disposing ? 1 : 0;
                if (dStore != null)
                {
                    dStore.Dispose();
                    dStore = null;
                }
            }
            disposedValue = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize((object)this);
        }

        public void WaitForMilliseconds(int Milliseconds)
        {
            m_StopWatch.Reset();
            m_StopWatch.Start();
            if (Milliseconds > 20)
            {
                double num = (double)checked(Milliseconds - 20) * (double)Stopwatch.Frequency / 1000.0;
                do
                {
                    Thread.Sleep(10);
                    //Application.DoEvents();
                }
                while ((double)m_StopWatch.ElapsedTicks < num);
            }
            double num1 = (double)Milliseconds * (double)Stopwatch.Frequency / 1000.0;
            while ((double)m_StopWatch.ElapsedTicks < num1)
                Thread.Sleep(0);
        }

        public double DMSToDegrees(string DMS)
        {
            DMS = Strings.Trim(DMS);
            short num1;
            if (Operators.CompareString(Strings.Left(DMS, 1), "-", false) == 0)
            {
                num1 = (short)-1;
                DMS = Strings.Right(DMS, checked(Strings.Len(DMS) - 1));
            }
            else
                num1 = (short)1;
            MatchCollection matchCollection = new Regex(Strings.InStr(Conversions.ToString(1.1), ",", CompareMethod.Binary) <= 0 ? "[0-9\\.]+" : "[0-9\\,]+").Matches(DMS);
            double num2 = 0.0;
            if (matchCollection.Count > 0)
            {
                num2 = Conversions.ToDouble(matchCollection[0].Value);
                if (matchCollection.Count > 1)
                {
                    num2 += Conversions.ToDouble(matchCollection[1].Value) / 60.0;
                    if (matchCollection.Count > 2)
                        num2 += Conversions.ToDouble(matchCollection[2].Value) / 3600.0;
                }
            }
            return (double)num1 * num2;
        }

        public double HMSToHours(string HMS)
        {
            return DMSToDegrees(HMS);
        }

        public double HMSToDegrees(string HMS)
        {
            return DMSToDegrees(HMS) * 15.0;
        }

        //[ComVisible(false)]
        public string DegreesToDMS(double Degrees)
        {
            return DegreesToDMS(Degrees, "° ", "' ", "\"", 0);
        }

        //[ComVisible(false)]
        public string DegreesToDMS(double Degrees, string DegDelim)
        {
            return DegreesToDMS(Degrees, DegDelim, "' ", "\"", 0);
        }

        //[ComVisible(false)]
        public string DegreesToDMS(double Degrees, string DegDelim, string MinDelim)
        {
            return DegreesToDMS(Degrees, DegDelim, MinDelim, "\"", 0);
        }

        //[ComVisible(false)]
        public string DegreesToDMS(double Degrees, string DegDelim, string MinDelim, string SecDelim)
        {
            return DegreesToDMS(Degrees, DegDelim, MinDelim, SecDelim, 0);
        }

        public string DegreesToDMS(double Degrees, string DegDelim, string MinDelim, string SecDelim, int SecDecimalDigits)
        {
            bool flag;
            if (Degrees < 0.0)
            {
                Degrees = -Degrees;
                flag = true;
            }
            else
                flag = false;
            string str1 = Strings.Format((object)Conversion.Fix(Degrees), "00");
            Degrees = (Degrees - Conversions.ToDouble(str1)) * 60.0;
            string Left = Strings.Format((object)Conversion.Fix(Degrees), "00");
            Degrees = (Degrees - Conversions.ToDouble(Left)) * 60.0;
            string Style;
            if (SecDecimalDigits == 0)
            {
                Style = "00";
            }
            else
            {
                Style = "00.";
                int num1 = 1;
                int num2 = SecDecimalDigits;
                int num3 = num1;
                while (num3 <= num2)
                {
                    Style += "0";
                    checked { ++num3; }
                }
            }
            string str2 = Strings.Format((object)Degrees, Style);
            if (Operators.CompareString(Strings.Left(str2, 2), "60", false) == 0)
            {
                str2 = Strings.Format((object)0, Style);
                Left = Strings.Format((object)checked((int)Conversions.ToShort(Left) + 1), "00");
                if (Operators.CompareString(Left, "60", false) == 0)
                {
                    Left = "00";
                    str1 = Strings.Format((object)checked((int)Conversions.ToShort(str1) + 1), "00");
                }
            }
            string str3 = str1 + DegDelim + Left + MinDelim + str2 + SecDelim;
            if (flag)
                str3 = "-" + str3;
            return str3;
        }

        //[ComVisible(false)]
        public string HoursToHMS(double Hours)
        {
            return DegreesToDMS(Hours, ":", ":", "", 0);
        }

        //[ComVisible(false)]
        public string HoursToHMS(double Hours, string HrsDelim)
        {
            return DegreesToDMS(Hours, HrsDelim, ":", "", 0);
        }

        //[ComVisible(false)]
        public string HoursToHMS(double Hours, string HrsDelim, string MinDelim)
        {
            return DegreesToDMS(Hours, HrsDelim, MinDelim, "", 0);
        }

        //[ComVisible(false)]
        public string HoursToHMS(double Hours, string HrsDelim, string MinDelim, string SecDelim)
        {
            return DegreesToDMS(Hours, HrsDelim, MinDelim, SecDelim, 0);
        }

        public string HoursToHMS(double Hours, string HrsDelim, string MinDelim, string SecDelim, int SecDecimalDigits)
        {
            return DegreesToDMS(Hours, HrsDelim, MinDelim, SecDelim, SecDecimalDigits);
        }

        //[ComVisible(false)]
        public string DegreesToHMS(double Degrees)
        {
            return DegreesToHMS(Degrees, ":", ":", "", 0);
        }

        //[ComVisible(false)]
        public string DegreesToHMS(double Degrees, string HrsDelim)
        {
            return DegreesToHMS(Degrees, HrsDelim, ":", "", 0);
        }

        //[ComVisible(false)]
        public string DegreesToHMS(double Degrees, string HrsDelim, string MinDelim)
        {
            return DegreesToHMS(Degrees, HrsDelim, MinDelim, "", 0);
        }

        //[ComVisible(false)]
        public string DegreesToHMS(double Degrees, string HrsDelim, string MinDelim, string SecDelim)
        {
            return DegreesToHMS(Degrees, HrsDelim, MinDelim, SecDelim, 0);
        }

        public string DegreesToHMS(double Degrees, string HrsDelim, string MinDelim, string SecDelim, int SecDecimalDigits)
        {
            return DegreesToDMS(Degrees / 15.0, HrsDelim, MinDelim, SecDelim, SecDecimalDigits);
        }

        //[ComVisible(false)]
        public string DegreesToDM(double Degrees)
        {
            return DegreesToDM(Degrees, "° ", "'", 0);
        }

        //[ComVisible(false)]
        public string DegreesToDM(double Degrees, string DegDelim)
        {
            return DegreesToDM(Degrees, DegDelim, "'", 0);
        }

        //[ComVisible(false)]
        public string DegreesToDM(double Degrees, string DegDelim, string MinDelim)
        {
            return DegreesToDM(Degrees, DegDelim, MinDelim, 0);
        }

        public string DegreesToDM(double Degrees, string DegDelim, string MinDelim, int MinDecimalDigits)
        {
            bool flag;
            if (Degrees < 0.0)
            {
                Degrees = -Degrees;
                flag = true;
            }
            else
                flag = false;
            string str1 = Strings.Format((object)Conversion.Fix(Degrees), "00");
            Degrees = (Degrees - Conversions.ToDouble(str1)) * 60.0;
            string Style;
            if (MinDecimalDigits == 0)
            {
                Style = "00";
            }
            else
            {
                Style = "00.";
                int num1 = 1;
                int num2 = MinDecimalDigits;
                int num3 = num1;
                while (num3 <= num2)
                {
                    Style += "0";
                    checked { ++num3; }
                }
            }
            string str2 = Strings.Format((object)Degrees, Style);
            if (Operators.CompareString(Strings.Left(str2, 2), "60", false) == 0)
            {
                str2 = Strings.Format((object)0, Style);
                str1 = Strings.Format((object)checked((int)Conversions.ToShort(str1) + 1), "00");
            }
            string str3 = str1 + DegDelim + str2 + MinDelim;
            if (flag)
                str3 = "-" + str3;
            return str3;
        }

        //[ComVisible(false)]
        public string HoursToHM(double Hours)
        {
            return DegreesToDM(Hours, ":", "", 0);
        }

        //[ComVisible(false)]
        public string HoursToHM(double Hours, string HrsDelim)
        {
            return DegreesToDM(Hours, HrsDelim, "", 0);
        }

        //[ComVisible(false)]
        public string HoursToHM(double Hours, string HrsDelim, string MinDelim)
        {
            return DegreesToDM(Hours, HrsDelim, MinDelim, 0);
        }

        public string HoursToHM(double Hours, string HrsDelim, string MinDelim, int MinDecimalDigits)
        {
            return DegreesToDM(Hours, HrsDelim, MinDelim, MinDecimalDigits);
        }

        //[ComVisible(false)]
        public string DegreesToHM(double Degrees)
        {
            return DegreesToHM(Degrees, ":", "", 0);
        }

        //[ComVisible(false)]
        public string DegreesToHM(double Degrees, string HrsDelim)
        {
            return DegreesToHM(Degrees, HrsDelim, "", 0);
        }

        //[ComVisible(false)]
        public string DegreesToHM(double Degrees, string HrsDelim, string MinDelim)
        {
            return DegreesToHM(Degrees, HrsDelim, MinDelim, 0);
        }

        public string DegreesToHM(double Degrees, string HrsDelim, string MinDelim, int MinDecimalDigits)
        {
            return DegreesToDM(Degrees / 15.0, HrsDelim, MinDelim, MinDecimalDigits);
        }

        public bool IsMinimumRequiredVersion(int RequiredMajorVersion, int RequiredMinorVersion)
        {
            return new Version(dStore.GetProfile("", "PlatformVersion")).CompareTo(new Version(RequiredMajorVersion, RequiredMinorVersion)) >= 0;
        }

        public double DateLocalToJulian(DateTime LocalDate)
        {
            return DateUTCToJulian(CvtUTC(ref LocalDate));
        }

        public DateTime DateJulianToLocal(double JD)
        {
            DateTime utc = DateJulianToUTC(JD);
            return CvtLocal(ref utc);
        }

        public double DateUTCToJulian(DateTime UTCDate)
        {
            return UTCDate.ToOADate() + 2415018.5;
        }

        public DateTime DateJulianToUTC(double JD)
        {
            return DateTime.FromOADate(JD - 2415018.5);
        }

        public DateTime DateUTCToLocal(DateTime UTCDate)
        {
            return CvtLocal(ref UTCDate);
        }

        public DateTime DateLocalToUTC(DateTime LocalDate)
        {
            return CvtUTC(ref LocalDate);
        }

        public ArrayList ToStringCollection(string[] stringArray)
        {
            ArrayList arrayList = new ArrayList();
            string[] strArray = stringArray;
            int index = 0;
            while (index < strArray.Length)
            {
                string str = strArray[index];
                arrayList.Add((object)str);
                checked { ++index; }
            }
            return arrayList;
        }

        public ArrayList ToIntegerCollection(int[] integerArray)
        {
            ArrayList arrayList = new ArrayList();
            int[] numArray = integerArray;
            int index = 0;
            while (index < numArray.Length)
            {
                int num = numArray[index];
                arrayList.Add((object)num);
                checked { ++index; }
            }
            return arrayList;
        }

        public double ConvertUnits(double InputValue, Units FromUnits, Units ToUnits)
        {
            if (FromUnits >= Units.metresPerSecond & FromUnits <= Units.knots & ToUnits >= Units.metresPerSecond & ToUnits <= Units.knots)
            {
                double num;
                switch (FromUnits)
                {
                    case Units.metresPerSecond:
                        num = InputValue;
                        break;
                    case Units.milesPerHour:
                        num = InputValue * 0.44704;
                        break;
                    case Units.knots:
                        num = InputValue * 0.514444444;
                        break;
                    default:
                        throw new InvalidValueException("Unknown \"From\" speed units: " + FromUnits.ToString());
                }
                switch (ToUnits)
                {
                    case Units.metresPerSecond:
                        return num;
                    case Units.milesPerHour:
                        return num / 0.44704;
                    case Units.knots:
                        return num / 0.514444444;
                    default:
                        throw new InvalidValueException("Unknown \"To\" speed units: " + ToUnits.ToString());
                }
            }
            else if (FromUnits >= Units.degreesCelsius & FromUnits <= Units.degreesKelvin & ToUnits >= Units.degreesCelsius & ToUnits <= Units.degreesKelvin)
            {
                double num;
                switch (FromUnits)
                {
                    case Units.degreesCelsius:
                        num = InputValue - -273.15;
                        break;
                    case Units.degreesFarenheit:
                        num = (InputValue + 459.67) * 5.0 / 9.0;
                        break;
                    case Units.degreesKelvin:
                        num = InputValue;
                        break;
                    default:
                        throw new InvalidValueException("Unknown \"From\" temperature units: " + FromUnits.ToString());
                }
                switch (ToUnits)
                {
                    case Units.degreesCelsius:
                        return num - 273.15;
                    case Units.degreesFarenheit:
                        return num * 9.0 / 5.0 - 459.67;
                    case Units.degreesKelvin:
                        return num;
                    default:
                        throw new InvalidValueException("Unknown \"To\" temperature units: " + ToUnits.ToString());
                }
            }
            else if (FromUnits >= Units.hPa & FromUnits <= Units.inHg & ToUnits >= Units.hPa & ToUnits <= Units.inHg)
            {
                double num;
                switch (FromUnits)
                {
                    case Units.hPa:
                        num = InputValue;
                        break;
                    case Units.mBar:
                        num = InputValue;
                        break;
                    case Units.mmHg:
                        num = InputValue * 1.33322368;
                        break;
                    case Units.inHg:
                        num = InputValue * 33.8638816;
                        break;
                    default:
                        throw new InvalidValueException("Unknown \"From\" pressure units: " + FromUnits.ToString());
                }
                switch (ToUnits)
                {
                    case Units.hPa:
                        return num;
                    case Units.mBar:
                        return num;
                    case Units.mmHg:
                        return num / 1.33322368;
                    case Units.inHg:
                        return num / 33.8638816;
                    default:
                        throw new InvalidValueException("Unknown \"To\" pressure units: " + ToUnits.ToString());
                }
            }
            else
            {
                if (!(FromUnits >= Units.mmPerHour & FromUnits <= Units.inPerHour & ToUnits >= Units.mmPerHour & ToUnits <= Units.inPerHour))
                    throw new ASCOM.InvalidOperationException("From and to units are not of the same type. From: " + FromUnits.ToString() + ", To: " + ToUnits.ToString());
                double num;
                switch (FromUnits)
                {
                    case Units.mmPerHour:
                        num = InputValue;
                        break;
                    case Units.inPerHour:
                        num = InputValue * 25.4;
                        break;
                    default:
                        throw new InvalidValueException("Unknown \"From\" rain rate units: " + FromUnits.ToString());
                }
                switch (ToUnits)
                {
                    case Units.mmPerHour:
                        return num;
                    case Units.inPerHour:
                        return num / 25.4;
                    default:
                        throw new InvalidValueException("Unknown \"To\" rain rate units: " + ToUnits.ToString());
                }
            }
        }

        public double Humidity2DewPoint(double RelativeHumidity, double AmbientTemperature)
        {
            if (RelativeHumidity < 0.0 | RelativeHumidity > 100.0)
                throw new InvalidValueException("Humidity2DewPoint - Relative humidity is < 0.0% or > 100.0%: " + RelativeHumidity.ToString());
            if (AmbientTemperature < -273.15 | AmbientTemperature > 100.0)
                throw new InvalidValueException("Humidity2DewPoint - Ambient temperature is < " + Conversions.ToString(-273.15) + "C or > 100.0C: " + AmbientTemperature.ToString());
            double num1 = 6.116441 * Math.Pow(10.0, 7.591386 * AmbientTemperature / (AmbientTemperature + 240.7263));
            double num2 = num1 * RelativeHumidity / 100.0;
            double num3 = 240.7263 / (7.591386 / Math.Log10(num2 / 6.116441) - 1.0);
            TL.LogMessage("Humidity2DewPoint", "DewPoint: " + Conversions.ToString(num3) + ", Given Relative Humidity: " + Conversions.ToString(RelativeHumidity) + ", Given Ambient temperaure: " + Conversions.ToString(AmbientTemperature) + ", Pws: " + Conversions.ToString(num1) + ", Pw: " + Conversions.ToString(num2));
            return num3;
        }

        public double DewPoint2Humidity(double DewPoint, double AmbientTemperature)
        {
            if (DewPoint < -273.15 | DewPoint > 100.0)
                throw new InvalidValueException("DewPoint2Humidity - Dew point is < " + Conversions.ToString(-273.15) + "C or > 100.0C: " + DewPoint.ToString());
            if (AmbientTemperature < -273.15 | AmbientTemperature > 100.0)
                throw new InvalidValueException("DewPoint2Humidity - Ambient temperature is < " + Conversions.ToString(-273.15) + "C or > 100.0C: " + AmbientTemperature.ToString());
            double num = 100.0 * Math.Pow(10.0, 7.591386 * (DewPoint / (DewPoint + 240.7263) - AmbientTemperature / (AmbientTemperature + 240.7263)));
            TL.LogMessage("DewPoint2Humidity", "RH: " + Conversions.ToString(num) + ", Given Dew point: " + Conversions.ToString(DewPoint) + ", Given Ambient temperaure: " + Conversions.ToString(AmbientTemperature));
            return num;
        }

        public double ConvertPressure(double Pressure, double FromAltitudeAboveMeanSeaLevel, double ToAltitudeAboveMeanSeaLevel)
        {
            double num1 = Pressure / Math.Pow(1.0 - 2.25577E-05 * FromAltitudeAboveMeanSeaLevel, 5.25588);
            double num2 = num1 * Math.Pow(1.0 - 2.25577E-05 * ToAltitudeAboveMeanSeaLevel, 5.25588);
            TL.LogMessage("ConvertPressure", "SeaLevelPressure: " + Conversions.ToString(num1) + ", ActualPressure: " + Conversions.ToString(num2) + ", Given Presure: " + Conversions.ToString(Pressure) + ", Given FromAltitudeAboveMeanSeaLevel: " + Conversions.ToString(FromAltitudeAboveMeanSeaLevel) + ", Given ToAltitudeAboveMeanSeaLevel: " + Conversions.ToString(ToAltitudeAboveMeanSeaLevel));
            return num2;
        }

        [return: MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_VARIANT)]
        public object ArrayToVariantArray(object SuppliedObject)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            try
            {
                if (SuppliedObject.GetType().IsArray)
                {
                    Array SuppliedArray = (Array)SuppliedObject;
                    System.Type type = SuppliedArray.GetType();
                    string name = type.GetElementType().Name;
                    TL.LogMessage("ArrayToVariantArray", "Array Type: " + type.Name + ", Element Type: " + name + ", Array Rank: " + Conversions.ToString(SuppliedArray.Rank));
                    string Left = name;
                    object objectValue;
                    if (Operators.CompareString(Left, typeof(object).Name, false) == 0)
                        objectValue = RuntimeHelpers.GetObjectValue(SuppliedObject);
                    else if (Operators.CompareString(Left, typeof(short).Name, false) == 0)
                        objectValue = RuntimeHelpers.GetObjectValue(ProcessArray<short>(RuntimeHelpers.GetObjectValue(SuppliedObject), SuppliedArray));
                    else if (Operators.CompareString(Left, typeof(int).Name, false) == 0)
                        objectValue = RuntimeHelpers.GetObjectValue(ProcessArray<int>(RuntimeHelpers.GetObjectValue(SuppliedObject), SuppliedArray));
                    else if (Operators.CompareString(Left, typeof(long).Name, false) == 0)
                        objectValue = RuntimeHelpers.GetObjectValue(ProcessArray<long>(RuntimeHelpers.GetObjectValue(SuppliedObject), SuppliedArray));
                    else if (Operators.CompareString(Left, typeof(ushort).Name, false) == 0)
                        objectValue = RuntimeHelpers.GetObjectValue(ProcessArray<ushort>(RuntimeHelpers.GetObjectValue(SuppliedObject), SuppliedArray));
                    else if (Operators.CompareString(Left, typeof(uint).Name, false) == 0)
                        objectValue = RuntimeHelpers.GetObjectValue(ProcessArray<uint>(RuntimeHelpers.GetObjectValue(SuppliedObject), SuppliedArray));
                    else if (Operators.CompareString(Left, typeof(ulong).Name, false) == 0)
                        objectValue = RuntimeHelpers.GetObjectValue(ProcessArray<ulong>(RuntimeHelpers.GetObjectValue(SuppliedObject), SuppliedArray));
                    else if (Operators.CompareString(Left, typeof(byte).Name, false) == 0)
                        objectValue = RuntimeHelpers.GetObjectValue(ProcessArray<byte>(RuntimeHelpers.GetObjectValue(SuppliedObject), SuppliedArray));
                    else if (Operators.CompareString(Left, typeof(sbyte).Name, false) == 0)
                        objectValue = RuntimeHelpers.GetObjectValue(ProcessArray<sbyte>(RuntimeHelpers.GetObjectValue(SuppliedObject), SuppliedArray));
                    else if (Operators.CompareString(Left, typeof(float).Name, false) == 0)
                        objectValue = RuntimeHelpers.GetObjectValue(ProcessArray<float>(RuntimeHelpers.GetObjectValue(SuppliedObject), SuppliedArray));
                    else if (Operators.CompareString(Left, typeof(double).Name, false) == 0)
                        objectValue = RuntimeHelpers.GetObjectValue(ProcessArray<double>(RuntimeHelpers.GetObjectValue(SuppliedObject), SuppliedArray));
                    else if (Operators.CompareString(Left, typeof(bool).Name, false) == 0)
                        objectValue = RuntimeHelpers.GetObjectValue(ProcessArray<bool>(RuntimeHelpers.GetObjectValue(SuppliedObject), SuppliedArray));
                    else if (Operators.CompareString(Left, typeof(DateTime).Name, false) == 0)
                        objectValue = RuntimeHelpers.GetObjectValue(ProcessArray<DateTime>(RuntimeHelpers.GetObjectValue(SuppliedObject), SuppliedArray));
                    else if (Operators.CompareString(Left, typeof(string).Name, false) == 0)
                    {
                        objectValue = RuntimeHelpers.GetObjectValue(ProcessArray<string>(RuntimeHelpers.GetObjectValue(SuppliedObject), SuppliedArray));
                    }
                    else
                    {
                        TL.LogMessage("ArrayToVariantArray", "Unsupported array type: " + name + ", throwing exception");
                        throw new InvalidValueException("Unsupported array type: " + name);
                    }
                    stopwatch.Stop();
                    TL.LogMessage("ArrayToVariantArray", "Completed processing in " + stopwatch.Elapsed.TotalMilliseconds.ToString("0.0") + " milliseconds");
                    return objectValue;
                }
                TL.LogMessage("ArrayToVariantArray", "Supplied object is not an array, throwing exception");
                throw new InvalidValueException("Supplied object is not an array");
            }
            catch (Exception ex)
            {
                //ProjectData.SetProjectError(ex);
                TL.LogMessageCrLf("ArrayToVariantArray", "Exception: " + ex.ToString());
                throw;
            }
        }

        private object ProcessArray<T>(object SuppliedObject, Array SuppliedArray)
        {
            switch (SuppliedArray.Rank)
            {
                case 1:
                    T[] objArray1 = (T[])SuppliedObject;
                    object[] objArray2 = new object[checked(SuppliedArray.GetLength(0) - 1 + 1)];
                    TL.LogMessage("ProcessArray", "Array Rank 1: " + Conversions.ToString(objArray1.GetLength(0)));
                    Array.Copy((Array)objArray1, (Array)objArray2, (long)objArray1.Length);
                    return (object)objArray2;
                case 2:
                    T[,] objArray3 = (T[,])SuppliedObject;
                    object[,] objArray4 = new object[checked(objArray3.GetLength(0) - 1 + 1), checked(objArray3.GetLength(1) - 1 + 1)];
                    TL.LogMessage("ProcessArray", "Array Rank 2: " + Conversions.ToString(objArray3.GetLength(0)) + " x " + Conversions.ToString(objArray3.GetLength(1)));
                    Array.Copy((Array)objArray3, (Array)objArray4, objArray3.LongLength);
                    return (object)objArray4;
                case 3:
                    T[,,] objArray5 = (T[,,])SuppliedObject;
                    object[,,] objArray6 = new object[checked(objArray5.GetLength(0) - 1 + 1), checked(objArray5.GetLength(1) - 1 + 1), checked(objArray5.GetLength(2) - 1 + 1)];
                    TL.LogMessage("ProcessArray", "Array Rank 3: " + Conversions.ToString(objArray5.GetLength(0)) + " x " + Conversions.ToString(objArray5.GetLength(1)) + " x " + Conversions.ToString(objArray5.GetLength(2)));
                    Array.Copy((Array)objArray5, (Array)objArray6, objArray5.LongLength);
                    return (object)objArray6;
                case 4:
                    T[,,,] objArray7 = (T[,,,])SuppliedObject;
                    object[,,,] objArray8 = new object[checked(objArray7.GetLength(0) - 1 + 1), checked(objArray7.GetLength(1) - 1 + 1), checked(objArray7.GetLength(2) - 1 + 1), checked(objArray7.GetLength(3) - 1 + 1)];
                    TL.LogMessage("ProcessArray", "Array Rank 4: " + Conversions.ToString(objArray7.GetLength(0)) + " x " + Conversions.ToString(objArray7.GetLength(1)) + " x " + Conversions.ToString(objArray7.GetLength(2)) + " x " + Conversions.ToString(objArray7.GetLength(3)));
                    Array.Copy((Array)objArray7, (Array)objArray8, objArray7.LongLength);
                    return (object)objArray8;
                case 5:
                    T[,,,,] objArray9 = (T[,,,,])SuppliedObject;
                    object[,,,,] objArray10 = new object[checked(objArray9.GetLength(0) - 1 + 1), checked(objArray9.GetLength(1) - 1 + 1), checked(objArray9.GetLength(2) - 1 + 1), checked(objArray9.GetLength(3) - 1 + 1), checked(objArray9.GetLength(4) - 1 + 1)];
                    TL.LogMessage("ProcessArray", "Array Rank 5: " + Conversions.ToString(objArray9.GetLength(0)) + " x " + Conversions.ToString(objArray9.GetLength(1)) + " x " + Conversions.ToString(objArray9.GetLength(2)) + " x " + Conversions.ToString(objArray9.GetLength(3)) + " x " + Conversions.ToString(objArray9.GetLength(4)));
                    Array.Copy((Array)objArray9, (Array)objArray10, objArray9.LongLength);
                    return (object)objArray10;
                default:
                    TL.LogMessage("ProcessArrayOfType", "Array rank is outside the range 1..5: " + Conversions.ToString(SuppliedArray.Rank) + ", throwing exception");
                    throw new InvalidValueException("Array rank is outside the range 1..5: " + Conversions.ToString(SuppliedArray.Rank));
            }
        }

        private double GetTimeZoneOffset()
        {
            return -TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).TotalHours;
        }

        private string GetTimeZoneName()
        {
            if (TimeZone.CurrentTimeZone.IsDaylightSavingTime(DateTime.Now))
                return TimeZone.CurrentTimeZone.DaylightName;
            return TimeZone.CurrentTimeZone.StandardName;
        }

        private DateTime CvtUTC(ref DateTime d)
        {
            return DateTime.FromOADate(d.ToOADate() + GetTimeZoneOffset() / 24.0);
        }

        private DateTime CvtLocal(ref DateTime d)
        {
            return DateTime.FromOADate(d.ToOADate() - GetTimeZoneOffset() / 24.0);
        }
    }
}