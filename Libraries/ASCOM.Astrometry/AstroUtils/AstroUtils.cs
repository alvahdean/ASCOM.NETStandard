// Decompiled with JetBrains decompiler
// Type: ASCOM.Astrometry.AstroUtils.AstroUtils
// Assembly: ASCOM.Astrometry, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 695EF5AA-123E-4689-AAF5-64FD1324F6BB
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.Astrometry\6.0.0.0__565de7938946fba7\ASCOM.Astrometry.dll

using ASCOM.Astrometry.NOVAS;
using ASCOM.Utilities;
using ASCOM;
using ASCOM.Utilities.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;

namespace ASCOM.Astrometry.AstroUtils
{
    //[ComVisible(true)]
    //[ClassInterface(ClassInterfaceType.None)]
    //[Guid("5679F94A-D4D1-40D3-A0F8-7CE61100A691")]
    public class AstroUtils : IAstroUtils, IDisposable
    {
        private ITraceLogger TL;
        private Util Utl;
        private IAccessExtra Store;
        private NOVAS31 Nov31;
        private int? UtcTaiOffset=null;
        private bool disposedValue;

        public double JulianDateUtc
        {
            get
            {
                DateTime utcNow = DateTime.UtcNow;
                double num = Nov31.JulianDate(Convert.ToInt16(utcNow.Year), Convert.ToInt16(utcNow.Month), Convert.ToInt16(utcNow.Day), utcNow.TimeOfDay.TotalHours);
                string dtString = utcNow.ToString("dddd dd MMMM yyyy HH:mm:ss.fff");
                TL.LogMessage("JulianDateUtc", $"Returning: {num} at UTC: {dtString}");
                return num;
            }
        }


        public AstroUtils() : this(36, null,null)
        {

        }

        public AstroUtils(int leapSeconds, IAscomDataStore store, ITraceLogger logger)
        {
            Utl = new Util();
            Nov31 = new NOVAS31();
            Store=store??(IAscomDataStore)new EntityStore();
            TL = logger ?? new TraceLogger("", "Astrometry");
            TL.Enabled = Store?.GetProfile<bool>("Utilities","Trace Astro Utils", false)??false;
            UtcTaiOffset = Store?.GetProfile("Astrometry", "Leap Seconds", 36)??36;
            TL.LogMessage("New", "AstroUtils created Utilities component OK");
            TL.LogMessage("New", $"Leap seconds: {UtcTaiOffset}");
            TL.LogMessage("New", "Finished initialisation OK");
        }
        public int LeapSeconds
        {
            get
            {
                return UtcTaiOffset?? Store?.GetProfile("Astrometry", "Leap Seconds",36)??36; ;
            }
            set
            {
                UtcTaiOffset = value;
                Store?.WriteProfile("Astrometry", "Leap Seconds", value);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (TL != null)
                    {
                        TL.Enabled = false;
                        TL.Dispose();
                        TL = null;
                    }
                    if (Utl != null)
                    {
                        Utl.Dispose();
                        Utl = (Util)null;
                    }
                }
                if (Nov31 != null)
                {
                    Nov31.Dispose();
                    Nov31 = (NOVAS31)null;
                }
            }
            disposedValue = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize((object)this);
        }

