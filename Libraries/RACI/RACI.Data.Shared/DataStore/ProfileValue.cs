using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace RACI.Data
{
    //public interface IProfileValue : IEquatable<IProfileValue>, IComparable<IProfileValue>
    //{
    //    int ParentProfileNodeId { get; set; }
    //    int ProfileValueId { get; set; }
    //    IProfileNode Parent { get; set; }
    //    String Key { get; set; }
    //    String Value { get; set; }
    //}

    public class ProfileValue : IProfileValue
    {
        private static ProfileKeyComparer keyComp = new ProfileKeyComparer();
        public ProfileValue() : this("", "")
        {
        }

        public ProfileValue(String key, String value = "")
        {
            Key = key;
            Value = value;
        }
        public int ParentProfileNodeId { get; set; }
        public int ProfileValueId { get; set; }
        public static implicit operator String(ProfileValue pv) => pv?.Value;
        public static implicit operator bool? (ProfileValue pv) => !String.IsNullOrWhiteSpace(pv.Value) ? bool.Parse(pv.Value) : (bool?)null;
        public static implicit operator int? (ProfileValue pv) => !String.IsNullOrWhiteSpace(pv.Value) ? int.Parse(pv.Value) : (int?)null;
        public static implicit operator double? (ProfileValue pv) => !String.IsNullOrWhiteSpace(pv.Value) ? double.Parse(pv.Value) : (double?)null;
        public static implicit operator DateTime? (ProfileValue pv) => !String.IsNullOrWhiteSpace(pv.Value) ? DateTime.Parse(pv.Value) : (DateTime?)null;
        //Parent & Key form unique constraint
        public ProfileNode Parent { get; set; }
        public String Key { get; set; }
        public String Value { get; set; }

        #region IEqualityComparisons
        public bool Equals(IProfileValue other) => keyComp.Equals(Key, other?.Key) && ParentProfileNodeId == other.ParentProfileNodeId;
        public int CompareTo(IProfileValue other) => keyComp.Compare(Key, other?.Key);

        IProfileNode IProfileValue.Parent
        {
            get => Parent;
            set { Parent = value as ProfileNode; }
        }
        //public static bool operator >(ProfileValue a, IProfileValue b) => a.CompareTo(b)>0;
        //public static bool operator <(ProfileValue a, IProfileValue b) => a.CompareTo(b)<0;
        //public static bool operator ==(ProfileValue a, IProfileValue b) => a.Equals(b);
        //public static bool operator !=(ProfileValue a, IProfileValue b) => a.Equals(b);
        //public override int GetHashCode()
        //{
        //    unchecked { return 13 * Key.GetHashCode() * ParentProfileNodeId ^ 101; }
        //}
        #endregion
    }
}
