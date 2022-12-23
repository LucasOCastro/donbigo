using System.Linq;

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
        
        protected override AIState OnTick(Entity entity, out AIObjective objective)
        {
            if (ShouldBeAlerted(entity))
            {
                objective = null;
                return new AlertedState();
            }
            
            Item targetItem = FindItemToPickup(entity, out var handedness);
            if (targetItem != null)
            {
                objective = new PickupItemObjective(entity, targetItem, handedness);
                return null;
            }
            
            /*if (CurrentObjective == null)
            {
                objective = new FollowObjective(worker.Owner, CharacterManager.DonBigo);
            }*/
            objective = null;
            return null;
        }
    }
}