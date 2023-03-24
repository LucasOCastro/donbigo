using UnityEngine;
using UnityEngine.Tilemaps;

namespace DonBigo
{
    public class DoorTileType : WallTileType, IRoomEntranceMarker
    {
        [SerializeField] private WallTileType inactiveSubstitute;

        public void SetInactive(GameGrid grid, Tilemap tilemap, Vector2Int pos)
        {
            tilemap.SetTile(new Vector3Int(pos.x, pos.y, WallHeight), inactiveSubstitute);
            grid[pos] = (inactiveSubstitute != null) ? new Tile(pos, inactiveSubstitute, grid, grid[pos].Room) : null;
        }
    }
}