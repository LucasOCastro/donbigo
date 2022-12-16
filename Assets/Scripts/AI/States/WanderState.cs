namespace DonBigo.AI
{
    public class WanderState : AIState
    {
        protected override AIState OnTick(AIWorker worker, out AIObjective objective)
        {
            objective = null;
            if (CurrentObjective == null)
            {
                objective = new FollowObjective(worker.Owner, CharacterManager.DonBigo);
            }
            return null;
        }
    }
}