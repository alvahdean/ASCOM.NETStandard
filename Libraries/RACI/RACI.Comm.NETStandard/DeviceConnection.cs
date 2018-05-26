using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
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
        public DeviceConnection(ConnectionType connType)
        {
            ConnectionType = connType;
            Settings = new ConfigurationBuilder()
                .AddInMemoryCollection()
                .Build();
        }
        public DeviceConnection(ConnectionType connType, string jsonSettings)
            : this(connType, JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonSettings)) { }
        public DeviceConnection(ConnectionType connType, IDictionary<string, string> settings)
            : this(connType)
        {
            if (settings != null)
            {
                foreach (string key in settings.Keys)
                    Settings[key] = settings[key];
            }
        }

        public ConnectionType ConnectionType { get; private set; }
        public IConfiguration Settings { get; private set; }
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
