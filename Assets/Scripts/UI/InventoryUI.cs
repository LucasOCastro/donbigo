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
            public CooldownTimer cooldownTimer;

            public void Update(bool selected, Item item)
            {
                image.sprite = selected ? selectedSprite : unselectedSprite;
                itemImage.gameObject.SetActive(item != null);
                itemImage.sprite = (item != null) ? item.Type.InventoryIcon : null;

                bool cooldown = item != null && item.IsInCooldown; 
                cooldownTimer.gameObject.SetActive(cooldown);
                if (cooldown)
                {
                    cooldownTimer.UpdateTimer(item.TurnsSinceLastUse, item.CooldownTurns);
                }
            }
        }
        
        [SerializeField] private HandImage left, right;

        private void Update()
        {
            // Isso deveria ser um observer pattern? Deveria. Porém, encontramos o problema grave de preguiça.

            Entity player = CharacterManager.DonBigo;
            
#if UNITY_EDITOR
            if (PlayerCamera.DEBUG_PHANTONETTE) player = CharacterManager.Phantonette;
#endif
            
            if (player == null) return;
            
            var inventory = player.Inventory;
            var selectedHandedness = inventory.CurrentHandedness;

            left.Update(selectedHandedness == Inventory.Handedness.Left, inventory.LeftHand);
            right.Update(selectedHandedness == Inventory.Handedness.Right, inventory.RightHand);
        }

        public void ClickToggle(bool leftHand)
        {
            if (CharacterManager.DonBigo == null) return;
            var inventory = CharacterManager.DonBigo.Inventory;
            inventory.CurrentHandedness = leftHand ? Inventory.Handedness.Left : Inventory.Handedness.Right;
        }
    }
}