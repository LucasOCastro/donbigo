using DonBigo.Actions;

namespace DonBigo
{
    public class StunStatus : TurnLimitHealthStatus
    {
        public StunStatus(int stunTurns) : base(stunTurns)
        {
        }

        protected override void OnEnd(HealthManager health)
        {
        }

        public override Action GenAction(HealthManager health)
        {
            return new IdleAction(health.Owner);
        }
    }
}