using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DonBigo
{
    public class Entity : TileObject
    {
        private Tile _tile;
        public override Tile Tile
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
                    transform.position = _tile.ParentGrid.TileToWorld(_tile) + new Vector3(0,0,2);
                }
            }
            
        }
        public void Walk(Tile target)
        {
            if (target != null)
            {
                if (Tile != null)
                    Tile.Entity = null;
                Tile = target;
                UpdateRenderVisibility();
            }
        }


    }
}