
#define Dome

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Runtime.InteropServices;
using ASCOM;
using ASCOM.Astrometry;
using ASCOM.Astrometry.AstroUtils;
using ASCOM.Utilities;
using ASCOM.DeviceInterface;
using System.Globalization;
using System.Collections;
using System.Linq;
using RestSharp;
using RACI.Data.RestObjects;

namespace ASCOM.RACI
{
    /// <summary>
    /// ASCOM Dome Driver for RACI.
    /// </summary>
    [Guid("18c229a2-5a78-4fff-881e-e6067b6f1cdd")]
    [ClassInterface(ClassInterfaceType.None)]
    public class RaciClient : RestClient
    {
        private  string _epDriver;
        private string[] _knownEndpoints = new string[] { };
        private string[] _epDrivers = new string[] { };
        private TraceLogger tl;
        public bool TraceState
        {
            get => tl.Enabled;
            set
            {
                if (tl.Enabled != value)
                {

                    if (tl.Enabled)
                    {
                        LogMessage("Internal", $"TraceState changed {tl.Enabled}=>{value}");
                        tl.Enabled = value;
                    }
                    else
                    {
                        tl.Enabled = value;
                        LogMessage("Internal", $"TraceState changed {!value}=>{value}");
                    }
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RACI"/> class.
        /// Must be public for COM registration.
        /// </summary>
        public RaciClient() : this("") { }
        public RaciClient(string url) : this(url, "") { }
        public RaciClient(string url,string driver)
        {
            tl = new TraceLogger("", "RACI");
            if(!String.IsNullOrWhiteSpace(url))
            {
                Url = url;
                if(!String.IsNullOrWhiteSpace(driver))
                    Driver = driver;
            }
        }

        public string Url
        {
            get => BaseUrl?.AbsoluteUri;
            set
            {
                BaseUrl = String.IsNullOrWhiteSpace(value) ? null : new Uri(value);
                _epDrivers = QueryDrivers().Keys.ToArray();
            }
        }
        public string Driver
        {
            get => _epDriver;
            set
            {
                _epDriver = "";
                value = value?.Trim() ?? "";
                if (_knownEndpoints.Contains(value))
                    _epDriver = value;
            }
        }

        public void Dispose()
        {
            // Clean up the tracelogger and util objects
            tl.Enabled = false;
            tl.Dispose();
            tl = null;
        }
        public Dictionary<string,string> QueryDrivers()
        {
            Dictionary<string, string> driverList = new Dictionary<string, string>();
            RestRequest req = new RestRequest("service/RegisteredDrivers", RestSharp.Method.GET);
            var result = RascomExecute<List<KeyValuePair<string, string>>>(req);
                foreach(var kvp in result)
                    driverList.Add(kvp.Key, kvp.Value);
            return driverList;
        }

        #region Base Sync
        protected T RascomResponseHandler<T>(IRestResponse<RascomResult<T>> response)
        {
            if (response == null)
                return default(T);

            if (response.ErrorException != null)
                throw response.ErrorException;

            try
            {
                if (!response.Data.Success)
                {
                    string msg =
                        response.Data?.Exception?.Message
                        ?? response.Data?.Message ?? "Unspecified error";
                    throw new ApplicationException($"[Request Error]: {msg}");
                }
            }
            catch (Exception ex)
            {
                LogMessage("RaciClient",$"[Service Request Failed]: {ex.GetType().Name}");
                LogMessage("RaciClient",$"\tMessage: {ex.Message}");
                LogMessage("RaciClient",$"\tRequest: {response?.Request?.Resource ?? "null"}");
                throw ex;
            }
            return response.Data.Data;
        }

        /// <summary>
        /// Send a non-generic RascomRequest
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected RascomResult RascomExecute(IRestRequest request) => RascomExecute<RascomResult>(request);

        /// <summary>
        /// Send a generic RascomRequest
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        protected T RascomExecute<T>(IRestRequest request)
        {
            IRestResponse<RascomResult<T>> response = Execute<RascomResult<T>>(request);
            T result = RascomResponseHandler(response);
            return result;
        }
        #endregion

        #region Private properties and methods
        /// <summary>
        /// Log helper function that takes formatted strings and arguments
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        protected void LogMessage(string identifier, string message, params object[] args)
        {
            var msg = string.Format(message, args);
            tl.LogMessage(identifier, msg);
        }
        #endregion
    }
}
