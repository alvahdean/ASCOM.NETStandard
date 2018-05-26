// Decompiled with JetBrains decompiler
// Type: ASCOM.Utilities.RegistryAccess
// Assembly: ASCOM.Utilities, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 06F2ED6E-C559-496D-B7FF-DCA800F2230D
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.Utilities\6.0.0.0__565de7938946fba7\ASCOM.Utilities.dll


using ASCOM.Utilities.Exceptions;
using ASCOM.Utilities.Interfaces;
//using Microsoft.VisualBasic;
using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading;

namespace ASCOM.Utilities
{
    //TODO: Work out how to leave this as internal 
#warning Internal class exposed during porting...
    //internal class RegistryAccess : IAccess, IDisposable
    public class WindowsRegistryAccess : IAscomDataStore
    {
        private RegistryKey ProfileRegKey;
        private Mutex ProfileMutex;
        private bool GotMutex;
        private TraceLogger TL;
        private bool disposedValue;
        private Stopwatch sw;
        private Stopwatch swSupport;
        [SpecialName]
        private int _CopyRegistry__2021128081128081__RecurseDepth;
        [SpecialName]
        private int _Copy55__2031E12814C128081__RecurseDepth;

        public WindowsRegistryAccess()
          : this(false)
        {
        }

        public WindowsRegistryAccess(string p_CallingComponent)
        {
            this.disposedValue = false;
            if (p_CallingComponent.ToUpper() == "UNINSTALLASCOM")
            {
                this.TL = new TraceLogger("", "ProfileMigration");
                this.TL.Enabled = RegistryCommonCode.GetBool("Trace Profile", false);
                VersionCode.RunningVersions(this.TL);
                this.sw = new Stopwatch();
                this.swSupport = new Stopwatch();
                this.ProfileMutex = new Mutex(false, "ASCOMProfileMutex");
                this.ProfileRegKey = null;
            }
            else
                this.NewCode(false);
        }

        public WindowsRegistryAccess(bool p_IgnoreChecks)
        {
            this.disposedValue = false;
            this.NewCode(p_IgnoreChecks);
        }

        public void NewCode(bool p_IgnoreChecks)
        {
            this.TL = new TraceLogger("", "RegistryAccess");
            TraceLogger.Debug($"Creating RegistryAccess object (IgnoreChecks={p_IgnoreChecks})");
            TL.LogMessage("NewCode",$"Creating RegistryAccess object (IgnoreChecks={p_IgnoreChecks})");
            this.TL.Enabled = RegistryCommonCode.GetBool("Trace XMLAccess", false);
            VersionCode.RunningVersions(this.TL);
            this.sw = new Stopwatch();
            this.swSupport = new Stopwatch();
            this.ProfileMutex = new Mutex(false, "ASCOMProfileMutex");
            try
            {
                this.ProfileRegKey = this.OpenSubKey(Registry.LocalMachine, "SOFTWARE\\ASCOM", true, WindowsRegistryAccess.RegWow64Options.KEY_WOW64_32KEY);
                if (ProfileRegKey == null)
                {
                    TL.LogMessage($"RegistryAccess", $"ProfileKey is not initialized!");
                    return;
                }
                TL.LogMessage($"RegistryAccess", $"ProfileKey Path: {ProfileRegKey.Name}");
                TL.LogMessage($"RegistryAccess", $"ProfileKey Valid?: {ProfileRegKey.Handle?.IsInvalid}");
                TL.LogMessage($"RegistryAccess", $"ProfileKey Closed?: {ProfileRegKey.Handle?.IsInvalid}");
                TL.LogMessage($"RegistryAccess", $"ProfileKey  SubKey Count: {ProfileRegKey.SubKeyCount}");
                TL.LogMessage($"RegistryAccess", $"ProfileKey  Value Count: {ProfileRegKey.ValueCount}");

                this.GetProfile("\\", "PlatformVersion");
            }
            catch (Win32Exception ex)
            {
                ////ProjectData.SetProjectError((Exception)ex);
                Win32Exception win32Exception = ex;
                if (p_IgnoreChecks)
                {
                    this.ProfileRegKey = null;
                    ////ProjectData.ClearProjectError();
                }
                else
                {
                    this.TL.LogMessageCrLf("RegistryAccessX.New - Profile not found in registry at HKLM\\SOFTWARE\\ASCOM", win32Exception.ToString());
                    throw new ProfilePersistenceException("RegistryAccessX.New - Profile not found in registry at HKLM\\SOFTWARE\\ASCOM", win32Exception);
                }
            }
            catch (Exception ex)
            {
                ////ProjectData.SetProjectError(ex);
                Exception inner = ex;
                if (p_IgnoreChecks)
                {
                    this.TL.LogMessageCrLf("RegistryAccessX.New IgnoreCheks is true so ignoring exception:", inner.ToString());
                    ////ProjectData.ClearProjectError();
                }
                else
                {
                    this.TL.LogMessageCrLf("RegistryAccessX.New Unexpected exception:", inner.ToString());
                    throw new ProfilePersistenceException("RegistryAccessX.New - Unexpected exception", inner);
                }
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                try
                {
                    this.TL.Enabled = false;
                }
                catch (Exception ex)
                {
                    ////ProjectData.SetProjectError(ex);
                    ////ProjectData.ClearProjectError();
                }
                try
                {
                    this.TL.Dispose();
                }
                catch (Exception ex)
                {
                    ////ProjectData.SetProjectError(ex);
                    ////ProjectData.ClearProjectError();
                }
                try
                {
                    this.TL = null;
                }
                catch (Exception ex)
                {
                    ////ProjectData.SetProjectError(ex);
                    ////ProjectData.ClearProjectError();
                }
                try
                {
                    this.sw.Stop();
                }
                catch (Exception ex)
                {
                    ////ProjectData.SetProjectError(ex);
                    ////ProjectData.ClearProjectError();
                }
                try
                {
                    this.sw = null;
                }
                catch (Exception ex)
                {
                    ////ProjectData.SetProjectError(ex);
                    ////ProjectData.ClearProjectError();
                }
                try
                {
                    this.swSupport.Stop();
                }
                catch (Exception ex)
                {
                    ////ProjectData.SetProjectError(ex);
                    ////ProjectData.ClearProjectError();
                }
                try
                {
                    this.swSupport = null;
                }
                catch (Exception ex)
                {
                    ////ProjectData.SetProjectError(ex);
                    ////ProjectData.ClearProjectError();
                }
                try
                {
                    this.ProfileMutex.Close();
                }
                catch (Exception ex)
                {
                    ////ProjectData.SetProjectError(ex);
                    ////ProjectData.ClearProjectError();
                }
                try
                {
                    this.ProfileMutex = null;
                }
                catch (Exception ex)
                {
                    ////ProjectData.SetProjectError(ex);
                    ////ProjectData.ClearProjectError();
                }
                try
                {
                    this.ProfileRegKey.Close();
                }
                catch (Exception ex)
                {
                    ////ProjectData.SetProjectError(ex);
                    ////ProjectData.ClearProjectError();
                }
                try
                {
                    this.ProfileRegKey = null;
                }
                catch (Exception ex)
                {
                    ////ProjectData.SetProjectError(ex);
                    ////ProjectData.ClearProjectError();
                }
            }
            this.disposedValue = true;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void CreateKey(string p_SubKeyName)
        {
            TraceLogger.Debug($"Creating Registry Subkey: '{p_SubKeyName}'");
            TL.LogMessageCrLf("CreateKey",$"Creating Registry Subkey: '{p_SubKeyName}'");
            try
            {
                this.GetProfileMutex("CreateKey", p_SubKeyName);
                this.sw.Reset();
                this.sw.Start();
                this.TL.LogMessage("CreateKey", "SubKey: \"" + p_SubKeyName + "\"");
                p_SubKeyName = Strings.Trim(p_SubKeyName);
                if ((p_SubKeyName != ""))
                {
                    this.ProfileRegKey.CreateSubKey(this.CleanSubKey(p_SubKeyName));
                    this.ProfileRegKey.Flush();
                }
                this.sw.Stop();
                this.TL.LogMessage("  ElapsedTime", "  " + this.sw.ElapsedMilliseconds.ToString() + " milliseconds");
            }
            finally
            {
                this.ProfileMutex.ReleaseMutex();
            }
        }

        public void DeleteKey(string p_SubKeyName)
        {
            try
            {
                this.GetProfileMutex("DeleteKey", p_SubKeyName);
                this.sw.Reset();
                this.sw.Start();
                this.TL.LogMessage("DeleteKey", "SubKey: \"" + p_SubKeyName + "\"");
                try
                {
                    this.ProfileRegKey.DeleteSubKeyTree(this.CleanSubKey(p_SubKeyName));
                    this.ProfileRegKey.Flush();
                }
                catch (Exception ex)
                {
                    ////ProjectData.SetProjectError(ex);
                    ////ProjectData.ClearProjectError();
                }
                this.sw.Stop();
                this.TL.LogMessage("  ElapsedTime", "  " + this.sw.ElapsedMilliseconds.ToString() + " milliseconds");
            }
            finally
            {
                this.ProfileMutex.ReleaseMutex();
            }
        }

        public void RenameKey(string OriginalSubKeyName, string NewSubKeyName)
        {
            if (this.ProfileRegKey.OpenSubKey(this.CleanSubKey(NewSubKeyName)) != null)
                throw new ProfilePersistenceException("Key " + NewSubKeyName + " already exists");
            this.CreateKey(NewSubKeyName);
            SortedList<string, string> sortedList = this.EnumProfile(OriginalSubKeyName);
            IEnumerator<System.Collections.Generic.KeyValuePair<string, string>> enumerator = null;
            try
            {
                enumerator = sortedList.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    System.Collections.Generic.KeyValuePair<string, string> current = enumerator.Current;
                    this.WriteProfile(NewSubKeyName, current.Key, current.Value);
                }
            }
            finally
            {
                if (enumerator != null)
                    enumerator.Dispose();
            }
            this.DeleteKey(OriginalSubKeyName);
        }

