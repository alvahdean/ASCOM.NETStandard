using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RACI.Settings;

namespace RACI.Data
{
    public class PathUtil
    {
        static protected string[] InvalidChars = new string[] { };
        static protected string[] InvalidStartEndChars = new string[] { " ", "\t", "\n", "\r", "\v" };

        private List<String> _items;
        private static String _sep;
        public IEnumerable<String> Items => _items ?? (_items = new List<string>());
        public IEnumerable<String> ParentItems => Parent.Split(new string[] {Separator},StringSplitOptions.RemoveEmptyEntries);
        public String RawPath { get; private set; }
        public String Path { get; private set; }
        public String Parent { get; private set; }
        public String Value { get; private set; }
        public bool IsDirRef { get; private set; }
        public bool IsValid { get; private set; }
        public static String Separator
        {
            get => _sep;
            set
            {
                if(_sep!=value)
                {
                    _sep = value ?? _sep;
                }
            }
        }
        public static Char SeparatorChar { get => Separator[0]; }
        private void init()
        {
            if (_items == null)
                _items = new List<string>();
            _items.Clear();
            Path = "";
            RawPath = "";
            Parent = "";
            Value = "";
            IsDirRef = false;
            IsValid = false;
        }
        private void parse(String path)
        {

            RawPath = path;
            if (String.IsNullOrWhiteSpace(path))
                path = Separator;

            Path = RawPath;
            IsDirRef = Path.Trim().EndsWith(Separator);
            _items = path
                .Split(new string[] { Separator }, StringSplitOptions.RemoveEmptyEntries)
                .Select(t => t.Trim())
                .Where(t => !String.IsNullOrWhiteSpace(t))
                .ToList();
            if (IsDirRef)
                _items.Add("");
            int c = _items.Count;
            IsValid = c > 0;
            //TODO: Add invalid character checking
            if (c > 0)
            {

                Value = _items[c - 1];
                for (int i = 0; i < c - 1; i++)
                    Parent = $"{Parent}{Separator}{_items[i]}";
                Parent = Parent.Trim(Separator[0]);
                Path = $"{Parent}{Separator}{Value}";
            }
        }

        static PathUtil()
        {
            _sep = "\\";
        }
        private PathUtil():this(null) { }
        public PathUtil(string path)
        {
            init();
            parse(path);
        }
        public static implicit operator String(PathUtil pu) => pu.Path;
        public static implicit operator PathUtil(String path) => new PathUtil(path);

        public static PathUtil NodePath(IProfileNode node) => NodePath(node?.ProfileNodeId ?? 0);
        public static PathUtil NodePath(int nodeId)
        {
            string result = null;
            using (var uow = new RaciUnitOfWork())
            {
                IProfileNode node = uow.Nodes.GetById(nodeId);
                if (node != null)
                {
                    result = $"{Separator}{node.Name}{Separator}";
                    while (node.ParentProfileNodeId != null)
                    {
                        node = uow.Nodes.GetById(node.ParentProfileNodeId);
                        result = $"{Separator}{node.Name}{result}";
                    }
                }
            }
            return Parse(result);
        }
        public static PathUtil ValuePath(ProfileValue val) => ValuePath(val?.ProfileValueId ?? 0);
        public static PathUtil ValuePath(int valueId)
        {
            string result = null;
            using (var uow = new RaciUnitOfWork())
            {
                IProfileValue pv = uow.Values.GetById(valueId);

                if (pv != null)
                {
                    result = $"{Separator}{pv.Key}";
                    IProfileNode parent = uow.Nodes.GetById(pv.ParentProfileNodeId);
                    while (parent != null)
                    {
                        result = $"{Separator}{parent.Name}{result}";
                        parent = parent.ParentProfileNodeId.HasValue
                            ? uow.Nodes.GetById(parent.ParentProfileNodeId)
                            : null;
                    }
                }
            }
            return Parse(result);
        }
        public static PathUtil Parse(String path) => new PathUtil(path);
    }
}
