using Core.SceneManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace RoguelikeGame
{
    public class RoguelikeGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private List<Node> _path;
        private Texture2D _pathRect;
        private SceneManager _sceneManager;

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
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Globals.SpriteBatch = _spriteBatch;
            Globals.GraphicsDevice = GraphicsDevice;
            Globals.Content = Content;
            Globals.InputManager = new InputManager();
            int seed = Environment.TickCount;
            System.Console.WriteLine("Using Seed: " + seed);
            Globals.Rng = new Random(seed);
            _sceneManager = new SceneManager(Content, GraphicsDevice, _spriteBatch);
            _sceneManager.Initialize();
            _pathRect = new Texture2D(Globals.GraphicsDevice, 1, 1);
            _pathRect.SetData(new Color[] { Color.White });
            base.Initialize();
        }

        protected override void LoadContent()
        {            
            _sceneManager.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            Globals.Update(gameTime);
            _sceneManager.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin();
            _sceneManager.Draw(gameTime);
            // if (_path != null && _mapConsole.ShowDebugOverlay)
            // {
            //     foreach (var node in _path)
            //     {
            //         var location = (_mapConsole.Position - _mapConsole.offset + new Vector2(node.X, node.Y)) * Globals.TILE_SIZE;
            //         _spriteBatch.Draw(_pathRect, new Rectangle(new Point((int)location.X, (int)location.Y), new Point(Globals.TILE_SIZE - 1, Globals.TILE_SIZE - 1)), Color.Yellow);
            //     }
            // }
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}