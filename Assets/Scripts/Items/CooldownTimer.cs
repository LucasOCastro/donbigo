using UnityEngine;
using UnityEngine.UI;

namespace DonBigo
{
    public class CooldownTimer : MonoBehaviour
    {
        private const string UIPrefabPath = "cooldownTimerUIPrefab";
        private const string SpritePrefabPath = "cooldownTimerSpritePrefab";
        
        [SerializeField] private Sprite[] sprites;

        private static CooldownTimer _globalUIPrefab, _globalSpritePrefab;

        public static CooldownTimer GetInstance(bool ui)
        {
            if (_globalUIPrefab == null || _globalSpritePrefab == null)
            {
                _globalUIPrefab = Resources.Load<CooldownTimer>(UIPrefabPath);
                _globalSpritePrefab = Resources.Load<CooldownTimer>(SpritePrefabPath);
                if (_globalUIPrefab == null || _globalSpritePrefab == null)
                {
                    Debug.LogError("Prefab de timer está nulo ou com caminho errado!");
                    return null;
                }
            }

            return Instantiate(ui ? _globalUIPrefab : _globalSpritePrefab);
        }

        private bool IsUI => image != null;
        [SerializeField] private SpriteRenderer sprite;
        [SerializeField] private Image image;
        private Sprite SetSprite(Sprite newSprite) => IsUI ? (image.sprite = newSprite) : (this.sprite.sprite = newSprite);

        public void UpdateTimer(int current, int max)
        {
            float normalized = Mathf.Clamp01((float)current / max);
            int index = Mathf.FloorToInt(normalized * sprites.Length);
            SetSprite(sprites[index]);
        }
    }
}