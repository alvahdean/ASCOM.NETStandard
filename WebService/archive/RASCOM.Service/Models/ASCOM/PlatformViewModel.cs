using ASCOM.Utilities;
using ASCOM.Utilities.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RACI.ASCOM.Service.Models
{
    public class PlatformModel
    {
        [EmailAddress]
        [Display(Name = "Admin Contact")]
        public string AdminContact { get; set; } = "";


        [Required]
        [Display(Name = "Root Pathl")]
        public string RootPath { get; set; } = "/rascom";

        [Required]
        [Display(Name = "Access Url")]
        public string AccessUrl { get; set; } = "";

        [Required]
        [Display(Name = "Service Name")]
        public string ServiceName { get; set; } = "";

        [Required]
        [Display(Name = "Platform Version")]
        public string PlatformVersion { get; set; } = "";

        [Required]
        [Display(Name = "Major Version")]
        public short MajorVersion { get; set; } = 0;

        [Required]
        [Display(Name = "Minor Version")]
        public short MinorVersion { get; set; } = 0;

        [Required]
        [Display(Name = "Service Pack")]
        public short ServicePack { get; set; } = 0;

        [Required]
        [Display(Name = "Build Number")]
        public short BuildNumber { get; set; } = 0;

        [Display(Name = "Device Types")]
        public List<string> DeviceTypes { get; set; } = new List<string>();

    }
}
