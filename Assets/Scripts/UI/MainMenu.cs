using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DonBigo.UI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button playButton;
        [SerializeField] private Button muteButton;
        [SerializeField] private Button creditsButton;

        [SerializeField] private int playSceneID;

        [SerializeField] private string playAnimationName;
        [SerializeField] private string creditsOpenAnimationName;
        [SerializeField] private string creditsCloseAnimationName;

        [SerializeField] private GameObject[] objectsToDestroyWhenPlay;

        private Animator _animator;
        private void Awake()
        {
            _animator = GetComponent<Animator>();
            playButton.onClick.AddListener(() => StartCoroutine(PlayCoroutine()));
            muteButton.onClick.AddListener(() => Debug.Log("mute"));
            creditsButton.onClick.AddListener((() => StartCoroutine(CreditsCoroutine())));
        }

        private bool AnimationPlaying(string anim)
        {
            var state = _animator.GetCurrentAnimatorStateInfo(0);
            return state.IsName(anim) && state.normalizedTime < 1;
        }

        private void SetButtons(bool active)
        {
            playButton.gameObject.SetActive(active);
            muteButton.gameObject.SetActive(active);
            creditsButton.gameObject.SetActive(active);
        }

        private IEnumerator PlayAndAwaitAnimationCoroutine(string anim)
        {
            _animator.Play(anim);
            yield return null;
            yield return new WaitWhile(() => AnimationPlaying(anim));
        }
        
        private IEnumerator PlayCoroutine()
        {
            Scene menuScene = SceneManager.GetActiveScene();
            GameObject canvas = transform.parent.gameObject;
            
            //Desativa botões e destroi os objetos que nao poderão existir na proxima cena
            SetButtons(false);
            foreach (var obj in objectsToDestroyWhenPlay) 
                Destroy(obj);
            
            //Carrega o jogo no fundo, move o canvas com a animação pra nova cena e ativa ela
            Scene playScene = SceneManager.LoadScene(playSceneID, new LoadSceneParameters(LoadSceneMode.Additive));
            yield return null; //Precisa esperar um frame pra terminar de carregar
            SceneManager.MoveGameObjectToScene(canvas, playScene);
            SceneManager.SetActiveScene(playScene);
            
            //Descarrega a cena do menu, toca a animação e destroy esse objeto
            //var asyncUnload = SceneManager.UnloadSceneAsync(menuScene);
            //yield return new WaitUntil(() => asyncUnload.isDone);
            yield return PlayAndAwaitAnimationCoroutine(playAnimationName);
            Destroy(canvas);
        }

        private IEnumerator CreditsCoroutine()
        {
            SetButtons(false);
            yield return PlayAndAwaitAnimationCoroutine(creditsOpenAnimationName);
            yield return new WaitUntil(() => Input.anyKeyDown);
            yield return PlayAndAwaitAnimationCoroutine(creditsCloseAnimationName);
            SetButtons(true);
        }
    }
}