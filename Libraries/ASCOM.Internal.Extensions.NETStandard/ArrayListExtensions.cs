
using ASCOM.Utilities.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ASCOM.Internal
{
    public static class ArrayListExtensions
    {
        public static bool CanConvert<T>(this ArrayList alist)
        {
            bool result=false;
            int count = alist?.Count ?? 0;
            if (count == 0)
                return true;
            try
            {
                Array arr = alist.ToArray(typeof(T));
                result = (arr.Length == count);
            }
            catch { }
            return result;
        }

        public static List<T> ToList<T>(this ArrayList alist)
        {
            if (!alist.CanConvert<T>())
                return null;
            List<T> result= new List<T>();
            foreach (T item in alist.ToArray(typeof(T)))
                result.Add(item);
            return result;
        }


        public static List<KeyValuePair<string,string>> ToKvps(this ArrayList alist)
        {
            return alist?
                .Cast<IKeyValuePair>()
                .Select(t=>new KeyValuePair<string, string>(t.Key,t.Value))
                .ToList();
        }

        public static List<String> ToStrings(this ArrayList alist)
        {
            return alist?.ToList<String>();
        }

        public static Dictionary<String, String> ToDictionary(this ArrayList alist)
        {
            var kvps = alist?.ToKvps();
            if (kvps == null)
                return null;
            Dictionary<String, String> result =new Dictionary<String,String>();
            foreach (var item in kvps)
            {
                if (!result.ContainsKey(item.Key))
                    result.Add(item.Key,item.Value);
                else
                {
                    UInt32 dupId = 1;
                    String key;
                    do
                    {
                        key = $"{item.Key}_{dupId++}";
                    } while (result.ContainsKey(key));
                    result[key] = item.Value;
                }
            }
            return result;
        }

        public static void AddRange(this ArrayList alist, IEnumerable<object> items)
        {
            alist.AddRange(items.ToList());
        }

        public static ArrayList Set(this ArrayList alist, IEnumerable<object> items)
        {
            alist.Clear();
            alist.AddRange(items);
            return alist;
        }
    }
}
