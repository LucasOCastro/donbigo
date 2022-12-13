using System.Collections;
using System.Collections.Generic;
using DonBigo.Actions;
using UnityEngine;

namespace DonBigo
{
    public class CharacterManager : MonoBehaviour
    {
        public static CharacterManager Instance { get; private set; }
        
        public static Bigodon DonBigo { get; private set; }
        [SerializeField] private Sprite[] donbigoSprite;
        public static Phantonette Phantonette { get; private set; }
        [SerializeField] private Sprite[] phantonetteSprite;

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
            SpriteRenderer DBRenderer = DonBigo.GetComponent<SpriteRenderer>();
            DBRenderer.sprite = donbigoSprite[7];
            DonBigo.tag = "Player";
            // Abaixo setar a posição quando tiver um Spawn implementado vvvvvvvvvvvvv
            DonBigo.Tile = GridManager.Instance.Grid[25, 25];
            spawner.Spawn(GridManager.Instance.Grid, DonBigo.GetComponent<Entity>());
            
            // Geração da Phantonette
            Phantonette = new GameObject("Foe", typeof(SpriteRenderer), typeof(Phantonette)).GetComponent<Phantonette>();
            SpriteRenderer PTRenderer = Phantonette.GetComponent<SpriteRenderer>();
            PTRenderer.sprite = phantonetteSprite[7];
            // Abaixo setar a posição quando tiver um Spawn implementado vvvvvvvvvvvvv
            Phantonette.Tile = GridManager.Instance.Grid[20, 20];
            spawner.Spawn(GridManager.Instance.Grid, Phantonette.GetComponent<Entity>());
            
            TurnManager.RegisterEntity(DonBigo);
            TurnManager.RegisterEntity(Phantonette);
            FieldOfViewRenderer.Origin = DonBigo;
            
        }
    }
}