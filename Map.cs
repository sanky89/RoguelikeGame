using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Vector2 = Microsoft.Xna.Framework.Vector2;

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
        public const int ROWS = 80;
        public const int COLS = 140;
        private const int MAX_ROOMS = 30;
        private const int MIN_ROOM_SIZE = 8;
        private const int MAX_ROOM_SIZE = 20;
        
        private List<Room> _rooms;
        private Tile[,] _tiles;
        private Player _player;
        private Room _startingRoom;
        public Player Player => _player;

        public List<Entity> Monsters;

        public Map(Player player)
        {
            _tiles = new Tile[COLS, ROWS];
            _rooms = new List<Room>();
            _player = player;
            Monsters = new List<Entity>();
        }

        public Tile GetTileAtIndex(int x, int y)
        {
            if(!IsWithinMapRange(x,y))
            {
                throw new ArgumentOutOfRangeException($"Index out of range {x}, {y}");
            }
            return _tiles[x, y];
        }

        public void GenerateMap()
        {
            GenerateRooms();
            DropPlayerInRandomRoom();
            for (int i = 0; i < 20; i++)
            {
                DropMonsterInRandomRoom();
            }
        }

        private void DropMonsterInRandomRoom()
        {
            Random r = new Random();
            var room = _rooms[r.Next(_rooms.Count - 1)];
            var point = room.GetRandomPointInsideRoom();
            var monster = new Entity(new Character(Glyphs.ZUpper, Color.Red), Vector2.Zero);
            monster.SetMapPosition(point.X, point.Y);
            SetTileType(point.X, point.Y, TileType.Solid);
            Monsters.Add(monster);
        }

        public void RegenerateMap()
        {
            _startingRoom = null;
            _rooms.Clear();
            Array.Clear(_tiles, 0, _tiles.Length);
            _tiles = new Tile[COLS, ROWS];
            GenerateRooms();
            DropPlayerInRandomRoom();
        }

        public void SetPlayer(Player player)
        {
            _player = player;
            DropPlayerInRandomRoom();
            //_player.SetInitialMapPosition(_position + new Vector2(41, 35), 41, 35);
        }

        private void SetTileType(int x, int y, TileType type)
        {
            _tiles[x, y].TileType = type;
        }

        public ActionResult CanMove(int x, int y)
        {
            if (x < 0 || y < 0 || x >= COLS || y >= ROWS)
            { 
                return ActionResult.None; 
            }

            var tile = _tiles[x, y];
            if (tile.TileType == TileType.Walkable)
            {
                return ActionResult.Move;
            }
            else if(IsMonsterTile(x, y, out _))
            {
                return ActionResult.HitEntity;
            }

            return ActionResult.HitWall;
        }

        public void ToggleMapVisible(bool visible)
        {
            foreach (var tile in _tiles)
            {
                tile.Visible = visible;
            }
        }

        public void ToggleTileVisible(int x, int y, bool visible)
        {
            _tiles[x, y].Visible = visible;
        }

        public void ToggleMapVisited(bool visited)
        {
            foreach (var tile in _tiles)
            {
                tile.Visited = visited;
            }
        }

        public void ToggleTileVisited(int x, int y, bool visited)
        {
            _tiles[x, y].Visited = visited;
        }

        public TileType GetTileType(int x, int y)
        {
            if (!IsWithinMapRange(x, y))
            {
                throw new ArgumentOutOfRangeException();
            }
            return _tiles[x, y].TileType;
        }

        public bool IsWithinMapRange(int x, int y)
        {
            return x >= 0 && x < COLS &&
                        y >= 0 && y < ROWS;
        }

        public bool IsPlayerTile(int x, int y)
        {
            return x == _player.MapX && y == _player.MapY;
        }

        public bool IsMonsterTile(int x, int y, out Entity m)
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

        private void DropPlayerInRandomRoom()
        {
            Random r = new Random();
            _startingRoom = _rooms[r.Next(_rooms.Count - 1)];
            if(_startingRoom.RoomRect.Right > Globals.MAP_CONSOLE_WIDTH || 
               _startingRoom.RoomRect.Bottom > Globals.MAP_CONSOLE_HEIGHT)
            {
                DropPlayerInRandomRoom();
                return;
            }    
            var point = _startingRoom.GetRandomPointInsideRoom();
            _player.SetMapPosition(point.X, point.Y);
        }

        private void GenerateRooms()
        {
            for (int y = 0; y < ROWS; y++)
            {
                for (int x = 0; x < COLS; x++)
                {
                    
                    _tiles[x, y] = new Tile(new Character(Glyphs.MediumFill, Color.DarkGray), TileType.Solid);
                }
            }

            Random r = new Random();
            int count = 0;
            int failedAttempts = 0;
            int maxFailedAttempts = 500;
            while (count < MAX_ROOMS)
            {
                int width = r.Next(MIN_ROOM_SIZE, MAX_ROOM_SIZE);
                int height = r.Next(MIN_ROOM_SIZE, MAX_ROOM_SIZE);
                int x = r.Next(COLS - width - 1);
                int y = r.Next(ROWS - height - 1);
                Room room = new Room(x, y, width, height);
                bool intersects = false;
                foreach (var room1 in _rooms)
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
                    _rooms.Add(room);
                    count++;
                }
                if(failedAttempts >= maxFailedAttempts)
                {
                    break;
                }
            }


            foreach (var room in _rooms)
            {
                for (int y = room.RoomRect.Y; y < room.RoomRect.Bottom; y++)
                {
                    for (int x = room.RoomRect.X; x < room.RoomRect.Right; x++)
                    {
                        _tiles[x, y].UpdateTile(new Character(Glyphs.Period, Color.Green), TileType.Walkable);
                    }
                }
            }

            _rooms.Sort((a, b) => a.RoomRect.Right.CompareTo(b.RoomRect.Right));
            System.Console.WriteLine($"Failed Attempts {failedAttempts} Rooms - {_rooms.Count}");

            GenerateCorridors();
        }

        private void GenerateTwoRooms()
        {
            for (int y = 0; y < ROWS; y++)
            {
                for (int x = 0; x < COLS; x++)
                {
                    _tiles[x, y] = new Tile(new Character(Glyphs.Fill, Color.RosyBrown), TileType.Solid);
                }
            }

            Room room1 = new Room(30, 10, 10, 10);
            Room room2 = new Room(30, 30, 10, 10);
            _rooms.Add(room1);
            _rooms.Add(room2);
            foreach (var room in _rooms)
            {
                for (int y = room.RoomRect.Y; y < room.RoomRect.Bottom; y++)
                {
                    for (int x = room.RoomRect.X; x < room.RoomRect.Right; x++)
                    {
                        _tiles[x, y].UpdateTile(new Character(Glyphs.Period, Color.Green), TileType.Walkable);
                    }
                }
            }
            GenerateCorridors();
        }

        private void GenerateCorridors()
        {
            for (int r = 0; r < _rooms.Count - 1; r++)
            {
                Room room1 = _rooms[r];
                Room room2 = _rooms[r + 1];
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
                    _tiles[i, start.Y].UpdateTile(new Character(Glyphs.Period, Color.Green), TileType.Walkable);
                }

                int incr = end.Y < start.Y ? -1 : 1;
                for (int i = start.Y; ; i += incr)
                {
                    if (i == end.Y)
                    {
                        break;
                    }
                    _tiles[turningPoint.X, i].UpdateTile(new Character(Glyphs.Period, Color.Green), TileType.Walkable);
                }
            }
        }

        private void GenerateTestRoom()
        {
            for (int y = 0; y < ROWS; y++)
            {
                for (int x = 0; x < COLS; x++)
                {
                    _tiles[x, y] = new Tile(new Character(Glyphs.Period, Color.Green), TileType.Walkable);
                }
            }

            _tiles[40, 30].UpdateTile(new Character(Glyphs.Digit0, Color.Brown), TileType.Solid);
            _tiles[41, 30].UpdateTile(new Character(Glyphs.Digit1, Color.Brown), TileType.Solid);
            _tiles[42, 30].UpdateTile(new Character(Glyphs.Digit2, Color.Brown), TileType.Solid);
            _tiles[40, 29].UpdateTile(new Character(Glyphs.Digit3, Color.Brown), TileType.Solid);
            _tiles[41, 29].UpdateTile(new Character(Glyphs.Digit4, Color.Brown), TileType.Solid);
            //_tiles[42, 29].UpdateTile(new Character(Glyphs.Fill, Color.Brown), TileType.Solid);
            _tiles[40, 28].UpdateTile(new Character(Glyphs.Digit5, Color.Brown), TileType.Solid);
            _tiles[41, 28].UpdateTile(new Character(Glyphs.Digit6, Color.Brown), TileType.Solid);
            //_tiles[42, 28].UpdateTile(new Character(Glyphs.Fill, Color.Brown), TileType.Solid);
            _tiles[40, 27].UpdateTile(new Character(Glyphs.Digit7, Color.Brown), TileType.Solid);
            _tiles[41, 27].UpdateTile(new Character(Glyphs.Digit8, Color.Brown), TileType.Solid);
            _tiles[42, 27].UpdateTile(new Character(Glyphs.Digit9, Color.Brown), TileType.Solid);
        }

        private void GenerateRandomWalls()
        {
            Random r = new Random();
            for (int y = 0; y < ROWS; y++)
            {
                for (int x = 0; x < COLS; x++)
                {
                    if ((r.NextDouble() < 0.1f) || (x == 0 || y == 0 || x == COLS - 1 || y == ROWS - 1))
                    {
                        _tiles[x, y] = new Tile(new Character(Glyphs.Fill, Color.RosyBrown), TileType.Solid);
                    }
                    else
                    {
                        _tiles[x, y] = new Tile(new Character(Glyphs.Period, Color.Green), TileType.Walkable);
                    }
                }
            }
        }

    }
}
