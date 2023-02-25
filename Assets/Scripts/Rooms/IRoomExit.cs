using DonBigo.Actions;
using UnityEngine;

namespace DonBigo.Rooms
{
    public interface IRoomExit
    {
        Tile UseTile(GameGrid grid);
        Vector2Int Position { get; }
        Action GenAction(Entity doer);
    }
}