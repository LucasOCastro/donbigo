using System;
using UnityEngine;

namespace DonBigo
{
    public abstract class TileObject : MonoBehaviour
    {
        public abstract Tile Tile { get; set; }

        private SpriteRenderer _sprite;
        private void Awake()
        {
            _sprite = GetComponent<SpriteRenderer>();
        }

        public void SetRenderVisibility(bool visible)
        {
            _sprite.enabled = visible;
        }
        public void UpdateRenderVisibility()
        {
            SetRenderVisibility(FieldOfViewRenderer.VisibleTiles.Contains(Tile.Pos));
        }
    }
}