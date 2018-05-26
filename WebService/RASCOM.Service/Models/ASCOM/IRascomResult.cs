using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASCOM;
using ASCOM.Utilities.Interfaces;
using ASCOM.Utilities.Exceptions;

namespace RACI.ASCOM
{

    public interface IRascomResult 
    {
        bool Success { get; set; }
        String Message { get; set; }
    }

    public interface IRascomResult<TEx> : IRascomResult
        where TEx : Exception,IAscomException
    {
        ErrorCode ErrorCode { get; set; }
        TEx Exception { get; set; }
    }

}
