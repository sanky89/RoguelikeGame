using System;

namespace RoguelikeGame
{
    public class Node
    {
        public int X;
        public int Y;
        public int Cost;
        public Node(int x, int y, int cost)
        {
            X = x;
            Y = y;
            Cost = cost;
        }
    }
    public class Pathfinder
    {
        public readonly Node[,] Nodes;
        public int Width => Nodes.GetLength(0);
        public int Height => Nodes.GetLength(1);

        public Pathfinder()
        {
            var width = Globals.Map.Cols;
            var height = Globals.Map.Rows;
            Nodes = new Node[width, height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var cost = Globals.Map.GetTileType(x,y) == TileType.Solid ? 1000 : 1;
                    Nodes[x,y] = new Node(x,y,cost);                    
                }
            }
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