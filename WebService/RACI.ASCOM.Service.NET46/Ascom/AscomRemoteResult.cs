using System;
using System.Collections.Generic;
using System.Text;

namespace RACI.ASCOM.Service
{
    public class AscomRemoteResult<T>
    {
        public T Result { get; private set; }
        public Exception Exception { get; private set; }
        public bool Success { get; private set; }
        public AscomRemoteState State { get; private set; }
    }
}
