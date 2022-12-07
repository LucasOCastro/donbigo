using System.Collections.Generic;
using System.Linq;
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
                Vector2Int pos = (Vector2Int)structurePos.pos + min; 
                grid[pos].Structures.Add(new StructureInstance(structurePos.structure, grid[pos], structurePos.pos.z));
            }
            
            foreach (var itemChance in room.GenItemsToSpawn())
            {
                var tile = grid.TilesInBounds(roomInstance.Bounds).Where(t => t.SupportsItem).Random();
                if (tile == null) continue;
                
                var spawned = itemChance.Instantiate(tile);
                Debug.Log("Coloquei item em "+tile.Pos + " e pos = " + spawned.transform.position);
            }
        }

        
        public static List<RoomInstance> Gen(GameGrid grid, Tilemap tilemap)
        {
            if (GridManager.Instance.DEBUG_TEST_ROOM != null)
            {
                RoomInstance inst = new RoomInstance(GridManager.Instance.DEBUG_TEST_ROOM, Vector2Int.one);
                List<RoomInstance> res = new List<RoomInstance>() { inst };
                PlaceRoom(grid, tilemap, inst);
                return res;
            }
            
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