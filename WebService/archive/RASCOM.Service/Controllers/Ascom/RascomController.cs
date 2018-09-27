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
        protected ILogger _logger;
        protected readonly UserManager<RaciUser> _userManager;
        protected SystemHelper _sysHelper;

        protected RascomController(
            UserManager<RaciUser> userManager,
            SystemHelper sysHelper,
            ILogger logger) : base()
        {
            _userManager = userManager;
            _logger = logger;
            _sysHelper = sysHelper;
        }
    }
}
