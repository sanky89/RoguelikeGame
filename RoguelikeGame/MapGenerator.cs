using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace RoguelikeGame
{
    public class MapGenerator
    {
        private MapConfiguration _mapConfig;

        public Map GenerateMap(MapConfiguration config, Player player, bool regenerate = false)
        {
            _mapConfig = config;
            System.Console.WriteLine($"Loading map config: Rows:{_mapConfig.Rows} Cols:{_mapConfig.Cols}");
            if(regenerate)
            {
                ClearMap(Globals.Map);
            }
            Map map = new Map(_mapConfig.Rows, _mapConfig.Cols, player);
            GenerateRooms(map);
            DropPlayerInRandomRoom(map);
            GenerateMonsters(map);
            GenerateItems(map);
            return map;
        }

        private void ClearMap(Map map)
        {
            for (int y = 0; y < _mapConfig.Rows; y++)
            {
                for (int x = 0; x < _mapConfig.Cols; x++)
                {

                    map.Tiles[x, y] = null;
                }
            }
            map.Rooms.Clear();
            map.Monsters.Clear();
            map.Items.Clear();
        }

        private void GenerateRooms(Map map)
        {
            map.Rooms = new List<Room>();
            for (int y = 0; y < _mapConfig.Rows; y++)
            {
                for (int x = 0; x < _mapConfig.Cols; x++)
                {

                    map.Tiles[x, y] = new Tile(new Character(Glyphs.MediumFill, Color.White, 1, 21), TileType.Solid);
                }
            }

            int count = 0;
            int failedAttempts = 0;
            int maxFailedAttempts = 500;
            while (count < _mapConfig.MaxRooms)
            {
                int width = Globals.Rng.Next(_mapConfig.MinRoomSize, _mapConfig.MaxRoomSize);
                int height = Globals.Rng.Next(_mapConfig.MinRoomSize, _mapConfig.MaxRoomSize);
                int x = Globals.Rng.Next(_mapConfig.Cols - width - 1);
                int y = Globals.Rng.Next(_mapConfig.Rows - height - 1);
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
                        map.Tiles[x, y].UpdateTile(new Character(Glyphs.Period, Color.White, 6, 21), TileType.Walkable);
                    }
                }
            }

            map.Rooms.Sort((a, b) => a.RoomRect.Right.CompareTo(b.RoomRect.Right));
            System.Console.WriteLine($"Failed Attempts {failedAttempts} Rooms - {map.Rooms.Count}");

            GenerateCorridors(map, map.Rooms);

            for (int y = 1; y < _mapConfig.Rows - 1; y++)
            {
                for (int x = 1; x < _mapConfig.Cols - 1; x++)
                {
                    if (map.Tiles[x, y].TileType == TileType.Solid &&
                        map.Tiles[x - 1, y].TileType == TileType.Solid &&
                        map.Tiles[x + 1, y].TileType == TileType.Solid &&
                        !IsUpperCornerTile(map, x, y))
                    {
                        map.Tiles[x, y].UpdateTile(new Character(Glyphs.MediumFill, Color.White, 1, 22), TileType.Solid);

                    }
                }
            }
        }

        private void GenerateCorridors(Map map, List<Room> rooms)
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
                    map.Tiles[i, start.Y].UpdateTile(new Character(Glyphs.Period, Color.White, 6, 21), TileType.Walkable);
                }

                int incr = end.Y < start.Y ? -1 : 1;
                for (int i = start.Y; ; i += incr)
                {
                    if (i == end.Y)
                    {
                        break;
                    }
                    map.Tiles[turningPoint.X, i].UpdateTile(new Character(Glyphs.Period, Color.White, 6, 21), TileType.Walkable);
                }
            }
        }

        private void GenerateMonsters(Map map)
        {
            for (int i = 0; i < _mapConfig.MaxMonstersCount; i++)
            {
                var room = map.Rooms[Globals.Rng.Next(map.Rooms.Count)];
                var point = room.GetRandomPointInsideRoom();
                Monster monster = Globals.AssetManager.CreateRandomMonster(i);

                if (monster != null)
                {
                    monster.SetMapPosition(map, point.X, point.Y);
                    map.Monsters.Add(monster);
                }
            }
        }

        private void GenerateItems(Map map)
        {
            for (int i = 0; i < _mapConfig.MaxItemsCount; i++)
            {
                var room = map.Rooms[Globals.Rng.Next(map.Rooms.Count)];
                var point = room.GetRandomPointInsideRoom();
                if (!map.IsMonsterTile(point.X, point.Y, out _))
                {
                    var item = Globals.AssetManager.CreateRandomItem();
                    item.SetMapPosition(map, point.X, point.Y);
                    map.SetTileType(point.X, point.Y, TileType.Walkable);
                    map.Items.Add(item);
                }
            }
        }

        private void DropPlayerInRandomRoom(Map map)
        {
            var rooms = map.Rooms;
            var startingRoom = rooms[Globals.Rng.Next(rooms.Count - 1)];
            var point = startingRoom.GetRandomPointInsideRoom();
            map.Player.SetMapPosition(map, point.X, point.Y);
            map.PlayerMapPosition = new Vec2Int(point.X, point.Y);
        }

        private bool IsUpperCornerTile(Map map, int x, int y)
        {
            return (map.Tiles[x - 1, y].TileType == TileType.Solid &&
                map.Tiles[x, y + 1].TileType == TileType.Solid &&
                map.Tiles[x - 1, y + 1].TileType == TileType.Walkable) ||
                (map.Tiles[x + 1, y].TileType == TileType.Solid &&
                map.Tiles[x, y + 1].TileType == TileType.Solid &&
                map.Tiles[x + 1, y + 1].TileType == TileType.Walkable);
        }
    }
}
