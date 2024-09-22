using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoguelikeGame
{
    public class InventoryConsole : Console
    {
        private Inventory _inventory;
        private StringBuilder _sb;
        private string _inventoryString = "";

        public InventoryConsole(Inventory inventory, string title, int width, int height, ConsoleLocation location, BorderStyle border = BorderStyle.None, Color borderColor = default) : base(title, width, height, location, border, borderColor)
        {
            _inventory = inventory;
            _sb = new StringBuilder();
        }

        public override void Draw()
        {
            base.Draw();
            _inventoryString = "";
            for(int i=0; i < Inventory.NumSlots; i++)
            {
                if(_inventory.GetItemInSlot(i, out var item))
                {
                    _inventoryString += $"[{i+1}] {item.Item.Name} X{item.Amount} \n";                    
                }
                else
                {
                    _inventoryString += $"[{i+1}] <empty>\n";
                }
                //_sb.AppendLine();
            }

            Globals.SpriteBatch.DrawString(Globals.Font, _inventoryString,
            new Vector2((_x + 2) * Globals.ASCII_SIZE, (_y + 2) * Globals.ASCII_SIZE), Color.White);
        }
    }
}