        public double Range(double Value, double LowerBound, bool LowerEqual, double UpperBound, bool UpperEqual)
        {
            if (LowerBound >= UpperBound)
                throw new InvalidValueException("Range", "LowerBound is >= UpperBound", "LowerBound must be less than UpperBound");
            double num = UpperBound - LowerBound;
            if (LowerEqual)
            {
                if (UpperEqual)
                {
                    do
                    {
                        if (Value < LowerBound)
                            Value += num;
                        if (Value > UpperBound)
                            Value -= num;
                    }
                    while (!(Value >= LowerBound & Value <= UpperBound));
                }
                else
                {
                    do
                    {
                        if (Value < LowerBound)
                            Value += num;
                        if (Value >= UpperBound)
                            Value -= num;
                    }
                    while (!(Value >= LowerBound & Value < UpperBound));
                }
            }
            else if (UpperEqual)
            {
                do
                {
                    if (Value <= LowerBound)
                        Value += num;
                    if (Value > UpperBound)
                        Value -= num;
                }
                while (!(Value > LowerBound & Value <= UpperBound));
            }
            else
            {
                if (Value == LowerBound)
                    throw new InvalidValueException("Range", "The supplied value equals the LowerBound. This can not be ranged when LowerEqual and UpperEqual are both false ", "LowerBound > Value < UpperBound");
                if (Value == UpperBound)
                    throw new InvalidValueException("Range", "The supplied value equals the UpperBound. This can not be ranged when LowerEqual and UpperEqual are both false ", "LowerBound > Value < UpperBound");
                do
                {
                    if (Value <= LowerBound)
                        Value += num;
                    if (Value >= UpperBound)
                        Value -= num;
                }
                while (!(Value > LowerBound & Value < UpperBound));
            }
            return Value;
        }

        public double ConditionHA(double HA)
        {
            double Hours = Range(HA, -12.0, true, 12.0, true);
            TL.LogMessage("ConditionHA", "Conditioned HA: " + Utl.HoursToHMS(HA, ":", ":", "", 3) + " to: " + Utl.HoursToHMS(Hours, ":", ":", "", 3));
            return Hours;
        }

        public double ConditionRA(double RA)
        {
            double Hours = Range(RA, 0.0, true, 24.0, false);
            TL.LogMessage("ConditionRA", "Conditioned RA: " + Utl.HoursToHMS(RA, ":", ":", "", 3) + " to: " + Utl.HoursToHMS(Hours, ":", ":", "", 3));
            return Hours;
        }

        public double DeltaT()
        {
            double julianDate = Utl.JulianDate;
            double num = DeltatCode.DeltaTCalc(julianDate);
            TL.LogMessage("DeltaT", "Returned: " + Conversions.ToString(num) + " at Julian date: " + Conversions.ToString(julianDate));
            return num;
        }

        public double JulianDateTT(double DeltaUT1)
        {
            if (DeltaUT1 < -0.9 | DeltaUT1 > 0.9)
                throw new InvalidValueException("JulianDateUT1", DeltaUT1.ToString(), "-0.9 to +0.9");
            DateTime utcNow = DateTime.UtcNow;
            DateTime dateTime;
            if (DeltaUT1 != 0.0)
            {
                TimeSpan timeSpan1 = TimeSpan.FromSeconds(DeltaT());
                TimeSpan timeSpan2 = TimeSpan.FromSeconds(DeltaUT1);
                dateTime = utcNow.Add(timeSpan2).Add(timeSpan1);
            }
            else
                dateTime = utcNow.Add(TimeSpan.FromSeconds((double)UtcTaiOffset + 4023.0 / 125.0));
            double num = Nov31.JulianDate(Convert.ToInt16(dateTime.Year), Convert.ToInt16(dateTime.Month), Convert.ToInt16(dateTime.Day), dateTime.TimeOfDay.TotalHours);
            TL.LogMessage("JulianDateTT", "Returning: " + Conversions.ToString(num) + "at TT: " + Strings.Format((object)dateTime, "dddd dd MMMM yyyy HH:mm:ss.fff") + ", at UTC: " + Strings.Format((object)utcNow, "dddd dd MMMM yyyy HH:mm:ss.fff"));
            return num;
        }

