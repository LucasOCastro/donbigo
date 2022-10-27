using UnityEngine;

namespace DonBigo
{
    public class Tile
    {
        public Vector2Int Pos { get; }
        public TileType Type { get; }
        public GameGrid ParentGrid { get; }
    
        //TODO ainda não temos um modo de settar o TileType na geração de comodos
        public Tile(Vector2Int pos, GameGrid grid)
        {
            Pos = pos;
            ParentGrid = grid;
        }

        private Item _item;
        public Item Item
        {
            get => _item;
            set
            {
                if (_item != null)
                {
                    Debug.LogError("Tentou colocar item onde já tem item");
                    return;
                }
                _item = value;
                if (_item != null && _item.Tile != this)
                {
                    _item.Tile = this;
                }
            }
        }
        
        public Entity Entity { get; set; }
    }
}
