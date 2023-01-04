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
        
        [field: SerializeField] public bool Walkable { get; private set; } = true;

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

            if (FieldOfViewRenderer.IsVisible(position))
            {
                return baseColor;
            }

            var grid = GridManager.Instance.Grid;
            if (FieldOfViewRenderer.Origin != null &&
                grid.RoomAt(position) != grid.RoomAt(FieldOfViewRenderer.Origin.Tile.Pos))
            {
                baseColor.a = 0;
                return baseColor;
            }
            //baseColor.a = 0;
            return Color.black;
        }
        
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

            tileData.color = GetColor((Vector2Int)position, tileData.color);
        }

        //Informações sobre som de pegada, etc
    }
}