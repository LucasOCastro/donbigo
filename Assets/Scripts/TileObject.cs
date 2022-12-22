using System;
using UnityEngine;

namespace DonBigo
{
    public abstract class TileObject : MonoBehaviour, ITileGiver
    {
        public abstract Tile Tile { get; set; }

        protected SpriteRenderer Renderer { get; private set; }
        protected virtual void Awake()
        {
            Renderer = GetComponent<SpriteRenderer>();
        }

        public void SetRenderVisibility(bool visible)
        {
            Renderer.enabled = visible;
        }
        public virtual void UpdateRenderVisibility()
        {
            SetRenderVisibility(Tile != null && FieldOfViewRenderer.IsVisible(Tile.Pos));
        }
    }
}