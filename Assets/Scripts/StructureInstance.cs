namespace DonBigo
{
    public class StructureInstance
    {
        public StructureTileType Type { get; }
        public Tile Tile { get; }

        private int _elevation;

        public bool BlocksView => Type.ViewBlockHeight >= 0 && _elevation == Type.ViewBlockHeight;
        public bool BlocksMovement => Type.BlocksMovement;

        public StructureInstance(StructureTileType type, Tile tile, int elevation)
        {
            Type = type;
            Tile = tile;
            _elevation = elevation;
        }
    }
}