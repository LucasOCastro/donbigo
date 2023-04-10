using UnityEngine;

namespace DonBigo
{
    public class UpdateFoVConformer : MonoBehaviour
    {
        private SpriteRenderer _renderer;
        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
        }
        
        public ITileGiver AssignedTileGiver { get; set; }

        private bool ShouldBeEnabled()
        {
            if (AssignedTileGiver == null) return true;
            Vector2Int tile = AssignedTileGiver.Tile.Pos;
            return FieldOfViewRenderer.IsVisible(tile);
        }
        
        private void Update()
        {
#if UNITY_EDITOR
            if (!FieldOfViewRenderer.DEBUG_drawVis)
            {
                _renderer.enabled = true;
                return;
            }
#endif
            
            if (_renderer)
                _renderer.enabled = ShouldBeEnabled();
        }
    }
}