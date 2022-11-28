using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DonBigo
{
    public class CharacterManager : MonoBehaviour
    {
        GameObject donbigo;
        [SerializeField] private Sprite[] donbigoSprite;
        void Start()
        {
            donbigo = new GameObject("Player", typeof(SpriteRenderer), typeof(Bigodon));
            SpriteRenderer DBRenderer = donbigo.GetComponent<SpriteRenderer>();
            DBRenderer.sprite = donbigoSprite[7];
            donbigo.GetComponent<Entity>().Walk(GridManager.Instance.Grid.WorldToTile(new Vector2(0,0)));
            // donbigo.GetComponent<SpriteRenderer>().sortingOrder = 1;
        }

        // Update is called once per frame
        void Update()
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
                    donbigo.GetComponent<Entity>().Walk(tile);
                }
            }
        }
    }
}