using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace DonBigo.Rooms
{
    public static class MapGen
    {
        private static void PlaceRoom(GameGrid grid, Tilemap tilemap, RoomInstance roomInstance)
        {
            Room room = roomInstance.Room;
            Vector2Int min = roomInstance.Bounds.min;
            room.FillTilemap(tilemap, min);
            for (int x = 0; x < room.Size.x; x++)
            {
                for (int y = 0; y < room.Size.y; y++)
                {
                    Vector2Int worldPos = new Vector2Int(x, y) + min;
                    grid[worldPos] = new Tile(worldPos, room.Tiles[x, y], grid);
                }
            }

            foreach (var structurePos in room.Structures)
            {
                grid[structurePos.pos + min].Structures.Add(structurePos.structure);
            }
        }

        
        public static List<RoomInstance> Gen(GameGrid grid, Tilemap tilemap)
        {
            //Usar uma Lista e acessar saidas aleatorias ao inves da Queue talvez dê um resultado mais devidamente aleatório.
            List<RoomInstance> rooms = new();
            Queue<RoomExit> possibleDoors = new();

            Room randRoom = RoomDatabase.RandomRoom();
            if (randRoom == null) return rooms;
            Vector2Int center = new Vector2Int((int)grid.Bounds.center.x, (int)grid.Bounds.center.y); 
            RoomInstance roomInstance = new RoomInstance(randRoom, center);
            PlaceRoom(grid, tilemap, roomInstance);
            rooms.Add(roomInstance);
            foreach (var door in roomInstance.Doors) {
                possibleDoors.Enqueue(door);
            }

            while (possibleDoors.Count > 0)
            {
                RoomExit possibleDoor = possibleDoors.Dequeue();
                randRoom = RoomDatabase.RandomRoomThatFits(grid, possibleDoor, out RoomExit chosenDoor);
                if (randRoom == null) 
                {
                    //Se não cabe nenhuma sala nessa porta, então essa porta não será conectada a nada.
                    possibleDoor.Marker.SetInactive(grid, tilemap, possibleDoor.Position);
                    continue;
                }
                //Ajustamos a posição em que a sala será colocada com base na posição da porta que será conectada.
                Vector2Int newRoomMin = (possibleDoor.Position + possibleDoor.DirectionVector) - chosenDoor.Position;
                roomInstance = new RoomInstance(randRoom, newRoomMin);
                PlaceRoom(grid, tilemap, roomInstance);
                rooms.Add(roomInstance);

                foreach (var exit in roomInstance.Doors)
                {
                    //Não adicionamos a porta que já foi conectada na queue
                    if (exit.Position == chosenDoor.Position + roomInstance.Bounds.min) continue;
                    possibleDoors.Enqueue(exit);
                }
            }
            tilemap.CompressBounds();
            return rooms;
        }
    }
}