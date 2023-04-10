using DonBigo.Actions;
using UnityEngine;

namespace DonBigo
{
    [RequireComponent(typeof(Entity), typeof(AudioSource))]
    public class EntitySoundManager : MonoBehaviour
    {
        private AudioSource _source;
        private Entity _entity;

        [SerializeField] private AudioClip moveSound;
        [SerializeField] private AudioClip doorSound;

        private void Awake()
        {
            _source = GetComponent<AudioSource>();
            _entity = GetComponent<Entity>();
        }

        private void ProcessAction(Action action)
        {
            if (Settings.MuteSfx) return;
            
            var clip = action switch
            {
                UseDoorAction => doorSound,
                MoveAction => moveSound,
                _ => null
            };
            
            if (clip != null) _source.PlayOneShot(clip);
        }
        
        private void OnEnable()
        {
            _entity.OnExecuteAction += ProcessAction;
        }
        private void OnDisable()
        {
            _entity.OnExecuteAction -= ProcessAction;
        }
    }
}