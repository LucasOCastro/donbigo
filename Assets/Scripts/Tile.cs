using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

        private Item _item;
        public Item Item
        {
            get => _item;
            set
            {
                if (_item != null)
                {
                    Debug.LogError("Tentou colocar item onde já tem item");
                    return;
                }
                _item = value;
                if (_item != null && _item.Tile != this)
                {
                    _item.Tile = this;
                }
            }
        }

        public List<StructureInstance> Structures { get; } = new List<StructureInstance>();
        
        public Entity Entity { get; set; }

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
