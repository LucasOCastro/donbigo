using UnityEngine;
using UnityEngine.Tilemaps;

namespace DonBigo
{
    public class DoorTileType : WallTileType
    {
        //TODO armazena informações sobre estado de abertura, trancado, mudança de sprite, etc
        [SerializeField] private WallTileType inactiveSubstitute;

        public WallTileType InactiveSubstitute => inactiveSubstitute;

        public void SetInactiveDoor(GameGrid grid, Tilemap tilemap, Vector2Int pos)
        {
            tilemap.SetTile(new Vector3Int(pos.x, pos.y, WallHeight), inactiveSubstitute);
            grid[pos] = new Tile(pos, inactiveSubstitute, grid);
        }
    }
}