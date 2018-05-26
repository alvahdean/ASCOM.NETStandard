using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace ASCOM
{
    /// <summary>
    /// All methods defined by the relevant ASCOM standard interface must exist in each driver. 
    /// However, those methods do not all have to be implemented. The minimum requirement for 
    /// each defined method is to throw the ASCOM.MethodNotImplementedException. 
    /// Note that no default constructor is supplied. Throwing this requires the the method name.
    /// </summary>
    //[Guid("BBED286E-5814-4467-9471-A499DED13452")]
    //[ComVisible(true)]
    //[Serializable]
    public class MethodNotImplementedException : NotImplementedException
    {
        //No default constructor? See summary notes...
        //public MethodNotImplementedException()
        //    : base() { }
        public MethodNotImplementedException(string method)
            : this(method, null) { }
        public MethodNotImplementedException(string method, Exception inner)
            : base($"Method '{method?.Trim()}' not implemented",inner)
        {
            Method = method?.Trim();
        }

        protected MethodNotImplementedException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }

        public string Method { get; } = String.Empty;
    }
}