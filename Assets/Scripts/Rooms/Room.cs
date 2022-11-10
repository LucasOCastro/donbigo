using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace DonBigo.Rooms
{
    [CreateAssetMenu(fileName = "NewRoom", menuName = "New Room")]
    public class Room : ScriptableObject
    {
        [Serializable]
        public struct StructurePosition
        {
            public Vector2Int pos;
            public StructureTileType structure;
        }
        [Serializable]
        private struct TransformOverride
        {
            public Vector3Int pos;
            public Matrix4x4 matrix;

            public void Override(Tilemap tm, Vector2Int start)
            {
                tm.SetTransformMatrix((Vector3Int)start + pos, matrix);
            }
        }

        [SerializeField] private string roomName;
        [SerializeField] private Vector3Int size;
        [SerializeField] private TileBase[] tilesBlock;
        [SerializeField] private TileType[] tileTypes;
        [SerializeField] private RoomExit[] doors;
        [SerializeField] StructurePosition[] structureTiles;
        [SerializeField] private TransformOverride[] transformOverrides;

        public string RoomName => roomName;
        public Vector3Int Size => size;
        /// <summary>
        /// Array de portas em espaço local na sala.
        /// </summary>
        public RoomExit[] Doors => doors;
        /// <summary>
        /// Array de estruturas em espaço local na sala.
        /// </summary>
        public StructurePosition[] Structures => structureTiles;

        // A Unity não suporta a serialização de arrays multidimensionais. Então eu serializo numa array normal,
        // transformo em uma array multidimensional quando necessário e guardo num cache.
        private TileType[,] _tiles;
        public TileType[,] Tiles
        {
            get
            {
                if (_tiles != null) return _tiles;
                
                _tiles = new TileType[size.x, size.y];
                for (int i = 0; i < tileTypes.Length; i++)
                {
                    int y = i / size.x;
                    int x = i - (y * size.x);
                    _tiles[x, y] = tileTypes[i];
                }

                return _tiles;
            }
        }


        public void FillTilemap(Tilemap tilemap, Vector2Int start)
        {
            BoundsInt fillBounds = new BoundsInt((Vector3Int)start, size);
            tilemap.SetTilesBlock(fillBounds, tilesBlock);
            foreach (var transformOverride in transformOverrides)
            {
                transformOverride.Override(tilemap, start);
            }
        }
    }
}