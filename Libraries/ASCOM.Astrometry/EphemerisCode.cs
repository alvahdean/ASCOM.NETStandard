using System;
using ASCOM;
using static ASCOM.Astrometry.NOVAS.NOVAS2;
using static ASCOM.Astrometry.GlobalItems;
using ASCOM.Utilities;
using ASCOM.Astrometry.NOVASCOM;
using ASCOM.Astrometry.Kepler;

namespace ASCOM.Astrometry
{
    internal static class EphemerisCode
    {
        private static double get_earth_tjd_last = 0;
        private static double solsys3_tlast = 0;
        private static double solsys3_sine, solsys3_cose, solsys3_tmass;
        private static double[] solsys3_pbary=new double[3], solsys3_vbary = new double[3];
        internal static void get_earth_nov(ref IEphemeris pEphDisp, double tjd, ref double tdb, ref double[] peb, ref double[] veb, ref double[] pes, ref double[] ves)
        {
            short i, rc;
            double dummy=0, secdiff=0;
            double ltdb;
            double[] lpeb = new double[3];
            double[] lveb = new double[3];
            double[] lpes = new double[3];
            double[] lves = new double[3];
            Tdb2Tdt(tjd, ref dummy, ref secdiff);
            ltdb = tjd + secdiff / 86400;
            try
            {
                rc = solarsystem_nov(ref pEphDisp, tjd, ltdb, Body.Earth, Origin.Barycentric, ref lpeb, ref lveb);
                if (rc != 0)
                    throw new Exceptions.NOVASFunctionException("EphemerisCode:get_earth_nov Earth eph exception", "solarsystem_nov", rc);
            }
            catch (Exception ex)
            {
                get_earth_tjd_last = 0;
                throw;
            }

            try
            {
                rc = solarsystem_nov(ref pEphDisp, tjd, ltdb, Body.Earth, Origin.Heliocentric, ref lpes, ref lves);
                if (rc != 0)
                    throw new Exceptions.NOVASFunctionException("EphemerisCode:get_earth_nov Earth eph exception", "solarsystem_nov", rc);
            }
            catch (Exception ex)
            {
                get_earth_tjd_last = 0;
                throw;
            }

            get_earth_tjd_last = tjd;
            tdb = ltdb;
            for (i = 0; i <= 2; i++)
            {
                peb[i] = lpeb[i];
                veb[i] = lveb[i];
                pes[i] = lpes[i];
                ves[i] = lves[i];
            }
        }

        internal static void ephemeris_nov(ref IEphemeris ephDisp, double tjd, BodyType btype, int num, string name, Origin origin, ref double[] pos, ref double[] vel)
        {
            int i;
            double[] posvel, p, v;
            if ((ephDisp == null))
            {
                throw new Exceptions.ValueNotSetException("Ephemeris_nov Ephemeris object not set");
            }
            else
            {
                if (((origin != Origin.Barycentric) & (origin != Origin.Heliocentric)))
                    throw new Utilities.Exceptions.InvalidValueException("Ephemeris_nov Origin is neither barycentric or heliocentric");
                BodyType kbtype;
                switch (btype)
                {
                    case BodyType.Comet:
                        kbtype = BodyType.Comet;
                        break;
                    case BodyType.MajorPlanet:
                        kbtype = BodyType.MajorPlanet;
                        break;
                    case BodyType.MinorPlanet:
                        kbtype = BodyType.MinorPlanet;
                        break;
                    default:
                        throw new NotSupportedException();
                }

                Body knum;
                switch (num)
                {
                    case 1:
                        knum = Body.Mercury;
                        break;
                    case 2:
                        knum = Body.Venus;
                        break;
                    case 3:
                        knum = Body.Earth;
                        break;
                    case 4:
                        knum = Body.Mars;
                        break;
                    case 5:
                        knum = Body.Jupiter;
                        break;
                    case 6:
                        knum = Body.Saturn;
                        break;
                    case 7:
                        knum = Body.Uranus;
                        break;
                    case 8:
                        knum = Body.Neptune;
                        break;
                    case 9:
                        knum = Body.Pluto;
                        break;
                    default:
                        throw new ArgumentException();
                }

                ephDisp.BodyType = kbtype;
                ephDisp.Number = knum;
                if ((name != ""))
                    ephDisp.Name = name;
                posvel = ephDisp.GetPositionAndVelocity(tjd);


                if ((origin == Origin.Barycentric))
                {
                    double[] sun_pos=new double[3], sun_vel = new double[3];
                    solsys3_nov(tjd, Body.Sun, Origin.Barycentric, ref sun_pos, ref sun_vel);
                    for (i = 0; i <= 2; i++)
                    {
                        posvel[i] += sun_pos[i];
                        posvel[i + 3] += sun_vel[i];
                    }
                }

                for (i = 0; i <= 2; i++)
                {
                    pos[i] = posvel[i];
                    vel[i] = posvel[i + 3];
                }
            }
        }

