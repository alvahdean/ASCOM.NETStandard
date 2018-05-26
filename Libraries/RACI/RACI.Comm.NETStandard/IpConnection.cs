using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ASCOM.Havla
{
 
    abstract public class IpConnection : DeviceConnection
    {
        protected IPEndPoint RemoteEndPoint { get { return new IPEndPoint(Address, Port); } }

        public IpConnection() : this(ConnectionType.None)
        {

        }
        public IpConnection(ConnectionType type)
                    : this(type, "{ }") { }
        public IpConnection(ConnectionType type, string jsonSettings)
                    : this(type,JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonSettings)) { }
        public IpConnection(ConnectionType type, IDictionary<string, string> settings) : base(type, settings)
        {
            Address = null;
            Port = 0;
        }

        
        public IPAddress Address
        {
            get => IPAddress.Parse(Settings["Address"]);
            set
            {
                if (Address != value)
                {
                    if (IsConnected)
                        throw new AscomException($"Cannot change {nameof(Address)} to '{value}' while connected");
                    Settings["Address"] = value.ToString();
                    Logger?.LogMessage("IpConnection", $"Set Address = {Address}");
                }
            }
        }
        public string HostName
        {
            get => Dns.GetHostEntry(Address)?.HostName;
            set
            {
                if (HostName != value)
                {
                    if (IsConnected)
                        throw new AscomException($"Cannot change {nameof(Address)} to '{value}' while connected");
                    Settings["Address"] = value.ToString();
                    Logger?.LogMessage("IpConnection", $"Set Address = {Address}");
                }
            }
        }
        public int Port
        {
            get => Int32.Parse(Settings["Port"] ?? "0");
            set
            {
                if (Port != value)
                {
                    if (IsConnected)
                        throw new AscomException($"Cannot change {nameof(Port)} to '{value}' while connected");
                    Settings["Port"] = value.ToString();
                    Logger?.LogMessage("IpConnection", $"Set Port = {Port}");
                }
            }
        }
        public override bool IsConfigured
        {
            get
            {
                return Port > 0 && Address != null;
            }
        }
    }
    abstract public class IpStreamConnection : StreamConnection
    {

        public IpStreamConnection() : this(ConnectionType.None)
        {

        }
        public IpStreamConnection(ConnectionType type)
                    : this(type, "{ }") { }
        public IpStreamConnection(ConnectionType type, string jsonSettings)
                    : this(type, JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonSettings)) { }
        public IpStreamConnection(ConnectionType type, IDictionary<string, string> settings) : base(type, settings)
        {
            Address = null;
            Port = 0;
        }

        protected IPEndPoint RemoteEndPoint { get { return new IPEndPoint(Address, Port); } }

        public IPAddress Address
        {
            get => IPAddress.Parse(Settings["Address"]);
            set
            {
                if (Address != value)
                {
                    if (IsConnected)
                        throw new AscomException($"Cannot change {nameof(Address)} to '{value}' while connected");
                    Settings["Address"] = value.ToString();
                    Logger?.LogMessage("IpConnection", $"Set Address = {Address}");
                }
            }
        }
        public string HostName
        {
            get => Dns.GetHostEntry(Address)?.HostName;
            set
            {
                if (HostName != value)
                {
                    if (IsConnected)
                        throw new AscomException($"Cannot change {nameof(Address)} to '{value}' while connected");
                    Settings["Address"] = value.ToString();
                    Logger?.LogMessage("IpConnection", $"Set Address = {Address}");
                }
            }
        }
        public int Port
        {
            get => Int32.Parse(Settings["Port"] ?? "0");
            set
            {
                if (Port != value)
                {
                    if (IsConnected)
                        throw new AscomException($"Cannot change {nameof(Port)} to '{value}' while connected");
                    Settings["Port"] = value.ToString();
                    Logger?.LogMessage("IpConnection", $"Set Port = {Port}");
                }
            }
        }
        public override bool IsConfigured
        {
            get
            {
                return Port > 0 && Address != null;
            }
        }
    }

}
