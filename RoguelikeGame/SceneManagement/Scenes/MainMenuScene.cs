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
        private GameRoot _gameRoot;
        private Tuple<string,Rectangle> _button;

        public void Load()
        {
            
        }

        public void Initialize(GameRoot gameRoot, GraphicsDevice graphics)
        {
            _gameRoot = gameRoot;
            _sceneManager = gameRoot.SceneManager;
            _texture = new Texture2D(graphics, 1, 1);
            _texture.SetData(new Color[] { Color.White });
            _button = new Tuple<string, Rectangle>("New Game", new Rectangle(100,100,100,50));
        }

        public void Update(GameTime gameTime)
        {            
            if (_gameRoot.InputManager.IsKeyReleased(Keys.Enter))
            {
                System.Console.WriteLine("Next Scene");
                _sceneManager.SwitchToNextScene();
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            //System.Console.WriteLine("Main menu Draw");
            spriteBatch.Draw(_texture, _button.Item2, Color.White);
            spriteBatch.DrawString(_gameRoot.Font, _button.Item1, new Vector2(_button.Item2.Left + 5.0f, _button.Item2.Top + 5.0f), Color.Black);
        }
    }
}