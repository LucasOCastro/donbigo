using System.Collections.Generic;
using UnityEngine;
using DonBigo.Rooms;
using UnityEngine.Tilemaps;

namespace DonBigo
{
    public class GameGrid
    {
        private Tilemap _tilemap;
        private Tile[,] _tiles;
        private List<RoomInstance> _rooms;
        
        public int Size { get; }
        public RectInt Bounds => new RectInt(Vector2Int.zero, Vector2Int.one  * Size);

        public Tile this[int x, int y]
        {
            get
            {
                if (!InBounds(x,y))
                {
                    Debug.LogError("Tentou acessar grid fora das bounds do mapa.");
                    return null;
                }
                return _tiles[x,y];
            }
            
            set
            {
                if (!InBounds(x,y))
                {
                    Debug.LogError("Tentou acessar grid fora das bounds do mapa.");
                    return;
                }
                _tiles[x, y] = value;
            }
        }
    
        public Tile this[Vector2Int xy]
        {
            get => this[xy.x, xy.y];
            set => this[xy.x, xy.y] = value;
        }

        public bool InBounds(int x, int y) => x >= 0 && x < Size && y >= 0 && y < Size;
        public bool InBounds(Vector2Int xy) => InBounds(xy.x, xy.y);

        public Vector3 TileToWorld(Vector2Int tile, int elevation = 0)
        {
            Vector3 basePos = _tilemap.CellToWorld((Vector3Int)tile);
            basePos.z = 2;
            basePos.y += elevation * .3f;
            return basePos;
        } 
        public Vector3 TileToWorld(Tile tile, int elevation = 0) => TileToWorld(tile.Pos, elevation);

        public Vector2Int WorldToTilePos(Vector2 pos) => (Vector2Int)_tilemap.WorldToCell(pos);
        public Tile WorldToTile(Vector2 pos)
        {
            Vector2Int tilePos = WorldToTilePos(pos);
            return InBounds(tilePos) ? this[tilePos] : null;
        }

        public Vector2Int MouseOverPos()
        {
            Vector2 mousePos = Input.mousePosition;
            Vector2 mouseScreenPos = Camera.main.ScreenToWorldPoint(mousePos);
            return WorldToTilePos(mouseScreenPos);;
        }
        
        //Não é muito otimizado
        public RoomInstance RoomAt(Vector2Int pos) => _rooms.Find(r => r.Bounds.Contains(pos));

        public IEnumerable<Tile> TilesInBounds(RectInt bounds)
        {
            for (int x = bounds.xMin; x < bounds.xMax; x++)
            {
                for (int y = bounds.yMin; y < bounds.yMax; y++)
                {
                    Tile tile = this[x, y];
                    if (tile != null)
                    {
                        yield return tile;
                    }
                }
            }
        }

        public GameGrid(int size, Tilemap tilemap)
        {
            Size = size;
            _tilemap = tilemap;
            _tiles = new Tile[size, size];
            _rooms = MapGen.Gen(this, tilemap);
        }
    }
}

