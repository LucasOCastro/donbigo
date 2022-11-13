using UnityEngine;
using UnityEngine.Tilemaps;

namespace DonBigo
{
    public interface IRoomEntranceMarker
    {
        void SetInactive(GameGrid grid, Tilemap tilemap, Vector2Int pos);
    }
}