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

        private void Update()
        {
            Vector2Int tile = GridManager.Instance.Grid.WorldToTilePos(transform.position);
            _renderer.enabled = FieldOfViewRenderer.IsVisible(tile);
        }
    }
}