using System.Collections;
using UnityEngine;
using DonBigo.Tutorial.Stages;

namespace DonBigo.Tutorial
{
    public class TutorialSequencer : MonoBehaviour
    {
        [SerializeField] private TutorialStage[] stages;

        private void Start()
        {
            StartCoroutine(TutorialCoroutine());
        }

        private IEnumerator TutorialCoroutine()
        {
            foreach (var stage in stages)
            {
                yield return stage.UpdateCoroutine();
            }
            GameEnder.Instance.EndGame(GameEnder.Condition.Exit);
        }
        
    }
}