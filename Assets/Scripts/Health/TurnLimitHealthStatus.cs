using UnityEngine;

namespace DonBigo
{
    public abstract class TurnLimitHealthStatus : HealthStatus
    {
        public int MaxTurns { get; }
        public int TurnProgress { get; private set; }
        
        private CooldownTimer _timer;
        public TurnLimitHealthStatus(int maxTurns)
        {
            MaxTurns = maxTurns;
        }

        protected override void OnStart(HealthManager health)
        {
            _timer = CooldownTimer.GetInstance(ui: false);
            _timer.AssignedTileGiver = health.Owner;
            OnEndEvent += () => Object.Destroy(_timer.gameObject);
        }

        public override bool Tick(HealthManager health)
        {
            TurnProgress++;

            _timer.transform.parent = health.Owner.transform;
            _timer.transform.localPosition = new Vector3(0.5f, 1, 2);
            _timer.UpdateTimer(TurnProgress, MaxTurns);
            
            return TurnProgress >= MaxTurns;
        }
    }
}