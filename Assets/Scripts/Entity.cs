using UnityEngine;

namespace DonBigo
{
    public class Entity : MonoBehaviour
    {
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
                if (_tile != null && _tile.Entity != this)
                {
                    _tile.Entity = this;
                    transform.position = _tile.ParentGrid.TileToWorld(_tile);
                }
            }
        }
    }
}