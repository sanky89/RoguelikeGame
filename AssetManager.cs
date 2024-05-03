using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Color = Microsoft.Xna.Framework.Color;

namespace RoguelikeGame
{
    public class AssetManager
    {
        private const string DATA_PATH = "../../../Data/content.json";

        private GameDataModel _gameData;
        private List<CharacterDataModel> _characterModels;
        private List<MonsterDataModel> _monsterModels;
        private List<ItemDataModel> _itemModels;

        public void ReadJsonFile()
        {
            try
            {
                string text = File.ReadAllText(DATA_PATH);
                _gameData = JsonSerializer.Deserialize<GameDataModel>(text);
            }
            catch (Exception e)
            {
                System.Console.WriteLine($"Error reading data file {e.Message}");
            }
            _characterModels = new List<CharacterDataModel>(_gameData.characters);
            _monsterModels = new List<MonsterDataModel>(_gameData.monsters);
            _itemModels = new List<ItemDataModel>(_gameData.items);
        }

        public Player CreatePlayer()
        {
            var playerModel = _characterModels[3];
            var player = new Player(new Character((Glyphs)playerModel.glyph, Color.Yellow, playerModel.row, playerModel.col));
            return player;
        }

        public Monster CreateRandomMonster()
        {
            var monsterData = _monsterModels[Globals.Rng.Next(0, _monsterModels.Count)];
            return new Monster(new Character((Glyphs)monsterData.glyph, Color.White, monsterData.row, monsterData.col), monsterData.name);
        }

        public Monster CreateMonster(string id)
        {
            var monsterData = _monsterModels.FirstOrDefault<MonsterDataModel>(x => x.id == id);
            if (monsterData == null)
            {
                return null;
            }
            return new Monster(new Character((Glyphs)monsterData.glyph, Color.White, monsterData.row, monsterData.col), monsterData.name);
        }

        public Item CreateRandomItem()
        {
            var itemData = _itemModels[Globals.Rng.Next(0, _itemModels.Count)];
            return new Item(new Character((Glyphs)itemData.glyph, Color.Green, itemData.row, itemData.col), itemData.name, new Tuple<int, int>(itemData.minAmount, itemData.maxAmount));
        }

        public Item CreateItem(string id)
        {
            var itemData = _itemModels.FirstOrDefault<ItemDataModel>(x => x.id == id);
            if(itemData == null)
            {
                return null;
            }

            return new Item(new Character((Glyphs)itemData.glyph, Color.Green, itemData.row, itemData.col), itemData.name, new Tuple<int, int>(itemData.minAmount, itemData.maxAmount));
        }
    }

    public class GameDataModel
    {
        public List<CharacterDataModel> characters { get; set; }
        public List<MonsterDataModel> monsters { get; set; }
        public List<ItemDataModel> items { get; set; }
    }

    public abstract class EntityDataModel
    {
        public string id { get; set; }
        public string name { get; set; }
        public int glyph { get; set; }
        public int row { get; set; }
        public int col { get; set; }
    }

    public class CharacterDataModel : EntityDataModel
    {
        public int health { get; set; }
    }

    public class MonsterDataModel : CharacterDataModel
    {
    }

    public class ItemDataModel : EntityDataModel
    {
        public int minAmount { get; set; }
        public int maxAmount { get; set; }
    }
}
