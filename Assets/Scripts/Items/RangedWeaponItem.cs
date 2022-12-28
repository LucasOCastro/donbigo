using System.Collections;
using DonBigo.Actions;
using DonBigo.UI;
using UnityEngine;

namespace DonBigo
{
    public class RangedWeaponItem : Item
    {
        [SerializeField] private int range;
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

        protected override void UseAction(Entity doer, Tile target)
        {
            //TODO lidar com algum tipo de aleatoriedade na hora de decidir se a bala acertou ou não?
            damage.Apply(target.Entity.Health);
        }
    }
}