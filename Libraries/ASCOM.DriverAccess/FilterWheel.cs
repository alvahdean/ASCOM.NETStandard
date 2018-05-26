// Decompiled with JetBrains decompiler
// Type: ASCOM.DriverAccess.FilterWheel
// Assembly: ASCOM.DriverAccess, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 4B15C8A3-EB2C-468B-8912-4BBDBC985936
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.DriverAccess\6.0.0.0__565de7938946fba7\ASCOM.DriverAccess.dll

using ASCOM.DeviceInterface;
using ASCOM.Utilities;
using System;

namespace ASCOM.DriverAccess
{
  public class FilterWheel : AscomDriver, IFilterWheelV2
  {
    private MemberFactory memberFactory;

    public int[] FocusOffsets
    {
      get
      {
        return (int[]) this.memberFactory.CallMember(1, "FocusOffsets", new Type[0]);
      }
    }

    public string[] Names
    {
      get
      {
        return (string[]) this.memberFactory.CallMember(1, "Names", new Type[0]);
      }
    }

    public short Position
    {
      get
      {
        return Convert.ToInt16(this.memberFactory.CallMember(1, "Position", new Type[0]));
      }
      set
      {
        this.memberFactory.CallMember(2, "Position", new Type[1]
        {
          typeof (short)
        }, (object) value);
      }
    }

    public FilterWheel(string filterWheelId)
      : base(filterWheelId)
    {
      this.memberFactory = this.MemberFactory;
    }

    public static string Choose(string filterWheelId)
    {
      using (Chooser chooser = new Chooser())
      {
        chooser.DeviceType = "FilterWheel";
        return chooser.Choose(filterWheelId);
      }
    }
  }
}
