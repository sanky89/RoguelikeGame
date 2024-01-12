using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Numerics;
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
        public const int ROWS = 100;
        public const int COLS = 80;
        private const int MAX_ROOMS = 25;
        private const int MIN_ROOM_SIZE = 8;
        private const int MAX_ROOM_SIZE = 16;

        public readonly int ViewportWidth;
        public readonly int ViewportHeight;
        
        private List<Room> _rooms;
        private Tile[,] _tiles;
        private Player _player;
        private Vector2 _position;
        private bool _addWalls = true;
        private int _colStartIndex = 0;
        private int _rowStartIndex = 0;
        private Room _startingRoom;

        public int ColStartIndex => _colStartIndex;
        public int RowStartIndex => _rowStartIndex;

        public Map(Vector2 position, int viewportWidth, int viewportHeight, Player player)
        {
            _tiles = new Tile[COLS, ROWS];
            _position = position;
            ViewportWidth = viewportWidth;
            ViewportHeight = viewportHeight;
            _colStartIndex = 0;
            _rowStartIndex = 0;
            _rooms = new List<Room>(MAX_ROOMS);
            _player = player;
        }

        public void GenerateMap()
        {
            GenerateRooms();
            DropPlayerInRandomRoom();
        }

        public void RegenerateMap()
        {
            _colStartIndex = 0;
            _rowStartIndex = 0;
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

        public bool CanMove(int x, int y)
        {
            x -= (int)(_position.X);
            y -= (int)(_position.Y);

            if (x < 0 || y < 0 || x + _colStartIndex >= COLS || y + _rowStartIndex >= ROWS)
            {
                return false;
            }
            return _tiles[x + _colStartIndex, y + _rowStartIndex].TileType == TileType.Walkable;
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
            return x >= _colStartIndex && x < COLS &&
                        y >= _rowStartIndex && y < ROWS &&
                        x + _tiles[x, y].Offset.X < ViewportWidth - 1 &&
                        y + _tiles[x, y].Offset.Y < ViewportHeight - 1;
        }

        public void DrawMapTile(int x, int y)
        {
            if (x + _tiles[x, y].Offset.X < ViewportWidth - 1 &&
               y + _tiles[x, y].Offset.Y < ViewportHeight - 1)
            {
                _tiles[x, y].Draw();
                _player.Draw(Globals.SpriteBatch, Globals.GlyphsTexture);
            }
        }

        public void Draw()
        {
            for (int y = _rowStartIndex; y < ROWS; y++)
            {
                for (int x = _colStartIndex; x < COLS; x++)
                {
                    DrawMapTile(x, y);
                }
            }
        }

        public void ScrollMap(Direction direction)
        {
            switch (direction)
            {
                case Direction.LEFT:
                    _colStartIndex++;
                    if (_colStartIndex >= COLS - 1)
                    {
                        _colStartIndex = COLS - 1;
                    }
                    break;
                case Direction.RIGHT:
                    _colStartIndex--;
                    if (_colStartIndex < 0)
                    {
                        _colStartIndex = 0;
                    }
                    break;
                case Direction.UP:
                    _rowStartIndex++;
                    if (_rowStartIndex >= ROWS - 1)
                    {
                        _rowStartIndex = ROWS - 1;
                    }
                    break;
                case Direction.DOWN:
                    _rowStartIndex--;
                    if (_rowStartIndex < 0)
                    {
                        _rowStartIndex = 0;
                    }
                    break;
                default:
                    break;
            }

            for (int y = 0; y < ROWS; y++)
            {
                for (int x = 0; x < COLS; x++)
                {
                    _tiles[x, y].SetOffset(-_colStartIndex, -_rowStartIndex);
                }
            }
        }

        private void DropPlayerInRandomRoom()
        {
            Random r = new Random();
            _startingRoom = _rooms[r.Next(MAX_ROOMS - 1)];
            int startingRoomRightEdge = _startingRoom.PaddedRoomRect.Right + 1;
            int startingRoomBottomEdge = _startingRoom.PaddedRoomRect.Bottom + 1;
            int diffX = startingRoomRightEdge - ViewportWidth;
            int diffY = startingRoomBottomEdge - ViewportHeight;
            while (diffX >= 0)
            {
                ScrollMap(Direction.LEFT);
                diffX--;
            }
            while (diffY >= 0)
            {
                ScrollMap(Direction.UP);
                diffY--;
            }
            var point = _startingRoom.GetRandomPointInsideRoom();
            Vector2 scrolledPosition = new Vector2(point.X - _colStartIndex, point.Y - _rowStartIndex);
            _player.SetInitialMapPosition(_position + scrolledPosition, (int)point.X, (int)point.Y);
        }

        private void GenerateRooms()
        {
            for (int y = 0; y < ROWS; y++)
            {
                for (int x = 0; x < COLS; x++)
                {
                    
                    _tiles[x, y] = new Tile(new Character(Glyphs.DarkFill, Color.Brown), new Vector2(x + (int)_position.X, y + (int)_position.Y), TileType.Solid);
                }
            }

            Random r = new Random();
            int count = 0;
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
                        break;
                    }
                }
                if (!intersects)
                {
                    _rooms.Add(room);
                    count++;
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

            GenerateCorridors();
        }

        private void GenerateTwoRooms()
        {
            for (int y = 0; y < ROWS; y++)
            {
                for (int x = 0; x < COLS; x++)
                {
                    _tiles[x, y] = new Tile(new Character(Glyphs.Fill, Color.RosyBrown), new Vector2(x + (int)_position.X + 1, y + (int)_position.Y + 1), TileType.Solid);
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

                var turningPoint = new Vector2(end.X, start.Y);

                for (int i = (int)start.X; i <= (int)turningPoint.X; i++)
                {
                    _tiles[i, (int)start.Y].UpdateTile(new Character(Glyphs.Period, Color.Green), TileType.Walkable);
                }

                int incr = end.Y < start.Y ? -1 : 1;
                for (int i = (int)start.Y; ; i += incr)
                {
                    if (i == (int)end.Y)
                    {
                        break;
                    }
                    _tiles[(int)turningPoint.X, i].UpdateTile(new Character(Glyphs.Period, Color.Green), TileType.Walkable);
                }
            }
        }

        private void DrawTestRoom()
        {
            for (int y = 0; y < ROWS; y++)
            {
                for (int x = 0; x < COLS; x++)
                {
                    _tiles[x, y] = new Tile(new Character(Glyphs.Period, Color.Green), new Vector2(x + (int)_position.X + 1, y + (int)_position.Y + 1), TileType.Walkable);
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
                    if ((_addWalls && r.NextDouble() < 0.1f) || (x == 0 || y == 0 || x == COLS - 1 || y == ROWS - 1))
                    {
                        var wall = new Tile(new Character(Glyphs.Fill, Color.RosyBrown), new Vector2(x + (int)_position.X + 1, y + (int)_position.Y + 1), TileType.Solid);
                        _tiles[x, y] = wall;
                    }
                    else
                    {
                        var tile = new Tile(new Character(Glyphs.Period, Color.Green), new Vector2(x + (int)_position.X + 1, y + (int)_position.Y + 1));
                        _tiles[x, y] = tile;
                    }
                }
            }
        }

    }
}
