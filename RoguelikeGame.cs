using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RoguelikeGame
{
    public class RoguelikeGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Player _player;
        private SpriteFont _font;
        private Console _inventoryConsole;
        private MapConsole _mapConsole;
        private bool _showMap = false;

        public RoguelikeGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = Globals.SCREEN_WIDTH;
            _graphics.PreferredBackBufferHeight = Globals.SCREEN_HEIGHT;
            IsFixedTimeStep = true;
            //SetFullScreen(true);
            _graphics.ApplyChanges();
            Globals.GraphicsDevice = GraphicsDevice;
            Globals.Content = Content;
            Globals.InputManager = new InputManager();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Globals.SpriteBatch = _spriteBatch;
            Globals.GlyphsTexture = Content.Load<Texture2D>("fontsheet");
            Globals.Font = Content.Load<SpriteFont>("rogue_font");
            Globals.Rows = Globals.GlyphsTexture.Width / Globals.TILE_SIZE;
            Globals.Columns = Globals.GlyphsTexture.Height / Globals.TILE_SIZE;

            _mapConsole = new MapConsole("Map", 80, 50, ConsoleLocation.TopLeft, BorderStyle.SingleLine, Color.Green);
            //_map = new Map(Vector2.Zero);
            _player = new Player(new Character(Glyphs.Face1, Color.Yellow), Vector2.Zero, _mapConsole.Map);
            //_inventoryConsole = new Console("Inventory", 20, 38, ConsoleLocation.TopRight, BorderStyle.SingleLine, Color.Yellow);
        }

        protected override void Update(GameTime gameTime)
        {
            Globals.Update(gameTime);
            if(Globals.InputManager.CheckAction(InputAction.ESCAPE))
            {
                Exit();
            }
            if(Globals.InputManager.IsKeyReleased(Keys.M))
            {
                _mapConsole.Map.RegenerateMap();
            }
            if (Globals.InputManager.IsKeyReleased(Keys.Space))
            {
                _mapConsole.Map.ScrollMap(Direction.LEFT);
            }
            if (Globals.InputManager.IsKeyReleased(Keys.OemTilde))
            {
                _showMap = !_showMap;
                _mapConsole.Map.ToggleMapVisible(_showMap);
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();
            _mapConsole.Draw();
            //_map.Draw();
            //_inventoryConsole.Draw();
            //_spriteBatch.DrawString(_font, "Test", new Vector2(600, 20), Color.Red);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}