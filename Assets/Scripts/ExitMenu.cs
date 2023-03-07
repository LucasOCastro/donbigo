using DonBigo.Actions;
using UnityEngine;
using UnityEngine.UI;

namespace DonBigo
{
    public class ExitMenu : MonoBehaviour
    {
        [SerializeField] private GameObject menu;
        [SerializeField] private Button exitButton;
        [SerializeField] private Button closeButton;
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
            closeButton.onClick.AddListener(() => Open = false);
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