using System.Collections.Generic;

namespace DonBigo
{
    public class Path
    {
        public bool Valid { get; }
        
        private int _currentIndex;
        private List<Tile> _path;
        public Path(Tile source, Tile destination, Entity pather, bool allowShorterPath, PathFinding.CostFunc costFunc = null)
        {
            _path = PathFinding.Path(source, destination, pather, costFunc);
            Valid = (_path != null) && (Length > 0);
            if (Valid && !allowShorterPath && _path[^1] != destination)
            {
                Valid = false;
            }
        }

        public int Length => _path.Count;
        public Tile this[int i] => _path[i];
        public int CurrentIndex => _currentIndex;
        public bool Finished => CurrentIndex >= Length;

        public Tile Start => Valid ? this[0] : null;
        public Tile End => Valid ? this[Length - 1] : null;

        public Tile Advance()
        {
            if (Finished) return null;

            _currentIndex++;
            return _path[_currentIndex - 1];
        }
    }
}