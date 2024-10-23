using Microsoft.Xna.Framework;

namespace RoguelikeGame
{
    public class Character
    {
        public Glyphs Glyph { get; private set; }
        public Color Color { get; private set; }
        public Color ColorDark { get; private set; }
        public int GlyphRow => _glyphRow;
        public int GlyphCol => _glyphCol;

        private int _glyphInt;
        private int _glyphRow;
        private int _glyphCol;
        private Console _parentConsole;

        public Character(Console console, char asciiChar, Color color)
        {
            Glyphs g = (Glyphs)(asciiChar);
            _parentConsole = console;
            SetupChar(g, color);
        }

        public Character(Console console,Glyphs glyph, Color color)
        {
            _parentConsole = console;
            SetupChar(glyph, color);
        }

        public Character(Glyphs glyph, Color color, int overrideRow, int overrideCol)
        {
            Glyph = glyph;
            Color = color;
            ColorDark = Color * 0.2f;
            _glyphRow = overrideRow;
            _glyphCol = overrideCol;
        }

        public Rectangle GetAsciiSourceRect()
        {
            return new Rectangle(_glyphCol * GameConstants.ASCII_SIZE, _glyphRow * GameConstants.ASCII_SIZE, GameConstants.ASCII_SIZE, GameConstants.ASCII_SIZE);
        }

        public Rectangle GetSourceRect()
        {
            return new Rectangle(_glyphCol * GameConstants.TILE_SIZE, _glyphRow * GameConstants.TILE_SIZE, GameConstants.TILE_SIZE, GameConstants.TILE_SIZE);
        }

        private void SetupChar(Glyphs glyph, Color color)
        {
            Glyph = glyph;
            Color = color;
            ColorDark = Color * 0.2f;
            _glyphInt = (int)glyph;
            _glyphRow = _glyphInt / _parentConsole.AsciiColumns;
            _glyphCol = _glyphInt % _parentConsole.AsciiRows;
        }
    }
}
