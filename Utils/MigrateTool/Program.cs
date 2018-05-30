using System;
using System.Collections.Generic;
using System.Security;
using System.Security.Principal;
using System.Linq;

using Microsoft.Win32;
using System.Net;
using Microsoft.EntityFrameworkCore;
using RACI.Settings;
using RACI.Data;

namespace RACI.Data
{
    class Program
    {
        //static RaciRepository mainRepo;
        static SystemHelper sysHlp = null;
        static RaciSystem sysNode = null;
        static RaciSettings raciSettingsNode = null;
        static AscomPlatformNode ascomNode = null;

        static void Main(string[] args)
        {
            //mainRepo = new RaciRepository();
            WipeData();
            if (!args.Any(t => t == "-U" || t == "--no-update"))
                UpdateDataStore();
            else
                Console.WriteLine("--no-update specified, skipping datastore update...");
        }

        static void UpdateDataStore()
        {

            Console.WriteLine();
            Console.WriteLine($"Updating LocalSystem meta data");
            sysHlp = new SystemHelper();
            //sysNode = sysHlp.System;
            //raciSettingsNode = sysHlp.Raci;
            //ascomNode = sysHlp.Ascom;
            Console.WriteLine();

            Console.WriteLine($"Loading ASCOM users from local system registry");
            List<String> userIds = AscomUserIds();
            Console.WriteLine($"\t*Found {userIds?.Count ?? 0} ASCOM Users...");
            foreach (String sid in userIds)
            {
                string userName = UserIdToName(sid);
                if (String.IsNullOrWhiteSpace(userName))
                {
                    Console.WriteLine($"Cannot get username for SID[{sid}], skipping");
                    continue;
                }
                Console.WriteLine($"\tUpdating user: {userName} ({sid})");
                UpdateUser(sid);
            }

            Console.WriteLine($"Loading ASCOM Platform Settings");
            RegistryKey regKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\WOW6432Node\\ASCOM");
            ProfileNode ascomRoot = sysHlp.Ascom;
            RegistryToProfileNode(regKey, ascomRoot, false);
            IEnumerable<string> driverTypes = regKey.GetSubKeyNames().Where(t => t.EndsWith(" Drivers"));
            IEnumerable<string> ascomSettings = regKey.GetSubKeyNames().Except(driverTypes);

            foreach (string subkey in ascomSettings)
            {
                Console.WriteLine($"Loading ASCOM Settings Node: {subkey}");
                RegistryKey regSubkey = regKey.OpenSubKey(subkey);
                if (regSubkey == null)
                    throw new Exception($"Failed to open registry subkey[{subkey}] in {regKey.Name}");
                ProfileNode dbNode = sysHlp.SubNode(ascomRoot, subkey, true);
                if (dbNode == null)
                    throw new Exception($"Failed to make subnode[{subkey}] in ASCOM Root");

                RegistryToProfileNode(regSubkey, dbNode, true);
            }

            foreach (string subkey in driverTypes)
            {
                string drvName = subkey;
                Console.WriteLine($"Loading Driver Type: {drvName}");
                RegistryKey drvKey = regKey.OpenSubKey(drvName);
                if (drvKey == null)
                    throw new Exception($"Failed to open registry Driver[{drvName}] in {regKey.Name}");
                AscomDriverNode drvNode = sysHlp.SubNode<AscomDriverNode>(ascomRoot, drvName, true);
                if (drvNode == null)
                    throw new Exception($"Failed to make Driver Node [{drvName}] in ASCOM Root");
                RegistryToProfileNode(drvKey, drvNode, false);
                foreach (string devName in drvKey.GetSubKeyNames())
                {
                    Console.WriteLine($"Loading Device: [{drvName}].{devName}");
                    RegistryKey devKey = drvKey.OpenSubKey(devName);
                    if (devKey == null)
                        throw new Exception($"Failed to open registry Device[{devName}] in {drvKey.Name}");
                    AscomDeviceNode devNode = sysHlp.SubNode<AscomDeviceNode>(drvNode, devName, true);
                    if (devNode == null)
                        throw new Exception($"Failed to make Device Node [{devName}] in Driver[{drvNode.Name}]");
                    RegistryToProfileNode(devKey, devNode, true);
                }
            }
        Console.WriteLine($"Committing Platform settings to data store");

            Console.WriteLine();
            Console.WriteLine($"ASCOM LocalSystem load complete.");
        }

