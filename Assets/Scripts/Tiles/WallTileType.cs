
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

            if (FieldOfViewRenderer.Origin == null || FieldOfViewRenderer.IsVisible(position)) return baseColor;

    
            var bounds = FieldOfViewRenderer.Origin.Tile.Room.Bounds;
            return bounds.Contains(position) ? FieldOfViewRenderer.HiddenOverlayColor : new Color(0, 0, 0, 0);
        }
    }
}