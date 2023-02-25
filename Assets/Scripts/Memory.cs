using System.Collections.Generic;
using System.Linq;
using DonBigo.Actions;
using DonBigo.Rooms;
using UnityEngine;

namespace DonBigo
{
    public class Memory
    {
        private readonly HashSet<Vector2Int> _usedDoors = new HashSet<Vector2Int>();
        private readonly HashSet<RoomInstance> _visitedRooms = new HashSet<RoomInstance>();
        //Os ultimos (mais da direita) foram visitados mais recentemente.
        private readonly List<RoomInstance> _roomVisitMemory = new List<RoomInstance>();
        private readonly HashSet<RoomInstance> _recordedDeadEnds = new HashSet<RoomInstance>();
        private readonly Dictionary<TileObject, Tile> _locationMemory = new Dictionary<TileObject, Tile>();

        public bool Visited(RoomInstance room) => _visitedRooms.Contains(room);
        public bool Used(RoomExit door) => _usedDoors.Contains(door.Position);

        /// <summary>O indice de quantas salas atras room foi visitada.</summary>
        /// <returns>-1 significa que nunca foi visitado. 0 significa que é a sala atual. 1 significa que é a sala que foi visitada mais recentemente.</returns>
        public int RoomVisitedOrder(RoomInstance room)
        {
            int index = _roomVisitMemory.IndexOf(room);
            if (index < 0) return -1;
            return _roomVisitMemory.Count - 1 - index;
        }

        public bool RoomFullyExplored(RoomInstance room) => Visited(room) && room.Doors.All(Used);

        public bool IsDeadEnd(RoomInstance room) => _recordedDeadEnds.Contains(room);

        public Tile LastSeenTile(TileObject obj) => _locationMemory.GetOrDefault(obj);

        public void RememberBeingAt(Tile tile)
        {
            RoomInstance room = tile.Room;
            if (_visitedRooms.Contains(room))
            {
                _roomVisitMemory.Remove(room);
            }
            _visitedRooms.Add(room);
            _roomVisitMemory.Add(room);
            if (room.Doors.Count <= 1)
            {
                _recordedDeadEnds.Add(room);
            }
        }

        public void RememberAction(Action action)
        {
            foreach (var tilePos in action.Doer.VisibleTiles)
            {
                var tile = action.Doer.Tile?.ParentGrid[tilePos];
                if (tile == null) continue;
                if (tile.Entity != null) _locationMemory.SetOrAdd(tile.Entity, tile);
                if (tile.Item != null) _locationMemory.SetOrAdd(tile.Item, tile);
            }
            
            if (action is UseDoorAction useDoorAction)
            {
                _usedDoors.Add(useDoorAction.Exit.Position);
            }
        }
    }
}