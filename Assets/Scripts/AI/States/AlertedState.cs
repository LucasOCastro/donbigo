namespace DonBigo.AI
{
    public class AlertedState : AIState
    {
        private AttackEntityObjective _attackObjective;
        private FleeObjective _fleeObjective;

        protected override AIState OnTick(Entity entity, out AIObjective objective)
        {
            //TODO Desse jeito, ele para de fugir/perseguir quando perde o inimigo de vista, o que é bem ineficaz.
            if (!entity.SeesPlayer)
            {
                objective = null;
                return new WanderState();
            }

            var player = CharacterManager.DonBigo;
            if (_attackObjective == null)
            {
                _attackObjective = new AttackEntityObjective(entity, player);
                _fleeObjective = new FleeObjective(entity, player);
            }
                
            bool stronger = entity.Inventory.CombatPower >= player.Inventory.CombatPower;
            UnityEngine.Debug.Log("Stronger = "+ stronger);
            objective = stronger ? _attackObjective : _fleeObjective;

            /*objective = stronger
                    ? new AttackEntityObjective(entity, player)
                    : new FleeObjective(entity, player);*/
            return null;
        }
    }
}