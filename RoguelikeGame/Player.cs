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
        public readonly Stats Stats;
        public Player(Character character, Stats stats) : base( character)
        {
            Stats = stats;
            Item.OnPickup += HandleItemPickup;
        }

        private void HandleItemPickup(Item item)
        {
            if(item.Name == "gold")
            {
                Stats["gold"].CurrentValue += item.Amount;
            }
        }

        public override void SetMapPosition(Map map, int x, int y)
        {
            base.SetMapPosition(map, x, y);
            System.Console.WriteLine($"Map Position: {MapX}, {MapY}");
            _fov = new Fov(map, 10);
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
                case InputAction.REST:
                    dx = 0;
                    dy = 0;
                    break;
                case InputAction.USE_ITEM_1:
                    Globals.Inventory.UseItem(1);
                    break;
                case InputAction.USE_ITEM_2:
                    Globals.Inventory.UseItem(2);
                    break;
                default:
                    break;
            }

            // System.Console.WriteLine($"{(int)_position.X + Globals.Map.colStartIndex}");
            var newX = MapX + dx;
            var newY = MapY + dy;
            var actionResult = GetActionResult(dx, dy, newX, newY);
            if (actionResult.ResultType == ActionResultType.Move ||
                actionResult.ResultType == ActionResultType.CollectItem)
            {
                Globals.Map.SetTileType(MapX, MapY, TileType.Walkable);
                base.SetMapPosition(Globals.Map, newX, newY);
                _fov.UpdateFov(MapX, MapY);
                System.Console.WriteLine($"Map Index: {MapX}, {MapY}");
            }
            return actionResult;
        }

        private ActionResult GetActionResult(int dx, int dy, int newX, int newY)
        {
            if(dx == 0 && dy == 0)
            {
                return new ActionResult(ActionResultType.Rest, null);
            }
            return Globals.Map.CanMove(newX, newY);
        }
    }
}
