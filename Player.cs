using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RoguelikeGame
{
    public class Player : Entity
    {
        public const int FOV_SIZE = 20;
        public int X => (int)_position.X;
        public int Y => (int)_position.Y;
        public int MapX { get; private set; }
        public int MapY { get; private set; }

        private Map _map;
        private Fov _fov;

        public Player(Character character, Vector2 position, Map map) : base( character, position)
        {
            _map = map;
            _fov = new Fov(_map);

            _map.SetPlayer(this);
            Globals.InputManager.KeyPressed += HandleKeyPressed;
        }

        public void SetInitialMapPosition(Vector2 pos, int x, int y)
        {
            _position.X = pos.X;
            _position.Y = pos.Y;
            MapX = x;
            MapY = y;
            System.Console.WriteLine($"Position: {pos.X}, {pos.Y} Map Index: {MapX}, {MapY}");
            _fov.UpdateFov(MapX, MapY);
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

           // System.Console.WriteLine($"{(int)_position.X + _map.colStartIndex}");
            if (_map.CanMove((int)(_position.X + dx), (int)(_position.Y + dy)))
            {
                MapX += dx;
                MapY += dy;
                _fov.UpdateFov(MapX, MapY);
                //Fov = new Rectangle(MapX - FOV_SIZE / 2, MapY - FOV_SIZE / 2, FOV_SIZE, FOV_SIZE);
                if (dx > 0 && (_map.ViewportWidth - (int)_position.X) <= 5 &&
                    (int)_position.X + _map.colStartIndex <= Map.COLS - 5)
                {
                    dx = 0;
                    _map.ScrollMap(Direction.LEFT);
                }
                else if (dx < 0 && (int)_position.X <= 5 && _map.colStartIndex > 0)
                {
                    dx = 0;
                    _map.ScrollMap(Direction.RIGHT);
                }
                else if (dy > 0 && (_map.ViewportHeight - (int)_position.Y) <= 5 &&
                    (int)_position.Y + _map.rowStartIndex <= Map.ROWS - 5)
                {
                    dy = 0;
                    _map.ScrollMap(Direction.UP);
                }
                else if (dy < 0 && (int)_position.Y <= 5 && _map.rowStartIndex > 0)
                {
                    dy = 0;
                    _map.ScrollMap(Direction.DOWN);
                }
                _position.X += dx;
                _position.Y += dy;
                System.Console.WriteLine($"Position: {_position.X}, {_position.Y} Map Index: {MapX}, {MapY}");
            }
        }

        public override void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
            if(X < _map.ViewportWidth && Y < _map.ViewportHeight)
            {
                base.Draw(spriteBatch, texture);
            }
        }
    }
}
