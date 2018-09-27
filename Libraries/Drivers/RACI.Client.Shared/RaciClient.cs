using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.Extensions.Logging;
using RACI.Data;
using RACI.Data.RestObjects;
using RACI.Logging;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

/*

*/

namespace RACI.Client
{
    public class RaciClient : RestClient
    {
        #region Class Initialization
        static RaciClient()
        {
            keyComp = new ProfileKeyComparer();
            LogFactory = RaciLog.Factory;
        }
        public RaciClient() : base()
        {
            asyncResponses = new Dictionary<RestRequestAsyncHandle, IRascomResult>();
            FollowRedirects = true;
            rnd = new Random();
            Logger = LogFactory.CreateLogger(GetType());
            SupportedMethods = new List<Method>() { Method.DELETE, Method.GET, Method.POST, Method.PUT };
            Driver = null;
            Drivers = new List<RaciEndpointDriver>();
            Endpoint = null;

        }
        /// <summary>
        /// Initialize a client instance with a specified identifier
        /// </summary>
        /// <param name="epId">An registered endpoint name or a valid service URL.
        /// If a valid url is specified and does not exist in the registry it will be automatically registered.
        /// </param>
        public RaciClient(string epId) : this()
        {
            epId = epId?.Trim() ?? "";
            Logger.LogInformation($"Creating instance with identifier: '{epId}'");
            if (string.IsNullOrWhiteSpace(epId))
            {
                Logger.LogWarning($"Empty service identifer, endpoint will remain unset");
            }
            else if (RaciServiceManager.IsValidUrl(epId))
            {
                Endpoint = RaciServiceManager.GetEndpointByUrl(epId)
                    ?? RaciServiceManager.AddEndpoint(epId)
                    ?? throw new ApplicationException($"Unable to obtain registry entry for service: {epId}");
                Logger.LogInformation($"Registry Entry: [{Endpoint.Name}].[{Endpoint.ServiceRoot}]");
            }
            else
            {
                Endpoint = RaciServiceManager.GetEndpointByName(epId);
                if (Endpoint != null)
                    Logger.LogInformation($"Registry Entry: [{Endpoint.Name}].[{Endpoint.ServiceRoot}]");
                else
                {
                    Logger.LogWarning($"Invalid service identifer: '{epId}', endpoint will remain unset");
                }
            }

            if (Endpoint != null)
            {
                Logger.LogInformation($"Loading drivers for endpoint '{Endpoint.Name}'...");
                Drivers.AddRange(RaciServiceManager.GetDrivers(Endpoint));
                Logger.LogInformation($"\tFound {Drivers.Count} drivers:");
#if DEBUG
                foreach (var dtype in Drivers.Select(t => t.DriverType).Distinct().OrderBy(t => t))
                {
                    Logger.LogDebug($"\t[{dtype}]");
                    foreach (var drv in Drivers.Where(t => t.DriverType == dtype))
                        Logger.LogDebug($"\t\t{drv.Name}");
                }
#endif
            }
        }
        public RaciClient(RaciEndpoint ep, string driverName) : this(ep?.ServiceRoot, driverName) { }
        public RaciClient(string epId, string driverName) : this(epId)
        {
            driverName = driverName?.Trim() ?? "";
            if (Endpoint == null)
            {
                Logger.LogInformation($"No endpoint with identifier '{epId}' found, skipping driver setup...");
            }
            else if (String.IsNullOrWhiteSpace(driverName))
            {
                Logger.LogInformation($"Empty driver, skipping driver setup...");
            }
            else if (!Drivers.AnyWithName(driverName))
            {
                Logger.LogInformation($"No driver named {driverName} found in the service registry, skipping driver setup...");
            }
            else
            {
                Logger.LogInformation($"Driver setup for {driverName}...");
                Driver = Drivers.WithName(driverName);
                if (Driver == null)
                    throw new ApplicationException($"Unable to get driver object for registered driver '{driverName}'");
            }
        }
        #endregion

        #region Non-public Fields/properties
        protected ILogger Logger { get; }
        protected Random rnd { get; }
        protected static ProfileKeyComparer keyComp;
        protected List<RaciEndpointDriver> Drivers { get; set; }
        protected RaciEndpointDriver Driver { get; set; }
        private Dictionary<RestRequestAsyncHandle, IRascomResult> asyncResponses;
        #endregion

