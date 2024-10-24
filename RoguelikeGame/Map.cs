﻿using System;
using System.Collections.Generic;

namespace RoguelikeGame
{
    public enum Direction
    {
        LEFT,
        RIGHT,
        UP,
        DOWN,
        NW,
        NE,
        SW,
        SE
    }

    public class Map
    {
        public int Rows { get; private set; }
        public int Cols { get; private set; }
        public Tile[,] Tiles { get; private set; }
        private Player _player;
        public Player Player => _player;
        public Vec2Int PlayerMapPosition { get; set; }
        public List<Room> Rooms { get; set; }
        public List<Monster> Monsters { get; set; }
        public List<Monster> VisibleMonsters { get; set; }
        public List<Item> Items { get; set; }
        public Pathfinder Pathfinder { get; private set; }

        public Map(int rows, int cols, Player player)
        {
            Rows = rows;
            Cols = cols;
            Tiles = new Tile[cols, rows];
            _player = player;
            Monsters = new List<Monster>();
            VisibleMonsters = new List<Monster>();
            Items = new List<Item>();
            Pathfinder = new Pathfinder(this, cols, rows);
        }

        public Tile GetTileAtIndex(int x, int y)
        {
            if(!IsWithinMapRange(x,y))
            {
                throw new ArgumentOutOfRangeException($"Index out of range {x}, {y}");
            }
            return Tiles[x, y];
        }

        public void RemoveMonster(Monster m)
        {
            SetTileType(m.MapX, m.MapY, TileType.Walkable);
            Monsters.Remove(m);
        }

        public void SetPlayer(Player player)
        {
            _player = player;
        }

        public void SetTileType(int x, int y, TileType type)
        {
            Tiles[x, y].TileType = type;
        }

        public ActionResult CanMove(int x, int y)
        {
            if (x < 0 || y < 0 || x >= Cols || y >= Rows)
            { 
                return new ActionResult(ActionResultType.None, null);
            }

            var tile = Tiles[x, y];
            if (tile.TileType == TileType.Walkable)
            {
                if (ContainsItem(x, y, out var item))
                {
                    Items.Remove(item);
                    return new ActionResult(ActionResultType.CollectItem, item);
                }
                return new ActionResult(ActionResultType.Move, null);;
            }
            else if(IsMonsterTile(x, y, out var monster))
            {
                return new ActionResult(ActionResultType.HitEntity, monster);
            }

            return new ActionResult(ActionResultType.HitWall, null);;
        }

        public void ToggleMapVisible(bool visible)
        {
            foreach (var tile in Tiles)
            {
                tile.Visible = visible;
            }
            VisibleMonsters.Clear();
        }

        public void ToggleTileVisible(int x, int y, bool visible)
        {
            Tiles[x, y].Visible = visible;
            if (IsMonsterTile(x, y, out var m))
            {
                if(visible && !VisibleMonsters.Contains(m))
                {
                    VisibleMonsters.Add(m);
                }
            }
        }

        public void ToggleMapVisited(bool visited)
        {
            foreach (var tile in Tiles)
            {
                tile.Visited = visited;
            }
        }

        public void ToggleTileVisited(int x, int y, bool visited)
        {
            Tiles[x, y].Visited = visited;
        }

        public TileType GetTileType(int x, int y)
        {
            if (!IsWithinMapRange(x, y))
            {
                throw new ArgumentOutOfRangeException();
            }
            return Tiles[x, y].TileType;
        }

        public bool IsWithinMapRange(int x, int y)
        {
            return x >= 0 && x < Cols && y >= 0 && y < Rows;
        }

        public bool IsPlayerTile(int x, int y)
        {
            return x == _player.MapX && y == _player.MapY;
        }

        public bool IsMonsterTile(int x, int y, out Monster m)
        {
            for(int i=0; i<Monsters.Count; i++)
            {
                if (x == Monsters[i].MapX && y == Monsters[i].MapY)
                {
                    m = Monsters[i];
                    return true;
                }
            }
            m = null;
            return false;
        }

        public bool ContainsItem(int x, int y, out Item item)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                if (x == Items[i].MapX && y == Items[i].MapY)
                {
                    item = Items[i];
                    return true;
                }
            }
            item = null;
            return false;
        }
    }
}
