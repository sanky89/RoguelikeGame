using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace RoguelikeGame
{
    public class Monster : Entity
    {
        public string Name {get; private set;}

        public MonsterType Type {get; private set;}
        public Monster(Character character, string name, MonsterType monsterType) : base(character)
        {
            Name = name;
            Type = monsterType;
        }
    }
}