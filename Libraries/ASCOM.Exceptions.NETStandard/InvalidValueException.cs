using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace ASCOM
{
    //[ComVisible(true)]
    //[Guid("939B5C76-A502-4729-8786-0C1600445EFE")]
    //[Serializable]
    public class InvalidValueException : DriverException
    {
        public InvalidValueException()
            : base() { }
        public InvalidValueException(string propertyOrMethod, string value, string range)
            : this(propertyOrMethod, value, range,null) { }

        public InvalidValueException(string propertyOrMethod, string value, string range, Exception inner)
            : this($"Invalid value for {propertyOrMethod} '{value}' {range??string.Empty}",inner)
        {
            PropertyOrMethod = propertyOrMethod;
            Value = value;
            Range = range;
        }
        public InvalidValueException(string message)
            : this(message,null) { }
        public InvalidValueException(string message, Exception inner)
            : base(message, inner) { }
        protected InvalidValueException(SerializationInfo info, StreamingContext context)
            :base(info,context) { }

        public string PropertyOrMethod { get; private set; }
        public string Value { get; private set; }
        public string Range { get; private set; }
    }
}