using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DonBigo.UI
{
    public class TutorialSelector : MonoBehaviour
    {
        [SerializeField] private Button brButton;
        [SerializeField] private Button enButton;

        [SerializeField] private int tutorialSceneID;

        private void Awake()
        {
            brButton.onClick.AddListener(() => StartTutorial(false));
            enButton.onClick.AddListener(() => StartTutorial(true));
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Escape)) gameObject.SetActive(false);
        }

        public void StartTutorial(bool english)
        {
            Settings.English = english;
            SceneManager.LoadScene(tutorialSceneID);
        }
    }
}