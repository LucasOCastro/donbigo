using System;
using UnityEngine;
using UnityEngine.UI;

namespace DonBigo.UI
{
    public class InventoryUI : MonoBehaviour
    {

        [Serializable]
        private class HandImage
        {
            public Image image, itemImage;
            public Color defaultColor;

            public void Update(bool selected, Color selectedColor, Item item)
            {
                image.color = selected ? selectedColor : defaultColor;
                itemImage.gameObject.SetActive(item != null);
                itemImage.sprite = (item != null) ? item.Type.InventoryIcon : null;
            }
        }
        
        [SerializeField] private HandImage left, right;
        [SerializeField] private Color selectedColor;

        private void Update()
        {
            // Isso deveria ser um observer pattern? Deveria. Porém, encontramos o problema grave de preguiça.
            if (CharacterManager.DonBigo == null) return;
            
            var inventory = CharacterManager.DonBigo.Inventory;
            var selectedHandedness = inventory.CurrentHandedness;

            left.Update(selectedHandedness == Inventory.Handedness.Left, selectedColor, inventory.LeftHand);
            right.Update(selectedHandedness == Inventory.Handedness.Right, selectedColor, inventory.RightHand);
        }
    }
}