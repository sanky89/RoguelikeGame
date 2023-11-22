using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RogueTest
{
    public class Player : Entity
    {
        public int X => (int)_position.X; 
        public int Y => (int)_position.Y;

        private Map _map;

        public Player(Character character, Vector2 position, Map map) : base( character, position)
        {
            _map = map;
            _map.SetPlayer(this);
            Globals.InputManager.KeyPressed += HandleKeyPressed;
        }

        public void SetInitialMapPosition(Vector2 pos)
        {
            _position.X = pos.X;
            _position.Y = pos.Y;
        }

        private void HandleKeyPressed(InputAction action)
        {
            int dx = 0;
            int dy = 0;

            switch (action)
            {
                case InputAction.MOVE_LEFT:
                    dx = -1;
                    break;
                case InputAction.MOVE_RIGHT:
                    dx = 1;
                    break;
                case InputAction.MOVE_UP:
                    dy = -1;
                    break;
                case InputAction.MOVE_DOWN:
                    dy = 1;
                    break;
                default:
                    break;
            }

            if(_map.CanMove((int)(_position.X + dx), (int)(_position.Y + dy)))
            {
                _position.X += dx;
                _position.Y += dy;
            }
            System.Console.WriteLine($"Player Pos: {_position.X}, {_position.Y}");
        }
    }
}
