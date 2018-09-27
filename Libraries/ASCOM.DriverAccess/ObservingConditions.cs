// Decompiled with JetBrains decompiler
// Type: ASCOM.DriverAccess.ObservingConditions
// Assembly: ASCOM.DriverAccess, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 4B15C8A3-EB2C-468B-8912-4BBDBC985936
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.DriverAccess\6.0.0.0__565de7938946fba7\ASCOM.DriverAccess.dll

using ASCOM.DeviceInterface;
using ASCOM.Utilities;
using System;

namespace ASCOM.DriverAccess
{
    public class ObservingConditions : AscomDriver<IObservingConditions>, IObservingConditions
    {
        public double AveragePeriod
        {
            get => Impl.AveragePeriod;
            set
            {
                if (AveragePeriod != value)
                {
                    Impl.AveragePeriod = value;
                    profile.SetValue(nameof(AveragePeriod), Impl.AveragePeriod.ToString());
                    RaisePropertyChanged(nameof(AveragePeriod));
                }
            }
        }

        public double CloudCover { get => Impl.CloudCover; }

        public double DewPoint { get => Impl.DewPoint; }

        public double Humidity { get => Impl.Humidity; }

        public double Pressure { get => Impl.Pressure; }

        public double RainRate { get => Impl.RainRate; }

        public double SkyBrightness { get => Impl.SkyBrightness; }

        public double SkyQuality { get => Impl.SkyQuality; }

        public double StarFWHM { get => Impl.StarFWHM; }

        public double SkyTemperature { get => Impl.SkyTemperature; }

        public double Temperature { get => Impl.Temperature; }

        public double WindDirection { get => Impl.WindDirection; }

        public double WindGust { get => Impl.WindGust; }

        public double WindSpeed { get => Impl.WindSpeed; }

        public ObservingConditions(string observingConditionsId) : base(observingConditionsId) { }

        public double TimeSinceLastUpdate(string propertyName)
        {
            TL.LogMessage($"Begin TimeSinceLastUpdate({nameof(propertyName)}={propertyName})", $"");
            var result = Impl.TimeSinceLastUpdate(propertyName);
            TL.LogMessage($"End TimeSinceLastUpdate", $"Result: {result}");
            return result;
        }

        public string SensorDescription(string propertyName)
        {
            TL.LogMessage($"Begin SensorDescription(azimuth={propertyName})", $"");
            var result = Impl.SensorDescription(propertyName);
            TL.LogMessage($"End SensorDescription", $"Result: { result}");
            return result;
        }

        public void Refresh()
        {
            TL.LogMessage($"Begin Refresh()", $"");
            Impl.Refresh();
            TL.LogMessage($"End Refresh", "");
        }
    }
}
