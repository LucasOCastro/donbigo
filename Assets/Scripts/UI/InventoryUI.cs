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
            public Sprite unselectedSprite, selectedSprite;

            public void Update(bool selected, Item item)
            {
                image.sprite = selected ? selectedSprite : unselectedSprite;
                itemImage.gameObject.SetActive(item != null);
                itemImage.sprite = (item != null) ? item.Type.InventoryIcon : null;
            }
        }
        
        [SerializeField] private HandImage left, right;

        private void Update()
        {
            // Isso deveria ser um observer pattern? Deveria. Porém, encontramos o problema grave de preguiça.
            if (CharacterManager.DonBigo == null) return;
            
            var inventory = CharacterManager.DonBigo.Inventory;
            var selectedHandedness = inventory.CurrentHandedness;

            left.Update(selectedHandedness == Inventory.Handedness.Left, inventory.LeftHand);
            right.Update(selectedHandedness == Inventory.Handedness.Right, inventory.RightHand);
        }
    }
}