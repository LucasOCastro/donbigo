using DonBigo.Rooms;

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

        private RoomExit? FindRandomExit(Entity entity)
        {
            var room = entity.Tile.ParentGrid.RoomAt(entity.Tile.Pos);
            return (room.Doors.Count > 0) ? room.Doors.Random() : null;
        }
        
        protected override AIState OnTick(Entity entity, out AIObjective objective)
        {
            if (ShouldBeAlerted(entity))
            {
                objective = null;
                return new AlertedState();
            }

            if (CurrentObjective != null && !CurrentObjective.Completed)
            {
                //TODO honestamente só mudar o CurrentObjective diretamente faz mais sentido.
                objective = CurrentObjective;
                return null;
            }
            
            Item targetItem = FindItemToPickup(entity, out var handedness);
            if (targetItem != null)
            {
                objective = new PickupItemObjective(entity, targetItem, handedness);
                return null;
            }

            RoomExit? randomExit = FindRandomExit(entity);
            if (randomExit != null)
            {
                objective = new GoToDoorObjective(entity, randomExit.Value);
                return null;
            }

            objective = null;
            return null;
        }
    }
}