// Decompiled with JetBrains decompiler
// Type: ASCOM.Utilities.XMLAccess
// Assembly: ASCOM.Utilities, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 06F2ED6E-C559-496D-B7FF-DCA800F2230D
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.Utilities\6.0.0.0__565de7938946fba7\ASCOM.Utilities.dll

using ASCOM.Utilities.Exceptions;
using ASCOM.Utilities.Interfaces;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Xml;



namespace ASCOM.Utilities
{
  internal class XMLAccess : IAccess, IDisposable
  {
    private const int RETRY_MAX = 1;
    private const int RETRY_INTERVAL = 200;
    private IFileStoreProvider FileStore;
    private bool disposedValue;
    private Mutex ProfileMutex;
    private bool GotMutex;
    private TraceLogger TL;
    private Stopwatch sw;
    private Stopwatch swSupport;
    [SpecialName]
    private int __STATIC__MigrateKey__2021128081E__RecurseDepth;

    public XMLAccess()
      : this(false)
    {
    }

    public XMLAccess(string p_CallingComponent)
      : this(false)
    {
    }

    public XMLAccess(bool p_IgnoreTest)
    {
      this.disposedValue = false;
      this.TL = new TraceLogger("", "XMLAccess");
      this.TL.Enabled = RegistryCommonCode.GetBool("Trace XMLAccess", false);
      VersionCode.RunningVersions(this.TL);
      this.sw = new Stopwatch();
      this.swSupport = new Stopwatch();
      this.FileStore = (IFileStoreProvider) new AllUsersFileSystemProvider();
      this.ProfileMutex = new Mutex(false, "ASCOMProfileMutex");
      if (p_IgnoreTest)
        return;
      try
      {
        if (!this.FileStore.get_Exists("\\Profile.xml"))
          throw new ProfileNotFoundException("Utilities Error Base key does not exist");
        this.GetProfile("\\", "PlatformVersion");
      }
      catch (Exception ex)
      {
        //ProjectData.SetProjectError(ex);
        this.TL.LogMessageCrLf("XMLAccess.New Unexpected exception:", ex.ToString());
        throw;
      }
    }

    ~XMLAccess()
    {
      this.Dispose(false);
      // ISSUE: explicit finalizer call
      //base.Finalize();
    }

