// Decompiled with JetBrains decompiler
// Type: ASCOM.Astrometry.NOVASCOM.Star
// Assembly: ASCOM.Astrometry, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 695EF5AA-123E-4689-AAF5-64FD1324F6BB
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.Astrometry\6.0.0.0__565de7938946fba7\ASCOM.Astrometry.dll

using ASCOM.Astrometry.Exceptions;
using ASCOM.Astrometry.NOVAS;
using Microsoft.VisualBasic;

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ASCOM.Astrometry.NOVASCOM
{
    //[Guid("8FD58EDE-DF7A-4fdc-9DEC-FD0B36424F5F")]
    [Obsolete("This class will be withdrawn in the next major release, please use the SOFA or NOVAS31 classes instead")]
    //[ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class Star : IStar
    {
        private double m_rv;
        private double m_plx;
        private double m_pmdec;
        private double m_pmra;
        private double m_ra;
        private double m_dec;
        private double m_deltat;
        private bool m_rav;
        private bool m_decv;
        private bool m_bDTValid;
        private object m_earthephobj;
        private string m_cat;
        private string m_name;
        private int m_num;
        private BodyDescription m_earth;
        private short hr;
        private double[] m_earthephdisps;

        public string Catalog
        {
            get
            {
                return this.m_cat;
            }
            set
            {
                if ((value?.Length??0) > 3)
                    throw new ASCOM.Utilities.Exceptions.InvalidValueException("Star.Catalog Catlog > 3 characters long: " + value);
                this.m_cat = value;
            }
        }

        public double Declination
        {
            get
            {
                if (!this.m_rav)
                    throw new ASCOM.Astrometry.Exceptions.ValueNotSetException("Star.Declination Value not available");
                return this.m_dec;
            }
            set
            {
                this.m_dec = value;
                this.m_decv = true;
            }
        }

        public double DeltaT
        {
            get
            {
                if (!this.m_bDTValid)
                    throw new ASCOM.Astrometry.Exceptions.ValueNotSetException("Star.DeltaT Value not available");
                return this.m_deltat;
            }
            set
            {
                this.m_deltat = value;
                this.m_bDTValid = true;
            }
        }

        public object EarthEphemeris
        {
            get
            {
                return this.m_earthephobj;
            }
            set
            {
                this.m_earthephobj = RuntimeHelpers.GetObjectValue(value);
            }
        }

        public string Name
        {
            get
            {
                return this.m_name;
            }
            set
            {
                if ((value?.Length??0) > 50)
                    throw new ASCOM.Utilities.Exceptions.InvalidValueException("Star.Name Name > 50 characters long: " + value);
                this.m_name = value;
            }
        }

        public int Number
        {
            get
            {
                return this.m_num;
            }
            set
            {
                this.m_num = value;
            }
        }

        public double Parallax
        {
            get
            {
                return this.m_plx;
            }
            set
            {
                this.m_plx = value;
            }
        }

        public double ProperMotionDec
        {
            get
            {
                return this.m_pmdec;
            }
            set
            {
                this.m_pmdec = value;
            }
        }

        public double ProperMotionRA
        {
            get
            {
                return this.m_pmra;
            }
            set
            {
                this.m_pmra = value;
            }
        }

        public double RadialVelocity
        {
            get
            {
                return this.m_rv;
            }
            set
            {
                this.m_rv = value;
            }
        }

        public double RightAscension
        {
            get
            {
                if (!this.m_rav)
                    throw new ASCOM.Astrometry.Exceptions.ValueNotSetException("Star.RightAscension Value not available");
                return this.m_ra;
            }
            set
            {
                this.m_ra = value;
                this.m_rav = true;
            }
        }

        public Star()
        {
            this.m_earthephdisps = new double[5];
            this.m_rv = 0.0;
            this.m_plx = 0.0;
            this.m_pmdec = 0.0;
            this.m_pmra = 0.0;
            this.m_rav = false;
            this.m_ra = 0.0;
            this.m_decv = false;
            this.m_dec = 0.0;
            this.m_cat = "";
            this.m_name = "";
            this.m_num = 0;
            this.m_earthephobj = (object)null;
            this.m_bDTValid = false;
            this.m_earth = new BodyDescription();
            this.m_earth.Number = Body.Earth;
            this.m_earth.Name = "Earth";
            this.m_earth.Type = BodyType.Moon;
        }

        public PositionVector GetApparentPosition(double tjd)
        {
            CatEntry star = new CatEntry();
            PositionVector positionVector = new PositionVector();
            double[] bary_earthp = new double[4];
            double[] bary_earthv = new double[4];
            double[] helio_earthp = new double[4];
            double[] helio_earthv = new double[4];
            double[] pos = new double[4];
            double[] pos2_1 = new double[4];
            double[] pos2_2 = new double[4];
            double[] pos2_3 = new double[4];
            double[] pos2_4 = new double[4];
            double[] pos2_5 = new double[4];
            double[] vel = new double[4];
            double[] pos2_6 = new double[4];
            if (!(this.m_rav & this.m_decv))
                throw new ASCOM.Astrometry.Exceptions.ValueNotSetException("Star.GetApparentPosition RA or DEC not available");
            double tdb = 0;
            this.hr = NOVAS2.GetEarth(tjd, ref this.m_earth, ref tdb, ref bary_earthp, ref bary_earthv, ref helio_earthp, ref helio_earthv);
            if ((int)this.hr > 0)
            {
                pos2_6[0] = 0.0;
                pos2_6[1] = 0.0;
                pos2_6[2] = 0.0;
                throw new NOVASFunctionException("Star.GetApparentPosition", "get_earth", this.hr);
            }
            star.RA = this.m_ra;
            star.Dec = this.m_dec;
            star.ProMoRA = this.m_pmra;
            star.ProMoDec = this.m_pmdec;
            star.Parallax = this.m_plx;
            star.RadialVelocity = this.m_rv;
            NOVAS2.StarVectors(star, ref pos, ref vel);
            NOVAS2.ProperMotion(2451545.0, pos, vel, tdb, ref pos2_1);
            double lighttime = 0;
            NOVAS2.BaryToGeo(pos2_1, bary_earthp, ref pos2_2, ref lighttime);
            int num1 = (int)NOVAS2.SunField(pos2_2, helio_earthp, ref pos2_3);
            int num2 = (int)NOVAS2.Aberration(pos2_3, bary_earthv, lighttime, ref pos2_4);
            NOVAS2.Precession(2451545.0, pos2_4, tdb, ref pos2_5);
            int num3 = (int)NOVAS2.Nutate(tdb, NutationDirection.MeanToTrue, pos2_5, ref pos2_6);
            positionVector.x = pos2_6[0];
            positionVector.y = pos2_6[1];
            positionVector.z = pos2_6[2];
            return positionVector;
        }

        public PositionVector GetAstrometricPosition(double tjd)
        {
            CatEntry star = new CatEntry();
            PositionVector positionVector = new PositionVector();
            double[] pos = new double[4];
            double[] vel = new double[4];
            double[] pos2_1 = new double[4];
            double[] bary_earthp = new double[4];
            double[] bary_earthv = new double[4];
            double[] helio_earthp = new double[4];
            double[] helio_earthv = new double[4];
            double[] pos2_2 = new double[4];
            if (!(this.m_rav & this.m_decv))
                throw new ASCOM.Astrometry.Exceptions.ValueNotSetException("Star.GetAstrometricPosition RA or DEC not available");
            double tdb = 0;
            this.hr = NOVAS2.GetEarth(tjd, ref this.m_earth, ref tdb, ref bary_earthp, ref bary_earthv, ref helio_earthp, ref helio_earthv);
            if ((int)this.hr > 0)
            {
                pos2_2[0] = 0.0;
                pos2_2[1] = 0.0;
                pos2_2[2] = 0.0;
                throw new NOVASFunctionException("Star.GetApparentPosition", "get_earth", this.hr);
            }
            star.RA = this.m_ra;
            star.Dec = this.m_dec;
            star.ProMoRA = this.m_pmra;
            star.ProMoDec = this.m_pmdec;
            star.Parallax = this.m_plx;
            star.RadialVelocity = this.m_rv;
            NOVAS2.StarVectors(star, ref pos, ref vel);
            NOVAS2.ProperMotion(2451545.0, pos, vel, tdb, ref pos2_1);
            double lighttime = 0;
            NOVAS2.BaryToGeo(pos2_1, bary_earthp, ref pos2_2, ref lighttime);
            positionVector.x = pos2_2[0];
            positionVector.y = pos2_2[1];
            positionVector.z = pos2_2[2];
            return positionVector;
        }

        public PositionVector GetLocalPosition(double tjd, Site site)
        {
            CatEntry star = new CatEntry();
            PositionVector positionVector = new PositionVector();
            SiteInfo locale = new SiteInfo();
            double[] pos2_1 = new double[4];
            double[] pos2_2 = new double[4];
            double[] earthvector1 = new double[4];
            double[] vel1 = new double[4];
            double[] earthvector2 = new double[4];
            double[] numArray = new double[4];
            double[] pos = new double[4];
            double[] vel2 = new double[4];
            double[] pos2_3 = new double[4];
            double[] pos2_4 = new double[4];
            double[] pos2_5 = new double[4];
            double[] pos2_6 = new double[4];
            double[] bary_earthp = new double[4];
            double[] bary_earthv = new double[4];
            double[] helio_earthp = new double[4];
            double[] helio_earthv = new double[4];
            double[] pos2_7 = new double[4];
            if (!(this.m_rav & this.m_decv))
                throw new ASCOM.Astrometry.Exceptions.ValueNotSetException("Star.GetLocalPosition RA or DEC not available");
            double jd_high = !this.m_bDTValid ? tjd - DeltatCode.DeltaTCalc(tjd) / 86400.0 : tjd - this.m_deltat;
            star.RA = this.m_ra;
            star.Dec = this.m_dec;
            star.ProMoRA = this.m_pmra;
            star.ProMoDec = this.m_pmdec;
            star.Parallax = this.m_plx;
            star.RadialVelocity = this.m_rv;
            try
            {
                locale.Latitude = site.Latitude;
            }
            catch (Exception ex)
            {
                //ProjectData.SetProjectError(ex);
                throw new ValueNotAvailableException("Star:GetLocalPosition Site.Latitude is not available");
            }
            try
            {
                locale.Longitude = site.Longitude;
            }
            catch (Exception ex)
            {
                //ProjectData.SetProjectError(ex);
                throw new ValueNotAvailableException("Star:GetLocalPosition Site.Longitude is not available");
            }
            try
            {
                locale.Height = site.Height;
            }
            catch (Exception ex)
            {
                //ProjectData.SetProjectError(ex);
                throw new ValueNotAvailableException("Star:GetLocalPosition Site.Height is not available");
            }
            double tdb = 0;
            this.hr = NOVAS2.GetEarth(tjd, ref this.m_earth, ref tdb, ref bary_earthp, ref bary_earthv, ref helio_earthp, ref helio_earthv);
            if ((int)this.hr > 0)
            {
                pos2_7[0] = 0.0;
                pos2_7[1] = 0.0;
                pos2_7[2] = 0.0;
                throw new NOVASFunctionException("Star.GetApparentPosition", "get_earth", this.hr);
            }
            double mobl = 0;
            double tobl = 0;
            double eq = 0;
            double dpsi = 0;
            double deps = 0;
            NOVAS2.EarthTilt(tdb, ref mobl, ref tobl, ref eq, ref dpsi, ref deps);
            double gst = 0;
            NOVAS2.SiderealTime(jd_high, 0.0, eq, ref gst);
            NOVAS2.Terra(ref locale, gst, ref pos, ref vel2);
            int num1 = (int)NOVAS2.Nutate(tdb, NutationDirection.TrueToMean, pos, ref pos2_3);
            NOVAS2.Precession(tdb, pos2_3, 2451545.0, ref pos2_1);
            int num2 = (int)NOVAS2.Nutate(tdb, NutationDirection.TrueToMean, vel2, ref pos2_4);
            NOVAS2.Precession(tdb, pos2_4, 2451545.0, ref pos2_2);
            int index = 0;
            do
            {
                earthvector1[index] = bary_earthp[index] + pos2_1[index];
                vel1[index] = bary_earthv[index] + pos2_2[index];
                earthvector2[index] = helio_earthp[index] + pos2_1[index];
                numArray[index] = helio_earthv[index] + pos2_2[index];
                checked { ++index; }
            }
            while (index <= 2);
            NOVAS2.StarVectors(star, ref pos, ref vel2);
            NOVAS2.ProperMotion(2451545.0, pos, vel2, tdb, ref pos2_3);
            double lighttime = 0;
            NOVAS2.BaryToGeo(pos2_3, earthvector1, ref pos2_5, ref lighttime);
            int num3 = (int)NOVAS2.SunField(pos2_5, earthvector2, ref pos2_6);
            int num4 = (int)NOVAS2.Aberration(pos2_6, vel1, lighttime, ref pos2_7);
            positionVector.x = pos2_7[0];
            positionVector.y = pos2_7[1];
            positionVector.z = pos2_7[2];
            return positionVector;
        }

        public PositionVector GetTopocentricPosition(double tjd, Site site, bool Refract)
        {
            CatEntry star = new CatEntry();
            SiteInfo siteInfo = new SiteInfo();
            double[] earthvector1 = new double[4];
            double[] pos2_1 = new double[4];
            double[] vel1 = new double[4];
            double[] pos2_2 = new double[4];
            double[] earthvector2 = new double[4];
            double[] pos1 = new double[4];
            double[] pos2_3 = new double[4];
            double[] pos2_4 = new double[4];
            double[] pos2_5 = new double[4];
            double[] pos2_6 = new double[4];
            double[] pos2_7 = new double[4];
            double[] vel2 = new double[4];
            double[] pos2_8 = new double[4];
            double[] bary_earthp = new double[4];
            double[] bary_earthv = new double[4];
            double[] helio_earthp = new double[4];
            double[] helio_earthv = new double[4];
            double[] pos2 = new double[4];
            if (!(this.m_rav & this.m_decv))
                throw new ASCOM.Astrometry.Exceptions.ValueNotSetException("Star.GetTopocentricPosition RA or DEC not available");
            double jd_high = !this.m_bDTValid ? tjd - DeltatCode.DeltaTCalc(tjd) / 86400.0 : tjd - this.m_deltat;
            try
            {
                siteInfo.Latitude = site.Latitude;
            }
            catch (Exception ex)
            {
                //ProjectData.SetProjectError(ex);
                throw new ValueNotAvailableException("Star:GetTopocentricPosition Site.Latitude is not available");
            }
            try
            {
                siteInfo.Longitude = site.Longitude;
            }
            catch (Exception ex)
            {
                //ProjectData.SetProjectError(ex);
                throw new ValueNotAvailableException("Star:GetTopocentricPosition Site.Longitude is not available");
            }
            try
            {
                siteInfo.Height = site.Height;
            }
            catch (Exception ex)
            {
                //ProjectData.SetProjectError(ex);
                throw new ValueNotAvailableException("Star:GetTopocentricPosition Site.Height is not available");
            }
            double tdb = 0;
            this.hr = NOVAS2.GetEarth(tjd, ref this.m_earth, ref tdb, ref bary_earthp, ref bary_earthv, ref helio_earthp, ref helio_earthv);
            if ((int)this.hr > 0)
            {
                pos2[0] = 0.0;
                pos2[1] = 0.0;
                pos2[2] = 0.0;
                throw new NOVASFunctionException("Star.GetApparentPosition", "get_earth", this.hr);
            }
            double mobl = 0;
            double tobl = 0;
            double eq = 0;
            double dpsi = 0;
            double deps = 0;
            NOVAS2.EarthTilt(tdb, ref mobl, ref tobl, ref eq, ref dpsi, ref deps);
            double gst = 0;
            NOVAS2.SiderealTime(jd_high, 0.0, eq, ref gst);
            NOVAS2.Terra(ref siteInfo, gst, ref pos1, ref vel2);
            int num1 = (int)NOVAS2.Nutate(tdb, NutationDirection.TrueToMean, pos1, ref pos2_3);
            NOVAS2.Precession(tdb, pos2_3, 2451545.0, ref pos2_1);
            int num2 = (int)NOVAS2.Nutate(tdb, NutationDirection.TrueToMean, vel2, ref pos2_8);
            NOVAS2.Precession(tdb, pos2_8, 2451545.0, ref pos2_2);
            int index = 0;
            do
            {
                earthvector1[index] = bary_earthp[index] + pos2_1[index];
                vel1[index] = bary_earthv[index] + pos2_2[index];
                earthvector2[index] = helio_earthp[index] + pos2_1[index];
                checked { ++index; }
            }
            while (index <= 2);
            star.RA = this.m_ra;
            star.Dec = this.m_dec;
            star.ProMoRA = this.m_pmra;
            star.ProMoDec = this.m_pmdec;
            star.Parallax = this.m_plx;
            star.RadialVelocity = this.m_rv;
            NOVAS2.StarVectors(star, ref pos1, ref vel2);
            NOVAS2.ProperMotion(2451545.0, pos1, vel2, tdb, ref pos2_3);
            double lighttime = 0;
            NOVAS2.BaryToGeo(pos2_3, earthvector1, ref pos2_4, ref lighttime);
            int num3 = (int)NOVAS2.SunField(pos2_4, earthvector2, ref pos2_5);
            int num4 = (int)NOVAS2.Aberration(pos2_5, vel1, lighttime, ref pos2_6);
            NOVAS2.Precession(2451545.0, pos2_6, tdb, ref pos2_7);
            int num5 = (int)NOVAS2.Nutate(tdb, NutationDirection.MeanToTrue, pos2_7, ref pos2);
            double ra = 0;
            double dec = 0;
            int num6 = (int)NOVAS2.Vector2RADec(pos2, ref ra, ref dec);
            double num7 = Math.Sqrt(Math.Pow(pos2[0], 2.0) + Math.Pow(pos2[1], 2.0) + Math.Pow(pos2[2], 2.0));
            RefractionOption ref_option = RefractionOption.NoRefraction;
            if (Refract)
            {
                bool flag = true;
                try
                {
                    siteInfo.Temperature = site.Temperature;
                }
                catch (Exception ex)
                {
                    //ProjectData.SetProjectError(ex);
                    flag = false;
                    //ProjectData.ClearProjectError();
                }
                try
                {
                    siteInfo.Pressure = site.Pressure;
                }
                catch (Exception ex)
                {
                    //ProjectData.SetProjectError(ex);
                    flag = false;
                    //ProjectData.ClearProjectError();
                }
                ref_option = !flag ? RefractionOption.StandardRefraction : RefractionOption.LocationRefraction;
            }
            double zd = 0;
            double az = 0;
            double rar = 0;
            double decr = 0;
            if (this.m_bDTValid)
                NOVAS2.Equ2Hor(tjd, this.m_deltat, 0.0, 0.0, ref siteInfo, ra, dec, ref_option, ref zd, ref az, ref rar, ref decr);
            else
                NOVAS2.Equ2Hor(tjd, DeltatCode.DeltaTCalc(tjd), 0.0, 0.0, ref siteInfo, ra, dec, ref_option, ref zd, ref az, ref rar, ref decr);
            if (ref_option > RefractionOption.NoRefraction)
                NOVAS2.RADec2Vector(rar, decr, num7, ref pos2);
            return new PositionVector(pos2[0], pos2[1], pos2[2], rar, decr, num7, num7 / 173.14463348, az, 90.0 - zd);
        }

        public PositionVector GetVirtualPosition(double tjd)
        {
            CatEntry star = new CatEntry();
            PositionVector positionVector = new PositionVector();
            double[] pos = new double[4];
            double[] vel = new double[4];
            double[] pos2_1 = new double[4];
            double[] pos2_2 = new double[4];
            double[] pos2_3 = new double[4];
            double[] bary_earthp = new double[4];
            double[] bary_earthv = new double[4];
            double[] helio_earthp = new double[4];
            double[] helio_earthv = new double[4];
            double[] pos2_4 = new double[4];
            if (!(this.m_rav & this.m_decv))
                throw new ASCOM.Astrometry.Exceptions.ValueNotSetException("Star.GetVirtualPosition RA or DEC not available");
            star.RA = this.m_ra;
            star.Dec = this.m_dec;
            star.ProMoRA = this.m_pmra;
            star.ProMoDec = this.m_pmdec;
            star.Parallax = this.m_plx;
            star.RadialVelocity = this.m_rv;
            double tdb = 0;
            this.hr = NOVAS2.GetEarth(tjd, ref this.m_earth, ref tdb, ref bary_earthp, ref bary_earthv, ref helio_earthp, ref helio_earthv);
            if ((int)this.hr > 0)
            {
                pos2_4[0] = 0.0;
                pos2_4[1] = 0.0;
                pos2_4[2] = 0.0;
                throw new NOVASFunctionException("Star.GetApparentPosition", "get_earth", this.hr);
            }
            NOVAS2.StarVectors(star, ref pos, ref vel);
            NOVAS2.ProperMotion(2451545.0, pos, vel, tdb, ref pos2_1);
            double lighttime = 0;
            NOVAS2.BaryToGeo(pos2_1, bary_earthp, ref pos2_2, ref lighttime);
            int num1 = (int)NOVAS2.SunField(pos2_2, helio_earthp, ref pos2_3);
            int num2 = (int)NOVAS2.Aberration(pos2_3, bary_earthv, lighttime, ref pos2_4);
            positionVector.x = pos2_4[0];
            positionVector.y = pos2_4[1];
            positionVector.z = pos2_4[2];
            return positionVector;
        }

        public void Set(double RA, double Dec, double ProMoRA, double ProMoDec, double Parallax, double RadVel)
        {
            this.m_ra = RA;
            this.m_dec = Dec;
            this.m_pmra = ProMoRA;
            this.m_pmdec = ProMoDec;
            this.m_plx = Parallax;
            this.m_rv = RadVel;
            this.m_rav = true;
            this.m_decv = true;
            this.m_num = 0;
            this.m_name = "";
            this.m_cat = "";
        }

        public void SetHipparcos(double RA, double Dec, double ProMoRA, double ProMoDec, double Parallax, double RadVel)
        {
            CatEntry hipparcos = new CatEntry();
            CatEntry fk5 = new CatEntry();
            hipparcos.RA = RA;
            hipparcos.Dec = Dec;
            hipparcos.ProMoRA = ProMoRA;
            hipparcos.ProMoDec = ProMoDec;
            hipparcos.Parallax = Parallax;
            hipparcos.RadialVelocity = RadVel;
            NOVAS2.TransformHip(ref hipparcos, ref fk5);
            this.m_ra = fk5.RA;
            this.m_dec = fk5.Dec;
            this.m_pmra = fk5.ProMoRA;
            this.m_pmdec = fk5.ProMoDec;
            this.m_plx = fk5.Parallax;
            this.m_rv = fk5.RadialVelocity;
            this.m_rav = true;
            this.m_decv = true;
            this.m_num = 0;
            this.m_name = "";
            this.m_cat = "";
        }
    }
}