        public double JulianDateUT1(double DeltaUT1)
        {
            if (DeltaUT1 < -0.9 | DeltaUT1 > 0.9)
                throw new InvalidValueException("JulianDateUT1", DeltaUT1.ToString(), "-0.9 to +0.9");
            DateTime utcNow = DateTime.UtcNow;
            DateTime dateTime;
            if (DeltaUT1 != 0.0)
            {
                TimeSpan timeSpan = TimeSpan.FromSeconds(DeltaUT1);
                dateTime = utcNow.Add(timeSpan);
            }
            else
            {
                double num = DeltatCode.DeltaTCalc(Nov31.JulianDate(Convert.ToInt16(utcNow.Year), Convert.ToInt16(utcNow.Month), Convert.ToInt16(utcNow.Day), utcNow.TimeOfDay.TotalHours));
                dateTime = utcNow.Add(TimeSpan.FromSeconds((double)UtcTaiOffset + 4023.0 / 125.0)).Subtract(TimeSpan.FromSeconds(num));
            }
            double num1 = Nov31.JulianDate(Convert.ToInt16(dateTime.Year), Convert.ToInt16(dateTime.Month), Convert.ToInt16(dateTime.Day), dateTime.TimeOfDay.TotalHours);
            TL.LogMessage("JulianDateUT1", "Returning: " + Conversions.ToString(num1) + "at UT1: " + Strings.Format((object)dateTime, "dddd dd MMMM yyyy HH:mm:ss.fff") + ", at UTC: " + Strings.Format((object)utcNow, "dddd dd MMMM yyyy HH:mm:ss.fff"));
            return num1;
        }

        public double UnRefract(OnSurface Location, RefractionOption RefOption, double ZdObs)
        {
            if (ZdObs < 0.0 | ZdObs > 90.0)
                throw new InvalidValueException("Unrefract", "Zenith distance", "0.0 to 90.0 degrees");
            int num1 = 0;
            double ZdObs1 = ZdObs;
            double num2;
            do
            {
                checked { ++num1; }
                num2 = ZdObs1 - Nov31.Refract(Location, RefOption, ZdObs1);
                ZdObs1 += ZdObs - num2;
                TL.LogMessage("Unrefract", Conversions.ToString(num1) + ": " + Conversions.ToString(num2) + " " + Conversions.ToString(ZdObs1));
            }
            while (!(num1 == 20 | num2 == ZdObs));
            TL.LogMessage("Unrefract", "Final: " + Conversions.ToString(num1) + ", Unrefracted zenith distance: " + Conversions.ToString(ZdObs1));
            return ZdObs1;
        }

        public double CalendarToMJD(int Day, int Month, int Year)
        {
            return Nov31.JulianDate(Convert.ToInt16(Year), Convert.ToInt16(Month), Convert.ToInt16(Day), 0.0) - 2400000.5;
        }

        public double MJDToOADate(double MJD)
        {
            return Utl.DateJulianToLocal(MJD + 2400000.5).ToOADate();
        }

        public DateTime MJDToDate(double MJD)
        {
            return Utl.DateJulianToLocal(MJD + 2400000.5);
        }

        public string FormatMJD(double MJD, string PresentationFormat)
        {
            return Strings.Format((object)MJDToDate(MJD), PresentationFormat);
        }

        public double DeltaUT(double JulianDate)
        {
            double num = (double)UtcTaiOffset + 4023.0 / 125.0 - DeltatCode.DeltaTCalc(JulianDate);
            TL.LogMessage("DeltaUT", "Returning: " + Conversions.ToString(num) + " at Julian date: " + Conversions.ToString(JulianDate));
            return num;
        }

        public string FormatJD(double JD, string PresentationFormat)
        {
            TL.LogMessage("FormatJD", "JD, PresentationFormat: " + Conversions.ToString(JD) + " " + PresentationFormat);
            double num = JD - 2451545.0;
            DateTime dateTime1 = new DateTime(2000, 1, 1, 12, 0, 0);
            DateTime dateTime2 = dateTime1.AddDays(num);
            TL.LogMessage("FormatJD", "  DaysSinceJ2000, J2000Date, ActualDate: " + Conversions.ToString(num) + " " + dateTime1.ToString() + " " + dateTime2.ToString());
            string str = Strings.Format((object)dateTime2, PresentationFormat);
            TL.LogMessage("FormatJD", "  Result: " + str);
            return str;
        }

