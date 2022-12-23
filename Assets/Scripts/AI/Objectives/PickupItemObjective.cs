using DonBigo.Actions;

namespace DonBigo.AI
{
    public class PickupItemObjective : GoToTargetObjective
    {
        private Inventory.Handedness _handedness;
        private Item _targetItem;
        public PickupItemObjective(Entity entity, Item targetItem, Inventory.Handedness handedness) 
            : base(entity, targetItem)
        {
            _handedness = handedness;
            _targetItem = targetItem;
        }

        private bool _completed;
        public override bool Completed => _completed;

        public override Action Tick()
        {
            if (IsAdjacentToTarget)
            {
                Doer.Inventory.CurrentHandedness = _handedness;
                _target = null;
                _completed = true;
                return new PickupAction(Doer, _targetItem);
            }
            
            return base.Tick();
        }
    }
}