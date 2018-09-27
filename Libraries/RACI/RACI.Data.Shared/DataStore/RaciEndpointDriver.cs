using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace RACI.Data
{

    public class RaciEndpointDriver : ProfileNode<RaciEndpoint>
    {
        public RaciEndpointDriver() : this("", "") { }
        public RaciEndpointDriver(String name, String description = "") : base(name, description) { }
        public String DriverType { get; set; }
    }
}
