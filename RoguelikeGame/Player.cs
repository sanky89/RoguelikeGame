using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Security;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RoguelikeGame
{
    public class Player : Entity
    {
        public readonly Stats Stats;
        private Direction _facing;
        private Fov _fov;

        public Direction Facing => _facing;
        public Player(GameRoot gameRoot, Character character, Stats stats) : base(gameRoot, character)
        {
            Stats = stats;
            Item.OnPickup += HandleItemPickup;
            Item.OnUse += HandleItemUsed;
            _facing = Direction.LEFT;
        }

        private void HandleItemPickup(Item item)
        {
            if(item.Name == "gold")
            {
                Stats["gold"].CurrentValue += item.Amount;
            }
        }

        private void HandleItemUsed(Item item)
        {
            var affectedStat = item.AffectedStat;
            Stats[affectedStat].CurrentValue += item.AmountAffected;
        }

        public override void SetMapPosition(Map map, int x, int y)
        {
            base.SetMapPosition(map, x, y);
            //System.Console.WriteLine($"Map Position: {MapX}, {MapY}");
            _fov = new Fov(map, 6);
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
                    _facing = Direction.LEFT;
                    break;
                case InputAction.MOVE_RIGHT:
                    dx = 1;
                    _facing = Direction.RIGHT;
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
                    _facing = Direction.LEFT;
                    break;
                case InputAction.MOVE_NE:
                    dx = 1;
                    dy = -1;
                    _facing = Direction.RIGHT;
                    break;
                case InputAction.MOVE_SW:
                    dx = -1;
                    dy = 1;
                    _facing = Direction.LEFT;
                    break;
                case InputAction.MOVE_SE:
                    dx = 1;
                    dy = 1;
                    _facing = Direction.RIGHT;
                    break;
                case InputAction.REST:
                    break;
                case InputAction.USE_ITEM_1:
                    _gameRoot.Inventory.UseItem(0);
                    break;
                case InputAction.USE_ITEM_2:
                    _gameRoot.Inventory.UseItem(1);
                    break;
                case InputAction.USE_ITEM_3:
                    _gameRoot.Inventory.UseItem(2);
                    break;
                case InputAction.USE_ITEM_4:
                    _gameRoot.Inventory.UseItem(3);
                    break;
                case InputAction.USE_ITEM_5:
                    _gameRoot.Inventory.UseItem(4);
                    break;
                default:
                    break;
            }

            // System.Console.WriteLine($"{(int)_position.X + Globals.Map.colStartIndex}");
            var newX = MapX + dx;
            var newY = MapY + dy;
            var actionResult = GetActionResult(action, newX, newY);            
            if (actionResult.ResultType == ActionResultType.Move ||
                actionResult.ResultType == ActionResultType.CollectItem)
            {
                _gameRoot.Map.SetTileType(MapX, MapY, TileType.Walkable);
                base.SetMapPosition(_gameRoot.Map, newX, newY);
                _fov.UpdateFov(MapX, MapY);
                //System.Console.WriteLine($"Map Index: {MapX}, {MapY}");
            }
            else if (actionResult.ResultType == ActionResultType.Rest)
            {
                Stats["health"].CurrentValue += 1;
            }
            return actionResult;
        }

        private ActionResult GetActionResult(InputAction action, int newX, int newY)
        {
            if(ActionHelper.IsUseItemAction(action))
            {
                return new ActionResult(ActionResultType.UseItem, null);
            }

            if(action is InputAction.REST)
            {
                return new ActionResult(ActionResultType.Rest, null);
            }
            return _gameRoot.Map.CanMove(newX, newY);
        }
    }
}
