using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RACI.Data
{
    public class SimpleValue
    {
        [Key]
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
