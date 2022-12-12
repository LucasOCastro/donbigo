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
        
        private Entity Owner { get; }
        public Handedness CurrentHandedness { get; set; }
        
        private readonly Item[] _inventory = new Item[2];
        
        public Inventory(Entity owner)
        {
            Owner = owner;
        }

        public Item CurrentHand => GetHand(CurrentHandedness);
        public Item GetHand(Handedness hand) => _inventory[(int)hand];
        
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
            DropHand(hand, dropTile);
            SetHandRaw(hand, item);
        }

        public void DropHand(Handedness hand) => DropHand(hand, Owner.Tile);
        public void DropHand(Handedness hand, Tile tile)
        {
            if (!tile.SupportsItem)
            {
                Debug.LogError("Tentou droppar item em tile que nao suporta item.");
                return;
            }
            
            Item heldItem = GetHand(hand);
            if (heldItem == null) return;

            heldItem.Holder = null;
            heldItem.Tile = tile;
            SetHandRaw(hand, null);
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