        public ArrayList EventTimes(EventType TypeofEvent, int Day, int Month, int Year, double SiteLatitude, double SiteLongitude, double SiteTimeZone)
        {
            OnSurface Location = new OnSurface();
            ArrayList arrayList = new ArrayList();
            List<double> doubleList1 = new List<double>();
            List<double> doubleList2 = new List<double>();
            bool flag1 = false;
            bool flag2 = false;
            try
            {
                DateTime.Parse(Conversions.ToString(Month) + "/" + Conversions.ToString(Day) + "/" + Conversions.ToString(Year), (IFormatProvider)CultureInfo.InvariantCulture);
            }
            catch (FormatException ex)
            {
                //ProjectData.SetProjectError((Exception) ex);
                throw new InvalidValueException("Day or Month", Day.ToString() + " " + Month.ToString() + " " + Year.ToString(), "Day must not exceed the number of days in the month");
            }
            catch (Exception ex)
            {
                //ProjectData.SetProjectError(ex);
                TL.LogMessageCrLf("EventTimes", ex.ToString());
                throw;
            }
            double JD = Nov31.JulianDate(checked((short)Year), checked((short)Month), checked((short)Day), 0.0) - SiteTimeZone / 24.0;
            Location.Latitude = SiteLatitude;
            Location.Longitude = SiteLongitude;
            double num1 = Nov31.Refract(Location, RefractionOption.StandardRefraction, 90.0);
            double Hour = 1.0;
            bool flag3 = false;
            do
            {
                ASCOM.Astrometry.AstroUtils.AstroUtils.BodyInfo bodyInfo1 = BodyAltitude(TypeofEvent, JD, Hour - 1.0, SiteLatitude, SiteLongitude);
                ASCOM.Astrometry.AstroUtils.AstroUtils.BodyInfo bodyInfo2 = BodyAltitude(TypeofEvent, JD, Hour, SiteLatitude, SiteLongitude);
                ASCOM.Astrometry.AstroUtils.AstroUtils.BodyInfo bodyInfo3 = BodyAltitude(TypeofEvent, JD, Hour + 1.0, SiteLatitude, SiteLongitude);
                double num2;
                double num3;
                double num4;
                switch (TypeofEvent)
                {
                    case EventType.SunRiseSunset:
                        num2 = bodyInfo1.Altitude - -5.0 / 6.0;
                        num3 = bodyInfo2.Altitude - -5.0 / 6.0;
                        num4 = bodyInfo3.Altitude - -5.0 / 6.0;
                        break;
                    case EventType.MoonRiseMoonSet:
                        num2 = bodyInfo1.Altitude - 365432.481734439 / bodyInfo1.Distance + bodyInfo1.Radius * (180.0 / Math.PI) / bodyInfo1.Distance + num1;
                        num3 = bodyInfo2.Altitude - 365432.481734439 / bodyInfo2.Distance + bodyInfo2.Radius * (180.0 / Math.PI) / bodyInfo2.Distance + num1;
                        num4 = bodyInfo3.Altitude - 365432.481734439 / bodyInfo3.Distance + bodyInfo3.Radius * (180.0 / Math.PI) / bodyInfo3.Distance + num1;
                        break;
                    case EventType.CivilTwilight:
                        num2 = bodyInfo1.Altitude - -6.0;
                        num3 = bodyInfo2.Altitude - -6.0;
                        num4 = bodyInfo3.Altitude - -6.0;
                        break;
                    case EventType.NauticalTwilight:
                        num2 = bodyInfo1.Altitude - -12.0;
                        num3 = bodyInfo2.Altitude - -12.0;
                        num4 = bodyInfo3.Altitude - -12.0;
                        break;
                    case EventType.AmateurAstronomicalTwilight:
                        num2 = bodyInfo1.Altitude - -15.0;
                        num3 = bodyInfo2.Altitude - -15.0;
                        num4 = bodyInfo3.Altitude - -15.0;
                        break;
                    case EventType.AstronomicalTwilight:
                        num2 = bodyInfo1.Altitude - -18.0;
                        num3 = bodyInfo2.Altitude - -18.0;
                        num4 = bodyInfo3.Altitude - -18.0;
                        break;
                    default:
                        num2 = bodyInfo1.Altitude + num1 + 180.0 / Math.PI * bodyInfo2.Radius / bodyInfo2.Distance;
                        num3 = bodyInfo2.Altitude + num1 + 180.0 / Math.PI * bodyInfo2.Radius / bodyInfo2.Distance;
                        num4 = bodyInfo3.Altitude + num1 + 180.0 / Math.PI * bodyInfo2.Radius / bodyInfo2.Distance;
                        break;
                }
                if (Hour == 1.0)
                    flag3 = num2 >= 0.0;
                double num5 = num3;
                double num6 = 0.5 * (num4 - num2);
                double num7 = 0.5 * (num4 + num2) - num3;
                double num8 = -num6 / (2.0 * num7);
                double num9 = (num7 * num8 + num6) * num8 + num5;
                double d = num6 * num6 - 4.0 * num7 * num5;
                double num10 = double.NaN;
                double num11 = double.NaN;
                int num12 = 0;
                if (d > 0.0)
                {
                    double num13 = 0.5 * Math.Sqrt(d) / Math.Abs(num7);
                    num10 = num8 - num13;
                    num11 = num8 + num13;
                    if (Math.Abs(num10) <= 1.0)
                        checked { ++num12; }
                    if (Math.Abs(num11) <= 1.0)
                        checked { ++num12; }
                    if (num10 < -1.0)
                        num10 = num11;
                }
                switch (num12)
                {
                    case 1:
                        if (num2 < 0.0)
                        {
                            flag1 = true;
                            doubleList1.Add(Hour + num10);
                            break;
                        }
                        flag2 = true;
                        doubleList2.Add(Hour + num10);
                        break;
                    case 2:
                        if (num2 < 0.0)
                        {
                            doubleList1.Add(Hour + num10);
                            doubleList2.Add(Hour + num11);
                        }
                        else
                        {
                            doubleList1.Add(Hour + num11);
                            doubleList2.Add(Hour + num10);
                        }
                        flag1 = true;
                        flag2 = true;
                        break;
                }
                Hour += 2.0;
            }
            while (!(flag1 & flag2 & Math.Abs(SiteLatitude) < 60.0 | Hour == 25.0));
            arrayList.Add((object)flag3);
            arrayList.Add((object)doubleList1.Count);
            arrayList.Add((object)doubleList2.Count);
            List<double>.Enumerator enumerator1 = default(List<double>.Enumerator);
            try
            {
                enumerator1 = doubleList1.GetEnumerator();
                while (enumerator1.MoveNext())
                {
                    double current = enumerator1.Current;
                    arrayList.Add((object)current);
                }
            }
            finally
            {
                enumerator1.Dispose();
            }
            List<double>.Enumerator enumerator2 = default(List<double>.Enumerator);
            try
            {
                enumerator2 = doubleList2.GetEnumerator();
                while (enumerator2.MoveNext())
                {
                    double current = enumerator2.Current;
                    arrayList.Add((object)current);
                }
            }
            finally
            {
                enumerator2.Dispose();
            }
            return arrayList;
        }

