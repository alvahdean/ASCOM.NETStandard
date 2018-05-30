// Decompiled with JetBrains decompiler
// Type: ASCOM.Utilities.Profile
// Assembly: ASCOM.Utilities, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 06F2ED6E-C559-496D-B7FF-DCA800F2230D
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.Utilities\6.0.0.0__565de7938946fba7\ASCOM.Utilities.dll


using ASCOM.Utilities.Exceptions;
using ASCOM.Utilities.Interfaces;
//using Microsoft.VisualBasic;
//using Microsoft.VisualBasic;
//using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml.Serialization;

namespace ASCOM.Utilities
{
    [ClassInterface(ClassInterfaceType.None)]
    //[Guid("880840E2-76E6-4036-AD8F-60A326D7F9DA")]
    //[ComVisible(true)]
    public class Profile : IProfile, IProfileExtra, IDisposable
    {
        private string m_sDeviceType;

        //TODO: Replace RegistryAccess with IAsconDataStore
        //private IAscomDataStore ProfileStore;
        private RegistryAccess ProfileStore;
        private TraceLogger TL;
        private string LastDriverID;
        private bool LastResult;
        private bool disposedValue;

        public string DeviceType
        {
            get
            {
                return m_sDeviceType;
            }
            set
            {
                TL.LogMessage("DeviceType Set", value.ToString());
                if (String.IsNullOrWhiteSpace(value))
                    throw new Exceptions.InvalidValueException("Illegal DeviceType value \"\" (empty string)");
                m_sDeviceType = value;
            }
        }

        public ArrayList RegisteredDeviceTypes
        {
            get
            {
                ArrayList arrayList = new ArrayList();
                TL.LogMessage("RegisteredDeviceTypes", "Reading ProfileStore...");

                SortedList<string, string> sortedList = ProfileStore.EnumKeys("");
                TL.LogMessage("RegisteredDeviceTypes", $"Found {sortedList.Count} values");
                try
                {
                    foreach (var name in sortedList.Select(t => t.Key))
                    {
                        TL.LogMessage("RegisteredDeviceTypes", "  " + name + " " + name);
                        if (name.EndsWith(" Drivers"))
                        {
                            int idx = name.LastIndexOf(" Drivers");
                            string str = name.Substring(0, idx);
                            TL.LogMessage("RegisteredDeviceTypes", "    Adding: " + str);
                            arrayList.Add(str);
                        }
                    }
                }
                catch { }
                arrayList.Sort();
                return arrayList;
            }
        }

        public Profile()
        {
            disposedValue = false;
            ProfileStore = new RegistryAccess("ASCOM Helper Registry Profile Object");
            m_sDeviceType = "Telescope";
            TL = new TraceLogger("", "Profile");
            TL.Enabled = RegistryCommonCode.GetBool("Trace Profile", false);
            TL.LogMessage("New", "Trace logger created OK");
        }

        public Profile(bool IgnoreExceptions)
        {
            disposedValue = false;
            ProfileStore = new RegistryAccess(IgnoreExceptions);
            m_sDeviceType = "Telescope";
            TL = new TraceLogger("", "Profile");
            TL.Enabled = RegistryCommonCode.GetBool("Trace Profile", false);
            TL.LogMessage("New", "Trace logger created OK - Ignoring any ProfileNotFound exceptions");
        }

        ~Profile()
        {
            Dispose(false);
            // ISSUE: explicit finalizer call
            //base.Finalize();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                int num = disposing ? 1 : 0;
                if (ProfileStore != null)
                {
                    ProfileStore.Dispose();
                    ProfileStore = (RegistryAccess)null;
                }
                if (TL != null)
                {
                    TL.Enabled = false;
                    TL.Dispose();
                    TL = (TraceLogger)null;
                }
            }
            disposedValue = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize((object)this);
        }

