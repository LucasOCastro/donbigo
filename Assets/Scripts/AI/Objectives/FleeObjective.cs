using DonBigo.Actions;
using DonBigo.Rooms;
using UnityEngine;

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

        public override bool Completed => !Doer.VisibleTiles.Contains(_fleeFrom.Tile.Pos);

        private float CalcDoorScore(Vector2Int doorPos)
        {
            const float distanceToDoerWeight = 1;
            const float distanceToTargetWeight = 3;
            
            int distanceToDoer = Doer.Tile.Pos.ManhattanDistance(doorPos);
            int distanceToTarget = _fleeFrom.Tile.Pos.ManhattanDistance(doorPos);
            //return (1f / distanceToDoer) - 3 * (1f / distanceToTarget);
            return (distanceToDoerWeight * -distanceToDoer) + (distanceToTargetWeight * distanceToTarget);
        }
        private RoomExit? GetBestExit()
        {
            var currentRoom = Doer.Tile.ParentGrid.RoomAt(Doer.Tile.Pos);

            RoomExit? bestExit = null;
            float bestScore = 0;
            foreach (var exit in currentRoom.Doors)
            {
                if (exit.Position.AdjacentTo(Doer.Tile.Pos)) return exit;
                
                float score = CalcDoorScore(exit.Position);
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
            if (IsAdjacentToTarget)
            {
                return new UseDoorAction(Doer, _targetExit.Value);
            }
            return base.Tick();
        }
    }
}