using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace ASCOM
{
    public enum ErrorCode
    {
        //TODO: Research more appropriate values
        NoError = 0,
        UnspecifiedError = 1,
        NotImplemented = 2,
        InvalidValue = 3,
        ValueNotSet = 4,
        NotConnected = 5,
        InvalidWhileParked = 6,
        InvalidWhileSlaved = 7,
        SettingsProviderError = 8,
        InvalidOperationException = 9,
        ActionNotImplementedException = 10,

        DriverBase = 2 ^ 8,
        DriverMax = 2 ^ 9 - 1
    }

    /// <summary>
    /// The range of permitted values falls within the class FACILTY_ITF as defined 
    /// by the operating system and COM. These values will never clash with COM, RPC, 
    /// or OS error codes.
    /// 
    /// Driver developers may extend this class by making use of the partial keyword.
    /// </summary>
    [Obsolete("The ErrorCodes class should be replaced with the enum ErrorCode",false)]
    public static class ErrorCodes
    {
        //TODO: Research more appropriate values
        public static readonly int UnspecifiedError = 1;
        public static readonly int NotImplemented = 1;
        public static readonly int InvalidValue = 2;
        public static readonly int ValueNotSet = 3;
        public static readonly int NotConnected = 4;
        public static readonly int InvalidWhileParked = 5;
        public static readonly int InvalidWhileSlaved = 6;
        public static readonly int SettingsProviderError = 7;
        public static readonly int InvalidOperationException = 8;
        public static readonly int ActionNotImplementedException = 9;

        public static readonly int DriverBase = 2 ^ 8;
        public static readonly int DriverMax = 2 ^ 9 - 1;
    }

}