        internal static short solarsystem_nov(ref IEphemeris ephDisp, double tjd, double tdb, Body planet, Origin origin, ref double[] pos, ref double[] vel)
        {
            short rc=0;
            if ((ephDisp == null))
            {
                throw new Exceptions.ValueNotSetException("EphemerisCode:SolarSystem_Nov No emphemeris object supplied");
            }
            else
            {
                ephemeris_nov(ref ephDisp, tdb, BodyType.MajorPlanet, (int)planet, "", origin, ref pos, ref vel);
            }

            return rc;
        }
        
        //Private Function solsys3_nov(ByVal tjd As Double, ByVal body As Body, ByVal origin As Origin, ByRef pos() As Double, ByRef vel() As Double) As Short
        private static short solsys3_nov(double tjd, Body body, Origin origin, ref double[] pos, ref double[] vel)
        {
            int i;
            double[] pm = { 1047.349, 3497.898, 22903, 19412.2 };
            double[] pa = { 5.203363, 9.53707, 19.191264, 30.068963 };
            double[] pl = { 0.60047, 0.871693, 5.466933, 5.32116 };
            double[] pn = { 0.001450138, 0.0005841727, 0.0002047497, 0.0001043891 };
            const double obl = 23.43929111;
            double oblr, qjd, ras=0, decs=0, diss=0, dlon, sinl, cosl, x, y, z, xdot, ydot, zdot, f;
            double[] pos1=new double[3];
            double[][] p = new double[3][] { new double[3], new double[3], new double[3] };
            if (((origin != Origin.Barycentric) & (origin != Origin.Heliocentric)))
                throw new Utilities.Exceptions.InvalidValueException("EphemerisCode.Solsys3 Invalid origin: " + origin);
            if (((tjd < 2340000.5) | (tjd > 2560000.5)))
                throw new Utilities.Exceptions.InvalidValueException("EphemerisCode.Solsys3 Invalid tjd: " + tjd);
            if ((solsys3_tlast == 0))
            {
                oblr = obl * TWOPI / 360;
                solsys3_sine = Math.Sin(oblr);
                solsys3_cose = Math.Cos(oblr);
                solsys3_tmass = 1;
                for (i = 0; i <= 3; i++)
                {
                    solsys3_tmass += (1 / pm[i]);
                }

                solsys3_tlast = 1;
            }

            if (((body == 0) | ((int)body == 1) | ((int)body == 10)))
            {
                for (i = 0; i <= 2; i++)
                {
                    pos[i] = 0;
                    vel[i] = 0;
                }
            }
            else if ((((int)body == 2) | ((int)body == 3)))
            {
                for (i = 0; i <= 2; i++)
                {
                    qjd = tjd + ((double)i - 1) * 0.1;
                    sun_eph_nov(qjd, ras, decs, diss);
                    RADec2Vector(ras, decs, diss, ref pos1);
                    Precession(qjd, pos1, T0, ref pos);
                    p[i][0] = -pos[0];
                    p[i][1] = -pos[1];
                    p[i][2] = -pos[2];
                }

                for (i = 0; i <= 2; i++)
                {
                    pos[i] = p[1][i];
                    vel[i] = (p[2][i] - p[0][i]) / 0.2;
                }
            }
            else
            {
                throw new Utilities.Exceptions.InvalidValueException("EphemerisCode.Solsys3 Invalid body: " + body);
            }

            if ((origin == Origin.Barycentric))
            {
                if (tjd != solsys3_tlast)
                {
                    for (i = 0; i <= 2; i++)
                    {
                        solsys3_pbary[i] = 0;
                        solsys3_vbary[i] = 0;
                    }

                    for (i = 0; i <= 3; i++)
                    {
                        dlon = pl[i] + pn[i] * (tjd - T0);
                        sinl = Math.Sin(dlon);
                        cosl = Math.Cos(dlon);
                        x = pa[i] * cosl;
                        y = pa[i] * sinl * solsys3_cose;
                        z = pa[i] * sinl * solsys3_sine;
                        xdot = -pa[i] * pn[i] * sinl;
                        ydot = pa[i] * pn[i] * cosl * solsys3_cose;
                        zdot = pa[i] * pn[i] * cosl * solsys3_sine;
                        f = 1 / (pm[i] * solsys3_tmass);
                        solsys3_pbary[0] += x * f;
                        solsys3_pbary[1] += y * f;
                        solsys3_pbary[2] += z * f;
                        solsys3_vbary[0] += xdot * f;
                        solsys3_vbary[1] += ydot * f;
                        solsys3_vbary[2] += zdot * f;
                    }

                    solsys3_tlast = tjd;
                }

                for (i = 0; i <= 2; i++)
                {
                    pos[i] -= solsys3_pbary[i];
                    vel[i] -= solsys3_vbary[i];
                }
            }

            return 0;
        }

