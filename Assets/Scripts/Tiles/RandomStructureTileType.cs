using UnityEngine;
using UnityEngine.Tilemaps;

namespace DonBigo
{
    public class RandomStructureTileType : StructureTileType
    {
        [SerializeField] private Sprite[] randomSprites;

        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {
            base.GetTileData(position, tilemap, ref tileData);
            if (randomSprites != null && randomSprites.Length > 0)
            {
                tileData.sprite = randomSprites.Random();
            }
        }
    }
}