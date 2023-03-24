using UnityEngine;

namespace DonBigo
{
    public class MeleeWeaponItem : Item
    {
        [SerializeField] private DamageData damage;
        
        public override bool CanBeUsed(Entity doer, Tile target)
        {
            return base.CanBeUsed(doer, target) && doer.Tile.Pos.AdjacentTo(target.Pos) && target.Entity != null && target.Entity != doer;
        }

        protected override void UseAction(Entity doer, Tile target)
        {
            damage.Apply(target.Entity.Health);
        }
    }
}