        #region Public Properties
        public static ILoggerFactory LogFactory { get; private set; }
        public List<Method> SupportedMethods { get; private set; }
        public string EndpointName { get => Endpoint?.Name; }
        public string EndpointHost { get => BaseUrl?.DnsSafeHost; }
        public string EndpointUrl { get => BaseUrl?.AbsoluteUri; }
        public string DriverUrl
        {
            get
            {
                EnsureDriverConfigured();
                return $"{EndpointUrl}/devices/{DriverType}/{DriverName}";
            }
        }
        public string Username { get; set; }
        public string UserToken { get; set; }
        protected string ApiTokenHeader { get; set; } = "RASCOM-API-TOKEN";
        protected string ApiToken { get; set; } = "";
        protected RaciEndpoint Endpoint { get; private set; }
        protected List<string> DriverTypes => Drivers?.Select(t => t.Name).Distinct().OrderBy(t => t).ToList();
        protected string Hostname => BaseUrl?.DnsSafeHost;
        protected string Protocol => BaseUrl?.Scheme;
        protected string RootPath => BaseUrl?.AbsolutePath;
        protected string DriverName => Driver?.Name;
        protected string DriverType => Driver?.DriverType;
        protected int Port => BaseUrl?.Port ?? 0;
        protected bool AsyncMode { get; set; } = false;
        #endregion

        #region Authentication

        protected IRestRequest AddApiKey(IRestRequest req, string apiToken)
        {
            Parameter curr = req.Parameters.FirstOrDefault(t =>
                t.Name == ApiTokenHeader
                && t.Type == ParameterType.HttpHeader);
            if (curr != null)
                curr.Value = apiToken;
            else if (curr == null)
                req.AddParameter(ApiTokenHeader, apiToken, ParameterType.HttpHeader);
            return req;
        }
        protected IRestRequest AddAuth(IRestRequest req)
        {
            HttpBasicAuthenticator credentials = null;
            if (!String.IsNullOrWhiteSpace(Username) && !String.IsNullOrWhiteSpace(UserToken))
                credentials = new HttpBasicAuthenticator(Username, UserToken);
            return AddAuth(req, credentials, false);
        }
        protected IRestRequest AddAuth(IRestRequest req, IAuthenticator credentials)
        {
            return AddAuth(req, credentials, false);
        }
        virtual protected IRestRequest AddAuth(IRestRequest req, IAuthenticator credentials, bool replace)
        {
            if (!String.IsNullOrWhiteSpace(Username) && !String.IsNullOrWhiteSpace(UserToken))
                credentials = credentials ?? new HttpBasicAuthenticator(Username, UserToken);
            if (credentials == null || req == null)
                return req;
            if (Authenticator != credentials)
                Authenticator = credentials;
            Type aType = credentials?.GetType();
            if (aType == typeof(HttpBasicAuthenticator))
                req.AddParameter("AccountSid", Username, ParameterType.UrlSegment);
            else
                Logger.LogWarning($"Unsupported authenticator type '{aType.FullName}'");
            return req;
        }
        virtual protected bool CheckCredentials(IAuthenticator credentials)
        {
            return credentials != null;
        }
        #endregion

        #region Service Registry members
        protected bool EndpointRegistered => RaciServiceManager.HasEndpointUrl(EndpointUrl);
        protected bool DriverRegistered => RaciServiceManager.HasDriver(Endpoint?.Name, Driver?.Name);

        #endregion

        #region Communication methods

        #region Connection utilities
        #region static utilities
        public static bool ValidateClient(RaciClient client, bool throwOnInvalid = true)
        {
            if (client == null || !ValidateUri(client.BaseUrl))
            {
                string msg = "Invalid client, aborting endpoint query";
                RaciLog.DefaultLogger.LogError(msg);
                if (throwOnInvalid)
                    throw new ApplicationException(msg);
                return false;
            }
            return true;
        }
        public static bool ValidateUri(Uri uri, bool throwOnInvalid = true)
        {
            if (uri != null || uri.IsAbsoluteUri)
                return true;
            string msg = $"Invalid Endpoint Uri: '{uri}'";
            RaciLog.DefaultLogger.LogError(msg);
            if (throwOnInvalid)
                throw new ApplicationException(msg);
            return false;
        }