        private struct sun_con
        {
            internal double l;
            internal double r;
            internal double alpha;
            internal double nu;
            internal sun_con(double pl, double pr, double palpha, double pnu)
            {
                l = pl;
                r = pr;
                alpha = palpha;
                nu = pnu;
            }
        }

        private static void sun_eph_nov(double jd, double ra, double dec, double dis)
        {
            int i;
            double sum_lon = 0;
            double sum_r = 0;
            const double factor = 1E-07;
            double u, arg, lon, lat, t, t2, emean, sin_lon;
            sun_con[] con = { new sun_con(403406, 0, 4.721964, 1.621043), new sun_con(195207, -97597, 5.937458, 62830.348067), new sun_con(119433, -59715, 1.115589, 62830.821524), new sun_con(112392, -56188, 5.781616, 62829.634302), new sun_con(3891, -1556, 5.5474, 125660.5691), new sun_con(2819, -1126, 1.512, 125660.9845), new sun_con(1721, -861, 4.1897, 62832.4766), new sun_con(0, 941, 1.163, 0.813), new sun_con(660, -264, 5.415, 125659.31), new sun_con(350, -163, 4.315, 57533.85), new sun_con(334, 0, 4.553, -33.931), new sun_con(314, 309, 5.198, 777137.715), new sun_con(268, -158, 5.989, 78604.191), new sun_con(242, 0, 2.911, 5.412), new sun_con(234, -54, 1.423, 39302.098), new sun_con(158, 0, 0.061, -34.861), new sun_con(132, -93, 2.317, 115067.698), new sun_con(129, -20, 3.193, 15774.337), new sun_con(114, 0, 2.828, 5296.67), new sun_con(99, -47, 0.52, 58849.27), new sun_con(93, 0, 4.65, 5296.11), new sun_con(86, 0, 4.35, -3980.7), new sun_con(78, -33, 2.75, 52237.69), new sun_con(72, -32, 4.5, 55076.47), new sun_con(68, 0, 3.23, 261.08), new sun_con(64, -10, 1.22, 15773.85), new sun_con(46, -16, 0.14, 188491.03), new sun_con(38, 0, 3.44, -7756.55), new sun_con(37, 0, 4.37, 264.89), new sun_con(32, -24, 1.14, 117906.27), new sun_con(29, -13, 2.84, 55075.75), new sun_con(28, 0, 5.96, -7961.39), new sun_con(27, -9, 5.09, 188489.81), new sun_con(27, 0, 1.72, 2132.19), new sun_con(25, -17, 2.56, 109771.03), new sun_con(24, -11, 1.92, 54868.56), new sun_con(21, 0, 0.09, 25443.93), new sun_con(21, 31, 5.98, -55731.43), new sun_con(20, -10, 4.03, 60697.74), new sun_con(18, 0, 4.27, 2132.79), new sun_con(17, -12, 0.79, 109771.63), new sun_con(14, 0, 4.24, -7752.82), new sun_con(13, -5, 2.01, 188491.91), new sun_con(13, 0, 2.65, 207.81), new sun_con(13, 0, 4.98, 29424.63), new sun_con(12, 0, 0.93, -7.99), new sun_con(10, 0, 2.21, 46941.14), new sun_con(10, 0, 3.59, -68.29), new sun_con(10, 0, 1.5, 21463.25), new sun_con(10, -9, 2.55, 157208.4) };
            u = (jd - T0) / 3652500;
            for (i = 0; i <= 49; i++)
            {
                arg = con[i].alpha + con[i].nu * u;
                sum_lon += con[i].l * Math.Sin(arg);
                sum_r += con[i].r * Math.Cos(arg);
            }

            lon = 4.9353929 + 62833.196168 * u + factor * sum_lon;
            if ((lon < 0))
                lon += TWOPI;
            lat = 0;
            dis = 1.0001026 + factor * sum_r;
            t = u * 100;
            t2 = t * t;
            emean = (0.001813 * t2 * t - 0.00059 * t2 - 46.815 * t + 84381.448) / RAD2SEC;
            sin_lon = Math.Sin(lon);
            ra = Math.Atan2((Math.Cos(emean) * sin_lon), Math.Cos(lon)) * RAD2DEG;
            if ((ra < 0))
                ra += 360;
            ra = ra / 15;
            dec = Math.Asin(Math.Sin(emean) * sin_lon) * RAD2DEG;
        }
    }
}


