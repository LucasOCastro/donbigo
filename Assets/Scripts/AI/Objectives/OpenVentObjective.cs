using DonBigo.Actions;

namespace DonBigo.AI
{
    public class OpenVentObjective : GoToTargetObjective
    {
        public Vent TargetVent { get; }
        public OpenVentObjective(AIWorker worker, Vent target, PathFinding.CostFunc costFunc = null) 
            : base(worker, target.Tile, costFunc)
        {
            TargetVent = target;
        }

        public override bool Completed => TargetVent.Open;

        public override Action Tick()
        {
            if (Doer.Tile.Pos.AdjacentTo(_target.Tile.Pos))
            {
                return new ToggleVentAction(Doer, TargetVent);
            }
            return base.Tick();
        }
    }
}