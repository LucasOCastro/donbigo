using DonBigo.Actions;
using DonBigo.AI;

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
            if (newObjective != null)
            {
                CurrentObjective = newObjective;
            }

            action = CurrentObjective.Tick();
            return state;
        }

        /// <param name="objective">Um novo objetivo a seguir, ou nulo se deve manter o mesmo.</param>
        /// <returns>Um estado para qual deve transicionar, ou nulo se deve permanecer no mesmo.</returns>
        protected abstract AIState OnTick(AIWorker worker, out AIObjective objective);
    }
}