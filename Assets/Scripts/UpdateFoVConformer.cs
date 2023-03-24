using UnityEngine;

namespace DonBigo
{
    //O certo seria conectar isso a um ITileGiver mas taok
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
            if (!FieldOfViewRenderer.DEBUG_drawVis) return;
        
            if (_renderer)
                _renderer.enabled = ShouldBeEnabled();
        }
    }
}