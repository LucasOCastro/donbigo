using UnityEngine;

namespace DonBigo
{
    public class TrapItem : Item
    {
        public enum ArmState {Idle, Armed, Activated}

        [SerializeField] private Sprite activatedSprite;
        [SerializeField] private DamageData damage;

        private Entity _armer;
        private ArmState _state = ArmState.Idle;
        public ArmState State
        {
            get => _state; 
            set
            {
                if (value == _state) return;
                
                _state = value;
                if (_state == ArmState.Activated) Renderer.sprite = activatedSprite;
                UpdateRenderVisibility();
            }
        }

        public override bool CanBePickedUp => State == ArmState.Idle;

        public override void SteppedOn(Entity stepper)
        {
            if (State != ArmState.Armed) return;
            
            State = ArmState.Activated;
            if (_armer != null) _armer.BlacklistedTiles.Remove(Tile.Pos);
            
            //isso é umm CRIME contra os princípios SOLID mas lhkgfçmgfm :)
            if (stepper.Inventory.ContainsItem<StunImmunityItem>(out var handedness))
            {
                stepper.Inventory.GetHand(handedness).Delete();
                return;
            }
            
            damage.Apply(stepper.Health);
        }

        public override bool CanBeUsed(Entity doer, Tile target)
        {
            return base.CanBeUsed(doer, target) && target != null && doer.Tile.Pos.AdjacentTo(target.Pos) && target.Walkable && target.SupportsItem;
        }

        protected override void UseAction(Entity doer, Tile target)
        {
            if (Holder != null && Holder.ContainsItem(this, out var heldHand))
            {
                Holder.DropHand(heldHand, target);
            }
            else
            {
                Tile = target;
            }
            State = ArmState.Armed;
            doer.BlacklistedTiles.Add(target.Pos);
            _armer = doer;
        }

        public override void UpdateRenderVisibility()
        {
            if (State == ArmState.Armed)
            {
                SetRenderVisibility(false);
                return;
            }
            base.UpdateRenderVisibility();
        }
    }
}