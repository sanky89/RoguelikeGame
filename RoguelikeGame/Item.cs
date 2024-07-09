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
        public readonly string AffectedStat;
        public readonly Tuple<int, int> AmountRange;

        public Item(Character character, string name, Tuple<int, int> amountRange, string affectedStat) : base(character)
        {
            Name = name;
            AmountRange = amountRange;
            Amount = Globals.Rng.Next(AmountRange.Item1, AmountRange.Item2);
            AffectedStat = affectedStat;
        }

        public void OnPickup()
        {
            if(string.IsNullOrEmpty(AffectedStat))
            {
                return;
            }

            Globals.Map.Player.PlayerStats.UpdateStat(AffectedStat, Amount);
        }
    }
}