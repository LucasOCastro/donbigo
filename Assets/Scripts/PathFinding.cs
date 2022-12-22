using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DonBigo
{
    //https://en.wikipedia.org/wiki/A*_search_algorithm
    public static class PathFinding
    {
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
        
        

        private static float TransitionCost(Tile from, Tile to)
        {
            if (!to.Walkable || to.Entity != null)
            {
                return -1;
            }
            bool isDiagonal = (from.Pos - to.Pos).sqrMagnitude > 1;
            return isDiagonal ? UtilVec2Int.DiagonalCost : UtilVec2Int.StraightCost;
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
            if (!source.Walkable) return null;
            
            var grid = source.ParentGrid;
            // Uma priority queue aqui seria mais apropriada
            List<Vector2Int> openSet = new List<Vector2Int>();
            Dictionary<Vector2Int, Node> nodes = new Dictionary<Vector2Int, Node>();
            HashSet<Vector2Int> closedSet = new HashSet<Vector2Int>();

            openSet.Add(source.Pos);
            nodes.Add(source.Pos, new Node(source.Pos, source.Pos, 0, source.Pos.ManhattanDistance(target.Pos)));

            while (openSet.Count > 0)
            {
                Node node = PopBest(openSet, nodes);
                closedSet.Add(node.tile);
                if (node.tile == target.Pos)
                {
                    return BackPath(node, nodes, grid);
                }

                foreach (var neighbor in grid[node.tile].Neighbors)
                {
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
                        Node neighborNode = new Node(node.tile, neighbor.Pos, possibleCCost, neighbor.Pos.ManhattanDistance(target.Pos));
                        nodes.Add(neighbor.Pos, neighborNode);
                    }

                    // Contain em lista é feio :(
                    if (!openSet.Contains(neighbor.Pos))
                    {
                        openSet.Add(neighbor.Pos);    
                    }
                }
            }

            Vector2Int closest = source.Pos;
            foreach (var node in nodes.Values)
            {
                if (node.gCost < nodes[closest].gCost)
                {
                    closest = node.tile;
                }
            }
            return BackPath(nodes[closest], nodes, grid); 
        }
    }
}