using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using RoguelikeGame;

namespace Core.SceneManagement
{
    public class GameScene : IScene
    {
        private GameRoot _gameRoot;
        private Player _player;
        private MapConsole _mapConsole;
        private StatsConsole _statsConsole;
        private InventoryConsole _inventoryConsole;
        private MapConfiguration _mapConfig;
        private Texture2D _asciiTexture;
        private Texture2D _spriteSheet;
        private CombatManager _combatManager;
        private ActionLog _actionLog;
        private bool _showMap = false;
        private int _turns = 0;

        public void Load()
        {
            if(_gameRoot.SceneManager.Content == null)
            {
                System.Console.WriteLine("NULL");
                return;
            }
            _asciiTexture = _gameRoot.SceneManager.Content.Load<Texture2D>("fontsheet");
            _spriteSheet = _gameRoot.SceneManager.Content.Load<Texture2D>("spritesheet");
            _gameRoot.Rows = _spriteSheet.Width / GameConstants.TILE_SIZE;
            _gameRoot.Columns = _spriteSheet.Height / GameConstants.TILE_SIZE;
            _gameRoot.AsciiRows = _asciiTexture.Width / GameConstants.ASCII_SIZE;
            _gameRoot.AsciiColumns = _asciiTexture.Height / GameConstants.ASCII_SIZE;
        

            _combatManager = _gameRoot.CombatManager;
            _player = _gameRoot.AssetManager.CreatePlayer();
            _actionLog = new ActionLog();
            string mapConfig = "Data/random_map_config";
            //string mapConfig = "Data/test_room";
            _mapConfig = _gameRoot.SceneManager.Content.Load<MapConfiguration>(mapConfig);
            _gameRoot.Map = _gameRoot.MapGenerator.GenerateMap(_gameRoot, _mapConfig, _player);
            
            _mapConsole = new MapConsole(_gameRoot, _spriteSheet, _asciiTexture, "", GameConstants.MAP_CONSOLE_WIDTH, GameConstants.MAP_CONSOLE_HEIGHT, ConsoleLocation.TopLeft, BorderStyle.None, Color.Green);
            _statsConsole = new StatsConsole(_gameRoot, _asciiTexture, " Stats", 20, GameConstants.SCREEN_HEIGHT/ GameConstants.ASCII_SIZE/2, ConsoleLocation.TopRight, BorderStyle.DoubleLine, Color.Yellow);
            _inventoryConsole = new InventoryConsole(_gameRoot, _asciiTexture, " Inventory", 20, GameConstants.SCREEN_HEIGHT/ GameConstants.ASCII_SIZE/2 - 2, ConsoleLocation.BottomRight, BorderStyle.DoubleLine, Color.Yellow);
        
        }

        public void Initialize(GameRoot gameRoot, GraphicsDevice graphics)
        {
            _gameRoot = gameRoot;
            _gameRoot.InputManager.KeyPressed += HandleAction;
        }

        public void Update(GameTime gameTime)
        {
            if (_gameRoot.InputManager.IsKeyReleased(Keys.P))
            {
                _gameRoot.SceneManager.SwitchToPreviousScene();
            }
                        //Debug actions
            if (_gameRoot.InputManager.IsKeyReleased(Keys.M))
            {
                _gameRoot.Map = _gameRoot.MapGenerator.GenerateMap(_gameRoot, _mapConfig, _player, true);
            }
            if (_gameRoot.InputManager.IsKeyReleased(Keys.OemTilde))
            {
                _showMap = !_showMap;
                _gameRoot.Map.ToggleMapVisible(_showMap);
            }

            if (_gameRoot.InputManager.IsKeyReleased(Keys.D0))
            {
                _mapConsole.ShowDebugOverlay = !_mapConsole.ShowDebugOverlay;
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            _mapConsole.Draw(spriteBatch);
            _statsConsole.Draw(spriteBatch);
            _inventoryConsole.Draw(spriteBatch);
            _gameRoot.SceneManager.Batch.DrawString(_gameRoot.Font, _gameRoot.ActionLog.LogString, new Vector2(10f, GameConstants.RENDER_TARGET_HEIGHT - 100f), Color.White);

        }

        private void HandleAction(InputAction inputAction)
        {
            switch (inputAction)
            {
                case InputAction.ESCAPE:
                    _gameRoot.SceneManager.SwitchToPreviousScene();
                    break;
                case InputAction.MOVE_LEFT:
                case InputAction.MOVE_RIGHT:
                case InputAction.MOVE_UP:
                case InputAction.MOVE_DOWN:
                case InputAction.MOVE_NW:
                case InputAction.MOVE_NE:
                case InputAction.MOVE_SW:
                case InputAction.MOVE_SE:
                case InputAction.REST:
                case InputAction.USE_ITEM_1:
                case InputAction.USE_ITEM_2:
                case InputAction.USE_ITEM_3:
                case InputAction.USE_ITEM_4:
                case InputAction.USE_ITEM_5:
                    PerformTurn(inputAction);
                    break;
                default:
                    break;
            }
        }

        private void PerformTurn(InputAction inputAction)
        {
            var actionResult = _player.PerformAction(inputAction);
            foreach(var monster in _gameRoot.Map.Monsters)
            {
                monster.PerformAction();    
            }
            switch (actionResult.ResultType)
            {
                case ActionResultType.Move:
                    _mapConsole.CheckScrollMap(inputAction);
                    break;
                case ActionResultType.HitWall:
                    _gameRoot.ActionLog.AddLog("You Hit a Wall");
                    break;
                case ActionResultType.HitEntity:
                    if(actionResult.Entity is Monster m)
                    {
                        _gameRoot.CombatManager.ResolveCombat(_player, m, out var log);
                        if(!string.IsNullOrEmpty(log))
                        {
                            _gameRoot.ActionLog.AddLog(log);
                        }
                    }
                    break;
                case ActionResultType.CollectItem:
                    _mapConsole.CheckScrollMap(inputAction);
                    if(actionResult.Entity is Item item)
                    {
                        _gameRoot.ActionLog.AddLog($"You collected {item.Amount} {item.Name}");
                        Item.RaiseItemPickup(item);
                    }
                    break;
                case ActionResultType.Rest:
                    System.Console.WriteLine("Resting");
                    
                    break;
                default:
                    break;
            }
            _turns++;
        }
    }
}