using System;
using System.Collections.Generic;
using System.Text;

namespace RACI.Data
{
    public class ValidationException: Exception
    {
        public ValidationException() : this("An unspecified validation error occured") { }
        public ValidationException(string msg) : base(msg) { }
        public ValidationException(string propName,string msg) : this($"[{(propName??"").Trim()}]: {msg}")
        {
            PropertyName = (propName ?? "").Trim();
        }
        public String PropertyName { get; protected set; }
    }
}
