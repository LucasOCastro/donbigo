using System.Collections;
using UnityEngine;

namespace DonBigo.Tutorial.Stages
{
    [CreateAssetMenu(fileName = "DialogueTutorialStage", menuName = "Tutorial/Dialogue")]
    public class TutorialSimpleDialogue : TutorialStage
    {
        [TextArea(2,5)]
        [SerializeField] private string[] dialogues;

        [TextArea(2,5)]
        [SerializeField] private string[] englishDialogues;

        public override IEnumerator UpdateCoroutine()
        {
            yield return WaitForDialogue(Settings.English ? englishDialogues : dialogues);
        }
    }
}