using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Collections;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ASCOM;
using ASCOM.Utilities;
using ASCOM.Utilities.Exceptions;
using ASCOM.WebService.Models;
using ASCOM.Internal;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using RACI.Data;
using ASCOM.Utilities.Interfaces;

namespace ASCOM.WebService.Controllers
{


    [Route("rascom/[controller]", Name = "Service")]
    public class ServiceController : Controller
    {

        public ServiceController(ILogger<PlatformController> logger) : base() { }

        [HttpGet]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult Index()
        {
            return Redirect("/");
        }

        [HttpGet("ping")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult Ping()
        {
            return new JsonResult(true);
        }

        [HttpGet("RegisteredDeviceTypes")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult DeviceTypes()
        {
            Profile p = new Profile();
            return new JsonResult(p.RegisteredDeviceTypes);
        }
        [HttpGet("RegisteredDevices")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult Devices()
        {
            return Devices(null);
        }

        [HttpGet("RegisteredDevices/{driverType}")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult Devices(string driverType)
        {
            driverType = driverType?.Trim() ?? "";
            Profile p = new Profile();
            ArrayList result = new ArrayList();
            foreach(string dt in p.RegisteredDeviceTypes)
            {
                if (driverType == "" || String.Equals(driverType, dt, StringComparison.InvariantCultureIgnoreCase))
                {
                    p.DeviceType = dt;
                    ArrayList drvs = p.RegisteredDevices(dt);
                    if (drvs != null && drvs.Count > 0)
                        result.AddRange(drvs);
                }
            }
            return new JsonResult(result);
        }
    }
}