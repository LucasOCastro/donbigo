using DonBigo.Rooms;
using UnityEngine;

namespace DonBigo
{
    //Isso seria facimente evitado se fosse uma classe gnfsklgmdfl
    public static class RoomExitUtility
    {
        public static Tile FindWalkable(Vector2Int tile, Vector2Int direction, GameGrid grid)
        {
            while (grid.InBounds(tile) && grid[tile] != null)
            {
                if (grid[tile].Walkable && grid[tile].Entity == null) return grid[tile];
                tile += direction;
            }
            return null;
        }
        
        public static RoomInstance FinalRoom(this RoomExit exit, GameGrid grid)
        {
            Tile finalTile = exit.FinalTile(grid);
            return finalTile?.Room;
        }
    }
}