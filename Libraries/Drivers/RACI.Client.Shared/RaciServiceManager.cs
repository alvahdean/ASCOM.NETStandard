using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RACI.Data;
using RACI.Data.RestObjects;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RACI.Client
{
    //TODO: Implement internal store as RaciModel
    public static class RaciServiceManager
    {
        #region Internal members
        private static object serviceSync = new object();
        private static IEqualityComparer<string> keyComparer;
        private static List<RaciEndpoint> endpoints;
        
        private static ILogger logger;
        private static string makeServiceLabel(string label)
        {
            if (String.IsNullOrWhiteSpace(label))
                label = "Service";
            string orig = label;
            int idx = 2;
            checked
            {
                lock (serviceSync)
                {
                    while (endpoints.AnyWithName(label))
                        label = $"{orig}-{idx++}";
                }
            }
            return label;
        }

        //public static bool Register(string url, string name = null, bool reload = false)
        //{
        //    bool result = false;
        //    url = url?.Trim() ?? "";
        //    name = name?.Trim() ?? "";

        //    if (url != "")
        //    {
        //        RaciClient client = new RaciClient(url) { FollowRedirects = true };

        //        RaciEndpoint ep = GetEndpointByUrl(url);
        //        if (ep != null && name != "" && name != ep.Name)
        //            ep.Name = name;
        //        if (reload || !ep.Nodes.Any())
        //        {
        //            //ep.Nodes.Clear(); //Use RemoveDriver in order to persist changes
        //            foreach (string dName in ep.Nodes.Select(t => t.Name))
        //                RemoveDriver(ep.Name, dName);
        //            //client = new RestClient(ep.ServiceRoot);
        //            foreach (var dtItem in GetDriverTypes(client))
        //            {
        //                if (String.IsNullOrWhiteSpace(dtItem))
        //                {
        //                    logger.LogWarning($"Skipping invalid driver type '{dtItem}'");
        //                    continue;
        //                }
        //                string dt = dtItem.Trim();
        //                //List<KeyValuePair<string,string>> dList = client.GetDrivers(dt).ToList();
        //                //foreach(var d in dList)
        //                foreach (var d in GetDrivers(client,dt))
        //                    UpdateOrCreateDriver(ep?.Name, d.Key, dt, d.Value);
        //                result = ep.Nodes.Count>0;
        //            }
        //        }
        //    }
        //    return result;
        //}

        #endregion

        #region Instance management
        static RaciServiceManager()
        {
#if DEBUG
            logger = new LoggerFactory().AddConsole(LogLevel.Debug).AddDebug().CreateLogger(nameof(RaciServiceManager));
#else
            logger = new LoggerFactory().AddConsole(LogLevel.Information).CreateLogger(nameof(RaciServiceManager));
#endif
            keyComparer = new CIKeyComparer();
            endpoints = new List<RaciEndpoint>();
        }
        #endregion  

        #region Public members

        public static bool IsValidUri(Uri uri) => uri?.IsAbsoluteUri ?? false;
        public static bool IsValidUrl(string url) => !String.IsNullOrWhiteSpace(url)
            ? IsValidUri(new Uri(url))
            : false;
        public static bool Ping(Uri uri)
        {
            if (!IsValidUri(uri))
            {
                logger.LogWarning($"Unable to ping service: Invalid URI '{uri?.OriginalString}'");
                return false;
            }
            IRascomResult result = null;

            if (!result?.Success ?? false)
            {
                logger.LogWarning($"Ping service '{uri?.AbsoluteUri}' failed");
                return false;
            }
            logger.LogDebug($"Ping service '{uri?.AbsoluteUri}' OK");
            return true;
        }
        public static bool Ping(string url) => Ping(new Uri(url));
        public static bool PingEndpoint(string epName) => Ping(GetEndpointByName(epName)?.ServiceRoot);

        public static RaciEndpoint AddEndpoint(string url, string label = null)
        {
            url = url?.Trim() ?? String.Empty;
            label = label?.Trim() ?? "";
            if (Uri.TryCreate(url, UriKind.Absolute, out Uri uri))
            {
                logger?.LogWarning($"Url is not valid. Must be an absolute Uri. '{url}'");
                return null;
            }
            RaciEndpoint ep = endpoints.WithUrl(url);
            if (ep != null)
            {
                if (label != "" && ep.Name != label)
                {
                    label = makeServiceLabel(label);
                    ep.Name = label;
                }
            }
            else
            {
                lock (serviceSync)
                {
                    if (label == "")
                        label = uri.DnsSafeHost;
                    label = makeServiceLabel(label);
                    endpoints.Add(new RaciEndpoint(label) { ServiceRoot = url });
                    ep = endpoints.WithUrl(url);
                }
            }
            if (ep == null)
                logger?.LogWarning($"Unable to add or update endpoint: Label: '{label}', Url: '{url}'");
           else
                logger?.LogWarning($"Added or updated endpoint: Label: '{label}', Url: '{url}'");
            return ep;
        }
        public static bool RemoveEndpointName(string name) => endpoints.Remove(GetEndpointByName(name));
        public static bool RemoveEndpointUrl(string url) => endpoints.Remove(GetEndpointByUrl(url));
        public static RaciEndpointDriver AddDriver(string epName, RaciEndpointDriver drv)
        {
            if (drv?.Name == null || String.IsNullOrWhiteSpace(epName) || ! HasEndpointName(epName))
                return null;

            RaciEndpoint ep = GetEndpointByName(epName);

            var d = ep.Nodes.WithName(drv?.Name);
            if (d != null)
                ep.Nodes.Remove(d);
            drv.Parent = ep;
            ep.Nodes.Add(drv);
            return drv;
        }
        public static RaciEndpointDriver AddDriver(string epName,string driverType,string driverName,string descr="")
        {
            if (String.IsNullOrWhiteSpace(driverName))
            {
                logger?.LogWarning($"Unable to add endpoint: Invalid driverName '{driverName}'");
                return null;
            }
            RaciEndpoint ep = GetEndpointByName(epName);
            RaciEndpointDriver drv = null;
            if (ep != null)
            {
                drv = GetDrivers(ep).WithName(driverName);
                if(drv==null)
                {
                    drv = new RaciEndpointDriver(driverName,descr)
                    {
                        DriverType = driverType,
                        ParentProfileNodeId = ep.ProfileNodeId,
                        Parent = ep
                    };
                    ep.Nodes.Add(drv);
                }
            }
            else 
                logger?.LogWarning($"Unable to add endpoint: No service named '{epName}' found");
            return drv;
        }
        public static bool RemoveDrivers(string epName)
        {
            RaciEndpoint ep = GetEndpointByName(epName);
            if (ep == null)
                return false;
            ep.Nodes.Clear();
            return ep.Nodes.Count() == 0;
        }
        public static bool RemoveDriver(string epName,string driverName)
        {
            RaciEndpoint ep = GetEndpointByName(epName);
            RaciEndpointDriver drv = ep.Nodes.WithName(driverName);
            if (drv != null)
                ep.Nodes.Remove(drv);
            return ep.Nodes.WithName(driverName)==null;
        }
        public static RaciEndpointDriver UpdateOrCreateDriver(RaciEndpoint ep, string driverName, string driverType, string descr)
        {
            return UpdateDriver(ep, driverName, driverType, descr, true);
        }
        public static RaciEndpointDriver UpdateOrCreateDriver(String epName, string driverName, string driverType, string descr)
        {
            return UpdateDriver(epName, driverName, driverType, descr, true);
        }
        public static RaciEndpointDriver UpdateDriver(RaciEndpoint ep, string driverName, string driverType, string descr,bool create=false)
        {
            return UpdateDriver(ep?.Name, driverName, driverType, descr,create);
        }
        public static RaciEndpointDriver UpdateDriver(string epName, string driverName,string driverType,string descr, bool create = false)
        {
            RaciEndpoint ep = GetEndpointByName(epName);
            if(ep==null)
            {
                string msg=$"Cannot add driver '{driverType}:{driverName}'. Endpoint '{epName}' does not exist";
                logger.LogError(msg);
                throw new KeyNotFoundException(msg);
            }
            RaciEndpointDriver drv = ep.Nodes.WithName(driverName);
            if (drv == null)
                drv=create ? AddDriver(ep?.Name, driverType, driverName, descr):null;
            else
            {
                if(!String.IsNullOrWhiteSpace(driverType))
                drv.DriverType = driverType;
                if (!String.IsNullOrWhiteSpace(descr))
                    drv.Description = descr;
            }
            return GetDriverByName(epName,driverName);
        }
        public static IQueryable<RaciEndpoint> GetEndpoints() =>
            endpoints.AsQueryable();
        public static IQueryable<string> GetEndpointNames() =>
            GetEndpoints()
            .Select(t => t.Name)
            .Distinct(keyComparer)
            .AsQueryable();
        public static IQueryable<string> GetEndpointUrls() =>
            GetEndpoints()
            .Select(t => t.ServiceRoot)
            .Distinct(keyComparer)
            .AsQueryable();
        public static RaciEndpoint GetEndpoint(string epName, string url, bool autoCreate = true)
        {
            epName = epName?.Trim() ?? "";
            url = url?.Trim() ?? "";
            if (epName == "" && url == "")
                return null;
            return url == ""
                ? GetEndpointByName(epName)
                : GetEndpointByUrl(url) ?? (autoCreate ? AddEndpoint(url, epName) : null);
        }
        public static RaciEndpoint GetEndpointByName(string epName) =>
            GetEndpoints()
            .WithName(epName);
        public static RaciEndpoint GetEndpointByUrl(string url) =>
            GetEndpoints()
            .WithUrl(url);
        public static string GetUrl(string epName) =>
            GetEndpointByName(epName)?.ServiceRoot;
        public static Uri GetUri(string epName)
        {
            string url = GetUrl(epName);
            Uri uri= new Uri(url);
            if(!IsValidUri(uri))
            {
                logger.LogWarning($"Uri is not valid for RACI operations: '{url}'");
                return null;
            }
            return uri;
        }
        public static IQueryable<RaciEndpointDriver> GetDrivers() => GetEndpoints().SelectMany(t=>t.Nodes);
        public static IQueryable<string> GetDriverTypes() =>
            GetDrivers()
            .Select(t => t.DriverType)
            .Distinct(keyComparer);
        public static IQueryable<string> GetDriverTypes(string epName) =>
            GetDrivers(epName)
            .Select(t => t.DriverType)
            .Distinct(keyComparer);
        public static IQueryable<string> GetDriverTypes(RaciEndpoint ep) =>
            GetDriverTypes(ep?.Name);
        public static IQueryable<RaciEndpointDriver> GetDrivers(string epName) =>
            GetDrivers().Where(t => t.Parent != null && keyComparer.Equals(t.Parent.Name, epName));
        public static IQueryable<RaciEndpointDriver> GetDrivers(RaciEndpoint ep) =>
            GetDrivers(ep?.Name);

        public static IQueryable<RaciEndpointDriver> GetDrivers(string epName,string driverType) =>
            GetDrivers(epName).Where(t => keyComparer.Equals(t.DriverType, driverType));
        public static IQueryable<RaciEndpointDriver> GetDrivers(RaciEndpoint ep, string driverType) =>
            GetDrivers(ep?.Name, driverType);
        public static IQueryable<string> GetDriverNames(string epName, string driverType) =>
            GetDrivers(epName)
            .Select(t => t.Name);
        public static IQueryable<string> GetDriverNames(RaciEndpoint ep, string driverType) =>
            GetDriverNames(ep?.Name, driverType);
        public static RaciEndpointDriver GetDriverByName(string epName, string driverName) =>
            GetDrivers(epName)
            .WithName(driverName);
        public static RaciEndpointDriver GetDriverByName(RaciEndpoint ep, string driverName) =>
            GetDriverByName(ep?.Name, driverName);

        public static bool HasEndpointName(string name) =>
            GetEndpointNames()
            .Any(t => keyComparer.Equals(t, name));
        public static bool HasEndpointUrl(string name) =>
            GetEndpointUrls()
            .Any(t => keyComparer.Equals(t, name));
        public static bool HasDriverType(string driverType) =>
            GetDriverTypes()
            .Any(t => keyComparer.Equals(t, driverType));
        public static bool HasDriverType(string epName, string driverType) =>
            GetDriverTypes(epName)
            .Any(t => keyComparer.Equals(t, driverType));
        public static bool HasDriverType(RaciEndpoint ep, string driverType) =>
            HasDriverType(ep?.Name, driverType);
        public static bool HasDriver(string epName, string driverName) =>
            GetDrivers(epName)
             .AnyWithName(driverName);
        public static bool HasDriver(RaciEndpoint ep, string driverName) =>
            HasDriver(ep?.Name, driverName);

        #endregion
    }
}
