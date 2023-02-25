using System.Linq;
using UnityEngine;

namespace DonBigo.AI
{
    public class WanderState : AIState
    {
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
            
            objective = new WanderDoorObjective(worker);
            return null;
        }
    }
}