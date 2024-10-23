using Core.SceneManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace RoguelikeGame
{
    public class RoguelikeGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private GameRoot _gameRoot;

        public RoguelikeGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = GameConstants.SCREEN_WIDTH;
            _graphics.PreferredBackBufferHeight = GameConstants.SCREEN_HEIGHT;
            IsFixedTimeStep = true;
            //SetFullScreen(true);
            _graphics.ApplyChanges();
            _gameRoot = new GameRoot(this);
            base.Initialize();
        }

        protected override void LoadContent()
        {            
            _gameRoot.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            _gameRoot.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _gameRoot.Draw(gameTime);

            base.Draw(gameTime);
        }
    }
}