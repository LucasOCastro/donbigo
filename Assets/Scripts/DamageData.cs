using System;
using UnityEngine;

namespace DonBigo
{
    [Serializable]
    public struct DamageData
    {
        public bool lethal;
        public int stunTurns;
        public Sprite stunIcon;

        public void Apply(HealthManager health)
        {
            if (lethal)
            {
                health.Kill();
            }

            if (stunTurns > 0)
            {
                var stun = new StunStatus(stunTurns);
                health.AddStatus(stun, stunIcon);
            }
        }
    }
}