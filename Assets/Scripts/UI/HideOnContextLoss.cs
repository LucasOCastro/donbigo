using UnityEngine;
using UnityEngine.EventSystems;

namespace DonBigo.UI
{
    public class HideOnContextLoss : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private bool closeOnKeyPress;
        private bool _inContext;

        private bool ShouldClose() =>
            (Input.GetMouseButtonDown(0) && !_inContext) ||
            (closeOnKeyPress && Input.anyKeyDown && !Input.GetMouseButtonDown(0));
            
        private void Update()
        {
            if (ShouldClose())
            {
                gameObject.SetActive(false);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _inContext = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _inContext = false;
        }
    }
}