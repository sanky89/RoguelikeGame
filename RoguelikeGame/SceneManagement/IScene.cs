using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Core.SceneManagement
{
    public interface IScene
    {
        public void Load();
        public void Initialize(SceneManager sceneManager, GraphicsDevice graphicsDevice);
        public void Update(GameTime gameTime);
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime);
    }
}