using DonBigo.Actions;
using UnityEngine;

namespace DonBigo.AI
{
    public class AttackEntityObjective : GoToTargetObjective
    {
        private HealthManager _targetHealth;
        public AttackEntityObjective(Entity doer, Entity target) : base(doer, target)
        {
            _targetHealth = target.Health;
        }

        public override bool Completed => _targetHealth.Dead;

        public override Action Tick()
        {
            //TODO Quando tivermos os novos itens, isso aqui deverá ser adaptado para diferentes tipos de "armas".
            //Melee: só ataca quando chega no player
            //Ranged: tenta atacar da distância que der
            //Trap: tenta colocar no caminho do player, ou entre si e o player (se tiver ranged)
            //Também tem que considerar usar uma netgun antes de dar tiro, por exemplo

            if (IsAdjacentToTarget)
            {
                Debug.Log("TOME de " + Doer + " para " + _target);
                return null;
            }
            return base.Tick();
        }
    }
}