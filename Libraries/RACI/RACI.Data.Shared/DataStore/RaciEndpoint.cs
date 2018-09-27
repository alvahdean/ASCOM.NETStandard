using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Net;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace RACI.Data
{
    //public interface IRaciEndpoint : IProfileNode//, IProfileNode<RaciSystem, RaciEndpointDriver>
    //{
    //    String ServiceRoot { get; set; }
    //    DateTime Created { get; set; }
    //    DateTime LastUpdate { get; set; }
    //    DateTime LastAccess { get; set; }
    //}

    public class RaciEndpoint : ProfileNode<RaciSystem, RaciEndpointDriver>//, IRaciEndpoint
    {
        public RaciEndpoint() : this("", "") { }
        public RaciEndpoint(String name) : this(name, $"{name} Service", null) { }
        public RaciEndpoint(String name, String description = "") : this(name, description, null) { }
        public RaciEndpoint(String name, String description = "", String serviceRoot = "") : base(name, description)
        {
            Uri uri = null;
            Name = "";
            Description = "";
            ServiceRoot = "";

            if (!String.IsNullOrWhiteSpace(serviceRoot))
            {
                uri = new Uri(serviceRoot);
                ServiceRoot = uri.IsAbsoluteUri ? uri.DnsSafeHost : "";
            }
            else
                ServiceRoot = "";

            if (String.IsNullOrWhiteSpace(name))
                Name = uri?.DnsSafeHost??"";

            if (String.IsNullOrWhiteSpace(description) && String.IsNullOrWhiteSpace(name))
                Description = $"{name} Endpoint";

            Created = DateTime.Now;
            LastAccess = Created;
            LastUpdate = Created;
            if (Nodes == null)
                Nodes = new HashSet<RaciEndpointDriver>();
            if (Values == null)
                Values = new HashSet<ProfileValue>();
        }
        public String ServiceRoot { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastUpdate { get; set; }
        public DateTime LastAccess { get; set; }

    }

    public static class RaciEndpointExt
    {

    }

    public static class RaciEndpointDriverExt
    {
        static private ProfileKeyComparer keyComp = new ProfileKeyComparer();

        public static bool TypeEquals(this RaciEndpointDriver driver, string driverType) =>
            keyComp.Equals(driver?.DriverType, driverType);
        public static IEnumerable<RaciEndpointDriver> OfDriverType(this IEnumerable<RaciEndpointDriver> epList, string driverType) =>
            epList?.Where(t => keyComp.Equals(t.DriverType, driverType));
    }

}
