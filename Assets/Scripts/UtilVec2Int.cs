using UnityEngine;

namespace DonBigo
{
    public static class UtilVec2Int
    {
        public const int StraightCost = 10, DiagonalCost = 14;
        
        public static Vector2Int Abs(this Vector2Int vec) => new Vector2Int(Mathf.Abs(vec.x), Mathf.Abs(vec.y));
        
        public static bool AdjacentTo(this Vector2Int a, Vector2Int b)
        {
            Vector2Int absDif = (b - a).Abs();
            return absDif.x < 2 && absDif.y < 2;
        }
        
        //http://theory.stanford.edu/~amitp/GameProgramming/Heuristics.html
        //Mistura de Manhattan Distance com Chebyshev e Octile, para permitir movimentos diagonais.
        public static int ManhattanDistance(this Vector2Int a, Vector2Int b)
        {
            int dx = Mathf.Abs(a.x - b.x);
            int dy = Mathf.Abs(a.y - b.y);
            return StraightCost * (dx + dy) + (DiagonalCost - 2 * StraightCost) * Mathf.Min(dx, dy);
        }
    }
}