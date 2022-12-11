using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DonBigo
{
    public class TurnManager : MonoBehaviour
    {
        [SerializeField] private float turnDurationSeconds = 1f;
        
        public static TurnManager Instance { get; private set; }

        public static void RegisterEntity(Entity entity) => Instance._entities.Add(entity);
        
        private List<Entity> _entities = new List<Entity>();
        private int _entityIndex;
        public Entity CurrentEntity => _entities[_entityIndex];

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            StartCoroutine(TurnTickCoroutine());
        }

        private void CycleEntity() => _entityIndex = (_entityIndex + 1) % _entities.Count;

        private IEnumerator TurnTickCoroutine()
        {
            while (true)
            {
                while (_entities.Count == 0)
                {
                    yield return null;
                }

                Action action;
                while ((action = CurrentEntity.GetAction()) == null)
                {
                    yield return null;
                }
                
                action.Execute();
                CycleEntity();
                yield return new WaitForSeconds(turnDurationSeconds);
            }
        }

        /*public void Update()
        {
            if (_entities.Count == 0) return;
            
            //TODO as entidades devem ter um método que espera a seleção de uma action a ser realizada.
            Action action = CurrentEntity.GetAction();
            if (action != null)
            {
                action.Execute();
                CycleEntity();
            }
        }*/
    }
}