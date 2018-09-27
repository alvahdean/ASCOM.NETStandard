using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace ASCOM.WebService.Models
{
    public class RascomResult: RACI.Data.RestObjects.RascomResult
    {
        public RascomResult() : base() { }
        public RascomResult(object data, bool success = true, String msg = null) : base(data, success, null) { }

        public static implicit operator JsonResult(RascomResult result) => result.JsonResult;
        [JsonIgnore, IgnoreDataMember]
        public JsonResult JsonResult
        {
            get
            {
                JsonResult result = new JsonResult(this);
                return result;
            }
        }
    }
}
