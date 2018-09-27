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
using RACI.Data;

namespace RACI.ASCOM.Service.Controllers
{
    public class ControllerHelper 
    {

        public ControllerHelper() 
        {
            KeyComparer = new CIKeyComparer();
        }
        public IEqualityComparer<string> KeyComparer { get; set; }
        public string ShortDriver(string driverName)
        {
            string fullTag = " Drivers";
            return driverName.EndsWith(fullTag)
                ? driverName.Substring(0, driverName.LastIndexOf(fullTag))
                : driverName;
        }
        public string FullDriver(string driverName)
        {
            string fullTag = " Drivers";
            return driverName.EndsWith(fullTag)
                ? driverName
                : $"{driverName}{fullTag}";
        }

        public bool IsSupportedType(string driver)
        {
            Profile prf = new Profile();

            return RegisteredDeviceTypes.Any(t => KeyComparer.Equals(driver, ShortDriver(driver)));
        }
        public bool IsRegisteredDevice(string driverName,string devName)
        {
            Profile prf = new Profile();
            return IsSupportedType(driverName)
                && RegisteredDevices(driverName).Any(t => KeyComparer.Equals(devName, t.Key));
        }

        public IEnumerable<String> RegisteredDeviceTypes 
        {
            get
            {
                Profile p = new Profile();
                return p.RegisteredDeviceTypes.ToStrings();
            }
        }

        public IEnumerable<KeyValuePair<String,String>> RegisteredDevices(string driverName)
        {
                Profile p = new Profile();
                if (!IsSupportedType(driverName))
                    return null;
                return  p.RegisteredDevices(ShortDriver(driverName)).ToKvps();
        }

        public IASCOMProfile DeviceProfile(string driverName,string devName)
        {
            driverName = ShortDriver(driverName);
            if (!IsRegisteredDevice(driverName, devName))
                return null;
            Profile p = new Profile() { DeviceType =  driverName};
            return p.GetProfile(devName);
        }
    }
}
