using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using ASCOM.Utilities.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace RACI.Data
{
    public class RaciSystem: ProfileNode
    {
        public RaciSystem() : this("Unspecified") { }
        public RaciSystem(String name, String description = "") 
            : base(name, String.IsNullOrWhiteSpace(description) 
                  ? $"{name} system definition"
                  :description)
        {

        }
        override public ProfileNode Parent
        {
            get { return null; }
            set { /*NOOP*/ }
        }
        public String Hostname { get; set; }
    }

}
