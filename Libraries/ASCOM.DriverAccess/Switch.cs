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
    public class Switch : AscomDriver<ISwitchV2>, ISwitchV2
    {

        public Switch(string switchId) : base(switchId) { }

        public short MaxSwitch { get => Impl.MaxSwitch; }

        public string GetSwitchName(short id)
        {
            TL.LogMessage($"Begin {nameof(GetSwitchName)}({id})", $"");
            var result = Impl.GetSwitchName(id);
            TL.LogMessage($"End {nameof(GetSwitchName)}", $"Result: { result}");
            return result;
        }

        public void SetSwitchName(short id, string name)
        {
            TL.LogMessage($"Begin {nameof(SetSwitchName)}({id},{name})", $"");
            Impl.SetSwitchName(id, name);
            TL.LogMessage($"End {nameof(SetSwitchName)}", $"");
        }
        public string GetSwitchDescription(short id)
        {
            TL.LogMessage($"Begin {nameof(GetSwitchDescription)}({id})", $"");
            var result = Impl.GetSwitchDescription(id);
            TL.LogMessage($"End {nameof(GetSwitchDescription)}", $"Result: { result}");
            return result;
        }

        public bool CanWrite(short id)
        {
            TL.LogMessage($"Begin {nameof(CanWrite)}({id})", $"");
            var result = Impl.CanWrite(id);
            TL.LogMessage($"End {nameof(CanWrite)}", $"Result: { result}");
            return result;
        }

        public bool GetSwitch(short id)
        {
            TL.LogMessage($"Begin {nameof(GetSwitch)}({id})", $"");
            var result = Impl.GetSwitch(id);
            TL.LogMessage($"End {nameof(GetSwitch)}", $"Result: { result}");
            return result;
        }

        public void SetSwitch(short id, bool state)
        {
            TL.LogMessage($"Begin {nameof(SetSwitch)}({id},{state})", $"");
            Impl.SetSwitch(id, state);
            TL.LogMessage($"End {nameof(SetSwitch)}", $"");
        }

        public double MaxSwitchValue(short id)
        {
            TL.LogMessage($"Begin {nameof(MaxSwitchValue)}({id})", $"");
            var result = Impl.MaxSwitchValue(id);
            TL.LogMessage($"End {nameof(MaxSwitchValue)}", $"Result: { result}");
            return result;
        }

        public double MinSwitchValue(short id)
        {
            var result = 0.0;
            try
            {
                TL.LogMessage($"Begin {nameof(MinSwitchValue)}({id})", $"");
                result = Impl.MinSwitchValue(id);
            }
            catch (System.NotImplementedException) { }
            TL.LogMessage($"End {nameof(MinSwitchValue)}", $"Result: { result}");
            return result;
        }

        public double SwitchStep(short id)
        {
            var result = 0.0;
            try
            {
                TL.LogMessage($"Begin {nameof(SwitchStep)}({id})", $"");
                result = Impl.SwitchStep(id);
            }
            catch (System.NotImplementedException) { }
            TL.LogMessage($"End {nameof(SwitchStep)}", $"Result: { result}");
            return result;
        }

        public double GetSwitchValue(short id)
        {
            var result = 0.0;
            try
            {
                TL.LogMessage($"Begin {nameof(SwitchStep)}({id})", $"");
                result = Impl.SwitchStep(id);
            }
            catch (System.NotImplementedException)
            {
                result = GetSwitch(id) ? 1.0 : 0.0;
            }
            TL.LogMessage($"End {nameof(SwitchStep)}", $"Result: { result}");
            return result;
        }

        public void SetSwitchValue(short id, double value)
        {
            try
            {
                TL.LogMessage($"Begin {nameof(SetSwitchValue)}({id})", $"");
                Impl.SetSwitchValue(id, value);
            }
            catch (System.NotImplementedException)
            {
                bool state = value >= 0.5;
                SetSwitch(id, state);
            }
            TL.LogMessage($"End {nameof(SetSwitchValue)}", $"");
        }
    }
}