        public static bool ValidateUri(string url, bool throwOnInvalid = true)
        {
            if (!String.IsNullOrWhiteSpace(url))
            {
                Uri uri = new Uri(url);
                if (uri != null || uri.IsAbsoluteUri)
                    return true;
            }
            string msg = $"Invalid Endpoint Url: '{url}'";
            RaciLog.DefaultLogger.LogError(msg);
            if (throwOnInvalid)
                throw new ApplicationException(msg);
            return false;
        }
        static public bool PingService(string url, int nPings = 5, int timeout = 5000, ILogger logger = null) =>
            PingService(Create(url));
        static public bool PingService(Uri uri, int nPings = 5, int timeout = 5000, ILogger logger = null) =>
            PingService(Create(uri));
        static public bool PingService(RaciClient client, int nPings = 5, int timeout = 5000, ILogger logger = null)
        {
            ValidateClient(client, true);
            ConnectionCheck result = ConnectionCheck.Run(client, nPings, timeout, logger);
            return result.Successful > 0 && result.Quality > .5d;
        }

        static public RaciClient Create(string url)
        {
            ValidateUri(url, true);
            return Create(new Uri(url));
        }
        static public RaciClient Create(Uri uri)
        {
            ValidateUri(uri);
            RaciClient client = new RaciClient();
            client.BaseUrl = uri;
            client.FollowRedirects = true;
            return client;
        }


        public static RaciEndpoint QueryEndpoint(string url) => QueryEndpoint(Create(url));
        public static RaciEndpoint QueryEndpoint(Uri uri) => QueryEndpoint(Create(uri));
        public static RaciEndpoint QueryEndpoint(RaciClient client)
        {
            ILogger log = RaciLog.DefaultLogger;
            if (client == null || !RaciServiceManager.IsValidUrl(client.BaseUrl.AbsoluteUri))
            {
                string msg = "Invalid client, aborting endpoint query";
                log.LogCritical(msg);
                throw new ApplicationException(msg);
            }
            Uri uri = client.BaseUrl;
            RaciEndpoint ep = new RaciEndpoint()
            {
                Name = uri.DnsSafeHost,
                ServiceRoot = uri.AbsoluteUri,
                Description = $"{uri.DnsSafeHost} Service"
            };
            log.LogInformation($"Target Endpoint: Server='{ep.Name}', Uri='{ep.ServiceRoot}'");
            log.LogInformation($"Connecting to Service...");

            foreach (var drv in QueryDrivers(uri))
            {
                drv.Parent = ep;
                ep.Nodes.Add(drv);
            }
            return ep;
        }

        public static IEnumerable<RaciEndpointDriver> QueryDrivers(string url) => QueryDrivers(Create(url));
        public static IEnumerable<RaciEndpointDriver> QueryDrivers(Uri uri) => QueryDrivers(Create(uri));
        public static IEnumerable<RaciEndpointDriver> QueryDrivers(RaciClient client)
        {
            ILogger log = RaciLog.DefaultLogger;
            ValidateClient(client, true);

            List<RaciEndpointDriver> result = new List<RaciEndpointDriver>();
            IRestRequest request = client.CreateRequest<List<string>>(Method.GET, $"service/RegisteredDeviceTypes");
            List<string> resp = client.RascomExecute<List<string>>(request);
            foreach (string dt in resp)
            {
                request = client.CreateRequest<List<string>>(Method.GET, $"service/RegisteredDevices/{dt}");
                List<KeyValuePair<string, string>> devList = client.RascomExecute<List<KeyValuePair<string, string>>>(request);
                foreach (var kvp in devList)
                {
                    RaciEndpointDriver driver = new RaciEndpointDriver(kvp.Key, kvp.Value) { DriverType = dt };
                    result.Add(driver);
                }
            }
            return result;
        }
        #endregion

        #region Instance utilities
        
