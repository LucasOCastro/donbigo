using System;
using System.Collections.Generic;
using System.Linq;
using DonBigo.Actions;
using UnityEngine;
using Action = DonBigo.Actions.Action;

namespace DonBigo
{
    public class Tile
    {
        public Vector2Int Pos { get; }
        public TileType Type { get; }
        public GameGrid ParentGrid { get; }
    
        public Tile(Vector2Int pos, TileType tileType, GameGrid grid)
        {
            Pos = pos;
            Type = tileType;
            ParentGrid = grid;
        }

        public IEnumerable<Tile> Neighbors
        {
            get
            {
                for (int x = -1; x <= 1; x++)
                {
                    for (int y = -1; y <= 1; y++)
                    {
                        if (x == 0 && y == 0) continue;

                        Vector2Int pos = new Vector2Int(Pos.x + x, Pos.y + y);
                        if (!ParentGrid.InBounds(pos)) continue;
                        Tile tile = ParentGrid[pos];
                        if (tile != null) yield return tile;
                    }
                }
            }
        }

        public List<StructureInstance> Structures { get; } = new List<StructureInstance>();

        private Entity _entity;
        public Entity Entity
        {
            get => _entity;
            set
            {
                if (_entity != null && value != null)
                {
                    Debug.LogError("Settou entidade em tile que ja tem entidade!");
                }
                _entity = value;
            }
        }

        private Item _item;

        public Item Item
        {
            get => _item;
            set
            {
                if (_item != null && value != null)
                {
                    Debug.LogError("Settou item em tile que ja tem item!");
                }
                _item = value;
            }
        }
        public bool Walkable => Type is not WallTileType && Type.Walkable && !Structures.Any(s => s.BlocksMovement);

        public bool SupportsItem =>
            Item == null && Type is not WallTileType && Structures.All(s => s.Type.SurfaceHeight >= 0);
        public int ItemSurfaceElevation => Structures.Count > 0 ? Structures.Max(s => s.Type.SurfaceHeight) : 0;

        public Action GenInteractAction(Entity doer)
        {
            if (Type is IRoomEntranceMarker || Structures.Any(s => s.Type is IRoomEntranceMarker))
            {
                //TODO isso é horroroso
                var room = ParentGrid.RoomAt(Pos);
                var doorIndex = room.Doors.FindIndex(d => d.Position == Pos);
                if (doorIndex < 0) return null;
                return new UseDoorAction(doer, room.Doors[doorIndex]);
            }

            if (Entity != null)
            {
                //Do something
            }

            if (Item != null)
            {
                return new PickupAction(doer, Item);
            }

            return null;
        }
        
        public bool IsSeeThrough()
        {
            if (Type is WallTileType)
            {
                return false;
            }
            return !Structures.Any(s => s.BlocksView);
        }
    }
}