        public void DeleteProfile(string p_SubKeyName, string p_ValueName)
        {
            try
            {
                this.GetProfileMutex("DeleteProfile", p_SubKeyName + " " + p_ValueName);
                this.sw.Reset();
                this.sw.Start();
                this.TL.LogMessage("DeleteProfile", "SubKey: \"" + p_SubKeyName + "\" Name: \"" + p_ValueName + "\"");
                try
                {
                    this.ProfileRegKey.OpenSubKey(this.CleanSubKey(p_SubKeyName), true).DeleteValue(p_ValueName);
                    this.ProfileRegKey.Flush();
                }
                catch (Exception ex)
                {
                    ////ProjectData.SetProjectError(ex);
                    this.TL.LogMessage("DeleteProfile", "  Value did not exist");
                    ////ProjectData.ClearProjectError();
                }
                this.sw.Stop();
                this.TL.LogMessage("  ElapsedTime", "  " + this.sw.ElapsedMilliseconds.ToString() + " milliseconds");
            }
            finally
            {
                this.ReleaseProfileMutex("DeleteProfile");
            }
        }

        public SortedList<string, string> EnumKeys(string p_SubKeyName)
        {
            SortedList<string, string> sortedList = new SortedList<string, string>();
            TraceLogger.Debug($"Enumerating keys @ [\\{p_SubKeyName}]...");
            TL.LogMessageCrLf("EnumKeys",$"Enumerating keys @ [\\{p_SubKeyName}]...");

            try
            {
                this.GetProfileMutex("EnumKeys", p_SubKeyName);
                this.sw.Reset();
                this.sw.Start();
                this.TL.LogMessage("EnumKeys", "SubKey: \"" + p_SubKeyName + "\"");
                string[] subKeyNames = this.ProfileRegKey.OpenSubKey(this.CleanSubKey(p_SubKeyName)).GetSubKeyNames();
                int index = 0;
                while (index < subKeyNames.Length)
                {
                    string str1 = subKeyNames[index];
                    try
                    {
                        string Left = p_SubKeyName;
                        string str2 = (Left == "") || (Left == "\\") ? this.ProfileRegKey.OpenSubKey(this.CleanSubKey(str1)).GetValue("", "").ToString() : this.ProfileRegKey.OpenSubKey(this.CleanSubKey(p_SubKeyName) + "\\" + str1).GetValue("", "").ToString();
                        sortedList.Add(str1, str2);
                    }
                    catch (Exception ex)
                    {
                        ////ProjectData.SetProjectError(ex);
                        Exception inner = ex;
                        this.TL.LogMessageCrLf("", "Read exception: " + inner.ToString());
                        throw new ProfilePersistenceException("RegistryAccessX.EnumKeys exception", inner);
                    }
                    checked { ++index; }
                }
                this.sw.Stop();
                this.TL.LogMessage("  ElapsedTime", "  " + this.sw.ElapsedMilliseconds.ToString() + " milliseconds");
            }
            finally
            {
                this.ReleaseProfileMutex("EnumKeys");
            }
            IEnumerator<System.Collections.Generic.KeyValuePair<string, string>> enumerator = null;
            try
            {
                enumerator = sortedList.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    System.Collections.Generic.KeyValuePair<string, string> current = enumerator.Current;
                    this.TL.LogMessage("", "Found: " + current.Key + " " + current.Value);
                }
            }
            finally
            {
                if (enumerator != null)
                    enumerator.Dispose();
            }
            return sortedList;
        }

        public SortedList<string, string> EnumProfile(string p_SubKeyName)
        {
            SortedList<string, string> sortedList = new SortedList<string, string>();
            try
            {
                this.GetProfileMutex("EnumProfile", p_SubKeyName);
                this.sw.Reset();
                this.sw.Start();
                this.TL.LogMessage("EnumProfile", "SubKey: \"" + p_SubKeyName + "\"");
                string[] valueNames = this.ProfileRegKey.OpenSubKey(this.CleanSubKey(p_SubKeyName)).GetValueNames();
                int index = 0;
                while (index < valueNames.Length)
                {
                    string str = valueNames[index];
                    sortedList.Add(str, this.ProfileRegKey.OpenSubKey(this.CleanSubKey(p_SubKeyName)).GetValue(str).ToString());
                    checked { ++index; }
                }
                this.sw.Stop();
                this.TL.LogMessage("  ElapsedTime", "  " + this.sw.ElapsedMilliseconds.ToString() + " milliseconds");
            }
            finally
            {
                this.ReleaseProfileMutex("EnumProfile");
            }
            return sortedList;
        }

