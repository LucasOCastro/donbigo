using DonBigo.Actions;

namespace DonBigo.AI
{
    public class GoToTargetObjective : AIObjective
    {
        protected ITileGiver _target;
        public GoToTargetObjective(Entity doer, ITileGiver target) : base(doer)
        {
            _target = target;
        }

        private Path _currentPath;
        public override Action Tick()
        {
            if (_currentPath == null || _currentPath.End != _target.Tile)
            {
                _currentPath = new Path(Doer.Tile, _target.Tile, allowShorterPath: true);
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