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
        
        public static FieldOfViewRenderer Instance { get; private set; }

        private Vector2Int _originTile;
        public Vector2Int OriginTile
        {
            get => _originTile;
            set
            {
                if (value == _originTile) return;

                _originTile = value;
                UpdateFoV();
            }
        }

        private void Update()
        {
            if (originFollowCamera)
            {
                Vector3 worldPos = Camera.main.transform.position;
                OriginTile = GridManager.Instance.Grid.WorldToTilePos(worldPos);
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

        private void UpdateFoV()
        {
            //Atualizar todos os tiles do tilemap eh algo bem caro.
            //Com o desenvolvimento de um sistema de FoV, podemos atualizar apenas os tiles necessarios.
            tilemap.RefreshAllTiles();
        }
    }
}