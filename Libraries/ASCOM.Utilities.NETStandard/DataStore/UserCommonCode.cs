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
#warning Internal class exposed as public during porting: UserSettings
    //[StandardModule]
    public class UserCommonCode
    {
        private UserSettings RaciUser(string sysName = null,string userName=null)
        {
            SystemHelper sys = new SystemHelper(sysName);
            return sys.GetUser(userName);
        }
        private ProfileNode UserUtilites(string sysName = null,string userName=null)
        {
            SystemHelper sys = new SystemHelper(sysName);
            UserSettings user=RaciUser(userName, sysName);
            return sys.SubNode(user, "Utilities",true);
        }

        public T GetEnum<T>(string sysName,string userName,string p_Name, T p_DefaultValue)
            where T: struct
        {
            T result = p_DefaultValue;
            ProfileNode util = UserUtilites(sysName, userName);
            SystemHelper sys = new SystemHelper(sysName);
            if (!sys.HasValue(util.ProfileNodeId, p_Name))
                sys.SetValueByName(util.ProfileNodeId, p_Name, p_DefaultValue);
            result = sys.ValueEnum<T>(util, p_Name);
            return result;
        }
        public T Get<T>(string sysName, string userName, string p_Name, T p_DefaultValue)
            where T: struct
        {
            SystemHelper sys = new SystemHelper(sysName);
            T result = p_DefaultValue;
            ProfileNode util = UserUtilites(sysName, userName);
            if (util != null)
                result = sys.ValueOrDefault(util.ProfileNodeId, p_Name,p_DefaultValue);
            return result;
        }
        public String Get(string sysName, string userName, string p_Name, String p_DefaultValue)
        {
            SystemHelper sys = new SystemHelper(sysName);
            String result = p_DefaultValue;
            ProfileNode util = UserUtilites(sysName, userName);
            if (util != null)
                result = sys.ValueOrDefault(util.ProfileNodeId, p_Name,p_DefaultValue);
            return result;
        }
        public void Set<T>(string sysName, string userName, string p_Name, T p_Value)
            where T : struct
        {
            SystemHelper sys = new SystemHelper(sysName);
            ProfileNode util = UserUtilites(sysName, userName);
            if (util != null)
                sys.SetValueByName(util.ProfileNodeId, p_Name, p_Value);
        }
        public void Set(string sysName, string userName, string p_Name, string p_Value)
        {
            SystemHelper sys = new SystemHelper(sysName);
            ProfileNode util = UserUtilites(sysName, userName);
            if (util != null)
                sys.SetValueByName(util.ProfileNodeId, p_Name, p_Value);
        }

        public Serial.WaitType GetWaitType(string sysName, string userName, string p_Name, Serial.WaitType p_DefaultValue)
            => GetEnum(sysName, userName, p_Name, p_DefaultValue);
        public Serial.WaitType GetWaitType(string userName, string p_Name, Serial.WaitType p_DefaultValue)
            => GetEnum(null, userName, p_Name, p_DefaultValue);
        public Serial.WaitType GetWaitType(string p_Name, Serial.WaitType p_DefaultValue)
            => GetEnum(null, null, p_Name, p_DefaultValue);

        public bool GetBool(string sysName, string userName, string p_Name, bool p_DefaultValue)
            => Get(sysName, userName, p_Name, p_DefaultValue);
        public bool GetBool(string userName, string p_Name, bool p_DefaultValue)
            => Get(null, userName, p_Name, p_DefaultValue);
        public bool GetBool(string p_Name, bool p_DefaultValue)
            => Get(null, null, p_Name, p_DefaultValue);

        public String GetString(string sysName, string userName, string p_Name, String p_DefaultValue)
            => Get(sysName, userName, p_Name, p_DefaultValue);
        public String GetString(string userName, string p_Name, String p_DefaultValue)
            => Get(null, userName, p_Name, p_DefaultValue);
        public String GetString(string p_Name, String p_DefaultValue)
            => Get(null, null, p_Name, p_DefaultValue);

        public double GetDouble(string sysName, string userName, string p_Name, double p_DefaultValue)
            => Get(sysName, userName, p_Name, p_DefaultValue);
        public double GetDouble(string userName, string p_Name, double p_DefaultValue)
            => Get(null, userName, p_Name, p_DefaultValue);
        public double GetDouble(string p_Name, double p_DefaultValue)
            => Get(null, null, p_Name, p_DefaultValue);


        public DateTime GetDate(string sysName, string userName, string p_Name, DateTime p_DefaultValue)
            => Get(sysName, userName, p_Name, p_DefaultValue);
        public DateTime GetDate(string userName, string p_Name, DateTime p_DefaultValue)
            => Get(null, userName, p_Name, p_DefaultValue);
        public DateTime GetDate(string p_Name, DateTime p_DefaultValue)
            => Get(null, null, p_Name, p_DefaultValue);

        public void SetName(string sysName, string userName, string p_Name, String p_Value)
            => Set(sysName, userName, p_Name, p_Value);
        public void SetName(string userName, string p_Name, String p_Value)
            => Set(null, userName, p_Name, p_Value);
        public void SetName(string p_Name, String p_Value)
            => Set(null, null, p_Name, p_Value);

    }

}