        protected bool ConnectService(string url, string driverName = "")
        {
            string logPrefix = $"[{GetType().FullName} Connect]";
            FollowRedirects = true;
            try
            {
                Endpoint = RefreshEndpoint(url);
                if (!String.IsNullOrWhiteSpace(driverName))
                    ConnectDriver(driverName);

            }
            catch { return false; }
            return true;
        }
        protected RaciEndpoint RefreshEndpoint(string url, string driverName = "")
        {
            Clear();
            RaciEndpoint ep = QueryEndpoint(url);
            if (ep != null)
            {
                RaciServiceManager.RemoveDrivers(ep.Name);
                foreach (var drv in ep.Nodes)
                    RaciServiceManager.AddDriver(ep?.Name, drv);
            }
            return ep;
        }
        protected bool ConnectDriver(string driverName)
        {
            if (String.IsNullOrWhiteSpace(driverName))
                Driver = Endpoint.Nodes.WithName(driverName);
            return (Driver?.Name ?? "") == (driverName?.Trim() ?? "");
        }
        protected bool IsConfigured { get; private set; } = false;
        protected bool EnsureEndpointConfigured(bool throwEx = true)
        {
            bool result = ValidateUri(BaseUrl);
            if (!result)
            {
                string msg = $"[{nameof(EnsureEndpointConfigured)}]: Client endpoint is not configured. Url = '{BaseUrl.AbsoluteUri}'";
                Logger.LogError(msg);
                if (throwEx)
                    throw new ApplicationException(msg);
            }
            return result;
        }
        protected bool EnsureDriverConfigured(bool throwEx = true)
        {
            bool result = EnsureEndpointConfigured(throwEx);
            if (String.IsNullOrWhiteSpace(DriverType) || String.IsNullOrWhiteSpace(DriverName))
            {
                string msg = $"[{nameof(EnsureDriverConfigured)}]: Client driver is not configured. DriverType = '{DriverType}', Driver = '{DriverName}'";
                Logger.LogError(msg);
                if (throwEx)
                    throw new ApplicationException(msg);
            }
            return result;
        }
        protected void Clear()
        {
            Endpoint = null;
            Drivers.Clear();
            Driver = null;
            BaseUrl = null;
        }
        public IRestRequest CreateRequest<T>(Method method, string urlFmt, params object[] args) =>
            CreateRequest<T>(method, null, urlFmt, args);
        public IRestRequest CreateRequest<T>(Method method, IEnumerable<KeyValuePair<string, string>> parameters, string urlFmt, params object[] args)
        {
            string reqUrl = args != null
                ? String.Format(urlFmt, args)
                : urlFmt;
            IRestRequest request = new RestRequest(reqUrl, method);

            if (parameters != null)
                foreach (var kvp in parameters)
                    request.AddParameter(kvp.Key, kvp.Value, ParameterType.UrlSegment);
            return request;
        }
        #endregion
        #endregion

