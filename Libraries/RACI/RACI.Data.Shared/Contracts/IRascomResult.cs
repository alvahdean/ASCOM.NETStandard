
using System;

namespace RACI.Data
{

    public interface IRascomResult
    {
        bool Success { get; set; }
        String Message { get; set; }
        string ErrCode { get; set; }
        Exception Exception { get; set; }
    }
    public interface IRascomResult<T> : IRascomResult
    {
        T Data { get; set; }
    }



}
