using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ASCOM.Havla
{
    public interface IDeviceConnection
    {
        ConnectionType ConnectionType { get; }
        bool IsConfigured { get; }
        bool IsConnected { get; set; }
        IConfiguration Configuration { get; }
        bool Connect();
        bool Disconnect();

        //
        // Summary:
        //     When overridden in a derived class, clears all buffers for this stream and causes
        //     any buffered data to be written to the underlying device.
        //
        // Exceptions:
        //   T:System.IO.IOException:
        //     An I/O error occurs.
        void Flush();

        //
        // Summary:
        //     When overridden in a derived class, reads a sequence of bytes from the current
        //     stream and advances the position within the stream by the number of bytes read.
        //
        // Parameters:
        //   buffer:
        //     An array of bytes. When this method returns, the buffer contains the specified
        //     byte array with the values between offset and (offset + count - 1) replaced by
        //     the bytes read from the current source.
        //
        //   offset:
        //     The zero-based byte offset in buffer at which to begin storing the data read
        //     from the current stream.
        //
        //   count:
        //     The maximum number of bytes to be read from the current stream.
        //
        // Returns:
        //     The total number of bytes read into the buffer. This can be less than the number
        //     of bytes requested if that many bytes are not currently available, or zero (0)
        //     if the end of the stream has been reached.
        //
        // Exceptions:
        //   T:System.ArgumentException:
        //     The sum of offset and count is larger than the buffer length.
        //
        //   T:System.ArgumentNullException:
        //     buffer is null.
        //
        //   T:System.ArgumentOutOfRangeException:
        //     offset or count is negative.
        //
        //   T:System.IO.IOException:
        //     An I/O error occurs.
        //
        //   T:System.NotSupportedException:
        //     The stream does not support reading.
        //
        //   T:System.ObjectDisposedException:
        //     Methods were called after the stream was closed.
        int Read(byte[] buffer, int offset, int count);

        //
        // Summary:
        //     Reads a byte from the stream and advances the position within the stream by one
        //     byte, or returns -1 if at the end of the stream.
        //
        // Returns:
        //     The unsigned byte cast to an Int32, or -1 if at the end of the stream.
        //
        // Exceptions:
        //   T:System.NotSupportedException:
        //     The stream does not support reading.
        //
        //   T:System.ObjectDisposedException:
        //     Methods were called after the stream was closed.
        int ReadByte();

        //
        // Summary:
        //     When overridden in a derived class, writes a sequence of bytes to the current
        //     stream and advances the current position within this stream by the number of
        //     bytes written.
        //
        // Parameters:
        //   buffer:
        //     An array of bytes. This method copies count bytes from buffer to the current
        //     stream.
        //
        //   offset:
        //     The zero-based byte offset in buffer at which to begin copying bytes to the current
        //     stream.
        //
        //   count:
        //     The number of bytes to be written to the current stream.
        //
        // Exceptions:
        //   T:System.ArgumentException:
        //     The sum of offset and count is greater than the buffer length.
        //
        //   T:System.ArgumentNullException:
        //     buffer is null.
        //
        //   T:System.ArgumentOutOfRangeException:
        //     offset or count is negative.
        //
        //   T:System.IO.IOException:
        //     An I/O error occured, such as the specified file cannot be found.
        //
        //   T:System.NotSupportedException:
        //     The stream does not support writing.
        //
        //   T:System.ObjectDisposedException:
        //     System.IO.Stream.Write(System.Byte[],System.Int32,System.Int32) was called after
        //     the stream was closed.
        void Write(byte[] buffer, int offset, int count);

        //
        // Summary:
        //     Writes a byte to the current position in the stream and advances the position
        //     within the stream by one byte.
        //
        // Parameters:
        //   value:
        //     The byte to write to the stream.
        //
        // Exceptions:
        //   T:System.IO.IOException:
        //     An I/O error occurs.
        //
        //   T:System.NotSupportedException:
        //     The stream does not support writing, or the stream is already closed.
        //
        //   T:System.ObjectDisposedException:
        //     Methods were called after the stream was closed.
        void WriteByte(byte value);
    }
    public interface IStreamConnection: IDeviceConnection
    {
        Stream Stream { get; }
    }
}

