﻿using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace RoguelikeGame
{
    public enum Direction
    {
        LEFT,
        RIGHT,
        UP,
        DOWN,
    }

    public class Map
    {
        public const int ROWS = 60;
        public const int COLS = 100;
        private const int MAX_ROOMS = 20;
        private const int MIN_ROOM_SIZE = 8;
        private const int MAX_ROOM_SIZE = 16;

        private Tile[,] _tiles;
        private Player _player;
        private Vector2 _position;
        public readonly int ViewportWidth;
        public readonly int ViewportHeight;
        private bool _addWalls = true;

        public int colStartIndex = 0;
        public int rowStartIndex = 0;
        public List<Room> Rooms;

        public Map(Vector2 position, int viewportWidth, int viewportHeight)
        {
            _tiles = new Tile[COLS, ROWS];
            _position = position;
            ViewportWidth = viewportWidth;
            ViewportHeight = viewportHeight;
            Rooms = new List<Room>(MAX_ROOMS);
            //GenerateRandomWalls();
            GenerateRooms();
        }

        public void SetPlayer(Player player)
        {
            _player = player;
            _player.SetInitialMapPosition(_position + Rooms[0].GetRandomPointInsideRoom());
        }

        public bool CanMove(int x, int y)
        {
            x -= (int)(_position.X + 1);
            y -= (int)(_position.Y + 1);

            if(x < 0 || y < 0 || x + colStartIndex >= COLS || y + rowStartIndex >= ROWS)
            {
                return false;
            }
            return _tiles[x + colStartIndex, y + rowStartIndex].TileType == TileType.Walkable;
        }

        public void DrawMapTile(int x, int y)
        {
            if(x >= colStartIndex && 
               y >= rowStartIndex &&
               x + _tiles[x,y].Offset.X < ViewportWidth-1 && 
               y + _tiles[x, y].Offset.Y < ViewportHeight-1)
            {
                _tiles[x, y].Draw();
                _player.Draw(Globals.SpriteBatch, Globals.GlyphsTexture);
            }
        }

        public void Draw()
        {
            for (int y = rowStartIndex; y < ROWS; y++)
            {
                for (int x = colStartIndex; x < COLS; x++)
                {
                    DrawMapTile(x, y);
                }
            }
        }

        public void ScrollLeft()
        {
            colStartIndex++;
            if (colStartIndex >= COLS-1)
            {
                colStartIndex = COLS-1;
            }
            //System.Console.WriteLine($"colStartIndex: {colStartIndex}");
            for (int y = 0; y < ROWS; y++)
            {
                for (int x = 0; x < COLS; x++)
                {
                    _tiles[x, y].SetOffset(-colStartIndex, 0);
                }
            }
        }

        public void ScrollMap(Direction direction)
        {
            switch (direction)
            {
                case Direction.LEFT:
                    colStartIndex++;
                    if (colStartIndex >= COLS - 1)
                    {
                        colStartIndex = COLS - 1;
                    }
                    break;
                case Direction.RIGHT:
                    colStartIndex--;
                    if (colStartIndex < 0)
                    {
                        colStartIndex = 0;
                    }
                    break;
                case Direction.UP:
                    rowStartIndex++;
                    if (rowStartIndex >= ROWS - 1)
                    {
                        rowStartIndex = ROWS - 1;
                    }
                    break;
                case Direction.DOWN:
                    rowStartIndex--;
                    if (rowStartIndex < 0)
                    {
                        rowStartIndex = 0;
                    }
                    break;
                default:
                    break;
            }

            for (int y = 0; y < ROWS; y++)
            {
                for (int x = 0; x < COLS; x++)
                {
                    _tiles[x, y].SetOffset(-colStartIndex, -rowStartIndex);
                }
            }
        }

        public void ScrollRight()
        {
            colStartIndex--;
            if(colStartIndex < 0)
            {
                colStartIndex = 0;
            }
            //System.Console.WriteLine($"colStartIndex: {colStartIndex}");
            for (int y = 0; y < ROWS; y++)
            {
                for (int x = 0; x < COLS; x++)
                {
                    _tiles[x, y].SetOffset(-colStartIndex, 0);
                }
            }
        }

        public void ScrollUp()
        {
            rowStartIndex--;
            if (rowStartIndex < 0)
            {
                rowStartIndex = 0;
            }
            //System.Console.WriteLine($"colStartIndex: {colStartIndex}");
            for (int y = 0; y < ROWS; y++)
            {
                for (int x = 0; x < COLS; x++)
                {
                    _tiles[x, y].SetOffset(-colStartIndex, 0);
                }
            }
        }

        public void ScrollDown()
        {
            colStartIndex--;
            if (colStartIndex < 0)
            {
                colStartIndex = 0;
            }
            //System.Console.WriteLine($"colStartIndex: {colStartIndex}");
            for (int y = 0; y < ROWS; y++)
            {
                for (int x = 0; x < COLS; x++)
                {
                    _tiles[x, y].SetOffset(-colStartIndex, 0);
                }
            }
        }

        private void GenerateRooms()
        {
            int[,] walls = new int[COLS, ROWS];
            for (int y = 0; y < ROWS; y++)
            {
                for (int x = 0; x < COLS; x++)
                {
                    _tiles[x,y] = new Tile(new Character(Glyphs.Fill, Color.RosyBrown), new Vector2(x + (int)_position.X + 1, y + (int)_position.Y + 1), TileType.Solid);
                } 
            }
            
            Random r = new Random();
            int count = 0;
            while(count < MAX_ROOMS)
            {
                int width = r.Next(MIN_ROOM_SIZE, MAX_ROOM_SIZE);
                int height = r.Next(MIN_ROOM_SIZE, MAX_ROOM_SIZE);
                int x = r.Next(COLS - width - 1);
                int y = r.Next(ROWS - height - 1);
                Room room = new Room(x, y, width, height);
                bool intersects = false;
                foreach(var room1 in Rooms)
                {
                    if(room.IntersectsWithPadding(room1))
                    {
                        intersects = true;
                        break;
                    }
                }
                if(!intersects)
                {
                    Rooms.Add(room);
                    count++;
                }
            }

            foreach(var room in Rooms)
            {
                for (int y = room.RoomRect.Y; y < room.RoomRect.Bottom; y++)
                {
                    for (int x = room.RoomRect.X; x < room.RoomRect.Right; x++)
                    {
                        _tiles[x, y].UpdateTile(new Character(Glyphs.Period, Color.DarkGreen), TileType.Walkable);
                    }
                }
            }

            Rooms.Sort((a,b) => a.RoomRect.X.CompareTo(b.RoomRect.X));

/*            for (int y = 0; y < ROWS; y++)
            {
                for (int x = 0; x < COLS; x++)
                {
                    System.Console.Write(walls[x, y]);
                }
                System.Console.Write("\n");
            }*/
        }

        private void GenerateRandomWalls()
        {
            Random r = new Random();
            for (int y = 0; y < ROWS; y++)
            {
                for (int x = 0; x < COLS; x++)
                {
                    if ((_addWalls && r.NextDouble() < 0.1f) || (x == 0 || y == 0 || x == COLS - 1 || y == ROWS - 1))
                    {
                        var wall = new Tile(new Character(Glyphs.Fill, Color.RosyBrown), new Vector2(x + (int)_position.X + 1, y + (int)_position.Y + 1), TileType.Solid);
                        _tiles[x, y] = wall;
                    }
                    else
                    {
                        var tile = new Tile(new Character(Glyphs.Period, Color.DarkGreen), new Vector2(x + (int)_position.X + 1, y + (int)_position.Y + 1));
                        _tiles[x, y] = tile;
                    }

                }
            }
        }
    }
}