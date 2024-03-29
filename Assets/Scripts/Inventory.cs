﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace DonBigo
{
    public class Inventory
    {
        public static IEnumerable<Handedness> AllHandednesses { get; } = new[] { Handedness.Left, Handedness.Right };
        public enum Handedness
        {
            Left = 0,
            Right = 1
        }
        
        public Entity Owner { get; }
        public Handedness CurrentHandedness { get; set; }

        private static int ItemCombatPower(Item item)
        {
            return (item == null || item.IsInCooldown) ? 0 : item.Type.CombatPower;
        }

        public Handedness WeakestHandedness => (ItemCombatPower(LeftHand) < ItemCombatPower(RightHand))
            ? Handedness.Left
            : Handedness.Right;

        public Handedness StrongestHandedness => (ItemCombatPower(LeftHand) > ItemCombatPower(RightHand))
            ? Handedness.Left
            : Handedness.Right;
        
        private readonly Item[] _inventory = new Item[2];
        
        public Inventory(Entity owner)
        {
            Owner = owner;
        }

        public Item CurrentHand => GetHand(CurrentHandedness);
        public Item GetHand(Handedness hand) => _inventory[(int)hand];

        public Item LeftHand => GetHand(Handedness.Left);
        public Item RightHand => GetHand(Handedness.Right);

        public int CombatPower => ItemCombatPower(LeftHand) + ItemCombatPower(RightHand);

        public bool HasLethal => (LeftHand != null && LeftHand.Type.WeaponType.HasFlag(WeaponUseType.Lethal)) ||
                                 (RightHand != null && RightHand.Type.WeaponType.HasFlag(WeaponUseType.Lethal));

        public bool Empty => LeftHand == null && RightHand == null;

        //Essa função só setta o item na array e muda o holder.
        private void SetHandRaw(Handedness hand, Item item)
        {
            _inventory[(int)hand] = item;
            if (item != null)
            {
                item.Holder = this;
            }
        }

        public void SetHand(Handedness hand, Item item, bool dropOnHand = true)
        {
            Tile dropTile = (item != null) ? item.Tile : Owner.Tile;
            if (item != null) item.Tile = null;
            if (dropOnHand)
            {
                DropHand(hand, dropTile);    
            }
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
            if (LeftHand == item)
            {
                hand = Handedness.Left;
                return true;
            }

            if (RightHand == item)
            {
                hand = Handedness.Right;
                return true;
            }

            hand = (Handedness)(-1);
            return false;
        }

        public bool ContainsItem<T>(out Handedness hand) where T : Item
        {
            if (LeftHand is T)
            {
                hand = Handedness.Left;
                return true;
            }

            if (RightHand is T)
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