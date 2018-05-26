// Decompiled with JetBrains decompiler
// Type: ASCOM.DriverAccess.Focuser
// Assembly: ASCOM.DriverAccess, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 4B15C8A3-EB2C-468B-8912-4BBDBC985936
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.DriverAccess\6.0.0.0__565de7938946fba7\ASCOM.DriverAccess.dll

using ASCOM.DeviceInterface;
using ASCOM.Utilities;
using System;

namespace ASCOM.DriverAccess
{
    public class Focuser : AscomDriver, IFocuserV2
    {
        private MemberFactory memberFactory;

        public bool Absolute
        {
            get
            {
                return memberFactory.GetPropertyValue<bool>("Absolute");
                //return Convert.ToBoolean(this.memberFactory.CallMember(1, "Absolute", new Type[0]));
            }
        }

        public bool IsMoving
        {
            get
            {
                return memberFactory.GetPropertyValue<bool>("IsMoving");
                //return Convert.ToBoolean(this.memberFactory.CallMember(1, "IsMoving", new Type[0]));
            }
        }

        public bool Link
        {
            get
            {
                return memberFactory.GetPropertyValue<bool>("Link");
                //return Convert.ToBoolean(this.memberFactory.CallMember(1, "Link", new Type[0]));
            }
            set
            {
                this.memberFactory.CallMember(2, "Link", new Type[0], (object)value);
            }
        }

        public int MaxIncrement
        {
            get
            {
                return memberFactory.GetPropertyValue<int>("MaxIncrement");
                //return Convert.ToInt32(this.memberFactory.CallMember(1, "MaxIncrement", new Type[0]));
            }
        }

        public int MaxStep
        {
            get
            {
                return memberFactory.GetPropertyValue<int>("MaxStep");
                //return Convert.ToInt32(this.memberFactory.CallMember(1, "MaxStep", new Type[0]));
            }
        }

        public int Position
        {
            get
            {
                return memberFactory.GetPropertyValue<int>("Position");
                //return Convert.ToInt32(this.memberFactory.CallMember(1, "Position", new Type[0]));
            }
        }

        public double StepSize
        {
            get
            {
                return memberFactory.GetPropertyValue<double>("StepSize");
                //return Convert.ToDouble(this.memberFactory.CallMember(1, "StepSize", new Type[0]));
            }
        }

        public bool TempComp
        {
            get
            {
                return memberFactory.GetPropertyValue<bool>("TempComp");
                //return Convert.ToBoolean(this.memberFactory.CallMember(1, "TempComp", new Type[0]));
            }
            set
            {
                memberFactory.SetPropertyValue("TempComp", value);
                //this.memberFactory.CallMember(2, "TempComp", new Type[0], (object) value);
            }
        }

        public bool TempCompAvailable
        {
            get
            {
                return memberFactory.GetPropertyValue<bool>("TempCompAvailable");
                //return Convert.ToBoolean(this.memberFactory.CallMember(1, "TempCompAvailable", new Type[0]));
            }
        }

        public double Temperature
        {
            get
            {
                return memberFactory.GetPropertyValue<double>("Temperature");
                //return Convert.ToDouble(this.memberFactory.CallMember(1, "Temperature", new Type[0]));
            }
        }

        public Focuser(string focuserId)
          : base(focuserId)
        {
            this.memberFactory = this.MemberFactory;
        }

        public static string Choose(string focuserId)
        {
            using (Chooser chooser = new Chooser())
            {
                chooser.DeviceType = "Focuser";
                return chooser.Choose(focuserId);
            }
        }

        public void Halt()
        {
            memberFactory.ExecMethod("Halt", null);
            //this.memberFactory.CallMember(3, "Halt", new Type[0]);
        }

        public void Move(int Position)
        {
            memberFactory.ExecMethod("Move", Position);
            //this.memberFactory.CallMember(3, "Move", new Type[1]{typeof (int)}, (object)Position);
        }
    }
}
