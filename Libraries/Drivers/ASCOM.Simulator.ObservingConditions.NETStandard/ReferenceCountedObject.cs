using System;
using System.Runtime.InteropServices;

namespace xASCOM.Simulator
{
    [ComVisible(false)]
    public class ReferenceCountedObjectBase
    {
        private static int _count=0;
        public ReferenceCountedObjectBase()
        {
            // We increment the global count of objects.
            OCSimulator.TL.LogMessage("ReferenceCountedObjectBase", "Incrementing object count");
            //Server.CountObject();
            _count++;
        }

        ~ReferenceCountedObjectBase()
        {
            // We decrement the global count of objects.
            OCSimulator.TL.LogMessage("~ReferenceCountedObjectBase", "Decrementing object count");
            //Server.UncountObject();
            _count--;
            // We then immediately test to see if we the conditions
            // are right to attempt to terminate this server application.
            OCSimulator.TL.LogMessage("~ReferenceCountedObjectBase", "Calling ExitIf");
            //Server.ExitIf();
        }
    }
}
