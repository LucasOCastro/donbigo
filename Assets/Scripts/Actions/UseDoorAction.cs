using DonBigo;
using DonBigo.Rooms;
using UnityEngine;

namespace DonBigo.Actions
{
    public class UseDoorAction : MoveAction
    {
        private static Tile GetFinalDoorPosition(RoomExit door, GameGrid grid)
        {
            Vector2Int tile = door.Position + door.DirectionVector;
            while (grid.InBounds(tile) && grid[tile] != null)
            {
                if (grid[tile].Walkable)
                {
                    return grid[tile];
                }
                tile += door.DirectionVector;
            }
            return null;
        }
        
        private Tile _target;
        public UseDoorAction(Entity doer, RoomExit door) : base(doer, GetFinalDoorPosition(door, doer.Tile.ParentGrid))
        {
        }
    }
}