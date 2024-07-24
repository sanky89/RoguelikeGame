using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Data;

namespace RoguelikeGame
{
    public class MapGenerator
    {
        public const int ROWS = 80;
        public const int COLS = 140;
        private const int MAX_ROOMS = 30;
        private const int MIN_ROOM_SIZE = 8;
        private const int MAX_ROOM_SIZE = 20;
        private const int MAX_MONSTERS = 50;
        private const int MAX_ITEMS = 50;
        private Tile[,] _tiles;

        public Map GenerateMap()
        {
            Map map = new Map(ROWS, COLS);
            GenerateRooms(map);
            DropPlayerInRandomRoom(map);

            return map;
        }

        private void GenerateRooms(Map map)
        {
            map.Rooms = new List<Room>();
            for (int y = 0; y < ROWS; y++)
            {
                for (int x = 0; x < COLS; x++)
                {

                    map.Tiles[x, y] = new Tile(new Character(Glyphs.MediumFill, Color.White, 1, 21), TileType.Solid);
                }
            }

            int count = 0;
            int failedAttempts = 0;
            int maxFailedAttempts = 500;
            while (count < MAX_ROOMS)
            {
                int width = Globals.Rng.Next(MIN_ROOM_SIZE, MAX_ROOM_SIZE);
                int height = Globals.Rng.Next(MIN_ROOM_SIZE, MAX_ROOM_SIZE);
                int x = Globals.Rng.Next(COLS - width - 1);
                int y = Globals.Rng.Next(ROWS - height - 1);
                Room room = new Room(x, y, width, height);
                bool intersects = false;
                foreach (var room1 in map.Rooms)
                {
                    if (room.IntersectsWithPadding(room1))
                    {
                        intersects = true;
                        failedAttempts++;
                        break;
                    }
                }
                if (!intersects)
                {
                    map.Rooms.Add(room);
                    count++;
                }
                if (failedAttempts >= maxFailedAttempts)
                {
                    break;
                }
            }


            foreach (var room in map.Rooms)
            {
                for (int y = room.RoomRect.Y; y < room.RoomRect.Bottom; y++)
                {
                    for (int x = room.RoomRect.X; x < room.RoomRect.Right; x++)
                    {
                        _tiles[x, y].UpdateTile(new Character(Glyphs.Period, Color.White, 6, 21), TileType.Walkable);
                    }
                }
            }

            map.Rooms.Sort((a, b) => a.RoomRect.Right.CompareTo(b.RoomRect.Right));
            System.Console.WriteLine($"Failed Attempts {failedAttempts} Rooms - {map.Rooms.Count}");

            GenerateCorridors(map.Rooms);

            for (int y = 1; y < ROWS - 1; y++)
            {
                for (int x = 1; x < COLS - 1; x++)
                {
                    if (_tiles[x, y].TileType == TileType.Solid &&
                        _tiles[x - 1, y].TileType == TileType.Solid &&
                        _tiles[x + 1, y].TileType == TileType.Solid &&
                        !IsUpperCornerTile(x, y))
                    {
                        _tiles[x, y].UpdateTile(new Character(Glyphs.MediumFill, Color.White, 1, 22), TileType.Solid);

                    }
                }
            }
        }

        private void GenerateCorridors(List<Room> rooms)
        {
            for (int r = 0; r < rooms.Count - 1; r++)
            {
                Room room1 = rooms[r];
                Room room2 = rooms[r + 1];
                var start = room1.GetRandomPointInsideRoom();
                var end = room2.GetRandomPointInsideRoom();
                if (end.X < start.X)
                {
                    var temp = start;
                    start = end;
                    end = temp;
                }

                var turningPoint = new Point(end.X, start.Y);

                for (int i = start.X; i <= turningPoint.X; i++)
                {
                    _tiles[i, start.Y].UpdateTile(new Character(Glyphs.Period, Color.White, 6, 21), TileType.Walkable);
                }

                int incr = end.Y < start.Y ? -1 : 1;
                for (int i = start.Y; ; i += incr)
                {
                    if (i == end.Y)
                    {
                        break;
                    }
                    _tiles[turningPoint.X, i].UpdateTile(new Character(Glyphs.Period, Color.White, 6, 21), TileType.Walkable);
                }
            }
        }

        private void GenerateMonsters(Map map)
        {
            for (int i = 0; i < MAX_MONSTERS; i++)
            {
                var room = map.Rooms[Globals.Rng.Next(map.Rooms.Count)];
                var point = room.GetRandomPointInsideRoom();
                Monster monster = Globals.AssetManager.CreateRandomMonster(i);

                if (monster != null)
                {
                    monster.SetMapPosition(point.X, point.Y);
                    map.SetTileType(point.X, point.Y, TileType.Entity);
                    map.Monsters.Add(monster);
                }
            }
        }

        private void DropPlayerInRandomRoom(Map map)
        {
            var rooms = map.Rooms;
            var startingRoom = rooms[Globals.Rng.Next(rooms.Count - 1)];
            var point = startingRoom.GetRandomPointInsideRoom();
            map.Player.SetMapPosition(point.X, point.Y);
            map.PlayerMapPosition = new Vec2Int(point.X, point.Y);
        }

        private bool IsUpperCornerTile(int x, int y)
        {
            return (_tiles[x - 1, y].TileType == TileType.Solid &&
                _tiles[x, y + 1].TileType == TileType.Solid &&
                _tiles[x - 1, y + 1].TileType == TileType.Walkable) ||
                (_tiles[x + 1, y].TileType == TileType.Solid &&
                _tiles[x, y + 1].TileType == TileType.Solid &&
                _tiles[x + 1, y + 1].TileType == TileType.Walkable);

        }

    }
}
