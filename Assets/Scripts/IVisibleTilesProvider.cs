using System.Collections.Generic;
using UnityEngine;

namespace DonBigo
{
    public interface IVisibleTilesProvider
    {
        Tile Tile { get; }
        
        public delegate void OnUpdateViewDelegate(HashSet<Vector2Int> oldTiles, HashSet<Vector2Int> newTiles);
        event OnUpdateViewDelegate OnUpdateViewEvent;
        
        HashSet<Vector2Int> VisibleTiles { get; }

        bool IsVisible(Vector2Int tile) => (VisibleTiles != null) && VisibleTiles.Contains(tile);
    }
}