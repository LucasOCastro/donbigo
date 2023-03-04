using System.Collections.Generic;
using UnityEngine;
using DonBigo.Rooms;
using UnityEngine.Tilemaps;

namespace DonBigo
{
    public class GameGrid
    {
        public const float WorldElevationOffsetMultiplier = 0.2807145f;
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
            basePos.y += elevation * WorldElevationOffsetMultiplier;
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

        public Tile MouseOverTile()
        {
            Vector2Int pos = MouseOverPos();
            return InBounds(pos) ? this[pos] : null;
        }

        public List<RoomInstance> AllRooms => _rooms;
        public List<Vent> AllVents { get; } = new List<Vent>();
        public bool CanUseVents => AllVents.MoreThan(v => v.Open, 1);

        public IEnumerable<Tile> TilesInBounds(RectInt bounds)
        {
            for (int x = bounds.xMin; x < bounds.xMax; x++)
            {
                for (int y = bounds.yMin; y < bounds.yMax; y++)
                {
                    if (!InBounds(x, y)) continue;
                    
                    Tile tile = this[x, y];
                    if (tile != null)
                    {
                        yield return tile;
                    }
                }
            }
        }

        public void SpreadTraps(ItemType trap, float doorChance)
        {
            if (trap == null || doorChance <= 0 || trap.InstanceType != typeof(TrapItem)) return;

            foreach (var room in AllRooms)
            {
                foreach (var exit in room.Doors)
                {
                    if (Random.Range(0f, 1f) >= doorChance) continue;
                    
                    var tile = exit.FinalTile(this);
                    if (!tile.SupportsItem) continue;

                    var instance = (TrapItem)trap.Instantiate(tile);
                    instance.State = TrapItem.ArmState.Armed;
                }
            }
        }

        public GameGrid(int size, Tilemap tilemap, TileType filler, EntranceMarkerTile fillerMat, Room startingRoom, Vector2 normalizedGenStart)
        {
            Size = size;
            _tilemap = tilemap;
            _tiles = new Tile[size, size];
            _rooms = MapGen.Gen(this, tilemap, filler, fillerMat, startingRoom, normalizedGenStart);
            
        }

        public void RefreshTile(Tile tile)
        {
            for (int i = 0; i < _tilemap.size.z; i++)
            {
                _tilemap.RefreshTile(new Vector3Int(tile.Pos.x, tile.Pos.y, i));    
            }
        }
    }
}

