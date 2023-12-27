using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoguelikeGame
{
    public class MapConsole : Console
    {
        public Map Map { get; private set; }
        public Vector2 Position => new Vector2(_x, _y);

        public MapConsole(string title, int width, int height, ConsoleLocation location, BorderStyle border, Color borderColor) : 
            base(title, width, height, location, border, borderColor)
        {
            Map = new Map(new Vector2(_x+1, _y+1), width, height);
        }

        public override void Draw()
        {
            Map.Draw();
            base.Draw();
        }
    }
}
