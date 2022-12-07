using System.Collections.Generic;
using UnityEngine;

namespace DonBigo
{
    public class TurnManager : MonoBehaviour
    {
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
        }

        private void CycleEntity() => _entityIndex = (_entityIndex + 1) % _entities.Count;

        public void Update()
        {
            if (_entities.Count == 0) return;
            
            //TODO as entidades devem ter um método que espera a seleção de uma action a ser realizada.
            Action action = new MoveAction(CurrentEntity, CurrentEntity.Tile); //CurrentEntity.GetAction();
            if (action != null)
            {
                action.Execute();
                CycleEntity();
            }
        }
    }
}