        public string GetProfile(string p_SubKeyName, string p_ValueName, string p_DefaultValue)
        {
            string str;
            try
            {
                this.GetProfileMutex("GetProfile", p_SubKeyName + " " + p_ValueName + " " + p_DefaultValue);
                this.sw.Reset();
                this.sw.Start();
                this.TL.LogMessage("GetProfile", "SubKey: \"" + p_SubKeyName + "\" Name: \"" + p_ValueName + "\"\" DefaultValue: \"" + p_DefaultValue + "\"");
                this.TL.LogMessage("  DefaultValue", "is nothing... " + (p_DefaultValue == null).ToString());
                str = "";
                try
                {
                    str = this.ProfileRegKey.OpenSubKey(this.CleanSubKey(p_SubKeyName)).GetValue(p_ValueName).ToString();
                    this.TL.LogMessage("  Value", "\"" + str + "\"");
                }
                catch (NullReferenceException ex)
                {
                    ////ProjectData.SetProjectError((Exception)ex);
                    if (p_DefaultValue != null)
                    {
                        this.WriteProfile(p_SubKeyName, p_ValueName, p_DefaultValue);
                        str = p_DefaultValue;
                        this.TL.LogMessage("  Value", "Value not yet set, returning supplied default value: " + p_DefaultValue);
                    }
                    else
                        this.TL.LogMessage("  Value", "Value not yet set and no default value supplied, returning null string");
                    ////ProjectData.ClearProjectError();
                }
                catch (Exception ex)
                {
                    ////ProjectData.SetProjectError(ex);
                    Exception inner = ex;
                    if (p_DefaultValue != null)
                    {
                        this.WriteProfile(this.CleanSubKey(p_SubKeyName), this.CleanSubKey(p_ValueName), p_DefaultValue);
                        str = p_DefaultValue;
                        this.TL.LogMessage("  Value", "Key not yet set, returning supplied default value: " + p_DefaultValue);
                        ////ProjectData.ClearProjectError();
                    }
                    else
                    {
                        this.TL.LogMessageCrLf("  Value", "Key not yet set and no default value supplied, throwing exception: " + inner.ToString());
                        throw new ProfilePersistenceException("GetProfile Exception", inner);
                    }
                }
                this.sw.Stop();
                this.TL.LogMessage("  ElapsedTime", "  " + this.sw.ElapsedMilliseconds.ToString() + " milliseconds");
            }
            finally
            {
                this.ReleaseProfileMutex("GetProfile");
            }
            return str;
        }

        public string GetProfile(string p_SubKeyName, string p_ValueName)
        {
            return this.GetProfile(p_SubKeyName, p_ValueName, null);
        }

        public void WriteProfile(string p_SubKeyName, string p_ValueName, string p_ValueData)
        {
            try
            {
                this.GetProfileMutex("WriteProfile", p_SubKeyName + " " + p_ValueName + " " + p_ValueData);
                this.sw.Reset();
                this.sw.Start();
                this.TL.LogMessage("WriteProfile", "SubKey: \"" + p_SubKeyName + "\" Name: \"" + p_ValueName + "\" Value: \"" + p_ValueData + "\"");
                if (p_SubKeyName == "")
                    this.ProfileRegKey.SetValue(p_ValueName, p_ValueData, RegistryValueKind.String);
                else
                    this.ProfileRegKey.CreateSubKey(this.CleanSubKey(p_SubKeyName)).SetValue(p_ValueName, p_ValueData, RegistryValueKind.String);
                this.ProfileRegKey.Flush();
                this.sw.Stop();
                this.TL.LogMessage("  ElapsedTime", "  " + this.sw.ElapsedMilliseconds.ToString() + " milliseconds");
            }
            catch (Exception ex)
            {
                ////ProjectData.SetProjectError(ex);
                Exception inner = ex;
                this.TL.LogMessageCrLf("WriteProfile", "Exception: " + inner.ToString());
                throw new ProfilePersistenceException("RegistryAccessX.WriteProfile exception", inner);
            }
            finally
            {
                this.ReleaseProfileMutex("WriteProfile");
            }
        }

        public void BackupProfile(string CurrentPlatformVersion)
        {
            try
            {
                this.GetProfileMutex("BackupProfile", "");
                this.sw.Reset();
                this.sw.Start();
                this.LogMessage("BackupProfile", "From platform: " + CurrentPlatformVersion + ", OS: " + Enum.GetName(typeof(VersionCode.Bitness), VersionCode.OSBits()));
                string Left = CurrentPlatformVersion;
                if (Left == "")
                    this.LogMessage("BackupProfile", "New installation so nothing to migrate");
                else if ((Left == "4") || (Left == "5"))
                {
                    this.LogMessage("BackupProfile", "Backing up Platform 5 Profile" + CurrentPlatformVersion);
                    this.Backup50();
                }
                else if ((Left == "5.5"))
                {
                    this.Backup50();
                    this.Backup55();
                }
                else
                    this.LogMessage("BackupProfile", "Profile reports previous Platform as " + CurrentPlatformVersion + " - no migration required");
                this.sw.Stop();
                this.LogMessage("  ElapsedTime", "  " + this.sw.ElapsedMilliseconds.ToString() + " milliseconds");
            }
            catch (Exception ex)
            {
                ////ProjectData.SetProjectError(ex);
                Exception inner = ex;
                this.LogError("BackupProfile", "Exception: " + inner.ToString());
                throw new ProfilePersistenceException("RegistryAccessX.BackupProfile exception", inner);
            }
            finally
            {
                this.ReleaseProfileMutex("BackupProfile");
            }
        }

        internal void RestoreProfile(string CurrentPlatformVersion)
        {
            try
            {
                this.GetProfileMutex("RestoreProfile", "");
                this.sw.Reset();
                this.sw.Start();
                this.LogMessage("RestoreProfile", "From platform: " + CurrentPlatformVersion + ", OS: " + Enum.GetName(typeof(VersionCode.Bitness), VersionCode.OSBits()));
                string Left = CurrentPlatformVersion;
                if (Left == "")
                    this.LogMessage("RestoreProfile", "New installation so nothing to migrate");
                else if (Left == "4" || Left == "5")
                {
                    this.LogMessage("RestoreProfile", "Restoring Platform 5 Profile" + CurrentPlatformVersion);
                    this.Restore50();
                }
                else if (Left == "5.5")
                    this.Restore55();
                else
                    this.LogMessage("RestoreProfile", "Profile reports previous Platform as " + CurrentPlatformVersion + " - no migration required");
                this.ProfileRegKey = this.OpenSubKey(Registry.LocalMachine, "SOFTWARE\\ASCOM", true, WindowsRegistryAccess.RegWow64Options.KEY_WOW64_32KEY);
                this.sw.Stop();
                this.LogMessage("  ElapsedTime", "  " + this.sw.ElapsedMilliseconds.ToString() + " milliseconds");
            }
            catch (Exception ex)
            {
                ////ProjectData.SetProjectError(ex);
                Exception inner = ex;
                this.LogError("RestoreProfile", "Exception: " + inner.ToString());
                throw new ProfilePersistenceException("RegistryAccessX.BackupProfile exception", inner);
            }
            finally
            {
                this.ReleaseProfileMutex("RestoreProfile");
            }
        }

        public IASCOMProfile GetProfile(string p_SubKeyName)
        {
            ASCOMProfile ProfileContents = new ASCOMProfile();
            try
            {
                this.GetProfileMutex("GetProfile", p_SubKeyName);
                this.sw.Reset();
                this.sw.Start();
                this.TL.LogMessage("GetProfile", "SubKey: \"" + p_SubKeyName + "\"");
                this.GetSubKey(p_SubKeyName, "", ref ProfileContents);
                this.sw.Stop();
                this.TL.LogMessage("  ElapsedTime", "  " + this.sw.ElapsedMilliseconds.ToString() + " milliseconds ");
            }
            finally
            {
                this.ReleaseProfileMutex("GetProfile");
            }
            return ProfileContents;
        }

