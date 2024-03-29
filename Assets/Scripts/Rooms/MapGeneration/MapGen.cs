﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace DonBigo.Rooms.MapGeneration
{
    public static class MapGen
    {
        private static void PlaceRoom(GameGrid grid, MapGenData data, RoomInstance roomInstance, Dictionary<ItemType, bool> itemChanceChecklist)
        {
            Room room = roomInstance.Room;
            Vector2Int min = roomInstance.Bounds.min;
            room.FillTilemap(data.tilemap, min);
            for (int x = 0; x < room.Size.x; x++)
            {
                for (int y = 0; y < room.Size.y; y++)
                {
                    Vector2Int worldPos = new Vector2Int(x, y) + min;
                    grid[worldPos] = new Tile(worldPos, room.Tiles[x, y], grid, roomInstance);
                }
            }

            List<Room.StructurePosition> vents = new List<Room.StructurePosition>();
            foreach (var structurePos in room.Structures)
            {
                if (structurePos.structure is VentTileType)
                {
                    vents.Add(structurePos);
                    continue;
                }
                
                Vector2Int pos = (Vector2Int)structurePos.pos + min;
                grid[pos].Structures.Add(structurePos.structure.GetInstance(grid[pos], structurePos.pos.z));
            }


            int ventIndex = -1;
            if (vents.Count > 0 && RandomUtility.Chance(room.VentChance))
            {
                ventIndex = vents.RandomIndex();
                var ventPosition = vents[ventIndex];
                
                Vector2Int pos = min + (Vector2Int)ventPosition.pos;
                Vent vent = new Vent(ventPosition.structure as VentTileType, grid[pos], ventPosition.pos.z);
                grid[pos].Structures.Add(vent);
                roomInstance.Vents.Add(vent);
                grid.AllVents.Add(vent);
            }
            
            for (int i = 0; i < vents.Count; i++)
            {
                if (i == ventIndex) continue;
                data.tilemap.SetTile(vents[i].pos + (Vector3Int)min, null);
            }

            foreach (var itemChance in room.GenItemsToSpawn())
            {
                var tile = grid.TilesInBounds(roomInstance.Bounds).Where(t => t.SupportsItem).Random();
                if (tile == null) continue;
                
                itemChance.Instantiate(tile);
                if (itemChanceChecklist != null && itemChanceChecklist.ContainsKey(itemChance))
                {
                    itemChanceChecklist[itemChance] = true;
                }
            }

            DecorationPlacer.PlaceDecorations(roomInstance, grid, data);
        }

        //Nem um pouco eficiente. Acho que RoomExit deveria ter sido uma classe desde o começo.
        private static void UnregisterDoor(RoomExit door, List<RoomInstance> rooms, GameGrid grid, Tilemap tilemap)
        {
            door.Marker.SetInactive(grid, tilemap, door.Position);
            foreach (var room in rooms)
            {
                for (int i = 0; i < room.Doors.Count; i++)
                {
                    if (room.Doors[i].Position == door.Position)
                    {
                        room.Doors.RemoveAt(i);
                        return;
                    }
                }
            }
        }

        private static void FillInternal(GameGrid grid, Tilemap tilemap, List<RoomExit> badExits,
            List<RoomInstance> rooms, TileType filler, EntranceMarkerTile fillerMat)
        {
            bool IsGoodInternal(List<Vector2Int> tiles)
            {
                if (tiles.Count == 0) return false;
                //Excluindo os internos que tocam nas bordas da grid
                Vector2Int min = grid.Bounds.min;
                Vector2Int max = grid.Bounds.max - Vector2Int.one;
                return tiles.All(t => grid.InBounds(t) && grid[t] == null && 
                                      (t.x != min.x) && (t.y != min.y) && (t.x != max.x) && (t.y != max.y));
            }

            void CreateDoor(RoomExit opposing)
            {
                const int matElevation = 1;
                Vector2Int pos = opposing.Position + opposing.DirectionVector;
                Tile tile = grid[pos];
                StructureInstance mat = new StructureInstance(fillerMat, tile, matElevation);
                tile.Structures.Add(mat);
                tile.Room.Doors.Add(new RoomExit(pos, opposing.OpposedDirection, fillerMat));

                Vector3Int tilemapPos = new Vector3Int(tile.Pos.x, tile.Pos.y, matElevation);
                tilemap.SetTile(tilemapPos, fillerMat);
                var matrix = Matrix4x4.identity;// * Matrix4x4.Scale(new Vector3(opposing.DirectionVector.x != 0 ? -1 : 1, 1, 1));
                tilemap.SetTransformMatrix(tilemapPos, matrix);
            }

            HashSet<Vector2Int> badSet = new HashSet<Vector2Int>();
            HashSet<Vector2Int> goodSet = new HashSet<Vector2Int>();
            foreach (var exit in badExits)
            {
                Vector2Int startPos = exit.Position + exit.DirectionVector;
                //Já decidi que aqui nao forma interno bom, então tira a porta
                if (badSet.Contains(startPos) || !grid.InBounds(startPos))
                {
                    UnregisterDoor(exit, rooms, grid, tilemap);
                    continue;
                }
                //Já foi preenchido com jardim, então nao tem o que fazer
                if (goodSet.Contains(startPos))
                {
                    CreateDoor(exit);
                    continue;
                } 

                List<Vector2Int> internalTiles = new();
                UtilVec2Int.FloodFill(startPos,
                    p => grid.InBounds(p) && grid[p] == null, 
                    p => internalTiles.Add(p));

                if (!IsGoodInternal(internalTiles))
                {
                    UnregisterDoor(exit, rooms, grid, tilemap);
                    foreach (Vector2Int badTile in internalTiles)
                    {
                        badSet.Add(badTile);
                    }
                    continue;
                }

                RoomInstance roomInstance = new RoomInstance(garden: true);
                foreach (Vector2Int tile in internalTiles)
                {
                    goodSet.Add(tile);
                    grid[tile] = new Tile(tile, filler, grid, roomInstance);
                    tilemap.SetTile((Vector3Int)tile, filler);
                }
                CreateDoor(exit);
                rooms.Add(roomInstance);
            }
        }


        private const int MaxSafetyRegenCount = 25;
        private static int _safetyRegenCount = 0;
        public static List<RoomInstance> Gen(GameGrid grid, MapGenData data)
        {
            //Usar uma Lista e acessar saidas aleatorias ao inves da Queue talvez dê um resultado mais devidamente aleatório.
            List<RoomInstance> rooms = new();
            Queue<RoomExit> possibleDoors = new();

            //Para garantir que não teremos softlock, armazenar os itens que ja foram colocados em um dicionario
            var necessaryItemChecklist = data.necessaryItems.ToDictionary(i => i, _ => false);

            //Colocar a sala inicial do mapa
            Room randRoom = (data.startingRoom != null) ? data.startingRoom : RoomDatabase.RandomRoom();
            if (randRoom == null) return rooms;
            Vector2Int genStart = new Vector2Int(
                (int)(grid.Bounds.size.x * data.normalizedGenStart.x + grid.Bounds.min.x),
                (int)(grid.Bounds.size.y * data.normalizedGenStart.y + grid.Bounds.min.y)
            );
            RoomInstance roomInstance = new RoomInstance(randRoom, genStart);
            PlaceRoom(grid, data, roomInstance, necessaryItemChecklist);
            rooms.Add(roomInstance);
            foreach (var door in roomInstance.Doors)
            {
                possibleDoors.Enqueue(door);
            }

            List<RoomExit> badExits = new();

            while (possibleDoors.Count > 0)
            {
                RoomExit possibleDoor = possibleDoors.Dequeue();
                randRoom = RoomDatabase.RandomRoomThatFits(grid, possibleDoor, out RoomExit chosenDoor);
                if (randRoom == null)
                {
                    //Se não cabe nenhuma sala nessa porta, então podemos tentar criar um jardim interno.
                    badExits.Add(possibleDoor);
                    continue;
                }

                //Ajustamos a posição em que a sala será colocada com base na posição da porta que será conectada.
                Vector2Int newRoomMin = (possibleDoor.Position + possibleDoor.DirectionVector) - chosenDoor.Position;
                roomInstance = new RoomInstance(randRoom, newRoomMin);
                PlaceRoom(grid, data, roomInstance, necessaryItemChecklist);
                rooms.Add(roomInstance);

                foreach (var exit in roomInstance.Doors)
                {
                    //Não adicionamos a porta que já foi conectada na queue
                    if (exit.Position == chosenDoor.Position + roomInstance.Bounds.min) continue;
                    possibleDoors.Enqueue(exit);
                }
            }

            //Se faltou algum item essencial, precisamos gerar outro mapa.
            if (_safetyRegenCount < MaxSafetyRegenCount && necessaryItemChecklist.Values.Any(v => v == false))
            {
                Debug.Log("Map Regen");
                _safetyRegenCount++;
                grid.ClearMap();
                return Gen(grid, data);
            }

            if (_safetyRegenCount >= MaxSafetyRegenCount)
                Debug.LogError("Regen map too much :(");
            _safetyRegenCount = 0;

            if (data.fillerTile != null && data.fillerTile != null)
            {
                FillInternal(grid, data.tilemap, badExits, rooms, data.fillerTile, data.fillerMat);
            }


            data.tilemap.CompressBounds();
            return rooms;
        }
    }
}