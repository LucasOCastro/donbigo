using System.Collections.Generic;
using UnityEngine;
using DonBigo.Rooms;
using UnityEngine.Tilemaps;
using DonBigo.Rooms.MapGeneration;

namespace DonBigo
{
    public class GameGrid
    {
        public const float WorldElevationOffsetMultiplier = 0.2714285f;
        private readonly Tilemap _tilemap;
        private readonly Tile[,] _tiles;
        private readonly List<RoomInstance> _rooms;
        
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

        public void MakeSound(Tile source, Entity doer)
        {
            foreach (var entity in CharacterManager.AllEntities)
            {
                entity.Memory.RememberLocation(doer, source);
            }
            
            if (doer is not Bigodon)
            {
                //FindObjectOfType é o exemplo mais classico de codigo porco no unity mas fazeroque
                //Ou é isso ou um singleton
                Object.FindObjectOfType<PlayerCamera>().Jump(doer);
            }
        }

        public GameGrid(MapGenData genData)
        {
            Size = genData.mapSize;
            _tilemap = genData.tilemap;
            _tiles = new Tile[Size, Size];
            _rooms = MapGen.Gen(this, genData);
        }

        public void RefreshTile(Tile tile)
        {
            for (int i = 0; i < _tilemap.size.z; i++)
            {
                Vector3Int tilePos = new Vector3Int(tile.Pos.x, tile.Pos.y, i);
                _tilemap.RefreshTile(tilePos);
            }
        }

        public void ClearMap()
        {
            _tilemap.ClearAllTiles();
            AllVents?.Clear();
            AllRooms?.Clear();
            for (int x = 0; x < Size; x++)
            {
                for (int y = 0; y < Size; y++)
                {
                    if (_tiles[x,y] == null) continue;
                    
                    if (_tiles[x,y].Item != null) _tiles[x,y].Item.Delete();
                    if (_tiles[x,y].Entity != null) _tiles[x,y].Entity.Delete();
                    _tiles[x, y] = null;
                }
            }
        }
    }
}

