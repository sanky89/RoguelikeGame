using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueTest
{
    public class Character
    {
        public Glyphs Glyph { get; private set; }
        public Color Color { get; private set; }
        public int GlyphRow => _glyphRow;
        public int GlyphCol => _glyphCol;

        private int _glyphInt;
        private int _glyphRow;
        private int _glyphCol;

        public Character(char asciiChar, Color color)
        {
            Glyphs g = (Glyphs)(asciiChar);
            SetupChar(g, color);
        }

        public Character(Glyphs glyph, Color color)
        {
            SetupChar(glyph, color);
        }

        public Rectangle GetSourceRect()
        {
            return new Rectangle(_glyphCol * Globals.TILE_SIZE, _glyphRow * Globals.TILE_SIZE, Globals.TILE_SIZE, Globals.TILE_SIZE);
        }

        private void SetupChar(Glyphs glyph, Color color)
        {
            Glyph = glyph;
            Color = color;

            _glyphInt = (int)glyph;
            _glyphRow = _glyphInt / Globals.Columns;
            _glyphCol = _glyphInt % Globals.Columns;
        }
    }
}
