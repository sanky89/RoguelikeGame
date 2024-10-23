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
        public readonly int AmountAffected;
        public readonly Tuple<int, int> AmountRange;

        public static event Action<Item> OnPickup;
        public static event Action<Item> OnUse;

        public Item(GameRoot gameRoot, Character character, string name, Tuple<int, int> amountRange, string affectedStat, int amountAffected) : base(gameRoot, character)
        {
            Name = name;
            AmountRange = amountRange;
            Amount = _gameRoot.Rng.Next(AmountRange.Item1, AmountRange.Item2);
            AffectedStat = affectedStat;
            AmountAffected = amountAffected;
        }

        public static void RaiseItemPickup(Item item)
        {
            if(OnPickup != null)
            {
                OnPickup(item);
            }
        }

        public static void RaiseItemUse(Item item)
        {
            if (OnUse != null)
            {
                OnUse(item);
            }
        }

    }
}