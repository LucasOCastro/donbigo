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
        
        public Entity Entity { get; set; }
        public Item Item { get; set; }
        public bool Walkable => Type is not WallTileType && Type.Walkable && Structures.All(s => s.BlocksMovement);

        public bool SupportsItem =>
            Item == null && Type is not WallTileType && Structures.All(s => s.Type.SurfaceHeight >= 0);
        public int ItemSurfaceElevation => Structures.Count > 0 ? Structures.Max(s => s.Type.SurfaceHeight) : 0;
        //public Vec3i
        
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
