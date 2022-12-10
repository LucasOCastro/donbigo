﻿using System;
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
        
        //https://stackoverflow.com/questions/56692/random-weighted-choice
        public static T RandomElementByWeight<T>(this IEnumerable<T> sequence, Func<T, float> weightSelector) 
        {
            float totalWeight = sequence.Sum(weightSelector);
            float itemWeightIndex =  (float)new Random().NextDouble() * totalWeight;
            float currentWeightIndex = 0;

            foreach(var item in from weightedItem in sequence select new { Value = weightedItem, Weight = weightSelector(weightedItem) }) {
                currentWeightIndex += item.Weight;
            
                // If we've hit or passed the weight we are after for this item then it's the one we want....
                if(currentWeightIndex >= itemWeightIndex)
                    return item.Value;
            
            }
        
            return default(T);
        
        }
    }
}