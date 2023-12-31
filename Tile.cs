using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RoguelikeGame
{
    public enum TileType
    {
        Walkable,
        Solid,
        Transparent
    }

    public class Tile
    {
        private Character _character;
        private TileType _tileType;
        private Vector2 _offset = Vector2.Zero;
        public readonly Vector2 Position;

        public TileType TileType => _tileType;
        public Vector2 Offset => _offset;
        public bool Visible { get; set; }
        public bool Visited { get; set; }

        private Color _displayColor;

        public Tile(Character character, Vector2 position) : this(character, position, TileType.Walkable)
        {
        }

        public Tile(Character character, Vector2 position, TileType tileType)
        {
            _character = character;
            Position = position;
            _tileType = tileType;
            Visible = false;
            Visited = false;
            _displayColor = _character.Color;
        }

        public void SetOffset(int x, int y)
        {
            _offset = new Vector2(x, y);
        }

        public void UpdateTile(Character character, TileType tileType)
        {
            _character = character;
            _tileType = tileType;
        }

        public void Draw()
        {
            if (!Visible && !Visited)
            {
                return;
            }
            if(Visible)
            {
                _displayColor = _character.Color;
            }
            else if(Visited)
            {
                _displayColor = _character.ColorDark;
            }

            Globals.SpriteBatch.Draw(Globals.GlyphsTexture, (Position + _offset) * Globals.TILE_SIZE * Globals.SCALE, _character.GetSourceRect(), _displayColor, 0f, Vector2.Zero, Globals.SCALE, SpriteEffects.None, 0);
        }
    }
}