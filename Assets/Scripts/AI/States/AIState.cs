using DonBigo.Actions;

namespace DonBigo.AI
{
    public abstract class AIState
    {
        protected AIObjective CurrentObjective { get; private set; }

        /// <param name="action">A ação a ser executada.</param>
        /// <returns>Um estado para qual deve transicionar, ou nulo se deve permanecer no mesmo.</returns>
        public AIState Tick(Entity entity, out Action action)
        {
            AIState state = OnTick(entity, out AIObjective newObjective);
            if (newObjective != null)
            {
                CurrentObjective = newObjective;
            }

            action = CurrentObjective?.Tick();
            return state;

            //TODO isso aqui seria ideal, então com objetivo nulo a ação seria Idle automatico. Essa mudança precisa refatorar os varios estados para diferenciar de objective=null pra objective=currentobjective.
            
            /*AIState state = OnTick(entity, out AIObjective newObjective);
            CurrentObjective = newObjective;
            action = CurrentObjective?.Tick();
            return state;*/
        }

        /// <param name="entity">A emtodade executando essa ação.</param>
        /// <param name="objective">Um novo objetivo a seguir, ou nulo se deve manter o mesmo.</param>
        /// <returns>Um estado para qual deve transicionar, ou nulo se deve permanecer no mesmo.</returns>
        protected abstract AIState OnTick(Entity entity, out AIObjective objective);
    }
}