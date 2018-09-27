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
using RACI.Data;
using ASCOM.Utilities.Interfaces;

namespace RACI.ASCOM.Service.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("rascom/[controller]", Name = "Platform")]
    public class PlatformController : RascomController
    {
        protected static object syncRoot = new object();
        protected static Util util = new Util();

        public PlatformController(
            UserManager<RaciUser> userManager,
            SystemHelper sysHelper,
            ILogger logger) : base(userManager, sysHelper, logger) { }

        [HttpGet(Name = "GetPlatform")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult Index()
        {
            return new RascomResult(_sysHelper.Ascom.Values).JsonResult;
        }

        [HttpGet("RegisteredDeviceTypes", Name = "GetPlatformRegisteredDeviceTypes")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult GetRegisteredDeviceTypes()
        {
            Profile p = new Profile();
            return new RascomResult(p.RegisteredDeviceTypes).JsonResult;
        }

        [HttpGet("RegisteredDevices/{driverType}", Name = "GetPlatformRegisteredDevices")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //[Route("{driverType}")]
        public IActionResult GetRegisteredDevices(string driverType)
        {
            Profile p = new Profile();
            return new RascomResult(p.RegisteredDevices(driverType)).JsonResult;
        }

        [HttpGet("Profile/{driverType}/{devName}", Name = "GetPlatformDeviceProfile")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //[Route("RegisteredDevices/{driverType}/{devName}")]
        public IActionResult GetDeviceProfile(string driverType, string devName)
        {
            Profile p = new Profile() { DeviceType = driverType };
            if (!p.IsRegistered(devName))
                return new NotFoundObjectResult($"{driverType}:{devName}");
            IASCOMProfile prf = p.GetProfile(devName);
            return new RascomResult(prf).JsonResult;
        }

        [HttpGet("SerialTrace", Name = "GetSerialTrace")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //[Route("SerialTrace")]
        public IActionResult GetSerialTrace()
        {
            return new RascomResult(util.SerialTrace).JsonResult;
        }

        [HttpPost("SerialTrace/{value}", Name = "SetSerialTrace")]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //[Route("SerialTrace/{value}")]
        public IActionResult SetSerialTrace(bool value)
        {
            util.SerialTrace = value;
            return new RascomResult(util.SerialTrace).JsonResult;
        }

        [HttpGet("SerialTraceFile", Name = "GetSerialTraceFile")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult GetSerialTraceFile()
        {
            return new RascomResult(util.SerialTraceFile).JsonResult;
        }

        [HttpPost("SerialTraceFile/{traceFile}", Name = "SetSerialTraceFile")]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //[Route("SerialTraceFile/{value}")]
        public IActionResult SetSerialTraceFile(String traceFile)
        {
            util.SerialTraceFile = traceFile;
            return new RascomResult(util.SerialTraceFile).JsonResult;
        }

        [HttpGet("JulianDate", Name = "GetJulianDate")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult GetJulianDate()
        {
            return new RascomResult(util.JulianDate).JsonResult;
        }

        [HttpGet("TimeZoneName", Name = "GetTimeZoneName")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult TimeZoneName()
        {
            return new RascomResult(util.TimeZoneName).JsonResult;
        }

        [HttpGet("TimeZoneOffset", Name = "GetTimeZoneOffset")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult TimeZoneOffset()
        {
            return new RascomResult(util.TimeZoneOffset).JsonResult;
        }

        [HttpGet("UTCDate", Name = "GetUTCDate")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult UTCDate()
        {
            return new RascomResult(util.UTCDate).JsonResult;
        }

        [HttpGet("PlatformVersion", Name = "GetPlatformVersion")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult PlatformVersion()
        {
            return new RascomResult(util.PlatformVersion).JsonResult;
        }
    }
}