        static bool RegistryToProfileNode(RegistryKey regKey, ProfileNode node,bool recurse=true)
        {
            if (regKey == null)
                throw new ArgumentNullException("Argument 'regKey' cannot be null");
            if (node == null)
                throw new ArgumentNullException("Argument 'node' cannot be null");

            Console.WriteLine($"Converting Registry [{regKey.Name}] => {node.GetType().Name}");
            if (String.IsNullOrWhiteSpace(node.Name))
                node.Name = RegKeyToNodeName(regKey);
            foreach (string key in regKey.GetValueNames())
            {
                string value = regKey.GetValue(key)?.ToString() ?? "";
                ProfileValue pv = sysHlp.SetValueByName(node.ProfileNodeId, key, value);
            }
            if (recurse)
            {
                foreach (var nodeKey in regKey.GetSubKeyNames())
                {
                    Console.WriteLine($"Updating user settings '{nodeKey}'");
                    if (String.IsNullOrEmpty(nodeKey))
                        continue;
                    RegistryKey subKey = regKey.OpenSubKey(nodeKey);
                    String nodeName = RegKeyToNodeName(subKey);
                    ProfileNode subNode = sysHlp.SubNode(node, nodeName, true);

                    if (!RegistryToProfileNode(subKey, subNode, recurse))
                        throw new Exception($"Error loading RegistryKey[{regKey.Name}][{subKey.Name}]");
                }
            }
            return true;
        }
        static String RegKeyToNodeName(RegistryKey regKey)
        {
            if (regKey == null)
                return "";
            String result = regKey.Name.TrimEnd(new char[] { '\\' });
            int idx = result.LastIndexOf('\\');
            return result.Substring(idx + 1);
        }
        static UserSettings UpdateUser(String sid)
        {
            string userName = UserIdToName(sid);
            if (String.IsNullOrWhiteSpace(userName))
                throw new InvalidOperationException($"Cannot get username for SID[{sid}], skipping");
            RegistryKey userKey = Registry.Users.OpenSubKey($"{sid}\\Software\\ASCOM");
            if (userKey == null)
                throw new InvalidOperationException($"No ASCOM User settings found for {userName} ({sid}), skipping...");
            Console.WriteLine($"Loading ASCOM User settings for {userName} ({sid})...");

            UserSettings userNode = sysHlp.GetOrCreateUser(userName,sid);
            try
            {
                Console.WriteLine($"{userName} ({sid}): Clearing existing settings");
                sysHlp.ClearNode(userNode.ProfileNodeId);
                using (var uow = new RaciUnitOfWork())
                {
                    userNode.UserId = sid;
                    userNode.HomeDir = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                    uow.NodesOfType<UserSettings>().Update(userNode);
                    uow.Save();
                }

                Console.WriteLine($"{userName} ({sid}): Loading current registry settings");
                if (RegistryToProfileNode(userKey, userNode))
                    Console.WriteLine($"{userName} ({sid}): Committed settings to datastore");
                else
                    Console.WriteLine($"{userName} ({sid}): Failed retrieve ASCOM settings from the registry");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unable to load ASCOM User settings for {userName}, skipping");
                Console.WriteLine($"***Exception[{ex.GetType().Name}]: {ex.Message}");
            }
            return userNode;
        }

        static List<String> AscomUserIds()
        {
            List<String> result = new List<string>();
            try
            {
                RegistryKey regKey = Registry.Users;
                foreach (var user in regKey.GetSubKeyNames())
                {

                    RegistryKey userAscomKey = null;
                    try
                    {
                        userAscomKey = regKey.OpenSubKey($"{user}\\Software\\ASCOM");
                        if (userAscomKey != null)
                            result.Add(user);
                    }
                    catch { }
                }
            }
            catch { }
            return result;
        }

        static string UserIdToName(string sid)
        {
            string result = null;
            try
            {
                result = GuidToSecurityIdentifier(sid)?.Translate(typeof(NTAccount)).Value;
                int domSepIdx = result.IndexOf('\\');
                if (domSepIdx > 0)
                    result = result.Substring(domSepIdx + 1);
            }
            catch { result = null; }
            return result;
        }

        static SecurityIdentifier GuidToSecurityIdentifier(string sid)
        {
            SecurityIdentifier result = null;
            try { result = new SecurityIdentifier(sid); }
            catch { result = null; }
            return result;
        }

       
        static void WipeData(bool localOnly = true)
        {
            Console.WriteLine($"Clearing ASCOM objects from the database");
            Console.WriteLine($"LocalOnly: {localOnly}");
            using (var db = new RaciModel(null,DeleteBehavior.Cascade,true))
            {
                Console.WriteLine($"Wiping ProfileObjects");
                foreach (RaciSystem sys in db.Systems)
                {
                    if (localOnly && sys.Name != ".")
                    {
                        Console.WriteLine($"Skipping non-local system {sys.Name}...");
                        continue;
                    }
                    Console.Write($"\tRemoving LocalSystem ({sys.Hostname})....");
                    db.Systems.Remove(sys);
                    db.SaveChanges();
                }
            }
        }
    }
}