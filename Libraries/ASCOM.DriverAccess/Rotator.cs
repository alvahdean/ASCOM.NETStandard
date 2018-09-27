// Decompiled with JetBrains decompiler
// Type: ASCOM.DriverAccess.Rotator
// Assembly: ASCOM.DriverAccess, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 4B15C8A3-EB2C-468B-8912-4BBDBC985936
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.DriverAccess\6.0.0.0__565de7938946fba7\ASCOM.DriverAccess.dll

using ASCOM.DeviceInterface;
using ASCOM.Utilities;
using System;

namespace ASCOM.DriverAccess
{
    public class Rotator : AscomDriver<IRotatorV2>, IRotatorV2
    {

        //protected Rotator() : base() { }
        public Rotator(string deviceProgId) : base(deviceProgId) { }

        public bool CanReverse { get => Impl.CanReverse; }

        public bool IsMoving { get => Impl.IsMoving; }

        public float Position { get => Impl.Position; }

        public float StepSize { get => Impl.StepSize; }

        public float TargetPosition { get => Impl.TargetPosition; }

        public bool Reverse
        {
            get => Impl.Reverse;
            set
            {
                if (Reverse != value)
                {
                    Impl.Reverse = value;
                    profile.SetValue(nameof(Reverse), Impl.Reverse.ToString());
                    RaisePropertyChanged(nameof(Reverse));
                }
            }
        }

        public void Halt()
        {
            TL.LogMessage($"Begin Halt()", $"");
            Impl.Halt();
            TL.LogMessage($"End Halt", $"");
        }

        public void Move(float position)
        {
            TL.LogMessage($"Begin Move(Position={position})", $"");
            Impl.Move(position);
            TL.LogMessage($"End Move", $"");
        }

        public void MoveAbsolute(float Position)
        {
            TL.LogMessage($"Begin MoveAbsolute(Position ={ Position})", $"");
            Impl.MoveAbsolute(Position);
            TL.LogMessage($"End MoveAbsolute", $"");
        }

    }
}
