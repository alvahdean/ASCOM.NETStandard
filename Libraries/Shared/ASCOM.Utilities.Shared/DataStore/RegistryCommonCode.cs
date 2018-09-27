// Decompiled with JetBrains decompiler
// Type: ASCOM.Utilities.RegistryCommonCode
// Assembly: ASCOM.Utilities, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 06F2ED6E-C559-496D-B7FF-DCA800F2230D
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.Utilities\6.0.0.0__565de7938946fba7\ASCOM.Utilities.dll

//using Microsoft.VisualBasic.CompilerServices;

using ASCOM.Utilities.Interfaces;
using Microsoft.Win32;
using RACI.Data;
using RACI.Settings;
using System;
using System.Collections.Generic;
using System.IO;

namespace ASCOM.Utilities
{
#warning Internal class exposed as public during porting: RegistryCommonCode
    //[StandardModule]
    public sealed class RegistryCommonCode 
    //internal sealed class UserSettings
    {
        static UserCommonCode cbase;
        static RegistryCommonCode()
        {
            cbase = new UserCommonCode();
        }

        public static T GetEnum<T>(string sysName, string userName, string p_Name, T p_DefaultValue) where T : struct
            => cbase.GetEnum(sysName, userName, p_Name, p_DefaultValue);
        public static T Get<T>(string sysName, string userName, string p_Name, T p_DefaultValue) where T : struct
            => cbase.Get<T>(sysName, userName, p_Name, p_DefaultValue);
        public static String Get(string sysName, string userName, string p_Name, String p_DefaultValue)
            => cbase.Get(sysName, userName, p_Name, p_DefaultValue);

        public static void Set<T>(string sysName, string userName, string p_Name, T p_Value) where T : struct
            => cbase.Set(sysName, userName, p_Name, p_Value);
        public static void Set(string sysName, string userName, string p_Name, string p_Value)
            => cbase.Get(sysName, userName, p_Name, p_Value);

        public static Serial.WaitType GetWaitType(string sysName, string userName, string p_Name, Serial.WaitType p_DefaultValue)
            => cbase.GetWaitType(sysName, userName, p_Name, p_DefaultValue);
        public static Serial.WaitType GetWaitType(string userName, string p_Name, Serial.WaitType p_DefaultValue)
                        => cbase.GetWaitType(userName, p_Name, p_DefaultValue);
        public static Serial.WaitType GetWaitType(string p_Name, Serial.WaitType p_DefaultValue)
                                    => cbase.GetWaitType(p_Name, p_DefaultValue);


        public static bool GetBool(string sysName, string userName, string p_Name, bool p_DefaultValue)
            => cbase.GetBool(sysName,userName, p_Name, p_DefaultValue);

        public static bool GetBool(string userName, string p_Name, bool p_DefaultValue)
            => cbase.GetBool(userName, p_Name, p_DefaultValue);
        public static bool GetBool(string p_Name, bool p_DefaultValue)
            => cbase.GetBool(p_Name, p_DefaultValue);

        public static String GetString(string sysName, string userName, string p_Name, String p_DefaultValue)
            => cbase.GetString(sysName, userName, p_Name, p_DefaultValue);
        public static String GetString(string userName, string p_Name, String p_DefaultValue)
                        => cbase.GetString(userName, p_Name, p_DefaultValue);

        public static String GetString(string p_Name, String p_DefaultValue)
                        => cbase.GetString(p_Name, p_DefaultValue);


        public static double GetDouble(string sysName, string userName, string p_Name, double p_DefaultValue)
                        => cbase.GetDouble(sysName, userName, p_Name, p_DefaultValue);

        public static double GetDouble(string userName, string p_Name, double p_DefaultValue)
            => cbase.GetDouble(userName, p_Name, p_DefaultValue);
        public static double GetDouble(string p_Name, double p_DefaultValue)
            => cbase.GetDouble(p_Name, p_DefaultValue);

        public static DateTime GetDate(string sysName, string userName, string p_Name, DateTime p_DefaultValue)
            => cbase.GetDate(sysName, userName, p_Name, p_DefaultValue);
        public static DateTime GetDate(string userName, string p_Name, DateTime p_DefaultValue)
                        => cbase.GetDate(userName, p_Name, p_DefaultValue);
        public static DateTime GetDate(string p_Name, DateTime p_DefaultValue)
                        => cbase.GetDate(p_Name, p_DefaultValue);

        public static void SetName(string sysName, string userName, string p_Name, String p_Value)
                        => cbase.SetName(sysName, userName, p_Name, p_Value);
        public static void SetName(string userName, string p_Name, String p_Value)
                                    => cbase.SetName(userName, p_Name, p_Value);
        public static void SetName(string p_Name, String p_Value)
                                    => cbase.SetName(p_Name, p_Value);
    }

}
