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
            public Color color;
        }
        
        private static Octant[] _octants = 
        {
            new Octant(){ startX = false, increment = new Vector2Int(-1, 1)}, //NNW
            new Octant(){ startX = true, increment = new Vector2Int(-1, 1)}, //WNW
            new Octant(){ startX = true, increment = new Vector2Int(-1, -1)}, //WSW
            new Octant(){ startX = false, increment = new Vector2Int(-1, -1)}, //SSW
            new Octant(){ startX = false, increment = new Vector2Int(1, -1)}, //SSE
            new Octant(){ startX = true, increment = new Vector2Int(1, -1)}, //ESE
            new Octant(){ startX = true, increment = new Vector2Int(1, 1)}, //ENE
            new Octant(){ startX = false, increment = new Vector2Int(1, 1)}, //NNE
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
            if (tile == null)
            {
                return false;
            }
            
            bool minFree = !obstacle.angleRange.InRange(tileAngles.Min);
            bool maxFree = !obstacle.angleRange.InRange(tileAngles.Max);
            bool centerFree = !obstacle.angleRange.InRange(tileAngles.Average);
            if (CanSeeThrough(tile))
            {
                return centerFree && (minFree || maxFree);
            }
            return centerFree && (minFree || maxFree);//minFree || maxFree || centerFree;
        }
        
        public static HashSet<Vector2Int> visibleTiles = new HashSet<Vector2Int>();
        private static void CastOctant(GameGrid grid, Vector2Int source, Octant octant, int range)
        {
            //HashSet<Vector2Int> visibleTiles = new HashSet<Vector2Int>();
            HashSet<Vector2Int> blockedTiles = new HashSet<Vector2Int>();
            
            List<Obstacle> obstacles = new List<Obstacle>();
            int lastLineObstacleCount = 0;
            for (int e1 = 1; e1 <= range; e1++)
            {
                // O correto é usar 1f/e1. Com e1-1, as vezes da até infinito, mas por alguma razão
                // as vezes os melhores resultados vieram com esse valor.
                float angleRange = 1f / e1;

                int lineObstacleCount = 0;
                for (int e2 = 0; e2 <= e1; e2++)
                {
                    Vector2Int offset = octant.startX ? new Vector2Int(e1, e2) : new Vector2Int(e2, e1);
                    offset.x *= octant.increment.x;
                    offset.y *= octant.increment.y;
                    Vector2Int tile = source + offset;
                    
                    if (!grid.InBounds(tile))
                    {
                        continue;
                    }

                    if (blockedTiles.Contains(tile))
                    {
                        continue;
                    }

                    float closeAngle = e2 * angleRange;
                    float farAngle = closeAngle + angleRange;
                    FloatRange tileAngles = new(closeAngle, farAngle);
                    
                    if (!CanSeeThrough(grid[tile]))
                    {
                        lineObstacleCount++;
                        obstacles.Add(new Obstacle {
                            angleRange =  tileAngles,
                            tile = tile
                        });
                    }

                    bool blocked = obstacles
                        .Where((_, i) => i < (obstacles.Count - lineObstacleCount))
                        .Any(o => !IsVisibleThroughObstacle(grid[tile], tileAngles, o)); 
                    if (blocked)
                    {
                        if (visibleTiles.Contains(tile)) {
                            visibleTiles.Remove(tile);
                        }
                        blockedTiles.Add(tile);
                        grid.DEBUG_SetColor(tile, Color.red);
                        continue;
                    }
                    if (blockedTiles.Contains(tile)) {
                        blockedTiles.Remove(tile);
                    }
                    visibleTiles.Add(tile);
                    grid.DEBUG_SetColor(tile, Color.green);    
                }
                lastLineObstacleCount += lineObstacleCount;
            }
        }
    
        public static void Cast(GameGrid grid, Vector2Int source, int range)
        {
            visibleTiles.Clear();
            visibleTiles.Add(source);
            foreach (var octant in _octants)
            {
                CastOctant(grid, source, octant, range);
            }
        }
    }    
}
