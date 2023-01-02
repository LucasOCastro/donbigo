using System.Collections.Generic;
using DonBigo.Actions;
using UnityEngine;

namespace DonBigo
{
    public abstract class Entity : TileObject, IVisibleTilesProvider
    {
        [field: SerializeField] public int VisionRange { get; set; } = 50;
        
        public Inventory Inventory { get; private set; }
        public HealthManager Health { get; private set; }
        public Memory Memory { get; } = new();

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
                    
                    OnUpdateViewEvent?.Invoke(oldVisible, VisibleTiles);
                    UpdateRenderVisibility();
                    
                    if (_tile.Entity != this)
                    {
                        _tile.Entity = this;
                    }
                    Memory.RememberBeingAt(Tile);
                }
            }
        }

        public abstract Action GetAction();
        
        //Provavelmente seria mais pratico armazenar Tile ao inves de Vector2Int.
        public event IVisibleTilesProvider.OnUpdateViewDelegate OnUpdateViewEvent;
        public HashSet<Vector2Int> VisibleTiles { get; private set; }

        //Usar o SeesPlayer significa hardcodar o Player como o inimigo unico da IA.
        //Isso funciona pro projeto atualmente, mas pode complicar alguma coisa no futuro.
        public bool SeesPlayer => VisibleTiles.Contains(CharacterManager.DonBigo.Tile.Pos);
        public HashSet<Vector2Int> BlacklistedTiles { get; } = new();
    }
}