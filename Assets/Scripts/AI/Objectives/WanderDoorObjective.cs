using DonBigo.Actions;
using DonBigo.Rooms;

namespace DonBigo.AI
{
    public class WanderDoorObjective : GoToTargetObjective
    {
        private RoomExit? _bestExit;
        public WanderDoorObjective(Entity doer) : base(doer, null)
        {
        }

        private bool _completed;
        public override bool Completed => _completed; 

        private static RoomExit? FindRandomExit(Entity entity)
        {
            var room = entity.Tile.ParentGrid.RoomAt(entity.Tile.Pos);
            return (room.Doors.Count > 0) ? room.Doors.Random() : null;
        }

        public override Action Tick()
        {
            if (_bestExit == null)
            {
                _bestExit = FindRandomExit(Doer);
                if (_bestExit == null) return null;
                _target = Doer.Tile.ParentGrid[_bestExit.Value.Position];
            }

            if (!IsAdjacentToTarget)
            {
                return base.Tick();
            }
            
            var action = new UseDoorAction(Doer, _bestExit.Value);
            _bestExit = null;
            _target = null;
            _completed = true;
            return action;
        }
    }
}