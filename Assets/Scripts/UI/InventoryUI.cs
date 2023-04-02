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
            public Outline outline;
            public Sprite unselectedSprite, selectedSprite;
            public CooldownTimer cooldownTimer;

            public void Update(bool selected, Item item, bool highlighted)
            {
                image.sprite = selected ? selectedSprite : unselectedSprite;
                itemImage.gameObject.SetActive(item != null);
                itemImage.sprite = (item != null) ? item.Type.InventoryIcon : null;

                if (outline)
                {
                    outline.enabled = highlighted;
                }

                bool cooldown = item != null && item.IsInCooldown; 
                cooldownTimer.gameObject.SetActive(cooldown);
                if (cooldown)
                {
                    cooldownTimer.UpdateTimer(item.TurnsSinceLastUse, item.CooldownTurns);
                }
            }
        }

        [SerializeField] private float holdScheduleTime;
        [SerializeField] private HandImage left, right;

        private void Update()
        {
            if (_hold) HoldPress();
            
            // Isso deveria ser um observer pattern? Deveria. Porém, encontramos o problema grave de preguiça.

            Entity player = CharacterManager.DonBigo;
            
#if UNITY_EDITOR
            if (PlayerCamera.DEBUG_PHANTONETTE) player = CharacterManager.Phantonette;
#endif
            
            if (player == null) return;
            
            var inventory = player.Inventory;
            var selectedHandedness = inventory.CurrentHandedness;

            bool IsSelected(Inventory.Handedness handedness) => selectedHandedness == handedness;

            bool IsHighlighted(Inventory.Handedness handedness) =>
                IsSelected(handedness) && player is Bigodon bigodon && bigodon.ScheduledItemInteractAction;

            left.Update(IsSelected(Inventory.Handedness.Left), inventory.LeftHand, IsHighlighted(Inventory.Handedness.Left));
            right.Update(IsSelected(Inventory.Handedness.Right), inventory.RightHand, IsHighlighted(Inventory.Handedness.Right));
        }

        public void ClickToggle(bool leftHand)
        {
            if (CharacterManager.DonBigo == null) return;
            CharacterManager.DonBigo.CancelScheduledItemAction();
            var inventory = CharacterManager.DonBigo.Inventory;
            inventory.CurrentHandedness = leftHand ? Inventory.Handedness.Left : Inventory.Handedness.Right;
        }

        private bool _hold;
        private float _holdTimer;

        public void StartHold(bool leftHand)
        {
            _holdTimer = 0;
            _hold = true;
        }
        
        public void HoldPress()
        {
            _holdTimer += Time.deltaTime;
            if (_holdTimer > holdScheduleTime)
            {
                if (!CharacterManager.DonBigo) return;
                _holdTimer = 0;
                CharacterManager.DonBigo.ScheduleUseItemAction();
            }
        }

        public void CancelHold(bool leftHand)
        {
            _holdTimer = 0;
            _hold = false;
        }
    }
}