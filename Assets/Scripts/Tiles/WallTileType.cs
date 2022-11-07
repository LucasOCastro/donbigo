using UnityEngine;
using UnityEngine.Tilemaps;

namespace DonBigo
{
    public class WallTileType : TileType
    {
        private const float TransparencyMultiplier = 0.1f;

        private bool ShouldBeTransparent(Vector2Int pos)
        {
            Vector2Int referenceTile = FieldOfViewRenderer.Instance.OriginTile;
            //Apenas no eixo y do tilemap
            //return pos.y < referenceTile.y;
            
            //Apenas paredes minimas do comado
            var room = GridManager.Instance.Grid.RoomAt(referenceTile);
            if (room == null) {
                return false;
            }
            
            if (!room.Bounds.Contains(pos)) {
                return true;
            }
            //Não é transparente se for as ultimas paredes do comodo
            if (pos.x == room.Bounds.xMax - 1 || pos.y == room.Bounds.yMax - 1) {
                return false;
            }
            
            return pos.y == room.Bounds.yMin || pos.x == room.Bounds.xMin;
            
            //No eixo y global
            /*Vector3 referencePos = GridManager.Instance.Grid.TileToWorld(referenceTile);
            Vector3 worldPos = GridManager.Instance.Grid.TileToWorld(pos);
            return worldPos.y < referencePos.y;*/
        }
    
        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {
            base.GetTileData(position, tilemap, ref tileData);
            if (GridManager.Instance == null || GridManager.Instance.Grid == null)
            {
                return;
            }

            if (ShouldBeTransparent((Vector2Int)position))
            {
                var tileDataColor = tileData.color;
                tileDataColor.a *= TransparencyMultiplier;
                tileData.color = tileDataColor;
            }
        }
    }
}