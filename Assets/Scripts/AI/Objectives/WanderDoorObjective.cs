using DonBigo.Actions;
using DonBigo.Rooms;
using UnityEngine;

namespace DonBigo.AI
{
    public class WanderDoorObjective : GoToTargetObjective
    {
        private IRoomExit _bestExit;
        public WanderDoorObjective(AIWorker worker) : base(worker, null)
        {
        }

        private bool _completed;
        public override bool Completed => _completed;

        private float CalcDoorScore(IRoomExit exit)
        {
            const float recentDoorWeight = 5f;
            const float fullyExploredPenalty = 30f;
            const float lastVisitedPenalty = 800f;

            const float distanceFromPlayerStrongWeight = 20f;
            const float distanceFromPlayerWeakWeight = 100f;

            const float ventChanceMultiplier = 0.75f;
            
            float score = 1000f;

            if (exit is RoomExit door)
            {
                RoomInstance finalRoom = door.FinalRoom(Doer.Tile.ParentGrid);
                int visitedOrder = Doer.Memory.RoomVisitedOrder(finalRoom);
                if (visitedOrder > 0)
                {
                    if (visitedOrder == 1) score -= lastVisitedPenalty;
                    else score -= recentDoorWeight / visitedOrder;
                }

                if (Doer.Memory.RoomFullyExplored(finalRoom))
                {
                    score -= fullyExploredPenalty;
                }
                
                Tile lastSeenPlayerTile = Doer.Memory.LastSeenTile(CharacterManager.DonBigo);
                if (lastSeenPlayerTile != null)
                {
                    var grid = Doer.Tile.ParentGrid;
                    if (door.FinalRoom(grid) == lastSeenPlayerTile.Room)
                    {
                        score += Worker.FeelsStrong ? distanceFromPlayerStrongWeight : -distanceFromPlayerWeakWeight;
                    }
                }
            }
            else if (exit is Vent)
            {
                score *= ventChanceMultiplier;
            }

            return score;
        }

        public override Action Tick()
        {
            if (_bestExit == null)
            {
                _bestExit = Doer.Tile.Room.GetExit(CalcDoorScore, null, true, true);
                if (_bestExit == null) Debug.Log("still null");
                if (_bestExit == null) return null;
                _target = Doer.Tile.ParentGrid[_bestExit.Position];
            }

            if (!IsAdjacentToTarget)
            {
                return base.Tick();
            }
            
            var action = _bestExit.GenAction(Doer);
            _bestExit = null;
            _target = null;
            _completed = true;
            return action;
        }
    }
}