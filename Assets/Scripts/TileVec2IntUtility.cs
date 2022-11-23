using UnityEngine;

namespace DonBigo
{
    public static class TileVec2IntUtility
    {
        public static Vector2 BottomLeft(this Vector2Int vec, float size = 1) => vec;
        public static Vector2 BottomRight(this Vector2Int vec, float size = 1) => vec + Vector2.right * size;
        public static Vector2 TopLeft(this Vector2Int vec, float size = 1) => vec + Vector2.up * size;
        public static Vector2 TopRight(this Vector2Int vec, float size = 1) =>
            vec + Vector2.up * size + Vector2.right * size;
        public static Vector2 Center(this Vector2Int vec, float size = 1) =>
            vec + Vector2.up * (size * .5f) + Vector2.right * (size * .5f);

        //Provavelmente dá pra refatorar de modo matematico ao invés de condicional
        public static (Vector2 close, Vector2 far) GetCloseFarVertex(Vector2Int source, Vector2Int tile, bool startHorizontal)
        {
            Vector2Int increment = tile - source;
            if (startHorizontal)
            {
                if (increment.x < 0)
                {
                    return increment.y > 0
                        ? (tile.BottomRight(), tile.TopRight()) //<^
                        : (tile.TopRight(), tile.BottomRight()); //<v
                }
                return increment.y > 0
                    ? (tile.BottomLeft(), tile.TopLeft()) //>^
                    : (tile.TopLeft(), tile.BottomLeft()); //>v
            }

            if (increment.y < 0)
            {
                return increment.x > 0
                    ? (tile.TopLeft(), tile.TopRight()) //v>
                    : (tile.TopRight(), tile.TopLeft()); //v<
            }
            return increment.x > 0
                ? (tile.BottomLeft(), tile.BottomRight()) //^>
                : (tile.BottomRight(), tile.BottomLeft()); //^<
        }
    }
}