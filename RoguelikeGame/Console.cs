using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RoguelikeGame
{
    public enum ConsoleLocation
    {
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight,
    }

    public enum BorderStyle
    {
        None,
        SingleLine,
        DoubleLine
    }

    public class Console
    {
        public readonly string Title;
        public readonly int Width;
        public readonly int Height;
        public readonly ConsoleLocation Location;
        public readonly BorderStyle Border;
        public readonly Color BorderColor;
        protected float Scale = 1.0f;

        private Character _vertical;
        private Character _horizontal;
        private Character _upperLeftCorner;
        private Character _lowerLeftCorner;
        private Character _upperRightCorner;
        private Character _lowerRightCorner;

        protected int _x, _y;
        private int _titleStartIndex = 0;
        private int _titleEndIndex = 0;
        private Character[] _titleCharacters;

        public Console(string title, int width, int height, ConsoleLocation location, BorderStyle border = BorderStyle.None, Color borderColor = default)
        {
            Title = title;
            Width = width;
            Height = height;
            Location = location;
            Border = border;
            BorderColor = borderColor;
            SetupGlyphs();
            SetupStartingCorner();
            SetupTitle();
        }

        private void SetupTitle()
        {
            if(Title != null && Title.Length > 0)
            {
                _titleCharacters = new Character[Title.Length];
                _titleStartIndex = _x + (Width - Title.Length)/2;
                _titleEndIndex = _titleStartIndex + Title.Length - 1;
                for (int i = 0; i < Title.Length; i++)
                {
                    _titleCharacters[i] = new Character(Title[i], BorderColor);
                }
            }
        }

        private void SetupGlyphs()
        {
            if(Border == BorderStyle.None)
            {
                return;
            }

            if (Border == BorderStyle.SingleLine)
            {
                _vertical = new Character(Glyphs.BarUpDown, BorderColor);
                _horizontal = new Character(Glyphs.BarLeftRight, BorderColor);
                _upperLeftCorner = new Character(Glyphs.BarDownRight, BorderColor); 
                _upperRightCorner = new Character(Glyphs.BarDownLeft, BorderColor);
                _lowerLeftCorner = new Character(Glyphs.BarUpRight, BorderColor);
                _lowerRightCorner = new Character(Glyphs.BarUpLeft, BorderColor);
            }
            else
            {
                _vertical = new Character(Glyphs.BarDoubleUpDown, BorderColor);
                _horizontal = new Character(Glyphs.BarDoubleLeftRight, BorderColor);
                _upperLeftCorner = new Character(Glyphs.BarDoubleDownRight, BorderColor);
                _upperRightCorner = new Character(Glyphs.BarDoubleDownLeft, BorderColor);
                _lowerLeftCorner = new Character(Glyphs.BarDoubleUpRight, BorderColor);
                _lowerRightCorner = new Character(Glyphs.BarDoubleUpLeft, BorderColor);
            }

        }

        private void SetupStartingCorner()
        {
            switch(Location)
            {
                case ConsoleLocation.TopLeft:
                    _x = 0;
                    _y = 0;
                    break;
                case ConsoleLocation.TopRight:
                    _x = Globals.SCREEN_WIDTH/Globals.TILE_SIZE - Width;
                    _y = 0;
                    break;
                case ConsoleLocation.BottomLeft:
                    _x = 0;
                    _y = Globals.SCREEN_HEIGHT / Globals.TILE_SIZE - Height-1;
                    break;
                case ConsoleLocation.BottomRight:
                    _x = Globals.SCREEN_WIDTH / Globals.TILE_SIZE - Width;
                    _y = Globals.SCREEN_HEIGHT / Globals.TILE_SIZE - Height-1;
                    break;
                default:
                    return;
            }
        }

        private void DrawTitle(int x)
        {
            if(!string.IsNullOrEmpty(Title))
                Globals.SpriteBatch.Draw(Globals.AsciiTexture, new Vector2(x, _y) * Globals.TILE_SIZE, _titleCharacters[x - _titleStartIndex].GetSourceRect(), BorderColor, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0);
        }

        public virtual void Draw()
        {
            if(Border == BorderStyle.None)
            {
                for (int x = _titleStartIndex; x <= _titleEndIndex; x++)
                {
                    DrawTitle(x);
                }
                return;
            }

            for (int x = _x+1; x < _x + Width; x++)
            {
                if (x < _titleStartIndex || x > _titleEndIndex)
                {
                    Globals.SpriteBatch.Draw(Globals.AsciiTexture, new Vector2(x, _y) * Globals.TILE_SIZE, _horizontal.GetSourceRect(), BorderColor, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0);
                }
                else
                {
                    DrawTitle(x);
                }
                Globals.SpriteBatch.Draw(Globals.AsciiTexture, new Vector2(x, _y + Height) * Globals.TILE_SIZE, _horizontal.GetSourceRect(), BorderColor, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0);
            }

            for (int y = _y+1; y < _y + Height; y++)
            {
                Globals.SpriteBatch.Draw(Globals.AsciiTexture, new Vector2(_x, y) * Globals.TILE_SIZE, _vertical.GetSourceRect(), BorderColor, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0);
                Globals.SpriteBatch.Draw(Globals.AsciiTexture, new Vector2(_x + Width, y) * Globals.TILE_SIZE, _vertical.GetSourceRect(), BorderColor, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0);
            }

            Globals.SpriteBatch.Draw(Globals.AsciiTexture, new Vector2(_x,_y) * Globals.TILE_SIZE, _upperLeftCorner.GetSourceRect(), BorderColor, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0);
            Globals.SpriteBatch.Draw(Globals.AsciiTexture, new Vector2(_x+Width, _y) * Globals.TILE_SIZE, _upperRightCorner.GetSourceRect(), BorderColor, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0);
            Globals.SpriteBatch.Draw(Globals.AsciiTexture, new Vector2(_x, _y+Height) * Globals.TILE_SIZE, _lowerLeftCorner.GetSourceRect(), BorderColor, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0);
            Globals.SpriteBatch.Draw(Globals.AsciiTexture, new Vector2(_x+Width, _y+Height) * Globals.TILE_SIZE, _lowerRightCorner.GetSourceRect(), BorderColor, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0);
        }
    }
}
