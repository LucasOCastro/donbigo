using System;
using UnityEngine;

namespace DonBigo
{
    public abstract class TileObject : MonoBehaviour
    {
        public abstract Tile Tile { get; set; }

        private SpriteRenderer _sprite;
        protected virtual void Awake()
        {
            _sprite = GetComponent<SpriteRenderer>();
        }

        public void SetRenderVisibility(bool visible)
        {
            _sprite.enabled = visible;
        }
        public virtual void UpdateRenderVisibility()
        {
            SetRenderVisibility(FieldOfViewRenderer.IsVisible(Tile.Pos));
        }
    }
}