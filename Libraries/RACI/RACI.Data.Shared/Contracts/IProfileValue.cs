using System;
using System.Collections.Generic;
using System.Text;

namespace RACI.Data
{
    public interface IProfileValue : IEquatable<IProfileValue>, IComparable<IProfileValue>
    {
        int ParentProfileNodeId { get; set; }
        int ProfileValueId { get; set; }
        IProfileNode Parent { get; set; }
        String Key { get; set; }
        String Value { get; set; }
    }
}
