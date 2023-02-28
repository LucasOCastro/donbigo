using System.Collections;
using UnityEngine;

namespace DonBigo.Tutorial.Stages
{
    [CreateAssetMenu(fileName = "DialogueTutorialStage", menuName = "Tutorial/Dialogue")]
    public class TutorialSimpleDialogue : TutorialStage
    {
        [SerializeField] private string[] dialogues;

        public override IEnumerator UpdateCoroutine()
        {
            yield return WaitForDialogue(dialogues);
        }
    }
}