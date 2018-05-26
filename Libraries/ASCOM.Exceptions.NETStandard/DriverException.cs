using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace ASCOM
{

    /// <summary>
    /// This is the generic driver exception. Drivers are permitted to directly throw this 
    /// exception as well as any derived exceptions. Note that the Message property is a 
    /// member of System.Exception, the base class of DriverException. 
    /// The System.Exception.HResult property of System.Exception is simply renamed to Number.
    /// 
    /// This exception should only be thrown if there is no other more appropriate exception 
    /// already defined, e.g.PropertyNotImplemented, InvalidOperationException, InvalidValueException, 
    /// NotConnectedException etc.These specific exceptions should be thrown where appropriate 
    /// rather than using the more generic DriverException.Conform will not accept DriverExceptions 
    /// where more appropriate exceptions are already defined.
    /// 
    /// As good programming practice, the Message property should not be empty, so that users 
    /// understand why the exception was thrown.
    /// </summary>
    //[ComVisible(true)]
    //[Guid("B6EE3D18-CF56-42D3-AED5-B97ABF36B4EE")]
    //[Serializable]
    public class DriverException : AscomException
    {
        public DriverException()
        :this(null,0){ }
        public DriverException(string message, int number)
            : this(null, number,null) { }
        public DriverException(string message, int number, Exception inner)
            : base(message,inner)
        {
            Number = number;
        }
        public DriverException(string message)
            : this(message,0,null) { }  //TODO: Check default number, maybe should be 1 (ErrorCodes.UnspecifiedError)?
        public DriverException(string message, Exception innerException)
            : base(message,innerException) { }
        protected DriverException(SerializationInfo info, StreamingContext context) 
            : base(info,context) { }
        public int Number { get; private set; }
    }
}