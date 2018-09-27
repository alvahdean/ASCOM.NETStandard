// Decompiled with JetBrains decompiler
// Type: ASCOM.Utilities.RegistryCommonCode
// Assembly: ASCOM.Utilities, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 06F2ED6E-C559-496D-B7FF-DCA800F2230D
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.Utilities\6.0.0.0__565de7938946fba7\ASCOM.Utilities.dll

//using Microsoft.VisualBasic.CompilerServices;
using Microsoft.Win32;
using System;
using System.IO;

namespace ASCOM.Utilities
{
#warning Internal class exposed as public during porting: RegistryCommonCode
    //[StandardModule]
    public sealed class WindowsRegistryCommonCode
    //internal sealed class RegistryCommonCode
    {
        public static Serial.WaitType GetWaitType(string p_Name, Serial.WaitType p_DefaultValue)
        {
            RegistryKey currentUser = Registry.CurrentUser;
            currentUser.CreateSubKey("Software\\ASCOM\\Utilities");
            RegistryKey registryKey = currentUser.OpenSubKey("Software\\ASCOM\\Utilities", true);
            Serial.WaitType waitType = Serial.WaitType.Sleep;
            try
            {
                if (registryKey.GetValueKind(p_Name) == RegistryValueKind.String)
                    waitType = (Serial.WaitType)Conversions.ToInteger(Enum.Parse(typeof(Serial.WaitType), registryKey.GetValue(p_Name).ToString()));
            }
            catch (IOException ex1)
            {
                //ProjectData.SetProjectError((Exception) ex1);
                try
                {
                    RegistryCommonCode.SetName(p_Name, p_DefaultValue.ToString());
                    waitType = p_DefaultValue;
                }
                catch (Exception ex2)
                {
                    //ProjectData.SetProjectError(ex2);
                    waitType = p_DefaultValue;
                    //ProjectData.ClearProjectError();
                }
                //ProjectData.ClearProjectError();
            }
            catch (Exception ex)
            {
                //ProjectData.SetProjectError(ex);
                waitType = p_DefaultValue;
                //ProjectData.ClearProjectError();
            }
            registryKey.Flush();
            registryKey.Close();
            currentUser.Flush();
            currentUser.Close();
            return waitType;
        }

        public static bool GetBool(string p_Name, bool p_DefaultValue)
        {
            string methSig = $"GetBool({p_Name},{p_DefaultValue})";
            RegistryKey currentUser = Registry.CurrentUser;
            TraceLogger.Debug($"{methSig}: RegistryKey[CurrentUser]='{currentUser?.Name??"NULL"}'");
            currentUser.CreateSubKey("Software\\ASCOM\\Utilities");
            RegistryKey registryKey = currentUser.OpenSubKey("Software\\ASCOM\\Utilities", true);
            TraceLogger.Debug($"{methSig}: RegistryKey[Utilities]='{registryKey?.Name ?? "NULL"}'");
            bool flag = false;
            try
            {
                if (registryKey.GetValueKind(p_Name) == RegistryValueKind.String)
                {
                    string keyValue=registryKey.GetValue(p_Name).ToString();
                    TraceLogger.Debug($"{methSig}: RegistryKey[Utilities.{p_Name}]='{keyValue ?? "NULL"}'");
                    flag = Conversions.ToBoolean(keyValue);
                }
            }
            catch (IOException ex1)
            {
                TraceLogger.Debug($"Exception1: Get RegistryValue: {ex1.GetType().Name}");
                TraceLogger.Debug(ex1.ToString());
                //ProjectData.SetProjectError((Exception) ex1);
                try
                {
                    RegistryCommonCode.SetName(p_Name, p_DefaultValue.ToString());
                    flag = p_DefaultValue;
                }
                catch (Exception ex2)
                {
                    TraceLogger.Debug($"Exception2: SetName: {ex2.GetType().Name}");
                    TraceLogger.Debug(ex2.ToString());
                    //ProjectData.SetProjectError(ex2);
                    flag = p_DefaultValue;
                    //ProjectData.ClearProjectError();
                }
                //ProjectData.ClearProjectError();
            }
            catch (Exception ex)
            {
                TraceLogger.Debug($"Exception: Get RegistryValue: {ex.GetType().Name}");
                TraceLogger.Debug(ex.ToString());
                //ProjectData.SetProjectError(ex);
                flag = p_DefaultValue;
                //ProjectData.ClearProjectError();
            }
            registryKey.Flush();
            registryKey.Close();
            currentUser.Flush();
            currentUser.Close();
            return flag;
        }

        public static string GetString(string p_Name, string p_DefaultValue)
        {
            string str = "";
            RegistryKey currentUser = Registry.CurrentUser;
            currentUser.CreateSubKey("Software\\ASCOM\\Utilities");
            RegistryKey registryKey = currentUser.OpenSubKey("Software\\ASCOM\\Utilities", true);
            try
            {
                if (registryKey.GetValueKind(p_Name) == RegistryValueKind.String)
                    str = registryKey.GetValue(p_Name).ToString();
            }
            catch (IOException ex1)
            {
                //ProjectData.SetProjectError((Exception) ex1);
                try
                {
                    RegistryCommonCode.SetName(p_Name, p_DefaultValue.ToString());
                    str = p_DefaultValue;
                }
                catch (Exception ex2)
                {
                    //ProjectData.SetProjectError(ex2);
                    str = p_DefaultValue;
                    //ProjectData.ClearProjectError();
                }
                //ProjectData.ClearProjectError();
            }
            catch (Exception ex)
            {
                //ProjectData.SetProjectError(ex);
                str = p_DefaultValue;
                //ProjectData.ClearProjectError();
            }
            registryKey.Flush();
            registryKey.Close();
            currentUser.Flush();
            currentUser.Close();
            return str;
        }

        public static double GetDouble(RegistryKey p_Key, string p_Name, double p_DefaultValue)
        {
            RegistryKey currentUser = Registry.CurrentUser;
            currentUser.CreateSubKey("Software\\ASCOM\\Utilities");
            RegistryKey registryKey = currentUser.OpenSubKey("Software\\ASCOM\\Utilities", true);
            double num = default(double);
            try
            {
                if (p_Key.GetValueKind(p_Name) == RegistryValueKind.String)
                    num = Conversions.ToDouble(p_Key.GetValue(p_Name));
            }
            catch (IOException ex1)
            {
                //ProjectData.SetProjectError((Exception) ex1);
                try
                {
                    RegistryCommonCode.SetName(p_Name, p_DefaultValue.ToString());
                    num = p_DefaultValue;
                }
                catch (Exception ex2)
                {
                    //ProjectData.SetProjectError(ex2);
                    num = p_DefaultValue;
                    //ProjectData.ClearProjectError();
                }
                //ProjectData.ClearProjectError();
            }
            catch (Exception ex)
            {
                //ProjectData.SetProjectError(ex);
                num = p_DefaultValue;
                //ProjectData.ClearProjectError();
            }
            registryKey.Flush();
            registryKey.Close();
            currentUser.Flush();
            currentUser.Close();
            return num;
        }

        public static DateTime GetDate(string p_Name, DateTime p_DefaultValue)
        {
            RegistryKey currentUser = Registry.CurrentUser;
            currentUser.CreateSubKey("Software\\ASCOM\\Utilities");
            RegistryKey registryKey = currentUser.OpenSubKey("Software\\ASCOM\\Utilities", true);
            DateTime dateTime = default(DateTime);
            try
            {
                if (registryKey.GetValueKind(p_Name) == RegistryValueKind.String)
                    dateTime = Conversions.ToDate(registryKey.GetValue(p_Name));
            }
            catch (IOException ex1)
            {
                //ProjectData.SetProjectError((Exception) ex1);
                try
                {
                    RegistryCommonCode.SetName(p_Name, p_DefaultValue.ToString());
                    dateTime = p_DefaultValue;
                }
                catch (Exception ex2)
                {
                    //ProjectData.SetProjectError(ex2);
                    dateTime = p_DefaultValue;
                    //ProjectData.ClearProjectError();
                }
                //ProjectData.ClearProjectError();
            }
            catch (Exception ex)
            {
                //ProjectData.SetProjectError(ex);
                dateTime = p_DefaultValue;
                //ProjectData.ClearProjectError();
            }
            registryKey.Flush();
            registryKey.Close();
            currentUser.Flush();
            currentUser.Close();
            return dateTime;
        }

        public static void SetName(string p_Name, string p_Value)
        {
            RegistryKey currentUser = Registry.CurrentUser;
            currentUser.CreateSubKey("Software\\ASCOM\\Utilities");
            RegistryKey registryKey = currentUser.OpenSubKey("Software\\ASCOM\\Utilities", true);
            registryKey.SetValue(p_Name, (object)p_Value.ToString(), RegistryValueKind.String);
            registryKey.Flush();
            registryKey.Close();
            currentUser.Flush();
            currentUser.Close();
        }
    }
}
