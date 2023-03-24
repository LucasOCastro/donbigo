using DonBigo.Actions;

namespace DonBigo.AI
{
    public abstract class AIObjective
    {
        public AIWorker Worker { get; }
        public Entity Doer => Worker.Owner;
        
        public abstract bool Completed { get; }

        public AIObjective(AIWorker worker)
        {
            Worker = worker;
        }
        
        /// <returns>Uma ação a executar.</returns>
        public abstract Action Tick();
    }
}