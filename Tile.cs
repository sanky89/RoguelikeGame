using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RogueTest
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

        public Tile(Character character, Vector2 position)
        {
            _character = character;
            Position = position;
            _tileType = TileType.Walkable;
        }

        public Tile(Character character, Vector2 position, TileType tileType)
        {
            _character = character;
            Position = position;
            _tileType = tileType;
        }

        public void SetOffset(int x, int y)
        {
            _offset = new Vector2(x, y);
        }

        public void Draw()
        {
            Globals.SpriteBatch.Draw(Globals.GlyphsTexture, (Position + _offset) * Globals.TILE_SIZE * Globals.SCALE, _character.GetSourceRect(), _character.Color, 0f, Vector2.Zero, Globals.SCALE, SpriteEffects.None, 0);
        }
    }
}