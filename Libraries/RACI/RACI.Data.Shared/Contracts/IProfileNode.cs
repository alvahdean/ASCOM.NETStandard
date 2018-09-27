using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace RACI.Data
{
#if !IPROFILE_ORIG
    public interface IProfileNode : IEquatable<IProfileNode>, IComparable<IProfileNode>
    {
        int ProfileNodeId { get; set; }
        string ProfileType { get; set; }
        int? ParentProfileNodeId { get; set; }
        String Name { get; set; }
        String Description { get; set; }

        ProfileNode Parent { get; set; }
        HashSet<ProfileValue> Values { get; set; }
        HashSet<ProfileNode> Nodes { get; set; }
    }

    //----------------------------------------------------------


    //public interface IProfileNode<TParent> : IProfileNode
    //where TParent :IProfileNode
    //{
    //    new TParent Parent { get; set; }

    //}
    /*
    public interface IProfileNode<TParent, TNode> : IProfileNode<TParent>
    where TParent : class, IProfileNode
    where TNode : class, IProfileNode
    {
        new ICollection<TNode> Nodes { get; set; }
    }

    public interface IProfileNode<TParent, TNode, TValue> : IProfileNode<TParent,TNode>
    where TParent : class, IProfileNode
    where TNode : class, IProfileNode
    where TValue : class, IProfileValue
    {
        new ICollection<TValue> Values { get; set; }
    }
    */
    //----------------------------------------------------------
#else
    //Original generic => specific
    //Original IProfileNode<P,N,V> : IProfileNode 
       public interface IProfileNode :  IEquatable<IProfileNode>, IComparable<IProfileNode>
    {
        int ProfileNodeId { get; set; }
        string ProfileType { get; set; }
        int? ParentProfileNodeId { get; set; }
        String Name { get; set; }
        String Description { get; set; }

        IProfileNode Parent { get; }
        ICollection<IProfileValue> Values { get; set; }
        ICollection<IProfileNode> Nodes { get; set; }

    }
    public interface IProfileNode<TParent> : IProfileNode
    where TParent : class,IProfileNode
    {
        new TParent Parent { get; set; }
    }

    public interface IProfileNode<TParent, TNode> : IProfileNode<TParent>
    where TParent : class, IProfileNode
    where TNode : class, IProfileNode
    {
        new ICollection<TNode> Nodes { get; set; }
    }

    public interface IProfileNode<TParent, TNode, TValue> : IProfileNode<TParent,TNode>
    where TParent : class, IProfileNode
    where TNode : class, IProfileNode
    where TValue : class, IProfileValue
    {
        new ICollection<TValue> Values { get; set; }
    }
#endif
}
