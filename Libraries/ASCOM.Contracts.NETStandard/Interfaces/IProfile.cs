// Decompiled with JetBrains decompiler
// Type: ASCOM.Utilities.Interfaces.IProfile
// Assembly: ASCOM.Utilities, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 06F2ED6E-C559-496D-B7FF-DCA800F2230D
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.Utilities\6.0.0.0__565de7938946fba7\ASCOM.Utilities.dll

using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using ASCOM.Utilities.Interfaces;
using System.Collections;

namespace ASCOM.Utilities.Interfaces
{

    //[ComVisible(true)]
    //[Guid("3503C303-B268-4da8-A0AA-CD6530B802AA")]
    public interface IProfile
    {
        [DispId(1)]
        string DeviceType { get; set; }

        [DispId(2)]
        ArrayList RegisteredDeviceTypes { get; }

        [DispId(3)]
        ArrayList RegisteredDevices(string DeviceType);

        [DispId(4)]
        bool IsRegistered(string DriverID);

        [DispId(5)]
        void Register(string DriverID, string DescriptiveName);

        [DispId(6)]
        void Unregister(string DriverID);

        [DispId(7)]
        string GetValue(string DriverID, string Name, string SubKey, string DefaultValue);

        [DispId(8)]
        void WriteValue(string DriverID, string Name, string Value, string SubKey);

        [DispId(9)]
        ArrayList Values(string DriverID, string SubKey);

        [DispId(10)]
        void DeleteValue(string DriverID, string Name, string SubKey);

        [DispId(11)]
        void CreateSubKey(string DriverID, string SubKey);

        [DispId(12)]
        ArrayList SubKeys(string DriverID, string SubKey);

        [DispId(13)]
        void DeleteSubKey(string DriverID, string SubKey);

        [DispId(14)]
        string GetProfileXML(string deviceId);

        [DispId(15)]
        void SetProfileXML(string deviceId, string xml);
    }


}
