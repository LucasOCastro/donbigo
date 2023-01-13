namespace DonBigo
{
    public class Vent : StructureInstance
    {
        private bool _open;
        public bool Open
        {
            get => _open;
            set
            {
                _open = value;
                Tile.ParentGrid.RefreshTile(Tile);
            }
        }

        private VentTileType VentType { get; }
        public Tile FinalTile { get; }
        public Vent(VentTileType type, Tile tile, int elevation) : base(type, tile, elevation)
        {
            VentType = type;
            FinalTile = Tile.Walkable ? Tile : Tile.ParentGrid[Tile.Pos + VentType.Direction];
            Open = false;
        }
    }
}