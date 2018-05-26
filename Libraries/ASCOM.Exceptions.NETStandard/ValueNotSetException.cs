using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace ASCOM
{
    /// <summary>
    /// Exception to report that no value has yet been set for this property.
    /// </summary>
    //[ComVisible(true)]
    //[Guid("6B8F457E-29D6-463D-841B-85C85A8E6A1F")]
    //[Serializable]
    public class ValueNotSetException : DriverException
    {
        public ValueNotSetException()
            :base() { }
        public ValueNotSetException(string propertyOrMethod)
            : this(propertyOrMethod, null) { }

        public ValueNotSetException(string propertyOrMethod, Exception inner)
            : base($"Value not set: '{propertyOrMethod}'", inner)
        {
            PropertyOrMethod = propertyOrMethod;
        }

        protected ValueNotSetException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
        public string PropertyOrMethod { get; }
    }
}