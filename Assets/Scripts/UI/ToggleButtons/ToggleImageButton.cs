using UnityEngine;
using UnityEngine.UI;

namespace DonBigo.UI.ToggleButtons
{
    [RequireComponent(typeof(Image), typeof(Button))]
    public abstract class ToggleImageButton : MonoBehaviour
    {
        [SerializeField] private Sprite offSprite, onSprite;
        
        public abstract bool On { get; set; }
        public void Toggle() => On = !On;
        
        private Image _image;
        private Button _button;
        private void Awake()
        {
            _image = GetComponent<Image>();
            _button = GetComponent<Button>();
        }
        
        private void OnEnable() => _button.onClick.AddListener(Toggle);
        private void OnDisable() =>_button.onClick.RemoveListener(Toggle); 

        private void Update()
        {
            _image.sprite = On ? onSprite : offSprite;
        }
    }
}