using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace RoguelikeGame
{
    public class RoguelikeGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Player _player;
        private MapConsole _mapConsole;
        private bool _showMap = false;
        private int _turns = 0;

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
            Globals.InputManager.KeyPressed += HandleAction;
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

            _player = new Player(new Character(Glyphs.Face1, Color.Yellow), Vector2.Zero);

            var mapConsoleWidth = 80;
            var mapConsoleHeight = 50;
            _mapConsole = new MapConsole( "Map", mapConsoleWidth, mapConsoleHeight, ConsoleLocation.TopLeft, BorderStyle.SingleLine, Color.Green);
            Globals.Map = new Map(_mapConsole.Position + Vector2.One, mapConsoleWidth, mapConsoleHeight, _player);
            Globals.Map.GenerateMap();
        }

        protected override void Update(GameTime gameTime)
        {
            Globals.Update(gameTime);

            //Debug actions
            if (Globals.InputManager.IsKeyReleased(Keys.M))
            {
                _mapConsole.Map.RegenerateMap();
            }
            if (Globals.InputManager.IsKeyReleased(Keys.OemTilde))
            {
                _showMap = !_showMap;
                _mapConsole.Map.ToggleMapVisible(_showMap);
            }
            base.Update(gameTime);
        }

        private void HandleAction(InputAction inputAction)
        {
            switch (inputAction)
            {
                case InputAction.ESCAPE:
                    Exit();
                    break;
                case InputAction.MOVE_LEFT:
                case InputAction.MOVE_RIGHT:
                case InputAction.MOVE_UP:
                case InputAction.MOVE_DOWN:
                    _player.PerformAction(inputAction);
                    PerformTurn();
                    break;
                default:
                    break;
            }
        }

        private void PerformTurn()
        {
            _turns++;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();
            _mapConsole.Draw();
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}