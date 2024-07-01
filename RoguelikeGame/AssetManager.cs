using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Microsoft.Xna.Framework.Content;
using Color = Microsoft.Xna.Framework.Color;

namespace RoguelikeGame
{
    public class AssetManager
    {
        private List<CharacterDataModel> _characterModels;
        private List<MonsterDataModel> _monsterModels;
        private List<ItemDataModel> _itemModels;
        public  AssetManager(GameDataModel gameData)
        {
            _characterModels = new List<CharacterDataModel>(gameData.characters);
            _monsterModels = new List<MonsterDataModel>(gameData.monsters);
            _itemModels = new List<ItemDataModel>(gameData.items);
        }

        public Player CreatePlayer()
        {
            var playerData = _characterModels[3];
            var player = new Player(new Character((Glyphs)playerData.glyph, Color.Yellow, playerData.row, playerData.col), playerData.stats);
            return player;
        }

        public Monster CreateRandomMonster(int id)
        {
            var monsterData = _monsterModels[Globals.Rng.Next(0, _monsterModels.Count)];
            var character = new Character((Glyphs)monsterData.glyph, Color.White, monsterData.row, monsterData.col);
            return new Monster(character, monsterData.name, id, monsterData.stats);
        }

        public Monster CreateMonster(string id)
        {
            var monsterData = _monsterModels.FirstOrDefault<MonsterDataModel>(x => x.id == id);
            if (monsterData == null)
            {
                return null;
            }
            return new Monster(new Character((Glyphs)monsterData.glyph, Color.White, monsterData.row, monsterData.col), monsterData.name, 0, monsterData.stats);
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
        public List<StatDataModel> stats { get; set; }
    }

    public class MonsterDataModel : CharacterDataModel
    {
    }

    public class ItemDataModel : EntityDataModel
    {
        public int minAmount { get; set; }
        public int maxAmount { get; set; }
    }

    public class StatDataModel
    {
        public string name { get; set; }
        public int minValue { get; set; }
        public int maxValue { get; set; }
        public int defaultValue { get; set; }
        public bool display { get; set; }
        
    }
}
