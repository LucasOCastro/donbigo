using DonBigo.Actions;

namespace DonBigo.AI
{
    public abstract class AIState
    {
        protected AIObjective CurrentObjective { get; private set; }

        /// <param name="action">A ação a ser executada.</param>
        /// <returns>Um estado para qual deve transicionar, ou nulo se deve permanecer no mesmo.</returns>
        public AIState Tick(AIWorker worker, out Action action)
        {
            AIState state = OnTick(worker, out AIObjective newObjective);
            CurrentObjective = newObjective;
            action = CurrentObjective?.Tick();
            return state;
        }

        /// <param name="entity">A entidade executando essa ação.</param>
        /// <param name="objective">Um novo objetivo a seguir.</param>
        /// <returns>Um estado para qual deve transicionar, ou nulo se deve permanecer no mesmo.</returns>
        protected abstract AIState OnTick(AIWorker worker, out AIObjective objective);
    }
}