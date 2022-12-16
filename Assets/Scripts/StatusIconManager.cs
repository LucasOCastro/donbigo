using UnityEngine;

namespace DonBigo
{
    public class StatusIconManager : MonoBehaviour
    {
        public static StatusIconManager Instance { get; private set; }
        
        [SerializeField] private SpriteRenderer iconPrefab;
        [SerializeField] private Vector3 offset;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(Instance);
                return;
            }
            Instance = this;
        }

        public void MakeIcon(HealthManager health, HealthStatus status, Sprite sprite)
        {
            var owner = health.Owner.transform;
            var iconPos = owner.position + offset;
            var instance = Instantiate(iconPrefab, iconPos, Quaternion.identity, owner);
            instance.sprite = sprite;
            status.OnEndEvent += () => Destroy(instance.gameObject);
        }
    }
}