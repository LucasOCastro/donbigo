using DonBigo.Actions;
using UnityEngine;

namespace DonBigo
{
    public class CharacterManager : MonoBehaviour
    {
        public static CharacterManager Instance { get; private set; }
        
        public static Bigodon DonBigo { get; private set; }
        [SerializeField] private DirectionalSpriteSet donbigoSprite;
        public static Phantonette Phantonette { get; private set; }
        [SerializeField] private DirectionalSpriteSet phantonetteSprite;

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
            DonBigo = new GameObject("Player", typeof(SpriteRenderer), typeof(Bigodon)).GetComponent<Bigodon>();
            DonBigo.SpriteSet = donbigoSprite;
            DonBigo.tag = "Player";
            // Abaixo setar a posição quando tiver um Spawn implementado vvvvvvvvvvvvv
            spawner.Spawn(GridManager.Instance.Grid, DonBigo.GetComponent<Entity>());
            
            // Geração da Phantonette
            Phantonette = new GameObject("Foe", typeof(SpriteRenderer), typeof(Phantonette)).GetComponent<Phantonette>();
            Phantonette.SpriteSet = phantonetteSprite;
            // Abaixo setar a posição quando tiver um Spawn implementado vvvvvvvvvvvvv
            spawner.Spawn(GridManager.Instance.Grid, Phantonette.GetComponent<Entity>());
            
            TurnManager.RegisterEntity(DonBigo);
            TurnManager.RegisterEntity(Phantonette);
            FieldOfViewRenderer.Origin = DonBigo;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                Tile tile = GridManager.Instance.Grid.MouseOverTile();
                if (tile == null || !tile.Walkable || tile.Entity != null) return;
                DonBigo.Tile = tile;
            }
        }
    }
}