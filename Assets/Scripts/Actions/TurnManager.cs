using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DonBigo.Actions
{
    public class TurnManager : MonoBehaviour
    {
        [SerializeField] private float turnDurationSeconds = 1f;
        public float TurnDuration => turnDurationSeconds;
        
        public static TurnManager Instance { get; private set; }
        public static int CurrentTurn { get; private set; }

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
        }

        private void CycleEntity() => _entityIndex = (_entityIndex + 1) % _entities.Count;

        private void OnEnable()
        {
            StartCoroutine(TurnTickCoroutine());
        }
        private void OnDisable()
        {
            StopAllCoroutines();
        }
        
        private IEnumerator TurnTickCoroutine()
        {
            while (true)
            {
                while (_entities.Count == 0)
                {
                    yield return null;
                }
                
                if (_entityIndex == 0)
                {
                    //Só cycla o turno na primeira entidade
                    CurrentTurn++;
                }

                Action action;
                do
                {
                    // Primeiro verifica se o HealthManager tem alguma ação prioritária, depois roda o GetAction da entidade.
                    action = CurrentEntity.Health.Tick();
                    action ??= CurrentEntity.GetAction();
                    yield return null;
                } while (action == null);
                
                action.Execute();
                CurrentEntity.OnExecuteAction?.Invoke(action);
                CycleEntity();
                yield return new WaitForSeconds(turnDurationSeconds);
            }
        }
    }
}