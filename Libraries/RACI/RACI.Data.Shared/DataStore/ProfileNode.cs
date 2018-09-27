using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace RACI.Data
{
    //public interface IProfileNode : IEquatable<IProfileNode>, IComparable<IProfileNode>
    //{
    //    int ProfileNodeId { get; set; }
    //    string ProfileType { get; set; }
    //    int? ParentProfileNodeId { get; set; }
    //    String Name { get; set; }
    //    String Description { get; set; }

    //    ProfileNode Parent { get; set; }
    //    HashSet<ProfileValue> Values { get; set; }
    //    HashSet<ProfileNode> Nodes { get; set; }
    //}

    public class ProfileNode : IProfileNode
    {
        private static ProfileKeyComparer keyComp = new ProfileKeyComparer();
        public ProfileNode() : this("", "") { }
        public ProfileNode(String name, String description = "")
        {
            Name = name?.Trim() ?? "";
            Description = description?.Trim() ?? "";
        }

        //Parent & Name form unique constraint
        [Key]
        public int ProfileNodeId { get; set; }
        public string ProfileType { get; set; }
        public int? ParentProfileNodeId { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }


        #region IEquatable
        public bool Equals(IProfileNode other) =>
            keyComp.Equals(Name, other?.Name)
            && ParentProfileNodeId == other.ParentProfileNodeId;
        public int CompareTo(IProfileNode other) => keyComp.Compare(Name, other?.Name);
        //public static int operator >(ProfileNode a, IProfileNode b) => a.CompareTo(b);
        //public static int operator <(ProfileNode a, IProfileNode b) => a.CompareTo(b);
        //public static bool operator ==(ProfileNode a, IProfileNode b) => a.Equals(b);
        //public static bool operator !=(ProfileNode a, IProfileNode b) => a.Equals(b);
        public override int GetHashCode()
        {
            unchecked { return 13 * Name.GetHashCode() * (ParentProfileNodeId ?? 0) ^ 101; }
        }

        [JsonIgnore]
        virtual public ProfileNode Parent { get; set; }
        virtual public HashSet<ProfileNode> Nodes { get; set; }
        virtual public HashSet<ProfileValue> Values { get; set; }

        //IProfileNode IProfileNode.Parent
        //{
        //    get => Parent;
        //    set { Parent = (ProfileNode)value; }
        //}

        //HashSet<IProfileNode> IProfileNode.Nodes
        //{
        //    //(HashSet<IProfileNode>)
        //    get => Nodes;
        //    set { Nodes = (HashSet<ProfileNode>)value; }
        //}

        //HashSet<IProfileValue> IProfileNode.Values
        //{
        //    get => Values;
        //    set { Values = (ICollection<ProfileValue>)value; }
        //}

        //ICollection<IProfileValue> IProfileNode.Values
        //{
        //    get { return (ICollection<IProfileValue>)Values.Cast<IProfileValue>(); }
        //    set { Values = (ICollection<object>)value; }
        //}
        //ICollection<IProfileNode> IProfileNode.Nodes
        //{
        //    get { return (ICollection<IProfileValue>)Nodes.Cast<IProfileValue>(); }
        //    set { Nodes = (ICollection<object>)value; }
        //}
        #endregion
    }

    public class ProfileNode<TParent> : ProfileNode,IProfileNode//, IProfileNode<TParent>
        where TParent : IProfileNode
    {
        public ProfileNode() : this("", "") { }
        public ProfileNode(String name, String description = "") : base(name, description) { }

        [JsonIgnore]
        public new TParent Parent
        {
            get;
            set;
        }
    }

    public class ProfileNode<TParent, TNode> : ProfileNode<TParent>//, IProfileNode<TParent, TNode>
        where TParent : IProfileNode, new()
        where TNode : IProfileNode, new()
    {
        public ProfileNode() : this("", "") { }
        public ProfileNode(String name, String description = "") : base(name, description) { }

        public new HashSet<TNode> Nodes
        {
            get;
            set;
        }
    }

    public class ProfileNode<TParent, TNode, TValue> : ProfileNode<TParent, TNode>//, IProfileNode<TParent, TNode>, IProfileNode<TParent, TNode, TValue>
        where TParent : IProfileNode, new()
        where TNode : IProfileNode, new()
        where TValue : IProfileValue, new()
    {
        private static ProfileKeyComparer keyComp = new ProfileKeyComparer();
        public ProfileNode() : this("", "") { }
        public ProfileNode(String name, String description = "") : base(name, description) { }

        public new HashSet<TValue> Values
        {
            get;
            set;
            //get => base.Values; 
            //set { base.Values = value; }
        }

    }
}
