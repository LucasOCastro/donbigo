using System;

namespace DonBigo
{
    [Serializable]
    public struct FloatRange
    {
        [field: UnityEngine.SerializeField]
        public float Min { get; set; }
        
        [field: UnityEngine.SerializeField]
        public float Max { get; set; }

        public FloatRange(float min, float max)
        {
            if (min > max)
            {
                (min, max) = (max, min);
            }

            Min = min;
            Max = max;
        }

        /// <returns>Random inclusivo de Min a Max</returns>
        public float Random() => UnityEngine.Random.Range(Min, Max);
        public float Average => (Min + Max) * .5f;

        public bool InRange(float x) => Min <= x && x <= Max;
    }
}