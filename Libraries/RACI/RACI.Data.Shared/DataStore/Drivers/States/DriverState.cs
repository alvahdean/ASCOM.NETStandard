using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RACI.Data
{
    
    public class DriverState
    {
        [Key]
        [Display(Name = "Id")]
        public int Id;

        [Required]
        [Display(Name = "Connected")]
        public bool Connected { get; set; } = false;
    }
    
}
