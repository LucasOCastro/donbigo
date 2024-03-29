﻿using UnityEngine;
using UnityEngine.Tilemaps;

namespace DonBigo
{
    public class VentTileType : StructureTileType
    {
        [SerializeField] private Sprite closedSprite, openSprite;
        [SerializeField] private Vector2Int direction;
        public Vector2Int Direction => direction;

        public override StructureInstance GetInstance(Tile tile, int elevation)
        {
            return new Vent(this, tile, elevation);
        }

        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {
            base.GetTileData(position, tilemap, ref tileData);
            if (!Application.isPlaying) return;
            if (GridManager.Instance == null || GridManager.Instance.Grid == null) return;

            var tile = GridManager.Instance.Grid[position.x, position.y];
            Vent vent = tile?.Structures.FindOfType<StructureInstance, Vent>();
            if (vent == null) return;

            tileData.sprite = vent.Open ? openSprite : closedSprite;
        }
    }
}