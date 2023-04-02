using System;
using DonBigo.Actions;
using UnityEngine;
using System.Collections.Generic;

namespace DonBigo
{
    public class CharacterManager : MonoBehaviour
    {
        public static CharacterManager Instance { get; private set; }
        
        public static Bigodon DonBigo { get; private set; }
        [SerializeField] private Bigodon donbigoPrefab;
        public static Phantonette Phantonette { get; private set; }
        [SerializeField] private Phantonette phantonettePrefab;

        public static IEnumerable<Entity> AllEntities { get; private set; }

        private void Start()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            
            
            Spawner spawner = new Spawner();
            // Geração do Don Bigo
            DonBigo = Instantiate(donbigoPrefab);
            spawner.Spawn(GridManager.Instance.Grid, DonBigo);
            
            // Geração da Phantonette
            Phantonette = Instantiate(phantonettePrefab);
            spawner.Spawn(GridManager.Instance.Grid, Phantonette);
            
            TurnManager.RegisterEntity(DonBigo);
            TurnManager.RegisterEntity(Phantonette);
            FieldOfViewRenderer.Origin = DonBigo;

            DonBigo.Health.OnDeathEvent += () => GameEnder.Instance.EndGame(GameEnder.Condition.Defeat);
            Phantonette.Health.OnDeathEvent += () => GameEnder.Instance.EndGame(GameEnder.Condition.Victory);
            
            AllEntities = new Entity[] { DonBigo, Phantonette };
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                Tile tile = GridManager.Instance.Grid.MouseOverTile();
                if (tile == null || !tile.Walkable || tile.Entity != null) return;
                DonBigo.Tile = tile;
            }
        }
#endif
    }
}