using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoguelikeGame
{
    public class Item : Entity
    {
        public readonly string Name;
        public readonly int Amount;
        public readonly Tuple<int, int> AmountRange;

        public Item(Character character, string name, Tuple<int, int> amountRange) : base(character)
        {
            Name = name;
            AmountRange = amountRange;
            Amount = Globals.Rng.Next(AmountRange.Item1, AmountRange.Item2);
        }
    }
}