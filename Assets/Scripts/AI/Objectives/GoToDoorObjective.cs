using DonBigo.Actions;
using DonBigo.Rooms;
using UnityEngine;

namespace DonBigo.AI
{
    public class GoToDoorObjective : GoToTargetObjective
    {
        private RoomExit _door;
        public GoToDoorObjective(Entity doer, RoomExit door) : base(doer, doer.Tile.ParentGrid[door.Position])
        {
            _door = door;
        }

        private bool _completed;
        public override bool Completed => _completed;

        public override Action Tick()
        {
            Debug.Log("is supposed to go to dor at "+_door.Position+" with target as "+ _target.Tile.Pos +" and am at "+Doer.Tile.Pos);
            if (IsAdjacentToTarget)
            {
                _completed = true;
                return new UseDoorAction(Doer, _door);
            }
            return base.Tick();
        }
    }
}