using DonBigo;
using DonBigo.Rooms;
using UnityEngine;

namespace DonBigo.Actions
{
    public class UseDoorAction : MoveAction
    {
        public Tile From { get; }
        public Tile To { get; }
        public RoomExit Exit { get; }
        public UseDoorAction(Entity doer, RoomExit door) : base(doer, door.FinalTile(doer.Tile.ParentGrid))
        {
            From = door.UseTile(doer.Tile.ParentGrid);
            To = door.FinalTile(doer.Tile.ParentGrid);
            Exit = door;
        }
    }
}