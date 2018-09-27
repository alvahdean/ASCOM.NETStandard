// Decompiled with JetBrains decompiler
// Type: ASCOM.Utilities.AllUsersFileSystemProvider
// Assembly: ASCOM.Utilities, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 06F2ED6E-C559-496D-B7FF-DCA800F2230D
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.Utilities\6.0.0.0__565de7938946fba7\ASCOM.Utilities.dll

using ASCOM.Utilities.Interfaces;
//using Microsoft.VisualBasic;
//using Microsoft.VisualBasic.CompilerServices;
using System;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using ASCOM.Utilities.Interfaces;
using Microsoft.VisualBasic;

namespace ASCOM.Utilities
{
  internal class AllUsersFileSystemProvider : IFileStoreProvider
  {
    private const string ASCOM_DIRECTORY = "\\ASCOM";
    private const string PROFILE_DIRECTORY = "\\ASCOM\\Profile";
    private string BaseFolder;
    private string ASCOMFolder;

    string IFileStoreProvider.BasePath
    {
      get
      {
        return this.BaseFolder;
      }
    }

    internal AllUsersFileSystemProvider()
    {
      this.ASCOMFolder = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\ASCOM";
      this.BaseFolder = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\ASCOM\\Profile";
    }

    void IFileStoreProvider.SetSecurityACLs(ITraceLogger p_tl)
    {
      DirectoryInfo directoryInfo = new DirectoryInfo(this.ASCOMFolder);
      SecurityIdentifier securityIdentifier = new SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, new SecurityIdentifier("S-1-0-0"));
      p_tl.LogMessage("  SetSecurityACLs", "Retrieving access control");
      DirectorySecurity accessControl = directoryInfo.GetAccessControl();
      p_tl.LogMessage("  SetSecurityACLs", "Adding full control access rule");
      accessControl.AddAccessRule(new FileSystemAccessRule((IdentityReference) securityIdentifier, FileSystemRights.FullControl, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
      p_tl.LogMessage("  SetSecurityACLs", "Setting access control");
      directoryInfo.SetAccessControl(accessControl);
      p_tl.LogMessage("  SetSecurityACLs", "Successfully set security ACL!");
    }

    bool IFileStoreProvider.get_Exists(string p_FileName)
    {
      bool flag;
      try
      {
        flag = File.Exists(this.CreatePath(p_FileName));
      }
      catch (Exception ex)
      {
        //ProjectData.SetProjectError(ex);
        Interaction.MsgBox((object) ("Exists " + ex.ToString()), MsgBoxStyle.OkOnly, (object) null);
        flag = false;
        //ProjectData.ClearProjectError();
      }
      return flag;
    }

    void IFileStoreProvider.CreateDirectory(string p_SubKeyName, ITraceLogger p_TL)
    {
      try
      {
        p_TL.LogMessage("  CreateDirectory", "Creating directory for: \"" + p_SubKeyName + "\"");
        Directory.CreateDirectory(this.CreatePath(p_SubKeyName));
        p_TL.LogMessage("  CreateDirectory", "Created directory OK");
      }
      catch (Exception ex)
      {
        //ProjectData.SetProjectError(ex);
        Exception exception = ex;
        p_TL.LogMessage("FileSystem.CreateDirectory", "Exception: " + exception.ToString());
        Interaction.MsgBox((object) ("CreateDirectory Exception: " + exception.ToString()), MsgBoxStyle.OkOnly, (object) null);
        //ProjectData.ClearProjectError();
      }
    }

    void IFileStoreProvider.DeleteDirectory(string p_SubKeyName)
    {
      Directory.Delete(this.CreatePath(p_SubKeyName), true);
    }

    void IFileStoreProvider.EraseFileStore()
    {
      //if (Interaction.MsgBox((object) "Are you sure you wish to erase the Utilities profile store?", MsgBoxStyle.OkCancel | MsgBoxStyle.Critical, (object) "ASCOM.Utilities")!=(int)MsgBoxResult.Ok)
      //  return;
      try
      {
        Directory.Delete(this.BaseFolder, true);
      }
      catch (Exception ex)
      {
        //ProjectData.SetProjectError(ex);
        //ProjectData.ClearProjectError();
      }
    }

    string[] IFileStoreProvider.get_GetDirectoryNames(string p_SubKeyName)
    {
      string[] directories = Directory.GetDirectories(this.CreatePath(p_SubKeyName));
      int index1 = 0;
      string[] strArray1 = directories;
      int index2 = 0;
      while (index2 < strArray1.Length)
      {
                string[] strArray2 = strArray1[index2].Split('\\');
//                string[] strArray2 = Strings.Split(strArray1[index2], "\\", -1, CompareMethod.Binary);
                directories[index1] = strArray2[checked (strArray2.Length - 1)];
        checked { ++index1; }
        checked { ++index2; }
      }
      return directories;
    }

    string IFileStoreProvider.get_FullPath(string p_FileName)
    {
      return this.CreatePath(p_FileName);
    }

    void IFileStoreProvider.Rename(string p_CurrentName, string p_NewName)
    {
      File.Delete(this.CreatePath(p_NewName));
      File.Move(this.CreatePath(p_CurrentName), this.CreatePath(p_NewName));
    }

    void IFileStoreProvider.RenameDirectory(string CurrentName, string NewName)
    {
      try
      {
        Directory.Delete(this.CreatePath(NewName), true);
      }
      catch (Exception ex)
      {
        //ProjectData.SetProjectError(ex);
        //ProjectData.ClearProjectError();
      }
      new DirectoryInfo(this.CreatePath(CurrentName)).MoveTo(this.CreatePath(NewName));
    }

    private string CreatePath(string p_FileName)
    {
            //if (Operators.CompareString(Strings.Left(p_FileName, 1), "\\", false) != 0)
            if (!p_FileName.StartsWith("\\"))
                    p_FileName = "\\" + p_FileName;
      return this.BaseFolder + p_FileName;
    }
  }
}
