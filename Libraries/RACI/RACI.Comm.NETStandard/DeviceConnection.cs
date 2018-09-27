using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;

using ASCOM;
using ASCOM.Utilities;
using ASCOM.Utilities.Interfaces;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ASCOM.Havla
{
    abstract public class DeviceConnection : IDeviceConnection
    {
        public DeviceConnection() : this(ConnectionType.None) { }
        //TODO: Fix Nuget to provide IConfigurationBuilder.AddConfiguration
        public DeviceConnection(ConnectionType connType,IConfiguration config=null)
        {
            ConnectionType = connType;
            IConfigurationBuilder builder = new ConfigurationBuilder().AddInMemoryCollection();
            Configuration=builder.Build();
            if (config != null)
            {
                foreach (var item in config.AsEnumerable())
                    Configuration[item.Key] = item.Value;
            }

        }

        public ConnectionType ConnectionType { get; private set; }
        public IConfiguration Configuration { get; private set; }
        public ITraceLogger Logger { get; set; }
        public bool Connect()
        {
            if (!IsConnected)
                IsConnected = true;
            return IsConnected;
        }
        public bool Disconnect()
        {
            if (IsConnected)
                IsConnected = false;
            return !IsConnected;
        }
        public virtual bool IsConfigured { get { return ConnectionType == ConnectionType.None; } }

        public abstract bool IsConnected { get; set; }

        public abstract void Flush();

        public abstract int Read(byte[] buffer, int offset, int count);

        public abstract int ReadByte();

        public abstract void Write(byte[] buffer, int offset, int count);

        public abstract void WriteByte(byte value);
    }
}
