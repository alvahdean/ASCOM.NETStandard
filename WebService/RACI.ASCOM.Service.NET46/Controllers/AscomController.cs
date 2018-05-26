using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ASCOM.DeviceInterface;
using ASCOM.DriverAccess;
using ASCOM.Utilities;

namespace RACI.ASCOM.Service.Controllers
{
    //[Authorize]
    public class AscomController : ApiController
    {

        private static Dictionary<string, List<string>> _drivers =
            new Dictionary<string, List<string>>();
        protected static Util _util;
        protected static Profile _prof;
        protected static List<string> _deviceTypes;

        static AscomController()
        {
            _util= new Util();
            _prof = new Profile(true);
            foreach (string dType in _prof.RegisteredDeviceTypes)
            {
                _drivers.Add(dType, new List<string>());
                foreach (string dName in _prof.RegisteredDevices(dType))
                {

                    _drivers[dType].Add(dName);
                }
                Profile dProf=new Profile(true) { DeviceType}
            }
        }
        [HttpGet]
        public AscomRemoteResult<bool> Connected(string type, string id)
        {
            AscomRemoteState state = AscomRemoteState.Running;
            bool result = false;

            AscomRemoteResult<bool> response = new AscomRemoteResult<bool>()
            {

            };

            response.State = state;
            return response;
        }

        [HttpPut]
        public bool Connected(string type, string id, [FromBody] bool connected)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public string Description(string type, string id)
        {
            throw new NotImplementedException();
        }

        public string DriverInfo(string type, string id)
        {
            throw new NotImplementedException();
        }

        public string DriverVersion(string type, string id)
        {
            throw new NotImplementedException();
        }

        public short InterfaceVersion(string type, string id)
        {
            throw new NotImplementedException();
        }

        public string Name(string type, string id)
        {
            throw new NotImplementedException();
        }

        public ArrayList SupportedActions(string type, string id)
        {
            throw new NotImplementedException();
        }

        public void SetupDialog(string type, string id)
        {
            throw new NotImplementedException();
        }

        public string Action(string type, string id, string ActionName, string ActionParameters)
        {
            throw new NotImplementedException();
        }

        public void CommandBlind(string type, string id, string Command, bool Raw = false)
        {
            throw new NotImplementedException();
        }

        public bool CommandBool(string type, string id, string Command, bool Raw = false)
        {
            throw new NotImplementedException();
        }

        public string CommandString(string type, string id, string Command, bool Raw = false)
        {
            throw new NotImplementedException();
        }

        private AscomDriver GetDriver(string type, string id)
        {
            AscomDriver result = null;
            switch (type)
            {
                case "Focuser":
                    result
                    break;
                case "Telescope":
                    break;
                case "Camera":
                    break;
                case "Dome":
                    break;
                case "FilterWheel":
                    break;
                case "ObservingConditions":
                    break;
                case "Rotator":
                    break;
                case "SafetyMonitor":
                    break;
                case "Switch":
                    break;
                default:
                    break;
            }
            return result;
        }

        private bool DriverExists(string type, string id)
        {

                //.Any(t => t==type);
        }
    }
}
