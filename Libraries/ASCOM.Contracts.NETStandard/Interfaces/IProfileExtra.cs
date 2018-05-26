// Decompiled with JetBrains decompiler
// Type: ASCOM.Utilities.Interfaces.IProfileExtra
// Assembly: ASCOM.Utilities, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 06F2ED6E-C559-496D-B7FF-DCA800F2230D
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.Utilities\6.0.0.0__565de7938946fba7\ASCOM.Utilities.dll

using System.Collections;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace ASCOM.Utilities.Interfaces
{
  //[ComVisible(false)]
  public interface IProfileExtra
  {
    [EditorBrowsable(EditorBrowsableState.Never)]
    void MigrateProfile(string CurrentPlatformVersion);

    void DeleteValue(string DriverID, string Name);

    string GetValue(string DriverID, string Name);

    string GetValue(string DriverID, string Name, string SubKey);

    ArrayList SubKeys(string DriverID);

    ArrayList Values(string DriverID);

    void WriteValue(string DriverID, string Name, string Value);

    IASCOMProfile GetProfile(string DriverId);

    void SetProfile(string DriverId, IASCOMProfile XmlProfileKey);
  }
}
