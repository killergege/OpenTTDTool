using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTTDTool
{
    public static class IDictionaryExtension
    {
        public static void SafeAdd<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue value )
        {
            if (!dict.ContainsKey(key))
                dict.Add(key, value);
        }
    }
}
