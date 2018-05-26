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
using RACI.ASCOM.Service.Models;
using ASCOM.Internal;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace RACI.ASCOM.Service.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("rascom/[controller]",Name ="Platform")]
    public class PlatformController : RascomController
    {
        private static Util util = new Util();
        public PlatformController(
            UserManager<ApplicationUser> userManager
            , ILogger<AccountController> logger) : base(userManager,logger)
        {

        }

        [HttpGet(Name = "GetPlatform")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult Index()
        {
            return new JsonResult(new JsonResult(new RascomResult(platform)));
        }

        [HttpGet("RegisteredDeviceTypes", Name = "GetPlatformRegisteredDeviceTypes")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult GetRegisteredDeviceTypes()
        {
            Profile p = new Profile();
            return new JsonResult(new JsonResult(new RascomResult(hlp.RegisteredDeviceTypes)));
        }

        [HttpGet("RegisteredDevices/{driverType}", Name = "GetPlatformRegisteredDevices")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //[Route("{driverType}")]
        public IActionResult GetRegisteredDevices(string driverType)
        {
            Profile p = new Profile();
            return new JsonResult(new RascomResult(hlp.RegisteredDevices(driverType)));
        }

        [HttpGet("Profile/{driverType}/{devName}", Name = "GetPlatformDeviceProfile")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //[Route("RegisteredDevices/{driverType}/{devName}")]
        public IActionResult GetDeviceProfile(string driverType,string devName)
        {
            return new JsonResult(new RascomResult(hlp.DeviceProfile(driverType,devName)));
        }

        [HttpGet("SerialTrace", Name = "GetSerialTrace")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //[Route("SerialTrace")]
        public IActionResult GetSerialTrace()
        {
            return new JsonResult(new RascomResult(util.SerialTrace));
        }

        [HttpPost("SerialTrace/{value}", Name = "SetSerialTrace")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //[Route("SerialTrace/{value}")]
        public IActionResult SetSerialTrace(bool value)
        {
            util.SerialTrace = value;
            return new JsonResult(new RascomResult(util.SerialTrace==value));
        }

        [HttpGet("SerialTraceFile", Name = "GetSerialTraceFile")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult GetSerialTraceFile()
        {
            return new JsonResult(new RascomResult(util.SerialTraceFile));
        }

        [HttpPost("SerialTraceFile/{traceFile}", Name = "SetSerialTraceFile")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //[Route("SerialTraceFile/{value}")]
        public IActionResult SetSerialTraceFile(String traceFile)
        {
            util.SerialTraceFile = traceFile;
            return new JsonResult(new RascomResult((object)util.SerialTraceFile, util.SerialTraceFile == traceFile, $"SerialTraceFile set to '{util.SerialTraceFile}'"));
        }

        [HttpGet("JulianDate", Name = "GetJulianDate")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult GetJulianDate()
        {
            return new JsonResult(new RascomResult(util.JulianDate));
        }

        [HttpGet("TimeZoneName", Name = "GetTimeZoneName")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult TimeZoneName()
        {
            return new JsonResult(new RascomResult(util.TimeZoneName));
        }

        [HttpGet("TimeZoneOffset", Name = "GetTimeZoneOffset")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult TimeZoneOffset()
        {
            return new JsonResult(new RascomResult(util.TimeZoneOffset));
        }

        [HttpGet("UTCDate", Name = "GetUTCDate")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult UTCDate()
        {
            return new JsonResult(new RascomResult(util.UTCDate));
        }
    }
}
