using System.IO;
using UnityEngine;

namespace DonBigo
{
    public class Bigodon : Entity
    {
        private Path _currentTargetPath;

        private Action GenInteractAction(Tile tile)
        {
            if (!this.Tile.Pos.AdjacentTo(tile.Pos))
            {
                return null;
            }

            return tile.GenInteractAction(this);
        }

        public override Action GetAction()
        {
            if (_currentTargetPath != null && _currentTargetPath.Valid && !_currentTargetPath.Finished)
            {
                return new MoveAction(this, _currentTargetPath.Advance());
            }

            if (!Input.GetMouseButtonDown(0))
            {
                return null;
            }
            
            Tile tile =  GridManager.Instance.Grid.MouseOverTile();
            if (tile == null)
            {
                return null;
            }

            var interactAction = GenInteractAction(tile);
            if (interactAction != null)
            {
                return interactAction;
            }
            
            Path path = new Path(this.Tile, tile);
            _currentTargetPath = (path.Valid && !path.Finished) ? path : null;
            
            /*if (_currentTargetPath == null || !_currentTargetPath.Valid || _currentTargetPath.Finished)
            {
                Path path = new Path(this.Tile, tile);
                _currentTargetPath = (path.Valid && !path.Finished) ? path : null;
            }
            return (_currentTargetPath != null) ? new MoveAction(this, _currentTargetPath.Advance()) : null;*/
            return null;
        }
    }
}