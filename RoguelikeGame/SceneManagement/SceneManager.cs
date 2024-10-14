using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Core.SceneManagement
{
    public class SceneManager
    {
        private List<IScene> _scenes;
        private ContentManager _content;
        private GraphicsDevice _graphics;
        private SpriteBatch _batch;
        private int _activeSceneIndex = 0;

        public IScene ActiveScene {get; private set;}

        public SceneManager(ContentManager content, GraphicsDevice graphics, SpriteBatch batch)
        {
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
            ActiveScene.Load();
            ActiveScene.Initialize(this,_graphics);
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
            ActiveScene.Load();
            ActiveScene.Initialize(this, _graphics);
        }

        public void LoadContent()
        {
            ActiveScene.Load();
        }

        public void Initialize()
        {
            ActiveScene.Initialize(this, _graphics);
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