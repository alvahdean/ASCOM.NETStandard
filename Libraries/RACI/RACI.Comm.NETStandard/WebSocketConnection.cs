using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ASCOM.Havla
{
    public class WebSocketConnection : IpConnection
    {
        WebSocket conn;
        public WebSocketConnection() : this("{ }")
        {

        }

        public WebSocketConnection(string jsonSettings)
                    : this(JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonSettings)) { }

        public WebSocketConnection(IDictionary<string, string> settings) : base(ConnectionType.Tcp, settings)
        {

            conn = null;
            throw new NotImplementedException("WebSocketConnection not implemented");
        }

        public override bool IsConnected
        {
            get => conn!=null && conn.State==WebSocketState.Open;
            set
            {
                string msg = "";
                if (IsConnected != value)
                {
                    if (value && !IsConfigured)
                    {
                        msg = $"Cannot connect: Not configured";
                        Logger?.LogMessage("WebSocketConnection", msg);
                        throw new NotConnectedException(msg);
                    }
                    if (value)
                    {
                        Logger?.LogMessage("WebSocketConnection", $"Connecting {Address}:{Port}");
                        throw new NotImplementedException("WebSocketConnection not implemented");
                    }
                    else
                    {
                        Logger?.LogMessage("WebSocketConnection", $"Disconnecting {Address}:{Port}");
                        if (conn != null)
                        {
                            if (IsConnected)
                            {
                                conn.Abort();
                            }
                            conn.Dispose();
                        }
                        conn = null;
                    }
                    Logger?.LogMessage("WebSocketConnection", $"{Address}:{Port}: Connected={IsConnected}");
                }
            }
        }

        public override void Flush()
        {
            if (!IsConnected)
                throw new NotConnectedException();
            throw new NotImplementedException("WebSocketConnection not implemented");
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (!IsConnected)
                throw new NotConnectedException();
            throw new NotImplementedException("WebSocketConnection not implemented");
        }

        public override int ReadByte()
        {
            if (!IsConnected)
                throw new NotConnectedException();
            throw new NotImplementedException("WebSocketConnection not implemented");
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (!IsConnected)
                throw new NotConnectedException();
            throw new NotImplementedException("WebSocketConnection not implemented");
        }

        public override void WriteByte(byte value)
        {
            if (!IsConnected)
                throw new NotConnectedException();
            throw new NotImplementedException("WebSocketConnection not implemented");
        }
    }
}
