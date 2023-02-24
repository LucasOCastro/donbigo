namespace DonBigo.Actions
{
    public class UseVentAction : Action
    {
        public Vent Vent { get; }
        public UseVentAction(Entity doer, Vent vent) : base(doer)
        {
            Vent = vent;
        }

        public override void Execute()
        {
            Doer.EnterVent(Vent);
        }
    }
}