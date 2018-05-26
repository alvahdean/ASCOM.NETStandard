using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace ASCOM
{
    /// <summary>
    /// This exception should be raised when an operation is attempted that requires communication with the device, but the device is disconnected.
    /// </summary>
    //[Guid("C22D9A81-63FA-4AC3-AD64-77B77D3CEB2B")]
    //[ComVisible(true)]
    //[Serializable]
    public class NotConnectedException : DriverException
    {
        public NotConnectedException()
            :base() { }
        public NotConnectedException(Exception innerException)
            : this(innerException?.Message,innerException) { }
        public NotConnectedException(string message)
            : this(message,null) { }
        public NotConnectedException(string message, Exception innerException)
            : base(message,innerException) { }
        protected NotConnectedException(SerializationInfo info, StreamingContext context)
            : base(info,context) { }
    }
}