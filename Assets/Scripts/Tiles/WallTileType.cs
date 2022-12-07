
using UnityEngine;

namespace DonBigo
{
    public class WallTileType : TileType
    {
        protected override Color GetColor(Vector2Int position, Color baseColor)
        {
            if (!FieldOfViewRenderer.DEBUG_drawVis)
            {
                return baseColor;
            }

            return FieldOfViewRenderer.VisibleTiles.Contains(position) ? baseColor : new Color(0, 0, 0, 0);
        }
    }
}