using System;
using System.Collections.Generic;

namespace RoguelikeGame
{
    public class InventoryItem
    {
        public readonly Item Item;
        private int _amount;

        public int Amount => _amount;

        public InventoryItem(Item item, int amount)
        {
            Item = item;
            _amount = amount;
        }

        public void AddAmount(int amount)
        {
            _amount += amount;
            if(_amount >= Inventory.MaxAmount)
            {
                _amount = Inventory.MaxAmount;
            }
        }
    }

    public class Inventory
    {
        private Dictionary<string, InventoryItem> _inventory;

        public Dictionary<string, InventoryItem> Data => _inventory;

        public Inventory()
        {
            _inventory = new Dictionary<string, InventoryItem>();
            Item.OnPickup += HandleItemPickedUp;
        }

        private void HandleItemPickedUp(Item item)
        {
            if(!string.IsNullOrEmpty(item.AffectedStat))
            {
               AddItem(item, item.Amount);
            }
        }

        public static int MaxAmount => 20;

        public void AddItem(Item item, int count)
        {
            var key = item.Name;
            if (_inventory.ContainsKey(key))
            {
                var current = _inventory[key];
                current.AddAmount(count);
            }
            else
            {
                _inventory[key] = new InventoryItem(item, count);
            }
        }
    }
}
