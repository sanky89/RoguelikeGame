using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RoguelikeGame
{
    public class Entity
    {

        protected Character _character;
        protected Vector2 _position;

        public Vector2 Position => _position;
        public int Id { get; set; }
        public Color Color => _character.Color;
        public Rectangle SourceRect => _character.GetSourceRect();

        public Entity(Character character, Vector2 position)
        {
            _character = character;
            _position = position;
        }

        public virtual void Update(GameTime gameTime) { }

        public virtual void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
            spriteBatch.Draw(texture, _position * Globals.TILE_SIZE * Globals.SCALE, _character.GetSourceRect(), _character.Color, 0f, Vector2.Zero, Globals.SCALE, SpriteEffects.None, 0);
        }
    }
}
