﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace DonBigo.Rooms
{
    [CreateAssetMenu(fileName = "NewRoom", menuName = "New Room")]
    public class Room : ScriptableObject
    {
        [Serializable]
        public struct StructurePosition
        {
            public Vector3Int pos;
            public StructureTileType structure;
        }
        [Serializable]
        private struct TransformOverride
        {
            public Vector3Int pos;
            public Matrix4x4 matrix;

            public void Override(Tilemap tm, Vector2Int start)
            {
                tm.SetTransformMatrix((Vector3Int)start + pos, matrix);
            }
        }

        [Serializable]
        private struct ItemChance
        {
            public ItemType item;
            [Range(0f,1f)] public float chance;
        }

        [SerializeField] private string roomName;
        [SerializeField] private Vector3Int size;
        [SerializeField] private TileBase[] tilesBlock;
        [SerializeField] private TileType[] tileTypes;
        [SerializeField] private RoomExit[] doors;
        [SerializeField] private StructurePosition[] structureTiles;
        [SerializeField] private TransformOverride[] transformOverrides;
        [SerializeField] private ItemChance[] possibleItems;
        [Range(0f, 1f)]
        [SerializeField] private float ventChance;

        public string RoomName => roomName;
        public Vector3Int Size => size;
        /// <summary>
        /// Array de portas em espaço local na sala.
        /// </summary>
        public RoomExit[] Doors => doors;
        /// <summary>
        /// Array de estruturas em espaço local na sala.
        /// </summary>
        public StructurePosition[] Structures => structureTiles;
        /// <summary>
        /// Chance de ser uma sala com vent, varia de 0 a 1.
        /// </summary>
        public float VentChance => ventChance;

        public IEnumerable<ItemType> GenItemsToSpawn()
        {
            if (possibleItems == null || possibleItems.Length == 0) yield break;
            
            foreach (var itemChance in possibleItems)
            {
                if (RandomUtility.Chance(itemChance.chance))
                {
                    yield return itemChance.item;
                }
            }   
        }

        // A Unity não suporta a serialização de arrays multidimensionais. Então eu serializo numa array normal,
        // transformo em uma array multidimensional quando necessário e guardo num cache.
        private TileType[,] _tiles;
        public TileType[,] Tiles
        {
            get
            {
                if (_tiles != null) return _tiles;
                
                _tiles = new TileType[size.x, size.y];
                for (int i = 0; i < tileTypes.Length; i++)
                {
                    int y = i / size.x;
                    int x = i - (y * size.x);
                    _tiles[x, y] = tileTypes[i];
                }

                return _tiles;
            }
        }
        
        public void FillTilemap(Tilemap tilemap, Vector2Int start)
        {
            BoundsInt fillBounds = new BoundsInt((Vector3Int)start, size);
            tilemap.SetTilesBlock(fillBounds, tilesBlock);
            foreach (var transformOverride in transformOverrides)
            {
                transformOverride.Override(tilemap, start);
            }
        }
    }
}