        public ArrayList RegisteredDevices(string DeviceType)
        {
            ArrayList arrayList = new ArrayList();
            if (string.IsNullOrEmpty(DeviceType))
            {
                TL.LogMessage("RegisteredDevices", "Empty string or Nothing supplied as DeviceType");
                throw new ASCOM.Utilities.Exceptions.InvalidValueException("Empty string or Nothing supplied as DeviceType");
            }
            SortedList<string, string> sortedList;
            try
            {
                sortedList = ProfileStore.EnumKeys(DeviceType + " Drivers");
            }
            catch (NullReferenceException ex)
            {
                //ProjectData.SetProjectError((Exception) ex);
                TL.LogMessage("RegisteredDevices", "WARNING: there are no devices of type: \"" + DeviceType + "\" registered on this system");
                sortedList = new SortedList<string, string>();
                //ProjectData.ClearProjectError();
            }
            TL.LogMessage("RegisteredDevices", "Device type: " + DeviceType + " - found " + Conversions.ToString(sortedList.Count) + " devices");
            try
            {
                foreach (var item in sortedList)
                {
                    TL.LogMessage("RegisteredDevices", "  " + item.Key + " - " + item.Value);
                    arrayList.Add((object)new KeyValuePair(item.Key, item.Value));
                }
            }
            catch { }

            return arrayList;
        }

        public bool IsRegistered(string DriverID)
        {
            return IsRegistered(DriverID, false);
        }

        private bool IsRegistered(string DriverID, bool Indent)
        {
            string str = "";
            if (Indent)
                str = "  ";
            if (Operators.CompareString(DriverID, LastDriverID, false) == 0)
            {
                TL.LogMessage(str + "IsRegistered", str + DriverID.ToString() + " - using cached value: " + Conversions.ToString(LastResult));
                return LastResult;
            }
            TL.LogStart(str + "IsRegistered", str + DriverID.ToString() + " ");
            bool flag = false;
            if (Operators.CompareString(DriverID, "", false) == 0)
            {
                TL.LogFinish("Null string so exiting False");
            }
            else
            {
                try
                {
                    SortedList<string, string> sortedList = ProfileStore.EnumKeys(MakeKey("", ""));
                    IEnumerator<System.Collections.Generic.KeyValuePair<string, string>> enumerator = null;
                    try
                    {
                        enumerator = sortedList.GetEnumerator();
                        while (enumerator.MoveNext())
                        {
                            if (Operators.CompareString(enumerator.Current.Key.ToUpperInvariant(), DriverID.ToUpperInvariant(), false) == 0)
                            {
                                TL.LogFinish("Key " + DriverID + " found");
                                flag = true;
                            }
                        }
                    }
                    finally
                    {
                        if (enumerator != null)
                            enumerator.Dispose();
                    }
                    if (!flag)
                        TL.LogFinish("Key " + DriverID + " not found");
                }
                catch (Exception ex)
                {
                    //ProjectData.SetProjectError(ex);
                    TL.LogFinish("Exception: " + ex.ToString());
                    //ProjectData.ClearProjectError();
                }
                LastDriverID = DriverID;
                LastResult = flag;
            }
            return flag;
        }

        public void Register(string DriverID, string DescriptiveName)
        {
            if (!IsRegistered(DriverID))
            {
                TL.LogMessage("Register", "Registering " + DriverID);
                ProfileStore.CreateKey(MakeKey(DriverID, ""));
                ProfileStore.WriteProfile(MakeKey(DriverID, ""), "", DescriptiveName);
                LastDriverID = "";
            }
            else
            {
                string Left = GetValue(DriverID, "", "", "");
                TL.LogMessage("Register", DriverID + " is already registered with description: \"" + Left + "\"");
                if (!(Operators.CompareString(Left, "", false) == 0 & Operators.CompareString(DescriptiveName, "", false) != 0))
                    return;
                TL.LogMessage("Register", "Description is missing and new value is supplied so refreshing with: \"" + DescriptiveName + "\"");
                ProfileStore.WriteProfile(MakeKey(DriverID, ""), "", DescriptiveName);
            }
        }

        public void Unregister(string DriverID)
        {
            TL.LogMessage("Unregister", DriverID);
            CheckRegistered(DriverID);
            TL.LogMessage("Unregister", "Unregistering " + DriverID);
            LastDriverID = "";
            ProfileStore.DeleteKey(MakeKey(DriverID, ""));
        }

        public string GetValue(string DriverID, string Name, string SubKey, string DefaultValue)
        {
            TL.LogMessage("GetValue", "Driver: " + DriverID + " Name: " + Name + " Subkey: \"" + SubKey + "\" DefaultValue: \"" + DefaultValue + "\"");
            CheckRegistered(DriverID);
            string profile = ProfileStore.GetProfile(MakeKey(DriverID, SubKey), Name, DefaultValue);
            TL.LogMessage("  GetValue", "  " + profile);
            return profile;
        }

