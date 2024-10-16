using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using RoguelikeGame;
using System;

namespace Core.SceneManagement
{
    public class MainMenuScene : IScene
    {
        private Texture2D _texture;
        private SceneManager _sceneManager;
        private Tuple<string,Rectangle> _button;

        public void Load()
        {
            Globals.Font = _sceneManager.Content.Load<SpriteFont>("rogue_font");
        }

        public void Initialize(SceneManager sceneManager, GraphicsDevice graphics)
        {
            _sceneManager = sceneManager;
            _texture = new Texture2D(graphics, 1, 1);
            _texture.SetData(new Color[] { Color.White });
            _button = new Tuple<string, Rectangle>("New Game", new Rectangle(100,100,100,50));
        }

        public void Update(GameTime gameTime)
        {
            if (Globals.InputManager.IsKeyReleased(Keys.Enter))
            {
                _sceneManager.SwitchToNextScene();
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            //System.Console.WriteLine("Main menu Draw");
            spriteBatch.Draw(_texture, _button.Item2, Color.White);
            spriteBatch.DrawString(Globals.Font, _button.Item1, new Vector2(_button.Item2.Left + 5.0f, _button.Item2.Top + 5.0f), Color.Black);
        }
    }
}