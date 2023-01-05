using System.Collections.Generic;
using UnityEngine;

namespace DonBigo
{
    public static class UtilVec2Int
    {
        public const int StraightCost = 10, DiagonalCost = 14;
        
        //O Sign do Unity é BURRO e retorna 1 para 0.
        private static int Sign(int x) => (x > 0) ? 1 : ((x < 0) ? -1 : 0);
        
        public static Vector2Int Abs(this Vector2Int vec) => new(Mathf.Abs(vec.x), Mathf.Abs(vec.y));
        public static Vector2Int Sign(this Vector2Int vec) => new(Sign(vec.x), Sign(vec.y));
        
        public static bool AdjacentTo(this Vector2Int a, Vector2Int b)
        {
            Vector2Int absDif = (b - a).Abs();
            return absDif.x < 2 && absDif.y < 2;
        }

        public static IEnumerable<Vector2Int> Neighbors(this Vector2Int vec)
        {
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0) continue;
                    yield return vec + new Vector2Int(x, y);
                }
            }
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