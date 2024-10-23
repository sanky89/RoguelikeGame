using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        private GameRoot _gameRoot;

        public InventoryConsole(GameRoot gameRoot, Texture2D asciiTexture, string title, int width, int height, ConsoleLocation location, BorderStyle border = BorderStyle.None, Color borderColor = default) : base(gameRoot, asciiTexture, title, width, height, location, border, borderColor)
        {
            _gameRoot = gameRoot;
            _inventory = gameRoot.Inventory;
            _sb = new StringBuilder();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
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
            base.Draw(spriteBatch);

            _gameRoot.SpriteBatch.DrawString(_gameRoot.Font, _inventoryString,
            new Vector2((_x + 2) * GameConstants.ASCII_SIZE, (_y + 2) * GameConstants.ASCII_SIZE), Color.White);
        }
    }
}
