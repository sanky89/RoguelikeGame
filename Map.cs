using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;

namespace RogueTest
{
    public class Map
    {
        public const int ROWS = 40;
        public const int COLS = 60;

        private Tile[,] _tiles;
        private Player _player;
        private Vector2 _position;
        private readonly int _viewportWidth;
        private readonly int _viewportHeight;
        private bool _addWalls = false;

        public Map(Vector2 position, int viewportWidth, int viewportHeight)
        {
            _tiles = new Tile[COLS, ROWS];
            _position = position;
            _viewportWidth = viewportWidth;
            _viewportHeight = viewportHeight;
           
            Random r = new Random();
            for (int y = 0; y < ROWS; y++)
            {
                for (int x = 0; x < COLS; x++)
                {
                    if (_addWalls && r.NextDouble() < 0.2f)
                    {
                        var wall = new Tile(new Character(Glyphs.Fill, Color.RosyBrown), new Vector2(x + (int)_position.X + 1, y + (int)_position.Y + 1), TileType.Solid);
                        _tiles[x, y] = wall;
                    }
                    else
                    {
                        var tile = new Tile(new Character(Glyphs.LightFill, Color.DarkGreen), new Vector2(x + (int)_position.X + 1, y + (int)_position.Y + 1));
                        _tiles[x, y] = tile;
                    }
                    
                }
            }
        }

        public void SetPlayer(Player player)
        {
            _player = player;
            _player.SetInitialMapPosition(_position + Vector2.One);
        }

        public bool CanMove(int x, int y)
        {
            x -= (int)(_position.X + 1);
            y -= (int)(_position.Y + 1);

            if(x < 0 || y < 0 || x >= COLS || y >= ROWS)
            {
                return false;
            }
            return _tiles[x, y].TileType == TileType.Walkable;
        }

        public void DrawMapTile(int x, int y)
        {
            if(x >= _viewportWidth-1 || y >= _viewportHeight-1)
            {
                return;
            }

            _tiles[x, y].Draw();
            _player.Draw(Globals.SpriteBatch, Globals.GlyphsTexture);
        }

        public void Draw()
        {
            for (int y = 0; y < ROWS; y++)
            {
                for (int x = 0; x < COLS; x++)
                {
                    DrawMapTile(x, y);
                }
            }
        }
        
    }
}
