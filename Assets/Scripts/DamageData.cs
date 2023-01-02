using System;
using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DonBigo
{
    [Serializable]
    public struct DamageData
    {
        public bool lethal;
        public int stunTurns;
        public Sprite stunIcon;
        public GameObject stunOverlayPrefab;

        public void Apply(HealthManager health)
        {
            if (lethal)
            {
                health.Kill();
            }
            else if (stunTurns > 0)
            {
                var stun = new StunStatus(stunTurns);
                health.AddStatus(stun, stunIcon, stunOverlayPrefab);
            }
        }
    }
}