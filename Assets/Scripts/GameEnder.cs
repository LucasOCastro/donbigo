using System.Collections;
using DonBigo.Actions;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DonBigo
{
    public class GameEnder : MonoBehaviour
    {
        public enum Condition
        {
            Victory,
            Defeat
        }

        [SerializeField] private Animator victoryAnimator;
        [SerializeField] private Animator defeatAnimator;
        [SerializeField] private int menuSceneIndex;
        
        public static GameEnder Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        private static IEnumerator PlayAnimationAndEndCoroutine(Animator animator, int scene)
        {
            animator.gameObject.SetActive(true);
            yield return new WaitWhile(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1);

            yield return new WaitUntil(() => Input.anyKeyDown);    
            
            
            var loadOperation = SceneManager.LoadSceneAsync(scene);
            yield return new WaitUntil(() => loadOperation.isDone);
        }

        public void EndGame(Condition condition)
        {
            TurnManager.Instance.enabled = false;
            var animator = condition switch
            {
                Condition.Victory => victoryAnimator,
                Condition.Defeat => defeatAnimator,
                _ => null
            };
            StartCoroutine(PlayAnimationAndEndCoroutine(animator, menuSceneIndex));
        }
    }
}