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

        private Animator _animator;
        private AudioSource _source;
        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _source = GetComponent<AudioSource>();
            playButton.onClick.AddListener(() => StartCoroutine(PlayCoroutine()));
            creditsButton.onClick.AddListener((() => StartCoroutine(CreditsCoroutine())));
        }
        
        private void Update()
        {
            if (Settings.MuteSfx) _source.mute = true;
            else _source.mute = false;
            
            if (Input.GetKeyUp(KeyCode.Escape)) Application.Quit();
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
            //Não destruo o canvas ao carregar a proxima cena pra tocar a animação de pagina em cima dela.
            GameObject canvas = transform.parent.gameObject;
            DontDestroyOnLoad(canvas);
            SetButtons(false);
            SceneManager.LoadScene(playSceneID);
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