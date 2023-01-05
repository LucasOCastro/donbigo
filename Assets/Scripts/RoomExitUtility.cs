using DonBigo.Rooms;
using UnityEngine;

namespace DonBigo
{
    //Isso seria facimente evitado se fosse uma classe gnfsklgmdfl
    public static class RoomExitUtility
    {
        private static Tile FindWalkable(Vector2Int tile, Vector2Int direction, GameGrid grid)
        {
            while (grid.InBounds(tile) && grid[tile] != null)
            {
                if (grid[tile].Walkable && grid[tile].Entity == null) return grid[tile];
                tile += direction;
            }
            return null;
        }
        
        public static Tile UseTile(this RoomExit exit, GameGrid grid)
        {
            return FindWalkable(exit.Position, -exit.DirectionVector, grid);
        }

        public static Tile FinalTile(this RoomExit exit, GameGrid grid)
        {
            return FindWalkable(exit.Position + exit.DirectionVector, exit.DirectionVector, grid);
        }

        public static RoomInstance FinalRoom(this RoomExit exit, GameGrid grid)
        {
            Tile finalTile = exit.FinalTile(grid);
            return finalTile?.Room;
        }
    }
}