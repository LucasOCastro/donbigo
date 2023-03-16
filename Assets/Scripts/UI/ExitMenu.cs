using DonBigo.Actions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DonBigo.UI
{
    public class ExitMenu : MonoBehaviour
    {
        [SerializeField] private GameObject menu;
        [SerializeField] private Button exitButton;
        [SerializeField] private KeyCode menuKey = KeyCode.Escape;
        [SerializeField] private UnityEvent<bool> onToggle;

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
                onToggle?.Invoke(value);
            }
        }

        private void Awake()
        {
            exitButton.onClick.AddListener(() => GameEnder.Instance.EndGame(GameEnder.Condition.Exit));
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