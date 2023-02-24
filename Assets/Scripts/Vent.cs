using DonBigo.Actions;
using DonBigo.Rooms;
using UnityEngine;

namespace DonBigo
{
    public class Vent : StructureInstance, IRoomExit
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
        public Tile UseTile => RoomExitUtility.FindWalkable(Tile.Pos, VentType.Direction, Tile.ParentGrid);
        public Vent(VentTileType type, Tile tile, int elevation) : base(type, tile, elevation)
        {
            VentType = type;
            Open = false;
        }

        Tile IRoomExit.UseTile(GameGrid grid) => UseTile;
        public Vector2Int Position => Tile.Pos;

        public Action GenAction(Entity doer)
        {
            return new UseVentAction(doer, this);
        }
    }
}