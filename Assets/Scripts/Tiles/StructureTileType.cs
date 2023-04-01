using UnityEngine;
using UnityEngine.Tilemaps;

namespace DonBigo
{
    [ScriptableObjectIcon("m_Sprite")]
    public class StructureTileType : UnityEngine.Tilemaps.Tile
    {
        [field: Tooltip("A altura mínima no tilemap pra impedir a visão. Valor negativo nunca impede.")]
        [field: SerializeField]
        public int ViewBlockHeight { get; private set; } = -1;
        
        [field: SerializeField]
        public bool BlocksMovement { get; private set; }

        [field: Tooltip("A altura em que itens spawnam sobre a estrutura. Valor negativo, nunca spawna item.")]
        [field: SerializeField]
        public int SurfaceHeight { get; private set; } = -1;
        
        [SerializeField] private Sprite[] randomSprites;
        private static int RandIndex(Vector3Int pos, int mod)
        {
            int val = (pos.x * pos.y) % 17 + 
                      (pos.y * pos.y * pos.x) % 2 + 
                      (pos.x * pos.x * pos.y) % 5 +
                      (pos.z) % 13;
            return val % mod;
        }

        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {
            base.GetTileData(position, tilemap, ref tileData);
            if (!Application.isPlaying)
            {
                return;
            }
            
            if (randomSprites != null && randomSprites.Length > 0)
            {
                tileData.sprite = randomSprites[RandIndex(position, randomSprites.Length)];
            }

#if UNITY_EDITOR
            if (!FieldOfViewRenderer.DEBUG_drawVis) return;
#endif
            
            if (FieldOfViewRenderer.IsVisible((Vector2Int)position)) return;
            if (GridManager.Instance == null || GridManager.Instance.Grid == null) return;
            
            if (GridManager.Instance.Grid[(Vector2Int)position].Room == FieldOfViewRenderer.Origin.Tile.Room)
            {
                tileData.color = FieldOfViewRenderer.HiddenOverlayColor;
                return;
            }

            Color color = tileData.color;
            color.a = 0;
            tileData.color = color;
        }

        public virtual StructureInstance GetInstance(Tile tile, int elevation)
        {
            return new StructureInstance(this, tile, elevation);
        }
    }
}