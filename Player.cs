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

        private Fov _fov;

        public Player(Character character, Vector2 position) : base( character, position)
        {
        }

        public void SetInitialMapPosition(Vector2 pos, int x, int y)
        {
            _position.X = pos.X;
            _position.Y = pos.Y;
            MapX = x;
            MapY = y;
            System.Console.WriteLine($"Position: {pos.X}, {pos.Y} Map Index: {MapX}, {MapY}");
            _fov = new Fov(Globals.Map);
            _fov.UpdateFov(MapX, MapY);
        }

        public void PerformAction(InputAction action)
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

           // System.Console.WriteLine($"{(int)_position.X + Globals.Map.colStartIndex}");
            if (Globals.Map.CanMove((int)(_position.X + dx), (int)(_position.Y + dy)))
            {
                MapX += dx;
                MapY += dy;
                _fov.UpdateFov(MapX, MapY);

                if (dx > 0 && (Globals.Map.ViewportWidth - (int)_position.X) <= 5 &&
                    (int)_position.X + Globals.Map.ColStartIndex <= Map.COLS - 5)
                {
                    dx = 0;
                    Globals.Map.ScrollMap(Direction.LEFT);
                }
                else if (dx < 0 && (int)_position.X <= 5 && Globals.Map.ColStartIndex > 0)
                {
                    dx = 0;
                    Globals.Map.ScrollMap(Direction.RIGHT);
                }
                else if (dy > 0 && (Globals.Map.ViewportHeight - (int)_position.Y) <= 5 &&
                    (int)_position.Y + Globals.Map.RowStartIndex <= Map.ROWS - 5)
                {
                    dy = 0;
                    Globals.Map.ScrollMap(Direction.UP);
                }
                else if (dy < 0 && (int)_position.Y <= 5 && Globals.Map.RowStartIndex > 0)
                {
                    dy = 0;
                    Globals.Map.ScrollMap(Direction.DOWN);
                }
                _position.X += dx;
                _position.Y += dy;
                System.Console.WriteLine($"Position: {_position.X}, {_position.Y} Map Index: {MapX}, {MapY}");
            }
        }
    }
}
