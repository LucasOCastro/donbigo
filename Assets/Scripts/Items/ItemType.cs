using System;
using UnityEngine;

namespace DonBigo
{
    [ScriptableObjectIcon("inventoryIcon")]
    [CreateAssetMenu(fileName = "NewItem", menuName = "New Item")]
    public class ItemType : ScriptableObject
    {
        [SerializeField] private string itemName;
        public string ItemName => itemName;

        [SerializeField] private Sprite inventoryIcon;
        public Sprite InventoryIcon => inventoryIcon;

        [SerializeField] private int combatPower;
        public int CombatPower => combatPower;

        [SerializeField] [SerializeFlagEnum] private WeaponUseType weaponType;
        public WeaponUseType WeaponType => weaponType;
        
        [SerializeField] private Item prefab;

        public Type InstanceType => prefab.GetType();
        
        public Item Instantiate(Tile tile)
        {
            Item instance = Instantiate(prefab);
            instance.Tile = tile;
            return instance;
        }
    }
}