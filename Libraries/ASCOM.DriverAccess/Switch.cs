// Decompiled with JetBrains decompiler
// Type: ASCOM.DriverAccess.Switch
// Assembly: ASCOM.DriverAccess, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 4B15C8A3-EB2C-468B-8912-4BBDBC985936
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.DriverAccess\6.0.0.0__565de7938946fba7\ASCOM.DriverAccess.dll

using ASCOM.DeviceInterface;
using ASCOM.Utilities;
using System;

namespace ASCOM.DriverAccess
{
  public class Switch : AscomDriver, ISwitchV2
  {
    private MemberFactory memberFactory;

    public short MaxSwitch
    {
      get
      {
        return Convert.ToInt16(this.memberFactory.CallMember(1, "MaxSwitch", new Type[0]));
      }
    }

    public Switch(string switchId)
      : base(switchId)
    {
      this.memberFactory = this.MemberFactory;
    }

    public static string Choose(string switchId)
    {
      using (Chooser chooser = new Chooser())
      {
        chooser.DeviceType = "Switch";
        return chooser.Choose(switchId);
      }
    }

    public string GetSwitchName(short id)
    {
      return (string) this.memberFactory.CallMember(3, "GetSwitchName", new Type[1]
      {
        typeof (short)
      }, (object) id);
    }

    public void SetSwitchName(short id, string name)
    {
      this.memberFactory.CallMember(3, "SetSwitchName", new Type[2]
      {
        typeof (short),
        typeof (string)
      }, (object) id, (object) name);
    }

    public string GetSwitchDescription(short id)
    {
      return (string) this.memberFactory.CallMember(3, "GetSwitchDescription", new Type[1]
      {
        typeof (short)
      }, (object) id);
    }

    public bool CanWrite(short id)
    {
      return (bool) this.memberFactory.CallMember(3, "CanWrite", new Type[1]
      {
        typeof (short)
      }, (object) id);
    }

    public bool GetSwitch(short id)
    {
      return (bool) this.memberFactory.CallMember(3, "GetSwitch", new Type[1]
      {
        typeof (short)
      }, (object) id);
    }

    public void SetSwitch(short id, bool state)
    {
      this.memberFactory.CallMember(3, "SetSwitch", new Type[2]
      {
        typeof (short),
        typeof (bool)
      }, (object) id, (object) state);
    }

    public double MaxSwitchValue(short id)
    {
      try
      {
        return (double) this.memberFactory.CallMember(3, "MaxSwitchValue", new Type[1]
        {
          typeof (short)
        }, (object) id);
      }
      catch (System.NotImplementedException ex)
      {
        return 1.0;
      }
    }

    public double MinSwitchValue(short id)
    {
      try
      {
        return (double) this.memberFactory.CallMember(3, "MinSwitchValue", new Type[1]
        {
          typeof (short)
        }, (object) id);
      }
      catch (System.NotImplementedException ex)
      {
        return 0.0;
      }
    }

    public double SwitchStep(short id)
    {
      try
      {
        return (double) this.memberFactory.CallMember(3, "SwitchStep", new Type[1]
        {
          typeof (short)
        }, (object) id);
      }
      catch (System.NotImplementedException ex)
      {
        return 1.0;
      }
    }

    public double GetSwitchValue(short id)
    {
      try
      {
        return (double) this.memberFactory.CallMember(3, "GetSwitchValue", new Type[1]
        {
          typeof (short)
        }, (object) id);
      }
      catch (System.NotImplementedException ex)
      {
        return this.GetSwitch(id) ? 1.0 : 0.0;
      }
    }

    public void SetSwitchValue(short id, double value)
    {
      try
      {
        this.memberFactory.CallMember(3, "SetSwitchValue", new Type[2]
        {
          typeof (short),
          typeof (double)
        }, (object) id, (object) value);
      }
      catch (System.NotImplementedException ex)
      {
        bool state = value >= 0.5;
        this.SetSwitch(id, state);
      }
    }
  }
}
