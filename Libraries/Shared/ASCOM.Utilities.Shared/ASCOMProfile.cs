// Decompiled with JetBrains decompiler
// Type: ASCOM.Utilities.ASCOMProfile
// Assembly: ASCOM.Utilities, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 06F2ED6E-C559-496D-B7FF-DCA800F2230D
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.Utilities\6.0.0.0__565de7938946fba7\ASCOM.Utilities.dll

//using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Linq;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ASCOM.Utilities
{
    //[ComVisible(true)]
    //[Guid("43325B3A-8B34-48db-8028-9D8CED9FA9E2")]
    //[ClassInterface(ClassInterfaceType.None)]
    public class ASCOMProfile : IXmlSerializable, IASCOMProfile
    {

        //[ComVisible(false)]
        public SortedList<string, SortedList<string, string>> ProfileValues { get; private set; }

        public ASCOMProfile()
        {
            ProfileValues = new SortedList<string, SortedList<string, string>>();
        }

        public void AddSubkey(string SubKeyName)
        {
            try
            {
                ProfileValues.Add(SubKeyName, new SortedList<string, string>());
                SetValue(SubKeyName, "", "");
            }
            catch (ArgumentException ex) { }
        }

        public void Clear()
        {
            ProfileValues.Clear();
        }

        public string GetValue(string Name, string SubKeyName)
        {
            string result = null;
            try { result = ProfileValues[SubKeyName][Name]; }
            catch (KeyNotFoundException kv) { result = null; }
            return result;
        }

        public void RemoveSubKey(string SubKeyName)
        {
            try { ProfileValues.Remove(SubKeyName); }
            catch (KeyNotFoundException kv) { }
        }

        public void RemoveValue(string ValueName, string SubKeyName)
        {
            if (Operators.CompareString(ValueName, "", false) == 0)
                return;
            try
            {
                ProfileValues[SubKeyName].Remove(ValueName);
            }
            catch (Exception ex)
            {
                //ProjectData.SetProjectError(ex);
                //ProjectData.ClearProjectError();
            }
        }

        public void SetValue(string Name, string Value, string SubKeyName)
        {
            if (ProfileValues.ContainsKey(SubKeyName))
                ProfileValues[SubKeyName][Name] = Value;
            else
            {
                ProfileValues.Add(SubKeyName, new SortedList<string, string>());
                ProfileValues[SubKeyName].Add("", "");
                ProfileValues[SubKeyName][Name] = Value;
            }
        }

        //[ComVisible(false)]
        public string GetValue(string Name)
        {
            return GetValue(Name, "");
        }

        //[ComVisible(false)]
        public void RemoveValue(string ValueName)
        {
            RemoveValue(ValueName, "");
        }

        //[ComVisible(false)]
        public void SetValue(string Name, string Value)
        {
            SetValue(Name, Value, "");
        }

        //[EditorBrowsable(EditorBrowsableState.Never)]
        public XmlSchema GetSchema()
        {
            return (XmlSchema)null;
        }

        public void ReadJson(StreamReader reader)
        {
            string json = reader.ReadToEnd();
            Clear();
            ProfileValues= JsonConvert.DeserializeObject<SortedList<string, SortedList<string, string>>>(json);
        }

        public void WriteJson(StreamWriter writer)
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.NullValueHandling = NullValueHandling.Ignore;
            serializer.Serialize(writer, ProfileValues);
        }

        //[EditorBrowsable(EditorBrowsableState.Never)]
        public void ReadXml(XmlReader reader)
        {
            string key1 = "";
            string key2 = "";
            ProfileValues.Clear();
            while (reader.Read())
            {
                string name = reader.Name;
                if (Operators.CompareString(name, "SubKeyName", false) == 0)
                {
                    key1 = reader.ReadString();
                    ProfileValues.Add(key1, new SortedList<string, string>());
                }
                else if (Operators.CompareString(name, "DefaultValue", false) == 0)
                    ProfileValues[key1].Add("", reader.ReadString());
                else if (Operators.CompareString(name, "Name", false) == 0)
                    key2 = reader.ReadString();
                else if (Operators.CompareString(name, "Data", false) == 0)
                    ProfileValues[key1].Add(key2, reader.ReadString());
            }
        }

        //[EditorBrowsable(EditorBrowsableState.Never)]
        public void WriteXml(XmlWriter writer)
        {
            IEnumerator<string> enumerator1 = null;
            try
            {
                enumerator1 = ProfileValues.Keys.GetEnumerator();
                while (enumerator1.MoveNext())
                {
                    string current1 = enumerator1.Current;
                    writer.WriteStartElement("SubKey");
                    writer.WriteElementString("SubKeyName", current1);
                    writer.WriteElementString("DefaultValue", ProfileValues[current1][""]);
                    writer.WriteStartElement("Values");
                    IEnumerator<KeyValuePair<string, string>> enumerator2 = null;
                    try
                    {
                        enumerator2 = ProfileValues[current1].GetEnumerator();
                        while (enumerator2.MoveNext())
                        {
                            KeyValuePair<string, string> current2 = enumerator2.Current;
                            if (!string.IsNullOrEmpty(current2.Key))
                            {
                                writer.WriteStartElement("Value");
                                writer.WriteElementString("Name", current2.Key);
                                writer.WriteElementString("Data", current2.Value);
                                writer.WriteEndElement();
                            }
                        }
                    }
                    finally
                    {
                        if (enumerator2 != null)
                            enumerator2.Dispose();
                    }
                    writer.WriteEndElement();
                    writer.WriteEndElement();
                }
            }
            finally
            {
                if (enumerator1 != null)
                    enumerator1.Dispose();
            }
        }

        public Dictionary<string, Dictionary<string, string>> ToDictionary()
        {
            Dictionary<string, Dictionary<string, string>> rdata = new Dictionary<string, Dictionary<string, string>>();
            foreach (string key in ProfileValues.Keys)
            {
                var sdict = new Dictionary<string, string>();
                rdata.Add(key, sdict);
                SortedList<string, string> slist = ProfileValues[key];
                foreach(var skvp in slist)
                    sdict.Add(skvp.Key, skvp.Value);
            }
            return rdata;
        }
    }
}
