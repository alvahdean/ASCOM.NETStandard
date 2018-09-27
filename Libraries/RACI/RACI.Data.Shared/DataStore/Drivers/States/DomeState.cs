using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RACI.Data
{
    public class DomeState : DriverState
    {
        [Required]
        [Display(Name = "Altitude")]
        public double Altitude { get; set; }

        [Required]
        [Display(Name = "AtHome")]
        public bool AtHome { get; set; }

        [Required]
        [Display(Name = "AtPark")]
        public bool AtPark { get; set; }

        [Required]
        [Display(Name = "Azimuth")]
        public double Azimuth { get; set; }

        [Required]
        [Display(Name = "ShutterStatus")]
        public string ShutterStatus { get; set; }

        [Required]
        [Display(Name = "Slaved")]
        public bool Slaved { get; set; }

        [Required]
        [Display(Name = "Slewing")]
        public bool Slewing { get; set; }
    }

}
