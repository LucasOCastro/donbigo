using DonBigo.Actions;

namespace DonBigo.AI
{
    public abstract class AIObjective
    {
        protected Entity Doer { get; }

        public AIObjective(Entity doer)
        {
            Doer = doer;
        }
        
        /// <returns>Uma ação a executar.</returns>
        public abstract Action Tick();
    }
}