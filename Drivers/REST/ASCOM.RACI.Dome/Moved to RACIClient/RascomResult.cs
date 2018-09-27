using System;

namespace RACI.Data.RestObjects
{

    public class RascomResult : RascomResult<object>
    {
        public RascomResult() : this(null) { }
        public RascomResult(object data, bool success = true, String msg = null)
            : base(data) { }
        public static implicit operator RascomResult(Exception ex)
        {
            return new RascomResult(ex);
        }
    }

    public class RascomResult<T> 
    {
        public RascomResult() : this(default(T)) { }
        public RascomResult(T data, bool success = true, String msg = null)
        {
            Type dType = typeof(T);
            DataType = dType.FullName;
            Data = data;
            Success = success;
            Message = !String.IsNullOrWhiteSpace(msg) ? msg : null;
            ErrCode = Success ? "NoError" : "UnspecifiedError";
            Type baseExType = typeof(Exception);
            Type appExType = typeof(ApplicationException);
            bool isException = baseExType.IsAssignableFrom(dType);
            bool isAppException = appExType.IsAssignableFrom(dType);

            if (isException)
            {
                Exception ex = Data as Exception;
                Data = default(T);
                Exception = isAppException ? ex : new ApplicationException(ex.Message, ex);
                Success = false;
                Message = ex.Message;
                ErrCode = ex.GetType().Name;
            }
        }
        public bool Success { get; set; }
        public String Message { get; set; }
        public Exception Exception { get; set; }
        public string ErrCode { get; set; }
        public string DataType { get; set; }

        public T Data { get; set; }
    }


}
