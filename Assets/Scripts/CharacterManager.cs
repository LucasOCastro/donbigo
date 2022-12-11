using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DonBigo;

namespace DonBigo
{
    public class CharacterManager : MonoBehaviour
    {
        public GameObject donbigo;
        [SerializeField] private Sprite[] donbigoSprite;
        public GameObject phantonette;
        [SerializeField] private Sprite[] phantonetteSprite;
        // private GameGrid grid;
        private void Start()
        {
            Spawner spawner = new Spawner();
            // Geração do Don Bigo
            donbigo = new GameObject("Player", typeof(SpriteRenderer), typeof(Bigodon));
            SpriteRenderer DBRenderer = donbigo.GetComponent<SpriteRenderer>();
            DBRenderer.sprite = donbigoSprite[7];
            donbigo.tag = "Player";
            // Abaixo setar a posição quando tiver um Spawn implementado vvvvvvvvvvvvv
            // donbigo.GetComponent<Entity>().Walk(GridManager.Instance.Grid.WorldToTile(new Vector2(0,0)));
            spawner.Spawn(GridManager.Instance.Grid, donbigo.GetComponent<Entity>());
            
            // Geração da Phantonette
            phantonette = new GameObject("Foe", typeof(SpriteRenderer), typeof(Phantonette));
            SpriteRenderer PTRenderer = phantonette.GetComponent<SpriteRenderer>();
            PTRenderer.sprite = phantonetteSprite[7];
            // Abaixo setar a posição quando tiver um Spawn implementado vvvvvvvvvvvvv
            phantonette.GetComponent<Entity>().Walk(GridManager.Instance.Grid.WorldToTile(new Vector2(0,0)));
        }
    }
}