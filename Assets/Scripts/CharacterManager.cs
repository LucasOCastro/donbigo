using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DonBigo
{
    public class CharacterManager : MonoBehaviour
    {
        public GameObject donbigo;
        [SerializeField] private Sprite[] donbigoSprite;
        public GameObject phantonette;
        [SerializeField] private Sprite[] phantonetteSprite;

        // private List<Tile> walkPath;
        // private int pathIndex;
        private void Start()
        {
            donbigo = new GameObject("Player", typeof(SpriteRenderer), typeof(Bigodon));
            SpriteRenderer DBRenderer = donbigo.GetComponent<SpriteRenderer>();
            DBRenderer.sprite = donbigoSprite[7];
            donbigo.tag = "Player";
            donbigo.GetComponent<Entity>().Walk(GridManager.Instance.Grid.WorldToTile(new Vector2(0,0)));
            
            phantonette = new GameObject("Foe", typeof(SpriteRenderer), typeof(Phantonette));
            SpriteRenderer PTRenderer = phantonette.GetComponent<SpriteRenderer>();
            PTRenderer.sprite = phantonetteSprite[7];
            phantonette.GetComponent<Entity>().Walk(GridManager.Instance.Grid.WorldToTile(new Vector2(0,0)));
            // donbigo.GetComponent<SpriteRenderer>().sortingOrder = 1;
        }

        // Update is called once per frame
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 mousePos = Input.mousePosition;
                Vector2 mouseScreenPos = Camera.main.ScreenToWorldPoint(mousePos);
                Tile tile = GridManager.Instance.Grid.WorldToTile(mouseScreenPos);
                if (tile == null) {
                    Debug.Log("NULL");
                }
                else {
                    Debug.Log($"{tile.Pos} ({tile.Type})");
                }
                if (tile != null && tile.Item == null)
                {
                    // walkPath = PathFinding.Path(donbigo.GetComponent<Entity>().Tile, tile);
                    // pathIndex = 0;
                   donbigo.GetComponent<Entity>().Walk(tile);
                   FieldOfViewRenderer.OriginTile = donbigo.GetComponent<Entity>().Tile.Pos;
                }

                
            }
            // if (walkPath != null)
            // {
            //     if (pathIndex < walkPath.Count)
            //     {
            //         donbigo.GetComponent<Entity>().Walk(walkPath[pathIndex++]);
            //     }
            //     else
            //     {
            //         pathIndex = 0;
            //         walkPath = null;
            //     }
            // }
            if (Input.GetKeyDown(KeyCode.F))
            {
                Vector2 mousePos = Input.mousePosition;
                Vector2 mouseScreenPos = Camera.main.ScreenToWorldPoint(mousePos);
                Tile tile = GridManager.Instance.Grid.WorldToTile(mouseScreenPos);
                if (tile == null) {
                    Debug.Log("NULL");
                }
                else {
                    Debug.Log($"{tile.Pos} ({tile.Type})");
                }
                if (tile != null && tile.Item == null)
                {
                   phantonette.GetComponent<Entity>().Walk(tile);
                }
            }
        }
    }
}