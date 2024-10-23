using System;
using System.Collections.Generic;
using System.Linq;
using Color = Microsoft.Xna.Framework.Color;

namespace RoguelikeGame
{
    public class AssetManager
    {
        private List<CharacterDataModel> _characterModels;
        private List<MonsterDataModel> _monsterModels;
        private List<ItemDataModel> _itemModels;
        private GameRoot _gameRoot;


        public AssetManager(GameRoot gameRoot, CharactersDataModel charactersData, MonstersDataModel monstersData, ItemsDataModel itemsData)
        {
            _gameRoot = gameRoot;
            _characterModels = new List<CharacterDataModel>(charactersData.characters);
            _monsterModels = new List<MonsterDataModel>(monstersData.monsters);
            _itemModels = new List<ItemDataModel>(itemsData.items);
        }

        public Player CreatePlayer()
        {
            var playerData = _characterModels[3];
            Stats statsDict = CreateStatsDict(playerData.stats);
            var player = new Player(_gameRoot, new Character((Glyphs)playerData.glyph, Color.Yellow, playerData.row, playerData.col), statsDict);
            return player;
        }

        public Monster CreateRandomMonster(int id)
        {
            var monsterData = _monsterModels[_gameRoot.Rng.Next(0, _monsterModels.Count)];
            Stats statsDict = CreateStatsDict(monsterData.stats);
            var character = new Character((Glyphs)monsterData.glyph, Color.White, monsterData.row, monsterData.col);
            return new Monster(_gameRoot, character, monsterData.name, id, statsDict);
        }

        public Monster CreateMonster(string id)
        {
            var monsterData = _monsterModels.FirstOrDefault<MonsterDataModel>(x => x.id == id);
            if (monsterData == null)
            {
                return null;
            }
            Stats statsDict = CreateStatsDict(monsterData.stats);
            return new Monster(_gameRoot, new Character((Glyphs)monsterData.glyph, Color.White, monsterData.row, monsterData.col), monsterData.name, 0, statsDict);
        }

        private Stats CreateStatsDict(List<StatDataModel> statsData)
        {
            Stats result = new Stats();
            foreach (var s in statsData)
            {
                result.Add(s.name, new Stat
                {
                    Name = s.name,
                    MinValue = s.minValue,
                    MaxValue = s.maxValue,
                    CurrentValue = s.defaultValue,
                    Display = s.display,
                });
            }
            return result;
        }

        public Item CreateRandomItem()
        {
            var itemData = _itemModels[_gameRoot.Rng.Next(0, _itemModels.Count)];
            var character = new Character((Glyphs)itemData.glyph, Color.Green, itemData.row, itemData.col);
            return new Item(_gameRoot, character, itemData.name, new Tuple<int, int>(itemData.minAmount, itemData.maxAmount), itemData.affectedStat, itemData.amountAffected);
        }

        public Item CreateItem(string id)
        {
            var itemData = _itemModels.FirstOrDefault<ItemDataModel>(x => x.id == id);
            if(itemData == null)
            {
                return null;
            }
            var character = new Character((Glyphs)itemData.glyph, Color.Green, itemData.row, itemData.col);
            return new Item(_gameRoot, character, itemData.name, new Tuple<int, int>(itemData.minAmount, itemData.maxAmount), itemData.affectedStat, itemData.amountAffected);
        }
    }

    public class CharactersDataModel
    {
        public List<CharacterDataModel> characters { get; set; }
    }

    public class MonstersDataModel
    {
        public List<MonsterDataModel> monsters { get; set; }
    }

    public class ItemsDataModel
    {
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
        public string affectedStat { get; set; }
        public int amountAffected { get; set; }
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
