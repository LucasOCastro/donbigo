using UnityEngine;

namespace DonBigo
{
    public class Phantonette : Entity
    {
        public override Action GetAction()
        {
            return new IdleAction(this);
        }

        private void Update() 
        {
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
                   this.GetComponent<Entity>().Walk(tile);
                }
            }
        }
    }
}