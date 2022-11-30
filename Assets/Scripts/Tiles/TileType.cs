using UnityEngine;
using UnityEngine.Tilemaps;

namespace DonBigo
{
    [ScriptableObjectIcon("sprite")]
    public class TileType : UnityEngine.Tilemaps.Tile
    {
        // Alturas rígidas não são uma prática muito boa, mas tá funcionando e é simples então...
        public const int FloorHeight = 0;
        public const int WallHeight = 2;
        
        [field: SerializeField] public virtual bool Walkable { get; private set; } = true;

        protected virtual Color GetColor(Vector2Int position, Color baseColor)
        {
            if (GridManager.DEBUG_start != null && GridManager.DEBUG_start.Pos == position)
            {
                return Color.green;
            }
            if (GridManager.DEBUG_end != null && GridManager.DEBUG_end.Pos == position)
            {
                return Color.red;
            }
            if (GridManager.DEBUG_pathTiles.Contains(position))
            {
                return Color.black;
            }
            
            
            //DEBUG
            if (!FieldOfViewRenderer.DEBUG_drawVis) return baseColor;

            if (!FieldOfViewRenderer.VisibleTiles.Contains(position))
            {
                //baseColor.a = 0;
                return Color.black;
            }

            return baseColor;
        }

        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {
            base.GetTileData(position, tilemap, ref tileData);

            tileData.color = GetColor((Vector2Int)position, tileData.color);
        }

        //Informações sobre som de pegada, etc
    }
}