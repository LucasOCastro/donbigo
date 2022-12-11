using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace DonBigo
{
    public class Path
    {
        public bool Valid { get; }
        
        private int _currentIndex;
        private List<Tile> _path;
        public Path(Tile source, Tile destination)
        {
            _path = PathFinding.Path(source, destination);
            Valid = (_path != null);
        }

        public int Length => _path.Count;
        public Tile this[int i] => _path[i];
        public int CurrentIndex => _currentIndex;
        public bool Finished => CurrentIndex >= Length;

        public Tile Advance()
        {
            if (Finished) return null;

            _currentIndex++;
            return _path[_currentIndex - 1];
        }
    }
    
    public class Bigodon : Entity
    {
        private Path _currentTargetPath;

        public override Tile Tile
        {
            get => base.Tile;
            set
            {
                base.Tile = value;
                FieldOfViewRenderer.OriginTile = value?.Pos ?? Vector2Int.zero;
            }
        }

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