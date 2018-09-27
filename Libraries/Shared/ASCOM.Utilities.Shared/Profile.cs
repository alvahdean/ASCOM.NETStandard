using ASCOM.Internal;
using ASCOM.Utilities.Exceptions;
using ASCOM.Utilities.Interfaces;
using RACI.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ASCOM.Utilities
{
    //[ClassInterface(ClassInterfaceType.None)]
    //[Guid("880840E2-76E6-4036-AD8F-60A326D7F9DA")]
    //[ComVisible(true)]
    public class Profile : IProfile, IProfileExtra, IDisposable
    {
        protected const bool defaultIgnore = true;
        protected const string defaultDriverType = "Telescope";
        protected static IAscomDataStore _pStore;
        protected static TraceLogger _tlog;

        protected string _deviceType;
        protected bool disposedValue;
        protected bool IgnoreExceptions { get; set; } = defaultIgnore;
        protected IEqualityComparer<String> KeyComparer { get; set; } = new CIKeyComparer();
        protected string shortName(string driverType)
        {
            driverType = driverType?.Trim() ?? "";
            int idx = driverType.LastIndexOf(" Drivers");
            if (idx >= 0)
                driverType = driverType.Substring(0, idx);
            return driverType.Trim();
        }
        protected string fullName(string driverType) => $"{shortName(driverType)} Drivers";

        protected static TraceLogger TL
        {
            get
            {
                if (_tlog == null)
                {
                    _tlog = new TraceLogger("", "Profile");
#if DEBUG
                    _tlog.EchoDebug = true;
                    _tlog.EchoConsole = true;
#endif
                    _tlog.Enabled = RegistryCommonCode.GetBool("Trace Profile", false);
                    _tlog.LogMessage("Profile", $"Trace logger created");
                }
                return _tlog;
            }
        }
        protected static IAscomDataStore Store
        {
            get
            {
                return _pStore ?? (_pStore = new EntityStore());
            }
        }

        public Profile() : this(defaultIgnore) { }
        public Profile(bool ignoreExceptions)
        {
            disposedValue = false;
            IgnoreExceptions = ignoreExceptions;
            _deviceType = defaultDriverType;
        }

        public string DeviceType
        {
            get=> _deviceType;
            set
            {
                if (String.IsNullOrWhiteSpace(value))
                    throw new Exceptions.InvalidValueException("Illegal DeviceType value (empty string)");
                value = shortName(value);
                if (DeviceType != value)
                {
                    TL.LogMessage("DeviceType Set", value);
                    _deviceType = value;
                }
            }
        }

        public ArrayList RegisteredDeviceTypes
        {
            get
            {
                ArrayList arrayList = new ArrayList();
                TL.LogMessage("RegisteredDeviceTypes", "Reading ProfileStore...");

                SortedList<string, string> sortedList = Store.EnumKeys("");
                TL.LogMessage("RegisteredDeviceTypes", $"Found {sortedList.Count} values");
                try
                {
                    foreach (var name in sortedList.Select(t => t.Key))
                    {
                        if (name.EndsWith(" Drivers"))
                        {
                            string str = shortName(name);
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

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                int num = disposing ? 1 : 0;
                if (_pStore != null)
                {
                    _pStore.Dispose();
                    _pStore = null;
                }
                if (_tlog != null)
                {
                    _tlog.Enabled = false;
                    _tlog.Dispose();
                    _tlog = null;
                }
            }
            disposedValue = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize((object)this);
        }

        public ArrayList RegisteredDevices(string deviceType)
        {
            ArrayList result= new ArrayList();
            if (string.IsNullOrEmpty(deviceType))
            {
                TL.LogMessage("RegisteredDevices", "Empty string or Nothing supplied as DeviceType");
                throw new ASCOM.Utilities.Exceptions.InvalidValueException("Empty string or Nothing supplied as DeviceType");
            }

            try
            {
                TL.LogMessage("RegisteredDevices", $"Device type: {deviceType}");
                string drvPath = fullName(deviceType);
                var devices = Store.EnumKeys(drvPath);
                foreach (var item in devices)
                {
                    string key = item.Key;
                    string val = item.Value;
                    if (!String.IsNullOrWhiteSpace(key))
                        result.Add(new KeyValuePair(key, val));
                }
            }
            catch(Exception ex)
            {
                TL.LogMessage("Exception", $"{ex.GetType().FullName}: {ex}");
                throw;
            }
            return result;
        }

        public bool IsRegistered(string DriverID)
        {

            var drvList = RegisteredDevices(DeviceType);
            var drvKvps=drvList.ToKvps();
            var drvKeys = drvKvps.Select(t => t.Key);
            bool result = drvKeys.Contains(DriverID);
            return result;
        }

        public void Register(string DriverID, string DescriptiveName)
        {
            string path = MakeKey(DriverID, "");
            if (!IsRegistered(DriverID))
            {
                if (String.IsNullOrWhiteSpace(DescriptiveName))
                    DescriptiveName = null;
                TL.LogMessage("Register", "Registering " + DriverID);
                Store.CreateKey<AscomDeviceNode>(path);
                Store.WriteProfile(path, "", DescriptiveName);
            }
            else
            {
                string curr = GetValue(DriverID, "", "", "");
                TL.LogMessage("Register", DriverID + " is already registered with description: \"" + curr + "\"");
                if (!String.IsNullOrWhiteSpace(curr))
                    return;
                TL.LogMessage("Register", "Description is missing and new value is supplied so refreshing with: \"" + DescriptiveName + "\"");
                Store.WriteProfile(path, "", DescriptiveName);
            }
        }

        public void Unregister(string DriverID)
        {
            TL.LogMessage("Unregister", DriverID);
            CheckRegistered(DriverID);
            TL.LogMessage("Unregister", "Unregistering " + DriverID);
            Store.DeleteKey(MakeKey(DriverID, ""));
        }

        public string GetValue(string DriverID, string Name, string SubKey, string DefaultValue)
        {
            TL.LogMessage("GetValue", "Driver: " + DriverID + " Name: " + Name + " Subkey: \"" + SubKey + "\" DefaultValue: \"" + DefaultValue + "\"");
            CheckRegistered(DriverID);
            string profile = Store.GetProfile(MakeKey(DriverID, SubKey), Name, DefaultValue);
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
                TL.LogMessage("WriteProfile", "WARNING - The device default value is protected as it contains the device description and is set by Profile.Register");
            Store.WriteProfile(MakeKey(DriverID, SubKey), Name, Value);
        }

        public ArrayList Values(string DriverID, string SubKey)
        {
            ArrayList arrayList = new ArrayList();
            TL.LogMessage("Values", "Driver: " + DriverID + " Subkey: \"" + SubKey + "\"");
            CheckRegistered(DriverID);
            SortedList<string, string> sortedList = Store.EnumProfile(MakeKey(DriverID, SubKey));
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
            Store.DeleteProfile(MakeKey(DriverID, SubKey), Name);
        }

        public void CreateSubKey(string DriverID, string SubKey)
        {
            TL.LogMessage("CreateSubKey", "Driver: " + DriverID + " Subkey: \"" + SubKey + "\"");
            CheckRegistered(DriverID);
            Store.CreateKey(MakeKey(DriverID, SubKey));
        }

        public ArrayList SubKeys(string DriverID, string SubKey)
        {
            ArrayList arrayList = new ArrayList();
            TL.LogMessage("SubKeys", "Driver: " + DriverID + " Subkey: \"" + SubKey + "\"");
            if (Operators.CompareString(DriverID, "", false) != 0)
                CheckRegistered(DriverID);
            SortedList<string, string> sortedList = Store.EnumKeys(MakeKey(DriverID, SubKey));
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
            Store.DeleteKey(MakeKey(DriverID, SubKey));
        }

        public string GetProfileXML(string DriverId)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ASCOMProfile));
            UTF8Encoding utF8Encoding = new UTF8Encoding();
            TL.LogMessage("GetProfileXML", "Driver: " + DriverId);
            CheckRegistered(DriverId);
            IASCOMProfile profile = Store.GetProfile(MakeKey(DriverId, ""));
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
            Store.SetProfile(MakeKey(DriverId, ""), p_ProfileKey);
            TL.LogMessage("  SetProfileXML", "  Complete");
        }

        public IASCOMProfile GetProfile(string DriverId)
        {
            TL.LogMessage("GetProfile", "Driver: " + DriverId);
            CheckRegistered(DriverId);
            string key = MakeKey(DriverId, "");
            TL.LogMessage("GetProfile", "Driver Key: " + key);
            IASCOMProfile profile = Store.GetProfile(key);
            TL.LogMessageCrLf("  GetProfile", "Complete");
            return profile;
        }

        public Dictionary<string, Dictionary<string, string>> GetProfileKvp(string DriverId)
        {
            TL.LogMessage("GetProfile", "Driver: " + DriverId);
            CheckRegistered(DriverId);
            IASCOMProfile profile = Store.GetProfile(MakeKey(DriverId, ""));
            TL.LogMessageCrLf("  GetProfile", "Complete");
            return profile?.ToDictionary();
        }

        public void SetProfile(string DriverId, IASCOMProfile NewProfileKey)
        {
            TL.LogMessage("SetProfile", "Driver: " + DriverId);
            CheckRegistered(DriverId);
            Store.SetProfile(MakeKey(DriverId, ""), NewProfileKey);
            TL.LogMessage("  SetProfile", "  Complete");
        }

        //[EditorBrowsable(EditorBrowsableState.Never)]
        //[ComVisible(false)]
        public void MigrateProfile(string CurrentPlatformVersion)
        {
            Store.BackupProfile(CurrentPlatformVersion);
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

        private string MakeKey(string baseKey, string subKey)
        {
            baseKey = baseKey?.Trim();
            subKey = subKey?.Trim();
            string str = fullName(DeviceType);
            if (!String.IsNullOrWhiteSpace(baseKey))
                str = $"{str}\\{baseKey}";
            if (!String.IsNullOrWhiteSpace(subKey))
                str = $"{str}\\{subKey}";
            return str;
        }

        private void CheckRegistered(string DriverID)
        {
            TL.LogMessage("  CheckRegistered", $"'{DriverID}' DeviceType: '{_deviceType}'");
            if (!IsRegistered(DriverID))
            {
                TL.LogMessage("  CheckRegistered", "Driver is not registered");
                if (String.IsNullOrWhiteSpace(DriverID))
                {
                    TL.LogMessage("  CheckRegistered", "Throwing illegal driver ID exception");
                    throw new ASCOM.Utilities.Exceptions.InvalidValueException($"Illegal DriverID value '{DriverID}'");
                }
                TL.LogMessage("  CheckRegistered", $"Throwing driver is not registered exception. ProgID: '{DriverID}' DeviceType: '{_deviceType}'");
                throw new DriverNotRegisteredException($"DriverID '{DriverID}' is not registered as a device of type: '{_deviceType}'");
            }
            TL.LogMessage("  CheckRegistered", "Driver is registered");
        }
    }

}