    protected virtual void Dispose(bool disposing)
    {
      if (!this.disposedValue)
      {
        try
        {
          this.FileStore = (IFileStoreProvider) null;
          this.TL.Enabled = false;
          this.TL.Dispose();
          this.TL = (TraceLogger) null;
          this.sw.Stop();
          this.sw = (Stopwatch) null;
          this.swSupport.Stop();
          this.swSupport = (Stopwatch) null;
          this.ProfileMutex.Close();
          this.ProfileMutex = (Mutex) null;
        }
        catch (Exception ex)
        {
          //ProjectData.SetProjectError(ex);
          int num = (int) Interaction.MsgBox((object) ("XMLAccess:Dispose Exception - " + ex.ToString()), MsgBoxStyle.OkOnly, (object) null);
          //ProjectData.ClearProjectError();
        }
      }
      this.disposedValue = true;
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    public void CreateKey(string p_SubKeyName)
    {
      SortedList<string, string> p_KeyValuePairs = new SortedList<string, string>();
      try
      {
        this.GetProfileMutex("CreateKey", p_SubKeyName);
        this.sw.Reset();
        this.sw.Start();
        this.TL.LogMessage("CreateKey", "SubKey: \"" + p_SubKeyName + "\"");
        p_SubKeyName = Strings.Trim(p_SubKeyName);
        string[] strArray = Strings.Split(p_SubKeyName, "\\", -1, CompareMethod.Text);
        string Left = p_SubKeyName;
        if (Operators.CompareString(Left, "", false) != 0)
        {
          if (Operators.CompareString(Left, "\\", false) == 0)
          {
            if (!this.FileStore.get_Exists("\\Profile.xml"))
            {
              this.TL.LogMessage("  CreateKey", "  Creating root key \"\\\"");
              p_KeyValuePairs.Clear();
              p_KeyValuePairs.Add("***** DefaultValueName *****", "===== ***** UnsetValue ***** =====");
              this.WriteValues("\\", ref p_KeyValuePairs, false);
            }
            else
              this.TL.LogMessage("  CreateKey", "  Root key alread exists");
          }
          else
          {
            int num1 = 0;
            int num2 = checked (strArray.Length - 1);
            int num3 = num1;
            while (num3 <= num2)
            {
              string p_SubKeyName1 = "";
              int num4 = 0;
              int num5 = num3;
              int index = num4;
              while (index <= num5)
              {
                p_SubKeyName1 = p_SubKeyName1 + "\\" + strArray[index];
                checked { ++index; }
              }
              if (!this.FileStore.get_Exists(p_SubKeyName1 + "\\Profile.xml"))
              {
                this.FileStore.CreateDirectory(p_SubKeyName1, this.TL);
                p_KeyValuePairs.Clear();
                p_KeyValuePairs.Add("***** DefaultValueName *****", "===== ***** UnsetValue ***** =====");
                this.WriteValues(p_SubKeyName1, ref p_KeyValuePairs, false);
              }
              checked { ++num3; }
            }
          }
        }
        this.sw.Stop();
        this.TL.LogMessage("  ElapsedTime", "  " + Conversions.ToString(this.sw.ElapsedMilliseconds) + " milliseconds");
      }
      finally
      {
        this.ProfileMutex.ReleaseMutex();
      }
    }

        public void DeleteKey(string p_SubKeyName)
    {
      try
      {
        this.GetProfileMutex("DeleteKey", p_SubKeyName);
        this.sw.Reset();
        this.sw.Start();
        this.TL.LogMessage("DeleteKey", "SubKey: \"" + p_SubKeyName + "\"");
        try
        {
          this.FileStore.DeleteDirectory(p_SubKeyName);
        }
        catch (Exception ex)
        {
          //ProjectData.SetProjectError(ex);
          //ProjectData.ClearProjectError();
        }
        this.sw.Stop();
        this.TL.LogMessage("  ElapsedTime", "  " + Conversions.ToString(this.sw.ElapsedMilliseconds) + " milliseconds");
      }
      finally
      {
        this.ProfileMutex.ReleaseMutex();
      }
    }

        public void RenameKey(string CurrentSubKeyName, string NewSubKeyName)
    {
      try
      {
        this.GetProfileMutex("RenameKey", CurrentSubKeyName + " " + NewSubKeyName);
        this.sw.Reset();
        this.sw.Start();
        this.TL.LogMessage("RenameKey", "Current SubKey: \"" + CurrentSubKeyName + "\" New SubKey: \"" + NewSubKeyName + "\"");
        this.FileStore.RenameDirectory(CurrentSubKeyName, NewSubKeyName);
        this.sw.Stop();
        this.TL.LogMessage("  ElapsedTime", "  " + Conversions.ToString(this.sw.ElapsedMilliseconds) + " milliseconds");
      }
      finally
      {
        this.ProfileMutex.ReleaseMutex();
      }
    }

        public void DeleteProfile(string p_SubKeyName, string p_ValueName)
    {
      try
      {
        this.GetProfileMutex("DeleteProfile", p_SubKeyName + " " + p_ValueName);
        this.sw.Reset();
        this.sw.Start();
        this.TL.LogMessage("DeleteProfile", "SubKey: \"" + p_SubKeyName + "\" Name: \"" + p_ValueName + "\"");
        SortedList<string, string> p_KeyValuePairs = this.ReadValues(p_SubKeyName);
        try
        {
          if (Operators.CompareString(p_ValueName, "", false) == 0)
          {
            p_KeyValuePairs["***** DefaultValueName *****"] = "===== ***** UnsetValue ***** =====";
            this.TL.LogMessage("DeleteProfile", "  Default name was changed to unset value");
          }
          else
          {
            p_KeyValuePairs.Remove(p_ValueName);
            this.TL.LogMessage("DeleteProfile", "  Value was deleted");
          }
        }
        catch (Exception ex)
        {
          //ProjectData.SetProjectError(ex);
          this.TL.LogMessage("DeleteProfile", "  Value did not exist");
          //ProjectData.ClearProjectError();
        }
        this.WriteValues(p_SubKeyName, ref p_KeyValuePairs);
        p_KeyValuePairs = (SortedList<string, string>) null;
        this.sw.Stop();
        this.TL.LogMessage("  ElapsedTime", "  " + Conversions.ToString(this.sw.ElapsedMilliseconds) + " milliseconds");
      }
      finally
      {
        this.ProfileMutex.ReleaseMutex();
      }
    }

        public SortedList<string, string> EnumKeys(string p_SubKeyName)
    {
      SortedList<string, string> sortedList = new SortedList<string, string>();
      try
      {
        this.GetProfileMutex("EnumKeys", p_SubKeyName);
        this.sw.Reset();
        this.sw.Start();
        this.TL.LogMessage("EnumKeys", "SubKey: \"" + p_SubKeyName + "\"");
        string[] strArray = this.FileStore.get_GetDirectoryNames(p_SubKeyName);
        int index = 0;
        while (index < strArray.Length)
        {
          string key = strArray[index];
          try
          {
            string Left = this.ReadValues(p_SubKeyName + "\\" + key)["***** DefaultValueName *****"];
            if (Operators.CompareString(Left, "===== ***** UnsetValue ***** =====", false) == 0)
              Left = "";
            sortedList.Add(key, Left);
          }
          catch (Exception ex)
          {
            //ProjectData.SetProjectError(ex);
            //ProjectData.ClearProjectError();
          }
          checked { ++index; }
        }
        this.sw.Stop();
        this.TL.LogMessage("  ElapsedTime", "  " + Conversions.ToString(this.sw.ElapsedMilliseconds) + " milliseconds");
      }
      finally
      {
        this.ProfileMutex.ReleaseMutex();
      }
      return sortedList;
    }

    public SortedList<string, string> EnumProfile(string p_SubKeyName)
    {
      SortedList<string, string> sortedList1 = new SortedList<string, string>();
      try
      {
        this.GetProfileMutex("EnumProfile", p_SubKeyName);
        this.sw.Reset();
        this.sw.Start();
        this.TL.LogMessage("EnumProfile", "SubKey: \"" + p_SubKeyName + "\"");
        SortedList<string, string> sortedList2 = this.ReadValues(p_SubKeyName);
        IEnumerator<System.Collections.Generic.KeyValuePair<string, string>> enumerator=null;
        try
        {
          enumerator = sortedList2.GetEnumerator();
          while (enumerator.MoveNext())
          {
            System.Collections.Generic.KeyValuePair<string, string> current = enumerator.Current;
            if (Operators.CompareString(current.Key, "***** DefaultValueName *****", false) == 0)
            {
              if (Operators.CompareString(current.Value, "===== ***** UnsetValue ***** =====", false) != 0)
                sortedList1.Add("", current.Value);
            }
            else
              sortedList1.Add(current.Key, current.Value);
          }
        }
        finally
        {
          if (enumerator != null)
            enumerator.Dispose();
        }
        this.sw.Stop();
        this.TL.LogMessage("  ElapsedTime", "  " + Conversions.ToString(this.sw.ElapsedMilliseconds) + " milliseconds");
      }
      finally
      {
        this.ProfileMutex.ReleaseMutex();
      }
      return sortedList1;
    }

    public string GetProfile(string p_SubKeyName, string p_ValueName, string p_DefaultValue)
    {
      string str;
      try
      {
        this.GetProfileMutex("GetProfile", p_SubKeyName + " " + p_ValueName + " " + p_DefaultValue);
        this.sw.Reset();
        this.sw.Start();
        this.TL.LogMessage("GetProfile", "SubKey: \"" + p_SubKeyName + "\" Name: \"" + p_ValueName + "\"\" DefaultValue: \"" + p_DefaultValue + "\"");
        str = "";
        try
        {
          SortedList<string, string> sortedList = this.ReadValues(p_SubKeyName);
          str = Operators.CompareString(p_ValueName, "", false) != 0 ? sortedList[p_ValueName] : sortedList["***** DefaultValueName *****"];
        }
        catch (KeyNotFoundException ex)
        {
          //ProjectData.SetProjectError((Exception) ex);
          if (p_DefaultValue != null)
          {
            this.WriteProfile(p_SubKeyName, p_ValueName, p_DefaultValue);
            str = p_DefaultValue;
            this.TL.LogMessage("GetProfile", "Value not yet set, returning supplied default value: " + p_DefaultValue);
          }
          else
            this.TL.LogMessage("GetProfile", "Value not yet set and no default value supplied, returning null string");
          //ProjectData.ClearProjectError();
        }
        catch (Exception ex)
        {
          //ProjectData.SetProjectError(ex);
          Exception exception = ex;
          if (p_DefaultValue != null)
          {
            this.WriteProfile(p_SubKeyName, p_ValueName, p_DefaultValue);
            str = p_DefaultValue;
            this.TL.LogMessage("GetProfile", "Key not yet set, returning supplied default value: " + p_DefaultValue);
            //ProjectData.ClearProjectError();
          }
          else
          {
            this.TL.LogMessage("GetProfile", "Key not yet set and no default value supplied, throwing exception: " + exception.Message);
            throw;
          }
        }
        this.sw.Stop();
        this.TL.LogMessage("  ElapsedTime", "  " + Conversions.ToString(this.sw.ElapsedMilliseconds) + " milliseconds");
      }
      finally
      {
        this.ProfileMutex.ReleaseMutex();
      }
      return str;
    }

        public string GetProfile(string p_SubKeyName, string p_ValueName)
    {
      return this.GetProfile(p_SubKeyName, p_ValueName, (string) null);
    }

        public void WriteProfile(string p_SubKeyName, string p_ValueName, string p_ValueData)
    {
      try
      {
        this.GetProfileMutex("WriteProfile", p_SubKeyName + " " + p_ValueName + " " + p_ValueData);
        this.sw.Reset();
        this.sw.Start();
        this.TL.LogMessage("WriteProfile", "SubKey: \"" + p_SubKeyName + "\" Name: \"" + p_ValueName + "\" Value: \"" + p_ValueData + "\"");
        if (!this.FileStore.get_Exists(p_SubKeyName + "\\Profile.xml"))
          this.CreateKey(p_SubKeyName);
        SortedList<string, string> p_KeyValuePairs = this.ReadValues(p_SubKeyName);
        if (Operators.CompareString(p_ValueName, "", false) == 0)
        {
          if (p_KeyValuePairs.ContainsKey("***** DefaultValueName *****"))
            p_KeyValuePairs["***** DefaultValueName *****"] = p_ValueData;
          else
            p_KeyValuePairs.Add("***** DefaultValueName *****", p_ValueData);
          this.WriteValues(p_SubKeyName, ref p_KeyValuePairs);
        }
        else
        {
          if (p_KeyValuePairs.ContainsKey(p_ValueName))
            p_KeyValuePairs.Remove(p_ValueName);
          p_KeyValuePairs.Add(p_ValueName, p_ValueData);
          this.WriteValues(p_SubKeyName, ref p_KeyValuePairs);
        }
        p_KeyValuePairs = (SortedList<string, string>) null;
        this.sw.Stop();
        this.TL.LogMessage("  ElapsedTime", "  " + Conversions.ToString(this.sw.ElapsedMilliseconds) + " milliseconds");
      }
      finally
      {
        this.ProfileMutex.ReleaseMutex();
      }
    }

    internal void SetSecurityACLs()
    {
      try
      {
        this.GetProfileMutex("SetSecurityACLs", "");
        this.sw.Reset();
        this.sw.Start();
        this.TL.LogMessage("SetSecurityACLs", "");
        bool enabled = this.TL.Enabled;
        this.TL.Enabled = true;
        VersionCode.RunningVersions(this.TL);
        this.TL.LogMessage("SetSecurityACLs", "Setting security ACLs on ASCOM root directory ");
        this.FileStore.SetSecurityACLs(this.TL);
        this.sw.Stop();
        this.TL.LogMessage("  ElapsedTime", "  " + Conversions.ToString(this.sw.ElapsedMilliseconds) + " milliseconds");
        this.TL.Enabled = enabled;
      }
      catch (Exception ex)
      {
        //ProjectData.SetProjectError(ex);
        this.TL.LogMessageCrLf("SetSecurityACLs", "Exception: " + ex.ToString());
        throw;
      }
    }

    void IAccess.MigrateProfile(string CurrentPlatformVersion)
    {
      try
      {
        this.GetProfileMutex("MigrateProfile", "");
        this.sw.Reset();
        this.sw.Start();
        this.TL.LogMessage("MigrateProfile", "");
        bool enabled = this.TL.Enabled;
        this.TL.Enabled = true;
        VersionCode.RunningVersions(this.TL);
        this.TL.LogMessage("MigrateProfile", "Migrating keys");
        if (!this.FileStore.get_Exists("\\Profile.xml"))
        {
          this.FileStore.CreateDirectory("\\", this.TL);
          this.CreateKey("\\");
          this.TL.LogMessage("MigrateProfile", "Successfully created root directory and root key");
        }
        else
          this.TL.LogMessage("MigrateProfile", "Root directory already exists");
        this.TL.LogMessage("MigrateProfile", "Setting security ACLs on ASCOM root directory ");
        this.FileStore.SetSecurityACLs(this.TL);
        this.TL.LogMessage("MigrateProfile", "Copying Profile from Registry");
        RegistryKey p_FromKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\ASCOM");
        if (p_FromKey == null)
          throw new ProfileNotFoundException("Cannot find ASCOM Profile in HKLM\\SOFTWARE\\ASCOM Is Platform 5 installed?");
        this.TL.LogMessage("MigrateProfile", "FromKey Opened OK: " + p_FromKey.Name + ", SubKeyCount: " + p_FromKey.SubKeyCount.ToString() + ", ValueCount: " + p_FromKey.ValueCount.ToString());
        this.MigrateKey(p_FromKey, "");
        this.TL.LogMessage("MigrateProfile", "Successfully migrated keys");
        p_FromKey.Close();
        this.TL.Enabled = RegistryCommonCode.GetBool("Trace XMLAccess", false);
        this.sw.Stop();
        this.TL.LogMessage("  ElapsedTime", "  " + Conversions.ToString(this.sw.ElapsedMilliseconds) + " milliseconds");
        this.TL.Enabled = enabled;
      }
      catch (Exception ex)
      {
        //ProjectData.SetProjectError(ex);
        this.TL.LogMessageCrLf("MigrateProfile", "Exception: " + ex.ToString());
        throw;
      }
    }

    public ASCOMProfile GetProfileXML(string DriverId)
    {
      throw new MethodNotImplementedException("XMLAccess:GetProfileXml");
    }

        public void SetProfileXML(string DriverId, ASCOMProfile Profile)
    {
      throw new MethodNotImplementedException("XMLAccess:SetProfileXml");
    }

    private SortedList<string, string> ReadValues(string p_SubKeyName)
    {
      SortedList<string, string> sortedList = new SortedList<string, string>();
      bool flag1 = false;
      bool flag2 = false;
      this.swSupport.Reset();
      this.swSupport.Start();
      if (Operators.CompareString(Strings.Left(p_SubKeyName, 1), "\\", false) != 0)
        p_SubKeyName = "\\" + p_SubKeyName;
      this.TL.LogMessage("  ReadValues", "  SubKeyName: " + p_SubKeyName);
      string Left = "Profile.xml";
      int num = -1;
      bool flag3 = this.FileStore.get_Exists(p_SubKeyName + "\\Profile.xml");
      bool flag4 = this.FileStore.get_Exists(p_SubKeyName + "\\ProfileOriginal.xml");
      this.FileStore.get_Exists(p_SubKeyName + "\\ProfileNew.xml");
      if (!flag3 & !flag4)
        throw new ProfileNotFoundException("No profile files exist for this key: " + p_SubKeyName);
      do
      {
        checked { ++num; }
        try
        {
          using (XmlReader xmlReader = XmlReader.Create(this.FileStore.get_FullPath(p_SubKeyName + "\\" + Left), new XmlReaderSettings()
          {
            IgnoreWhitespace = true
          }))
          {
            xmlReader.Read();
            xmlReader.Read();
            while (xmlReader.Read())
            {
              if (xmlReader.NodeType == XmlNodeType.Element)
              {
                string name = xmlReader.Name;
                if (Operators.CompareString(name, "DefaultElement", false) == 0)
                {
                  sortedList.Add("***** DefaultValueName *****", xmlReader.GetAttribute("Value"));
                  this.TL.LogMessage("    ReadValues", "    found ***** DefaultValueName ***** = " + sortedList["***** DefaultValueName *****"]);
                }
                else if (Operators.CompareString(name, "Element", false) == 0)
                {
                  string attribute = xmlReader.GetAttribute("Name");
                  sortedList.Add(attribute, xmlReader.GetAttribute("Value"));
                  this.TL.LogMessage("    ReadValues", "    found " + attribute + " = " + sortedList[attribute]);
                }
                else
                  this.TL.LogMessage("    ReadValues", "    ## Found unexpected Reader.Name: " + xmlReader.Name.ToString());
              }
            }
            xmlReader.Close();
          }
          this.swSupport.Stop();
          this.TL.LogMessage("  ReadValues", "  added to cache - " + Conversions.ToString(this.swSupport.ElapsedMilliseconds) + " milliseconds");
          flag1 = true;
        }
        catch (Exception ex)
        {
          //ProjectData.SetProjectError(ex);
          Exception inner = ex;
          flag2 = true;
          if (num == 1)
          {
            if (Operators.CompareString(Left, "Profile.xml", false) == 0)
            {
              Left = "ProfileOriginal.xml";
              num = -1;
              EventLogCode.LogEvent("XMLAccess:ReadValues", "Error reading profile on final retry - attempting recovery from previous version", EventLogEntryType.Warning, GlobalConstants.EventLogErrors.XMLAccessRecoveryPreviousVersion, inner.ToString());
              this.TL.LogMessageCrLf("  ReadValues", "Final retry exception - attempting recovery from previous version: " + inner.ToString());
            }
            else
            {
              EventLogCode.LogEvent("XMLAccess:ReadValues", "Error reading profile on final retry", EventLogEntryType.Error, GlobalConstants.EventLogErrors.XMLAccessReadError, inner.ToString());
              this.TL.LogMessageCrLf("  ReadValues", "Final retry exception: " + inner.ToString());
              throw new ProfilePersistenceException("XMLAccess Exception", inner);
            }
          }
          else
          {
            EventLogCode.LogEvent("XMLAccess:ReadValues", "Error reading profile - retry: " + Conversions.ToString(num), EventLogEntryType.Warning, GlobalConstants.EventLogErrors.XMLAccessRecoveryPreviousVersion, inner.Message);
            this.TL.LogMessageCrLf("  ReadValues", "Retry " + Conversions.ToString(num) + " exception: " + inner.ToString());
          }
          //ProjectData.ClearProjectError();
        }
        if (flag2)
          Thread.Sleep(200);
      }
      while (!flag1);
      if (flag2)
      {
        EventLogCode.LogEvent("XMLAccess:ReadValues", "Recovered from read error OK", EventLogEntryType.SuccessAudit, GlobalConstants.EventLogErrors.XMLAccessRecoveredOK, (string) null);
        this.TL.LogMessage("  ReadValues", "Recovered from read error OK");
      }
      return sortedList;
    }

    private void WriteValues(string p_SubKeyName, ref SortedList<string, string> p_KeyValuePairs)
    {
      this.WriteValues(p_SubKeyName, ref p_KeyValuePairs, true);
    }

    private void WriteValues(string p_SubKeyName, ref SortedList<string, string> p_KeyValuePairs, bool p_CheckForCurrentProfileStore)
    {
      this.swSupport.Reset();
      this.swSupport.Start();
      this.TL.LogMessage("  WriteValues", "  SubKeyName: " + p_SubKeyName);
      if (Operators.CompareString(Strings.Left(p_SubKeyName, 1), "\\", false) != 0)
        p_SubKeyName = "\\" + p_SubKeyName;
      try
      {
        int num1 = 0;
        IEnumerator<System.Collections.Generic.KeyValuePair<string, string>> enumerator1=null;
        try
        {
          enumerator1 = p_KeyValuePairs.GetEnumerator();
          while (enumerator1.MoveNext())
          {
            System.Collections.Generic.KeyValuePair<string, string> current = enumerator1.Current;
            checked { ++num1; }
            this.TL.LogMessage("  WriteValues List", "  " + num1.ToString() + " " + current.Key + " = " + current.Value);
          }
        }
        finally
        {
          if (enumerator1 != null)
            enumerator1.Dispose();
        }
        XmlWriterSettings settings = new XmlWriterSettings();
        settings.Indent = true;
        FileStream fileStream = new FileStream(this.FileStore.get_FullPath(p_SubKeyName + "\\ProfileNew.xml"), FileMode.Create, FileAccess.Write, FileShare.None, 2048, FileOptions.WriteThrough);
        XmlWriter xmlWriter = XmlWriter.Create((Stream) fileStream, settings);
        using (xmlWriter)
        {
          xmlWriter.WriteStartDocument();
          xmlWriter.WriteStartElement("Profile");
          xmlWriter.WriteStartElement("DefaultElement");
          xmlWriter.WriteAttributeString("Value", p_KeyValuePairs["***** DefaultValueName *****"]);
          xmlWriter.WriteEndElement();
          int num2 = 0;
          IEnumerator<System.Collections.Generic.KeyValuePair<string, string>> enumerator2 = null;
          try
          {
            enumerator2 = p_KeyValuePairs.GetEnumerator();
            while (enumerator2.MoveNext())
            {
              System.Collections.Generic.KeyValuePair<string, string> current = enumerator2.Current;
              checked { ++num2; }
              this.TL.LogMessage("  Writing Value", "  " + num2.ToString() + " " + current.Key + " = " + current.Value);
              if (current.Value == null)
                this.TL.LogMessage("  Writing Value", "  WARNING - Suppplied Value is Nothing not empty string");
              if (Operators.CompareString(current.Key, "***** DefaultValueName *****", false) != 0)
              {
                xmlWriter.WriteStartElement("Element");
                xmlWriter.WriteAttributeString("Name", current.Key);
                xmlWriter.WriteAttributeString("Value", current.Value);
                xmlWriter.WriteEndElement();
              }
            }
          }
          finally
          {
            if (enumerator2 != null)
              enumerator2.Dispose();
          }
          xmlWriter.WriteEndElement();
          xmlWriter.Close();
        }
        try
        {
          fileStream.Flush();
          fileStream.Close();
          fileStream.Dispose();
        }
        catch (Exception ex)
        {
          //ProjectData.SetProjectError(ex);
          //ProjectData.ClearProjectError();
        }
        try
        {
          if (p_CheckForCurrentProfileStore)
            this.FileStore.Rename(p_SubKeyName + "\\Profile.xml", p_SubKeyName + "\\ProfileOriginal.xml");
          try
          {
            this.FileStore.Rename(p_SubKeyName + "\\ProfileNew.xml", p_SubKeyName + "\\Profile.xml");
          }
          catch (Exception ex1)
          {
            //ProjectData.SetProjectError(ex1);
            Exception exception1 = ex1;
            this.TL.Enabled = true;
            this.TL.LogMessage("XMLAccess:WriteValues", "Unable to rename new profile file to current - " + p_SubKeyName + "\\ProfileNew.xmlto " + p_SubKeyName + "\\Profile.xml " + exception1.ToString());
            try
            {
              this.FileStore.Rename(p_SubKeyName + "\\ProfileOriginal.xml", p_SubKeyName + "\\Profile.xml");
            }
            catch (Exception ex2)
            {
              //ProjectData.SetProjectError(ex2);
              Exception exception2 = ex2;
              this.TL.Enabled = true;
              this.TL.LogMessage("XMLAccess:WriteValues", "Unable to rename original profile file to current - " + p_SubKeyName + "\\ProfileOriginal.xmlto " + p_SubKeyName + "\\Profile.xml " + exception2.ToString());
              //ProjectData.ClearProjectError();
            }
            //ProjectData.ClearProjectError();
          }
        }
        catch (Exception ex)
        {
          //ProjectData.SetProjectError(ex);
          Exception exception = ex;
          this.TL.Enabled = true;
          this.TL.LogMessage("XMLAccess:WriteValues", "Unable to rename current profile file to original - " + p_SubKeyName + "\\Profile.xmlto " + p_SubKeyName + "\\ProfileOriginal.xml " + exception.ToString());
          //ProjectData.ClearProjectError();
        }
        this.swSupport.Stop();
        this.TL.LogMessage("  WriteValues", "  Created cache entry " + p_SubKeyName + " - " + Conversions.ToString(this.swSupport.ElapsedMilliseconds) + " milliseconds");
      }
      catch (Exception ex)
      {
        //ProjectData.SetProjectError(ex);
        Exception exception = ex;
        this.TL.LogMessageCrLf("  WriteValues", "  Exception " + p_SubKeyName + " " + exception.ToString());
        int num = (int) Interaction.MsgBox((object) ("XMLAccess:Writevalues " + p_SubKeyName + " " + exception.ToString()), MsgBoxStyle.OkOnly, (object) null);
        //ProjectData.ClearProjectError();
      }
    }

    private void MigrateKey(RegistryKey p_FromKey, string p_ToDir)
    {
      SortedList<string, string> p_KeyValuePairs = new SortedList<string, string>();
      this.__STATIC__MigrateKey__2021128081E__RecurseDepth = checked (this.__STATIC__MigrateKey__2021128081E__RecurseDepth + 1);
      Stopwatch stopwatch = Stopwatch.StartNew();
      this.TL.LogMessage("MigrateKeys " + this.__STATIC__MigrateKey__2021128081E__RecurseDepth.ToString(), "To Directory: " + p_ToDir);
      try
      {
        this.TL.LogMessage("MigrateKeys" + this.__STATIC__MigrateKey__2021128081E__RecurseDepth.ToString(), "From Key: " + p_FromKey.Name + ", SubKeyCount: " + p_FromKey.SubKeyCount.ToString() + ", ValueCount: " + p_FromKey.ValueCount.ToString());
      }
      catch (Exception ex)
      {
        //ProjectData.SetProjectError(ex);
        Exception exception = ex;
        this.TL.LogMessage("MigrateKeys", "Exception processing \"" + p_ToDir + "\": " + exception.ToString());
        this.TL.LogMessage("MigrateKeys", "Exception above: no action taken, continuing...");
        //ProjectData.ClearProjectError();
      }
      string[] valueNames = p_FromKey.GetValueNames();
      p_KeyValuePairs.Add("***** DefaultValueName *****", "===== ***** UnsetValue ***** =====");
      string[] strArray = valueNames;
      int index1 = 0;
      while (index1 < strArray.Length)
      {
        string str = strArray[index1];
        if (Operators.CompareString(str, "", false) == 0)
        {
          p_KeyValuePairs.Remove("***** DefaultValueName *****");
          p_KeyValuePairs.Add("***** DefaultValueName *****", p_FromKey.GetValue(str).ToString());
        }
        else
          p_KeyValuePairs.Add(str, p_FromKey.GetValue(str).ToString());
        checked { ++index1; }
      }
      this.WriteValues(p_ToDir, ref p_KeyValuePairs);
      string[] subKeyNames = p_FromKey.GetSubKeyNames();
      int index2 = 0;
      while (index2 < subKeyNames.Length)
      {
        string name = subKeyNames[index2];
        RegistryKey p_FromKey1 = p_FromKey.OpenSubKey(name);
        this.CreateKey(p_ToDir + "\\" + name);
        this.MigrateKey(p_FromKey1, p_ToDir + "\\" + name);
        p_FromKey1.Close();
        checked { ++index2; }
      }
      stopwatch.Stop();
      this.TL.LogMessage("  ElapsedTime " + this.__STATIC__MigrateKey__2021128081E__RecurseDepth.ToString(), "  " + Conversions.ToString(stopwatch.ElapsedMilliseconds) + " milliseconds, Completed Directory: " + p_ToDir);
      this.__STATIC__MigrateKey__2021128081E__RecurseDepth = checked (this.__STATIC__MigrateKey__2021128081E__RecurseDepth - 1);
    }

    private void GetProfileMutex(string Method, string Parameters)
    {
      this.GotMutex = this.ProfileMutex.WaitOne(5000, false);
      if (!this.GotMutex)
      {
        this.TL.LogMessage("GetProfileMutex", "***** WARNING ***** Timed out waiting for Profile mutex in " + Method + ", parameters: " + Parameters);
        EventLogCode.LogEvent(Method, "Timed out waiting for Profile mutex in " + Method + ", parameters: " + Parameters, EventLogEntryType.Error, GlobalConstants.EventLogErrors.XMLProfileMutexTimeout, (string) null);
        throw new ProfilePersistenceException("Timed out waiting for Profile mutex in " + Method + ", parameters: " + Parameters);
      }
    }

        public void SetProfile(string DriverId, IASCOMProfile XmlProfile)
        {
            throw new System.NotImplementedException();
        }

        public IASCOMProfile GetProfile(string DriverId)
        {
            throw new System.NotImplementedException();
        }
    }
}
