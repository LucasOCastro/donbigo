using UnityEngine;

namespace DonBigo
{
    public class MeleeWeaponItem : Item
    {
        [SerializeField] private DamageData damage;
        
        public override bool CanBeUsed(Entity doer, Tile target)
        {
            return doer.Tile.Pos.AdjacentTo(target.Pos) && target.Entity != null && target.Entity != doer;
        }

        public override void UseAction(Entity doer, Tile target)
        {
            damage.Apply(target.Entity.Health);
        }
    }
}