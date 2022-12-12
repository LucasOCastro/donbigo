using UnityEngine;

namespace DonBigo
{
    public static class UtilVec2Int
    {
        public static Vector2Int Abs(this Vector2Int vec) => new Vector2Int(Mathf.Abs(vec.x), Mathf.Abs(vec.y));
        
        public static bool AdjacentTo(this Vector2Int a, Vector2Int b)
        {
            Vector2Int absDif = (b - a).Abs();
            return absDif.x < 2 && absDif.y < 2;
        }
    }
}