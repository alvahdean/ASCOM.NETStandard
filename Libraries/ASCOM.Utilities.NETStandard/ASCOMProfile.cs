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

namespace ASCOM.Utilities
{
  //[ComVisible(true)]
  //[Guid("43325B3A-8B34-48db-8028-9D8CED9FA9E2")]
  [ClassInterface(ClassInterfaceType.None)]
  public class ASCOMProfile : IXmlSerializable, IASCOMProfile
    {
    private SortedList<string, SortedList<string, string>> Subkey;

    //[ComVisible(false)]
    public SortedList<string, SortedList<string, string>> ProfileValues
    {
      get
      {
        return this.Subkey;
      }
    }

    public ASCOMProfile()
    {
      this.Subkey = new SortedList<string, SortedList<string, string>>();
    }

    public void AddSubkey(string SubKeyName)
    {
      try
      {
        this.Subkey.Add(SubKeyName, new SortedList<string, string>());
        this.SetValue(SubKeyName, "", "");
      }
      catch (ArgumentException ex)
      {
        //ProjectData.SetProjectError((Exception) ex);
        //ProjectData.ClearProjectError();
      }
    }

    public void Clear()
    {
      this.Subkey.Clear();
    }

    public string GetValue(string Name, string SubKeyName)
    {
      return this.Subkey[SubKeyName][Name];
    }

    public void RemoveSubKey(string SubKeyName)
    {
      try
      {
        this.Subkey.Remove(SubKeyName);
      }
      catch (Exception ex)
      {
        //ProjectData.SetProjectError(ex);
        //ProjectData.ClearProjectError();
      }
    }

    public void RemoveValue(string ValueName, string SubKeyName)
    {
      if (Operators.CompareString(ValueName, "", false) == 0)
        return;
      try
      {
        this.Subkey[SubKeyName].Remove(ValueName);
      }
      catch (Exception ex)
      {
        //ProjectData.SetProjectError(ex);
        //ProjectData.ClearProjectError();
      }
    }

    public void SetValue(string Name, string Value, string SubKeyName)
    {
      if (this.Subkey.ContainsKey(SubKeyName))
      {
        this.Subkey[SubKeyName][Name] = Value;
      }
      else
      {
        this.Subkey.Add(SubKeyName, new SortedList<string, string>());
        this.Subkey[SubKeyName].Add("", "");
        this.Subkey[SubKeyName][Name] = Value;
      }
    }

    //[ComVisible(false)]
    public string GetValue(string Name)
    {
      return this.GetValue(Name, "");
    }

    //[ComVisible(false)]
    public void RemoveValue(string ValueName)
    {
      this.RemoveValue(ValueName, "");
    }

    //[ComVisible(false)]
    public void SetValue(string Name, string Value)
    {
      this.SetValue(Name, Value, "");
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public XmlSchema GetSchema()
    {
      return (XmlSchema) null;
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public void ReadXml(XmlReader reader)
    {
      string key1 = "";
      string key2 = "";
      this.Subkey.Clear();
      while (reader.Read())
      {
        string name = reader.Name;
        if (Operators.CompareString(name, "SubKeyName", false) == 0)
        {
          key1 = reader.ReadString();
          this.Subkey.Add(key1, new SortedList<string, string>());
        }
        else if (Operators.CompareString(name, "DefaultValue", false) == 0)
          this.Subkey[key1].Add("", reader.ReadString());
        else if (Operators.CompareString(name, "Name", false) == 0)
          key2 = reader.ReadString();
        else if (Operators.CompareString(name, "Data", false) == 0)
          this.Subkey[key1].Add(key2, reader.ReadString());
      }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public void WriteXml(XmlWriter writer)
    {
      IEnumerator<string> enumerator1 = null;
      try
      {
        enumerator1 = this.Subkey.Keys.GetEnumerator();
        while (enumerator1.MoveNext())
        {
          string current1 = enumerator1.Current;
          writer.WriteStartElement("SubKey");
          writer.WriteElementString("SubKeyName", current1);
          writer.WriteElementString("DefaultValue", this.Subkey[current1][""]);
          writer.WriteStartElement("Values");
          IEnumerator<KeyValuePair<string, string>> enumerator2 = null;
          try
          {
            enumerator2 = this.Subkey[current1].GetEnumerator();
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

        public List<KeyValuePair<string, string>> ToKvpList()
        {
            List<KeyValuePair<string, string>> rdata = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<string, SortedList<string, string>> kvp in ProfileValues)
            {
                string key = !String.IsNullOrWhiteSpace(kvp.Key) ? kvp.Key : "";
                SortedList<string, string> svals = kvp.Value;
                if (svals.ContainsKey(""))
                    rdata.Add(new KeyValuePair<string, string>(key, svals[key]));
                else
                    rdata.Add(new KeyValuePair<string, string>(key, ""));
                foreach (string skey in svals.Keys.Where(t=>!String.IsNullOrWhiteSpace(t)))
                    rdata.Add(new KeyValuePair<string, string>(skey, svals[skey]));
            }
            return rdata;
        }
    }
}
