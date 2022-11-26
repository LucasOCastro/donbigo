using UnityEngine;
using UnityEngine.Tilemaps;

namespace DonBigo
{
    [ScriptableObjectIcon("sprite")]
    public class TileType : UnityEngine.Tilemaps.Tile
    {
        public const int FloorHeight = 0;
        public const int WallHeight = 2;

        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {
            base.GetTileData(position, tilemap, ref tileData);
            if (GridManager.DEBUG_drawVis)
            {
                tileData.color = ShadowCasting.visibleTiles.Contains((Vector2Int)position) ? Color.green : Color.red;
            }
        }

        //Informações sobre som de pegada, etc
    }
}