// Decompiled with JetBrains decompiler
// Type: ASCOM.Utilities.EventLogCode
// Assembly: ASCOM.Utilities, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 06F2ED6E-C559-496D-B7FF-DCA800F2230D
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.Utilities\6.0.0.0__565de7938946fba7\ASCOM.Utilities.dll

using System;

namespace ASCOM.Utilities
{
    internal class DateAndTime
    {
        public static double Timer
        {
            get
            {
                throw new NotImplementedException();
                //return Microsoft.VisualBasic.DateAndTime.Timer; 
            }
        }
        public static string TimeString
        {
            get
            {
                throw new NotImplementedException();
                //return Microsoft.VisualBasic.DateAndTime.TimeString; 
            }
            set
            {
                throw new NotImplementedException();
                //Microsoft.VisualBasic.DateAndTime.TimeString = value;
            }
        }
        public static DateTime TimeOfDay
        {
            get
            {
                throw new NotImplementedException();
                //return Microsoft.VisualBasic.DateAndTime.TimeOfDay; 
            }
            set
            {
                throw new NotImplementedException();
                //Microsoft.VisualBasic.DateAndTime.TimeOfDay = value;
            }
        }

        public static DateTime Now
        {
            get { return DateTime.Now; }
        }

        public static DateTime Today
        {
            get
            {
                throw new NotImplementedException();
                //return Microsoft.VisualBasic.DateAndTime.Today; 
            }
            set
            {
                throw new NotImplementedException();
                //Microsoft.VisualBasic.DateAndTime.Today = value;
            }
        }

        public static string DateString
        {
            get
            {
                throw new NotImplementedException();
                //return Microsoft.VisualBasic.DateAndTime.DateString; 
            }
            set
            {
                throw new NotImplementedException();
                //Microsoft.VisualBasic.DateAndTime.DateString = value; 
            }
        }

        public static DateTime DateAdd(DateInterval Interval, double Number, DateTime DateValue)
        {
            throw new NotImplementedException();
            //return Microsoft.VisualBasic.DateAndTime.DateAdd((Microsoft.VisualBasic.DateInterval)Interval,Number,DateValue);
        }

        public static DateTime DateAdd(string Interval, double Number, object DateValue)
        {
            throw new NotImplementedException();
            //return Microsoft.VisualBasic.DateAndTime.DateAdd(Interval, Number, DateValue);
        }

        public static long DateDiff(DateInterval Interval, DateTime Date1, DateTime Date2, FirstDayOfWeek DayOfWeek = FirstDayOfWeek.Sunday, FirstWeekOfYear WeekOfYear = FirstWeekOfYear.Jan1)
        {
            throw new NotImplementedException();
            //return Microsoft.VisualBasic.DateAndTime.DateDiff((Microsoft.VisualBasic.DateInterval)Interval,Date1,Date2,(Microsoft.VisualBasic.FirstDayOfWeek)DayOfWeek, (Microsoft.VisualBasic.FirstWeekOfYear) WeekOfYear);
        }

        public static long DateDiff(string Interval, object Date1, object Date2, FirstDayOfWeek DayOfWeek = FirstDayOfWeek.Sunday, FirstWeekOfYear WeekOfYear = FirstWeekOfYear.Jan1)
        {
            throw new NotImplementedException();
            //return Microsoft.VisualBasic.DateAndTime.DateDiff(Interval, Date1, Date2, (Microsoft.VisualBasic.FirstDayOfWeek)DayOfWeek, (Microsoft.VisualBasic.FirstWeekOfYear)WeekOfYear);
        }

        public static int DatePart(DateInterval Interval, DateTime DateValue, FirstDayOfWeek FirstDayOfWeekValue = FirstDayOfWeek.Sunday, FirstWeekOfYear FirstWeekOfYearValue = FirstWeekOfYear.Jan1)
        {
            throw new NotImplementedException();
            //return Microsoft.VisualBasic.DateAndTime.DatePart((Microsoft.VisualBasic.DateInterval) Interval, DateValue, (Microsoft.VisualBasic.FirstDayOfWeek) FirstDayOfWeekValue, (Microsoft.VisualBasic.FirstWeekOfYear) FirstWeekOfYearValue);
        }

        public static int DatePart(string Interval, object DateValue, FirstDayOfWeek DayOfWeek = FirstDayOfWeek.Sunday, FirstWeekOfYear WeekOfYear = FirstWeekOfYear.Jan1)
        {
            throw new NotImplementedException();
            //return Microsoft.VisualBasic.DateAndTime.DatePart(Interval,DateValue,(Microsoft.VisualBasic.FirstDayOfWeek) DayOfWeek, (Microsoft.VisualBasic.FirstWeekOfYear)WeekOfYear);
        }

        public static DateTime DateSerial(int Year, int Month, int Day)
        {
            throw new NotImplementedException();
            //return Microsoft.VisualBasic.DateAndTime.DateSerial(Year,Month,Day);
        }

        public static DateTime DateValue(string StringDate)
        {
            throw new NotImplementedException();
            //return Microsoft.VisualBasic.DateAndTime.DateValue(StringDate);
        }

        public static int Day(DateTime DateValue)
        {
            throw new NotImplementedException();
            //return Microsoft.VisualBasic.DateAndTime.Day(DateValue);
        }

        public static int Hour(DateTime TimeValue)
        {
            throw new NotImplementedException();
            //return Microsoft.VisualBasic.DateAndTime.Hour(TimeValue);
        }

        public static int Minute(DateTime TimeValue)
        {
            throw new NotImplementedException();
            //return Microsoft.VisualBasic.DateAndTime.Minute(TimeValue);
        }

        public static int Month(DateTime DateValue)
        {
            throw new NotImplementedException();
            //return Microsoft.VisualBasic.DateAndTime.Month(DateValue);
        }

        public static string MonthName(int Month, bool Abbreviate = false)
        {
            throw new NotImplementedException();
            //return Microsoft.VisualBasic.DateAndTime.MonthName(Month,Abbreviate);
        }

        public static int Second(DateTime TimeValue)
        {
            throw new NotImplementedException();
            //return Microsoft.VisualBasic.DateAndTime.Second(TimeValue);
        }

        public static DateTime TimeSerial(int Hour, int Minute, int Second)
        {
            throw new NotImplementedException();
            //return Microsoft.VisualBasic.DateAndTime.TimeSerial(Hour,Minute,Second);
        }

        public static DateTime TimeValue(string StringTime)
        {
            throw new NotImplementedException();
            //return Microsoft.VisualBasic.DateAndTime.TimeValue(StringTime);
        }

        public static int Weekday(DateTime DateValue, FirstDayOfWeek DayOfWeek = FirstDayOfWeek.Sunday)
        {
            throw new NotImplementedException();
            //return Microsoft.VisualBasic.DateAndTime.Weekday(DateValue,(Microsoft.VisualBasic.FirstDayOfWeek)DayOfWeek);
        }

        public static string WeekdayName(int Weekday, bool Abbreviate = false, FirstDayOfWeek FirstDayOfWeekValue = FirstDayOfWeek.System)
        {
            throw new NotImplementedException();
            //return Microsoft.VisualBasic.DateAndTime.WeekdayName(Weekday, Abbreviate, (Microsoft.VisualBasic.FirstDayOfWeek)FirstDayOfWeekValue);
        }

        public static int Year(DateTime DateValue)
        {
            return DateValue.Year;
        }


    }
}