        public void SetProfile(string p_SubKeyName, IASCOMProfile p_ProfileKey)
        {
            try
            {
                this.GetProfileMutex("SetProfile", p_SubKeyName);
                this.sw.Reset();
                this.sw.Start();
                this.TL.LogMessage("SetProfile", "SubKey: \"" + p_SubKeyName + "\"");
                IEnumerator<string> enumerator1 = null;
                try
                {
                    enumerator1 = p_ProfileKey.ProfileValues.Keys.GetEnumerator();
                    while (enumerator1.MoveNext())
                    {
                        string current1 = enumerator1.Current;
                        this.TL.LogMessage("SetProfile", "Received SubKey: " + current1);
                        IEnumerator<string> enumerator2 = null;
                        try
                        {
                            enumerator2 = p_ProfileKey.ProfileValues[current1].Keys.GetEnumerator();
                            while (enumerator2.MoveNext())
                            {
                                string current2 = enumerator2.Current;
                                this.TL.LogMessage("SetProfile", "  Received value: " + current2 + " = " + p_ProfileKey.ProfileValues[current1][current2]);
                            }
                        }
                        finally
                        {
                            if (enumerator2 != null)
                                enumerator2.Dispose();
                        }
                    }
                }
                finally
                {
                    if (enumerator1 != null)
                        enumerator1.Dispose();
                }
                IEnumerator<string> enumerator3 = null;
                try
                {
                    enumerator3 = p_ProfileKey.ProfileValues.Keys.GetEnumerator();
                    while (enumerator3.MoveNext())
                    {
                        string current1 = enumerator3.Current;
                        RegistryKey registryKey = (p_SubKeyName != "") ? ((current1 != "") ? this.ProfileRegKey.CreateSubKey(p_SubKeyName + "\\" + this.CleanSubKey(current1)) : this.ProfileRegKey.CreateSubKey(p_SubKeyName)) : ((current1 != "") ? this.ProfileRegKey.CreateSubKey(this.CleanSubKey(current1)) : this.ProfileRegKey);
                        IEnumerator<string> enumerator2 = null;
                        try
                        {
                            enumerator2 = p_ProfileKey.ProfileValues[current1].Keys.GetEnumerator();
                            while (enumerator2.MoveNext())
                            {
                                string current2 = enumerator2.Current;
                                registryKey.SetValue(current2, p_ProfileKey.ProfileValues[current1][current2], RegistryValueKind.String);
                            }
                        }
                        finally
                        {
                            if (enumerator2 != null)
                                enumerator2.Dispose();
                        }
                        registryKey.Flush();
                    }
                }
                finally
                {
                    if (enumerator3 != null)
                        enumerator3.Dispose();
                }
                this.ProfileRegKey.Flush();
                this.sw.Stop();
                this.TL.LogMessage("  ElapsedTime", "  " + this.sw.ElapsedMilliseconds.ToString() + " milliseconds ");
            }
            finally
            {
                this.ReleaseProfileMutex("SetProfile");
            }
        }

        private void LogMessage(string section, string logMessage)
        {
            this.TL.LogMessageCrLf(section, logMessage);
            EventLogCode.LogEvent(section, logMessage, EventLogEntryType.Information, GlobalConstants.EventLogErrors.MigrateProfileRegistryKey, "");
        }

        private void LogError(string section, string logMessage)
        {
            this.TL.LogMessageCrLf(section, logMessage);
            EventLogCode.LogEvent(section, "Exception", EventLogEntryType.Error, GlobalConstants.EventLogErrors.MigrateProfileRegistryKey, logMessage);
        }

        private void GetSubKey(string BaseSubKey, string SubKeyOffset, ref ASCOMProfile ProfileContents)
        {
            ASCOMProfile ascomProfile = new ASCOMProfile();
            BaseSubKey = this.CleanSubKey(BaseSubKey);
            SubKeyOffset = this.CleanSubKey(SubKeyOffset);
            RegistryKey registryKey = (BaseSubKey != "") ? ((SubKeyOffset != "") ? this.ProfileRegKey.OpenSubKey(BaseSubKey + "\\" + SubKeyOffset) : this.ProfileRegKey.OpenSubKey(BaseSubKey)) : ((SubKeyOffset != "") ? this.ProfileRegKey.OpenSubKey(SubKeyOffset) : this.ProfileRegKey);
            string[] valueNames = registryKey.GetValueNames();
            if (((IEnumerable<string>)valueNames).Count<string>() == 0)
            {
                ProfileContents.SetValue("", "", SubKeyOffset);
            }
            else
            {
                string[] strArray = valueNames;
                int index = 0;
                while (index < strArray.Length)
                {
                    string str1 = strArray[index];
                    string str2 = registryKey.GetValue(str1).ToString();
                    ProfileContents.SetValue(str1, str2, SubKeyOffset);
                    checked { ++index; }
                }
            }
            string[] subKeyNames = registryKey.GetSubKeyNames();
            int index1 = 0;
            while (index1 < subKeyNames.Length)
            {
                string SubKey = subKeyNames[index1];
                this.GetSubKey(BaseSubKey, SubKeyOffset + "\\" + this.CleanSubKey(SubKey), ref ProfileContents);
                checked { ++index1; }
            }
        }

        private string CleanSubKey(string SubKey)
        {
            TraceLogger.Debug($"Cleaning Registry Subkey: '{SubKey}'");
            TL.LogMessageCrLf("CleanSubKey",$"Cleaning Registry Subkey: '{SubKey}'");
            if (SubKey.StartsWith("\\"))
                return Strings.Mid(SubKey, 2);
            return SubKey;
        }

        private void CopyRegistry(RegistryKey FromKey, RegistryKey ToKey)
        {
            this._CopyRegistry__2021128081128081__RecurseDepth = checked(this._CopyRegistry__2021128081128081__RecurseDepth + 1);
            this.LogMessage("CopyRegistry " + this._CopyRegistry__2021128081128081__RecurseDepth.ToString(), "Copy from: " + FromKey.Name + " to: " + ToKey.Name + " Number of values: " + FromKey.ValueCount.ToString() + ", number of subkeys: " + FromKey.SubKeyCount.ToString());
            string[] valueNames = FromKey.GetValueNames();
            int index1 = 0;
            while (index1 < valueNames.Length)
            {
                string name = valueNames[index1];
                string str = FromKey.GetValue(name, "").ToString();
                ToKey.SetValue(name, str);
                this.LogMessage("CopyRegistry", ToKey.Name + " - \"" + name + "\"  \"" + str + "\"");
                checked { ++index1; }
            }
            string[] subKeyNames = FromKey.GetSubKeyNames();
            int index2 = 0;
            while (index2 < subKeyNames.Length)
            {
                string str = subKeyNames[index2];
                string Left = FromKey.OpenSubKey(str).GetValue("", "").ToString();
                RegistryKey FromKey1 = FromKey.OpenSubKey(str);
                RegistryKey subKey = ToKey.CreateSubKey(str);
                if ((Left != ""))
                    subKey.SetValue("", Left);
                this.CopyRegistry(FromKey1, subKey);
                checked { ++index2; }
            }
            this._CopyRegistry__2021128081128081__RecurseDepth = checked(this._CopyRegistry__2021128081128081__RecurseDepth - 1);
        }

