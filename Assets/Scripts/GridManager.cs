using System.Collections.Generic;
using UnityEngine;
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
        
        [SerializeField] private ItemType DEBUG_testItem;
        public DonBigo.Rooms.Room DEBUG_TEST_ROOM;
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            Grid = new GameGrid(mapSize, tilemap);
        }

        public static Tile DEBUG_start, DEBUG_end;
        public static HashSet<Vector2Int> DEBUG_pathTiles = new HashSet<Vector2Int>();
        private void Update()
        {
            //DEBUG spawnando pantufa no click
            /*if (Input.GetMouseButtonDown(0))
            {
                Vector2 mousePos = Input.mousePosition;
                Vector2 mouseScreenPos = Camera.main.ScreenToWorldPoint(mousePos);
                Tile tile = Grid.WorldToTile(mouseScreenPos);
                if (tile == null) {
                    Debug.Log("NULL");
                }
                else {
                    Debug.Log($"{tile.Pos} ({tile.Type})");
                }
                if (tile != null && tile.Item == null)
                {
                    DEBUG_testItem.Instantiate(tile);
                }
            }*/
            
            if (!Input.GetMouseButtonDown(1)) return;

            if (DEBUG_pathTiles.Count > 0)
            {
                DEBUG_start = null;
                DEBUG_end = null;
                DEBUG_pathTiles.Clear();
                tilemap.RefreshAllTiles();
            }

            Vector2Int mouse = Grid.MouseOverPos();
            if (!Grid.InBounds(mouse)) return;
            if (DEBUG_start == null)
            {
                DEBUG_start = Grid[mouse];
            }
            else
            {
                DEBUG_end = Grid[mouse];
            }

            tilemap.RefreshAllTiles();
            if (DEBUG_end == null) return;

            List<Tile> path = PathFinding.Path(DEBUG_start, DEBUG_end);
            if (path == null)
            {
                Debug.Log("Nao consegui achar caminho");
                DEBUG_start = null;
                DEBUG_end = null;
                tilemap.RefreshAllTiles();
                return;
            }

            foreach (var tile in path)
            {
                DEBUG_pathTiles.Add(tile.Pos);
            }
            tilemap.RefreshAllTiles();
        }
    }
}