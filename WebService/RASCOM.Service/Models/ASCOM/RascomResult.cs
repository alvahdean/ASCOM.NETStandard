using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASCOM;
using ASCOM.Utilities.Interfaces;
using Microsoft.AspNetCore.Mvc;
using RACI.ASCOM;

namespace RACI.ASCOM.Service.Models
{

    public class RascomResult : IRascomResult
    {

        public bool Success { get; set; } = true;
        public String Message { get; set; } = "";
        public Exception Exception { get; set; } = null;
        public ErrorCode ErrorCode { get; set; } = ErrorCode.NoError;
        public object Data { get; set; } = null;
        public RascomResult() : this(null, true, "Success") { }
        public RascomResult(object data)
            : this(data ?? new object(), data != null, data == null ? "No data" : "") { }
        public RascomResult(object data,bool success, String msg = null)
        {
            Data = data;
            Success = success;
            if (!Success)
            {
                Message = $"Error: {msg ?? "No error details"}";
                ErrorCode = ErrorCode.UnspecifiedError;
            }
            Message = msg ?? "";
        }
        public RascomResult(Exception ex, object data = null) : this(data)
        {
            Success = false;
            Exception = ex ?? new AscomException("No details");
            Type exType = ex.GetType();
            Message = $"{exType.Name}: {ex.Message}";
            ErrorCode = ErrorCode.UnspecifiedError;
        }
        public RascomResult(AscomException ex, object data = null) : this(data)
        {
            Success = false;
            Exception = ex ?? new AscomException("No details");
            Type exType = ex.GetType();
            Message = $"{exType.Name}: {ex.Message}";
            ErrorCode = ErrorCode.UnspecifiedError;
        }

        public static implicit operator RascomResult(AscomException ex)
        {
            return new RascomResult(ex,null);
        }
    }
}
