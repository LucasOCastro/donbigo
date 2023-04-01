using UnityEngine;
using UnityEngine.UI;


namespace DonBigo.UI
{
    [RequireComponent(typeof(Image))]
    public class CameraNoise : MonoBehaviour
    {
        [SerializeField] private float interval;
        [SerializeField] private Sprite[] sprites;

        private Image _image;

        private void Awake()
        {
            _image = GetComponent<Image>();
        }

        private int _lastIndex = -1;
        private float _timer = 0;
        private void Update()
        {
            if (_timer < interval)
            {
                _timer += Time.deltaTime;
                return;
            }

            _timer = 0;
            _lastIndex = sprites.RandomIndex(excluding: _lastIndex);
            _image.sprite = sprites[_lastIndex];
        }
    }
}