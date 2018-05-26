using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace ASCOM
{
    /// <summary>
    /// All properties and methods defined by the relevant ASCOM standard interface must exist in each driver. However, those properties and methods do not all have to be implemented. This exception is a base class for PropertyNotImplementedException and MethodNotImplementedException, which drivers should use for throwing the relevant exception(s). This class is intended to be used by clients who wish to catch either of the two specific exceptions in a single catch() clause.
    /// </summary>
    //[Guid("46584278-AC16-4CFC-8878-09CA960AEABE")]
    //[ComVisible(true)]
    //[Serializable]
    public class NotImplementedException : DriverException
    {
        public NotImplementedException(string propertyOrMethod)
            : base(propertyOrMethod, null) { }

        public NotImplementedException(string propertyOrMethod, Exception inner)
            : base($"Property or method '{propertyOrMethod}' is not implemented", inner)
        {
            PropertyOrMethod = propertyOrMethod;
        }
        public NotImplementedException()
            :base() { }
        protected NotImplementedException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
        public string PropertyOrMethod { get; private set; }
    }
}