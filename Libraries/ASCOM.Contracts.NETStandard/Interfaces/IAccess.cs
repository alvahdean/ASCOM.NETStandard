// Decompiled with JetBrains decompiler
// Type: ASCOM.Utilities.Interfaces.IAccess
// Assembly: ASCOM.Utilities, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 06F2ED6E-C559-496D-B7FF-DCA800F2230D
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.Utilities\6.0.0.0__565de7938946fba7\ASCOM.Utilities.dll

using System.Collections.Generic;

namespace ASCOM.Utilities.Interfaces
{
#warning Internal interface exposed as public during porting: IAccess
    public interface IAccess
    //internal interface IAccess
    {
    string GetProfile(string p_SubKeyName, string p_ValueName);

    string GetProfile(string p_SubKeyName, string p_ValueName, string p_DefaultValue);

    void WriteProfile(string p_SubKeyName, string p_ValueName, string p_ValueData);

    SortedList<string, string> EnumProfile(string p_SubKeyName);

    void DeleteProfile(string p_SubKeyName, string p_ValueName);

    void CreateKey(string p_SubKeyName);

    SortedList<string, string> EnumKeys(string p_SubKeyName);

    void DeleteKey(string p_SubKeyName);

    void RenameKey(string CurrentSubKeyName, string NewSubKeyName);

    void MigrateProfile(string CurrentPlatformVersion);

    void SetProfile(string DriverId, IASCOMProfile XmlProfile);

    IASCOMProfile GetProfile(string DriverId);
  }
}
