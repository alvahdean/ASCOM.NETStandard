// Decompiled with JetBrains decompiler
// Type: ASCOM.Utilities.Interfaces.IAccess
// Assembly: ASCOM.Utilities, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 06F2ED6E-C559-496D-B7FF-DCA800F2230D
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.Utilities\6.0.0.0__565de7938946fba7\ASCOM.Utilities.dll


using System.Collections.Generic;

namespace ASCOM.Utilities.Interfaces
{
#warning Internal interface exposed as public during porting: IAccess
    //internal interface IAccess
    public interface IAccess
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

    //public interface IAccessExtra
    //{
    //    void CreateKey<TNode>(string p_SubKeyName) where TNode : class, IProfileNode, new();
    //    bool KeyExists(string nodePath);
    //    bool ValueExists(string valuePath);
    //    T GetProfile<T>(string p_SubKeyName, string p_ValueName) where T: struct;
    //    T GetProfile<T>(string p_SubKeyName, string p_ValueName, T p_DefaultValue) where T : struct;
    //    void WriteProfile<T>(string p_SubKeyName, string p_ValueName, T p_ValueData) where T : struct;
    //}
}
