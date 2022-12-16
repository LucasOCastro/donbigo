using DonBigo.Actions;

namespace DonBigo.AI
{
    public abstract class AIObjective
    {
        /// <returns>Uma ação a executar.</returns>
        public abstract Action Tick();
    }
}