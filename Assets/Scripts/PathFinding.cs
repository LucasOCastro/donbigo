using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DonBigo
{
    //https://en.wikipedia.org/wiki/A*_search_algorithm
    public static class PathFinding
    {
        private const int BlacklistedCost = 100;
        
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

        private static Node PopBest(HashSet<Vector2Int> set, Dictionary<Vector2Int, Node> nodes)
        {
            Vector2Int best = set.Best((n1, n2) => nodes[n1].fCost < nodes[n2].fCost);
            set.Remove(best);    
            return nodes[best];
        }

        public delegate int CostFunc(Tile from, Tile to);

        private static int TransitionCost(Tile from, Tile to, Entity pather, CostFunc costBonusFunc)
        {
            if (!to.Walkable || to.Entity != null || to.Room != from.Room)
            {
                return -1;
            }
            bool isDiagonal = (from.Pos - to.Pos).sqrMagnitude > 1;
            int baseCost = isDiagonal ? UtilVec2Int.DiagonalCost : UtilVec2Int.StraightCost;
//eu sou demais
            if (pather != null && pather.BlacklistedTiles.Contains(to.Pos)) baseCost += BlacklistedCost;
            if (costBonusFunc != null) baseCost += costBonusFunc(from, to);

            return baseCost;
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

        public static List<Tile> Path(Tile source, Tile target, Entity pather, CostFunc costBonusFunc = null)
        {
            if (!source.Walkable) return null;
            
            var grid = source.ParentGrid;
            // Uma priority queue aqui seria mais apropriada
            Dictionary<Vector2Int, Node> nodes = new Dictionary<Vector2Int, Node>();
            HashSet<Vector2Int> openSet = new HashSet<Vector2Int>();
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

                    int transitionCost = TransitionCost(grid[node.tile], neighbor, pather, costBonusFunc);
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
                    openSet.Add(neighbor.Pos);    
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