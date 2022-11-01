using UnityEngine;
using UnityEngine.Tilemaps;

namespace DonBigo.Rooms
{
    [CreateAssetMenu(fileName = "NewRoom", menuName = "New Room")]
    public class Room : ScriptableObject
    {
        [SerializeField] private string roomName;
        [SerializeField] private Vector3Int bounds;
        [SerializeField] private TileBase[] tilesBlock;
        [SerializeField] private TileType[] tileTypes;

        public string RoomName => roomName;
        public Vector3Int Bounds => bounds;

        // A Unity não suporta a serialização de arrays multidimensionais. Então eu serializo numa array normal,
        // transformo em uma array multidimensional quando necessário e guardo num cache.
        private TileType[,] _tiles;
        public TileType[,] Tiles
        {
            get
            {
                if (_tiles != null) return _tiles;
                
                _tiles = new TileType[bounds.x, bounds.y];
                for (int i = 0; i < tileTypes.Length; i++)
                {
                    int y = i / bounds.x;
                    int x = i - (y * bounds.x);
                    _tiles[x, y] = tileTypes[i];
                }

                return _tiles;
            }
        }

        public void FillTilemap(Tilemap tilemap, Vector2Int start)
        {
            BoundsInt fillBounds = new BoundsInt((Vector3Int)start, bounds);
            tilemap.SetTilesBlock(fillBounds, tilesBlock);
        }
    }
}