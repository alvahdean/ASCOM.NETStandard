using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using ASCOM.Utilities.Interfaces;

namespace ASCOM
{

    /// <summary>
    /// Exception thrown by DriverAccess to return a driver COM error to the client. 
    /// This exception appears as a COMException to the client having the original 
    /// exception's description and error number as well as the original exception 
    /// as the inner exception.
    /// </summary>
    //[ComVisible(true)]
    //[Guid("06CC64FC-3833-48D5-BC54-82DF40CA3900")]
    //[Serializable]
    public class DriverAccessCOMException : COMException, IAscomException
    {
        public DriverAccessCOMException(string Message, int ErrorCode, Exception InnerException)
            : base(Message, InnerException)
        {
            //this.ErrorCode = ErrorCode;
        }

        public string TargetSiteName { get { return TargetSite?.Name??""; } }
    }
}
