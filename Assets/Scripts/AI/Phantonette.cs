using DonBigo.Actions;
using DonBigo.AI;

namespace DonBigo
{
    public class Phantonette : Entity
    {
        private AIWorker _aiWorker;

        protected override void Awake()
        {
            base.Awake();
            _aiWorker = new AIWorker(this);
        }

        public override Action GetAction()
        {
            return _aiWorker.GetAction() ?? new IdleAction(this);
            /*if (_targetPath != null && _targetPath.Valid && !_targetPath.Finished)
            {
                return new MoveAction(this, _targetPath.Advance());
            }
            
            return new IdleAction(this);*/
        }

        /*private void Update() 
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
        }*/
    }
}