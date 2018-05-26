using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ASCOM.Havla
{
 
    public class UdpConnection : IpConnection
    {
        private UdpClient conn;
        public UdpConnection() : this("{ }") { }
        public UdpConnection(string jsonSettings) : this(JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonSettings)) { }
        public UdpConnection(IDictionary<string, string> settings) : base(ConnectionType.Tcp, settings)
        {
            conn = null;
            throw new NotImplementedException("UdpConnection not implemented");
        }
        
        public override bool IsConnected
        {
            get => conn?.Client?.Connected ?? false;
            set
            {
                string msg = "";
                if (IsConnected != value)
                {
                    if (value && !IsConfigured)
                    {
                        msg = $"Cannot connect: Not configured";
                        Logger?.LogMessage("UdpConnection", msg);
                        throw new NotConnectedException(msg);
                    }
                    if (value)
                    {
                        Logger?.LogMessage("UdpConnection", $"Connecting {Address}:{Port}");
                        conn = new UdpClient();
                        conn.Connect(Address, Port);
                    }
                    else
                    {
                        Logger?.LogMessage("UdpConnection", $"Disconnecting {Address}:{Port}");
                        if (conn != null)
                        {
                            if (IsConnected)
                            {
                                conn.Close();
                            }
                            conn.Dispose();
                        }
                        conn = null;
                    }
                    Logger?.LogMessage("UdpConnection", $"{Address}:{Port}: Connected={IsConnected}");
                }
            }
        }
        
        public override void Flush()
        {
            if (!IsConnected)
                throw new NotConnectedException();
            throw new NotImplementedException("UdpConnection not implemented");
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (!IsConnected)
                throw new NotConnectedException();
            throw new NotImplementedException("UdpConnection not implemented");
        }

        public override int ReadByte()
        {
            if (!IsConnected)
                throw new NotConnectedException();
            throw new NotImplementedException("UdpConnection not implemented");
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (!IsConnected)
                throw new NotConnectedException();
            throw new NotImplementedException("UdpConnection not implemented");
        }

        public override void WriteByte(byte value)
        {
            if (!IsConnected)
                throw new NotConnectedException();
            conn.Send(new byte[] { value }, 1);
        }

    }
}
