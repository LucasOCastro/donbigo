using UnityEngine;
using UnityEngine.Tilemaps;

namespace DonBigo
{
    public class WallTileType : TileType
    {
        private const float TransparencyMultiplier = 0.1f;
    
        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {
            base.GetTileData(position, tilemap, ref tileData);
            if (GridManager.Instance == null || GridManager.Instance.Grid == null)
            {
                return;
            }

            Vector2Int referenceTile = FieldOfViewRenderer.Instance.OriginTile;
            //Apenas no eixo y do tilemap
            /*if (position.y < referenceTile.y)
            {
                var tileDataColor = tileData.color;
                tileDataColor.a *= TransparencyMultiplier;
                tileData.color = tileDataColor;
            }*/
            
            //No eixo y global
            Vector3 referencePos = GridManager.Instance.Grid.TileToWorld(referenceTile);
            Vector3 worldPos = GridManager.Instance.Grid.TileToWorld((Vector2Int)position);
            if (worldPos.y < referencePos.y)
            {
                var tileDataColor = tileData.color;
                tileDataColor.a *= TransparencyMultiplier;
                tileData.color = tileDataColor;
            }
        }
    }
}