using UnityEngine;
using UnityEngine.Tilemaps;

namespace DonBigo.Rooms.MapGeneration
{
    public static class DecorationPlacer
    {
        private static void PlaceDecoration(Vector3Int pos, MapGenData.DecorationData decData, Tilemap tm, bool flipX)
        {
            var decType = decData.decorations.Random();
            if (decType == null) return;

            pos.z = Random.Range(decData.elevationRange.x, decData.elevationRange.y);
            if (tm.GetTile(pos) != null) return;
                
            tm.SetTile(pos, decType);
                
            if (flipX)
            {
                var flipMatrix = Matrix4x4.Scale(new Vector3(-1, 1, 1));
                tm.SetTransformMatrix(pos, flipMatrix);
            }
        }

        private static MapGenData.DecorationData? GetData(Tile tile, MapGenData data, out bool flip)
        {
            var grid = tile.ParentGrid;
            switch (tile.Type)
            {
                case WallTileType:
                {
                    //flip = tile.Pos.y == tile.Room.Bounds.max.y - 1;
                    flip = grid[tile.Pos + Vector2Int.left]?.Type is not WallTileType &&
                           grid[tile.Pos + Vector2Int.right]?.Type is not WallTileType;
                    
                    //As paredes de cantos nao sao visiveis entao nao precisa de decoração
                    bool inaccessible = grid[tile.Pos + Vector2Int.down]?.Type is WallTileType &&
                                        grid[tile.Pos + Vector2Int.left]?.Type is WallTileType;
                    return inaccessible ? null : data.wallDecorationData;
                }
                default:
                {
                    flip = Random.value < .5f;

                    bool isCorner = grid[tile.Pos + Vector2Int.up]?.Type is WallTileType &&
                                    grid[tile.Pos + Vector2Int.right]?.Type is WallTileType;
                    return isCorner ? data.cornerDecorationData : data.floorDecorationData;
                }
            }
        }
        
        public static void PlaceDecorations(RoomInstance room, GameGrid grid, MapGenData data)
        {
            foreach (var tile in grid.TilesInBounds(room.Bounds))
            {
                var decData = GetData(tile, data, out bool flip);
                if (decData == null) continue;

                if (Random.value < decData.Value.chance)
                {
                    Vector3Int tmPos = (Vector3Int)tile.Pos;
                    PlaceDecoration(tmPos, decData.Value, data.tilemap, flip);    
                }
            }
        }
    }
}