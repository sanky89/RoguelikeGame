using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace RoguelikeGame
{
    public class Monster : Entity
    {
        public readonly string Name;
        public readonly EntityStats MonsterStats;

        public Monster(Character character, string name, int id, List<StatDataModel> stats) : base(character)
        {
            Id = id;
            Name = name;
            MonsterStats = new EntityStats(stats);
        }
    }
}