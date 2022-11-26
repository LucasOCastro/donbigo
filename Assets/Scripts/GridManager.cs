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
        [SerializeField] private int DEBUG_testFOVRange = 10;
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

        public static HashSet<Vector2Int> DEBUG_visibleTiles = new HashSet<Vector2Int>();
        public static bool DEBUG_drawVis = false;
        private void Update()
        {
            //DEBUG fov test
            if (Input.GetKeyDown(KeyCode.K))
            {
                DEBUG_drawVis = !DEBUG_drawVis;
                tilemap.RefreshAllTiles();
            }
            if (Input.GetKeyDown(KeyCode.P))
            {
                Vector2 mousePos = Input.mousePosition;
                Vector2 mouseScreenPos = Camera.main.ScreenToWorldPoint(mousePos);
                Vector2Int tile = Grid.WorldToTilePos(mouseScreenPos);
                if (!Grid.InBounds(tile)) return;
                DEBUG_drawVis = true;
                DEBUG_visibleTiles = ShadowCasting.Cast(Grid, tile, DEBUG_testFOVRange);
                tilemap.RefreshAllTiles();
            }
            
            //DEBUG spawnando pantufa no click
            if (Input.GetMouseButtonDown(0))
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
            }
        }
    }
}