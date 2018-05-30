using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Win32;
using System.Net;
using System.Runtime.InteropServices;
using RACI.Data;
using Microsoft.EntityFrameworkCore;


namespace ProfileEdit
{
    class Program
    {
        static SystemHelper sys;
        static void Main(string[] args)
        {
            bool isSet = false;
            bool doList = true;
            bool listValues = true;
            string sysName = null;
            string key = null;
            string val = null;

            for(int i=0;i<args.Length;i++)
            {
                string arg = args[i];
                if (arg == "-s" || arg == "--set")
                    isSet = true;
                else if (arg == "-V" || arg == "--no-values")
                    listValues = false;
                else if (arg == "-L" || arg == "--no-list")
                    listValues = false;
                else if ((arg == "-S" || arg == "--system") && i<args.Length-1)
                    sysName = args[++i];
                else if (!arg.StartsWith("-"))
                {
                    if (key == null)
                        key = arg;
                    else if (val == null)
                        val = arg;
                }
            }
            if (string.IsNullOrWhiteSpace(sysName))
                sysName = ".";
            sysName=sysName.ToLowerInvariant();
            sys = new SystemHelper(sysName);

            Console.WriteLine("RACI Profile Editor");
            Console.WriteLine($"System: {sysName}");
            if (doList)
            {
                using (RaciModel db = new RaciModel())
                {
                    Console.WriteLine("Platform:");
                    Console.WriteLine($"[System]");
                    Console.WriteLine($"\tName: {sys.Name}");
                    Console.WriteLine($"\tHostname: {sys.Hostname}");
                    Console.WriteLine();
                    Console.WriteLine($"[Users] ({sys.Users.Count()} entries)");
                    foreach (var user in sys.Users)
                        DisplayNode(user, 1, listValues);
                    Console.WriteLine();

                    var raci = sys.Raci;
                    Console.WriteLine($"[RACI Settings] (Id={raci?.ProfileNodeId ?? 0})");
                    if (raci != null)
                    {
                        Console.WriteLine($"\tUrlRoot: {sys.SetRaci("UrlRoot", RaciModel.Configuration.RACI.UrlRoot)}");
                        Console.WriteLine($"\tProductVersion: {sys.SetRaci("tProductVersion", RaciModel.Configuration.RACI.ProductVersion)}");
                        Console.WriteLine($"\tRestApiVersion: {sys.SetRaci("tRestApiVersion", RaciModel.Configuration.RACI.RestApiVersion)}");
                        Console.WriteLine($"\tEnableInternalLog: {sys.SetRaci("tEnableInternalLog", RaciModel.Configuration.RACI.EnableInternalLog)}");
                        Console.WriteLine($"\tLogfile: {sys.SetRaci("Logfile", RaciModel.Configuration.RACI.Logfile)}");
                    }
                    Console.WriteLine();
                    AscomPlatformNode ascom = sys.Ascom;
                    Console.WriteLine($"[ASCOM Settings:{ascom.ProfileNodeId}]");
                    foreach (var snode in sys.Nodes(ascom.ProfileNodeId).Where(t => !t.Name.EndsWith("Drivers")))
                        DisplayNode(snode, 1, listValues);
                    Console.WriteLine();

                    Console.WriteLine($"[ASCOM Drivers] (PlatformId={ascom.ProfileNodeId})");
                    foreach (var dnode in sys.Nodes(ascom.ProfileNodeId).Where(t => t.Name.EndsWith("Drivers")))
                        DisplayNode(dnode, 1, listValues);
                    Console.WriteLine();
                }
            }
            if(isSet)
            {
                Console.WriteLine("Set operation: Not implemented");
            }
        }

        static public void DisplayNode(ProfileNode node,int indent=0,bool displayValues=true,bool recurse=true)
        {
            String itxt = "";
            for (int i = 0; i < indent; i++)
                itxt += "\t";
            
            string path = sys.NodePath(node.ProfileNodeId);

            Console.WriteLine($"{itxt}[{path}]: {node.Description}");
            if (displayValues)
                foreach (var attr in sys.SubValues(node.ProfileNodeId))
                    Console.WriteLine($"{itxt}\t{attr.Key}: {attr.Value}");
            if (recurse)
            {
                
                foreach (ProfileNode subnode in sys.Nodes(node.ProfileNodeId))
                    DisplayNode(subnode, indent + 1, displayValues);
            }
        }
    }
}
