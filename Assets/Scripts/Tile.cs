using System;
using System.Collections.Generic;
using UnityEngine;

namespace DonBigo
{
    public class Tile
    {
        public Vector2Int Pos { get; }
        public TileType Type { get; }
        public GameGrid ParentGrid { get; }
    
        //TODO ainda não temos um modo de settar o TileType na geração de comodos
        public Tile(Vector2Int pos, TileType tileType, GameGrid grid)
        {
            Pos = pos;
            Type = tileType;
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

        private List<StructureTileType> _structure;
        public List<StructureTileType> Structures
        {
            //Isso ainda não tem nenhum suporte a mudar a estrutura duma tile durante o jogo
            //Possivelmente vamos precisar de uma StructureInstance que carrega informaçao de dano, etc
            get => _structure;
            set => _structure = value;
        }
        
        public Entity Entity { get; set; }
    }
}
