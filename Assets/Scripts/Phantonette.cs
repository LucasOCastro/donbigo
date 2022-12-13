using DonBigo.Actions;
using UnityEngine;

namespace DonBigo
{
    public class Phantonette : Entity
    {
        private Path _targetPath;
        public override Action GetAction()
        {
            if (_targetPath != null && _targetPath.Valid && !_targetPath.Finished)
            {
                return new MoveAction(this, _targetPath.Advance());
            }
            
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
                    _targetPath = new Path(Tile, tile);
                }
            }
        }
    }
}