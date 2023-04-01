using System;
using System.Collections.Generic;
using System.Linq;

namespace DonBigo
{
    public static class CollectionUtility
    {
        public static int RangeExcluding(int minInclusive, int maxExclusive, int excluding)
        {
            int[] range = new int[maxExclusive - minInclusive - 1];
            int index = 0;
            for (int x = minInclusive; x < maxExclusive; x++)
            {
                if (x == excluding) continue;
                range[index] = x;
                index++;
            }
            return range.Random();
            //return UnityEngine.Random.Range(excluding + 1, maxExclusive + (maxExclusive - minInclusive))  % maxExclusive + minInclusive;
        }
        
        public static int RandomIndex<T>(this IList<T> col, int excluding = -1, bool throwOnEmpty = false)
        {
            int count = col.Count;
            if (count == 0)
            {
                return !throwOnEmpty ? default : throw new InvalidOperationException("Sequence was empty");
            }

            return excluding >= 0 
                ? RangeExcluding(0, count, excluding) 
                : UnityEngine.Random.Range(0, count);
        }

        public static T Random<T>(this IList<T> col, int excluding = -1, bool throwOnEmpty = false) =>
            col[col.RandomIndex(excluding, throwOnEmpty)];

        //https://stackoverflow.com/a/648240
        public static T Random<T>(this IEnumerable<T> col, bool throwOnEmpty = false)
        {
            if (col is IList<T> list) return Random(list, throwOnEmpty);
            
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
        
        //https://stackoverflow.com/questions/56692/random-weighted-choice
        public static T RandomElementByWeight<T>(this IEnumerable<T> sequence, Func<T, float> weightSelector) 
        {
            float totalWeight = sequence.Sum(weightSelector);
            float itemWeightIndex =  UnityEngine.Random.value * totalWeight;
            float currentWeightIndex = 0;

            foreach(var item in from weightedItem in sequence select new { Value = weightedItem, Weight = weightSelector(weightedItem) }) {
                currentWeightIndex += item.Weight;
            
                // If we've hit or passed the weight we are after for this item then it's the one we want....
                if(currentWeightIndex > itemWeightIndex)
                    return item.Value;
            
            }
            return default;
        }

        public static T Best<T>(this IEnumerable<T> col, Func<T, T, bool> isBetter)
        {
            bool found = false;
            T best = default;
            foreach (T t in col)
            {
                if (!found || isBetter(t, best))
                {
                    best = t;
                    found = true;
                }
            }

            return best;
        }

        public static T2 FindOfType<T1, T2>(this IEnumerable<T1> col) where T2 : class, T1 =>
            col.FirstOrDefault(t => t is T2) as T2;

        public static bool MoreThan<T>(this IEnumerable<T> col, Func<T, bool> condition, int moreThan)
        {
            int count = 0;
            foreach (T t in col)
            {
                if (condition(t))
                {
                    count++;
                    if (count > moreThan) return true;
                }
            }

            return false;
        }

        public static void SetOrAdd<T1, T2>(this Dictionary<T1, T2> dict, T1 key, T2 val)
        {
            if (dict.ContainsKey(key)) dict[key] = val;
            else dict.Add(key, val);
        }

        public static T2 GetOrDefault<T1, T2>(this Dictionary<T1, T2> dict, T1 key, T2 def = default)
        {
            return dict.TryGetValue(key, out T2 val) ? val : def;
        }
    }
}