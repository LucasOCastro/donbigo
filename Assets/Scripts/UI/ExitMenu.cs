using DonBigo.Actions;
using UnityEngine;
using UnityEngine.UI;

namespace DonBigo.UI
{
    public class ExitMenu : MonoBehaviour
    {
        [SerializeField] private GameObject menu;
        [SerializeField] private Button exitButton;
        [SerializeField] private Button muteButton;
        [SerializeField] private KeyCode menuKey = KeyCode.Escape;

        public bool Open
        {
            get => menu.activeSelf;
            set
            {
                menu.SetActive(value);
                if (TurnManager.Instance != null)
                {
                    TurnManager.Instance.enabled = !value;
                }
            }
        }

        private void Awake()
        {
            exitButton.onClick.AddListener(() => GameEnder.Instance.EndGame(GameEnder.Condition.Exit));
            muteButton.onClick.AddListener(() => Settings.Mute = !Settings.Mute);
            Open = false;
        }

        private void Update()
        {
            if (Input.GetKeyDown(menuKey))
            {
                Open = !Open;
            }
        }
    }
}