        private void Backup50()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            RegistryKey FromKey = this.OpenSubKey(Registry.LocalMachine, "SOFTWARE\\ASCOM", false, WindowsRegistryAccess.RegWow64Options.KEY_WOW64_32KEY);
            RegistryKey subKey = Registry.CurrentUser.CreateSubKey("SOFTWARE\\ASCOM\\Platform5Original");
            if (string.IsNullOrEmpty(subKey.GetValue("PlatformVersion", "").ToString()))
            {
                this.LogMessage("Backup50", "Backup PlatformVersion not found, backing up Profile 5 to Platform5Original");
                this.CopyRegistry(FromKey, subKey);
            }
            else
                this.LogMessage("Backup50", "Platform 5 backup found at HKCU\\SOFTWARE\\ASCOM\\Platform5Original, no further action taken.");
            FromKey.Close();
            subKey.Close();
            stopwatch.Stop();
            this.LogMessage("Backup50", "ElapsedTime " + stopwatch.ElapsedMilliseconds.ToString() + " milliseconds");
        }

        internal void ListRegistryACLs(RegistryKey Key, string Description)
        {
            this.LogMessage("ListRegistryACLs", Description + ", Key: " + Key.Name);
            AuthorizationRuleCollection accessRules = Key.GetAccessControl().GetAccessRules(true, true, typeof(NTAccount));
            try
            {
                foreach (RegistryAccessRule registryAccessRule in accessRules)
                    this.LogMessage("ListRegistryACLs",
                        registryAccessRule.AccessControlType.ToString() + " " +
                        registryAccessRule.IdentityReference.ToString() + " " +
                        ((WindowsRegistryAccess.AccessRights)registryAccessRule.RegistryRights).ToString() + " " +
                        (registryAccessRule.IsInherited ? "Inherited" : "NotInherited " + registryAccessRule.InheritanceFlags.ToString() + " " + registryAccessRule.PropagationFlags.ToString())
                        );
                //Interaction.IIf(registryAccessRule.IsInherited, (object)"Inherited", (object)"NotInherited").ToString() + " " + registryAccessRule.InheritanceFlags.ToString() + " " + registryAccessRule.PropagationFlags.ToString());
            }
            finally
            {
                //#CODESMELL
                //IEnumerator enumerator;
                //if (enumerator is IDisposable)
                //    (enumerator as IDisposable).Dispose();
            }
            this.TL.BlankLine();
        }

        internal void SetRegistryACL()
        {
            SortedList<string, string> sortedList = new SortedList<string, string>();
            Stopwatch stopwatch = Stopwatch.StartNew();
            this.LogMessage("SetRegistryACL", "Creating security identifier");
            SecurityIdentifier securityIdentifier = new SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, new SecurityIdentifier("S-1-0-0"));
            this.LogMessage("SetRegistryACL", "Creating FullControl ACL rule");
            RegistryAccessRule rule = new RegistryAccessRule(securityIdentifier, RegistryRights.FullControl, InheritanceFlags.ContainerInherit, PropagationFlags.None, AccessControlType.Allow);
            this.TL.LogMessage("SetRegistryACL", "Listing base key ACLs");
            if (VersionCode.ApplicationBits() == VersionCode.Bitness.Bits64)
            {
                this.TL.LogMessage("SetRegistryACL", "Listing base key ACLs in 64bit mode");
                this.ListRegistryACLs(Registry.ClassesRoot, "HKEY_CLASSES_ROOT");
                this.ListRegistryACLs(Registry.ClassesRoot.OpenSubKey("SOFTWARE"), "HKEY_CLASSES_ROOT\\SOFTWARE");
                this.ListRegistryACLs(Registry.ClassesRoot.OpenSubKey("SOFTWARE\\Microsoft"), "HKEY_CLASSES_ROOT\\SOFTWARE\\Microsoft");
                this.ListRegistryACLs(this.OpenSubKey(Registry.LocalMachine, "SOFTWARE", true, WindowsRegistryAccess.RegWow64Options.KEY_WOW64_64KEY), "HKEY_CLASSES_ROOT\\SOFTWARE\\Wow6432Node");
                this.ListRegistryACLs(this.OpenSubKey(Registry.LocalMachine, "SOFTWARE\\Microsoft", true, WindowsRegistryAccess.RegWow64Options.KEY_WOW64_32KEY), "HKEY_CLASSES_ROOT\\SOFTWARE\\Wow6432Node\\Microsoft");
            }
            else
            {
                this.TL.LogMessage("SetRegistryACL", "Listing base key ACLS in 32bit mode");
                this.ListRegistryACLs(Registry.ClassesRoot, "HKEY_CLASSES_ROOT");
                this.ListRegistryACLs(Registry.ClassesRoot.OpenSubKey("SOFTWARE"), "HKEY_CLASSES_ROOT\\SOFTWARE");
                this.ListRegistryACLs(Registry.ClassesRoot.OpenSubKey("SOFTWARE\\Microsoft"), "HKEY_CLASSES_ROOT\\SOFTWARE\\Microsoft");
            }
            this.LogMessage("SetRegistryACL", "Creating root ASCOM key \"\\\"");
            RegistryKey registryKey = this.OpenSubKey(Registry.LocalMachine, "SOFTWARE\\ASCOM", true, WindowsRegistryAccess.RegWow64Options.KEY_WOW64_32KEY);
            this.LogMessage("SetRegistryACL", "Retrieving ASCOM key ACL rule");
            this.TL.BlankLine();
            RegistrySecurity accessControl = registryKey.GetAccessControl();
            AuthorizationRuleCollection accessRules1 = accessControl.GetAccessRules(true, true, typeof(NTAccount));
            try
            {
                foreach (RegistryAccessRule registryAccessRule in accessRules1)
                    this.LogMessage("SetRegistryACL Before", registryAccessRule.AccessControlType.ToString() + " " + registryAccessRule.IdentityReference.ToString() + " " + ((WindowsRegistryAccess.AccessRights)registryAccessRule.RegistryRights).ToString() + " "
                        + (registryAccessRule.IsInherited ? "Inherited" : "NotInherited").ToString() + " " + registryAccessRule.InheritanceFlags.ToString() + " " + registryAccessRule.PropagationFlags.ToString());
            }
            finally
            {
                //CODESMELL
                //IEnumerator enumerator;
                //if (enumerator is IDisposable)
                //    (enumerator as IDisposable).Dispose();
            }
            this.TL.BlankLine();
            if (accessControl.AreAccessRulesCanonical)
            {
                this.TL.LogMessage("SetRegistryACL", "Current access rules on the ASCOM profile key are canonical, no fix-up action required");
            }
            else
            {
                this.TL.LogMessage("SetRegistryACL", "***** Current access rules on the ASCOM profile key are NOT canonical, fixing them");
                this.CanonicalizeDacl(accessControl);
                this.TL.LogMessage("SetRegistryACL", "Are Access Rules Canonical after fix: " + accessControl.AreAccessRulesCanonical.ToString());
            }
            this.TL.BlankLine();
            this.LogMessage("SetRegistryACL", "Adding new ACL rule");
            accessControl.AddAccessRule(rule);
            this.TL.LogMessage("SetRegistryACL", "Are Access Rules Canonical after adding full access rule: " + accessControl.AreAccessRulesCanonical.ToString());
            this.TL.BlankLine();
            AuthorizationRuleCollection accessRules2 = accessControl.GetAccessRules(true, true, typeof(NTAccount));
            try
            {
                foreach (RegistryAccessRule registryAccessRule in accessRules2)
                    this.LogMessage("SetRegistryACL After", registryAccessRule.AccessControlType.ToString() + " " + registryAccessRule.IdentityReference.ToString() + " " + ((WindowsRegistryAccess.AccessRights)registryAccessRule.RegistryRights).ToString() + " "
                        + (registryAccessRule.IsInherited ? "Inherited" : "NotInherited") + " " + registryAccessRule.InheritanceFlags.ToString() + " " + registryAccessRule.PropagationFlags.ToString());
            }
            finally
            {
                //CODESMELL
                //IEnumerator enumerator;
                //if (enumerator is IDisposable)
                //    (enumerator as IDisposable).Dispose();
            }
            this.TL.BlankLine();
            this.LogMessage("SetRegistryACL", "Applying new ACL rule to the Profile key");
            registryKey.SetAccessControl(accessControl);
            this.LogMessage("SetRegistryACL", "Flushing key");
            registryKey.Flush();
            this.LogMessage("SetRegistryACL", "Closing key");
            registryKey.Close();
            stopwatch.Stop();
            this.LogMessage("SetRegistryACL", "ElapsedTime " + stopwatch.ElapsedMilliseconds.ToString() + " milliseconds");
        }

        internal void CanonicalizeDacl(NativeObjectSecurity objectSecurity)
        {
            RawSecurityDescriptor securityDescriptor = new RawSecurityDescriptor(objectSecurity.GetSecurityDescriptorSddlForm(AccessControlSections.Access));
            List<CommonAce> commonAceList1 = new List<CommonAce>();
            List<CommonAce> commonAceList2 = new List<CommonAce>();
            List<CommonAce> commonAceList3 = new List<CommonAce>();
            List<CommonAce> commonAceList4 = new List<CommonAce>();
            List<CommonAce> commonAceList5 = new List<CommonAce>();
            int index = 0;
            RawAcl rawAcl = new RawAcl(securityDescriptor.DiscretionaryAcl.Revision, securityDescriptor.DiscretionaryAcl.Count);
            if (objectSecurity == null)
                throw new ArgumentNullException("objectSecurity");
            if (objectSecurity.AreAccessRulesCanonical)
            {
                this.TL.LogMessage("CanonicalizeDacl", "Rules are already canonical, no action taken");
            }
            else
            {
                this.TL.BlankLine();
                this.TL.LogMessage("CanonicalizeDacl", "***** Rules are not canonical, restructuring them *****");
                this.TL.BlankLine();
                foreach (CommonAce commonAce in securityDescriptor.DiscretionaryAcl)
                {
                    if ((commonAce.AceFlags & AceFlags.Inherited) == AceFlags.Inherited)
                    {
                        commonAceList3.Add(commonAce);
                        this.TL.LogMessage("CanonicalizeDacl", "Found Inherited Ace,                  "
                            + (commonAce.AceType == AceType.AccessAllowed ? (object)"Allow" : (object)"Deny") + ": " + commonAce.SecurityIdentifier.Translate(Type.GetType("System.Security.Principal.NTAccount")).ToString() + " " + ((WindowsRegistryAccess.AccessRights)commonAce.AccessMask).ToString().ToString() + " " + commonAce.AceFlags.ToString());
                    }
                    else
                    {
                        switch (commonAce.AceType)
                        {
                            case AceType.AccessAllowed:
                                commonAceList4.Add(commonAce);
                                this.TL.LogMessage("CanonicalizeDacl", "Found NotInherited Ace,               Allow: " + commonAce.SecurityIdentifier.Translate(Type.GetType("System.Security.Principal.NTAccount")).ToString() + " " + ((WindowsRegistryAccess.AccessRights)commonAce.AccessMask).ToString().ToString() + " " + commonAce.AceFlags.ToString());
                                continue;
                            case AceType.AccessDenied:
                                commonAceList1.Add(commonAce);
                                this.TL.LogMessage("CanonicalizeDacl", "Found NotInherited Ace,                Deny: " + commonAce.SecurityIdentifier.Translate(Type.GetType("System.Security.Principal.NTAccount")).ToString() + " " + ((WindowsRegistryAccess.AccessRights)commonAce.AccessMask).ToString().ToString() + " " + commonAce.AceFlags.ToString());
                                continue;
                            case AceType.AccessAllowedObject:
                                commonAceList5.Add(commonAce);
                                this.TL.LogMessage("CanonicalizeDacl", "Found NotInherited Ace, Object        Allow:" + commonAce.SecurityIdentifier.Translate(Type.GetType("System.Security.Principal.NTAccount")).ToString() + " " + ((WindowsRegistryAccess.AccessRights)commonAce.AccessMask).ToString().ToString() + " " + commonAce.AceFlags.ToString());
                                continue;
                            case AceType.AccessDeniedObject:
                                commonAceList2.Add(commonAce);
                                this.TL.LogMessage("CanonicalizeDacl", "Found NotInherited Ace, Object         Deny: " + commonAce.SecurityIdentifier.Translate(Type.GetType("System.Security.Principal.NTAccount")).ToString() + " " + ((WindowsRegistryAccess.AccessRights)commonAce.AccessMask).ToString().ToString() + " " + commonAce.AceFlags.ToString());
                                continue;
                            default:
                                continue;
                        }
                    }
                }
                this.TL.BlankLine();
                this.TL.LogMessage("CanonicalizeDacl", "Rebuilding in correct order...");
                this.TL.BlankLine();
                List<CommonAce>.Enumerator enumerator1=default(List<CommonAce>.Enumerator);
                try
                {
                    enumerator1 = commonAceList1.GetEnumerator();
                    while (enumerator1.MoveNext())
                    {
                        CommonAce current = enumerator1.Current;
                        rawAcl.InsertAce(index, current);
                        this.TL.LogMessage("CanonicalizeDacl", "Adding NotInherited Deny Ace,         "
                            + (current.AceType == AceType.AccessAllowed ? (object)"Allow" : (object)" Deny") + ": " + current.SecurityIdentifier.Translate(Type.GetType("System.Security.Principal.NTAccount")).ToString() + " " + ((WindowsRegistryAccess.AccessRights)current.AccessMask).ToString().ToString() + " " + current.AceFlags.ToString());
                        checked { ++index; }
                    }
                }
                finally
                {
                    enumerator1.Dispose();
                }
                List<CommonAce>.Enumerator enumerator2= default(List<CommonAce>.Enumerator);
                try
                {
                    enumerator2 = commonAceList2.GetEnumerator();
                    while (enumerator2.MoveNext())
                    {
                        CommonAce current = enumerator2.Current;
                        rawAcl.InsertAce(index, current);
                        this.TL.LogMessage("CanonicalizeDacl", "Adding NotInherited Deny Object Ace,  " + (current.AceType == AceType.AccessAllowed ? (object)"Allow" : (object)" Deny") + ": " + current.SecurityIdentifier.Translate(Type.GetType("System.Security.Principal.NTAccount")).ToString() + " " + ((WindowsRegistryAccess.AccessRights)current.AccessMask).ToString().ToString() + " " + current.AceFlags.ToString());
                        checked { ++index; }
                    }
                }
                finally
                {
                    enumerator2.Dispose();
                }
                List<CommonAce>.Enumerator enumerator3 = default(List<CommonAce>.Enumerator);
                try
                {
                    enumerator3 = commonAceList4.GetEnumerator();
                    while (enumerator3.MoveNext())
                    {
                        CommonAce current = enumerator3.Current;
                        rawAcl.InsertAce(index, current);
                        this.TL.LogMessage("CanonicalizeDacl", "Adding NotInherited Allow Ace,        " + (current.AceType == AceType.AccessAllowed ? (object)"Allow" : (object)" Deny") + ": " + current.SecurityIdentifier.Translate(Type.GetType("System.Security.Principal.NTAccount")).ToString() + " " + ((WindowsRegistryAccess.AccessRights)current.AccessMask).ToString().ToString() + " " + current.AceFlags.ToString());
                        checked { ++index; }
                    }
                }
                finally
                {
                    enumerator3.Dispose();
                }
                List<CommonAce>.Enumerator enumerator4 = default(List<CommonAce>.Enumerator);
                try
                {
                    enumerator4 = commonAceList5.GetEnumerator();
                    while (enumerator4.MoveNext())
                    {
                        CommonAce current = enumerator4.Current;
                        rawAcl.InsertAce(index, current);
                        this.TL.LogMessage("CanonicalizeDacl", "Adding NotInherited Allow Object Ace, " + (current.AceType == AceType.AccessAllowed ? (object)"Allow" : (object)" Deny") + ": " + current.SecurityIdentifier.Translate(Type.GetType("System.Security.Principal.NTAccount")).ToString() + " " + ((WindowsRegistryAccess.AccessRights)current.AccessMask).ToString().ToString() + " " + current.AceFlags.ToString());
                        checked { ++index; }
                    }
                }
                finally
                {
                    enumerator4.Dispose();
                }
                List<CommonAce>.Enumerator enumerator5 = default(List<CommonAce>.Enumerator);
                try
                {
                    enumerator5 = commonAceList3.GetEnumerator();
                    while (enumerator5.MoveNext())
                    {
                        CommonAce current = enumerator5.Current;
                        rawAcl.InsertAce(index, current);
                        this.TL.LogMessage("CanonicalizeDacl", "Adding Inherited Ace,                 " + (current.AceType == AceType.AccessAllowed ? (object)"Allow" : (object)" Deny") + ": " + current.SecurityIdentifier.Translate(Type.GetType("System.Security.Principal.NTAccount")).ToString() + " " + ((WindowsRegistryAccess.AccessRights)current.AccessMask).ToString().ToString() + " " + current.AceFlags.ToString());
                        checked { ++index; }
                    }
                }
                finally
                {
                    enumerator5.Dispose();
                }
                if (index != securityDescriptor.DiscretionaryAcl.Count)
                    return;
                securityDescriptor.DiscretionaryAcl = rawAcl;
                objectSecurity.SetSecurityDescriptorSddlForm(securityDescriptor.GetSddlForm(AccessControlSections.Access), AccessControlSections.Access);
            }
        }

        private void Backup55()
        {
            this.LogMessage("Backup55", "Creating Profile 5.5 XMLAccess object");
            XMLAccess Prof55 = new XMLAccess();
            this.LogMessage("Backup55", "Opening SOFTWARE\\ASCOM\\Platform55Original Registry Key");
            RegistryKey subKey = Registry.CurrentUser.CreateSubKey("SOFTWARE\\ASCOM\\Platform55Original");
            this.LogMessage("Backup55", "Backing up Platform 5.5 Profile");
            this.Copy55("", Prof55, subKey);
            subKey.Flush();
            subKey.Close();
            Prof55.Dispose();
            this.LogMessage("Backup55", "Completed copying the Profile");
        }

        private void Copy55(string CurrentSubKey, XMLAccess Prof55, RegistryKey RegistryTarget)
        {
            this._Copy55__2031E12814C128081__RecurseDepth = checked(this._Copy55__2031E12814C128081__RecurseDepth + 1);
            Stopwatch stopwatch = Stopwatch.StartNew();
            this.LogMessage("Copy55ToRegistry " + this._Copy55__2031E12814C128081__RecurseDepth.ToString(), "Starting key: " + CurrentSubKey);
            SortedList<string, string> sortedList1 = Prof55.EnumProfile(CurrentSubKey);
            IEnumerator<System.Collections.Generic.KeyValuePair<string, string>> enumerator1 = null;
            try
            {
                enumerator1 = sortedList1.GetEnumerator();
                while (enumerator1.MoveNext())
                {
                    System.Collections.Generic.KeyValuePair<string, string> current = enumerator1.Current;
                    RegistryTarget.SetValue(current.Key, current.Value);
                    this.LogMessage("  Copy55ToRegistry", "  Key: " + CurrentSubKey + " - \"" + current.Key + "\"  \"" + current.Value + "\"");
                }
            }
            finally
            {
                if (enumerator1 != null)
                    enumerator1.Dispose();
            }
            RegistryTarget.Flush();
            SortedList<string, string> sortedList2 = Prof55.EnumKeys(CurrentSubKey);
            IEnumerator<System.Collections.Generic.KeyValuePair<string, string>> enumerator2 = null;
            try
            {
                enumerator2 = sortedList2.GetEnumerator();
                while (enumerator2.MoveNext())
                {
                    System.Collections.Generic.KeyValuePair<string, string> current = enumerator2.Current;
                    this.LogMessage("  Copy55ToRegistry", "  Processing subkey: " + current.Key + " " + current.Value);
                    RegistryKey subKey = RegistryTarget.CreateSubKey(current.Key);
                    if ((current.Value != ""))
                        subKey.SetValue("", current.Value);
                    this.Copy55(CurrentSubKey + "\\" + current.Key, Prof55, subKey);
                    subKey.Flush();
                    subKey.Close();
                }
            }
            finally
            {
                if (enumerator2 != null)
                    enumerator2.Dispose();
            }
            stopwatch.Stop();
            this.LogMessage("  Copy55ToRegistry", "  Completed subkey: " + CurrentSubKey + " " + this._Copy55__2031E12814C128081__RecurseDepth.ToString() + ",  Elapsed time: " + stopwatch.ElapsedMilliseconds.ToString() + " milliseconds");
            this._Copy55__2031E12814C128081__RecurseDepth = checked(this._Copy55__2031E12814C128081__RecurseDepth - 1);
        }

        private void Restore50()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            RegistryKey subKey = Registry.CurrentUser.CreateSubKey("SOFTWARE\\ASCOM\\Platform5Original");
            RegistryKey ToKey = this.OpenSubKey(Registry.LocalMachine, "SOFTWARE\\ASCOM", true, WindowsRegistryAccess.RegWow64Options.KEY_WOW64_32KEY);
            this.LogMessage("Restore50", "Restoring Profile 5 to " + ToKey.Name);
            this.CopyRegistry(subKey, ToKey);
            subKey.Close();
            ToKey.Close();
            stopwatch.Stop();
            this.LogMessage("Restore50", "ElapsedTime " + stopwatch.ElapsedMilliseconds.ToString() + " milliseconds");
        }

        private void Restore55()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            RegistryKey FromKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\ASCOM\\Platform55Original");
            RegistryKey ToKey = this.OpenSubKey(Registry.LocalMachine, "SOFTWARE\\ASCOM", true, WindowsRegistryAccess.RegWow64Options.KEY_WOW64_32KEY);
            this.LogMessage("Restore55", "Restoring Profile 5.5 to " + ToKey.Name);
            this.CopyRegistry(FromKey, ToKey);
            FromKey.Close();
            ToKey.Close();
            stopwatch.Stop();
            this.LogMessage("Restore55", "ElapsedTime " + stopwatch.ElapsedMilliseconds.ToString() + " milliseconds");
        }

        private void GetProfileMutex(string Method, string Parameters)
        {
            try
            {
                this.GotMutex = this.ProfileMutex.WaitOne(5000, false);
                this.TL.LogMessage("GetProfileMutex", "Got Profile Mutex for " + Method);
            }
            catch (AbandonedMutexException ex)
            {
                ////ProjectData.SetProjectError((Exception)ex);
                AbandonedMutexException abandonedMutexException = ex;
                this.TL.LogMessage("GetProfileMutex", "***** WARNING ***** AbandonedMutexException in " + Method + ", parameters: " + Parameters);
                this.TL.LogMessageCrLf("AbandonedMutexException", abandonedMutexException.ToString());
                EventLogCode.LogEvent("RegistryAccess", "AbandonedMutexException in " + Method + ", parameters: " + Parameters, EventLogEntryType.Error, GlobalConstants.EventLogErrors.RegistryProfileMutexAbandoned, abandonedMutexException.ToString());
                if (RegistryCommonCode.GetBool("Trace Abandoned Mutexes", false))
                {
                    this.TL.LogMessage("RegistryAccess", "Throwing exception to application");
                    EventLogCode.LogEvent("RegistryAccess", "AbandonedMutexException in " + Method + ": Throwing exception to application", EventLogEntryType.Warning, GlobalConstants.EventLogErrors.RegistryProfileMutexAbandoned, null);
                    throw;
                }
                else
                {
                    this.TL.LogMessage("RegistryAccess", "Absorbing exception, continuing normal execution");
                    EventLogCode.LogEvent("RegistryAccess", "AbandonedMutexException in " + Method + ": Absorbing exception, continuing normal execution", EventLogEntryType.Warning, GlobalConstants.EventLogErrors.RegistryProfileMutexAbandoned, null);
                    this.GotMutex = true;
                    ////ProjectData.ClearProjectError();
                }
            }
            if (!this.GotMutex)
            {
                this.TL.LogMessage("GetProfileMutex", "***** WARNING ***** Timed out waiting for Profile mutex in " + Method + ", parameters: " + Parameters);
                EventLogCode.LogEvent(Method, "Timed out waiting for Profile mutex in " + Method + ", parameters: " + Parameters, EventLogEntryType.Error, GlobalConstants.EventLogErrors.RegistryProfileMutexTimeout, null);
                throw new ProfilePersistenceException("Timed out waiting for Profile mutex in " + Method + ", parameters: " + Parameters);
            }
        }

        public void ReleaseProfileMutex(string Method)
        {
            try
            {
                this.ProfileMutex.ReleaseMutex();
                this.TL.LogMessage("ReleaseProfileMutex", "Released Profile Mutex for " + Method);
            }
            catch (Exception ex)
            {
                ////ProjectData.SetProjectError(ex);
                Exception exception = ex;
                this.TL.LogMessage("ReleaseProfileMutex", "Exception: " + exception.ToString());
                if (RegistryCommonCode.GetBool("Trace Abandoned Mutexes", false))
                {
                    this.TL.LogMessage("ReleaseProfileMutex", "Release Mutex Exception in " + Method + ": Throwing exception to application");
                    EventLogCode.LogEvent("RegistryAccess", "Release Mutex Exception in " + Method + ": Throwing exception to application", EventLogEntryType.Error, GlobalConstants.EventLogErrors.RegistryProfileMutexAbandoned, exception.ToString());
                    throw;
                }
                else
                {
                    this.TL.LogMessage("ReleaseProfileMutex", "Release Mutex Exception in " + Method + ": Absorbing exception, continuing normal execution");
                    EventLogCode.LogEvent("RegistryAccess", "Release Mutex Exception in " + Method + ": Absorbing exception, continuing normal execution", EventLogEntryType.Error, GlobalConstants.EventLogErrors.RegistryProfileMutexAbandoned, exception.ToString());
                    ////ProjectData.ClearProjectError();
                }
            }
        }

        internal RegistryKey OpenSubKey(RegistryKey ParentKey, string SubKeyName, bool Writeable, WindowsRegistryAccess.RegWow64Options Options)
        {
            RegistryKey result = null;
            if (ParentKey?.Handle?.IsClosed??true)
                throw new ProfilePersistenceException("OpenSubKey: Parent key is not open");
            //New code
            if (!ParentKey.GetSubKeyNames().Contains(SubKeyName))
            {
                result = ParentKey.CreateSubKey(SubKeyName, Writeable);
            }
            else
            {
                result = ParentKey.OpenSubKey(SubKeyName, Writeable);
            }
            return result;
        }

        internal RegistryKey OpenSubKeyOld(RegistryKey ParentKey, string SubKeyName, bool Writeable, WindowsRegistryAccess.RegWow64Options Options)
        {
            RegistryKey result = null;
            //if (ParentKey == null || this.GetRegistryKeyHandle(ParentKey).Equals(IntPtr.Zero))
            //    throw new ProfilePersistenceException("OpenSubKey: Parent key is not open");
            //int num1 = 131097;
            //int phkResult = default(int);
            //int num2;
            //if (Writeable)
            //{
            //    int num3 = 131078;
            //    num2 = RegistryAccessX.RegCreateKeyEx(this.GetRegistryKeyHandle(ParentKey), ref SubKeyName, 0, IntPtr.Zero, 0, (int)((RegistryAccessX.RegWow64Options)num3 | Options), IntPtr.Zero.ToInt32(), ref phkResult, IntPtr.Zero.ToInt32());
            //}
            //else
            //{
            //    result = ParentKey.OpenSubKey(SubKeyName, Writeable);
            //    num2 = RegistryAccessX.RegOpenKeyEx(this.GetRegistryKeyHandle(ParentKey), ref SubKeyName, 0, (int)((RegistryAccessX.RegWow64Options)num1 | Options), ref phkResult);
            //}
            //switch (num2)
            //{
            //    case 0:
            //        return this.PointerToRegistryKey((IntPtr)phkResult, Writeable, false, Options);
            //    case 2:
            //        throw new ProfilePersistenceException("Cannot open key " + SubKeyName + " as it does not exist - Result: 0x" + $"{num2:X4}");
            //    default:
            //        throw new Win32Exception(num2, $"OpenSubKey: Exception encountered opening key - Result: 0x" + $"{num2:X4}");
            //}
            return result;
        }

        /*private RegistryKey PointerToRegistryKey(IntPtr hKey, bool writable, bool ownsHandle, RegistryAccessX.RegWow64Options options)*/
        //private RegistryKey PointerToRegistryKey(IntPtr hKey, bool writable, bool ownsHandle, RegistryAccessX.RegWow64Options options)
        //{
        //    Type[] types1 = new Type[2]
        //    {
        //typeof (IntPtr),
        //typeof (bool)
        //    };
        //    BindingFlags bindingAttr1 = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
        //    BindingFlags bindingAttr2 = BindingFlags.Instance | BindingFlags.NonPublic;
        //    Type type1 = typeof(SafeHandleZeroOrMinusOneIsInvalid).Assembly.GetType("Microsoft.Win32.SafeHandles.SafeRegistryHandle");
        //    object objectValue = RuntimeHelpers.GetObjectValue(type1.GetConstructor(bindingAttr1, null, types1, null).Invoke(new object[2]
        //    {
        // hKey,
        // ownsHandle
        //    }));
        //    RegistryKey registryKey;
        //    if (Environment.Version.Major >= 4)
        //    {
        //        Type type2 = typeof(SafeHandleZeroOrMinusOneIsInvalid).Assembly.GetType("Microsoft.Win32.RegistryView");
        //        Type[] types2 = new Type[3]
        //        {
        //  type1,
        //  typeof (bool),
        //  type2
        //        };
        //        registryKey = (RegistryKey)typeof(RegistryKey).GetConstructor(bindingAttr2, null, types2, null).Invoke(new object[3]
        //        {
        //  RuntimeHelpers.GetObjectValue(objectValue),
        //   writable,
        //   options
        //        });
        //    }
        //    else
        //    {
        //        Type[] types2 = new Type[2]
        //        {
        //  type1,
        //  typeof (bool)
        //        };
        //        registryKey = (RegistryKey)typeof(RegistryKey).GetConstructor(bindingAttr2, null, types2, null).Invoke(new object[2]
        //        {
        //  RuntimeHelpers.GetObjectValue(objectValue),
        //   writable
        //        });
        //    }
        //    return registryKey;
        //}

        /*private IntPtr GetRegistryKeyHandle(RegistryKey regKey)*/
        //private IntPtr GetRegistryKeyHandle(RegistryKey regKey)
        //{
        //    return regKey.Handle.DangerousGetHandle();
        //    //SafeHandle safeHandle = (SafeHandle)Type.GetType("Microsoft.Win32.RegistryKey").GetField("hkey", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(regKey);
        //    //safeHandle.DangerousGetHandle();
        //    //return safeHandle.DangerousGetHandle();
        //}

        //[DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        //private static extern int RegOpenKeyEx(IntPtr hKey, [MarshalAs(UnmanagedType.VBByRefStr)] ref string lpSubKey, int ulOptions, int samDesired, ref int phkResult);

        //[DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        //private static extern int RegCreateKeyEx(IntPtr hKey, [MarshalAs(UnmanagedType.VBByRefStr)] ref string lpSubKey, int Reserved, IntPtr lpClass, int dwOptions, int samDesired, int lpSecurityAttributes, ref int phkResult, int lpdwDisposition);

        [Obsolete]
        internal RegistryRights AccessToRegistryRights(AccessRights aRights)
        {
            RegistryRights result = RegistryRights.ReadKey;

            return result;
        }

        [Flags]
        public enum AccessRights
        {
            Query = 1,
            SetKey = 2,
            CreateSubKey = 4,
            EnumSubkey = 8,
            Notify = 16,
            CreateLink = 32,
            Unknown40 = 64,
            Unknown80 = 128,
            Wow64_64Key = 256,
            Wow64_32Key = 512,
            Unknown400 = 1024,
            Unknown800 = 2048,
            Unknown1000 = 4096,
            Unknown2000 = 8192,
            Unknown4000 = 16384,
            Unknown8000 = 32768,
            StandardDelete = 65536,
            StandardReadControl = 131072,
            StandardWriteDAC = 262144,
            StandardWriteOwner = 524288,
            StandardSynchronize = 1048576,
            Unknown200000 = 2097152,
            Unknown400000 = 4194304,
            AuditAccess = 8388608,
            AccessSystemSecurity = 16777216,
            MaximumAllowed = 33554432,
            Unknown4000000 = 67108864,
            Unknown8000000 = 134217728,
            GenericAll = 268435456,
            GenericExecute = 536870912,
            GenericWrite = 1073741824,
            GenericRead = -2147483648,
        }

        internal enum RegWow64Options
        {
            None = 0,
            KEY_WOW64_64KEY = 256,
            KEY_WOW64_32KEY = 512,
        }

        public void MigrateProfile(string CurrentPlatformVersion)
        {
            throw new System.NotImplementedException();
        }
    }
}