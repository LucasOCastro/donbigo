using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

namespace DonBigo
{
    public abstract class Entity : TileObject, IVisibleTilesProvider
    {
        [field: SerializeField] public int VisionRange { get; set; } = 50;
        
        public Inventory Inventory { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            Inventory = new Inventory(this);
        }


        private Tile _tile;
        public override Tile Tile
        {
            get => _tile;
            set
            {
                if (_tile == value) return;
                
                if (_tile != null)
                {
                    _tile.Entity = null;
                }

                _tile = value;
                if (_tile != null)
                {
                    if (_tile.Entity != this)
                    {
                        _tile.Entity = this;
                    }
                    transform.position = _tile.ParentGrid.TileToWorld(_tile);

                    var oldVisible = VisibleTiles;
                    VisibleTiles = ShadowCasting.Cast(_tile.ParentGrid, _tile.Pos, VisionRange);
                    OnUpdateViewEvent?.Invoke(oldVisible, VisibleTiles);
                    
                    UpdateRenderVisibility();
                }
            }
        }

        public abstract Action GetAction();
        
        public event IVisibleTilesProvider.OnUpdateViewDelegate OnUpdateViewEvent;
        public HashSet<Vector2Int> VisibleTiles { get; private set; }
    }
}