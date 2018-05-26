using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ASCOM.Havla
{
    public class JsonHashStore: Dictionary<String,String>, IDictionary<String,object>
    {
        private Dictionary<String, Type> _typeMap = new Dictionary<string, Type>();
        private String Serialize(object obj) => Serialize<object>(obj);
        private String Serialize<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.Indented);
        }
        private object Deserialize(String json) => Deserialize<object>(json);
        private void DeserializeTo(String json,object obj) => JsonConvert.PopulateObject(json, obj);
        private T Deserialize<T>(String json)=> JsonConvert.DeserializeObject<T>(json);

        //
        // Summary:
        //     Initializes a new instance of the GenericHashStore class
        //     that is empty, has the default initial capacity, and uses the default equality
        //     comparer for the key type.
        public JsonHashStore() : base() { }
        //
        // Summary:
        //     Initializes a new instance of the GenericHashStore class
        //     that contains elements copied from the specified System.Collections.Generic.IDictionary`2
        //     and uses the default equality comparer for the key type.
        //
        // Parameters:
        //   dictionary:
        //     The System.Collections.Generic.IDictionary`2 whose elements are copied to the
        //     new GenericHashStore.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     dictionary is null.
        //
        //   T:System.ArgumentException:
        //     dictionary contains one or more duplicate keys.
        public JsonHashStore(IDictionary<String, object> dictionary) : base()
        {
            populate(dictionary);
        }
        //
        // Summary:
        //     Initializes a new instance of the GenericHashStore class
        //     that is empty, has the default initial capacity, and uses the specified System.Collections.Generic.IEqualityComparer`1.
        //
        // Parameters:
        //   comparer:
        //     The System.Collections.Generic.IEqualityComparer`1 implementation to use when
        //     comparing keys, or null to use the default System.Collections.Generic.EqualityComparer`1
        //     for the type of the key.
        public JsonHashStore(IEqualityComparer<String> comparer) : base(comparer) { }
        //
        // Summary:
        //     Initializes a new instance of the GenericHashStore class
        //     that is empty, has the specified initial capacity, and uses the default equality
        //     comparer for the key type.
        //
        // Parameters:
        //   capacity:
        //     The initial number of elements that the GenericHashStore
        //     can contain.
        //
        // Exceptions:
        //   T:System.ArgumentOutOfRangeException:
        //     capacity is less than 0.
        public JsonHashStore(int capacity) : base(capacity) { }
        //
        // Summary:
        //     Initializes a new instance of the GenericHashStore class
        //     that contains elements copied from the specified System.Collections.Generic.IDictionary`2
        //     and uses the specified System.Collections.Generic.IEqualityComparer`1.
        //
        // Parameters:
        //   dictionary:
        //     The System.Collections.Generic.IDictionary`2 whose elements are copied to the
        //     new GenericHashStore.
        //
        //   comparer:
        //     The System.Collections.Generic.IEqualityComparer`1 implementation to use when
        //     comparing keys, or null to use the default System.Collections.Generic.EqualityComparer`1
        //     for the type of the key.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     dictionary is null.
        //
        //   T:System.ArgumentException:
        //     dictionary contains one or more duplicate keys.
        public JsonHashStore(IDictionary<String, object> dictionary, IEqualityComparer<string> comparer)
            : base(comparer)
        {
            populate(dictionary);
        }
        //
        // Summary:
        //     Initializes a new instance of the GenericHashStore class
        //     that is empty, has the specified initial capacity, and uses the specified System.Collections.Generic.IEqualityComparer`1.
        //
        // Parameters:
        //   capacity:
        //     The initial number of elements that the GenericHashStore
        //     can contain.
        //
        //   comparer:
        //     The System.Collections.Generic.IEqualityComparer`1 implementation to use when
        //     comparing keys, or null to use the default System.Collections.Generic.EqualityComparer`1
        //     for the type of the key.
        //
        // Exceptions:
        //   T:System.ArgumentOutOfRangeException:
        //     capacity is less than 0.
        public JsonHashStore(int capacity, IEqualityComparer<string> comparer) : base(capacity, comparer) { }

        virtual protected void populate(IDictionary<String,object> dictionary)
        {
            if (dictionary != null)
                foreach (var kvp in dictionary)
                    Add(kvp.Key, kvp.Value);
        }

        #region IDictionary<String,object> implementation

        public bool TryGetValue(string key, out object value)
        {
            return TryGetValue<object>(key, out value);
        }

        public bool TryGetValue<T>(string key, out T value)
        {

            string json = null;
            value = default(T);
            if (!base.TryGetValue(key, out json))
                return false;
            Type targType = typeof(T);
            Type actType = _typeMap[key];
            if (!targType.IsAssignableFrom(actType))
            {
                return false;
                //throw new InvalidCastException($"Can't convert item[{key}] from '{actType.Name}' => '{targType.Name}'");
            }
            object actValue = Activator.CreateInstance(actType);

            DeserializeTo(json, actValue);
            value = (T)actValue;
            return true;
        }

        new public object this[string key]
        {
            get
            {
                if (TryGetValue(key, out object result))
                    return result;
                throw new KeyNotFoundException($"No item with key '{key}' was found");
            }
            set 
            {
                Add(key, value);
            }
        }

        ICollection<string> IDictionary<string, object>.Keys => Keys;

        ICollection<object> IDictionary<string, object>.Values => Items.Select(t => t.Value).ToList();

        public bool Remove(KeyValuePair<string, object> item) =>
            Contains(item) ? Remove(item.Key) : false;

        new public void Clear()
        {
            _typeMap.Clear();
            base.Clear();
        }

        /// <summary>
            /// Indicates whether the specified key and value exist in the dictionary
            /// Note: Since all actual values are translated into string representations
            ///       then the object references are not compared, the string representations are
            /// </summary>
            /// <param name="item"></param>
            /// <returns></returns>
        public bool Contains(KeyValuePair<string, object> item)
        {
            if (!ContainsKey(item.Key))
                return false;
            string tstVal = Serialize(item.Value);
            string actVal = base[item.Key];
            return tstVal == actVal;
        }

        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            //Dictionary<string, string> x = new Dictionary<string, object>();

            if (array == null)
                throw new ArgumentNullException($"argument '{nameof(array)}' cannot be null");
            if(arrayIndex<0)
                throw new ArgumentOutOfRangeException($"argument '{nameof(arrayIndex)}' must be a valid array index");
            foreach(var key in Keys)
                array[arrayIndex++] = new KeyValuePair<string, object>(key, this[key]);
        }

        public bool IsReadOnly => false;

        IEnumerator<KeyValuePair<string, object>> IEnumerable<KeyValuePair<string, object>>.GetEnumerator() =>
            Items.GetEnumerator();

        public IEnumerable<KeyValuePair<string, object>> Items
        {
            get
            {
                List<KeyValuePair<string, object>> result = new List<KeyValuePair<string, object>>(Count);
                foreach (var key in Keys)
                    result.Add(new KeyValuePair<string, object>(key, this[key]));
                return result;
            }
        }

        public void Add(string key, object value) => Add<object>(key, value);

        public void Add(KeyValuePair<string, object> item) => Add(item.Key, item.Value);

        public void Add<T>(string key, T value)
        {
            if (ContainsKey(key))
                Remove(key);
            base.Add(key, Serialize(value));
            _typeMap.Add(key, typeof(T));
        }

        new bool Remove(String key)
        {
            return _typeMap.Remove(key) && base.Remove(key);
        }

        #endregion

    }
}
