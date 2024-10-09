using System;
using System.Collections;
using System.Collections.Generic;

namespace RoguelikeGame
{
    public class Node
    {
        public int X;
        public int Y;
        public int Cost => GCost + HCost;

        public int GCost;
        public int HCost;

        public Node Parent;

        public Node(int x, int y)
        {
            X = x;
            Y = y;
            GCost = int.MaxValue;
            Parent = null;
        }
    }
    public class Pathfinder
    {
        public readonly Node[,] Nodes;
        public int Width => Nodes.GetLength(0);
        public int Height => Nodes.GetLength(1);

        public Pathfinder(int width, int height)
        {
            Nodes = new Node[width, height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Nodes[x, y] = new Node(x, y);
                }
            }
        }

        public List<Node> CalculatePath(Node start, Node end)
        {
            PriorityQueue<Node, int> openQueue = new PriorityQueue<Node, int>();
            List<Node> openList = new List<Node>();
            List<Node> closedList = new List<Node>();
            start.GCost = 0;
            start.HCost = 0;
            openList.Add(start);
            openQueue.Enqueue(start, start.Cost);
            System.Console.WriteLine($"Start: {start.X} {start.Y} {start.Cost}");
            System.Console.WriteLine($"End: {end.X} {end.Y} {end.Cost}");
            while (openList.Count > 0)
            {
                Node current = openList[0];
                for (int i = 0; i < openList.Count; i++)
                {
                    if (openList[i].Cost < current.Cost ||
                        openList[i].Cost == current.Cost && openList[i].HCost < current.HCost)
                    {
                        current = openList[i];
                    }
                }

                //System.Console.WriteLine($"{current.X} {current.Y} {current.Cost}");
                openList.Remove(current);
                closedList.Add(current);

                if(current.X == end.X && current.Y == end.Y)
                {
                    return RetracePath(start, current);
                }

                var neighbors = GetNeighbors(current);
                foreach (var node in neighbors)
                {
                    if (closedList.Contains(node))
                    {
                        continue;
                    }

                    var newGCost = current.GCost + GetMovementCost(current, node);
                    bool contains = openList.Contains(node);
                    if (newGCost < node.GCost || !contains)
                    {
                        node.GCost = newGCost;
                        node.HCost = GetManhattanDistance(node, end);
                        node.Parent = current;
                        if (!contains)
                        {
                            openList.Add(node);
                        }
                    }
                }
            }
            return null;
        }

        private int GetMovementCost(Node current, Node node)
        {
            if(current.X == node.X || current.Y == node.Y)
            {
                return 1;
            }
            return 2;
        }

        private List<Node> RetracePath(Node start, Node end)
        {
            List<Node> path = new List<Node>();
            Node pathNode = end;
            while (pathNode != start)
            {
                path.Add(pathNode);
                pathNode = pathNode.Parent;
            }
            path.Reverse();
            return path;
        }

        private int GetManhattanDistance(Node start, Node end)
        {
            return Math.Abs(end.X - start.X) + Math.Abs(end.Y - start.Y);
        }

        private List<Node> GetNeighbors(Node current)
        {
            int nodeX = current.X;
            int nodeY = current.Y;
            List<Node> neighbors = new List<Node>();
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    var neighborX = nodeX + j;
                    var neighborY = nodeY + i;
                    if((i == 0 && j == 0) || !IsInRange(neighborX,neighborY) || !Walkable(neighborX, neighborY))
                    {
                        continue;
                    }
                    neighbors.Add(Nodes[neighborX, neighborY]);
                }
            }
            return neighbors;
        }

        private bool IsInRange(int x, int y)
        {
            return x >= 0 && x < Nodes.GetLength(0) && y >= 0 && y < Nodes.GetLength(1);
        }

        private bool Walkable(int x, int y)
        {
            return Globals.Map.GetTileType(x, y) == TileType.Walkable || Globals.Map.IsPlayerTile(x,y);
        }

        private Node GetLeastCostNode(List<Node> openList)
        {
            Node minCostNode = openList[0];
            foreach (Node node in openList)
            {
                if(node.Cost < minCostNode.Cost)
                {
                    minCostNode = node;
                }
            }
            return minCostNode;
        }

        public int GetNodeCost(int row, int col)
        {
            if(col < 0 || col >= Width || row < 0 || row >= Height)
            {
                throw new ArgumentOutOfRangeException();
            }
            return Nodes[col, row].Cost;
        }
    }
}