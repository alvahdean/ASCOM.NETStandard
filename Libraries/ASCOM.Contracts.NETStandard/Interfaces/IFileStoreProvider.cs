// Decompiled with JetBrains decompiler
// Type: ASCOM.Utilities.Interfaces.IFileStoreProvider
// Assembly: ASCOM.Utilities, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 06F2ED6E-C559-496D-B7FF-DCA800F2230D
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.Utilities\6.0.0.0__565de7938946fba7\ASCOM.Utilities.dll

namespace ASCOM.Utilities.Interfaces
{
#warning Internal interface exposed as public during porting: IFileStoreProvider
    public interface IFileStoreProvider
        //internal interface IFileStoreProvider
    {
    string BasePath { get; }

    void CreateDirectory(string p_SubKeyName, ITraceLogger p_TL);

    void DeleteDirectory(string p_SubKeyName);

    void EraseFileStore();

    string[] get_GetDirectoryNames(string p_SubKeyName);

    bool get_Exists(string p_FileName);

    string get_FullPath(string p_FileName);

    void Rename(string p_CurrentName, string p_NewName);

    void RenameDirectory(string CurrentName, string NewName);

    void SetSecurityACLs(ITraceLogger p_TL);
  }
}
