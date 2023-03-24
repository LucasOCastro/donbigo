using UnityEngine;

namespace DonBigo.Actions
{
    public class ToggleVentAction : Action
    {
        public Vent Vent { get; }
        public ToggleVentAction(Entity doer, Vent vent) : base(doer)
        {
            Vent = vent;
        }

        public override void Execute()
        {
            Debug.Log("toggle vent");
            Vent.Open = !Vent.Open;
        }
    }
}