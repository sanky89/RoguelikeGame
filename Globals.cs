using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace RogueTest
{
    public static class Globals
    {
        public const int SCREEN_WIDTH = 1280;
        public const int SCREEN_HEIGHT = 720;
        public const int RENDER_TARGET_WIDTH = 1280;
        public const int RENDER_TARGET_HEIGHT = 720;
        public const int TILE_SIZE = 12;
        public const float SCALE = 1f;

        public static GraphicsDevice GraphicsDevice;
        public static ContentManager Content { get; set; }
        public static SpriteBatch SpriteBatch { get; set; }
        public static InputManager InputManager { get; set; }
        public static float ElapsedTime { get; set; }
        public static Texture2D GlyphsTexture { get; set; }
        public static SpriteFont Font { get; set; }

        public static int Rows { get; set; }
        public static int Columns { get; set; }

        public static void Update(GameTime gameTime)
        {
            ElapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            InputManager.Update(gameTime);
        }

    }
}
