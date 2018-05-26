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

        public static PathUtil NodePath(ProfileNode node)
            => NodePath(node?.ProfileNodeId ?? 0);
        public static PathUtil NodePath(int nodeId)
        {
            string result = null;
            using (var uow = new RaciUnitOfWork())
            {
                ProfileNode node = uow.Nodes.GetById(nodeId);
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
        public static PathUtil ValuePath(ProfileValue val)
            => ValuePath(val?.ProfileValueId ?? 0);
        public static PathUtil ValuePath(int valueId)
        {
            string result = null;
            using (var uow = new RaciUnitOfWork())
            {
                ProfileValue pv = uow.Values.GetById(valueId);

                if (pv != null)
                {
                    result = $"{Separator}{pv.Key}";
                    ProfileNode parent = uow.Nodes.GetById(pv.ParentProfileNodeId);
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
    public class SystemHelper
    {
        private bool _ignoreCase;

        public SystemHelper() : this(".") { }
        public SystemHelper(string sysName)
        {
            IgnoreKeyCase = true;
            System = GetOrCreateSystem(sysName);
        }

        #region Static members
        static protected bool isNullable(Type t) => Nullable.GetUnderlyingType(t) == null;

        public static string CleanSysName(string sysName)
            => !String.IsNullOrWhiteSpace(sysName) ? sysName.Trim() : ".";
        public static String CleanDriverName(String driverName)
            => driverName.Replace("Drivers", "").Trim();
        public static String CleanPath(string path, bool removeTrailingSeparator = true)
        {
            PathUtil pi = new PathUtil(path);
            return removeTrailingSeparator
                ? pi.Path.TrimEnd(new char[] { PathUtil.SeparatorChar })
                : pi.Path;
        }

        #endregion

        /// <summary>
        /// Returns the IEqualityComparer<String> used to comparer string keys
        /// </summary>
        public IEqualityComparer<String> KeyComparer { get; private set; }

        /// <summary>
        /// Gets or sets whether string keys will be compared case-insensitively
        /// </summary>
        /// <remarks>Modifying this value will change the KeyComparer used</remarks>
        public bool IgnoreKeyCase
        {
            get => _ignoreCase;
            set
            {
                _ignoreCase = value;
                KeyComparer = _ignoreCase
                    ? (IEqualityComparer<String>)new CIKeyComparer()
                    : new CSKeyComparer();
            }
        }

        public RaciSystem System { get; private set; } = null;
        /// <summary>
        /// Removes all values and child nodes from the specified node but leaves the node record intact
        /// </summary>
        /// <param name="node">The node target</param>
        public void ClearNode(int nodeId)
        {
            ClearValues(nodeId);
            ClearChildren(nodeId);
        }
        /// <summary>
        /// Removes all values from the specified node but leaves the node record intact
        /// </summary>
        /// <param name="node">The node target</param>
        public void ClearValues(int nodeId)
        {
            using (var uow = UnitWork)
            {
                ProfileNode node = uow.Nodes
                    .Get(t => t.ProfileNodeId == nodeId, null, "Values")
                    .FirstOrDefault();
                if (node == null)
                    return;
                foreach (var pv in node.Values)
                    DeleteValue(pv.ProfileValueId);
                uow.Nodes.Update(node);
                uow.Save();
            }
        }
        /// <summary>
        /// Removes all child nodes from the specified node but leaves the node record intact
        /// </summary>
        /// <param name="node">The node target</param>
        public void ClearChildren(int nodeId)
        {
            using (var uow = UnitWork)
            {
                ProfileNode node = uow.Nodes
                    .Get(t => t.ProfileNodeId == nodeId, null, "Nodes")
                    .FirstOrDefault();
                if (node == null)
                    return;
                foreach (var n in node.Nodes)
                    DeleteNode(n.ProfileNodeId);
                uow.Nodes.Update(node);
                uow.Save();
            }
        }

        public void Delete()
        {
            RaciUnitOfWork uow = new RaciUnitOfWork();
            uow.Systems.Delete(System);
            uow.Save();
            System = null;
        }
        public void DeleteNode(int nodeId)
        {
            RaciUnitOfWork uow = new RaciUnitOfWork();
            uow.Nodes.Delete(nodeId);
            uow.Save();
        }
        public void DeleteValue(int valueId)
        {
            RaciUnitOfWork uow = new RaciUnitOfWork();
            uow.Values.Delete(valueId);
            uow.Save();
        }
        public String Name
        {
            get => System.Name;
            set
            {
                if (System?.Name != value)
                {
                    using (RaciUnitOfWork uow = new RaciUnitOfWork())
                    {
                        System.Name = value;
                        uow.Systems.Update(System);
                        uow.Save();
                    }
                }
            }
        }
        public String Hostname
        {
            get => System.Hostname;
            set
            {
                if (System?.Hostname != value)
                {
                    using (RaciUnitOfWork uow = new RaciUnitOfWork())
                    {
                        System.Hostname = value;
                        uow.Systems.Update(System);
                        uow.Save();
                    }
                }
            }
        }
        public String Description
        {
            get => System.Description;
            set
            {
                if (System?.Description != value)
                {
                    using (RaciUnitOfWork uow = new RaciUnitOfWork())
                    {
                        System.Description = value;
                        uow.Systems.Update(System);
                        uow.Save();
                    }
                }
            }
        }

        protected TResult ConvertValue<TResult>(String value)
        {
            return (TResult)Convert.ChangeType(value, typeof(TResult));
        }

        protected RaciSystem GetOrCreateSystem(string sysName)
        {
            RaciSystem sys = null;
            using (var uow = UnitWork)
            {
                var kcomp = uow.Systems.KeyComparer;
                if (String.IsNullOrWhiteSpace(sysName))
                    sysName = ".";
                else
                    sysName = sysName.Trim();

                //uow.Systems.Get(t => kcomp.Equals(sysName, t.Name) || kcomp.Equals(sysName, t.Hostname)).FirstOrDefault();
                sys = uow.Systems.Get(t => KeyComparer.Equals(sysName, t.Name)).FirstOrDefault();
                if (sys == null)
                {
                    string hostname = sysName == "."
                        ? Dns.GetHostName()
                        : Dns.GetHostEntry(sysName).HostName;
                    sys = new RaciSystem(sysName, hostname) { Hostname = hostname };
                    uow.Systems.Insert(sys);
                    uow.Save();
                }
            }
            return sys;
        }
        protected RaciUnitOfWork UnitWork => new RaciUnitOfWork();

        public IOrderedEnumerable<String> SubKeys(int rootId, bool recurse = true)
        {
            string rootPath = PathUtil.NodePath(rootId);
            int rPathLen = rootPath.Length;
            return EnumKeyValues(rootId, recurse)
                .Select(t => PathUtil.ValuePath(t.ProfileValueId).Path.Substring(rPathLen))
                .OrderBy(t => t);
        }
        public IOrderedEnumerable<KeyValuePair<string, ProfileValue>> SubValues(int rootId, bool recurse = true)
        {
            string rootPath = PathUtil.NodePath(rootId);
            int rPathLen = rootPath.Length;
            return EnumKeyValues(rootId, recurse)
                .Select(t => new KeyValuePair<string, ProfileValue>(
                    PathUtil.ValuePath(t.ProfileValueId).Path.Substring(rPathLen)
                    , t))
                .OrderBy(t => t.Key);
        }
        public IEnumerable<ProfileValue> EnumKeyValues(int rootId, bool recurse = true)
        {
            List<ProfileValue> result = new List<ProfileValue>();
            using (var uow = UnitWork)
            {
                ProfileNode node = NodeById(rootId);
                if (node != null)
                {
                    result.AddRange(node.Values);
                    if (recurse)
                    {
                        foreach (var pn in Nodes(rootId))
                            result.AddRange(EnumKeyValues(pn.ProfileNodeId, recurse));
                    }
                }
            }
            return result;
        }


        public bool IsChildNode(int parentId, int nodeId)
        {
            var uow = UnitWork;
            ProfileNode pnode = uow.Nodes.Get(t => t.ProfileNodeId == nodeId, null, "Parent").FirstOrDefault();
            while (pnode != null && pnode.ParentProfileNodeId.HasValue && pnode.ParentProfileNodeId != parentId)
                pnode = uow.Nodes.Get(t => t.ProfileNodeId == pnode.ParentProfileNodeId.Value, null, "Parent").FirstOrDefault();
            return pnode?.ProfileNodeId == parentId;
        }
        public bool IsChildValue(int parentId, int valueId)
        {
            var uow = UnitWork;
            ProfileValue pv = uow.Values.GetById(valueId);
            if (pv == null)
                return false;
            ProfileNode pnode = uow.Nodes.Get(t => t.ProfileNodeId == pv.ParentProfileNodeId, null, "Parent").FirstOrDefault();
            while (pnode != null && pnode.ParentProfileNodeId.HasValue && pnode.ParentProfileNodeId != parentId)
                pnode = uow.Nodes.Get(t => t.ProfileNodeId == pnode.ParentProfileNodeId.Value, null, "Parent").FirstOrDefault();
            return pnode?.ProfileNodeId == parentId;
        }
        public void SetParent(int nodeId, int? parentId)
        {
            RaciUnitOfWork uow = new RaciUnitOfWork();
            ProfileNode node = uow.Nodes.GetById(nodeId);
            if (node == null)
                throw new ArgumentNullException($"Node with id '{nodeId}' cannot be found");
            if (parentId.HasValue && uow.Nodes.GetById(parentId.Value) == null)
                throw new ArgumentNullException($"Parent node with id '{parentId}' cannot be found");

            if (node.ParentProfileNodeId != parentId)
            {
                node.ParentProfileNodeId = parentId;
                uow.Nodes.Update(node);
                uow.Save();
            }
        }
        public void SetParent(int nodeId, ProfileNode parent)
        {
            RaciUnitOfWork uow = new RaciUnitOfWork();
            ProfileNode node = uow.Nodes.GetById(nodeId);
            if (node == null)
                throw new ArgumentNullException($"Node with id '{nodeId}' cannot be found");
            int? parentId = parent?.ProfileNodeId;

            if (parentId.HasValue && uow.Nodes.GetById(parentId.Value) == null)
                throw new ArgumentNullException($"Parent node with id '{parentId}' cannot be found");

            if (node.ParentProfileNodeId != parentId)
            {
                node.ParentProfileNodeId = parentId;
                uow.Nodes.Update(node);
                uow.Save();
            }
        }
        public ProfileNode SetNodeName(int nodeId, string value)
        {
            value = CleanPath(value);
            if (String.IsNullOrWhiteSpace(value))
                throw new InvalidOperationException($"{nameof(SetNodeName)}: Invalid new name '{value}'");
            ProfileNode obj = null;
            using (var uow = UnitWork)
            {
                obj = uow.Nodes.GetById(nodeId);
                if (obj.Description != value)
                {
                    obj.Name = value;
                    uow.Nodes.Update(obj);
                    uow.Save();
                }
            }
            return obj;
        }
        public ProfileNode SetNodeDescription(int nodeId, string value)
        {
            ProfileNode obj = null;
            using (var uow = UnitWork)
            {
                obj = uow.Nodes.GetById(nodeId);
                if (obj.Description != value)
                {
                    obj.Description = value ?? "";
                    uow.Nodes.Update(obj);
                    uow.Save();
                }
            }
            return obj;
        }

        public bool HasNode(int parentId, String name)
            => UnitWork.Nodes
            .Get(t => t.ParentProfileNodeId == parentId)
            .Select(t => t.Name)
            .Any(t => KeyComparer.Equals(t, name));
        public IEnumerable<ProfileNode> Nodes(int parentId)
        {
            return UnitWork.Nodes.Get(t => t.ParentProfileNodeId == parentId, null, "Values,Nodes");
        }
        public IEnumerable<TNode> Nodes<TNode>(int parentId)
            where TNode : ProfileNode, new()
        {
            return UnitWork.NodesOfType<TNode>().Get(t => t.ParentProfileNodeId == parentId, null, "Values,Nodes");
        }
        public ProfileNode NodeById(int nodeId)
            => NodeById<ProfileNode>(nodeId);
        public TNode NodeById<TNode>(int nodeId)
            where TNode : ProfileNode, new()
        {
            return (TNode)UnitWork.Nodes
                .Get(t => t.ProfileNodeId == nodeId, null, "Parent,Nodes,Values")
                .FirstOrDefault();
        }
        private ProfileNode ChildNode(int parentId, String name, bool autoCreate = true)
            => ChildNode<ProfileNode>(parentId, name, autoCreate);
        private TNode ChildNode<TNode>(int parentId, String name, bool autoCreate = true)
            where TNode : ProfileNode, new()
        {
            if (String.IsNullOrWhiteSpace(name))
                throw new InvalidOperationException($"Node name must contain at least one non-space character");
            TNode sn = null;
            using (var uow = UnitWork)
            {
                ProfileNode parent = uow.Nodes.Get(t => t.ProfileNodeId == parentId, null, "Nodes").FirstOrDefault()
                    ?? throw new InvalidOperationException($"Parent node '{parentId}' not found");
                sn = parent.Nodes.FirstOrDefault(t => KeyComparer.Equals(t.Name, name)) as TNode;

                //Type ntype = typeof(TNode);
                //if ((sn != null && sn.ProfileType != ntype.Name) && ntype != typeof(ProfileNode))
                //    throw new InvalidCastException($"SubNode '{name}' was found but type does not match. Requested '{ntype.Name}', found {sn.ProfileType}");

                if (sn == null)
                {
                    sn = new TNode() { Name = name, ParentProfileNodeId = parentId };
                    uow.Nodes.Insert(sn);
                    parent.Nodes.Add(sn);
                    uow.Save();
                }
            }
            return sn;
        }

        public TNode SubNode<TNode>(ProfileNode rootNode, String path, bool autoCreate = false)
            where TNode : ProfileNode, new()
        {
            return SubNode<TNode>(rootNode?.ProfileNodeId ?? 0, path, autoCreate);
        }
        public ProfileNode SubNode(ProfileNode rootNode, String path, bool autoCreate = false)
            => SubNode<ProfileNode>(rootNode, path);
        public ProfileNode SubNode(int rootNodeId, String path, bool autoCreate = false)
            => SubNode<ProfileNode>(rootNodeId, path);
        public TNode SubNode<TNode>(int rootNodeId, String path, bool autoCreate = false) where TNode : ProfileNode, new()
        {
            ProfileNode rootNode = NodeById(rootNodeId);
            if (rootNode == null)
                throw new ArgumentNullException($"{nameof(SubNode)}: rootNode[{rootNodeId}] cannot be found");
            PathUtil pu = path;
            List<string> nodeNames = pu.Items.ToList();
            ProfileNode parent = rootNode;
            ProfileNode child = null;
            for (int i = 0; i < nodeNames.Count; i++)
            {
                string childName = nodeNames[i];
                if (!String.IsNullOrWhiteSpace(childName))
                {
                    child = i == nodeNames.Count - 1
                        ? ChildNode<TNode>(parent.ProfileNodeId, childName, autoCreate)
                        : ChildNode(parent.ProfileNodeId, childName, autoCreate);
                    if (child == null)
                    {
                        if (!autoCreate)
                            return null;
                        throw new Exception($"{nameof(SubNode)}: Could not create a subnode named '{childName}' in node Node[{parent.ProfileNodeId}] (autoCreate={autoCreate})");
                    }
                }
                parent = child;
            }
            if (child == null)
                child = NodeById<TNode>(rootNodeId);
            return (TNode)child;
        }

        public bool HasValue(int parentId, String key)
            => NodeValues(parentId).Any(t => KeyComparer.Equals(t.Key, key));
        public ProfileValue SetValueKey(int valueId, string value)
        {
            ProfileValue obj = null;
            value = CleanPath(value?.Trim() ?? "");

            using (var uow = UnitWork)
            {
                obj = uow.Values.GetById(valueId);
                if (obj != value)
                {
                    obj.Key = value ?? "";
                    uow.Values.Update(obj);
                    uow.Save();
                }
            }
            return obj;
        }
        public IEnumerable<ProfileValue> NodeValues(int parentId)
        {
            return UnitWork.Values.Get(t => t.ParentProfileNodeId == parentId);
        }

        public ProfileValue KeyValueById(int valueId)
            => UnitWork.Values.Get(t => t.ProfileValueId == valueId).FirstOrDefault();
        public ProfileValue KeyValueByName(int parentId, String key)
        {
            var parent = NodeById(parentId)
                ?? throw new InvalidOperationException($"Parent node[{parentId}] does not exist");
            return parent.Values.FirstOrDefault(t => KeyComparer.Equals(t.Key, key))
                ?? throw new InvalidOperationException($"Parent node[{parentId}] does not contain value key '{key}'");

        }
        public ProfileValue KeyValueByName(int parentId, String key, String defaultValue)
        {
            ProfileValue result = null;
            try { result = KeyValueByName(parentId, key); }
            catch (KeyNotFoundException) { result = null; }
            catch (Exception ex) { throw ex; }
            return result ?? SetValueByName(parentId, key, defaultValue);
        }
        public ProfileValue KeyValueByName<T>(int parentId, String key, T? defaultValue)
            where T : struct
        {
            return KeyValueByName(parentId, key, defaultValue?.ToString());
        }
        public ProfileValue KeyValueByPath(ProfileNode rootNode, string keyPath)
        {
            return KeyValueByPath(rootNode?.ProfileNodeId ?? 0, keyPath);
        }
        public ProfileValue KeyValueByPath(ProfileNode rootNode, string keyPath, string defaultValue)
        {
            return KeyValueByPath(rootNode, keyPath)
                ?? SetValueByPath(rootNode, keyPath, defaultValue);
        }
        public ProfileValue KeyValueByPath(int rootNodeId, string keyPath)
        {

            ProfileNode rootNode = NodeById(rootNodeId);
            if (rootNode == null)
                throw new InvalidOperationException($"{nameof(KeyValueByPath)}: rootNode[{rootNodeId}] cannot be null");
            PathUtil pi = new PathUtil(keyPath);
            string nodePath = pi.Parent;
            String key = pi.Value;

            ProfileNode child = SubNode(rootNode.ProfileNodeId, nodePath)
                ?? throw new InvalidOperationException($"{nameof(KeyValueByPath)}: rootNode[{rootNode.ProfileNodeId}] does not contain a subnode[{nodePath}]");
            return KeyValueByName(child.ProfileNodeId, key);
        }
        public ProfileValue KeyValueByPath(int nodeid, string keyPath, string defaultValue)
        {
            return KeyValueByPath(nodeid, keyPath)
                ?? SetValueByPath(nodeid, keyPath, defaultValue);
        }

        public String ValueByPath(ProfileNode parent, string keyPath)
            => KeyValueByPath(parent, keyPath)?.Value;
        public TResult ValueByPath<TResult>(ProfileNode parent, string keyPath)
            where TResult : struct
        {
            return ConvertValue<TResult>(KeyValueByPath(parent, keyPath).Value);
        }
        public String ValueByPath(ProfileNode parent, string keyPath, string defaultValue)
            => (KeyValueByPath(parent, keyPath)
            ?? SetValueByPath(parent, keyPath, defaultValue))
            .Value;
        public TResult ValueByPath<TResult>(ProfileNode parent, string keyPath, TResult defaultValue)
            where TResult : struct
        {
            ProfileValue pv = KeyValueByPath(parent, keyPath)
            ?? SetValueByPath(parent, keyPath, defaultValue);
            return ConvertValue<TResult>(pv.Value);
        }
        public TResult ValueOrDefault<TResult>(int parentId, String key, TResult defaultValue)
        where TResult : struct
        {
            string sval = ValueOrDefault(parentId, key, defaultValue.ToString());
            return ConvertValue<TResult>(sval);
        }
        public String ValueOrDefault(int parentId, String key, String defaultValue)
        {
            return KeyValueByName(parentId, key, defaultValue)?.Value ?? defaultValue;
        }
        public TResult ValueOrDefault<TResult>(ProfileValue valueObj, TResult defaultValue)
            where TResult : struct
        {
            ProfileValue pv = KeyValueById(valueObj.ProfileValueId);
            return pv != null
                ? ConvertValue<TResult>(pv.Value)
                : defaultValue;
        }
        public String ValueOrDefault(ProfileValue valueObj, String defaultValue)
        {
            return KeyValueById(valueObj.ProfileValueId)?.Value ?? defaultValue;
        }

        public TEnum ValueEnum<TEnum>(int pvId) where TEnum : struct
            => ValueEnum<TEnum>(KeyValueById(pvId));
        public TEnum ValueEnum<TEnum>(ProfileNode pNode, string key) where TEnum : struct
            => ValueEnum<TEnum>(KeyValueByName(pNode.ProfileNodeId, key, default(TEnum).ToString()));
        public TEnum ValueEnum<TEnum>(ProfileValue pv) where TEnum : struct
            => (TEnum)Enum.Parse(typeof(TEnum), pv?.Value ?? default(TEnum).ToString());

        public ProfileValue SetValueByPath<T>(ProfileNode rootNode, string path, T value) where T : struct
            => SetValueByPath(rootNode, path, value.ToString());
        public ProfileValue SetValueByPath(ProfileNode rootNode, string path, String value)
        {
            if (rootNode == null)
                throw new ArgumentNullException($"{nameof(KeyValueByPath)}: rootNode cannot be null");
            return SetValueByPath(rootNode.ProfileNodeId, path, value);
        }
        public ProfileValue SetValueByPath<T>(int rootNodeId, string path, T value) where T : struct
            => SetValueByPath(rootNodeId, path, value.ToString());
        public ProfileValue SetValueByPath(int rootNodeId, string path, String value)
        {
            PathUtil pu = path;
            string nodePath = pu.Parent;
            string key = pu.Value;

            ProfileNode subnode = SubNode(rootNodeId, nodePath, true);
            if (subnode == null)
                return null;
            return SetValueByName(subnode.ProfileNodeId, key, value);
        }

        public ProfileValue SetValueById(int valueId, String value)
        {
            ProfileValue result = null;
            using (var uow = UnitWork)
            {
                result = uow.Values.Get(t => t.ProfileValueId == valueId).FirstOrDefault();
                if (result == null)
                    throw new KeyNotFoundException($"{nameof(SetValue)}: Value with id='{valueId}' not found");
                result.Value = value;
                uow.Values.Update(result);
                uow.Save();
            }
            return result;
        }
        public ProfileValue SetValueById<T>(int valueId, T value) where T : struct
            => SetValueById(valueId, value.ToString());
        public ProfileValue SetValue(ProfileValue valueObj, String value)
            => SetValueById(valueObj?.ProfileValueId ?? 0, value);
        public ProfileValue SetValue<T>(ProfileValue valueObj, T value) where T : struct
            => SetValue(valueObj, value.ToString());
        public ProfileValue SetValueByName(int parentId, String key, String value)
        {
            RaciUnitOfWork uow = new RaciUnitOfWork();
            ProfileNode parent = uow.Nodes.Get(t => t.ProfileNodeId == parentId, null, "Values").FirstOrDefault()
                ?? throw new InvalidOperationException($"Parent node '{parentId}' not found");
            ProfileValue pv = parent.Values.FirstOrDefault(t => uow.Nodes.KeyComparer.Equals(t.Key, key));
            if (pv != null)
            {
                pv.Value = value;
                uow.Values.Update(pv);
            }
            else
            {
                pv = new ProfileValue(key, value.ToString()) { ParentProfileNodeId = parentId };
                uow.Values.Insert(pv);
                parent.Values.Add(pv);
            }
            uow.Save();
            return pv;
        }
        public ProfileValue SetValueByName<T>(int parentId, String key, T value)
            where T : struct
        {
            return SetValueByName(parentId, key, value.ToString());
        }
        public ProfileValue SetValueEnum<TEnum>(int valueId, TEnum value) where TEnum : struct
            => SetValueById(valueId, value.ToString());
        public ProfileValue SetValueEnum<TEnum>(ProfileNode parent, string key, TEnum value) where TEnum : struct
            => SetValueByName(parent.ProfileNodeId, key, value.ToString());
        public ProfileValue SetValueEnum<TEnum>(ProfileValue valueObj, TEnum value) where TEnum : struct
            => SetValue(valueObj, value.ToString());

        public String CurrentUserName
        {
            get
            {
                string result = CurrentFullUserName;
                int domsep = result.IndexOf("\\");
                if (domsep > 0)
                    result = result.Substring(domsep + 1);
                return result;
            }
        }
        public String CurrentFullUserName
        {
            get
            {
                return System.Name == "."
                    ? Environment.UserName
                    : throw new NotImplementedException($"Current user info is not implemented for remote system entries");
            }
        }
        public IEnumerable<RaciUser> Users =>
             UnitWork.NodesOfType<RaciUser>().Get(t => t.ParentProfileNodeId == System.ProfileNodeId);
        public RaciUser GetUser(string userName)
        {
            if (String.IsNullOrWhiteSpace(userName))
                userName = CurrentUserName;
            return Users.FirstOrDefault(t => KeyComparer.Equals(userName, t.Name));
        }
        public IEnumerable<String> UserSubKeys(string userName)
        {
            if (String.IsNullOrWhiteSpace(userName))
                userName = CurrentUserName;
            return SubKeys(GetUser(userName)?.ProfileNodeId ?? 0);
        }
        public RaciUser GetUser() => GetOrCreateUser(CurrentUserName);
        public RaciUser GetOrCreateUser(String userName, string userId = "")
        {
            userId = userId?.Trim() ?? "";
            RaciUser result = ChildNode<RaciUser>(System.ProfileNodeId, userName, true);
            if (userId != "" && result.Description != userId)
            {
                RaciUnitOfWork uow = new RaciUnitOfWork();
                result.Description = userId;
                uow.Nodes.Update(result);
                uow.Save();
            }
            return result;
        }

        public AscomPlatformNode Ascom => SubNode<AscomPlatformNode>(System, "ASCOM", true);
        /// <summary>
        /// Returns a dictionary of all drivers registered 
        /// on the system managed by the SystemHelper instance
        /// </summary>
        public IEnumerable<AscomDriverNode> AscomDrivers
        {
            get
            {
                var all = UnitWork.NodesOfType<AscomDriverNode>().All.ToList();
                var drvList = all.Where(t => t.ParentProfileNodeId == Ascom.ProfileNodeId).ToList();
                return drvList;
            }
    }

        /// <summary>
        /// Returns the specified driver registered on the system managed by the SystemHelper instance
        /// </summary>
        public AscomDriverNode AscomDriver(string driverName) =>
            driverName.ToLower().EndsWith(" drivers")
            ? AscomDrivers.FirstOrDefault(t => KeyComparer.Equals(t.Name, driverName))
            : AscomDrivers.FirstOrDefault(t => KeyComparer.Equals(t.Name, $"{driverName} Drivers"));

        /// <summary>
        /// Returns a dictionary of all devices of the specified driver
        /// registered on the system managed by the SystemHelper instance
        /// </summary>
        public IEnumerable<AscomDeviceNode> AscomDevices(string driverName) =>
            AscomDevices(AscomDriver(driverName));

        /// <summary>
        /// Returns a dictionary of all devices of the specified driver
        /// registered on the system managed by the SystemHelper instance
        /// </summary>
        public IEnumerable<AscomDeviceNode> AscomDevices(AscomDriverNode driver) =>
            UnitWork.NodesOfType<AscomDeviceNode>()
            .Get(t => t.ParentProfileNodeId == driver.ProfileNodeId);

        /// <summary>
        /// Returns a dictionary of all devices of the specified driver
        /// registered on the system managed by the SystemHelper instance
        /// </summary>
        public AscomDeviceNode AscomDevice(string driverName, string deviceName) =>
            AscomDevices(driverName).SingleOrDefault(t => KeyComparer.Equals(t.Name, deviceName));

        /// <summary>
        /// Returns a dictionary of all devices of the specified driver
        /// registered on the system managed by the SystemHelper instance
        /// </summary>
        public AscomDeviceNode AscomDevice(AscomDriverNode driver, string deviceName) =>
            AscomDevices(driver).SingleOrDefault(t => KeyComparer.Equals(t.Name, deviceName));

        /// <summary>
        /// Returns a list of all subkeys under the specified Ascom device and system managed by the SystemHelper instance
        /// </summary>
        public IEnumerable<String> AscomDeviceValueKeys(string driverName, string deviceName)
        {
            AscomDeviceNode node = AscomDevice(driverName, deviceName);
            return node != null
                ? SubKeys(node.ProfileNodeId)
                : null;
        }

        public RaciSettings Raci  
            => UnitWork
            .NodesOfType<RaciSettings>()
            .Get(t => t.ParentProfileNodeId == System.ProfileNodeId,null,"Nodes,Values")
            .SingleOrDefault();
        public ProfileValue GetRaciKeyValue(String keyPath) => KeyValueByPath(Raci, keyPath);
        public String GetRaciValue(String keyPath) => ValueByPath(Raci, keyPath);
        public T GetRaciValue<T>(String keyPath) where T: struct
            => ValueByPath<T>(Raci, keyPath);
        public ProfileValue SetRaci<T>(String path, T value) where T : struct 
            => SetRaci(path, value.ToString());
        public ProfileValue SetRaci(String path, String value) 
            => SetValueByPath(Raci, path, value);

        public string NodePath(int profileNodeId) => PathUtil.NodePath(profileNodeId);
        public string ValuePath(int profileValueId) => PathUtil.ValuePath(profileValueId);
    }
}
