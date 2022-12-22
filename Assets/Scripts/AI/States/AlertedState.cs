using UnityEngine;

namespace DonBigo.AI
{
    public class AlertedState : AIState
    {
        protected override AIState OnTick(Entity entity, out AIObjective objective)
        {
            //TODO Desse jeito, ele para de fugir/perseguir quando perde o inimigo de vista, o que é bem ineficaz.
            if (entity.SeesPlayer)
            {
                var player = CharacterManager.DonBigo;
                //TODO não é efetivo retornar um novo objetivo todo tick, tenho que criar 1 e só criar outro quando precisar
                bool stronger = entity.Inventory.CombatPower >= player.Inventory.CombatPower;
                Debug.Log("Stronger = "+ stronger);
                objective = stronger
                    ? new AttackEntityObjective(entity, player)
                    : new FleeObjective(entity, player);
                return null;
            }

            objective = null;
            return new WanderState();
        }

        
    }
}