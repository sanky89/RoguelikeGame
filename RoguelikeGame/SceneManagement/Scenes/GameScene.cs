using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using RoguelikeGame;

namespace Core.SceneManagement
{
    public class GameScene : IScene
    {
        private SceneManager _sceneManager;
        private Player _player;
        private MapConsole _mapConsole;
        private StatsConsole _statsConsole;
        private InventoryConsole _inventoryConsole;
        private MapConfiguration _mapConfig;
        private bool _showMap = false;
        private int _turns = 0;

        public void Load()
        {
            if(_sceneManager.Content == null)
            {
                System.Console.WriteLine("NULL");
                return;
            }
            Globals.AsciiTexture = _sceneManager.Content.Load<Texture2D>("fontsheet");
            Globals.SpriteSheet = _sceneManager.Content.Load<Texture2D>("spritesheet");
            //Globals.Font = _sceneManager.Content.Load<SpriteFont>("rogue_font");
            Globals.Rows = Globals.SpriteSheet.Width / Globals.TILE_SIZE;
            Globals.Columns = Globals.SpriteSheet.Height / Globals.TILE_SIZE;
            Globals.AsciiRows = Globals.AsciiTexture.Width / Globals.ASCII_SIZE;
            Globals.AsciiColumns = Globals.AsciiTexture.Height / Globals.ASCII_SIZE;
            Globals.IsAscii = false;
        
            var charactersData = _sceneManager.Content.Load<CharactersDataModel>("Data/characters");
            var monstersData = _sceneManager.Content.Load<MonstersDataModel>("Data/monsters");
            var itemsData = _sceneManager.Content.Load<ItemsDataModel>("Data/items");
            Globals.AssetManager = new AssetManager(charactersData, monstersData, itemsData);
            Globals.CombatManager = new CombatManager();
            _player = Globals.AssetManager.CreatePlayer();
            Globals.ActionLog = new ActionLog();
            Globals.MapGenerator = new();
            string mapConfig = "Data/random_map_config";
            //string mapConfig = "Data/test_room";
            _mapConfig = _sceneManager.Content.Load<MapConfiguration>(mapConfig);
            Globals.Map = Globals.MapGenerator.GenerateMap(_mapConfig, _player);
            Globals.Inventory = new();
            
            _mapConsole = new MapConsole( "", Globals.MAP_CONSOLE_WIDTH, Globals.MAP_CONSOLE_HEIGHT, ConsoleLocation.TopLeft, BorderStyle.None, Color.Green);
            _statsConsole = new StatsConsole( " Stats", 20, Globals.SCREEN_HEIGHT/Globals.ASCII_SIZE/2, ConsoleLocation.TopRight, BorderStyle.DoubleLine, Color.Yellow);
            _inventoryConsole = new InventoryConsole(Globals.Inventory, " Inventory", 20, Globals.SCREEN_HEIGHT/Globals.ASCII_SIZE/2 - 2, ConsoleLocation.BottomRight, BorderStyle.DoubleLine, Color.Yellow);
        
        }

        public void Initialize(SceneManager sceneManager, GraphicsDevice graphics)
        {
            _sceneManager = sceneManager;
            Globals.InputManager.KeyPressed += HandleAction;
        }

        public void Update(GameTime gameTime)
        {
            if (Globals.InputManager.IsKeyReleased(Keys.P))
            {
                _sceneManager.SwitchToPreviousScene();
            }
                        //Debug actions
            if (Globals.InputManager.IsKeyReleased(Keys.M))
            {
                Globals.Map = Globals.MapGenerator.GenerateMap(_mapConfig, _player, true);
            }
            if (Globals.InputManager.IsKeyReleased(Keys.OemTilde))
            {
                _showMap = !_showMap;
                Globals.Map.ToggleMapVisible(_showMap);
            }

            if (Globals.InputManager.IsKeyReleased(Keys.D0))
            {
                _mapConsole.ShowDebugOverlay = !_mapConsole.ShowDebugOverlay;
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            _mapConsole.Draw();
            _statsConsole.Draw();
            _inventoryConsole.Draw();
            _sceneManager.Batch.DrawString(Globals.Font, Globals.ActionLog.LogString, new Vector2(10f, Globals.RENDER_TARGET_HEIGHT - 100f), Color.White);

        }

        private void HandleAction(InputAction inputAction)
        {
            switch (inputAction)
            {
                case InputAction.ESCAPE:
                    _sceneManager.SwitchToPreviousScene();
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
            foreach(var monster in Globals.Map.Monsters)
            {
                monster.PerformAction();    
            }
            switch (actionResult.ResultType)
            {
                case ActionResultType.Move:
                    _mapConsole.CheckScrollMap(inputAction);
                    break;
                case ActionResultType.HitWall:
                    Globals.ActionLog.AddLog("You Hit a Wall");
                    break;
                case ActionResultType.HitEntity:
                    if(actionResult.Entity is Monster m)
                    {
                        Globals.CombatManager.ResolveCombat(_player, m, out var log);
                        if(!string.IsNullOrEmpty(log))
                        {
                            Globals.ActionLog.AddLog(log);
                        }
                    }
                    break;
                case ActionResultType.CollectItem:
                    _mapConsole.CheckScrollMap(inputAction);
                    if(actionResult.Entity is Item item)
                    {
                        Globals.ActionLog.AddLog($"You collected {item.Amount} {item.Name}");
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