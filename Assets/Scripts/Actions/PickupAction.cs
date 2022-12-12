using DonBigo;
using UnityEngine;

namespace DonBigo.Actions
{
    public class PickupAction : Action
    {
        private Item _item;
        public PickupAction(Entity doer, Item item) : base(doer)
        {
            if (!item.CanBePickedUp)
            {
                Debug.LogError("Ação de pegar item que não pode ser pegado.");
            }
            _item = item;
        }

        public override void Execute()
        {
            Doer.Inventory.SetHand(Doer.Inventory.CurrentHandedness, _item);
        }
    }
}