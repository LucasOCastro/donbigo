using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DonBigo
{
    //https://en.wikipedia.org/wiki/A*_search_algorithm
    public static class PathFinding
    {
        private const int StraightCost = 10, DiagonalCost = 14;
        
        private struct Node
        {
            public Vector2Int tile, parent;
            public float cCost, gCost;
            public float fCost => cCost + gCost;

            public Node(Vector2Int parentNode, Vector2Int nodeTile, float nodeCCost, float distanceToTarget)
            {
                tile = nodeTile;
                parent = parentNode;
                cCost = nodeCCost;
                gCost = distanceToTarget;
            }
            
        }

        private static Node PopBest(List<Vector2Int> set, Dictionary<Vector2Int, Node> nodes)
        {
            int bestI = 0;
            for (int i = 1; i < set.Count; i++)
            {
                if (nodes[set[i]].fCost < nodes[set[bestI]].fCost)
                {
                    bestI = i;
                }
            }

            Node node = nodes[set[bestI]];
            set.RemoveAt(bestI);
            return node;
        }
        
        //http://theory.stanford.edu/~amitp/GameProgramming/Heuristics.html
        //Mistura de Manhattan Distance com Chebyshev e Octile, para permitir movimentos diagonais.
        private static int ManhattanDistance(Vector2Int a, Vector2Int b)
        {
            int dx = Mathf.Abs(a.x - b.x);
            int dy = Mathf.Abs(a.y - b.y);
            return StraightCost * (dx + dy) + (DiagonalCost - 2 * StraightCost) * Mathf.Min(dx, dy);
        }

        private static float TransitionCost(Tile from, Tile to)
        {
            if (!to.Walkable)
            {
                return -1;
            }
            bool isDiagonal = (from.Pos - to.Pos).sqrMagnitude > 1;
            return isDiagonal ? DiagonalCost : StraightCost;
        }

        private static List<Tile> BackPath(Node final, Dictionary<Vector2Int, Node> nodes, GameGrid grid)
        {
            Stack<Tile> path = new Stack<Tile>();
            Node node = final;
            while (node.parent != node.tile)
            {
                path.Push(grid[node.tile]);
                node = nodes[node.parent];
            }
            return path.ToList();
        }
        
        public static List<Tile> Path(Tile source, Tile target)
        {
            if (!source.Walkable || !target.Walkable) return null;
            
            var grid = source.ParentGrid;
            List<Vector2Int> openSet = new List<Vector2Int>();
            Dictionary<Vector2Int, Node> nodes = new Dictionary<Vector2Int, Node>();
            HashSet<Vector2Int> closedSet = new HashSet<Vector2Int>();

            openSet.Add(source.Pos);
            nodes.Add(source.Pos, new Node(source.Pos, source.Pos, 0, ManhattanDistance(source.Pos, target.Pos)));
            //closedSet.Add(source.Pos);

            while (openSet.Count > 0)
            {
                Node node = PopBest(openSet, nodes);
                closedSet.Add(node.tile);
                if (node.tile == target.Pos)
                {
                    return BackPath(node, nodes, grid);
                }

                //closedSet.Add(node.tile);
                foreach (var neighbor in grid[node.tile].Neighbors)
                {
                    Debug.Log("neighbor="+neighbor.Pos);
                    if (closedSet.Contains(neighbor.Pos)) continue;

                    float transitionCost = TransitionCost(grid[node.tile], neighbor);
                    if (transitionCost < 0) continue;
                    
                    float possibleCCost = node.cCost + transitionCost;

                    bool contains = nodes.ContainsKey(neighbor.Pos);
                    if (contains)
                    {
                        Node neighborNode = nodes[neighbor.Pos];
                        if (possibleCCost < neighborNode.cCost)
                        {
                            neighborNode.parent = node.tile;
                            neighborNode.cCost = possibleCCost;
                            nodes[neighbor.Pos] = neighborNode;
                        }
                    }
                    else
                    {
                        Node neighborNode = new Node(node.tile, neighbor.Pos, possibleCCost, ManhattanDistance(neighbor.Pos, target.Pos));
                        nodes.Add(neighbor.Pos, neighborNode);
                    }
                    
                    //if (!closedSet.Contains(neighbor.Pos))
                    //{
                        openSet.Add(neighbor.Pos);
                    //}
                }
            }

            // Not found
            return null; 
        }
    }
}