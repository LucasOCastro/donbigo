﻿using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace DonBigo
{
    [ScriptableObjectIcon("sprite")]
    [CreateAssetMenu(fileName = "NewTile", menuName = "New Tile")]
    public class TileType : UnityEngine.Tilemaps.Tile
    {
        #if UNITY_EDITOR
        [CreateTileFromPalette]
        private static TileBase CreateTile(Sprite sprite)
        {
            var tile = CreateInstance<TileType>();
            tile.sprite = sprite;
            tile.name = sprite.name; 
            return tile;
        }
        #endif

        //Informações sobre som de pegada, etc
    }
}