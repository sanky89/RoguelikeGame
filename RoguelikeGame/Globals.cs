using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using System;

namespace RoguelikeGame
{
    public static class Globals
    {
        public const int SCREEN_WIDTH = 1280;
        public const int SCREEN_HEIGHT = 720;
        public const int RENDER_TARGET_WIDTH = 1280;
        public const int RENDER_TARGET_HEIGHT = 720;
        public const int MAP_CONSOLE_WIDTH = 32;
        public const int MAP_CONSOLE_HEIGHT = 20;
        public const int TILE_SIZE = 32;
        public const int ASCII_SIZE = 12;

        public static GraphicsDevice GraphicsDevice;
        public static ContentManager Content { get; set; }
        public static SpriteBatch SpriteBatch { get; set; }
        public static InputManager InputManager { get; set; }
        public static float ElapsedTime { get; set; }
        public static bool IsAscii { get; set; }
        public static Texture2D AsciiTexture { get; set; }
        public static Texture2D SpriteSheet { get; set; }
        public static SpriteFont Font { get; set; }
        public static Map Map {get; set;}
        public static MapGenerator MapGenerator {get; set;}
        public static Random Rng { get; set; }
        public static AssetManager AssetManager { get; set; }
        public static CombatManager CombatManager { get; set; }

        public static int Rows { get; set; }
        public static int Columns { get; set; }
        public static int AsciiRows { get; set; }
        public static int AsciiColumns { get; set; }

        public static void Update(GameTime gameTime)
        {
            ElapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            InputManager.Update(gameTime);
        }

    }
}
