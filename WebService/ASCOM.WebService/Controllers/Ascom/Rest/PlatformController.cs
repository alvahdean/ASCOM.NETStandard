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
    [Authorize]
    [Produces("application/json")]
    [Route("rascom/[controller]", Name = "Platform")]
    public class PlatformController : RascomController
    {
        protected static object syncRoot = new object();
        protected static Util util = new Util();

        public PlatformController(
            SystemHelper sysHelper,
            UserManager<RaciUser> userManager,
            SignInManager<RaciUser> signInManager,
            ILogger<PlatformController> logger)
            : base(sysHelper,userManager, signInManager, logger) { }

        [HttpGet(Name = "GetPlatform")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult Index()
        {

            var pVals=_sysHelper.Ascom.Values
                .Where(t=>t.Key!="")
                .Select(t=>new KeyValuePair<string,string>(t.Key,t.Value))
                .ToList();
            return (JsonResult)new JsonResult(pVals);
            //return new JsonResult(result);
        }

        [HttpGet("RegisteredDeviceTypes", Name = "GetPlatformRegisteredDeviceTypes")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public JsonResult GetRegisteredDeviceTypes()
        {
            Profile p = new Profile();
            return new JsonResult(p.RegisteredDeviceTypes);
        }

        [HttpGet("RegisteredDevices/{driverType}", Name = "GetPlatformRegisteredDevices")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //[Route("{driverType}")]
        public IActionResult GetRegisteredDevices(string driverType)
        {
            Profile p = new Profile();
            return new JsonResult(p.RegisteredDevices(driverType));
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

            return new JsonResult(prf.ToDictionary());
        }

        [HttpGet("SerialTrace", Name = "GetSerialTrace")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //[Route("SerialTrace")]
        public IActionResult GetSerialTrace()
        {
            return new JsonResult(util.SerialTrace);
        }

        [HttpPost("SerialTrace/{value}", Name = "SetSerialTrace")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //[Route("SerialTrace/{value}")]
        public IActionResult SetSerialTrace(bool value)
        {
            JsonResult result;
            try
            {
                //bool bvalue = bool.Parse(value);
                util.SerialTrace = value;
                result = new JsonResult(util.SerialTrace);
            }
            catch(Exception ex) { result = new JsonResult(ex); }
            return result;
        }

        [HttpGet("SerialTraceFile", Name = "GetSerialTraceFile")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult GetSerialTraceFile()
        {
            return new JsonResult(util.SerialTraceFile);
        }

        [HttpPost("SerialTraceFile/{traceFile}", Name = "SetSerialTraceFile")]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //[Route("SerialTraceFile/{value}")]
        public IActionResult SetSerialTraceFile(String traceFile)
        {
            util.SerialTraceFile = traceFile;
            return new JsonResult(util.SerialTraceFile);
        }

        [HttpGet("JulianDate", Name = "GetJulianDate")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult GetJulianDate()
        {
            return new JsonResult(util.JulianDate);
        }

        [HttpGet("TimeZoneName", Name = "GetTimeZoneName")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult TimeZoneName()
        {
            return new JsonResult(util.TimeZoneName);
        }

        [HttpGet("TimeZoneOffset", Name = "GetTimeZoneOffset")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult TimeZoneOffset()
        {
            return new JsonResult(util.TimeZoneOffset);
        }

        [HttpGet("UTCDate", Name = "GetUTCDate")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult UTCDate()
        {
            return new JsonResult(util.UTCDate);
        }

        [HttpGet("PlatformVersion", Name = "GetPlatformVersion")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult PlatformVersion()
        {
            return new JsonResult(util.PlatformVersion);
        }
    }
}