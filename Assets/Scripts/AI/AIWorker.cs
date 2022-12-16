using DonBigo.Actions;

namespace DonBigo.AI
{
    public class AIWorker
    {
        public Entity Owner { get; }

        public AIWorker(Entity owner)
        {
            Owner = owner;
            _currentState = new WanderState();
        }
        
        private AIState _currentState;
        public Action GetAction()
        {
            AIState newState = _currentState.Tick(this, out Action action);
            if (newState != null)
            {
                _currentState = newState;
            }
            return action;
        }
    }
}