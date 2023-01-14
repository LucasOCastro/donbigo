using System.Linq;
using UnityEngine;

namespace DonBigo.AI
{
    public class WanderState : AIState
    {
        private const float VentChanceMultiplier = 0.8f;
        
        private static bool ShouldBeAlerted(Entity entity)
        {
            //Mais outros gatilhos
            return entity.SeesPlayer;
        }
        
        private static Item FindItemToPickup(Entity entity, out Inventory.Handedness handedness)
        {
            handedness = entity.Inventory.WeakestHandedness;
            Item minPowerItem = entity.Inventory.GetHand(handedness);
            int minPower = (minPowerItem != null) ? minPowerItem.Type.CombatPower : -1;

            Item strongestVisibleItem = null;
            foreach (var tile in entity.VisibleTiles)
            {
                var item = entity.Tile.ParentGrid[tile].Item;
                if (item == null || !item.CanBePickedUp) continue;
                
                int power = item.Type.CombatPower; 
                if (power > minPower)
                {
                    strongestVisibleItem = item;
                    minPower = power;
                }
            }

            return strongestVisibleItem;
        }

        private Vent _targetVent;
        protected override AIState OnTick(AIWorker worker, out AIObjective objective)
        {
            var entity = worker.Owner;
            var room = entity.Tile.Room;
            if (ShouldBeAlerted(entity))
            {
                objective = null;
                return new AlertedState();
            }

            if (CurrentObjective is { Completed: false })
            {
                //TODO honestamente só mudar o CurrentObjective diretamente faz mais sentido.
                objective = CurrentObjective;
                return null;
            }

            if (CurrentObjective is { Completed: true } && _targetVent != null)
            {
                Debug.Log("entered vent");
                objective = null;
                return new VentingState(_targetVent);
            }

            var closedVent = room.Vents.FirstOrDefault(v => !v.Open);
            if (closedVent != null)
            {
                Debug.Log("will open vent");
                objective = new OpenVentObjective(worker, closedVent);
                return null;
            }
            
            Item targetItem = FindItemToPickup(entity, out var handedness);
            if (targetItem != null)
            {
                objective = new PickupItemObjective(worker, targetItem, handedness);
                return null;
            }

            
            var openVents = room.Vents.Where(v => v.Open);
            int openVentCount = openVents.Count();
            if (openVentCount > 0)
            {
                float ventChance = (openVentCount * VentChanceMultiplier) / (room.Doors.Count + openVentCount);
                if (entity.Tile.ParentGrid.CanUseVents && Random.value < ventChance)
                {
                    var vent = openVents.Random();
                    objective = new GoToTargetObjective(worker, vent.Tile);
                    _targetVent = vent;
                    return null;
                }
            }

            objective = new WanderDoorObjective(worker);
            return null;
        }
    }
}