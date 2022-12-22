using DonBigo.Actions;
using DonBigo.Rooms;

namespace DonBigo.AI
{
    public class FleeObjective : GoToTargetObjective
    {
        private ITileGiver _fleeFrom;
        private RoomExit? _targetExit;
        private Path _targetPath;
        public FleeObjective(Entity doer, ITileGiver target) : base(doer, null)
        {
            _fleeFrom = target;
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
            if (_targetExit != null && _targetExit.Value.Position.AdjacentTo(Doer.Tile.Pos))
            {
                return new UseDoorAction(Doer, _targetExit.Value);
            }
                
            RoomExit? bestExit = GetBestExit();
            if (_targetExit == null && bestExit == null)
            {
                return null;
            }
                
            if (bestExit != null && (_targetExit == null || _targetExit.Value.Position != bestExit.Value.Position))
            {
                _targetExit = bestExit;
                //objective = new FollowObjective(entity, entity.Tile.ParentGrid[bestExit.Value.Position]);
            }
            return base.Tick();
        }
    }
}