using DonBigo.Actions;

namespace DonBigo.AI
{
    public class GoToTargetObjective : AIObjective
    {
        private PathFinding.CostFunc _costFunc;
        protected ITileGiver _target;
        public GoToTargetObjective(AIWorker worker, ITileGiver target, PathFinding.CostFunc costFunc = null) : base(worker)
        {
            _target = target;
            _costFunc = costFunc;
        }

        protected bool IsAdjacentToTarget => _target?.Tile != null && Doer.Tile.Pos.AdjacentTo(_target.Tile.Pos);

        public override bool Completed => IsAdjacentToTarget;

        private Path _currentPath;
        public override Action Tick()
        {
            if (_target == null)
            {
                return null;
            }
            
            if (_currentPath == null || _currentPath.End != _target.Tile)
            {
                _currentPath = new Path(Doer.Tile, _target.Tile, Doer, allowShorterPath: true, _costFunc);
            }

            if (!_currentPath.Valid || _currentPath.Finished)
            {
                return null;
            }

            Tile nextTile = _currentPath.Advance();
            if (nextTile == Doer.Tile)
            {
                return null;
            }
            return new MoveAction(Doer, nextTile);
        }
    }
}