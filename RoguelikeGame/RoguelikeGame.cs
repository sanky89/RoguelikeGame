using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;

namespace RoguelikeGame
{
    public class RoguelikeGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Player _player;
        private MapConsole _mapConsole;
        private StatsConsole _statsConsole;
        private InventoryConsole _inventoryConsole;
        private ActionLog _actionLog;
        private bool _showMap = false;
        private int _turns = 0;
        private List<Node> _path;
        private Texture2D _pathRect;

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
            int seed = Environment.TickCount;
            System.Console.WriteLine("Using Seed: " + seed);
            Globals.Rng = new Random(seed);

            _pathRect = new Texture2D(Globals.GraphicsDevice, 1, 1);
            _pathRect.SetData(new Color[] { Color.White });
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Globals.SpriteBatch = _spriteBatch;
            Globals.AsciiTexture = Content.Load<Texture2D>("fontsheet");
            Globals.SpriteSheet = Content.Load<Texture2D>("spritesheet");
            Globals.Font = Content.Load<SpriteFont>("rogue_font");
            Globals.Rows = Globals.SpriteSheet.Width / Globals.TILE_SIZE;
            Globals.Columns = Globals.SpriteSheet.Height / Globals.TILE_SIZE;
            Globals.AsciiRows = Globals.AsciiTexture.Width / Globals.ASCII_SIZE;
            Globals.AsciiColumns = Globals.AsciiTexture.Height / Globals.ASCII_SIZE;
            Globals.IsAscii = false;
        
            var charactersData = Content.Load<CharactersDataModel>("Data/characters");
            var monstersData = Content.Load<MonstersDataModel>("Data/monsters");
            var itemsData = Content.Load<ItemsDataModel>("Data/items");
            Globals.AssetManager = new AssetManager(charactersData, monstersData, itemsData);
            Globals.CombatManager = new CombatManager();
            _player = Globals.AssetManager.CreatePlayer();
            _actionLog = new ActionLog();
            Globals.MapGenerator = new();
            //string mapConfig = "Data/random_map_config";
            string mapConfig = "Data/test_room";
            MapConfiguration mapConfiguration = Content.Load<MapConfiguration>(mapConfig);
            Globals.Map = Globals.MapGenerator.GenerateMap(mapConfiguration, _player);
            Globals.Map.Pathfinder = new Pathfinder(Globals.Map.Cols, Globals.Map.Rows);
            Globals.Inventory = new();
            
            _mapConsole = new MapConsole( "", Globals.MAP_CONSOLE_WIDTH, Globals.MAP_CONSOLE_HEIGHT, ConsoleLocation.TopLeft, BorderStyle.None, Color.Green);
            _statsConsole = new StatsConsole( " Stats", 20, Globals.SCREEN_HEIGHT/Globals.ASCII_SIZE/2, ConsoleLocation.TopRight, BorderStyle.DoubleLine, Color.Yellow);
            _inventoryConsole = new InventoryConsole(Globals.Inventory, " Inventory", 20, Globals.SCREEN_HEIGHT/Globals.ASCII_SIZE/2 - 2, ConsoleLocation.BottomRight, BorderStyle.DoubleLine, Color.Yellow);
        }

        protected override void Update(GameTime gameTime)
        {
            Globals.Update(gameTime);

            //Debug actions
            if (Globals.InputManager.IsKeyReleased(Keys.M))
            {
                //Globals.Map.RegenerateMap();
            }
            if (Globals.InputManager.IsKeyReleased(Keys.OemTilde))
            {
                _showMap = !_showMap;
                Globals.Map.ToggleMapVisible(_showMap);
            }

            //if (Globals.InputManager.IsKeyReleased(Keys.D1))
            //{
            //    _mapConsole.ShowDebugOverlay = !_mapConsole.ShowDebugOverlay;
            //}

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
            CheckMonstersFov();
            switch (actionResult.ResultType)
            {
                case ActionResultType.Move:
                    _mapConsole.CheckScrollMap(inputAction);
                    break;
                case ActionResultType.HitWall:
                    _actionLog.AddLog("You Hit a Wall");
                    break;
                case ActionResultType.HitEntity:
                    if(actionResult.Entity is Monster m)
                    {
                        Globals.CombatManager.ResolveCombat(_player, m, out var log);
                        if(!string.IsNullOrEmpty(log))
                        {
                            _actionLog.AddLog(log);
                        }
                    }
                    break;
                case ActionResultType.CollectItem:
                    _mapConsole.CheckScrollMap(inputAction);
                    if(actionResult.Entity is Item item)
                    {
                        _actionLog.AddLog($"You collected {item.Amount} {item.Name}");
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

        private void CheckMonstersFov()
        {
            foreach(var monster in Globals.Map.Monsters)
            {
                if(monster.IsPlayerInFov())
                {
                    if(monster.IsPlayerInAttackRange())
                    {
                        System.Console.WriteLine($"player is in {monster.Name}_{monster.Id} fov");
                        Globals.CombatManager.ResolveCombat(_player, monster, out var log, false);
                        _actionLog.AddLog(log);
                        return;
                    }
                    var startNode = new Node(monster.MapX, monster.MapY);
                    var endNode = new Node(_player.MapX, _player.MapY);
                    var path = Globals.Map.Pathfinder.CalculatePath(startNode, endNode);
                    if(path != null && path.Count > 0)
                    {
                        var pathString = "";
                        foreach (var node in path)
                        {
                            pathString += $" ({node.X},{node.Y}) ->";
                        }
                        System.Console.WriteLine(pathString);
                        monster.Move(Globals.Map, path[0].X, path[0].Y);
                    }
                }
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();
            _mapConsole.Draw();
            _statsConsole.Draw();
            _inventoryConsole.Draw();
            _spriteBatch.DrawString(Globals.Font, _actionLog.LogString, new Vector2(10f, Globals.RENDER_TARGET_HEIGHT - 100f), Color.White);

            if (_path != null && _mapConsole.ShowDebugOverlay)
            {
                foreach (var node in _path)
                {
                    var location = (_mapConsole.Position - _mapConsole.offset + new Vector2(node.X, node.Y)) * Globals.TILE_SIZE;
                    _spriteBatch.Draw(_pathRect, new Rectangle(new Point((int)location.X, (int)location.Y), new Point(Globals.TILE_SIZE - 1, Globals.TILE_SIZE - 1)), Color.Yellow);
                }
            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}