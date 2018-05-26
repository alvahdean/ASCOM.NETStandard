using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace ASCOM
{
    public class InvalidOperationException : DriverException
    {
        public InvalidOperationException()
            : this(nameof(InvalidOperationException)) { }

        public InvalidOperationException(Exception innerException)
            : this(innerException?.Message, innerException) { }
        public InvalidOperationException(string message)
            : this(message, null) { }
        public InvalidOperationException(string message, Exception innerException)
            : base(message, innerException) { }
        protected InvalidOperationException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}