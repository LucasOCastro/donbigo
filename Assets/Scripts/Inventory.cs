using System;
using UnityEngine;

namespace DonBigo
{
    public class Inventory
    {
        public enum Handedness
        {
            Left = 0,
            Right = 1
        }
        
        public Entity Owner { get; }
        public Handedness CurrentHandedness { get; set; }
        
        private readonly Item[] _inventory = new Item[2];
        
        public Inventory(Entity owner)
        {
            Owner = owner;
        }

        public Item CurrentHand => GetHand(CurrentHandedness);
        public Item GetHand(Handedness hand) => _inventory[(int)hand];

        public Item LeftHand => GetHand(Handedness.Left);
        public Item RightHand => GetHand(Handedness.Right);
        
        
        //Essa função só setta o item na array e muda o holder.
        private void SetHandRaw(Handedness hand, Item item)
        {
            _inventory[(int)hand] = item;
            if (item != null)
            {
                item.Holder = this;
            }
        }

        public void SetHand(Handedness hand, Item item)
        {
            Tile dropTile = (item != null) ? item.Tile : Owner.Tile;
            item.Tile = null;
            DropHand(hand, dropTile);
            SetHandRaw(hand, item);
        }

        public void DropHand(Handedness hand) => DropHand(hand, Owner.Tile);
        public void DropHand(Handedness hand, Tile tile)
        {
            Item heldItem = GetHand(hand);
            if (heldItem == null) return;
            
            if (!tile.SupportsItem)
            {
                Debug.LogError("Tentou droppar item em tile que nao suporta item.");
                return;
            }

            heldItem.Holder = null;
            heldItem.Tile = tile;
            SetHandRaw(hand, null);
        }

        public bool ContainsItem(Item item, out Handedness hand)
        {
            if (GetHand(Handedness.Left) == item)
            {
                hand = Handedness.Left;
                return true;
            }

            if (GetHand(Handedness.Right) == item)
            {
                hand = Handedness.Right;
                return true;
            }

            hand = (Handedness)(-1);
            return false;
        }
        
        public void CycleHandedness()
        {
            CurrentHandedness = CurrentHandedness switch
            {
                Handedness.Left => Handedness.Right,
                Handedness.Right => Handedness.Left,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}