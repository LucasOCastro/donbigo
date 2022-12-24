using UnityEngine;

namespace DonBigo
{
    public class RangedWeaponItem : Item
    {
        [SerializeField] private int range;
        [SerializeField] private DamageData damage;
        
        public override bool CanBeUsed(Entity doer, Tile target)
        {
            return doer.Tile.Pos.ManhattanDistance(target.Pos) <= range;
        }

        public override void UseAction(Entity doer, Tile target)
        {
            //TODO lidar com algum tipo de aleatoriedade na hora de decidir se a bala acertou ou não?
            damage.Apply(target.Entity.Health);
        }
    }
}