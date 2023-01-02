using System;
using UnityEngine;

namespace DonBigo
{
    [Serializable]
    public class DirectionalSpriteSet
    {
        public static readonly Vector2Int[] SpriteOrder =
        {
            new Vector2Int(0, 1),
            new Vector2Int(1, 1),
            new Vector2Int(1, 0),
            new Vector2Int(1, -1),
            new Vector2Int(0, -1),
            new Vector2Int(-1, -1),
            new Vector2Int(-1, 0),
            new Vector2Int(-1, 1),
        };

        [SerializeField] private Sprite[] sprites;
        
        public Sprite GetDirectionalSprite(Vector2Int dir)
        {
            if (sprites.Length != SpriteOrder.Length)
            {
                Debug.LogError("Quantidade de sprite diferente do necessário!");
                return null;
            }
            
            int index = Array.IndexOf(SpriteOrder, dir);
            if (index < 0) return null;
            return sprites[index];
        }

        public Sprite GetDirectionalSprite(Tile from, Tile to)
        {
            if (from == null || to == null)
            {
                return GetDirectionalSprite(SpriteOrder[0]);
            }
            Vector2Int dir = (to.Pos - from.Pos).Sign(); 
            return GetDirectionalSprite(dir);
        }
    }
}