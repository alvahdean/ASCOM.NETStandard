using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace xASCOM.Simulator
{
    internal class AltAz : IEquatable<AltAz>
    {
        private double _alt;
        private double _az;
        public static double NormalizeAltitude(double value)
        {
            value = value % 180;
            if (value < -90)
                value = -90;
            if (value > 90)
                value = 90;
            return value;
        }
        public static double NormalizeAzimuth(double value)
        {
            value = value % 360;
            if (value < 0)
                value = 360 - value;
            return value;
        }
        protected void normalize()
        {
            Altitude = NormalizeAltitude(Altitude);
            Azimuth = NormalizeAzimuth(Azimuth);
        }
        public AltAz() { Altitude = 0; Azimuth = 0; }
        public AltAz(double alt, double az) { Altitude = alt; Azimuth = az; }
        public AltAz(AltAz other)
        {
            Altitude = other?.Altitude ?? 0;
            Azimuth = other?.Azimuth ?? 0;
        }
        public static implicit operator AltAz(String json)
        {
            return String.IsNullOrWhiteSpace(json)
                ? new AltAz()
                : JsonConvert.DeserializeObject<AltAz>(json);
        }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        public static AltAz Normalized(AltAz value)
        {
            AltAz norm = new AltAz(value);
            norm.Altitude = NormalizeAltitude(value.Altitude);
            norm.Azimuth = NormalizeAzimuth(value.Azimuth);
            return norm;
        }
        public double Altitude
        {
            get => _alt;
            set
            {
                if(Altitude!=value)
                    _alt = NormalizeAltitude(value);
            }
        }
        public double Azimuth
        {
            get => _az;
            set
            {
                if (Azimuth != value)
                    _az = NormalizeAzimuth(value);
            }
        }
        public static AltAz operator +(AltAz a, AltAz b)
        {
            AltAz result = new AltAz(a);
            result.Altitude += b.Altitude;
            result.Azimuth += b.Azimuth;
            result.normalize();
            return result;
        }
        public static AltAz operator -(AltAz a, AltAz b)
        {
            AltAz result = new AltAz(a);
            result.Altitude -= b.Altitude;
            result.Azimuth -= b.Azimuth;
            result.normalize();
            return result;
        }

        public bool Equals(AltAz other)
        {
            return Equals(other, null);
        }
        public bool Equals(AltAz other, AltAz precision)
        {
            if (object.Equals(other, null))
                return false;
            if (object.Equals(other, this))
                return true;
            precision = Normalized(precision ?? new AltAz(1/60, 1/60));
            return AzimuthEquals(other.Azimuth,precision.Azimuth)
                && AltitudeEquals(other.Altitude, precision.Altitude);
        }
        public bool AzimuthEquals(double other, double precision = .5)
        {

            other = NormalizeAzimuth(other);
            precision = Math.Abs(precision);
            double min = NormalizeAzimuth(other - precision);
            double max = NormalizeAzimuth(other + precision);
            return Azimuth >= min && Azimuth <= max;
        }
        public bool AltitudeEquals(double other, double precision = .5)
        {
            other = NormalizeAltitude(other);
            precision = Math.Abs(precision);
            double min = NormalizeAltitude(other - precision);
            double max = NormalizeAltitude(other + precision);
            return Altitude >= min && Altitude <= max;
        }
    }
}
