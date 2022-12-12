using System.Collections;
using System.Collections.Generic;
using DonBigo.Actions;
using UnityEngine;

namespace DonBigo
{
    public class CharacterManager : MonoBehaviour
    {
        public Bigodon donbigo;
        [SerializeField] private Sprite[] donbigoSprite;
        public Phantonette phantonette;
        [SerializeField] private Sprite[] phantonetteSprite;

        private void Start()
        {
            Spawner spawner = new Spawner();
            // Geração do Don Bigo
            donbigo = new GameObject("Player", typeof(SpriteRenderer), typeof(Bigodon)).GetComponent<Bigodon>();
            SpriteRenderer DBRenderer = donbigo.GetComponent<SpriteRenderer>();
            DBRenderer.sprite = donbigoSprite[7];
            donbigo.tag = "Player";
            // Abaixo setar a posição quando tiver um Spawn implementado vvvvvvvvvvvvv
            donbigo.Tile = GridManager.Instance.Grid[25, 25];
            spawner.Spawn(GridManager.Instance.Grid, donbigo.GetComponent<Entity>());
            
            // Geração da Phantonette
            phantonette = new GameObject("Foe", typeof(SpriteRenderer), typeof(Phantonette)).GetComponent<Phantonette>();
            SpriteRenderer PTRenderer = phantonette.GetComponent<SpriteRenderer>();
            PTRenderer.sprite = phantonetteSprite[7];
            // Abaixo setar a posição quando tiver um Spawn implementado vvvvvvvvvvvvv
            phantonette.Tile = GridManager.Instance.Grid[20, 20];
            spawner.Spawn(GridManager.Instance.Grid, phantonette.GetComponent<Entity>());
            
            TurnManager.RegisterEntity(donbigo);
            TurnManager.RegisterEntity(phantonette);
            FieldOfViewRenderer.Origin = donbigo;
            
        }
    }
}