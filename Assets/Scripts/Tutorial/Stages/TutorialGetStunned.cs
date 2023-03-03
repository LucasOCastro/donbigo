using System.Collections;
using UnityEngine;

namespace DonBigo.Tutorial.Stages
{
    [CreateAssetMenu(fileName = "StunnedTutorialStage", menuName = "Tutorial/Stunned")]
    public class TutorialGetStunned : TutorialStage
    {
        [SerializeField] private bool phantonette;
        public override IEnumerator UpdateCoroutine()
        {
            Entity character = phantonette ? CharacterManager.Phantonette : CharacterManager.DonBigo;
            yield return new WaitUntil(() => character.Health.HasStatusOfType<StunStatus>());
        }
    }
}