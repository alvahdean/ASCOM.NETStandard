// Decompiled with JetBrains decompiler
// Type: ASCOM.DriverAccess.Dome
// Assembly: ASCOM.DriverAccess, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 4B15C8A3-EB2C-468B-8912-4BBDBC985936
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.DriverAccess\6.0.0.0__565de7938946fba7\ASCOM.DriverAccess.dll

using ASCOM.DeviceInterface;
using ASCOM.Utilities;
using System;

namespace ASCOM.DriverAccess
{
    public class Dome : AscomDriver<IDomeV2>, IDomeV2
    {
        public Dome(string domeId) : base(domeId) { }

        public double Altitude { get => Impl.Altitude; }

        public bool AtHome { get => Impl.AtHome; }

        public bool AtPark { get => Impl.AtPark; }

        public double Azimuth { get => Impl.Azimuth; }

        public bool CanFindHome { get => Impl.CanFindHome; }

        public bool CanPark { get => Impl.CanPark; }

        public bool CanSetAltitude { get => Impl.CanSetAltitude; }

        public bool CanSetAzimuth { get => Impl.CanSetAzimuth; }

        public bool CanSetPark { get => Impl.CanSetPark; }

        public bool CanSetShutter { get => Impl.CanSetShutter; }

        public bool CanSlave { get => Impl.CanSlave; }

        public bool CanSyncAzimuth { get => Impl.CanSyncAzimuth; }

        public ShutterState ShutterStatus { get => Impl.ShutterStatus; }

        public bool Slaved
        {
            get => Impl.Slaved;
            set
            {
                if (Slaved != value)
                {
                    Impl.Slaved = value;
                    profile.SetValue(nameof(Slaved), Impl.Slaved.ToString());
                    RaisePropertyChanged(nameof(Slaved));
                }
            }
        }

        public bool Slewing { get => Impl.Slewing; }

        public void AbortSlew()
        {
            TL.LogMessage($"Begin AbortSlew()", $"");
            Impl.AbortSlew();
            TL.LogMessage($"End AbortSlew", $"");
        }

        public void CloseShutter()
        {
            TL.LogMessage($"Begin CloseShutter()", $"");
            Impl.CloseShutter();
            TL.LogMessage($"End CloseShutter", $"");
        }

        public void FindHome()
        {
            TL.LogMessage($"Begin FindHome()", $"");
            Impl.FindHome();
            TL.LogMessage($"End Move", $"");
        }

        public void OpenShutter()
        {
            TL.LogMessage($"Begin OpenShutter()", $"");
            Impl.OpenShutter();
            TL.LogMessage($"End Move", $"");
        }

        public void Park()
        {
            TL.LogMessage($"Begin Park()", $"");
            Impl.Park();
            TL.LogMessage($"End Park", "");
        }

        public void SetPark()
        {
            TL.LogMessage($"Begin SetPark()", "");
            Impl.SetPark();
            TL.LogMessage($"End SetPark", "");
        }

        public void SlewToAltitude(double altitude)
        {
            TL.LogMessage($"Begin SlewToAltitude(altitude={altitude})", $"");
            Impl.SlewToAltitude(altitude);
            TL.LogMessage($"End SlewToAltitude", "");
        }

        public void SlewToAzimuth(double azimuth)
        {
            TL.LogMessage($"Begin SlewToAzimuth(azimuth={azimuth})", $"");
            Impl.SlewToAzimuth(azimuth);
            TL.LogMessage($"End SlewToAzimuth", "");
        }

        public void SyncToAzimuth(double azimuth)
        {
            TL.LogMessage($"Begin SyncToAzimuth(azimuth={azimuth})", $"");
            Impl.SyncToAzimuth(azimuth);
            TL.LogMessage($"End SyncToAzimuth", "");
        }
    }
}
