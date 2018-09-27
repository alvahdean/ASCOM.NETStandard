using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
//using Microsoft.Extensions.Logging;
using RACI.Data;
using RestSharp;
using RestSharp.Authenticators;

namespace RACI.Client
{
    //TODO: Add locking for drivers
    //TODO: Load/Save config in AscomDataStore
//    public class ServiceInfo: IDisposable
//    {
//        static ServiceInfo()
//        {
//#if DEBUG
//            string epName = "Local Development";
//            string epUrl = "https://localhost:44378/rascom";
//            RaciServiceManager.AddEndpoint(epUrl,epName);
//            RaciServiceManager.AddDriver(epName, "Dome","xASCOM.Simulator.Dome");
//            RaciServiceManager.AddDriver(epName, "Focuser", "xASCOM.Simulator.Focuser");
//            RaciServiceManager.AddDriver(epName, "FilterWheel", "xASCOM.Simulator.FilterWheel");
//#endif
//        }

//        #region Internal members
//        private static ProfileKeyComparer _keyComp = new ProfileKeyComparer();
//        protected ILogger _logger;
//        private object _credentials;
//        private bool HaveAuth { get => _credentials!=null; }
//        #endregion

//        #region Instance management
//        public ServiceInfo() : this(null,null,null) { }
//        public ServiceInfo(RaciEndpoint ep,object credentials,ILogger log)
//        {
//            _logger=log ?? new LoggerFactory().AddConsole().AddDebug().CreateLogger(GetType());
//            _credentials = credentials;
//        }
//        #endregion  

//        #region Public members
//        private bool Connect(RaciEndpoint ep)
//        {
//            Disconnect();
//            if (ep == null)
//                return false;

//            return IsConnected;
//        }
//        private bool ConnectAdHoc(String epUrl, string epName = null, bool register = true)
//        {
//            epName = epName?.Trim() ?? String.Empty;
//            RaciEndpoint ep = RaciServiceManager.GetEndpointByUrl(epUrl);
//            if (ep == null)
//                ep = new RaciEndpoint(epUrl, epName);
//            return Connect(ep);
//        }
//        private void Disconnect()
//        {
//            IsConnected = false;
//            ServiceUri = null;

//        }
//        public IRascomResult Ping()
//        {
//            IRascomResult result=null;
//            return result;
//        }
//        public T Execute<T>(RestRequest request) where T : new()
//        {
//            if (!IsConnected)
//                throw new NotConnectedException($"Not connected to Service '{_epp}'");
//            var client = new RestClient();
//            client.BaseUrl = new System.Uri(BaseUrl);

//            client.Authenticator = new HttpBasicAuthenticator(_accountSid, _secretKey);
//            request.AddParameter("AccountSid", _accountSid, ParameterType.UrlSegment); // used on every request
//            var response = client.Execute<T>(request);

//            if (response.ErrorException != null)
//            {
//                const string message = "Error retrieving response.  Check inner details for more info.";
//                var twilioException = new ApplicationException(message, response.ErrorException);
//                throw twilioException;
//            }
//            return response.Data;
//        }
//        #endregion

//        public bool IsConnected { get; private set; }
//        public object Credentials { get; set; }
//        public string EndpointName { get; private set; }
//        public string Endpointurl { get; private set; }
//        public RaciClient Client { get; private set; }
//        public IEnumerable<string> DriverTypes { get => RaciServiceManager.GetDriverTypes(_ep); }
//        public bool DriverTypeExists(string driverType) => DriverTypes?.Contains(driverType, _keyComp)??false;
//        public IEnumerable<KeyValuePair<string,string>> DriverInfo(string driverType)
//        {

//            return RaciServiceManager
//                .EndpointDrivers(_ep?.Name)
//                .Where(t=> _keyComp.Equals(t.Name,t.DriverType))
//                .Select(t=>new KeyValuePair<string, string>(t.Name,t.Description));
//        }

//        #region IDisposable Support
//        private bool disposedValue = false; // To detect redundant calls

//        protected virtual void Dispose(bool disposing)
//        {
//            if (!disposedValue)
//            {
//                if (disposing)
//                {
//                    // TODO: dispose managed state (managed objects).
//                }

//                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
//                // TODO: set large fields to null.

//                disposedValue = true;
//            }
//        }

//        public void Dispose()
//        {
//            Dispose(true);
//            // TODO: uncomment the following line if the finalizer is overridden above.
//            // GC.SuppressFinalize(this);
//        }
//        #endregion
//    }
}
