using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace ASCOM
{
    /// <summary>
    /// All properties defined by the relevant ASCOM standard interface must exist in each driver. However, those properties do not all have to be implemented. The minimum requirement for each defined property is to throw the ASCOM.PropertyNotImplementedException for each of its accessors. Note that no default constructor is supplied. Throwing this requires both the property name and unimplemented accessor type to be supplied.
    ///
    /// </summary>
    //[Guid("EA016028-4929-4962-B768-3A4F33FC36A8")]
    //[ComVisible(true)]
    //[Serializable]
    public class PropertyNotImplementedException : NotImplementedException
    {
        public PropertyNotImplementedException(string property, bool accessorSet)
            : this(property, accessorSet, null) { }

        public PropertyNotImplementedException(string property, bool accessorSet, Exception inner)
            : base($"Property not implemented: '{property}'", null)
        {
            Property = property;
            AccessorSet = accessorSet;
        }

        public PropertyNotImplementedException(string message, Exception inner)
            : base(message, inner) { }
        public PropertyNotImplementedException()
            : base() { }
        public PropertyNotImplementedException(string message)
            : base(message) { }
        protected PropertyNotImplementedException(SerializationInfo info, StreamingContext context)
            : base(info,context) { }
        public string Property { get; }
        public bool AccessorSet { get; }
    }
}