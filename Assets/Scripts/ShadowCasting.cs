using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DonBigo
{
    //http://www.roguebasin.com/index.php/Restrictive_Precise_Angle_Shadowcasting
    public static class ShadowCasting
    {
        private static float AntiClockwiseAngle(Vector2 vector)
        {
            //Acho q nao precisa normalizar o vetor
            float angle = Vector2.SignedAngle(Vector2.up, vector);
            return (angle < 0) ? 360 + angle : angle;
        }

        private struct Octant
        {
            public bool startX;
            public Vector2Int increment;
            public FloatRange angleRange;
            public Color color;
        }
        
        private static Octant[] _octants = 
        {
            new Octant(){ startX = false, increment = new Vector2Int(-1, 1), angleRange = new FloatRange(0, 45), color = Color.red}, //NNW
            new Octant(){ startX = true, increment = new Vector2Int(-1, 1), angleRange = new FloatRange(45, 90), color = Color.blue}, //WNW
            new Octant(){ startX = true, increment = new Vector2Int(-1, -1), angleRange = new FloatRange(90, 135), color = Color.green}, //WSW
            new Octant(){ startX = false, increment = new Vector2Int(-1, -1), angleRange = new FloatRange(135, 180), color = Color.yellow}, //SSW
            new Octant(){ startX = false, increment = new Vector2Int(1, -1), angleRange = new FloatRange(180, 225), color = Color.magenta}, //SSE
            new Octant(){ startX = true, increment = new Vector2Int(1, -1), angleRange = new FloatRange(225, 270), color = Color.cyan}, //ESE
            new Octant(){ startX = true, increment = new Vector2Int(1, 1), angleRange = new FloatRange(270, 315), color = Color.white}, //ENE
            new Octant(){ startX = false, increment = new Vector2Int(1, 1), angleRange = new FloatRange(315, 360), color = Color.black}, //NNE
        };
        
        private struct Obstacle
        {
            public Vector2Int tile;
            public FloatRange angleRange;
        }

        private static bool CanSeeThrough(Tile tile)
        {
            if (tile == null)
            {
                return false;
            }
            return tile.Type is not WallTileType;
        }

        private static bool IsVisibleThroughObstacle(Tile tile, FloatRange tileAngles, Obstacle obstacle)
        {
            bool minFree = obstacle.angleRange.InRange(tileAngles.Min);
            bool maxFree = obstacle.angleRange.InRange(tileAngles.Max);
            bool centerFree = obstacle.angleRange.InRange(tileAngles.Average);
            if (CanSeeThrough(tile))
            {
                return centerFree && (minFree || maxFree);
            }
            return minFree || maxFree || centerFree;
        }
        

        private static void CastOctant(GameGrid grid, Vector2Int source, Octant octant, int range)
        {
            HashSet<Vector2Int> visibleTiles = new HashSet<Vector2Int>();
            
            List<Obstacle> obstacles = new List<Obstacle>();
            for (int e1 = 1; e1 <= range; e1++)
            {
                float angleRange = 1f / (e1 + 1);
                for (int e2 = 0; e2 <= e1; e2++)
                {
                    Vector2Int offset = octant.startX ? new Vector2Int(e1, e2) : new Vector2Int(e2, e1);
                    offset.x *= octant.increment.x;
                    offset.y *= octant.increment.y;
                    Vector2Int tile = source + offset;
                    //grid.DEBUG_SetColor(tile, octant.color);
                    if (!grid.InBounds(tile))
                    {
                        continue;
                    }

                    float closeAngle = e2 * angleRange;
                    float farAngle = closeAngle + angleRange;
                    FloatRange tileAngles = new(closeAngle, farAngle);
                    
                    if (obstacles.Any(obstacle => !IsVisibleThroughObstacle(grid[tile], tileAngles, obstacle)))
                    {
                        //visibleTiles.Add()
                        grid.DEBUG_SetColor(tile, Color.red);
                        //Set as blocked
                        continue;
                    }
                    
                    grid.DEBUG_SetColor(tile, Color.green);

                    if (!CanSeeThrough(grid[tile]))
                    {
                        obstacles.Add(new Obstacle {
                            angleRange =  new FloatRange(closeAngle, farAngle),
                            tile = tile
                        });
                        //Debug.Log($"Obstacle at {tile} from {source} and sourceCenter={source.Center()} at closeAngle = {closeAngle}");
                        /*Debug.DrawRay(source.Center(), Quaternion.AngleAxis(closeAngle, Vector3.forward) * Vector3.up, Color.magenta, 30);
                        Debug.DrawRay(source.Center(), Quaternion.AngleAxis(tileAngles.Average, Vector3.forward) * Vector3.up, Color.red, 30);
                        Debug.DrawRay(source.Center(), Quaternion.AngleAxis(farAngle, Vector3.forward) * Vector3.up, Color.blue, 30);
                        */
                    }
                }
            }
        }
    
        public static void Cast(GameGrid grid, Vector2Int source, int range)
        {
            foreach (var octant in _octants)
            {
                CastOctant(grid, source, octant, range);
            }
        }
    }    
}
