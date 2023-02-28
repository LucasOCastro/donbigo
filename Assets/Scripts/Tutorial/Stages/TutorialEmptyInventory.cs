using System.Collections;
using UnityEngine;

namespace DonBigo.Tutorial.Stages
{
    [CreateAssetMenu(fileName = "EmptyInventoryTutorialStage", menuName = "Tutorial/EmptyInventory")]
    public class TutorialEmptyInventory : TutorialStage
    {
        public override IEnumerator UpdateCoroutine()
        {
            yield return new WaitUntil(() => CharacterManager.DonBigo.Inventory.Empty);
        }
    }
}