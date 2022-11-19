using UnityEngine;
using UnityEngine.Tilemaps;

namespace DonBigo
{
    public class EntranceMarkerTile : StructureTileType, IRoomEntranceMarker
    {
        public void SetInactive(GameGrid grid, Tilemap tilemap, Vector2Int pos)
        {
            grid[pos].Structures.Remove(this);
            //Achar a elevação correta é meio estranho, isso é plausivel pra refatoração na geração.
            for (int z = 0; z <= tilemap.size.z; z++)
            {
                Vector3Int tilePos = new Vector3Int(pos.x, pos.y, z);
                if (tilemap.GetTile<TileBase>(tilePos) is EntranceMarkerTile)
                {
                    tilemap.SetTile(new Vector3Int(pos.x, pos.y, z), null);
                    break;
                }
            }
        }
    }
}