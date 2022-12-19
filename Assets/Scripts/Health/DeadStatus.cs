using DonBigo.Actions;

namespace DonBigo
{
    public class DeadStatus : HealthStatus
    {
        protected override void OnStart(HealthManager health)
        {
        }

        protected override void OnEnd(HealthManager health)
        {
        }

        public override bool Tick(HealthManager health)
        {
            return false;
        }

        public override Action GenAction(HealthManager health)
        {
            return new IdleAction(health.Owner);
        }
    }
}