using DonBigo;

namespace DonBigo.Actions
{
    public class PickupAction : Action
    {
        private Item _item;
        public PickupAction(Entity doer, Item item) : base(doer)
        {
            _item = item;
        }

        public override void Execute()
        {
            Doer.Inventory.SetHand(Doer.Inventory.CurrentHandedness, _item);
        }
    }
}