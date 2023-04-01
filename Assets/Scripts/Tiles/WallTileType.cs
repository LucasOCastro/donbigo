
using UnityEngine;
using UnityEngine.Tilemaps;

namespace DonBigo
{
    public class WallTileType : TileType
    {
        private static Color _transparent = new Color(0, 0, 0, 0);

        public bool HideToNotObstructView(Vector2Int position)
        {
            if (GridManager.Instance == null || GridManager.Instance.Grid == null ||
                FieldOfViewRenderer.Origin == null) return false;
            
            var origin = FieldOfViewRenderer.Origin.Tile.Pos;
            bool mayBlockOrigin = position.y < origin.y;
            bool border = position.x == GridManager.Instance.Grid[position].Room.Bounds.xMax - 1;
            return mayBlockOrigin && !border;
        }
        
        protected override Color GetColor(Vector2Int position, Color baseColor, ITilemap tilemap)
        {
#if UNITY_EDITOR
            if (!FieldOfViewRenderer.DEBUG_drawVis)
            {
                return baseColor;
            }
#endif

            if (FieldOfViewRenderer.Origin == null) return baseColor;

            var bounds = FieldOfViewRenderer.Origin.Tile.Room.Bounds;
            if (!bounds.Contains(position) || HideToNotObstructView(position)) return _transparent;
            return FieldOfViewRenderer.IsVisible(position) ? baseColor : FieldOfViewRenderer.HiddenOverlayColor;            
        }
    }
}