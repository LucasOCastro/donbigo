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
                return _entryVent.Open ? _entryVent : null;
            }
            
            var playerTile = entity.Memory.LastSeenTile(CharacterManager.DonBigo);

            Vent vent;
            if (playerTile == null)
            {
                vent = vents.RandomElementByWeight(v => worker.Owner.Memory.RoomFullyExplored(v.UseTile.Room) ? 1 : 10);
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
            //Se a vent alvo foi fechada, anula pra poder escolher outra
            if (_targetVent is { Open: false }) _targetVent = null;
            
            objective = null;

            //Se já saiu da vent, começa a wanderar.
            if (_targetVent != null && _progress >= _distance) return new WanderState();
            
            //Se não tem vent alvo, encontra uma.
            if (_targetVent == null )
            {
                _targetVent = GetBestTargetVent(worker, out int bestDistance);
                //Se não encontrou uma vent alvo, então ficou preso. Se ficou preso, morre.
                if (_targetVent == null)
                {
                    worker.Owner.Health.Kill();
                    return null;
                }
                _progress = 0;
                _distance = bestDistance;
            }

            _progress++;
            if (_progress < _distance) return null;
            
            //Se completou a viagem, sai da vent mas espera o proximo turno pra começar a wanderar.
            worker.Owner.ExitVent(_targetVent);
            return null;
        }
    }
}