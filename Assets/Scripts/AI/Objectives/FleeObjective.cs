using System.Linq;
using DonBigo.Actions;
using DonBigo.Rooms;
using UnityEngine;

namespace DonBigo.AI
{
    public class FleeObjective : GoToTargetObjective
    {
        private Entity _fleeFrom;
        private RoomExit? _targetExit;
        private Path _targetPath;
        private float _fightBackChance;

        private static int CalcExtraTileCost(Tile from, Tile to, Entity fleeFrom)
        {
            /*Vector2 dir = ((Vector2)(to.Pos - from.Pos)).normalized;
            Vector2 dirToFlee = ((Vector2)(fleeFrom.Tile.Pos - from.Pos)).normalized;
            return Vector2.Dot(dir, dirToFlee);*/
            
            return to.Pos.ManhattanDistance(fleeFrom.Tile.Pos);
        }

        public FleeObjective(Entity doer, Entity fleeFrom, float fightBackChance) : base(doer, null,
            (t1, t2) => CalcExtraTileCost(t1, t2, fleeFrom))
        {
            _fleeFrom = fleeFrom;
            _fightBackChance = fightBackChance;
        }

        public override bool Completed => !Doer.VisibleTiles.Contains(_fleeFrom.Tile.Pos);

        private float CalcDoorScore(RoomExit door)
        {
            Vector2Int doorPos = door.Position;
            const float distanceToDoerWeight = 1;
            const float distanceToTargetWeight = 3;
            const float deadEndPenalty = 200;

            int distanceToDoer = Doer.Tile.Pos.ManhattanDistance(doorPos);
            int distanceToTarget = _fleeFrom.Tile.Pos.ManhattanDistance(doorPos);
            //return (1f / distanceToDoer) - 3 * (1f / distanceToTarget);
            
            float score = 100f + (distanceToDoerWeight * -distanceToDoer) + (distanceToTargetWeight * distanceToTarget);

            GameGrid grid = Doer.Tile.ParentGrid;
            RoomInstance finalRoom = door.FinalRoom(grid);
            if (Doer.Memory.IsDeadEnd(finalRoom))
            {
                score -= deadEndPenalty;
            }

            return score;
        }
        private RoomExit? GetBestExit()
        {
            var currentRoom = Doer.Tile.ParentGrid.RoomAt(Doer.Tile.Pos);

            RoomExit? bestExit = null;
            float bestScore = 0;
            foreach (var exit in currentRoom.Doors)
            {
                if (exit.Position.AdjacentTo(Doer.Tile.Pos)) return exit;
                
                float score = CalcDoorScore(exit);
                if (bestExit == null || score > bestScore)
                {
                    bestExit = exit;
                    bestScore = score;
                }
            }

            return bestExit;
        }

        private static int ItemDefensivenessScore(Item item)
        {
            return item.Type.WeaponType.Score(t => t switch
            {
                WeaponUseType.Ranged => 2,
                WeaponUseType.Trap => 1,
                _ => 0
            });
        }
        
        //TODO meud eus cara que método HORRIVEL arruma isso ae pleo amor de deusss
        private UseItemAction GetDefensiveFleeAction()
        {
            if (_fleeFrom.Health.HasStatusOfType<StunStatus>()) return null;
            
            var inventory = Doer.Inventory;

            var possibleHandednesses =
                Inventory.AllHandednesses
                    .Where(h => inventory.GetHand(h) != null
                                && inventory.GetHand(h).Type.WeaponType.HasFlag(WeaponUseType.Defensive)
                                && inventory.GetHand(h).GetAttackTile(Doer, _fleeFrom) != null)
                    .ToArray();
            if (!possibleHandednesses.Any()) return null;

            var chosenHandedness = possibleHandednesses
                .RandomElementByWeight(h => ItemDefensivenessScore(inventory.GetHand(h)));
            
            
            Item item = inventory.GetHand(chosenHandedness);
            Tile targetTile = item.GetAttackTile(Doer, _fleeFrom);
            return new UseItemAction(Doer, item, targetTile);
        }
            
        public override Action Tick()
        {
            if (_targetExit != null && IsAdjacentToTarget)
            {
                return new UseDoorAction(Doer, _targetExit.Value);
            }

            if (Random.value < _fightBackChance)
            {
                Action fightBackAction = GetDefensiveFleeAction();
                if (fightBackAction != null) return fightBackAction;
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