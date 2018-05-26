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
  public class SafetyMonitor : AscomDriver, ISafetyMonitor
  {
    private readonly MemberFactory _memberFactory;

    public bool IsSafe
    {
      get
      {
        return (bool) this._memberFactory.CallMember(1, "IsSafe", new Type[0]);
      }
    }

    public SafetyMonitor(string safetyMonitorId)
      : base(safetyMonitorId)
    {
      this._memberFactory = this.MemberFactory;
    }

    public static string Choose(string safetyMonitorId)
    {
      using (Chooser chooser = new Chooser())
      {
        chooser.DeviceType = "SafetyMonitor";
        return chooser.Choose(safetyMonitorId);
      }
    }
  }
}
