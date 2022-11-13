using System.Collections.Generic;
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
        public List<RoomExit> Doors { get; }

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
    }
}