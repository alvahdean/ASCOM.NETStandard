using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Schema;

namespace ASCOM.Utilities
{
    public interface IASCOMProfile
    {
        SortedList<string, SortedList<string, string>> ProfileValues { get; }

        void AddSubkey(string SubKeyName);
        void Clear();
        XmlSchema GetSchema();
        string GetValue(string Name);
        string GetValue(string Name, string SubKeyName);
        
        void RemoveSubKey(string SubKeyName);
        void RemoveValue(string ValueName);
        void RemoveValue(string ValueName, string SubKeyName);
        void SetValue(string Name, string Value);
        void SetValue(string Name, string Value, string SubKeyName);
        void ReadXml(XmlReader reader);
        void WriteXml(XmlWriter writer);
        void ReadJson(StreamReader reader);
        void WriteJson(StreamWriter writer);

        Dictionary<string, Dictionary<string, string>> ToDictionary();

    }
}