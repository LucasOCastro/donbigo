using System.Linq;
using DonBigo.Actions;
using DonBigo.Rooms;
using Unity.VisualScripting;
using UnityEngine;

namespace DonBigo.AI
{
    public class FleeObjective : GoToTargetObjective
    {
        private Entity _fleeFrom;
        private RoomExit? _targetExit;
        private Path _targetPath;
        private float _fightBackChance;
        public FleeObjective(Entity doer, Entity fleeFrom, float fightBackChance) : base(doer, null)
        {
            _fleeFrom = fleeFrom;
            _fightBackChance = fightBackChance;
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