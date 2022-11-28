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

        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {
            base.GetTileData(position, tilemap, ref tileData);

            if (GridManager.DEBUG_start != null && GridManager.DEBUG_start.Pos == (Vector2Int)position)
            {
                tileData.color = Color.green;
            }
            else if (GridManager.DEBUG_end != null && GridManager.DEBUG_end.Pos == (Vector2Int)position)
            {
                tileData.color = Color.red;
            }
            else if (GridManager.DEBUG_pathTiles.Contains((Vector2Int)position))
            {
                tileData.color = Color.yellow;
            }
            
            
            //DEBUG
            if (!FieldOfViewRenderer.DEBUG_drawVis) return;

            if (!FieldOfViewRenderer.VisibleTiles.Contains((Vector2Int)position))
            {
                tileData.color = Color.black;
            }
        }

        //Informações sobre som de pegada, etc
    }
}