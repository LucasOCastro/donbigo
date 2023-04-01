using System;
using System.Collections;
using UnityEngine;

namespace DonBigo.UI
{
    public class Shaker : MonoBehaviour
    {
        [SerializeField] private Vector2 amplitude;
        [SerializeField] private Vector2 frequency;
        [SerializeField] private Vector2 duration;
        [SerializeField] private Vector2 chance;
        [SerializeField] private float cooldown;

        [SerializeField] private bool lockX, lockY;
        
        public Vector2 ShakeOffset { get; private set; }

        private float _cooldownTimer;
        private bool LockX => lockX || _cooldownTimer < cooldown;//|| (!allowSimultaneous && lockY);
        private bool LockY => lockY || _cooldownTimer < cooldown;//|| (!allowSimultaneous && lockX);

        private Vector2 _timer;
        private void LateUpdate()
        {
            _timer += Vector2.one * Time.deltaTime;
            _cooldownTimer += Time.deltaTime;

            if (!LockX && _timer.x > frequency.x)
            {
                _timer.x = 0;
                if (RandomUtility.Chance(chance.x))
                {
                    Vector2 dir = Vector2.right * (amplitude.x * RandomUtility.Sign());
                    lockX = true;
                    StartCoroutine(NoiseAxisCoroutine(dir, duration.x, () => lockX = false));    
                }
            }
            
            if (!LockY && _timer.y > frequency.y)
            {
                _timer.y = 0;
                if (RandomUtility.Chance(chance.y))
                {
                    Vector2 dir = Vector2.up * (amplitude.y * RandomUtility.Sign());
                    lockY = true;
                    StartCoroutine(NoiseAxisCoroutine(dir, duration.y, () => lockY = false));    
                }
            }
        }

        private IEnumerator NoiseAxisCoroutine(Vector2 offset, float seconds, Action endAction)
        {
            _cooldownTimer = 0;

            ShakeOffset += offset;
            yield return new WaitForSeconds(seconds);
            ShakeOffset -= offset;
            endAction?.Invoke();
        }
    }
}