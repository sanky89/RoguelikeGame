using System;
using System.Collections.Generic;
using System.Security;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RoguelikeGame
{
    public class Player : Entity
    {
        public const int FOV_SIZE = 20;

        private Fov _fov;
        public readonly EntityStats PlayerStats;
        public Player(Character character, List<StatDataModel> stats) : base( character)
        {
            PlayerStats = new EntityStats(stats);
        }

        public override void SetMapPosition(int x, int y)
        {
            base.SetMapPosition(x, y);
            System.Console.WriteLine($"Map Position: {MapX}, {MapY}");
            _fov = new Fov(Globals.Map);
            _fov.UpdateFov(MapX, MapY);
        }

        public ActionResult PerformAction(InputAction action)
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
                case InputAction.MOVE_NW:
                    dx = -1;
                    dy = -1;
                    break;
                case InputAction.MOVE_NE:
                    dx = 1;
                    dy = -1;
                    break;
                case InputAction.MOVE_SW:
                    dx = -1;
                    dy = 1;
                    break;
                case InputAction.MOVE_SE:
                    dx = 1;
                    dy = 1;
                    break;
                default:
                    break;
            }

            // System.Console.WriteLine($"{(int)_position.X + Globals.Map.colStartIndex}");
            var newX = MapX + dx;
            var newY = MapY + dy;
            var actionResult = Globals.Map.CanMove(newX, newY);
            if (actionResult.ResultType == ActionResultType.Move ||
                actionResult.ResultType == ActionResultType.CollectedCoins)
            {
                MapX += dx;
                MapY += dy;
                _fov.UpdateFov(MapX, MapY);
                System.Console.WriteLine($"Map Index: {MapX}, {MapY}");
            }
            return actionResult;
        }
    }
}
