using System;
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

        public IRoomExit GetExit(Func<IRoomExit, float> scoreFunc, Func<IRoomExit, bool> guaranteedSelect,
            bool allowVents, bool useRandom)
        {
            IEnumerable<IRoomExit> exits = Doors.Select(d => d as IRoomExit);

            var openVents = Vents.Where(v => v.Open);
            int openVentCount = openVents.Count();
            if (openVentCount > 0 && allowVents && GridManager.Instance.Grid.CanUseVents)
            {
                exits = exits.Concat(openVents);
            }

            if (guaranteedSelect != null)
            {
                var guaranteedExit = exits.FirstOrDefault(guaranteedSelect);
                if (guaranteedExit != null) return guaranteedExit;
            }

            bool IsBetter(IRoomExit a, IRoomExit b) => scoreFunc(a) > scoreFunc(b);
            return useRandom ? exits.RandomElementByWeight(scoreFunc) : exits.Best(IsBetter);
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