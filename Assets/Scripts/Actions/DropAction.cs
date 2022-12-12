using DonBigo;

namespace DonBigo.Actions
{
    public class DropAction : Action
    {
        private Tile _target;
        public DropAction(Entity doer, Tile target) : base(doer)
        {
            _target = target;
        }

        public override void Execute()
        {
            Doer.Inventory.DropHand(Doer.Inventory.CurrentHandedness, _target);
        }
    }
}