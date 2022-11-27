using UnityEngine;

namespace DonBigo
{
    public class StructureInstance
    {
        public StructureTileType Type { get; }
        public Tile Tile { get; }

        private int _elevation;

        public bool BlocksView => Type.MinViewBlockHeight >= 0 && _elevation >= Type.MinViewBlockHeight;
        
        public StructureInstance(StructureTileType type, Tile tile, int elevation)
        {
            Type = type;
            Tile = tile;
            _elevation = elevation;
        }
    }
}