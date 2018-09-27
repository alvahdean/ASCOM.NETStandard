// Decompiled with JetBrains decompiler
// Type: ASCOM.DriverAccess.SafetyMonitor
// Assembly: ASCOM.DriverAccess, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 4B15C8A3-EB2C-468B-8912-4BBDBC985936
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.DriverAccess\6.0.0.0__565de7938946fba7\ASCOM.DriverAccess.dll

using ASCOM.DeviceInterface;
using ASCOM.Utilities;
using System;

namespace ASCOM.DriverAccess
{
  public class SafetyMonitor : AscomDriver<ISafetyMonitor>, ISafetyMonitor
  {
        public SafetyMonitor(string safetyMonitorId) : base(safetyMonitorId) { }

        public bool IsSafe { get => Impl.IsSafe; }

    }
}
