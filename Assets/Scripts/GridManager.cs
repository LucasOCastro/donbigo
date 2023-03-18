using System.Collections.Generic;
using DonBigo.Rooms;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace DonBigo
{
    [RequireComponent(typeof(Grid))]
    public class GridManager : MonoBehaviour
    {
        public static GridManager Instance { get; private set; }
        
        public GameGrid Grid { get; private set; }
        
        [SerializeField] private Tilemap tilemap;
        [SerializeField] private MapGenData genData;

        [SerializeField] private int seed = -1;
        
        public DonBigo.Rooms.Room DEBUG_TEST_ROOM;
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            
            if (seed >= 0)
            {
                Random.InitState(seed);    
            }

            Instance = this;
            Grid = new GameGrid(tilemap, genData);
            Grid.SpreadTraps(genData.doorTrap, genData.doorTrapChance);
        }

        public static Tile DEBUG_start, DEBUG_end;
        public static HashSet<Vector2Int> DEBUG_pathTiles = new HashSet<Vector2Int>();

        [SerializeField] private ItemType DEBUG_Item;
        private void Update()
        {
            var tile = Grid.MouseOverTile();
            if (tile == null) return;
            if (Input.GetKeyDown(KeyCode.I) && tile.Item == null)
            {
                var i = DEBUG_Item.Instantiate(Grid.MouseOverTile());
                if (i is TrapItem trap) trap.State = TrapItem.ArmState.Armed;
            }
            if (Input.GetMouseButtonDown(1))
            {
                Debug.Log($"{tile.Pos} - {tile.Type.name} - e={tile.Entity} - i={tile.Item}");
            }
            if (Input.GetKeyDown(KeyCode.R)) tilemap.RefreshAllTiles();
        }
    }
}