using System.Collections.Generic;
using System.Linq;
using DonBigo.Rooms;
using UnityEngine;

namespace DonBigo
{
    //http://www.roguebasin.com/index.php/Restrictive_Precise_Angle_Shadowcasting
    public static class ShadowCasting
    {
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
            return tile.IsSeeThrough();
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

        private static bool IsBlocked(GameGrid grid, Vector2Int tile, FloatRange tileAngles,
            List<Obstacle> obstacles, int lastLineObstacleCount,
            RoomInstance sourceRoom, Vector2Int source)
        {
            //Tile nula nao e visivel
            if (grid[tile] == null)
            {
                return true;
            }

            //Sempre mostra as paredes externas da sala
            bool isWall = grid[tile].Type is WallTileType;
            if (isWall && (sourceRoom != null && sourceRoom.IsOuterWall(grid[tile])))
            {
                return false;
            }
            
            //Tile de outra sala nao é visivel
            if (grid[tile].Room != sourceRoom)
            {
                return true;
            }

            //Nao quero mostrar a parede das salas de baixo, nem as paredes internas que obstruem a visão
            if (isWall && tile.x < source.x && tile.y < source.y)
            {
                return true;
            }
            
            //Se tem algum obstaculo que impede a visão, é bloqueado
            return obstacles
                .Where((_, i) => i < lastLineObstacleCount)
                .Any(o => !IsVisibleThroughObstacle(grid[tile], tileAngles, o));
        }

        private static void CastOctant(GameGrid grid, Vector2Int source, RoomInstance sourceRoom, Octant octant,
            int range, Vector2Int direction,
            HashSet<Vector2Int> visibleTiles)
        {
            List<Obstacle> obstacles = new List<Obstacle>();
            for (int e1 = 1; e1 <= Mathf.Ceil(range/10f); e1++)
            {
                // O correto é usar 1f/e1. Com e1-1, as vezes da até infinito, mas por alguma razão
                // as vezes os melhores resultados vieram com esse valor.
                float angleStep = 1f / e1;
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

                    float closeAngle = e2 * angleStep;
                    float farAngle = closeAngle + angleStep;
                    FloatRange tileAngles = new(closeAngle, farAngle);
                    
                    if (!CanSeeThrough(grid[tile]))
                    {
                        lineObstacleCount++;
                        obstacles.Add(new Obstacle {
                            angleRange =  tileAngles,
                            tile = tile
                        });
                    }


                    float trueAngle = Vector2.Angle(tile - source, direction);
                    Vector2Int difVec = (tile - source).Abs();
                    bool blocked =  difVec.x > range || difVec.y > range || trueAngle > 45f||
                        //(tile.ManhattanDistance(source) > range) ||
                                   IsBlocked(grid, tile, tileAngles, obstacles,
                                       obstacles.Count - lineObstacleCount,
                                       sourceRoom, source);
                    
                    if (blocked)
                    {
                        visibleTiles.Remove(tile);
                    }
                    else
                    {
                        visibleTiles.Add(tile);    
                    }
                }
            }
        }
    
        public static HashSet<Vector2Int> Cast(GameGrid grid, Vector2Int source, Vector2Int direction, int range)
        {
            HashSet<Vector2Int> visibleTiles = new HashSet<Vector2Int> { source };
            RoomInstance sourceRoom = grid[source].Room;
            foreach (var octant in _octants)
            {
                CastOctant(grid, source, sourceRoom, octant, range, direction, visibleTiles);
            }
            return visibleTiles;
        }
    }    
}
