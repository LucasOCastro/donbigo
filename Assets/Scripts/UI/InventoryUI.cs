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

            Entity player = CharacterManager.DonBigo;

            if (PlayerCamera.DEBUG_PHANTONETTE) player = CharacterManager.Phantonette;
            
            if (player == null) return;
            
            var inventory = player.Inventory;
            var selectedHandedness = inventory.CurrentHandedness;

            left.Update(selectedHandedness == Inventory.Handedness.Left, inventory.LeftHand);
            right.Update(selectedHandedness == Inventory.Handedness.Right, inventory.RightHand);
        }
    }
}