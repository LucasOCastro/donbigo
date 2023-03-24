using DonBigo.Actions;
using DonBigo.Rooms;

namespace DonBigo.AI
{
    public class GoToDoorObjective : GoToTargetObjective
    {
        private RoomExit _door;
        public GoToDoorObjective(AIWorker worker, RoomExit door) : base(worker, worker.Owner.Tile.ParentGrid[door.Position])
        {
            _door = door;
        }

        private bool _completed;
        public override bool Completed => _completed;

        public override Action Tick()
        {
            if (IsAdjacentToTarget)
            {
                _completed = true;
                return new UseDoorAction(Doer, _door);
            }
            return base.Tick();
        }
    }
}