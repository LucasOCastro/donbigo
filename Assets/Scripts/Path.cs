using System.Collections.Generic;

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
}