        private ASCOM.Astrometry.AstroUtils.AstroUtils.BodyInfo BodyAltitude(EventType TypeOfEvent, double JD, double Hour, double Latitude, double Longitude)
        {
            Object3 CelObject = new Object3();
            OnSurface onSurface = new OnSurface();
            CatEntry3 catEntry3 = new CatEntry3();
            SkyPos Output = new SkyPos();
            Observer Location = new Observer();
            ASCOM.Astrometry.AstroUtils.AstroUtils.BodyInfo bodyInfo = new ASCOM.Astrometry.AstroUtils.AstroUtils.BodyInfo();
            double JdHigh = JD + Hour / 24.0;
            double DeltaT = DeltatCode.DeltaTCalc(JD);
            switch (TypeOfEvent)
            {
                case EventType.SunRiseSunset:
                case EventType.CivilTwilight:
                case EventType.NauticalTwilight:
                case EventType.AmateurAstronomicalTwilight:
                case EventType.AstronomicalTwilight:
                    CelObject.Name = "Sun";
                    CelObject.Number = Body.Sun;
                    break;
                case EventType.MoonRiseMoonSet:
                    CelObject.Name = "Moon";
                    CelObject.Number = Body.Moon;
                    break;
                case EventType.MercuryRiseSet:
                    CelObject.Name = "Mercury";
                    CelObject.Number = Body.Mercury;
                    break;
                case EventType.VenusRiseSet:
                    CelObject.Name = "Venus";
                    CelObject.Number = Body.Venus;
                    break;
                case EventType.MarsRiseSet:
                    CelObject.Name = "Mars";
                    CelObject.Number = Body.Mars;
                    break;
                case EventType.JupiterRiseSet:
                    CelObject.Name = "Jupiter";
                    CelObject.Number = Body.Jupiter;
                    break;
                case EventType.SaturnRiseSet:
                    CelObject.Name = "Saturn";
                    CelObject.Number = Body.Saturn;
                    break;
                case EventType.UranusRiseSet:
                    CelObject.Name = "Uranus";
                    CelObject.Number = Body.Uranus;
                    break;
                case EventType.NeptuneRiseSet:
                    CelObject.Name = "Neptune";
                    CelObject.Number = Body.Neptune;
                    break;
                case EventType.PlutoRiseSet:
                    CelObject.Name = "Pluto";
                    CelObject.Number = Body.Pluto;
                    break;
                default:
                    throw new InvalidValueException("TypeOfEvent", TypeOfEvent.ToString(), "Unknown type of event");
            }
            CelObject.Star = catEntry3;
            CelObject.Type = ASCOM.Astrometry.ObjectType.MajorPlanetSunOrMoon;
            Location.OnSurf = onSurface;
            Location.Where = ObserverLocation.EarthGeoCenter;
            int num1 = (int)Nov31.Place(JdHigh + DeltaT * 1.15740740740741E-05, CelObject, Location, DeltaT, CoordSys.EquinoxOfDate, Accuracy.Full, ref Output);
            bodyInfo.Distance = Output.Dis * 149597870.691;
            double Gst = 0;
            Nov31.SiderealTime(JdHigh, 0.0, DeltaT, GstType.GreenwichApparentSiderealTime, ASCOM.Astrometry.Method.EquinoxBased, Accuracy.Full, ref Gst);
            double num2 = 15.0 * (Range(Gst + Longitude * (1.0 / 15.0), 0.0, true, 24.0, false) - Output.RA);
            bodyInfo.Altitude = Math.Asin(Math.Sin(Latitude * (Math.PI / 180.0)) * Math.Sin(Output.Dec * (Math.PI / 180.0)) + Math.Cos(Latitude * (Math.PI / 180.0)) * Math.Cos(Output.Dec * (Math.PI / 180.0)) * Math.Cos(num2 * (Math.PI / 180.0))) * (180.0 / Math.PI);
            switch (TypeOfEvent)
            {
                case EventType.MoonRiseMoonSet:
                    bodyInfo.Radius = 1737.0;
                    break;
                case EventType.MercuryRiseSet:
                    bodyInfo.Radius = 2439.7;
                    break;
                case EventType.VenusRiseSet:
                    bodyInfo.Radius = 2439.7;
                    break;
                case EventType.MarsRiseSet:
                    bodyInfo.Radius = 3396.2;
                    break;
                case EventType.JupiterRiseSet:
                    bodyInfo.Radius = 69911.0;
                    break;
                case EventType.SaturnRiseSet:
                    bodyInfo.Radius = 6051.8;
                    break;
                case EventType.UranusRiseSet:
                    bodyInfo.Radius = 24973.0;
                    break;
                case EventType.NeptuneRiseSet:
                    bodyInfo.Radius = 24767.0;
                    break;
                case EventType.PlutoRiseSet:
                    bodyInfo.Radius = 1153.0;
                    break;
                default:
                    bodyInfo.Radius = 696342.0;
                    break;
            }
            return bodyInfo;
        }

