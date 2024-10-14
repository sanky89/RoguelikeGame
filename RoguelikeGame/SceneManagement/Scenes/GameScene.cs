using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using RoguelikeGame;

namespace Core.SceneManagement
{
    public class GameScene : IScene
    {
        private SceneManager _sceneManager;
        
        public void Load()
        {
            
        }

        public void Initialize(SceneManager sceneManager, GraphicsDevice graphics)
        {
            _sceneManager = sceneManager;
        }

        public void Update(GameTime gameTime)
        {
            if (Globals.InputManager.IsKeyReleased(Keys.P))
            {
                _sceneManager.SwitchToPreviousScene();
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
        
        }
    }
}