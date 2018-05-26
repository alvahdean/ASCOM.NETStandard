using System;
using System.Collections.Generic;
using System.Text;

namespace RACI.ASCOM.Service
{
    [Flags]
    public enum AscomRemoteState
    {
        Created=0,
        PendingSend=1,
        Sent=2,
        PendingRun=4,
        Running=8,
        Aborting=16,
        Success=128,
        Failed=256,
        Aborted=512
    }
}
