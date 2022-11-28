using UnityEngine;
using UnityEngine.Tilemaps;

namespace DonBigo
{
    [ScriptableObjectIcon("sprite")]
    public class StructureTileType : UnityEngine.Tilemaps.Tile
    {
        [Tooltip("A altura mínima no tilemap pra impedir a visão. Valor negativo nunca impede.")]
        [SerializeField] private int minViewBlockHeight = -1;
        public int MinViewBlockHeight => minViewBlockHeight;

        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {
            base.GetTileData(position, tilemap, ref tileData);
            
            if (FieldOfViewRenderer.DEBUG_drawVis && !FieldOfViewRenderer.VisibleTiles.Contains((Vector2Int)position))
            {
                Color color = tileData.color;
                color.a = 0;
                tileData.color = color;
            }
        }
    }
}