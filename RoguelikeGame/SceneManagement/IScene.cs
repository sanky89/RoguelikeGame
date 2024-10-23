using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RoguelikeGame;

namespace Core.SceneManagement
{
    public interface IScene
    {
        public void Load();
        public void Initialize(GameRoot gameRoot, GraphicsDevice graphicsDevice);
        public void Update(GameTime gameTime);
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime);
    }
}