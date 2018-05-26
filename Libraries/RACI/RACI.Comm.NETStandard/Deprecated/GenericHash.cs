using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ASCOM.Havla
{
    public class GenericHash: Dictionary<String,object>
    {

        //
        // Summary:
        //     Initializes a new instance of the GenericHashStore class
        //     that is empty, has the default initial capacity, and uses the default equality
        //     comparer for the key type.
        public GenericHash() : base() { }
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
        public GenericHash(IDictionary<String, object> dictionary) : base()
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
        public GenericHash(IEqualityComparer<String> comparer) : base(comparer) { }
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
        public GenericHash(int capacity) : base(capacity) { }
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
        public GenericHash(IDictionary<String, object> dictionary, IEqualityComparer<string> comparer)
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
        public GenericHash(int capacity, IEqualityComparer<string> comparer) : base(capacity, comparer) { }

        virtual protected void populate(IDictionary<String, object> dictionary)
        {
            if (dictionary != null)
                foreach (var kvp in dictionary)
                    Add(kvp.Key, kvp.Value);
        }

        new public object this[string key]
        {
            get => TryGetValue(key, out object result) ? result : throw new KeyNotFoundException($"No item with key '{key}' was found");
            set => Add(key, value);
        }
        new public void Add(string key, object value)
        {
            if (ContainsKey(key))
                Remove(key);
            Add(key, value);
        }
        public object Get(string key) => Get<object>(key);
        public T Get<T>(string key) => (T)this[key];
    }
}
