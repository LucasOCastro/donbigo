﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.Tilemaps;

namespace DonBigo
{
    [RequireComponent(typeof(Grid))]
    public class GridManager : MonoBehaviour
    {
        public static GridManager Instance { get; private set; }
        
        public GameGrid Grid { get; private set; }
        
        [SerializeField] private int mapSize = 50;
        [SerializeField] private Tilemap tilemap;
        [SerializeField] private TileType fillerTile; 
        
        public DonBigo.Rooms.Room DEBUG_TEST_ROOM;
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            Grid = new GameGrid(mapSize, tilemap, fillerTile);
        }

        public static Tile DEBUG_start, DEBUG_end;
        public static HashSet<Vector2Int> DEBUG_pathTiles = new HashSet<Vector2Int>();

        void RefreshTile(Vector2Int tile)
        {
            for (int z = 0; z < tilemap.size.z; z++)
            {
                tilemap.RefreshTile(new Vector3Int(tile.x, tile.y, z));
            }
        }
        void RefreshTiles(IEnumerable<Vector2Int> tiles)
        {
            foreach (var tile in tiles)
            {
                RefreshTile(tile);
            }
        }
        
        private void Update()
        {
            if (Input.GetMouseButtonDown(1))
            {
                Tile tile = Grid.MouseOverTile();
                if (tile == null) return;
                Debug.Log($"{tile.Pos} - {tile.Type.name} - e={tile.Entity} - i={tile.Item}");
            }
        }
    }
}