        #region Request/Response handlers
        protected IRestRequest PrepRequest(IRestRequest req)
        {
            EnsureEndpointConfigured();
            return AddAuth(req);
        }
        protected T RascomResponseHandler<T>(IRestResponse<T> response)
            where T : IRascomResult, new()
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
                Logger.LogError($"[Service Request Failed]: {ex.GetType().Name}");
                Logger.LogError($"\tMessage: {ex.Message}");
                Logger.LogError($"\tRequest: {response?.Request?.Resource ?? "null"}");
                throw ex;
            }
            return response.Data;
        }
        #endregion

        #region Synchronous Rascom requests

        #region Generic Sync
        protected RascomResult RascomDelete(string urlFmt) =>
            RascomDelete(null, urlFmt, null);
        protected RascomResult RascomDelete(string urlFmt, params object[] args) =>
            RascomDelete(null, urlFmt, args);
        protected RascomResult RascomDelete(IEnumerable<KeyValuePair<string, string>> parameters, string urlFmt, params object[] args) =>
            RascomExecute(Method.DELETE, null, urlFmt, args);

        protected RascomResult RascomPut(string urlFmt) =>
            RascomPut(null, urlFmt, null);
        protected RascomResult RascomPut(string urlFmt, params object[] args) =>
            RascomPut(null, urlFmt, args);
        protected RascomResult RascomPut(IEnumerable<KeyValuePair<string, string>> parameters, string urlFmt, params object[] args) =>
            RascomExecute(Method.PUT, null, urlFmt, args);

        protected RascomResult RascomPost(string urlFmt) =>
            RascomPost(null, urlFmt, null);
        protected RascomResult RascomPost(string urlFmt, params object[] args) =>
            RascomPost(null, urlFmt, args);
        protected RascomResult RascomPost(IEnumerable<KeyValuePair<string, string>> parameters, string urlFmt, params object[] args) =>
            RascomExecute(Method.POST, null, urlFmt, args);

        protected RascomResult RascomGet(string urlFmt) =>
            RascomGet(null, urlFmt, null);
        protected RascomResult RascomGet(string urlFmt, params object[] args) =>
            RascomGet(null, urlFmt, args);
        protected RascomResult RascomGet(IEnumerable<KeyValuePair<string, string>> parameters, string urlFmt, params object[] args) =>
            RascomExecute(Method.GET, null, urlFmt, args);
        #endregion

        #region Typed Sync
        protected T RascomDelete<T>(string urlFmt) =>
            RascomDelete<T>(null, urlFmt, null);
        protected T RascomDelete<T>(string urlFmt, params object[] args) =>
            RascomDelete<T>(null, urlFmt, args);
        protected T RascomDelete<T>(IEnumerable<KeyValuePair<string, string>> parameters, string urlFmt, params object[] args) =>
            RascomExecute<T>(Method.DELETE, null, urlFmt, args);

        protected T RascomPut<T>(string urlFmt) =>
            RascomPut<T>(null, urlFmt, null);
        protected T RascomPut<T>(string urlFmt, params object[] args) =>
            RascomPut<T>(null, urlFmt, args);
        protected T RascomPut<T>(IEnumerable<KeyValuePair<string, string>> parameters, string urlFmt, params object[] args) =>
            RascomExecute<T>(Method.PUT, null, urlFmt, args);

        protected T RascomPost<T>(string urlFmt) =>
            RascomPost<T>(null, urlFmt, null);
        protected T RascomPost<T>(string urlFmt, params object[] args) =>
            RascomPost<T>(null, urlFmt, args);
        protected T RascomPost<T>(IEnumerable<KeyValuePair<string, string>> parameters, string urlFmt, params object[] args) =>
            RascomExecute<T>(Method.POST, null, urlFmt, args);

        protected T RascomGet<T>(string urlFmt) =>
            RascomGet<T>(null, urlFmt, null);
        protected T RascomGet<T>(string urlFmt, params object[] args) =>
            RascomGet<T>(null, urlFmt, args);
        protected T RascomGet<T>(IEnumerable<KeyValuePair<string, string>> parameters, string urlFmt, params object[] args) =>
            RascomExecute<T>(Method.GET, null, urlFmt, args);
        #endregion

        #region Synchronous Exec variants
        protected RascomResult RascomExecute(Method method, IEnumerable<KeyValuePair<string, string>> parameters, string urlFmt, params object[] args) =>
            RascomExecute<RascomResult>(method, parameters, urlFmt, args);
        protected T RascomExecute<T>(Method method, IEnumerable<KeyValuePair<string, string>> parameters, string urlFmt, params object[] args) =>
            RascomExecute<T>(CreateRequest<T>(method, parameters, urlFmt, args));
        #endregion

        #region Base Sync
        /// <summary>
        /// Send a non-generic RascomRequest
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected RascomResult RascomExecute(IRestRequest request) =>
            RascomExecute<RascomResult>(request);
        /// <summary>
        /// Send a generic RascomRequest
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        protected T RascomExecute<T>(IRestRequest request)
        {
            EnsureEndpointConfigured();
            AddAuth(request);
            IRestResponse<RascomResult<T>> response = Execute<RascomResult<T>>(request);
            //RascomResult<T> result = RascomResponseHandler<T>(response);
            return response.Data.Data;
        }
        #endregion

        #endregion

        #region Async Rascom requests
        #region Generic Async
        protected async Task<RascomResult> RascomDeleteAsync(string urlFmt) =>
            await RascomDeleteAsync(null, urlFmt, null);
        protected async Task<RascomResult> RascomDeleteAsync(string urlFmt, params object[] args) =>
            await RascomDeleteAsync(null, urlFmt, args);
        protected async Task<RascomResult> RascomDeleteAsync(IEnumerable<KeyValuePair<string, string>> parameters, string urlFmt, params object[] args) =>
            await RascomExecuteAsync(Method.DELETE, null, urlFmt, args);

        protected async Task<RascomResult> RascomPutAsync(string urlFmt) =>
            await RascomPutAsync(null, urlFmt, null);
        protected async Task<RascomResult> RascomPutAsync(string urlFmt, params object[] args) =>
            await RascomPutAsync(null, urlFmt, args);
        protected async Task<RascomResult> RascomPutAsync(IEnumerable<KeyValuePair<string, string>> parameters, string urlFmt, params object[] args) =>
            await RascomExecuteAsync(Method.PUT, null, urlFmt, args);

        protected async Task<RascomResult> RascomPostAsync(string urlFmt) =>
            await RascomPostAsync(null, urlFmt, null);
        protected async Task<RascomResult> RascomPostAsync(string urlFmt, params object[] args) =>
            await RascomPostAsync(null, urlFmt, args);
        protected async Task<RascomResult> RascomPostAsync(IEnumerable<KeyValuePair<string, string>> parameters, string urlFmt, params object[] args) =>
            await RascomExecuteAsync(Method.POST, null, urlFmt, args);

        protected async Task<RascomResult> RascomGetAsync(string urlFmt) =>
            await RascomGetAsync(null, urlFmt, null);
        protected async Task<RascomResult> RascomGetAsync(string urlFmt, params object[] args) =>
            await RascomGetAsync(null, urlFmt, args);
        protected async Task<RascomResult> RascomGetAsync(IEnumerable<KeyValuePair<string, string>> parameters, string urlFmt, params object[] args) =>
            await RascomExecuteAsync(Method.GET, null, urlFmt, args);
        #endregion

        #region Typed Async
        protected async Task<T> RascomDeleteAsync<T>(string urlFmt) =>
            await RascomDeleteAsync<T>(null, urlFmt, null);
        protected async Task<T> RascomDeleteAsync<T>(string urlFmt, params object[] args) =>
            await RascomDeleteAsync<T>(null, urlFmt, args);
        protected async Task<T> RascomDeleteAsync<T>(IEnumerable<KeyValuePair<string, string>> parameters, string urlFmt, params object[] args) =>
            await RascomExecuteAsync<T>(Method.DELETE, null, urlFmt, args);

        protected async Task<T> RascomPutAsync<T>(string urlFmt) =>
            await RascomPutAsync<T>(null, urlFmt, null);
        protected async Task<T> RascomPutAsync<T>(string urlFmt, params object[] args) =>
            await RascomPutAsync<T>(null, urlFmt, args);
        protected async Task<T> RascomPutAsync<T>(IEnumerable<KeyValuePair<string, string>> parameters, string urlFmt, params object[] args) =>
            await RascomExecuteAsync<T>(Method.PUT, null, urlFmt, args);

        protected async Task<T> RascomPostAsync<T>(string urlFmt) =>
            await RascomPostAsync<T>(null, urlFmt, null);
        protected async Task<T> RascomPostAsync<T>(string urlFmt, params object[] args) =>
            await RascomPostAsync<T>(null, urlFmt, args);
        protected async Task<T> RascomPostAsync<T>(IEnumerable<KeyValuePair<string, string>> parameters, string urlFmt, params object[] args) =>
            await RascomExecuteAsync<T>(Method.POST, null, urlFmt, args);

        protected async Task<T> RascomGetAsync<T>(string urlFmt) =>
            await RascomGetAsync<T>(null, urlFmt, null);
        protected async Task<T> RascomGetAsync<T>(string urlFmt, params object[] args) =>
            await RascomGetAsync<T>(null, urlFmt, args);
        protected async Task<T> RascomGetAsync<T>(IEnumerable<KeyValuePair<string, string>> parameters, string urlFmt, params object[] args) =>
            await RascomExecuteAsync<T>(Method.GET, null, urlFmt, args);
        #endregion

        #region Async Exec variants
        //Common multi-param forms
        protected async Task<RascomResult> RascomExecuteAsync(Method method, IEnumerable<KeyValuePair<string, string>> parameters, string urlFmt, params object[] args) =>
            await RascomExecuteAsync<RascomResult>(method, parameters, urlFmt, args);
        protected async Task<T> RascomExecuteAsync<T>(Method method, IEnumerable<KeyValuePair<string, string>> parameters, string urlFmt, params object[] args) =>
            await RascomExecuteAsync<T>(CreateRequest<T>(method, parameters, urlFmt, args));

        protected async Task RascomExecuteAsync(IRestRequest request, Action<RascomResult> callback, CancellationToken? token = null) =>
            await RascomExecuteAsync<RascomResult>(request, callback, token);
        protected async Task RascomExecuteAsync<T>(IRestRequest request, Action<T> callback, CancellationToken? token = null)
        {
            T data = await RascomExecuteAsync<T>(request, token);
            callback(data);
        }
        protected async Task<bool> RascomExecuteAsync(IRestRequest request, Func<RascomResult, bool> callback, CancellationToken? token = null) =>
            await RascomExecuteAsync<RascomResult>(request, callback, token);
        protected async Task<bool> RascomExecuteAsync<T>(IRestRequest request, Func<T, bool> callback, CancellationToken? token = null)
        {
            T data = await RascomExecuteAsync<T>(request, token);
            return callback(data);
        }
        #endregion

        #region Base Async 

        protected async Task<RascomResult> RascomExecuteAsync(IRestRequest request, CancellationToken? token = null) =>
            await RascomExecuteAsync<RascomResult>(request, token);
        protected async Task<T> RascomExecuteAsync<T>(IRestRequest request, CancellationToken? token = null)
        {
            IRestResponse<RascomResult<T>> response = token.HasValue
                ? await ExecuteTaskAsync<RascomResult<T>>(request, token.Value)
                : await ExecuteTaskAsync<RascomResult<T>>(request);
            //RascomResult<T> result = RascomResponseHandler<RascomResult<T>>(response);
            return response.Data.Data;
        }
        #endregion

        #endregion

        #region RestClient override
        override public IRestResponse Execute(IRestRequest request) =>
            base.Execute(PrepRequest(request));

        override public IRestResponse<T> Execute<T>(IRestRequest request) =>
            base.Execute<T>(PrepRequest(request));

        override public RestRequestAsyncHandle ExecuteAsync(IRestRequest request, Action<IRestResponse, RestRequestAsyncHandle> callback) =>
            base.ExecuteAsync(PrepRequest(request), callback);

        override public RestRequestAsyncHandle ExecuteAsync<T>(IRestRequest request, Action<IRestResponse<T>, RestRequestAsyncHandle> callback) =>
            base.ExecuteAsync(PrepRequest(request), callback);

        public override Task<IRestResponse> ExecuteTaskAsync(IRestRequest request) =>
            base.ExecuteTaskAsync(PrepRequest(request));

        public override Task<IRestResponse<T>> ExecuteTaskAsync<T>(IRestRequest request) =>
            base.ExecuteTaskAsync<T>(PrepRequest(request));

        public override Task<IRestResponse> ExecuteTaskAsync(IRestRequest request, CancellationToken token) =>
            base.ExecuteTaskAsync(AddAuth(request), token);

        public override Task<IRestResponse<T>> ExecuteTaskAsync<T>(IRestRequest request, CancellationToken token) =>
            base.ExecuteTaskAsync<T>(PrepRequest(request), token);

        //public override Task<IRestResponse> ExecuteGetTaskAsync(IRestRequest request, CancellationToken token)
        //{
        //    return base.ExecuteGetTaskAsync(AddAuth(request), token);
        //}
        //public override Task<IRestResponse<T>> ExecuteGetTaskAsync<T>(IRestRequest request, CancellationToken token)
        //{
        //    return base.ExecuteGetTaskAsync<T>(AddAuth(request), token);
        //}
        //public override Task<IRestResponse> ExecutePostTaskAsync(IRestRequest request, CancellationToken token)
        //{
        //    return base.ExecutePostTaskAsync(AddAuth(request), token);
        //}
        //public override Task<IRestResponse<T>> ExecutePostTaskAsync<T>(IRestRequest request, CancellationToken token)
        //{
        //    return base.ExecutePostTaskAsync<T>(AddAuth(request), token);
        //}

        #endregion

        #endregion

        #region Utility Methods
        protected Stream DownloadUrlRaw(string uri)
        {
            return DownloadUrlRaw(new Uri(uri));
        }
        protected Stream DownloadUrlRaw(Uri uri)
        {
            IRestRequest dlReq = AddAuth(new RestRequest(uri, Method.GET));
            MemoryStream result = null;
            try
            {
                result = new MemoryStream(DownloadData(dlReq));
                if (result == null || result.Length <= 0)
                    throw new Exception($"No data returned");
                result.Position = 0;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Error downloading data from [{uri.AbsolutePath}]");
                throw ex;
            }
            return result;
        }
        protected byte[] DownloadUrlRawBytes(Uri uri)
        {
            IRestRequest req = AddAuth(new RestRequest(uri, Method.GET));
            byte[] result = null;
            try
            {
                result = DownloadData(req);
                if (result == null || result.Length <= 0)
                    throw new Exception($"No data returned");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Error downloading data from [{uri.AbsolutePath}]");
                throw ex;
            }
            return result;
        }
        protected MemoryStream Unzip(byte[] data)
        {
            return UnzipNext(new MemoryStream(data));
        }
        protected MemoryStream UnzipNext(Stream data)
        {
            MemoryStream result = null;
            byte[] buffer = new byte[4096];
            if (data == null)
                throw new ArgumentNullException($"{nameof(data)}");
            try
            {
                ZipInputStream zipInputStream = new ZipInputStream(data);
                ZipEntry zipEntry = zipInputStream.GetNextEntry();
                if (zipEntry != null)
                {
                    string name = zipEntry.Name ?? zipEntry.Offset.ToString();
                    Logger.LogInformation($"Decompressing ZipStream[{name}]: Starting...");
                    result = new MemoryStream();
                    StreamUtils.Copy(zipInputStream, result, buffer);
                    Logger.LogInformation($"Decompressing ZipStream[{name}]: Complete");
                }
                else
                    Logger.LogInformation($"No more entries in ZipStream");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error decompressing MemoryStream");
                throw ex;
            }
            return result;
        }
        protected MemoryStream[] UnzipAll(Stream data)
        {
            List<MemoryStream> result = new List<MemoryStream>();
            byte[] buffer = new byte[4096];
            if (data == null)
                throw new ArgumentNullException($"{nameof(data)}");
            try
            {
                MemoryStream ms = null;
                do
                {
                    ms = UnzipNext(data);
                    if (ms != null)
                        result.Add(ms);
                } while (ms != null);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error decompressing MemoryStream");
                throw ex;
            }
            return result?.ToArray();
        }
        protected Dictionary<string, string> DecompressZipEntriesText(byte[] bytes)
        {
            //Byte[] bytes = Convert.FromBase64String(text);
            Dictionary<string, string> result = new Dictionary<string, string>();
            MemoryStream inStream = null;
            ZipInputStream zStream = null;
            bool useFile = false;
            try
            {
                inStream = new MemoryStream(bytes);
                inStream.Position = 0;
                zStream = new ZipInputStream(inStream);
                ZipEntry entry = null;
                string entryKey = null;
                int idx = rnd.Next();
                int subIdx = -1;
                while (zStream != null && (entry = zStream.GetNextEntry()) != null)
                {
                    subIdx++;
                    entryKey = entry?.Name?.Replace(".json", "");
                    if (string.IsNullOrWhiteSpace(entryKey))
                        entryKey = $"{subIdx}";
                    result.Add(entryKey, null);
                    try
                    {
                        using (TextReader entryStream = new StreamReader(zStream))
                        {
                            if (!useFile)
                                result[entryKey] = entryStream.ReadToEnd();
                            else
                            {
                                using (FileStream fStream = File.OpenWrite(entryKey))
                                {
                                    //long zLen = zStream.Length;
                                    zStream.CopyTo(fStream);
                                };
                                Logger.LogDebug($"Successfully decompressed '{entryKey}'");
                                using (StreamReader txtStream = File.OpenText(entryKey))
                                {
                                    string newData = txtStream.ReadToEnd();
                                    result[entryKey] = newData;
                                };
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.LogWarning($"Failed decompressing entry '{entryKey}': {ex.Message}");
                        result.Remove(entryKey);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed decompressing response data: {ex.Message}");
            }
            finally
            {
                inStream?.Close();
                zStream?.Close();
            }
            return result;
        }
        protected void LogMessage(string identifier, string message, params object[] args)
        {
            var msg = string.Format(message, args);
            Logger.LogInformation($"{identifier}: {msg}");
        }

        #endregion
    }
}
