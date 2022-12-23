using DonBigo.Actions;
using DonBigo.Rooms;

namespace DonBigo.AI
{
    public class FleeObjective : GoToTargetObjective
    {
        private ITileGiver _fleeFrom;
        private RoomExit? _targetExit;
        private Path _targetPath;
        public FleeObjective(Entity doer, ITileGiver fleeFrom) : base(doer, null)
        {
            _fleeFrom = fleeFrom;
        }

        private RoomExit? GetBestExit()
        {
            var currentRoom = Doer.Tile.ParentGrid.RoomAt(Doer.Tile.Pos);

            RoomExit? bestExit = null;
            float bestScore = 0;
            foreach (var exit in currentRoom.Doors)
            {
                var distanceToDoer = Doer.Tile.Pos.ManhattanDistance(exit.Position);
                var distanceToTarget = _fleeFrom.Tile.Pos.ManhattanDistance(exit.Position);
                float score = (1f / distanceToDoer) - distanceToTarget;
                if (bestExit == null || score > bestScore)
                {
                    bestExit = exit;
                    bestScore = score;
                }
            }

            return bestExit;
        }
            
        public override Action Tick()
        {
            if (_targetExit != null && IsAdjacentToTarget)
            {
                return new UseDoorAction(Doer, _targetExit.Value);
            }
                
            RoomExit? bestExit = GetBestExit();
            if (bestExit == null) return null;
            
            _targetExit = bestExit;
            _target = Doer.Tile.ParentGrid[bestExit.Value.Position];
            return base.Tick();
        }
    }
}