        public double MoonIllumination(double JD)
        {
            Object3 CelObject = new Object3();
            OnSurface onSurface = new OnSurface();
            CatEntry3 catEntry3 = new CatEntry3();
            SkyPos Output1 = new SkyPos();
            SkyPos Output2 = new SkyPos();
            Observer Location = new Observer();
            double DeltaT = DeltatCode.DeltaTCalc(JD);
            CelObject.Name = "Moon";
            CelObject.Number = Body.Moon;
            CelObject.Star = catEntry3;
            CelObject.Type = ASCOM.Astrometry.ObjectType.MajorPlanetSunOrMoon;
            Location.OnSurf = onSurface;
            Location.Where = ObserverLocation.EarthGeoCenter;
            int num1 = (int)Nov31.Place(JD + DeltaT * 1.15740740740741E-05, CelObject, Location, DeltaT, CoordSys.EquinoxOfDate, Accuracy.Full, ref Output2);
            CelObject.Name = "Sun";
            CelObject.Number = Body.Sun;
            CelObject.Star = catEntry3;
            CelObject.Type = ASCOM.Astrometry.ObjectType.MajorPlanetSunOrMoon;
            int num2 = (int)Nov31.Place(JD + DeltaT * 1.15740740740741E-05, CelObject, Location, DeltaT, CoordSys.EquinoxOfDate, Accuracy.Full, ref Output1);
            double num3 = Math.Acos(Math.Sin(Output1.Dec * (Math.PI / 180.0)) * Math.Sin(Output2.Dec * (Math.PI / 180.0)) + Math.Cos(Output1.Dec * (Math.PI / 180.0)) * Math.Cos(Output2.Dec * (Math.PI / 180.0)) * Math.Cos((Output1.RA - Output2.RA) * 15.0 * (Math.PI / 180.0)));
            return (1.0 + Math.Cos(Math.Atan2(Output1.Dis * Math.Sin(num3), Output2.Dis - Output1.Dis * Math.Cos(num3)))) / 2.0;
        }

