using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace DonBigo
{
    [CreateAssetMenu(fileName = "NewTile", menuName = "New Tile")]
    public class TileType : UnityEngine.Tilemaps.Tile
    {
        [CreateTileFromPalette]
        private static TileBase CreateTile(Sprite sprite)
        {
            var tile = CreateInstance<TileType>();
            tile.sprite = sprite;
            tile.name = sprite.name;
            return tile;
        }
        //Informações sobre som de pegada, etc
    }
}