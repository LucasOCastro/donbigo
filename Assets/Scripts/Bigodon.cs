using UnityEngine;

namespace DonBigo
{
    public class Bigodon : Entity
    {
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
                   this.GetComponent<Entity>().Walk(tile);
                   FieldOfViewRenderer.OriginTile = this.GetComponent<Entity>().Tile.Pos;
                }
            }
        }
    }
}