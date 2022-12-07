using UnityEngine;
using UnityEngine.Tilemaps;

namespace DonBigo
{
    public class RandomStructureTileType : StructureTileType
    {
        [SerializeField] private Sprite[] randomSprites;

        private static int RandIndex(Vector3Int pos, int mod)
        {
            int val = (pos.x * pos.y) % 17 + 
                      (pos.y * pos.y * pos.x) % 2 + 
                      (pos.x * pos.x * pos.y) % 5 +
                      (pos.z) % 13;
            return val % mod;
        }
        
        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {
            base.GetTileData(position, tilemap, ref tileData);
            if (randomSprites != null && randomSprites.Length > 0)
            {
                tileData.sprite = randomSprites[RandIndex(position, randomSprites.Length)];
            }
        }
    }
}