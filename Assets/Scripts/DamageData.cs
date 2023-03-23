using System;

namespace DonBigo
{
    [Serializable]
    public struct DamageData
    {
        public bool lethal;
        public int stunTurns;
        public UpdateFoVConformer stunOverlayPrefab;

        public void Apply(HealthManager health)
        {
            if (lethal)
            {
                health.Kill();
            }
            else if (stunTurns > 0)
            {
                var stun = new StunStatus(stunTurns);
                health.AddStatus(stun, stunOverlayPrefab);
            }
        }
    }
}