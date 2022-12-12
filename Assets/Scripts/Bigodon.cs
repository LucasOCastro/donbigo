using System.IO;
using UnityEngine;

namespace DonBigo
{
    public class Bigodon : Entity
    {
        private Path _currentTargetPath;

        private Action GenInteractAction(Tile tile)
        {
            if (!this.Tile.Pos.AdjacentTo(tile.Pos))
            {
                return null;
            }

            return tile.GenInteractAction(this);
        }

        public override Action GetAction()
        {
            //Se já tem um caminho, segue ele.
            if (_currentTargetPath != null && _currentTargetPath.Valid && !_currentTargetPath.Finished)
            {
                return new MoveAction(this, _currentTargetPath.Advance());
            }

            //Se não clicou, não tem ação pra gerar.
            if (!Input.GetMouseButtonDown(0))
            {
                return null;
            }
            
            Tile tile =  GridManager.Instance.Grid.MouseOverTile();
            if (tile == null)
            {
                return null;
            }

            //Se clicou em si mesmo, espera um turno.
            if (tile.Entity == this)
            {
                return new IdleAction(this);
            }

            //Se a tile tem uma ação de interação, retorna ela.
            var interactAction = GenInteractAction(tile);
            if (interactAction != null)
            {
                return interactAction;
            }
            
            //Se nenhuma outra ação foi criada, então cria um caminho pra seguir.
            if (tile.Entity == null && tile.Walkable)
            {
                Path path = new Path(this.Tile, tile);
                _currentTargetPath = (path.Valid && !path.Finished) ? path : null;    
            }
            return null;
            
            /*if (_currentTargetPath == null || !_currentTargetPath.Valid || _currentTargetPath.Finished)
            {
                Path path = new Path(this.Tile, tile);
                _currentTargetPath = (path.Valid && !path.Finished) ? path : null;
            }
            return (_currentTargetPath != null) ? new MoveAction(this, _currentTargetPath.Advance()) : null;*/
        }
    }
}