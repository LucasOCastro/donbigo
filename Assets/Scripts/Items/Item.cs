using UnityEngine;

namespace DonBigo
{
    public class Item : TileObject
    {
        [SerializeField] private ItemType itemType;
        public ItemType Type => itemType;

        
        private Inventory _holder;
        /// <summary>
        /// Inventário que atualmente contem esse item. Pra pegar um item, usa Inventory.SetHand, e não isso.
        /// </summary>
        public Inventory Holder
        {
            get => _holder;
            set
            {
                if (value == _holder) return;
                
                if (_holder != null && value != null)
                {
                    Debug.LogError("Tentou settar inventário de item que já tem inventário!");
                    return;
                }

                _holder = value;
                UpdateRenderVisibility();
                transform.parent = _holder?.Owner.transform;
                if (_holder != null) Tile = null;
            }
        }

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
                if (_tile != null && _tile.Item != this)
                {
                    _tile.Item = this;

                    Vector3 worldPos = _tile.ParentGrid.TileToWorld(_tile, _tile.ItemSurfaceElevation);
                    worldPos.z = 0;
                    transform.position = worldPos;
                }
                UpdateRenderVisibility();
            }
        }

        public virtual void SteppedOn(Entity stepper)
        {
        }

        public override void UpdateRenderVisibility()
        {
            if (Holder != null)
            {
                SetRenderVisibility(false);
                return;
            }
            base.UpdateRenderVisibility();
        }

        public virtual bool CanBePickedUp => true;
        
        public virtual bool CanBeUsed(Entity doer, Tile target)
        {
            return false;
        }
        public virtual void UseAction(Entity doer, Tile target)
        {
        }

        public void Delete()
        {
            if (Holder != null && Holder.ContainsItem(this, out var handedness))
            {
                Holder.SetHand(handedness, null, dropOnHand: false);
            }

            Holder = null;
            Tile = null;
            Destroy(gameObject);
        }
    }
}