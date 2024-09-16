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
            foreach(var kvp in _inventory.Data)
            {
                //_sb.AppendLine();
                _inventoryString += kvp.Key + " X" + kvp.Value.Amount + "\n";
            }

            Globals.SpriteBatch.DrawString(Globals.Font, _inventoryString,
            new Vector2((_x + 2) * Globals.ASCII_SIZE, (_y + 2) * Globals.ASCII_SIZE), Color.White);
        }
    }
}
