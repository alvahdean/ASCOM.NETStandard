using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using ASCOM.Utilities.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace RACI.Data
{
    public class ProfileNode
    {
        public ProfileNode() : this("", "")
        {
        }
        public ProfileNode(String name, String description = "")
        {
            Values = new List<ProfileValue>();
            Nodes = new List<ProfileNode>();
            Name = name;
            Description = description;
        }

        //Parent & Name form unique constraint
        public int ProfileNodeId { get; set; }
        public string ProfileType { get; set; }
        public int? ParentProfileNodeId { get; set; }
        virtual public ProfileNode Parent { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        virtual public ICollection<ProfileValue> Values { get; set; }
        virtual public ICollection<ProfileNode> Nodes { get; set; }
    }

    public class ProfileNode<TParent> : ProfileNode
    where TParent : ProfileNode, new()
    {
        public ProfileNode() : this("", "") { }
        public ProfileNode(String name, String description = "") : base(name, description) { }

        new virtual public TParent Parent
        {
            get => base.Parent as TParent;
            set => base.Parent = value;
        }
    }

    public class ProfileNode<TParent, TNode> : ProfileNode<TParent>
        where TParent : ProfileNode, new()
        where TNode : ProfileNode, new()
    {
        public ProfileNode() : this("", "") { }
        public ProfileNode(String name, String description = "") : base(name, description) { }

        new virtual public ICollection<TNode> Nodes
        {
            get => (ICollection<TNode>)base.Nodes;
            set => base.Nodes = (ICollection<ProfileNode>)value;
        }
    }


}
