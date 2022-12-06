using System;
using System.Collections.Generic;
using System.Linq;

namespace DonBigo
{
    public static class CollectionUtility
    {
        public static T Random<T>(this IList<T> col, bool throwOnEmpty = false)
        {
            int count = col.Count;
            if (count == 0)
            {
                return !throwOnEmpty ? default : throw new InvalidOperationException("Sequence was empty");
            }

            return col[UnityEngine.Random.Range(0, count)];
        }
        
        //https://stackoverflow.com/a/648240
        public static T Random<T>(this IEnumerable<T> col, bool throwOnEmpty = false)
        {
            T current = default;
            int count = 0;
            foreach (T element in col)
            {
                count++;
                if (UnityEngine.Random.Range(0, count) == 0)
                {
                    current = element;
                }            
            }
            if (count == 0)
            {
                return !throwOnEmpty ? default : throw new InvalidOperationException("Sequence was empty");
            }
            return current;
        }
    }
}