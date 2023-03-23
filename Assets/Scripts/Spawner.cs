using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DonBigo.Rooms;

namespace DonBigo
{
    public class Spawner
    {
        private static RoomInstance GetSpawnRoom(Entity character, IEnumerable<RoomInstance> rooms)
            => character switch
            {
                Bigodon => rooms.First(),
                Phantonette => rooms.Last(r => !r.IsGarden),
                _ => throw new NotImplementedException()
            };
        
        public void Spawn(GameGrid grid, Entity character)
        {
            Vector2 center = GetSpawnRoom(character, grid.AllRooms).Bounds.center;
             
            while(!grid[(int) center.x, (int) center.y].Walkable || grid[(int)center.x, (int)center.y].Entity != null)
            {
                if ((int)(center.x - center.y) == 0)
                {
                    center.x += 1;
                }
                else if ((int)(center.x - center.y) > 0)
                {
                    center.y += 1;
                }
                else if ((int)(center.x - center.y) > 0)
                {
                    center.y -= 1;
                }
            }
                
            character.Tile = grid[(int) center.x, (int) center.y];
            // character.transform.position = new Vector3(spawnPoint.x, spawnPoint.y, 2);
        }
    }
}