using System;
using System.Collections.Generic;
using System.Text;

namespace ASCOM.Utilities
{
#warning Internal enum exposed as public during porting: EventLogEntryType
    public enum EventLogEntryType
    {
        Unspecified = 0,
        Information,
        Warning,
        Error,
        SuccessAudit,
        FailureAudit
    }
    public enum DateInterval
    {
        Year = 0,
        Quarter = 1,
        Month = 2,
        DayOfYear = 3,
        Day = 4,
        WeekOfYear = 5,
        Weekday = 6,
        Hour = 7,
        Minute = 8,
        Second = 9
    }
    public enum FirstWeekOfYear
    {
        System = 0,
        Jan1 = 1,
        FirstFourDays = 2,
        FirstFullWeek = 3
    }
    public enum FirstDayOfWeek
    {
        System = 0,
        Sunday = 1,
        Monday = 2,
        Tuesday = 3,
        Wednesday = 4,
        Thursday = 5,
        Friday = 6,
        Saturday = 7
    }
    public enum CallType
    {
        Method = 1,
        Get = 2,
        Let = 4,
        Set = 8
    }
    [Flags]
    public enum MsgBoxStyle
    {
        OkOnly = 0,
        DefaultButton1 = 0,
        ApplicationModal = 0,
        OkCancel = 1,
        AbortRetryIgnore = 2,
        YesNoCancel = 3,
        YesNo = 4,
        RetryCancel = 5,
        Critical = 16,
        Question = 32,
        Exclamation = 48,
        Information = 64,
        DefaultButton2 = 256,
        DefaultButton3 = 512,
        SystemModal = 4096,
        MsgBoxHelp = 16384,
        MsgBoxSetForeground = 65536,
        MsgBoxRight = 524288,
        MsgBoxRtlReading = 1048576
    }

    [Flags]
    public enum VbStrConv
    {
        None = 0,
        Uppercase = 1,
        Lowercase = 2,
        ProperCase = 3,
        Wide = 4,
        Narrow = 8,
        Katakana = 16,
        Hiragana = 32,
        SimplifiedChinese = 256,
        TraditionalChinese = 512,
        LinguisticCasing = 1024
    }
    public enum TriState
    {
        UseDefault = -2,
        True = -1,
        False = 0
    }

    public enum DateFormat
    {
        GeneralDate = 0,
        LongDate = 1,
        ShortDate = 2,
        LongTime = 3,
        ShortTime = 4
    }
    public enum CompareMethod
    {
        Binary = 0,
        Text = 1
    }
    public enum MsgBoxResult
    {
        Ok = 1,
        Cancel = 2,
        Abort = 3,
        Retry = 4,
        Ignore = 5,
        Yes = 6,
        No = 7
    }
}
