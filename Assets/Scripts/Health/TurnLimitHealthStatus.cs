namespace DonBigo
{
    public abstract class TurnLimitHealthStatus : HealthStatus
    {
        public int MaxTurns { get; }
        public int TurnProgress { get; private set; }
        public TurnLimitHealthStatus(int maxTurns)
        {
            MaxTurns = maxTurns;
        }

        public override bool Tick(HealthManager health)
        {
            TurnProgress++;
            return TurnProgress >= MaxTurns;
        }
    }
}