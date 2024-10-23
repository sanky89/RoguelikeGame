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
        protected GameRoot _gameRoot;
        private Texture2D _asciiTexture;

        public int AsciiColumns => _gameRoot.AsciiColumns;
        public int AsciiRows => _gameRoot.AsciiRows;

        public Console(GameRoot gameRoot, Texture2D asciiTexture, string title, int width, int height, ConsoleLocation location, BorderStyle border = BorderStyle.None, Color borderColor = default)
        {
            _gameRoot = gameRoot;
            _asciiTexture = asciiTexture;
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
                    _titleCharacters[i] = new Character(this, Title[i], BorderColor);
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
                _vertical =         new Character(this, Glyphs.BarUpDown, BorderColor);
                _horizontal =       new Character(this, Glyphs.BarLeftRight, BorderColor);
                _upperLeftCorner =  new Character(this, Glyphs.BarDownRight, BorderColor); 
                _upperRightCorner = new Character(this, Glyphs.BarDownLeft, BorderColor);
                _lowerLeftCorner =  new Character(this, Glyphs.BarUpRight, BorderColor);
                _lowerRightCorner = new Character(this, Glyphs.BarUpLeft, BorderColor);
            }
            else
            {
                _vertical =         new Character(this, Glyphs.BarDoubleUpDown, BorderColor);
                _horizontal =       new Character(this, Glyphs.BarDoubleLeftRight, BorderColor);
                _upperLeftCorner =  new Character(this, Glyphs.BarDoubleDownRight, BorderColor);
                _upperRightCorner = new Character(this, Glyphs.BarDoubleDownLeft, BorderColor);
                _lowerLeftCorner =  new Character(this, Glyphs.BarDoubleUpRight, BorderColor);
                _lowerRightCorner = new Character(this, Glyphs.BarDoubleUpLeft, BorderColor);
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
                    _x = GameConstants.SCREEN_WIDTH/GameConstants.ASCII_SIZE - Width;
                    _y = 0;
                    break;
                case ConsoleLocation.BottomLeft:
                    _x = 0;
                    _y = GameConstants.SCREEN_HEIGHT / GameConstants.ASCII_SIZE - Height-1;
                    break;
                case ConsoleLocation.BottomRight:
                    _x = GameConstants.SCREEN_WIDTH / GameConstants.ASCII_SIZE - Width;
                    _y = GameConstants.SCREEN_HEIGHT / GameConstants.ASCII_SIZE - Height-1;
                    break;
                default:
                    return;
            }
        }

        private void DrawTitle(int x)
        {
            if(!string.IsNullOrEmpty(Title))
                _gameRoot.SpriteBatch.Draw(_asciiTexture, new Vector2(x, _y) * GameConstants.ASCII_SIZE, _titleCharacters[x - _titleStartIndex].GetAsciiSourceRect(), BorderColor, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0);
        }

        private void DrawTitleString(int x)
        {
            _gameRoot.SpriteBatch.DrawString(_gameRoot.Font, Title, new Vector2(x, _y) * GameConstants.ASCII_SIZE, BorderColor);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if(Border == BorderStyle.None)
            {
                return;
            }
            DrawTitleString(_titleStartIndex);

            for (int x = _x+1; x < _x + Width; x++)
            {
                if (x < _titleStartIndex || x > _titleEndIndex)
                {
                    _gameRoot.SpriteBatch.Draw(_asciiTexture, new Vector2(x, _y) * GameConstants.ASCII_SIZE, _horizontal.GetAsciiSourceRect(), BorderColor, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0);
                }
                _gameRoot.SpriteBatch.Draw(_asciiTexture, new Vector2(x, _y + Height) * GameConstants.ASCII_SIZE, _horizontal.GetAsciiSourceRect(), BorderColor, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0);
            }

            for (int y = _y+1; y < _y + Height; y++)
            {
                _gameRoot.SpriteBatch.Draw(_asciiTexture, new Vector2(_x, y) * GameConstants.ASCII_SIZE, _vertical.GetAsciiSourceRect(), BorderColor, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0);
                _gameRoot.SpriteBatch.Draw(_asciiTexture, new Vector2(_x + Width, y) * GameConstants.ASCII_SIZE, _vertical.GetAsciiSourceRect(), BorderColor, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0);
            }

            _gameRoot.SpriteBatch.Draw(_asciiTexture, new Vector2(_x,_y) * GameConstants.ASCII_SIZE, _upperLeftCorner.GetAsciiSourceRect(), BorderColor, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0);
            _gameRoot.SpriteBatch.Draw(_asciiTexture, new Vector2(_x+Width, _y) * GameConstants.ASCII_SIZE, _upperRightCorner.GetAsciiSourceRect(), BorderColor, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0);
            _gameRoot.SpriteBatch.Draw(_asciiTexture, new Vector2(_x, _y+Height) * GameConstants.ASCII_SIZE, _lowerLeftCorner.GetAsciiSourceRect(), BorderColor, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0);
            _gameRoot.SpriteBatch.Draw(_asciiTexture, new Vector2(_x+Width, _y+Height) * GameConstants.ASCII_SIZE, _lowerRightCorner.GetAsciiSourceRect(), BorderColor, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0);
        }
    }
}
