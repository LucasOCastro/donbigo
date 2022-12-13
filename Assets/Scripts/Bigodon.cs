using DonBigo.Actions;
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
            //Quando aperta X, muda a mão ativa do inventário.
            if (Input.GetKeyDown(KeyCode.X))
            {
                Inventory.CycleHandedness();
            }
        
            //Se já tem um caminho, segue ele.
            if (_currentTargetPath != null && _currentTargetPath.Valid && !_currentTargetPath.Finished)
            {
                return new MoveAction(this, _currentTargetPath.Advance());
            }

            //Se apertou Q e ta segurando item, droppa.
            if (Inventory.CurrentHand != null && Input.GetKeyDown(KeyCode.Q))
            {
                return new DropAction(this, Tile);
            }
            
            Tile tile =  GridManager.Instance.Grid.MouseOverTile();
            //Se tem um item na mao e aperta o botão direito, tenta usar o item.
            Item heldItem = Inventory.CurrentHand; 
            if (heldItem != null && Input.GetMouseButtonDown(1) && heldItem.CanBeUsed(this, tile))
            {
                return new UseItemAction(this, heldItem, tile);
            }

            //Clique esquerdo
            if (Input.GetMouseButtonDown(0) && tile != null)
            {
                //Se a tile tem uma ação de interação, retorna ela.
                var interactAction = GenInteractAction(tile);
                if (interactAction != null)
                {
                    return interactAction;
                }

                //Se clicou em si mesmo, espera um turno.
                if (tile.Entity == this)
                {
                    return new IdleAction(this);
                }
                
                //Se nenhuma outra ação foi criada, então cria um caminho pra seguir.
                if (tile.Entity == null && tile.Walkable)
                {
                    Path path = new Path(this.Tile, tile);
                    _currentTargetPath = (path.Valid && !path.Finished) ? path : null;    
                }
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