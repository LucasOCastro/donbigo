using DonBigo.Actions;
using DonBigo.Rooms;
using UnityEngine;

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

        private float CalcDoorScore(RoomExit door)
        {
            const float recentDoorWeight = 3f;
            const float fullyExploredPenalty = 10f;
            const float lastVisitedPenalty = 6f;
            
            float score = 100f;
            
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
            
            Debug.Log("visited at "+door.Position+" got score " + score + " with ord = " + visitedOrder + " and fully visited = "+Doer.Memory.RoomFullyExplored(finalRoom));

            return score;
        }
        
        private RoomExit? FindRandomExit()
        {
            var room = Doer.Tile.ParentGrid.RoomAt(Doer.Tile.Pos);
            return room.Doors.RandomElementByWeight(CalcDoorScore);
        }

        public override Action Tick()
        {
            if (_bestExit == null)
            {
                _bestExit = FindRandomExit();
                if (_bestExit == null) Debug.Log("still null");
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