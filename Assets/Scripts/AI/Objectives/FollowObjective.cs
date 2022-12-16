using DonBigo.Actions;

namespace DonBigo.AI
{
    public class FollowObjective : AIObjective
    {
        private Entity _doer;
        private TileObject _target;
        public FollowObjective(Entity doer, TileObject target)
        {
            _doer = doer;
            _target = target;
        }

        private Path _currentPath;
        public override Action Tick()
        {
            if (_currentPath == null || _currentPath.End != _target.Tile)
            {
                _currentPath = new Path(_doer.Tile, _target.Tile, allowShorterPath: true);
            }

            if (!_currentPath.Valid || _currentPath.Finished)
            {
                return null;
            }

            Tile nextTile = _currentPath.Advance();
            if (nextTile == _doer.Tile)
            {
                return new IdleAction(_doer);
            }
            return new MoveAction(_doer, nextTile);
        }
    }
}