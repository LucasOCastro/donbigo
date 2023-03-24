using System.Collections;
using UnityEngine;

namespace DonBigo.Tutorial.Stages
{
    [CreateAssetMenu(fileName = "PickUpTutorialStage", menuName = "Tutorial/PickUp")]
    public class TutorialPickItem : TutorialStage
    {
        [SerializeField] private Vector2Int itemPos;
        [SerializeField] private ItemType itemType;
        [SerializeField] private bool useItem;
        public override IEnumerator UpdateCoroutine()
        {
            var tile = GetTile(itemPos);
            if (tile.Item != null) tile.Item.Delete();
            var item = itemType.Instantiate(tile);
            yield return new WaitUntil(() => item.Holder == CharacterManager.DonBigo.Inventory);
            if (useItem)
            {
                yield return new WaitUntil(() => ItemUsed(item));
            }
        }

        private static bool ItemUsed(Item item)
        {
            if (item is TrapItem trap) return trap.State == TrapItem.ArmState.Armed;
            if (item.IsInCooldown) return true;
            return item == null;
        }
    }
}