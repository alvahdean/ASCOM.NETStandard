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
    abstract public class StreamConnection : DeviceConnection, IStreamConnection
    {
        public StreamConnection() : this(ConnectionType.None) { }
        public StreamConnection(ConnectionType connType) : this(connType, null) { }
        public StreamConnection(ConnectionType connType, IConfiguration settings)
            : base(connType, settings) { }

        public abstract Stream Stream { get; }

        public override void Flush()
        {
            if (!IsConnected)
                throw new NotConnectedException();
            Stream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (!IsConnected)
                throw new NotConnectedException();
            return Stream.Read(buffer, offset, count);
        }

        public override int ReadByte()
        {
            if (!IsConnected)
                throw new NotConnectedException();
            return Stream.ReadByte();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (!IsConnected)
                throw new NotConnectedException();
            Stream.Write(buffer, offset, count);
        }

        public override void WriteByte(byte value)
        {
            if (!IsConnected)
                throw new NotConnectedException();
            Stream.WriteByte(value);
        }
    }
}
