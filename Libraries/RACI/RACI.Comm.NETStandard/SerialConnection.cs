using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using RJCP.IO.Ports;
using ASCOM;
using ASCOM.Utilities;
using ASCOM.Utilities.Interfaces;

namespace ASCOM.Havla
{
    public class SerialConnection : StreamConnection
    {
        private SerialPortStream conn;

        protected TEnum ConvertEnum<TEnum>(int value)
            where TEnum: struct
        {
            if (!typeof(TEnum).IsEnum)
                throw new InvalidCastException($"TEnum must be an Enumueration");
            return (TEnum)Enum.ToObject(typeof(TEnum),value);
        }
        protected TEnum ConvertEnum<TEnum>(string value)
            where TEnum : struct
        {
            if (!typeof(TEnum).IsEnum)
                throw new InvalidCastException($"TEnum must be an Enumueration");
            return (TEnum)Enum.Parse(typeof(TEnum), value);
        }

        protected bool IsPortValid(string port=null)
        {
            if (String.IsNullOrWhiteSpace(port))
                port = PortName;
            return SerialPortStream.GetPortNames().Any(t => t.Equals(port, StringComparison.InvariantCultureIgnoreCase));
        }

        public SerialConnection() : this("{ }") { }
        public SerialConnection(string jsonSettings) 
            : this(JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonSettings)) { }
        public SerialConnection(IDictionary<string, string> settings) : base(ConnectionType.Serial, settings)
        {
            conn = null;
        }

        public override Stream Stream => IsConnected ? conn : null;
        public SerialHandshake Handshake
        {
            get => ConvertEnum<SerialHandshake>(Settings["Handshake"]??SerialHandshake.None.ToString());
            set
            {
                if (Handshake != value)
                {
                    if (IsConnected)
                        throw new AscomException($"Cannot change {nameof(Handshake)} to '{value}' while connected");
                    Settings["Handshake"] = value.ToString();
                    Logger?.LogMessage("SerialConnection", $"Set Handshake = {Handshake}");
                }
            }
        }
        public SerialParity Parity
        {
            get => ConvertEnum<SerialParity>(Settings["Parity"]??SerialParity.None.ToString());
            set
            {
                if (Parity != value)
                {
                    if (IsConnected)
                        throw new AscomException($"Cannot change {nameof(Parity)} to '{value}' while connected");
                    Settings["Parity"] = value.ToString();
                    Logger?.LogMessage("SerialConnection", $"Set Parity = {Parity}");
                }
            }
        }
        public SerialSpeed Speed
        {
            get => ConvertEnum<SerialSpeed>(Settings["Speed"]??"9600");
            set
            {
                if (Speed != value)
                {
                    if (IsConnected)
                        throw new AscomException($"Cannot change {nameof(Speed)} to '{value}' while connected");
                    Settings["Speed"] = value.ToString();
                    Logger?.LogMessage("SerialConnection", $"Set Speed = {Speed}({(int)Speed})");
                }
            }
        }
        public SerialStopBits StopBits
        {
            get => ConvertEnum<SerialStopBits>(Settings["StopBits"] ?? SerialStopBits.One.ToString());
            set
            {
                if (StopBits != value)
                {
                    if (IsConnected)
                        throw new AscomException($"Cannot change {nameof(StopBits)} to '{value}' while connected");
                    Settings["StopBits"] = value.ToString();
                    Logger?.LogMessage("SerialConnection", $"Set StopBits = {StopBits}");
                }
            }
        }
        public int DataBits
        {
            get => Int32.Parse(Settings["DataBits"] ?? "8");
            set
            {
                if (DataBits != value)
                {
                    if (IsConnected)
                        throw new AscomException($"Cannot change {nameof(DataBits)} to '{value}' while connected");
                    Settings["DataBits"] = value.ToString();
                    Logger?.LogMessage("SerialConnection", $"Set DataBits = {DataBits}");
                }
            }
        }
        public string PortName
        {
            get => Settings["PortName"];
            set
            {
                value = value.Trim();

                if (!IsPortValid(value))
                    throw new AscomException($"Port '{value}' not found on system");

                if (!PortName.Equals(value,StringComparison.InvariantCultureIgnoreCase))
                {
                    if (IsConnected)
                        throw new AscomException($"Cannot change {nameof(PortName)} to '{value}' while connected");
                    Settings["PortName"] = value;
                    Logger?.LogMessage("SerialConnection", $"Set PortName = {PortName}");
                }
            }
        }
        public override bool IsConfigured
        {
            get
            {
                return IsPortValid();
            }
        }
        public override bool IsConnected
        {
            get=>conn?.IsOpen??false;
            set
            {
                string msg = "";
                if (IsConnected != value)
                {
                    if (value && !IsConfigured)
                    {
                        msg = $"Cannot connect: Not configured";
                        Logger?.LogMessage("SerialConnection", msg);
                        throw new NotConnectedException(msg);
                    }
                    if(value)
                    {
                        Logger?.LogMessage("SerialConnection", $"Connecting {PortName}:{DataBits},{Parity},{StopBits}");
                        Parity parity = ConvertEnum<Parity>(Parity.ToString());
                        StopBits stopBits = ConvertEnum<StopBits>(StopBits.ToString());
                        conn = new SerialPortStream(PortName,(int)Speed,DataBits, parity, stopBits);
                        conn.Open();
                    }
                    else
                    {
                        Logger?.LogMessage("SerialConnection", $"Disconnecting {PortName}");
                        if (conn != null)
                        {
                            if (conn.IsOpen)
                            {
                                conn.Flush();
                                conn.Close();
                            }
                            conn.Dispose();
                        }
                        conn = null;
                    }
                    Logger?.LogMessage("SerialConnection", $"{PortName}: Connected={IsConnected}");
                }
            }
        }
    }
}
