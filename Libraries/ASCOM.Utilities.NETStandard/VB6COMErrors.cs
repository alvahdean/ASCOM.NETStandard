// Decompiled with JetBrains decompiler
// Type: ASCOM.Utilities.VB6COMErrors
// Assembly: ASCOM.Utilities, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 06F2ED6E-C559-496D-B7FF-DCA800F2230D
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.Utilities\6.0.0.0__565de7938946fba7\ASCOM.Utilities.dll

//using Microsoft.VisualBasic.CompilerServices;
using System.Diagnostics;

namespace ASCOM.Utilities
{
  //[StandardModule]
  internal sealed class VB6COMErrors
  {
    internal const string ERR_SOURCE_SERIAL = "ASCOM Helper Serial Port Object";
    internal const int SCODE_UNSUP_SPEED = -2147220480;
    internal const string MSG_UNSUP_SPEED = "Unsupported port speed. Use the PortSpeed enumeration.";
    internal const int SCODE_INVALID_TIMEOUT = -2147220479;
    internal const string MSG_INVALID_TIMEOUT = "Timeout must be 1 - 120 seconds.";
    internal const int SCODE_RECEIVE_TIMEOUT = -2147220478;
    internal const string MSG_RECEIVE_TIMEOUT = "Timed out waiting for received data.";
    internal const int SCODE_EMPTY_TERM = -2147220477;
    internal const string MSG_EMPTY_TERM = "Terminator string must have at least one character.";
    internal const int SCODE_ILLEGAL_COUNT = -2147220476;
    internal const string MSG_ILLEGAL_COUNT = "Character count must be positive and greater than 0.";
    internal const int SCODE_TRACE_ERR = -2147220475;
    internal const string MSG_TRACE_ERR = "Serial Trace file: ";
    internal const string ERR_SOURCE_CHOOSER = "ASCOM Helper Device Chooser Object";
    internal const string ERR_SOURCE_PROFILE = "ASCOM Helper Registry Profile Object";
    internal const int SCODE_DRIVER_NOT_REG = -2147220448;
    internal const int SCODE_ILLEGAL_DRIVERID = -2147220447;
    internal const string MSG_ILLEGAL_DRIVERID = "Illegal DriverID value \"\" (empty string)";
    internal const int SCODE_ILLEGAL_REGACC = -2147220446;
    internal const string MSG_ILLEGAL_REGACC = "Illegal access to registry area";
    internal const int SCODE_ILLEGAL_DEVTYPE = -2147220445;
    internal const string MSG_ILLEGAL_DEVTYPE = "Illegal DeviceType value \"\" (empty string)";
    internal const string ERR_SOURCE_UTIL = "ASCOM Helper Utilities Object";
    internal const int SCODE_DLL_LOADFAIL = -2147220432;
    internal const int SCODE_TIMER_FAIL = -2147220431;
    internal const string MSG_TIMER_FAIL = "Hi-res timer failed. Delay out of range?";
    internal const int SCODE_REGERR = -2147220416;

    [DebuggerNonUserCode]
    static VB6COMErrors()
    {
    }
  }
}
