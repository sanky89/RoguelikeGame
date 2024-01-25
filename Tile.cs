using Microsoft.Xna.Framework;

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
        private Vector2 _offset = Vector2.Zero;
        public readonly Vector2 Position;

        public TileType TileType { get; set; }
        public Vector2 Offset => _offset;
        public bool Visible { get; set; }
        public bool Visited { get; set; }

        public Color DisplayColor
        {
            get
            {
                if(Visible)
                {
                    return _character.Color;
                }
                return _character.ColorDark;
            }
        }

        public Rectangle SourceRect => _character.GetSourceRect();

        public Tile(Character character, Vector2 position) : this(character, position, TileType.Walkable)
        {
        }

        public Tile(Character character, Vector2 position, TileType tileType)
        {
            _character = character;
            Position = position;
            TileType = tileType;
            Visible = false;
            Visited = false;
        }

        public Tile(Character character, TileType tileType)
        {
            _character = character;
            TileType = tileType;
            Visible = false;
            Visited = false;
        }

        public void SetOffset(int x, int y)
        {
            _offset = new Vector2(x, y);
        }

        public void UpdateTile(Character character, TileType tileType)
        {
            _character = character;
            TileType = tileType;
        }
    }
}