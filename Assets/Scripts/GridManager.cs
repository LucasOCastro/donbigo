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
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            
            Grid = new GameGrid(mapSize, tilemap);
            DEBUG_testItem.Instantiate(Grid[5, 5]);
            DEBUG_testItem.Instantiate(Grid[0, 0]);
        }

        private void Update()
        {
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