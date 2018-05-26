using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ASCOM.DeviceInterface;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace RACI.ASCOM.Service.Models
{
    public class DeviceViewModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger _logger;

        public DeviceViewModel() 
        {
            SupportedActions = new List<string>();
        }

        [Required]
        [Display(Name = "Connected")]
        public bool Connected { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "DriverInfo")]
        public string DriverInfo { get; set; }

        [Required]
        [Display(Name = "DriverVersion")]
        public string DriverVersion { get; set; }

        [Required]
        [Display(Name = "InterfaceVersion")]
        public short InterfaceVersion { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "SupportedActions")]
        public List<string> SupportedActions { get; set; }
    }

    public class DeviceViewModel<IDriver> : DeviceViewModel
        where IDriver : IAscomDriver
    {
        [Required]
        [Display(Name = "DeviceInstance")]
        public Dictionary<string,IDriver> DeviceInstance { get; set; }

        public DeviceViewModel() :base()
        {
            DeviceInstance = new Dictionary<string, IDriver>();
        }
    }
}
