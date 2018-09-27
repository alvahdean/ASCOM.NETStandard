using RACI.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASCOM.Utilities.Interfaces
{
    public interface IAccessExtra
    {
        void CreateKey<TNode>(string p_SubKeyName) where TNode : ProfileNode, new();
        bool KeyExists(string nodePath);
        bool ValueExists(string valuePath);
        T GetProfile<T>(string p_SubKeyName, string p_ValueName) where T : struct;
        T GetProfile<T>(string p_SubKeyName, string p_ValueName, T p_DefaultValue) where T : struct;
        void WriteProfile<T>(string p_SubKeyName, string p_ValueName, T p_ValueData) where T : struct;
    }

    public interface IAscomDataStore : IAccess, IDisposable , IAccessExtra
    {
        void BackupProfile(string currentPlatformVersion);
    }

}
