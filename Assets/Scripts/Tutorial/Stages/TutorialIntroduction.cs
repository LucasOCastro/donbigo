using System.Collections;
using UnityEngine;

namespace DonBigo.Tutorial.Stages
{
    [CreateAssetMenu(fileName = "IntroductionTutorialStage", menuName = "Tutorial/Introduction")]
    public class TutorialIntroduction : TutorialStage
    {
        [SerializeField] private string[] introductionDialogue, grabInstruction;
        [SerializeField] private Vector2Int walkTarget;
        [SerializeField] private ItemType grabItem;
        [SerializeField] private Vector2Int itemTile;
        public override IEnumerator UpdateCoroutine()
        {
            yield return WaitForDialogue(introductionDialogue);
            yield return new WaitUntil(() => CharacterManager.DonBigo.Tile == GetTile(walkTarget));

            var item = grabItem.Instantiate(GetTile(itemTile));
            yield return WaitForDialogue(grabInstruction);
            yield return new WaitUntil(() => item.Holder == CharacterManager.DonBigo.Inventory);
            
        }
    }
}