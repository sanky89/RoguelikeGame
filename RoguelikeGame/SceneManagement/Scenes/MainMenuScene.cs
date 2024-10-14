using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using RoguelikeGame;

namespace Core.SceneManagement
{
    public class MainMenuScene : IScene
    {
        private Texture2D _texture;
        private SceneManager _sceneManager;

        public void Load()
        {
        }

        public void Initialize(SceneManager sceneManager, GraphicsDevice graphics)
        {
            _sceneManager = sceneManager;
            _texture = new Texture2D(graphics, 1, 1);
            _texture.SetData(new Color[] { Color.White });
        }

        public void Update(GameTime gameTime)
        {
            if (Globals.InputManager.IsKeyReleased(Keys.F))
            {
                _sceneManager.SwitchToNextScene();
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            //System.Console.WriteLine("Main menu Draw");
            spriteBatch.Draw(_texture, new Rectangle(100,100,100,50), Color.Red);
        }
    }
}