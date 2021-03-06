﻿using ASCOM.Utilities.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using RACI.Data;
using ASCOM.Utilities.Exceptions;
using Microsoft.Win32;

namespace ASCOM.Utilities
{

    public class EntityStore: IAscomDataStore, IAccessExtra
    {
        #region Internal fields
        private static bool initComplete;
        private static CIKeyComparer nameComparer;
        private static TraceLogger _tl;
        private static AscomPlatformNode rootNode;
        private static SystemHelper sysHelper;


        private TraceLogger _localTL=null;
        private Stopwatch sw;
        private Stopwatch swSupport;
        private bool ignoreChecks = false;
        private String _sysName;
        private String rootPath;
        #endregion

        protected TraceLogger TL
        {
            get=> _localTL ?? _tl;
            set
            {
                if(value!=null && _tl!=value)
                {
                    if (_localTL != null)
                        _localTL.Dispose();
                    _localTL = value;
                }
            }
        }

        #region Instance management
        //TODO: Remove reg from class, this is for porting help only.
        //private static WindowsRegistryAccess reg;
        static EntityStore()
        {
            nameComparer = new CIKeyComparer();

            sysHelper = new SystemHelper();
            _tl = new TraceLogger("EntityStore", "Profile")
            {
                Enabled = RegistryCommonCode.GetBool("Trace Profile", false)
            };
            rootNode = sysHelper.Ascom
                ?? throw new ASCOM.InvalidValueException($"ASCOMPlatform root node is not initialized!");
            _tl.LogMessage("InitStore",$"RootNodePath: {PathUtil.NodePath(rootNode.ProfileNodeId)}");
        }
        public EntityStore() : this(true) { }
        public EntityStore(string p_CallingComponent) : this(false)
        {
            if (p_CallingComponent.ToUpper() == "UNINSTALLASCOM")
            {
                _localTL = new TraceLogger("", "ProfileMigration")
                {
                    Enabled = RegistryCommonCode.GetBool("Trace Profile", false)
                };
                VersionCode.RunningVersions(TL);
                sw = new Stopwatch();
                swSupport = new Stopwatch();
            }
        }
        public EntityStore(bool p_IgnoreChecks)
        {
            initStore();
            ignoreChecks = p_IgnoreChecks;
            NewCode(p_IgnoreChecks);
        }
        #endregion

        #region IAccessExtra
        protected void initStore()
        {
            if (!initComplete)
            {
                ignoreChecks = true;
                Version caver = GetType().Assembly.GetName().Version;
                string defVer = $"{caver.Major}.{caver.Minor}";
                string vString = GetProfile("", "PlatformVersion", defVer);
                initComplete = true;
            }
        }
        public string SystemName
        {
            get => _sysName;
            set
            {
                string sysName = CleanSysName(value);
                if(_sysName==null || !nameComparer.Equals(sysName,_sysName))
                {
                    _sysName = sysName;
                    sysHelper = new SystemHelper(_sysName);
                    rootNode = sysHelper.Ascom;
                    rootPath = PathUtil.NodePath(rootNode.ProfileNodeId);
                }
            }
        }
        public static string CleanSysName(string sysName)
        {
            if (String.IsNullOrWhiteSpace(sysName))
                sysName = ".";
            return sysName.Trim();
        }
        protected string MakeSubKey(String p_SubKeyName,String p_ValueName) =>
            PathUtil.Parse($"{p_SubKeyName}{PathUtil.Separator}{p_ValueName}")??"";
        public void CreateKey<TNode>(string p_SubKeyName) where TNode: ProfileNode,new() =>
            sysHelper.SubNode<TNode>(rootNode, p_SubKeyName, true);
        public bool KeyExists(string nodePath) =>
            sysHelper.HasNode(rootNode.ProfileNodeId, nodePath);
        public bool ValueExists(string valuePath) =>
            sysHelper.HasValue(rootNode.ProfileNodeId, valuePath);
        public T GetProfile<T>(string p_SubKeyName, string p_ValueName) where T : struct
            => sysHelper.ValueByPath<T>(rootNode, MakeSubKey(p_SubKeyName, p_ValueName));
        public T GetProfile<T>(string p_SubKeyName, string p_ValueName, T p_DefaultValue) where T : struct
            => sysHelper.ValueByPath(rootNode, MakeSubKey(p_SubKeyName, p_ValueName), p_DefaultValue);
        public void WriteProfile<T>(string p_SubKeyName, string p_ValueName, T p_ValueData) where T : struct
            => sysHelper.SetValueByPath(rootNode, MakeSubKey(p_SubKeyName, p_ValueName), p_ValueData);
        #endregion

        #region IAccess implementation

        public string GetProfile(string p_SubKeyName, string p_ValueName)
            => sysHelper.ValueByPath(rootNode, MakeSubKey(p_SubKeyName, p_ValueName));
        public string GetProfile(string p_SubKeyName, string p_ValueName, string p_DefaultValue)
            => sysHelper.ValueByPath(rootNode, MakeSubKey(p_SubKeyName, p_ValueName), p_DefaultValue);

        public void WriteProfile(string p_SubKeyName, string p_ValueName, string p_ValueData)
            => sysHelper.SetValueByPath(rootNode, MakeSubKey(p_SubKeyName, p_ValueName), p_ValueData);

        public SortedList<string, string> EnumProfile(string p_SubKeyName)
        {
            SortedList<string, string> result = new SortedList<string, string>();
            IProfileNode node = sysHelper.SubNode(rootNode, p_SubKeyName);
            if (node == null)
                return result;
            foreach (var kvp in sysHelper.SubValues(node.ProfileNodeId))
                result.Add(kvp.Key, kvp.Value.Value);
            return result;
        }

