using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ASCOM.DeviceInterface;
using ASCOM.DriverAccess;

namespace RACI.ASCOM.Service.Models
{
    public class FocuserViewModel: DeviceViewModel<Focuser>
    {
        public FocuserViewModel() : base()
        {

        }
        [Required]
        [Display(Name = "Position (Abs)")]
        public bool Absolute { get; set; }

        [Required]
        [Display(Name = "IsMoving")]
        public bool IsMoving { get; set; }

        [Required]
        [Display(Name = "Link")]
        public bool Link { get; set; }

        [Required]
        [Display(Name = "MaxIncrement")]
        public int MaxIncrement { get; set; }

        [Required]
        [Display(Name = "MaxStep")]
        public int MaxStep { get; set; }

        [Required]
        [Display(Name = "Position")]
        public int Position { get; set; }

        [Required]
        [Display(Name = "StepSize")]
        public double StepSize { get; set; }

        [Required]
        [Display(Name = "TempComp")]
        public bool TempComp { get; set; }

        [Required]
        [Display(Name = "TempCompAvailable")]
        public bool TempCompAvailable { get; set; }

        [Required]
        [Display(Name = "Temperature")]
        public double Temperature { get; set; }

    }
}
