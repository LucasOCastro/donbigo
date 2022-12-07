using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

namespace DonBigo
{
    [ScriptableObjectIcon("sprite")]
    public class StructureTileType : UnityEngine.Tilemaps.Tile
    {
        [field: Tooltip("A altura mínima no tilemap pra impedir a visão. Valor negativo nunca impede.")]
        [field: SerializeField]
        public int ViewBlockHeight { get; private set; } = -1;
        
        [field: SerializeField]
        public bool BlocksMovement { get; private set; }
        
        [field: Tooltip("A altura em que itens spawnam sobre a estrutura. Valor negativo, nunca spawna item.")]
        [field: SerializeField]
        public int SurfaceHeight { get; private set; }

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