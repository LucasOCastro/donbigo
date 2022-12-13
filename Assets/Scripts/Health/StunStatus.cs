using DonBigo.Actions;

namespace DonBigo
{
    public class StunStatus : TurnLimitHealthStatus
    {
        public StunStatus(int stunTurns) : base(stunTurns)
        {
        }
        
        public override void Start(HealthManager health)
        {
        }

        public override void End(HealthManager health)
        {
        }

        public override Action GenAction(HealthManager health)
        {
            return new IdleAction(health.Owner);
        }
    }
}