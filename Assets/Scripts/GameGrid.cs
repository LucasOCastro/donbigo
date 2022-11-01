using UnityEngine;
using DonBigo.Rooms;
using UnityEngine.Tilemaps;

namespace DonBigo
{
    public class GameGrid
    {
        private Grid _unityGrid;
        private Tile[,] _tiles;
        
        public int Size { get; }

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
            
            private set
            {
                _tiles[x, y] = value;
            }
        }
    
        public Tile this[Vector2Int xy]
        {
            get => this[xy.x, xy.y];
            private set => this[xy.x, xy.y] = value;
        }

        public bool InBounds(int x, int y) => x >= 0 && x < Size && y >= 0 && y < Size;
        public bool InBounds(Vector2Int xy) => InBounds(xy.x, xy.y);

        public Vector3 TileToWorld(Vector2Int tile) => _unityGrid.CellToWorld((Vector3Int)tile);
        public Vector3 TileToWorld(Tile tile) => TileToWorld(tile.Pos);

        public Vector2Int WorldToTilePos(Vector2 pos) => (Vector2Int)_unityGrid.WorldToCell(pos);
        public Tile WorldToTile(Vector2 pos)
        {
            Vector2Int tilePos = WorldToTilePos(pos);
            return InBounds(tilePos) ? this[tilePos] : null;
        }

        public GameGrid(int size, Grid unityGrid, Room DEBUG_testRoom)
        {
            Size = size;
            _unityGrid = unityGrid;
            _tiles = new Tile[size, size];
            
            //Temp
            DEBUG_PlaceRoom(DEBUG_testRoom, Vector2Int.zero);
            
            //TODO preencher o mapa com base nos comodos
        }

        public void DEBUG_PlaceRoom(Room room, Vector2Int pos)
        {
            room.FillTilemap(_unityGrid.GetComponentInChildren<Tilemap>(), pos);
            var tiles = room.Tiles;
            for (int x = 0; x < tiles.GetLength(0); x++)
            {
                for (int y = 0; y < tiles.GetLength(1); y++)
                {
                    _tiles[x, y] = new Tile(new Vector2Int(x, y) + pos, tiles[x, y], this);
                }
            }
        }
    }
}

