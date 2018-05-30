// Decompiled with JetBrains decompiler
// Type: ASCOM.DriverAccess.Rotator
// Assembly: ASCOM.DriverAccess, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 4B15C8A3-EB2C-468B-8912-4BBDBC985936
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.DriverAccess\6.0.0.0__565de7938946fba7\ASCOM.DriverAccess.dll

using ASCOM.DeviceInterface;
using ASCOM.Utilities;
using System;

namespace ASCOM.DriverAccess.Legacy
{
  public class Rotator : AscomDriver, IRotatorV2
  {
    private MemberFactory memberFactory;

    public bool CanReverse
    {
      get
      {
        return (bool) this.memberFactory.CallMember(1, "CanReverse", new Type[0]);
      }
    }

    public bool IsMoving
    {
      get
      {
        return (bool) this.memberFactory.CallMember(1, "IsMoving", new Type[0]);
      }
    }

    public float Position
    {
      get
      {
        return (float) this.memberFactory.CallMember(1, "Position", new Type[0]);
      }
    }

    public bool Reverse
    {
      get
      {
        return (bool) this.memberFactory.CallMember(1, "Reverse", new Type[0]);
      }
      set
      {
        this.memberFactory.CallMember(2, "Reverse", new Type[0], (object) value);
      }
    }

    public float StepSize
    {
      get
      {
        return (float) this.memberFactory.CallMember(1, "StepSize", new Type[0]);
      }
    }

    public float TargetPosition
    {
      get
      {
        return (float) this.memberFactory.CallMember(1, "TargetPosition", new Type[0]);
      }
    }

    public Rotator(string rotatorId)
      : base(rotatorId)
    {
      this.memberFactory = this.MemberFactory;
    }

    public static string Choose(string rotatorId)
    {
      using (Chooser chooser = new Chooser())
      {
        chooser.DeviceType = "Rotator";
        return chooser.Choose(rotatorId);
      }
    }

    public void Halt()
    {
      this.memberFactory.CallMember(3, "Halt", new Type[0]);
    }

    public void Move(float Position)
    {
      this.memberFactory.CallMember(3, "Move", new Type[1]
      {
        typeof (float)
      }, (object) Position);
    }

    public void MoveAbsolute(float Position)
    {
      this.memberFactory.CallMember(3, "MoveAbsolute", new Type[1]
      {
        typeof (float)
      }, (object) Position);
    }
  }
}