        public void WriteValue(string DriverID, string Name, string Value, string SubKey)
        {
            TL.LogMessage("WriteValue", "Driver: " + DriverID + " Name: " + Name + " Value: " + Value + " Subkey: \"" + SubKey + "\"");
            if (Value == null)
            {
                TL.LogMessage("WriteProfile", "WARNING - Supplied data value is Nothing, not empty string");
                Value = "";
            }
            CheckRegistered(DriverID);
            if (Operators.CompareString(Name, "", false) == 0 & Operators.CompareString(SubKey, "", false) == 0)
                throw new RestrictedAccessException("The device default value is protected as it contains the device description and is set by Profile.Register");
            ProfileStore.WriteProfile(MakeKey(DriverID, SubKey), Name, Value);
        }

        public ArrayList Values(string DriverID, string SubKey)
        {
            ArrayList arrayList = new ArrayList();
            TL.LogMessage("Values", "Driver: " + DriverID + " Subkey: \"" + SubKey + "\"");
            CheckRegistered(DriverID);
            SortedList<string, string> sortedList = ProfileStore.EnumProfile(MakeKey(DriverID, SubKey));
            TL.LogMessage("  Values", "  Returning " + Conversions.ToString(sortedList.Count) + " values");
            IEnumerator<System.Collections.Generic.KeyValuePair<string, string>> enumerator = null;
            try
            {
                enumerator = sortedList.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    System.Collections.Generic.KeyValuePair<string, string> current = enumerator.Current;
                    TL.LogMessage("  Values", "  " + current.Key + " = " + current.Value);
                    arrayList.Add((object)new KeyValuePair(current.Key, current.Value));
                }
            }
            finally
            {
                if (enumerator != null)
                    enumerator.Dispose();
            }
            return arrayList;
        }

        public void DeleteValue(string DriverID, string Name, string SubKey)
        {
            TL.LogMessage("DeleteValue", "Driver: " + DriverID + " Name: " + Name + " Subkey: \"" + SubKey + "\"");
            CheckRegistered(DriverID);
            ProfileStore.DeleteProfile(MakeKey(DriverID, SubKey), Name);
        }

        public void CreateSubKey(string DriverID, string SubKey)
        {
            TL.LogMessage("CreateSubKey", "Driver: " + DriverID + " Subkey: \"" + SubKey + "\"");
            CheckRegistered(DriverID);
            ProfileStore.CreateKey(MakeKey(DriverID, SubKey));
        }

        public ArrayList SubKeys(string DriverID, string SubKey)
        {
            ArrayList arrayList = new ArrayList();
            TL.LogMessage("SubKeys", "Driver: " + DriverID + " Subkey: \"" + SubKey + "\"");
            if (Operators.CompareString(DriverID, "", false) != 0)
                CheckRegistered(DriverID);
            SortedList<string, string> sortedList = ProfileStore.EnumKeys(MakeKey(DriverID, SubKey));
            TL.LogMessage("  SubKeys", "  Returning " + Conversions.ToString(sortedList.Count) + " subkeys");
            IEnumerator<System.Collections.Generic.KeyValuePair<string, string>> enumerator = null;
            try
            {
                enumerator = sortedList.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    System.Collections.Generic.KeyValuePair<string, string> current = enumerator.Current;
                    TL.LogMessage("  SubKeys", "  " + current.Key + " = " + current.Value);
                    arrayList.Add((object)new KeyValuePair(current.Key, current.Value));
                }
            }
            finally
            {
                if (enumerator != null)
                    enumerator.Dispose();
            }
            return arrayList;
        }

        public void DeleteSubKey(string DriverID, string SubKey)
        {
            TL.LogMessage("DeleteSubKey", "Driver: " + DriverID + " Subkey: \"" + SubKey + "\"");
            CheckRegistered(DriverID);
            ProfileStore.DeleteKey(MakeKey(DriverID, SubKey));
        }

        public string GetProfileXML(string DriverId)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ASCOMProfile));
            UTF8Encoding utF8Encoding = new UTF8Encoding();
            TL.LogMessage("GetProfileXML", "Driver: " + DriverId);
            CheckRegistered(DriverId);
            IASCOMProfile profile = ProfileStore.GetProfile(MakeKey(DriverId, ""));
            string str;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                xmlSerializer.Serialize((Stream)memoryStream, (object)profile);
                byte[] array = memoryStream.ToArray();
                str = utF8Encoding.GetString(array);
            }
            TL.LogMessageCrLf("  GetProfileXML", "\r\n" + str);
            return str;
        }

        public void SetProfileXML(string DriverId, string XmlProfile)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ASCOMProfile));
            UTF8Encoding utF8Encoding = new UTF8Encoding();
            TL.LogMessageCrLf("SetProfileXML", "Driver: " + DriverId + "\r\n" + XmlProfile);
            CheckRegistered(DriverId);
            ASCOMProfile p_ProfileKey;
            using (MemoryStream memoryStream = new MemoryStream(utF8Encoding.GetBytes(XmlProfile)))
                p_ProfileKey = (ASCOMProfile)xmlSerializer.Deserialize((Stream)memoryStream);
            ProfileStore.SetProfile(MakeKey(DriverId, ""), p_ProfileKey);
            TL.LogMessage("  SetProfileXML", "  Complete");
        }

        public IASCOMProfile GetProfile(string DriverId)
        {
            TL.LogMessage("GetProfile", "Driver: " + DriverId);
            CheckRegistered(DriverId);
            string key = MakeKey(DriverId, "");
            TL.LogMessage("GetProfile", "Driver Key: " + key);
            IASCOMProfile profile = ProfileStore.GetProfile(key);
            TL.LogMessageCrLf("  GetProfile", "Complete");
            return profile;
        }

        public IList<KeyValuePair<String, String>> GetProfileKvp(string DriverId)
        {
            TL.LogMessage("GetProfile", "Driver: " + DriverId);
            CheckRegistered(DriverId);
            IASCOMProfile profile = ProfileStore.GetProfile(MakeKey(DriverId, ""));
            TL.LogMessageCrLf("  GetProfile", "Complete");
            return profile?.ToKvpList();
        }

        public void SetProfile(string DriverId, IASCOMProfile NewProfileKey)
        {
            TL.LogMessage("SetProfile", "Driver: " + DriverId);
            CheckRegistered(DriverId);
            ProfileStore.SetProfile(MakeKey(DriverId, ""), NewProfileKey);
            TL.LogMessage("  SetProfile", "  Complete");
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        //[ComVisible(false)]
        public void MigrateProfile(string CurrentPlatformVersion)
        {
            ProfileStore.BackupProfile(CurrentPlatformVersion);
        }

        //[ComVisible(false)]
        public void DeleteValue(string DriverID, string Name)
        {
            DeleteValue(DriverID, Name, "");
        }

        //[ComVisible(false)]
        public string GetValue(string DriverID, string Name)
        {
            return GetValue(DriverID, Name, "", (string)null);
        }

        //[ComVisible(false)]
        public string GetValue(string DriverID, string Name, string SubKey)
        {
            return GetValue(DriverID, Name, SubKey, (string)null);
        }

        //[ComVisible(false)]
        public ArrayList SubKeys(string DriverID)
        {
            return SubKeys(DriverID, "");
        }

        //[ComVisible(false)]
        public ArrayList Values(string DriverID)
        {
            return Values(DriverID, "");
        }

        //[ComVisible(false)]
        public void WriteValue(string DriverID, string Name, string Value)
        {
            WriteValue(DriverID, Name, Value, "");
        }

        private string MakeKey(string BaseKey, string SubKey)
        {
            string str = m_sDeviceType + " Drivers";
            if (BaseKey!="")
                str = str + "\\" + BaseKey;
            if (SubKey!="")
                str = str + "\\" + SubKey;
            return str;
        }

        private void CheckRegistered(string DriverID)
        {
            TL.LogMessage("  CheckRegistered", "\"" + DriverID + "\" DeviceType: " + m_sDeviceType);
            if (!IsRegistered(DriverID, true))
            {
                TL.LogMessage("  CheckRegistered", "Driver is not registered");
                if (Operators.CompareString(DriverID, "", false) == 0)
                {
                    TL.LogMessage("  CheckRegistered", "Throwing illegal driver ID exception");
                    throw new ASCOM.Utilities.Exceptions.InvalidValueException("Illegal DriverID value \"\" (empty string)");
                }
                TL.LogMessage("  CheckRegistered", "Throwing driver is not registered exception. ProgID: " + DriverID + " DeviceType: " + m_sDeviceType);
                throw new DriverNotRegisteredException("DriverID " + DriverID + " is not registered as a device of type: " + m_sDeviceType);
            }
            TL.LogMessage("  CheckRegistered", "Driver is registered");
        }
    }
}