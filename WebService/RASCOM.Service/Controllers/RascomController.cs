using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Collections;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using RACI.ASCOM.Service.Models;
using ASCOM.Utilities;
using ASCOM.DriverAccess;
using ASCOM.Utilities.Exceptions;
using ASCOM.Internal;
using ASCOM.Utilities.Interfaces;
using RACI.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace RACI.ASCOM.Service.Controllers
{
    public abstract class RascomController : Controller
    {
        public const string RootPath = "/rascom";

        protected static ControllerHelper hlp;
        protected static PlatformModel platform;
        protected ILogger logger;
        private readonly UserManager<ApplicationUser> _userManager;

        protected RascomController(
            UserManager<ApplicationUser> userManager, 

            ILogger logger) : base()
        {
            hlp = new ControllerHelper();
            string localHost = RascomInfo.SysName;
            uint localPort = RascomInfo.Port;
            Profile prfUtil = new Profile();
            Util util = new Util();

            platform = new PlatformModel()
            {
                AdminContact = "admin@havlaconsortium.com",
                ServiceName = "RASCOM WebService",
                RootPath = RootPath,
                AccessUrl = $"http://{localHost}:{localPort}{RootPath}",
                MajorVersion = (short)util.MajorVersion,
                MinorVersion = (short)util.MinorVersion,
                BuildNumber = (short)util.BuildNumber,
                ServicePack = (short)util.ServicePack,
                PlatformVersion = util.PlatformVersion,
                DeviceTypes = prfUtil.RegisteredDeviceTypes.ToStrings()
            };
            _userManager = userManager;
            this.logger = logger;
        }
    }
}
