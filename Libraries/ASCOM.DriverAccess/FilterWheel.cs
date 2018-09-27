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
    public class FilterWheel : AscomDriver<IFilterWheelV2>, IFilterWheelV2
    {

        //public FilterWheel() : base() { }
        public FilterWheel(string filterWheelId) : base(filterWheelId) { }

        public int[] FocusOffsets { get => Impl.FocusOffsets; }

        public string[] Names { get => Impl.Names; }

        public short Position
        {
            get => Impl.Position;
            set
            {
                if (Position != value)
                {
                    Impl.Position = value;
                    profile.SetValue(nameof(Position), Impl.Position.ToString());
                    RaisePropertyChanged(nameof(Position));
                }
            }
        }

    }
}
