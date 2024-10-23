using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RoguelikeGame;

namespace Core.SceneManagement
{
    public class SceneManager
    {
        private List<IScene> _scenes;
        private ContentManager _content;
        private GraphicsDevice _graphics;
        private SpriteBatch _batch;
        private int _activeSceneIndex = 0;
        private ContentManager content;
        private GraphicsDevice graphicsDevice;
        private GameRoot _gameRoot;

        public IScene ActiveScene {get; private set;}
        public ContentManager Content => _content;
        public GraphicsDevice Graphics => _graphics;
        public SpriteBatch Batch => _batch;

        public SceneManager(GameRoot gameRoot, ContentManager content, GraphicsDevice graphics, SpriteBatch batch)
        {
            _gameRoot = gameRoot;
            _content = content;
            _graphics = graphics;
            _batch = batch;
            _scenes = new()
            {
                new MainMenuScene(),
                new GameScene()
            };
            ActiveScene = _scenes[_activeSceneIndex];
        }

        public void SwitchToNextScene()
        {
            if(ActiveScene == null)
            {
                return;
            }
            if(_activeSceneIndex == _scenes.Count-1)
            {
                return;
            }
            ActiveScene = _scenes[++_activeSceneIndex];
            ActiveScene.Initialize(_gameRoot,_graphics);
            ActiveScene.Load();
        }

        public void SwitchToPreviousScene()
        {
            if(ActiveScene == null)
            {
                return;
            }
            if(_activeSceneIndex == 0)
            {
                return;
            }

            ActiveScene = _scenes[--_activeSceneIndex];
            ActiveScene.Initialize(_gameRoot, _graphics);
            ActiveScene.Load();
        }

        public void LoadContent()
        {
            ActiveScene.Load();
        }

        public void Initialize()
        {
            if(ActiveScene == null)
            {
                System.Console.WriteLine("Active Scene is null");
                return;
            }
            ActiveScene.Initialize(_gameRoot, _graphics);
        }

        public void Update(GameTime gameTime)
        {
            
            ActiveScene.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            ActiveScene.Draw(_batch, gameTime);
        }
    }
}