using System.Linq;

namespace DonBigo.AI
{
    public class VentingState : AIState
    {
        private Vent _entryVent;
        private Vent _targetVent;
        private int _distance, _progress;
        public VentingState(Vent entryVent)
        {
            _entryVent = entryVent;
        }

        private int TurnsToReach(Vent vent)
        {
            //return _entryVent.Tile.Pos.ManhattanDistance(to.Tile.Pos);
            return 5;
        }
        
        private Vent GetBestTargetVent(AIWorker worker, out int distance)
        {
            var entity = worker.Owner;
            var vents = _entryVent.Tile.ParentGrid.AllVents.Where(v => v != _entryVent && v.Open);
            if (!vents.Any())
            {
                distance = 0;
                return _entryVent;
            }
            
            var playerTile = entity.Memory.LastSeenTile(CharacterManager.DonBigo);

            Vent vent;
            if (playerTile == null)
            {
                vent = vents.Random();
            }
            else if (worker.FeelsStrong)
            {
                vent = vents.Best((v1, v2) => 
                    v1.Tile.Pos.ManhattanDistance(playerTile.Pos) < v2.Tile.Pos.ManhattanDistance(playerTile.Pos)
                    );
            }
            else
            {
                vent = vents.Best((v1, v2) => 
                    v1.Tile.Pos.ManhattanDistance(playerTile.Pos) > v2.Tile.Pos.ManhattanDistance(playerTile.Pos)
                );
            }
            
            distance = TurnsToReach(vent);
            return vent;
        }
        
        protected override AIState OnTick(AIWorker worker, out AIObjective objective)
        {
            worker.Owner.Tile = null;
            objective = null;
            
            var bestTargetVent = GetBestTargetVent(worker, out int bestDistance);
            if (bestTargetVent != _targetVent)
            {
                _targetVent = bestTargetVent;
                _progress = 0;
                _distance = bestDistance;
            }

            if (_targetVent == null) return null;

            _progress++;
            if (_progress <= _distance) return null;
            worker.Owner.Tile = _targetVent.FinalTile;
            return new WanderState();
        }
    }
}