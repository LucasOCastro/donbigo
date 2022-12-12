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
        [SerializeField] private Image creditsMenu;

        [SerializeField] private int playSceneID;
        
        private void Awake()
        {
            playButton.onClick.AddListener(PlayGame);
            muteButton.onClick.AddListener(() => Debug.Log("mute"));
            creditsButton.onClick.AddListener(() => SetCredits(true));
        }

        private void PlayGame()
        {
            SceneManager.LoadScene(playSceneID);
        }

        private void SetCredits(bool open)
        {
            creditsMenu.gameObject.SetActive(open);
            StopAllCoroutines();
            if (open)
            {
                StartCoroutine(CreditsCoroutine());
            }
        }

        private IEnumerator CreditsCoroutine()
        {
            while (creditsMenu.isActiveAndEnabled)
            {
                if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
                {
                    SetCredits(false);
                    break;
                }
                yield return null;
            }
        }
    }
}