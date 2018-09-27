using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ASCOM.DeviceInterface;
using ASCOM.DriverAccess;
using RACI.Data;

namespace ASCOM.WebService.Models
{
    public class DomeViewModel //: DeviceViewModel<Focuser>
    {
        public DomeViewModel() : base() { }

        [Required]
        [Display(Name = "Dome Metadata")]
        public DomeMeta Meta { get; set; }

        [Required]
        [Display(Name = "Dome Properties")]
        public DomeProperties Properties { get; set; }

        [Required]
        [Display(Name = "Dome State")]
        public DomeState State { get; set; }

    }
}
