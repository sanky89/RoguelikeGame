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
        public const int NumSlots = 5;
        public const int MaxAmount = 20;
        public Dictionary<string, InventoryItem> Data => _inventory;
        private Dictionary<string, InventoryItem> _inventory;
        private InventoryItem[] _slots = new InventoryItem[NumSlots];
        private int _count = 0;

        public Inventory()
        {
            _inventory = new Dictionary<string, InventoryItem>();
            Item.OnPickup += HandleItemPickedUp;
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
                _inventory[key] = new InventoryItem(item, count);
                var slot = GetFreeSlot();
                if(slot != -1)
                {
                    _slots[slot] = _inventory[key];
                }
            }
        }

        public void UseItem(int index)
        {
            if(index < 0 || index >= NumSlots)
            {
                return;
            }
            var inventoryItem = _slots[index];
            if(inventoryItem == null)
            {
                return;
            }

            Item.RaiseItemUse(inventoryItem.Item);
            inventoryItem.SubtractAmount(1);
            if(inventoryItem.Amount <= 0)
            {
                _inventory.Remove(inventoryItem.Item.Name);
                _slots[index] = null;
            }
        }

        public bool GetItemInSlot(int index, out InventoryItem item)
        {
            item = null;
            if(_slots[index] != null)
            {
                item = _slots[index];
                return true;
            }

            return false;
        }
        

        private void HandleItemPickedUp(Item item)
        {
            if(!string.IsNullOrEmpty(item.AffectedStat))
            {
               AddItem(item, item.Amount);
            }
        }

        private int GetFreeSlot()
        {
            for(int i=0; i<NumSlots; i++)
            {
                if(_slots[i] == null)
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
