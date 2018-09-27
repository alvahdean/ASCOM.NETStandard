using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Extensions.Logging;
using RACI.Logging;
using System.Threading;
using RestSharp;

namespace RACI.Client
{
    public class ConnectionCheck
    {
        public const int MaxTimeout = 5000;
        public const int DefaultAttempts = 5;
        private ConnectionCheck() : this(5000) { }
        private ConnectionCheck(double msTimeout)
        {
            _respTimes = new List<double?>();
            _respMsgs = new List<string>();
            Timeout = Math.Max(1, Math.Min(msTimeout, MaxTimeout));
        }
        private void clear() => _respTimes.Clear();
        private List<double?> _respTimes;
        private List<string> _respMsgs;
        private IEnumerable<double?> failed =>
            _respTimes.Where(t => !t.HasValue || t.Value >= Timeout);
        private IEnumerable<double?> completed =>
            _respTimes.Where(t => t.HasValue);
        private IEnumerable<double> normalized =>
            _respTimes.Select(t => normalize(t));
        private double normalize(double? respTime) => respTime.HasValue
            ? Math.Max(0d, Math.Min(Timeout, respTime.Value))
            : Timeout;
        private void AddResponse(double? respTime,string msg=null)
        {
            if (respTime.HasValue && respTime.Value < 0)
                respTime = null;
            if(msg==null)
            {
                if ((respTime ?? -1) < 0) msg = "No response";
                else if (respTime >= Timeout) msg = "Timeout";
                else msg = "OK";
            }
            _respTimes.Add(respTime);
            _respMsgs.Add(msg);
        }
        public double Timeout { get; private set; }
        public string Url { get; private set; }
        public int Count => _respTimes.Count;
        public IEnumerable<double?> ResponseTimes => _respTimes.AsEnumerable();
        public int Successful => _respTimes.Where(t => t.HasValue && t.Value < Timeout).Count();
        public int Timeouts => _respTimes.Where(t => t.HasValue && t.Value >= Timeout).Count();
        public int Invalid => _respTimes.Where(t => !t.HasValue).Count();
        public double? AvgResponseTime => completed.Average();
        public double Quality => AvgResponseTime.HasValue ? Math.Max(Timeout - AvgResponseTime.Value, 0d) / Timeout : 0d;
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"------------------------------------------");
            sb.AppendLine($"Target: {Url}");
            sb.AppendLine($"Timeout: {Timeout:f0}ms");
            sb.AppendLine($"Average Response Time: {AvgResponseTime}");
            sb.AppendLine($"Quality: {Quality * 100:f2}%");
            sb.AppendLine($"\tAttempts: {Count}");
            sb.AppendLine($"\tSuccessful: {Successful}");
            sb.AppendLine($"\tTimeout: {Timeouts}");
            sb.AppendLine($"\tInvalid: {Invalid}");
            sb.AppendLine($"Responses:");
            int i = 1;
            foreach (double? r in ResponseTimes)
            {
                if(r.HasValue)
                    sb.AppendLine($"\t[{i}]: {r.Value}ms ({_respMsgs[i-1]})");
                else
                    sb.AppendLine($"\t[{i}]: [null] ({_respMsgs[i - 1]})");
                i++;
            }
            sb.AppendLine($"------------------------------------------");
            return sb.ToString();
        }
        public static ConnectionCheck Run(string url, int count = DefaultAttempts, int timeout = MaxTimeout, ILogger logger = null) =>
            Run(RaciClient.Create(url), count, timeout, logger);
        public static ConnectionCheck Run(Uri uri, int count = DefaultAttempts, int timeout = MaxTimeout, ILogger logger = null) =>
            Run(RaciClient.Create(uri), count, timeout, logger);
        public static ConnectionCheck Run(RaciClient client, int count = DefaultAttempts, int timeout = MaxTimeout,ILogger logger=null)
        {
            if(logger==null)
                logger = RaciLog.DefaultLogger;
            ConnectionCheck result = new ConnectionCheck(timeout);
            result.Url = client?.BaseUrl?.AbsoluteUri;
            if (count <= 0) count = DefaultAttempts;

            logger.LogInformation($"RACI Client connection test starting (timeout={result.Timeout:f2}ms");

            for (int i = 0; i < count; i++)
            {
                double? respTime = null;
                string respMsg = null;
                if (client != null || client.BaseUrl != null)
                {
                    try
                    {
                        IRestRequest request = client.CreateRequest<bool>(Method.GET, "service/ping");
                        DateTime ti = DateTime.Now;
                        client.Timeout = (int)result.Timeout;
                        IRestResponse<bool> resp=null;
                        resp=client.Execute<bool>(request);

                        if (resp != null)
                        {
                            if (resp.ResponseStatus == ResponseStatus.TimedOut)
                            {
                                respTime = (DateTime.Now - ti).TotalMilliseconds;
                                respMsg = "Timeout";
                            }
                            else if (resp.ResponseStatus != ResponseStatus.Completed)
                            {
                                respTime = null;
                                respMsg = resp.ResponseStatus.ToString();
                                if (resp.ErrorException != null)
                                    respMsg = $"{respMsg}: {resp.ErrorException.Message}";
                            }
                            else if (resp.StatusCode != System.Net.HttpStatusCode.OK)
                            {
                                respTime = null;
                                respMsg = resp.StatusDescription.ToString();
                            }
                            else
                            {
                                respTime = (DateTime.Now - ti).TotalMilliseconds;
                                respMsg = "OK";
                            }
                        }
                        else
                        {
                            respTime = null;
                            respMsg = "No response";
                        }
                    }
                    catch (Exception ex)
                    {
                        respMsg = ex.Message;
                        respTime = null;
                    }
                }
                result.AddResponse(respTime,respMsg);

            }
            logger.LogInformation($"Connection test results: {result.Successful}/{result.Count} OK, Avg Time: {result.AvgResponseTime??Double.NaN}ms, Quality: {result.Quality*100:f0}%");
            return result;
        }
    }
}
