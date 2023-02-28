using System.Collections;
using UnityEngine;

namespace DonBigo.Tutorial.Stages
{
    [CreateAssetMenu(fileName = "IntroductionTutorialStage", menuName = "Tutorial/Introduction")]
    public class TutorialIntroduction : TutorialStage
    {
        [SerializeField] private string[] introductionDialogue;
        public override IEnumerator UpdateCoroutine()
        {
            yield return WaitForDialogue(introductionDialogue);
        }
    }
}