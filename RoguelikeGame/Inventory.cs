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

        public void SubtractAmount(int amount)
        {
            _amount -= amount;
            if (_amount < 0)
            {
                _amount = 0;
            }
        }
    }

    public class Inventory
    {
        private Dictionary<string, InventoryItem> _inventory;
        private Dictionary<int, string> _actionsIventoryItemsMap;
        public Dictionary<string, InventoryItem> Data => _inventory;
        public static int MaxAmount => 20;
        private static int Count = 0;

        public Inventory()
        {
            _inventory = new Dictionary<string, InventoryItem>();
            _actionsIventoryItemsMap = new Dictionary<int, string>();
            Item.OnPickup += HandleItemPickedUp;
        }

        private void HandleItemPickedUp(Item item)
        {
            if(!string.IsNullOrEmpty(item.AffectedStat))
            {
               AddItem(item, item.Amount);
            }
        }


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
                Count++;
                _inventory[key] = new InventoryItem(item, count);
                _actionsIventoryItemsMap[Count] = key;
            }
        }

        public void UseItem(int inputAction)
        {
            if(!_actionsIventoryItemsMap.TryGetValue(inputAction, out var itemName))
            {
                return;
            }
            var inventoryItem = _inventory[itemName];
            Item.RaiseItemUse(inventoryItem.Item);
            inventoryItem.SubtractAmount(1);
            if(inventoryItem.Amount <= 0)
            {
                _inventory.Remove(itemName);
                _actionsIventoryItemsMap.Remove(inputAction);
            }

        }
    }
}
