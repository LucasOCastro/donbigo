using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DonBigo.Rooms
{
    public class RoomInstance
    {
        public Room Room { get; }
        public RectInt Bounds { get; }

        /// <summary>
        /// Array de portas em espaço global no mapa.
        /// </summary>
        public List<RoomExit> Doors { get; } = new List<RoomExit>();

        public List<Vent> Vents { get; } = new List<Vent>();

        public RoomInstance(Room room, Vector2Int pos)
        {
            Room = room;
            Bounds = new RectInt(pos, (Vector2Int)room.Size);
            Doors = new List<RoomExit>(room.Doors.Length);
            foreach (var door in room.Doors)
            {
                Vector2Int realPos = pos + door.Position;
                Doors.Add(new RoomExit(realPos, door.ExitDirection, door.Marker));
            }
        }

        public RoomInstance()
        {
        }

        public bool IsOuterWall(Tile tile)
        {
            return Bounds.Contains(tile.Pos) && (tile.Pos.x == Bounds.xMax - 1 || tile.Pos.y == Bounds.yMax - 1);
        }
    }
}