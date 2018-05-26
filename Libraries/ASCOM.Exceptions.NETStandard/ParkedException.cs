using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace ASCOM
{
    /// <summary>
    /// This exception should be used to indicate that movement (or other invalid operation) was attempted while the device was in a parked state.
    /// </summary>
    //[ComVisible(true)]
    //[Guid("89EA7E2A-7C74-461C-ABD5-75EE3D46DA13")]
    //[Serializable]
    public class ParkedException : DriverException
    {
        public ParkedException()
            : base() { }
        public ParkedException(Exception inner)
            : this("Cannot move while parked",inner) { }
        public ParkedException(string message)
            : this(message, null) { }
        public ParkedException(string message, Exception inner)
            : base(message, inner) { }
        protected ParkedException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}