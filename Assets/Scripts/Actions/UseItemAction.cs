using DonBigo.Actions;

namespace DonBigo.Actions
{
    public class UseItemAction : Action
    {
        private Item _item;
        private Tile _target;
        public UseItemAction(Entity doer, Item item, Tile target) : base(doer)
        {
            _item = item;
            _target = target;
        }

        public override void Execute()
        {
            _item.UseAction(Doer, _target);
        }
    }
}