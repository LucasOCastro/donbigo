using UnityEngine;

namespace DonBigo
{
    public class TrapItem : Item
    {
        public enum ArmState {Idle, Armed, Activated}

        [SerializeField] private Sprite activatedSprite, stunIcon;
        [SerializeField] private int stunTurns;

        private ArmState _state = ArmState.Idle;
        public ArmState State
        {
            get => _state;
            private set
            {
                if (value == _state) return;
                
                _state = value;
                UpdateRenderVisibility();
            }
        }

        public override bool CanBePickedUp => State == ArmState.Idle;

        public override void SteppedOn(Entity stepper)
        {
            if (State != ArmState.Armed) return;

            State = ArmState.Activated;
            Renderer.sprite = activatedSprite;
            
            var stunStatus = new StunStatus(stunTurns);
            stepper.Health.AddStatus(stunStatus, stunIcon);
            Debug.Log("Mordi o " + stepper);
        }

        public override bool CanBeUsed(Entity doer, Tile target)
        {
            return target != null && doer.Tile.Pos.AdjacentTo(target.Pos) && target.Walkable && target.SupportsItem;
        }

        public override void UseAction(Entity doer, Tile target)
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