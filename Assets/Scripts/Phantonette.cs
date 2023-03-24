using DonBigo.Actions;
using DonBigo.AI;

namespace DonBigo
{
    public class Phantonette : Entity
    {
        private AIWorker _aiWorker;

        protected override void Awake()
        {
            base.Awake();
            _aiWorker = new AIWorker(this);
        }

        public override Action GetAction()
        {
            return _aiWorker.GetAction() ?? new IdleAction(this);
        }

        public override void EnterVent(Vent vent)
        {
            base.EnterVent(vent);
            _aiWorker.EnterVentState(vent);
        }
    }
}