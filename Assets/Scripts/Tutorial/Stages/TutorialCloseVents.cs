using System.Collections;
using UnityEngine;

namespace DonBigo.Tutorial.Stages
{
    [CreateAssetMenu(fileName = "CloseVentTutorialStage", menuName = "Tutorial/Close Vents")]
    public class TutorialCloseVents : TutorialStage
    {
        public override IEnumerator UpdateCoroutine()
        {
            var vents = GridManager.Instance.Grid.AllVents;
            foreach (var vent in vents) vent.Open = true;
            yield return new WaitUntil(() => vents.TrueForAll(v => !v.Open));
        }
    }
}