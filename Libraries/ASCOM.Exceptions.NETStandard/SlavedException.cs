using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace ASCOM
{
    /// <summary>
    /// This exception should be used to indicate that movement (or other invalid operation) 
    /// was attempted while the device was in slaved mode. 
    /// This applies primarily to domes drivers.
    /// </summary>
    //[Guid("537BF13D-55E0-4C80-98EB-BE270E653E10")]
    //[ComVisible(true)]
    //[Serializable]
    public class SlavedException : DriverException
    {
        public SlavedException()
            : base() { }
        public SlavedException(Exception inner)
            : this(string.Empty,inner) { }
        public SlavedException(string message)
            : this(message,null) { }
        public SlavedException(string message, Exception inner)
            : base(message,inner) { }

        protected SlavedException(SerializationInfo info, StreamingContext context)
            : base(info,context) { }
    }
}