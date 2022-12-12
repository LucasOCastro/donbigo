using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace DonBigo
{
    //Atualmente, a tile de origem e definida manualmente.
    //No futuro, isso provavelmente sera atrelado a algum outro sistema de FoV.
    //Um padrao de observador seria interessante.
    public class FieldOfViewRenderer : MonoBehaviour
    {
        //No sistema final, o tilemap sera atrelado a grid atual.
        [SerializeField] private Tilemap tilemap;
        
        public static FieldOfViewRenderer Instance { get; private set; }

        private static IVisibleTilesProvider _origin;
        public static IVisibleTilesProvider Origin
        {
            get => _origin;
            set
            {
                if (value == _origin) return;

                if (_origin != null) {
                    _origin.OnUpdateViewEvent -= Instance.UpdateFoV;
                }
                
                _origin = value;
                
                if (value != null) {
                    value.OnUpdateViewEvent += Instance.UpdateFoV;
                    Instance.UpdateFoV(null, value.VisibleTiles);
                }
            }
        }

        public static bool IsVisible(Vector2Int tile) => (Origin != null) && Origin.IsVisible(tile);

        public static bool DEBUG_drawVis = true;
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                DEBUG_drawVis = !DEBUG_drawVis;
                tilemap.RefreshAllTiles();
            }
        }

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        private void UpdateTiles(HashSet<Vector2Int> tiles)
        {
            foreach (var tilePos in tiles)
            {
                var tile = GridManager.Instance.Grid[tilePos];
                if (tile == null) continue;
                
                if (tile.Item != null) tile.Item.UpdateRenderVisibility();
                if (tile.Entity != null) tile.Entity.UpdateRenderVisibility();
                for (int i = 0; i < tilemap.size.z; i++)
                {
                    tilemap.RefreshTile(new Vector3Int(tilePos.x, tilePos.y, i));
                }
            }
        }

        void RefreshAllTiles()
        {
            var grid = GridManager.Instance.Grid;
            for (int x = 0; x < grid.Size; x++)
            {
                for (int y = 0; y < grid.Size; y++)
                {
                    if (grid[x, y] == null) continue;
                    if (grid[x,y].Entity != null) grid[x,y].Entity.UpdateRenderVisibility();
                    if (grid[x,y].Item != null) grid[x,y].Item.UpdateRenderVisibility();
                    for (int z = 0; z < tilemap.size.z; z++)
                    {
                        tilemap.RefreshTile(new Vector3Int(x,y,z));
                    }
                }
            }
        }

        private void UpdateFoV(HashSet<Vector2Int> oldVisibleTiles, HashSet<Vector2Int> newVisibleTiles)
        {
            /*if (oldVisibleTiles == null || newVisibleTiles == null || oldVisibleTiles.Count == 0)
            {
                RefreshAllTiles();
            }
            else
            {
                UpdateTiles(oldVisibleTiles);
                UpdateTiles(newVisibleTiles);    
            }*/
            RefreshAllTiles();
        }
    }
}