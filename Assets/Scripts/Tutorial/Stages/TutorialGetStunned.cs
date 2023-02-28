using System.Collections;
using UnityEngine;

namespace DonBigo.Tutorial.Stages
{
    [CreateAssetMenu(fileName = "StunnedTutorialStage", menuName = "Tutorial/Stunned")]
    public class TutorialGetStunned : TutorialStage
    {
        public override IEnumerator UpdateCoroutine()
        {
            yield return new WaitUntil(() => CharacterManager.DonBigo.Health.HasStatusOfType<StunStatus>());
        }
    }
}