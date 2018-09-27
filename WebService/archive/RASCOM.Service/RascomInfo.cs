using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Reflection;


namespace RACI.ASCOM.Service
{
    public static class RascomInfo
    {
        private static AssemblyName _aName;
        static RascomInfo()
        {
            _aName = Assembly.GetExecutingAssembly().GetName();
            SysName = Dns.GetHostName();
            Port = 80;
            Version = _aName.Version.ToString(4);
            RootUrl = "/rascom";
        }
        public static string SysName { get; }
        public static uint Port { get; }
        public static string Version { get; }
        public static string RootUrl { get; }
        //public static IEnumerable<String> DeviceTypes { get { return _devList.Keys.OrderBy(t => t); } }
        //public static IEnumerable<String> Devices(String devType)
        //{
        //    devType = devType.ToUpperInvariant();
        //    foreach (var dtype in _devList.Keys)
        //    {
        //        if (devType==dtype.ToUpperInvariant())
        //            return _devList[dtype].OrderBy(t=>t);
        //    }
        //    return null;
        //}
    }
}
