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
        [SerializeField] private bool originFollowCamera;
        [SerializeField] private int fovRange = 10;
        
        public static FieldOfViewRenderer Instance { get; private set; }

        private static Vector2Int _originTile;
        public static Vector2Int OriginTile
        {
            get => _originTile;
            set
            {
                if (value == _originTile) return;

                _originTile = value;
                Instance.UpdateFoV();
                
            }
        }

        public static HashSet<Vector2Int> VisibleTiles { get; private set; } = new();

        public static bool DEBUG_drawVis;
        private void Update()
        {
            //DEBUG
            if (Input.GetKeyDown(KeyCode.K))
            {
                DEBUG_drawVis = !DEBUG_drawVis;
                tilemap.RefreshAllTiles();
            }
            
            if (originFollowCamera)
            {
                Vector3 worldPos = Camera.main.transform.position;
                OriginTile = GridManager.Instance.Grid.WorldToTilePos(worldPos);
            }
            else if (Input.GetMouseButtonDown(0))
            {
                Vector2Int mouseTile = GridManager.Instance.Grid.MouseOverPos();
                if (GridManager.Instance.Grid.InBounds(mouseTile))
                {
                    OriginTile = mouseTile;
                }
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
                //if (tile.Entity != null) tile.Entity.UpdateRenderVisibility();
                for (int i = 0; i < tilemap.size.z; i++)
                {
                    tilemap.RefreshTile(new Vector3Int(tilePos.x, tilePos.y, i));
                }
            }
        }

        private void UpdateFoV()
        {
            DEBUG_drawVis = true;
            var oldVisibleTiles = VisibleTiles;
            VisibleTiles = ShadowCasting.Cast(GridManager.Instance.Grid, OriginTile, fovRange);

            if (oldVisibleTiles.Count == 0)
            {
                tilemap.RefreshAllTiles();
            }
            else
            {
                UpdateTiles(oldVisibleTiles);
                UpdateTiles(VisibleTiles);    
            }
            //tilemap.RefreshAllTiles();
        }
    }
}