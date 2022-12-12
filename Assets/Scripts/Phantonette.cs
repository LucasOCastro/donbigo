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
                Tile tile = GridManager.Instance.Grid.MouseOverTile();
                if (tile == null) {
                    Debug.Log("NULL");
                }
                else {
                    Debug.Log($"{tile.Pos} ({tile.Type})");
                }
                if (tile != null && tile.Entity == null)
                {
                   Tile = tile;
                }
            }
        }
    }
}