        public void DeleteProfile(string p_SubKeyName, string p_ValueName)
        {
            IProfileNode node = sysHelper.SubNode(rootNode,p_SubKeyName);
            if (node == null)
                return;
            sysHelper.DeleteNode(node.ProfileNodeId);
        }

        public void CreateKey(string p_SubKeyName)
        {
            CreateKey<ProfileNode>(p_SubKeyName);
        }

        public SortedList<string, string> EnumKeys(string p_SubKeyName)
        {
            
            SortedList<string, string> result = new SortedList<string, string>();
            IProfileNode node = sysHelper.SubNode(rootNode,p_SubKeyName);
            if (node == null)
                return result;
            foreach (var val in sysHelper.Nodes(node.ProfileNodeId))
                result.Add(val.Name, val.Description);
            return result;
        }

        public void DeleteKey(string p_SubKeyName)
        {
            IProfileNode node = sysHelper.SubNode(rootNode,p_SubKeyName);
            if (node != null)
                sysHelper.DeleteNode(node.ProfileNodeId);
        }

        public void RenameKey(string CurrentSubKeyName, string NewSubKeyName)
        {
            CIKeyComparer keyComp = new CIKeyComparer();
            IProfileNode node = sysHelper.SubNode(rootNode, CurrentSubKeyName);
            if (node == null)
                return;
            sysHelper.SetNodeName(node.ProfileNodeId, NewSubKeyName);
        }

        public void MigrateProfile(string CurrentPlatformVersion)
        {
            throw new System.NotImplementedException();
        }

        public void SetProfile(string driverId, IASCOMProfile ascomProfile)
        {
            string drvType = GetDriverType(driverId);
            string devName = GetDeviceName(driverId);
            ASCOMProfile result = new ASCOMProfile();
            AscomDeviceNode devNode = sysHelper.AscomDevice(drvType, devName)
                ?? throw new Exception($"Unable to find ProfileStore data for driverId: '{driverId}', (Type={drvType}, Device={devName}");

        }

        private String GetDriverTypeOld(string driverId)
        {
            string[] items = driverId.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            if (items?.Length != 3)
                throw new Exception($"Unable to parse driverId: '{driverId}', is not fully qualified");
            return items[2];
        }
        private String GetDeviceNameOld(string driverId)
        {
            string[] items = driverId.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            if (items?.Length != 3)
                throw new Exception($"Unable to parse driverId: '{driverId}', is not fully qualified");
            return items[1];
        }

        private String GetDriverType(string driverId)
        {
            string[] items = driverId.Split(new char[] { PathUtil.SeparatorChar }, StringSplitOptions.RemoveEmptyEntries);
            if (items?.Length != 2)
                throw new Exception($"Unable to parse driverId: '{driverId}', is not fully qualified");
            return items[0];
        }
        private String GetDeviceName(string driverId)
        {
            string[] items = driverId.Split(new char[] { PathUtil.SeparatorChar }, StringSplitOptions.RemoveEmptyEntries);
            if (items?.Length != 2)
                throw new Exception($"Unable to parse driverId: '{driverId}', is not fully qualified");
            return items[1];
        }

        public IASCOMProfile GetProfile(string DriverId)
        {
            string drvType = GetDriverType(DriverId);
            string devName = GetDeviceName(DriverId);
            ASCOMProfile result = new ASCOMProfile();
            AscomDeviceNode devNode=sysHelper.AscomDevice(drvType,devName);

            if (devNode == null)
                throw new Exception($"Could not load device profile[{DriverId}] ({drvType}:{devName}) [devNode={devNode?.ProfileNodeId}]");
            var svals = sysHelper.SubValues(devNode.ProfileNodeId);
            foreach(var item in svals)
            {
                PathUtil pu = item.Key;
                if(pu.Parent=="")
                    result.SetValue(pu.Value, item.Value.Value);
                else
                    result.SetValue(pu.Value, item.Value.Value, pu.Parent);
            }
            return result;
        }
        #endregion

        #region Helper Methods
        public void NewCode(bool p_IgnoreChecks)
        {
            TL.LogMessage("NewCode", $"Creating RegistryAccess object (IgnoreChecks={p_IgnoreChecks})");
            TL.Enabled = RegistryCommonCode.GetBool("Trace XMLAccess", false);
            TL.EchoConsole = true;
            TL.EchoDebug = true;
            VersionCode.RunningVersions(TL);
            sw = new Stopwatch();
            swSupport = new Stopwatch();
            //ProfileMutex = new Mutex(false, "ASCOMProfileMutex");
            try
            {
                GetProfile("\\", "PlatformVersion");
            }
            catch (Exception ex)
            {
                ////ProjectData.SetProjectError(ex);
                Exception inner = ex;
                if (p_IgnoreChecks)
                {
                    TL.LogMessageCrLf($"{GetType().Name}.New IgnoreCheks is true so ignoring exception:", inner.ToString());
                    ////ProjectData.ClearProjectError();
                }
                else
                {
                    TL.LogMessageCrLf($"{GetType().Name}.New Unexpected exception:", inner.ToString());
                    throw new ProfilePersistenceException($"{GetType().Name}.New - Unexpected exception", inner);
                }
            }
        }
        private string CleanSubKey(string subKey) => SystemHelper.CleanPath(subKey);
        public void BackupProfile(string currentPlatformVersion)
        {
            throw new System.NotImplementedException("BackupProfile not implemented in EntityStore");
        }

        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~AscomDataStore() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }

        #endregion
    }

}
