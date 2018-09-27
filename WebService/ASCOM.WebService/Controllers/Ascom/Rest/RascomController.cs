using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Collections;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using ASCOM.WebService.Models;
using ASCOM.Utilities;
using ASCOM.DriverAccess;
using ASCOM.Utilities.Exceptions;
using ASCOM.Internal;
using ASCOM.Utilities.Interfaces;
using RACI.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using ASCOM.DeviceInterface;

namespace ASCOM.WebService.Controllers
{
    public abstract class RascomController : Controller
    {
        public const string RootPath = "/rascom";
        protected readonly ILogger _logger;
        protected readonly UserManager<RaciUser> _userManager;
        protected readonly SignInManager<RaciUser> _signInManager;
        protected readonly SystemHelper _sysHelper;

        protected RascomController(
            SystemHelper sysHelper,
            UserManager<RaciUser> userManager,
            SignInManager<RaciUser> signInManager,
            ILogger logger)
            : base()
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _sysHelper = sysHelper;
        }

    }
}
