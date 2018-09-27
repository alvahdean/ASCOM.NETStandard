// Decompiled with JetBrains decompiler
// Type: ASCOM.Utilities.UtilitiesSettings
// Assembly: ASCOM.Utilities, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 06F2ED6E-C559-496D-B7FF-DCA800F2230D
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.Utilities\6.0.0.0__565de7938946fba7\ASCOM.Utilities.dll

//using Microsoft.VisualBasic.CompilerServices;
using Microsoft.Win32;
using System;
using System.IO;

namespace ASCOM.Utilities
{
  internal class UtilitiesSettings : IDisposable
  {
    private RegistryKey m_HKCU;
    private RegistryKey m_SettingsKey;
    private const string REGISTRY_CONFORM_FOLDER = "Software\\ASCOM\\Utilities";
    private const string TRACE_XMLACCESS = "Trace XMLAccess";
    private const bool TRACE_XMLACCESS_DEFAULT = false;
    private const string TRACE_PROFILE = "Trace Profile";
    private const bool TRACE_PROFILE_DEFAULT = false;
    private const string PROFILE_ROOT_EDIT = "Profile Root Edit";
    private const bool PROFILE_ROOT_EDIT_DEFAULT = false;
    private bool disposedValue;

    public bool TraceXMLAccess
    {
      get
      {
        return this.GetBool("Trace XMLAccess", false);
      }
      set
      {
        this.SetName(this.m_SettingsKey, "Trace XMLAccess", value.ToString());
      }
    }

    public bool TraceProfile
    {
      get
      {
        return this.GetBool("Trace Profile", false);
      }
      set
      {
        this.SetName(this.m_SettingsKey, "Trace Profile", value.ToString());
      }
    }

    public bool ProfileRootEdit
    {
      get
      {
        return this.GetBool("Profile Root Edit", false);
      }
      set
      {
        this.SetName(this.m_SettingsKey, "Profile Root Edit", value.ToString());
      }
    }

    public UtilitiesSettings()
    {
      this.disposedValue = false;
      this.m_HKCU = Registry.CurrentUser;
      this.m_HKCU.CreateSubKey("Software\\ASCOM\\Utilities");
      this.m_SettingsKey = this.m_HKCU.OpenSubKey("Software\\ASCOM\\Utilities", true);
    }

    ~UtilitiesSettings()
    {
    }

    protected virtual void Dispose(bool disposing)
    {
      if (!this.disposedValue)
      {
        int num = disposing ? 1 : 0;
        this.m_SettingsKey.Flush();
        this.m_SettingsKey.Close();
        this.m_SettingsKey = (RegistryKey) null;
        this.m_HKCU.Flush();
        this.m_HKCU.Close();
        this.m_HKCU = (RegistryKey) null;
      }
      this.disposedValue = true;
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    private bool GetBool(string p_Name, bool p_DefaultValue)
    {
      bool flag=false;
      try
      {
        if (this.m_SettingsKey.GetValueKind(p_Name) == RegistryValueKind.String)
          flag = Conversions.ToBoolean(this.m_SettingsKey.GetValue(p_Name));
      }
      catch (IOException ex)
      {
        //ProjectData.SetProjectError((Exception) ex);
        this.SetName(this.m_SettingsKey, p_Name, p_DefaultValue.ToString());
        flag = p_DefaultValue;
        //ProjectData.ClearProjectError();
      }
      catch (Exception ex)
      {
        //ProjectData.SetProjectError(ex);
        flag = p_DefaultValue;
        //ProjectData.ClearProjectError();
      }
      return flag;
    }

    private string GetString(string p_Name, string p_DefaultValue)
    {
      string str = "";
      try
      {
        if (this.m_SettingsKey.GetValueKind(p_Name) == RegistryValueKind.String)
          str = this.m_SettingsKey.GetValue(p_Name).ToString();
      }
      catch (IOException ex)
      {
        //ProjectData.SetProjectError((Exception) ex);
        this.SetName(this.m_SettingsKey, p_Name, p_DefaultValue.ToString());
        str = p_DefaultValue;
        //ProjectData.ClearProjectError();
      }
      catch (Exception ex)
      {
        //ProjectData.SetProjectError(ex);
        str = p_DefaultValue;
        //ProjectData.ClearProjectError();
      }
      return str;
    }

    private double GetDouble(RegistryKey p_Key, string p_Name, double p_DefaultValue)
    {
            double num = 0d;
      try
      {
        if (p_Key.GetValueKind(p_Name) == RegistryValueKind.String)
          num = Conversions.ToDouble(p_Key.GetValue(p_Name));
      }
      catch (IOException ex)
      {
        //ProjectData.SetProjectError((Exception) ex);
        this.SetName(p_Key, p_Name, p_DefaultValue.ToString());
        num = p_DefaultValue;
        //ProjectData.ClearProjectError();
      }
      catch (Exception ex)
      {
        //ProjectData.SetProjectError(ex);
        num = p_DefaultValue;
        //ProjectData.ClearProjectError();
      }
      return num;
    }

    private DateTime GetDate(string p_Name, DateTime p_DefaultValue)
    {
      DateTime dateTime=default(DateTime);
      try
      {
        if (this.m_SettingsKey.GetValueKind(p_Name) == RegistryValueKind.String)
          dateTime = Conversions.ToDate(this.m_SettingsKey.GetValue(p_Name));
      }
      catch (IOException ex)
      {
        //ProjectData.SetProjectError((Exception) ex);
        this.SetName(this.m_SettingsKey, p_Name, p_DefaultValue.ToString());
        dateTime = p_DefaultValue;
        //ProjectData.ClearProjectError();
      }
      catch (Exception ex)
      {
        //ProjectData.SetProjectError(ex);
        dateTime = p_DefaultValue;
        //ProjectData.ClearProjectError();
      }
      return dateTime;
    }

    private void SetName(RegistryKey p_Key, string p_Name, string p_Value)
    {
      p_Key.SetValue(p_Name, (object) p_Value.ToString(), RegistryValueKind.String);
      p_Key.Flush();
    }
  }
}
