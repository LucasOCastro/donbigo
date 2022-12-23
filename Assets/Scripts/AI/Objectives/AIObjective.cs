using DonBigo.Actions;

namespace DonBigo.AI
{
    public abstract class AIObjective
    {
        public Entity Doer { get; }
        
        public abstract bool Completed { get; }

        public AIObjective(Entity doer)
        {
            Doer = doer;
        }
        
        /// <returns>Uma ação a executar.</returns>
        public abstract Action Tick();
    }
}