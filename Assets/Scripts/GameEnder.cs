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
            Defeat,
            Exit
        }

        [SerializeField] private Animator victoryAnimator;
        [SerializeField] private Animator defeatAnimator;
        [SerializeField] private int menuSceneIndex;
        Camera mainCamera;
        private AudioSource source;
        
        public static GameEnder Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            mainCamera = Camera.main;
            source = null;
        }

        // Toca animação de fim de jogo com respectiva música
        private static IEnumerator PlayAnimationAndEndCoroutine(Animator animator, AudioSource source, int scene)
        {
            if (animator != null && source != null)
            {
                animator.gameObject.SetActive(true);
                source.Play();
                yield return new WaitWhile(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1);

                yield return new WaitUntil(() => Input.anyKeyDown);
            }

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
                Condition.Exit => null,
                _ => null
            };
            
            // Separa o emissor, para a música anterior, encerra o loop e escolhe música de fim de jogo
            source = mainCamera.GetComponent<AudioSource>();
            source.Stop();
            source.loop = false;
            source.clip = condition switch
            {
                Condition.Victory => Resources.Load<AudioClip>("BGMusics/BGM_vitoria"),
                Condition.Defeat => Resources.Load<AudioClip>("BGMusics/BGM_derrota"),
                Condition.Exit => null,
                _ => null
            };

            StartCoroutine(PlayAnimationAndEndCoroutine(animator, source, menuSceneIndex));
        }
    }
}