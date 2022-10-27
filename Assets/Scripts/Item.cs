using UnityEngine;

namespace DonBigo
{
    public class Item : MonoBehaviour
    {
        [SerializeField] private ItemType itemType;
        public ItemType Type => itemType;

        private Tile _tile;
        public Tile Tile
        {
            get => _tile;
            set
            {
                if (_tile != null)
                {
                    _tile.Item = null;
                }

                _tile = value;
                if (_tile != null && _tile.Item != this)
                {
                    _tile.Item = this;
                    transform.position = _tile.ParentGrid.TileToWorld(_tile);
                }
            }
        }

    }
}