        public double MoonPhase(double JD)
        {
            Object3 CelObject = new Object3();
            OnSurface onSurface = new OnSurface();
            CatEntry3 catEntry3 = new CatEntry3();
            SkyPos Output1 = new SkyPos();
            SkyPos Output2 = new SkyPos();
            Observer Location = new Observer();
            double DeltaT = DeltatCode.DeltaTCalc(JD);
            CelObject.Name = "Moon";
            CelObject.Number = Body.Moon;
            CelObject.Star = catEntry3;
            CelObject.Type = ASCOM.Astrometry.ObjectType.MajorPlanetSunOrMoon;
            Location.OnSurf = onSurface;
            Location.Where = ObserverLocation.EarthGeoCenter;
            int num1 = (int)Nov31.Place(JD + DeltaT * 1.15740740740741E-05, CelObject, Location, DeltaT, CoordSys.EquinoxOfDate, Accuracy.Full, ref Output2);
            CelObject.Name = "Sun";
            CelObject.Number = Body.Sun;
            CelObject.Star = catEntry3;
            CelObject.Type = ASCOM.Astrometry.ObjectType.MajorPlanetSunOrMoon;
            int num2 = (int)Nov31.Place(JD + DeltaT * 1.15740740740741E-05, CelObject, Location, DeltaT, CoordSys.EquinoxOfDate, Accuracy.Full, ref Output1);
            return Range((Output2.RA - Output1.RA) * 15.0, -180.0, false, 180.0, true);
        }

        internal struct BodyInfo
        {
            public double Altitude;
            public double Distance;
            public double Radius;
        }
    }
}