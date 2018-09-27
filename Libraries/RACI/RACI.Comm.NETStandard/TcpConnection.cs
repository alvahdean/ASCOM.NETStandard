using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using Microsoft.Extensions.Configuration;

namespace ASCOM.Havla
{

    public class TcpConnection : IpStreamConnection
    {
        private TcpClient conn;

        public TcpConnection() : this(null) { }
        public TcpConnection(IConfiguration settings) : base(ConnectionType.Tcp, settings)
        {
            conn = null;
        }

        public override bool IsConfigured
        {
            get
            {
                return Port>0 && Address!=null;
            }
        }
        public override Stream Stream => IsConnected ? conn.GetStream() : null;
        public override bool IsConnected
        {
            get => conn?.Connected ?? false;
            set
            {
                string msg = "";
                if (IsConnected != value)
                {
                    if (value && !IsConfigured)
                    {
                        msg = $"Cannot connect: Not configured";
                        Logger?.LogMessage("TcpConnection", msg);
                        throw new NotConnectedException(msg);
                    }
                    if (value)
                    {
                        Logger?.LogMessage("TcpConnection", $"Connecting {Address}:{Port}");
                        conn = new TcpClient();
                        conn.Connect(Address,Port);
                    }
                    else
                    {
                        Logger?.LogMessage("TcpConnection", $"Disconnecting {Address}:{Port}");
                        if (conn != null)
                        {
                            if (conn.Connected)
                            {
                                conn.Close();
                            }
                            conn.Dispose();
                        }
                        conn = null;
                    }
                    Logger?.LogMessage("TcpConnection", $"{Address}:{Port}: Connected={IsConnected}");
                }
            }
        }
    }
}
