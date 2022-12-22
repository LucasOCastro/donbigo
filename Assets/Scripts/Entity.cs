using System.Collections;
using System.Collections.Generic;
using DonBigo.Actions;
using UnityEngine;
using UnityEngine.XR;

namespace DonBigo
{
    public abstract class Entity : TileObject, IVisibleTilesProvider
    {
        [field: SerializeField] public int VisionRange { get; set; } = 50;
        
        public Inventory Inventory { get; private set; }
        public HealthManager Health { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            Inventory = new Inventory(this);
            Health = new HealthManager(this);
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
                    Vector3 worldPos = _tile.ParentGrid.TileToWorld(_tile);
                    worldPos.z = 2;
                    transform.position = worldPos;

                    var oldVisible = VisibleTiles;
                    VisibleTiles = ShadowCasting.Cast(_tile.ParentGrid, _tile.Pos, VisionRange);
                    VisibleEntities.Clear();
                    VisibleItems.Clear();
                    SeesPlayer = false;
                    foreach (var tile in VisibleTiles)
                    {
                        var item = Tile.ParentGrid[tile].Item; 
                        if (item != null)
                        {
                            VisibleItems.Add(item);
                        }

                        var entity = Tile.ParentGrid[tile].Entity;
                        if (entity is Bigodon) SeesPlayer = true;
                        if (entity != null && entity != this)
                        {
                            VisibleEntities.Add(entity);
                        }
                    }
                    
                    OnUpdateViewEvent?.Invoke(oldVisible, VisibleTiles);
                    UpdateRenderVisibility();
                    
                    if (_tile.Entity != this)
                    {
                        _tile.Entity = this;
                    }
                }
            }
        }

        public abstract Action GetAction();
        
        public event IVisibleTilesProvider.OnUpdateViewDelegate OnUpdateViewEvent;
        public HashSet<Vector2Int> VisibleTiles { get; private set; }

        //Honestamente não gosto muito desse SeesPlayer, nem da VisibleEntities
        //Idealmente, chegar visibilidade de uma entitade deveria sr feito pelo VisibleTiles.
        public bool SeesPlayer { get; private set; }
        public List<Entity> VisibleEntities { get; } = new List<Entity>();
        public List<Item> VisibleItems { get; } = new List<Item>();
    }
}