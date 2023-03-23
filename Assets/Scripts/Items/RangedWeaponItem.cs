using System.Collections;
using DonBigo.Actions;
using DonBigo.UI;
using UnityEngine;

namespace DonBigo
{
    public class RangedWeaponItem : Item
    {
        [Tooltip("Distância máxima de disparo.")]
        [SerializeField] private int range;
        [Tooltip("Bônus somado na chance.")]
        [SerializeField] private float accuracyBonus;
        [Tooltip("A cada tile de distância, menos chance de acertar.")]
        [SerializeField] private float distanceAccuracyPenalty;
        [Tooltip("Se verdadeiro e atira quando adjacente, sempre acerta.")]
        [SerializeField] private bool isPointBlankGuaranteed = true;
        [SerializeField] private DamageData damage;

        
        public override bool CanBeUsed(Entity doer, Tile target)
        {
            if (!base.CanBeUsed(doer, target))
            {
                return false;
            }
            
            if (target.Entity == null || target.Entity == doer)
            {
                return false;
            }

            if (doer.Tile.Pos.ManhattanDistance(target.Pos) > range)
            {
                return false;
            }

            return true;
        }

        private bool ShouldHit(Tile from, Tile target, float aimBonus)
        {
            if (isPointBlankGuaranteed && from.Pos.AdjacentTo(target.Pos)) return true;
            float distance = from.Pos.ManhattanDistance(target.Pos) * 0.1f;
            float chanceToHit = 100 - (distance * distanceAccuracyPenalty) + accuracyBonus + aimBonus;
            float random = Random.Range(0f, 100f);
            
            Debug.Log($"{chanceToHit} = 100 - ({distance} * {distanceAccuracyPenalty}) + {accuracyBonus}");
            Debug.Log($"{random} {(random < chanceToHit ? "<" : ">")} {chanceToHit} : {(random < chanceToHit ? "HIT" : "MISS")}");
            return random < chanceToHit;
        }
        
        protected override void UseAction(Entity doer, Tile target)
        {
            if (ShouldHit(doer.Tile, target, doer.AimAccuracyBonus))
            {
                damage.Apply(target.Entity.Health);    
            }
        }
    }
}