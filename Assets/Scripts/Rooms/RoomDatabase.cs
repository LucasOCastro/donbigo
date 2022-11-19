using UnityEngine;
using System.Linq;

namespace DonBigo.Rooms
{
    //Mais tarde, seria possivel implementar um sistema de pesos/raridade.
    public static class RoomDatabase
    {
        private const string RoomsPath = "Rooms";
        private static Room[] _rooms;

        public static Room[] Rooms
        {
            get
            {
                if (_rooms == null)
                {
                    _rooms = Resources.LoadAll<Room>(RoomsPath);
                }
                return _rooms;
            }
        }
        
        public static Room RandomRoom() => Rooms.Random();

        private static bool RoomFits(GameGrid grid, Vector2Int pos, Room room, RoomExit door)
        {
            RectInt gridBounds = grid.Bounds;
            RectInt realBounds = new RectInt(pos - door.Position, (Vector2Int)room.Size);
            if (!gridBounds.Contains(realBounds.min) || !gridBounds.Contains(realBounds.max - Vector2Int.one))
            {
                return false;
            }

            foreach (var tile in realBounds.allPositionsWithin)
            {
                if (grid[tile] != null)
                {
                    return false;
                }
            }
            return true;
        }

        public static Room RandomRoomThatFits(GameGrid grid, RoomExit door, out RoomExit chosenDoor)
        {
            Vector2Int conPos = door.Position + door.DirectionVector;
            if (!grid.InBounds(conPos)) //Se a porta dá pra fora da grid, nenhuma sala caberá.
            {
                chosenDoor = default;
                return null;
            }

            bool Validation(Room r, RoomExit d) => d.IsOpposed(door.ExitDirection) && RoomFits(grid, conPos, r, d);
            var chosen = Rooms.Where(r => r.Doors.Any(d => Validation(r, d))).Random();
            if (chosen == null)
            {
                chosenDoor = default;
                return null;
            }
            
            chosenDoor = chosen.Doors.First(d => Validation(chosen, d));
            return chosen;
        }
            
    }
}