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
    public class Focuser : AscomDriver<IFocuserV2>, IFocuserV2
    {
        //protected Focuser() : base() { }
        public Focuser(string deviceProgId) : base(deviceProgId) { }

        public bool IsMoving { get => Impl.IsMoving; }

        public int Position { get => Impl.Position; }

        public double StepSize { get => Impl.StepSize; }

        public bool Absolute { get => Impl.Absolute; }

        public bool Link
        {
            get => Impl.Connected;
            set => Impl.Connected = value;
        }

        public int MaxIncrement { get => Impl.MaxIncrement; }

        public int MaxStep { get => Impl.MaxStep; }

        public bool TempComp
        {
            get => Impl.TempComp;
            set
            {
                if (TempComp != value)
                {
                    Impl.TempComp = value;
                    profile.SetValue(nameof(TempComp), Impl.TempComp.ToString());
                    RaisePropertyChanged(nameof(TempComp));
                }
            }
        }

        public bool TempCompAvailable { get => Impl.TempCompAvailable; }

        public double Temperature { get => Impl.Temperature; }

        public void Halt()
        {
            TL.LogMessage($"Halt", $"Begin");
            Impl.Halt();
            TL.LogMessage($"Halt", "End");
        }

        public void Move(int position)
        {
            TL.LogMessage($"Move", $"Begin: {Position} => {position}");
            Impl.Move(position);
            TL.LogMessage($"Move", "End");
        }

    }
}