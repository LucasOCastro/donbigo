using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DonBigo.Rooms;

namespace DonBigo
{
    public class Spawner
    {
        public void Spawn(GameGrid grid, Entity character)
        {
            Vector2 center = new Vector2(0,0);
            // Tile spawnPoint;
            if (character is Bigodon)
            {
                center = GridManager.Instance.Grid.AllRooms[0].Bounds.center;
            }
            else if(character is Phantonette)
            { 
                center = GridManager.Instance.Grid.AllRooms.Last().Bounds.center;
            }
             
            while(!grid[(int) center.x, (int) center.y].Walkable)
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