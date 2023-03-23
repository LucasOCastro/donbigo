using System.Collections.Generic;
using DonBigo.Actions;
using DonBigo.UI;
using UnityEngine;
using Action = DonBigo.Actions.Action;

namespace DonBigo
{
    public class Bigodon : Entity
    {
        private Path _currentTargetPath;
        private bool _seesPhantonette;
        private Action _pathEndAction;
        private Tile _pathEndTile;

        //Pra evitar cliques em portas não sendo considerados, clicar fora duma sala tenta achar algo interativel perto do player
        private Tile CastFromShadows(Vector2Int tile)
        {
            const int maxDistance = 30;
            const int forwardSweepRange = 1;
            const int sideSweepRange = 1;

            var grid = Tile.ParentGrid;
            var room = Tile.Room;
            var bounds = room.Bounds;
            if (tile.x < bounds.min.x || tile.y < bounds.min.y)
            {
                return null;
            }
            
            IEnumerable<Tile> CastTiles()
            {
                for (int i = 0; i <= forwardSweepRange; i++)
                {
                    for (int j = -sideSweepRange; j <= sideSweepRange; j++)
                    {
                        Vector2Int offset = (tile.x >= bounds.max.x) ? new Vector2Int(i, j) : new Vector2Int(j, i);
                        Vector2Int checkTilePos = Tile.Pos + offset;
                        int distance = checkTilePos.ManhattanDistance(tile);
                        if (grid[checkTilePos] == null || distance > maxDistance || !VisibleTiles.Contains(checkTilePos))
                        {
                            continue;
                        }

                        var checkTile = grid[checkTilePos];
                        if (checkTile.Type is DoorTileType
                            || checkTile.Structures.Exists(s => s is Vent || s.Type is EntranceMarkerTile)
                            || (checkTile.Item && checkTile.Item.CanBePickedUp))
                        {
                            yield return checkTile;    
                        }
                    }
                }
            }

            var chosenTile = CastTiles().Best(
                (t1, t2) => t1.Pos.ManhattanDistance(tile) < t2.Pos.ManhattanDistance(tile)
                );
            return chosenTile;
        }

        private void Update()
        {
            //Quando aperta E, muda a mão ativa do inventário.
            if (Input.GetKeyDown(KeyCode.E))
            {
                Inventory.CycleHandedness();
            }
            
            //Quando aperta Espaço, limpa o caminho atual.
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _currentTargetPath = null;
            }
        }

        
        protected override void UpdateView(HashSet<Vector2Int> oldVisible, HashSet<Vector2Int> newVisible)
        {
            base.UpdateView(oldVisible, newVisible);


            if (CharacterManager.Phantonette == null || CharacterManager.Phantonette.Tile == null) return;
            
            bool sees = newVisible.Contains(CharacterManager.Phantonette.Tile.Pos);
            if (sees != _seesPhantonette)
            {
                _currentTargetPath = null;
            }
            _seesPhantonette = sees;
        }

        private void MakePathTo(Tile tile)
        {
            Path path = new Path(this.Tile, tile, this, allowShorterPath: true);
            _currentTargetPath = (path.Valid && !path.Finished) ? path : null; 
        }

        public override Action GetAction()
        {
            TileHighlighter.Highlight(null);
            
            //Quando aperta espaço, pula um turno.
            if (_currentTargetPath == null && Input.GetKey(KeyCode.Space))//GetKeyDown(KeyCode.Space))
            {
                return new IdleAction(this);
            }
            
            //Se já tem um caminho, segue ele.
            if (_currentTargetPath != null && _currentTargetPath.Valid && !_currentTargetPath.Finished)
            {
                var advance = _currentTargetPath.Advance();
                if (advance.Entity == null)
                {
                    return new MoveAction(this, advance);
                }
                _currentTargetPath = null;
            }

            //Se temos uma ação encaminhada para o fim do caminho e chegamos, retorna a ação.
            //Caso não chegamos, o caminho foi cancelado e podemos descartar.
            if (_pathEndAction != null)
            {
                bool adjacent = _pathEndTile.Pos.AdjacentTo(Tile.Pos);
                var action = _pathEndAction;
                _pathEndAction = null;
                _pathEndTile = null;
                if (adjacent) return action;
            }

            //Se apertou Q e ta segurando item, droppa.
            if (Inventory.CurrentHand != null && Tile.SupportsItem && Input.GetKeyDown(KeyCode.Q))
            {
                return new DropAction(this, Tile);
            }

            var mousePos = GridManager.Instance.Grid.MouseOverPos();
            var tile =  GridManager.Instance.Grid.MouseOverTile();
            
            //Se tem um item na mao e aperta o botão direito, tenta usar o item.
            Item heldItem = Inventory.CurrentHand; 
            if (tile != null && VisibleTiles.Contains(mousePos) && heldItem != null && Input.GetMouseButtonDown(1) && heldItem.CanBeUsed(this, tile))
            {
                return new UseItemAction(this, heldItem, tile);
            }

            if (tile == null || tile.Type is WallTileType and not DoorTileType || tile.Room != Tile.Room)
            {
                tile = CastFromShadows(mousePos);
                if (tile == null) return null;
            }
            TileHighlighter.Highlight(tile);
            
            //Clique esquerdo
            if (Input.GetMouseButtonDown(0))
            {
                if (Vector2.Angle(tile.Pos - Tile.Pos, LookDirection) > VisionAngle*.5f)
                {
                    return new TurnAction(this, (tile.Pos - Tile.Pos).Sign());
                }
                
                //Gera uma ação de interação na tile
                var interactAction = tile.GenInteractAction(this);
                if (interactAction != null)
                {
                    //Se existe e estamos adjacentes, retorna a interação.
                    if (tile.Pos.AdjacentTo(Tile.Pos)) return interactAction;
                    //Caso contrário, cria um caminho e agenda a interação.
                    MakePathTo(tile);
                    _pathEndAction = interactAction;
                    _pathEndTile = tile;
                    return null;
                }
                
                //Se clicou em si mesmo, espera um turno.
                if (tile.Entity == this)
                {
                    return new IdleAction(this);
                }
                
                //Se nenhuma outra ação foi criada, então cria um caminho pra seguir.
                MakePathTo(tile);
            }
            
            return null;
        }
    }
}