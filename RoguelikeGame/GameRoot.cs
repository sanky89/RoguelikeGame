using Core.SceneManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;


namespace RoguelikeGame
{
    public class GameRoot
    {
        private bool _isAscii;
        private SpriteFont _font;
        private MapGenerator _mapGenerator;
        private AssetManager _assetManager;
        //private List<Node> _path;
        //private Texture2D _pathRect;
        private SceneManager _sceneManager;

        public readonly Inventory Inventory;
        public readonly Random Rng;
        public readonly InputManager InputManager;
        public readonly SpriteBatch SpriteBatch;
        public readonly ActionLog ActionLog;
        public readonly CombatManager CombatManager;

        public MapGenerator MapGenerator => _mapGenerator;
        public Map Map {get;set;}
        public AssetManager AssetManager => _assetManager;
        public SceneManager SceneManager => _sceneManager;
        public SpriteFont Font => _font;
        public bool IsAscii => _isAscii;

        public int Rows { get; set; }
        public int Columns { get; set; }
        public int AsciiRows { get; set; }
        public int AsciiColumns { get; set; }

        public GameRoot(RoguelikeGame game)
        {
            _isAscii = false;
            int seed = Environment.TickCount;
            System.Console.WriteLine("Using Seed: " + seed);
            Rng = new Random(seed);

            InputManager = new();
            SpriteBatch = new SpriteBatch(game.GraphicsDevice);
            _mapGenerator = new();
            Inventory = new();
            ActionLog = new();
            CombatManager = new(this);
            _sceneManager = new SceneManager(this, game.Content, game.GraphicsDevice, SpriteBatch);
            _sceneManager.Initialize();
            //_pathRect = new Texture2D(game.GraphicsDevice, 1, 1);
            //_pathRect.SetData(new Color[] { Color.White });
        }

        public void LoadContent()
        {
            _font = _sceneManager.Content.Load<SpriteFont>("rogue_font");
            var charactersData = _sceneManager.Content.Load<CharactersDataModel>("Data/characters");
            var monstersData = _sceneManager.Content.Load<MonstersDataModel>("Data/monsters");
            var itemsData = _sceneManager.Content.Load<ItemsDataModel>("Data/items");
            _assetManager = new AssetManager(this, charactersData, monstersData, itemsData);
            _sceneManager.LoadContent();
        }

        public void Update(GameTime gameTime)
        {
            InputManager.Update(gameTime);
            _sceneManager.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin();
            _sceneManager.Draw(gameTime);
            SpriteBatch.End();
        }
    }
}
