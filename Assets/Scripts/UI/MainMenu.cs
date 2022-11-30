using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DonBigo.UI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button playButton;
        [SerializeField] private Button muteButton;
        [SerializeField] private Button creditsButton;
        [SerializeField] private Image creditsMenu;
        
        private void Awake()
        {
            playButton.onClick.AddListener(() => Debug.Log("play"));
            muteButton.onClick.AddListener(() => Debug.Log("mute"));
            creditsButton.onClick.AddListener(() => SetCredits(true));
        }

        private void SetCredits(bool open)
        {
            creditsMenu.gameObject.SetActive(open);
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