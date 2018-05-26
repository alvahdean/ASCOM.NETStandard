using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace ASCOM
{
    /// <summary>
    /// Exception thrown by a driver when it receives an unknown command through the Action method.
    /// </summary>
    //[Guid("6D6475A7-A6E0-4983-A4A8-EF7A8BCFFF1E")]
    //[ComVisible(true)]
    //[Serializable]
    public class ActionNotImplementedException : NotImplementedException
    {
        public ActionNotImplementedException()
            : base() { }

        public ActionNotImplementedException(string Action)
            : base(Action, null) { }

        public ActionNotImplementedException(string Action, Exception inner)
            : base(Action, inner)
        {
            this.Action = Action;
        }

        protected ActionNotImplementedException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
        public string Action { get; }
    }
}
