using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace RoguelikeGame
{
    public class Monster : Entity
    {
        public readonly string Name;
        public readonly Stats Stats;
        public readonly int Fov;

        public Monster(Character character, string name, int id, Stats stats) : base(character)
        {
            Id = id;
            Name = name;
            Stats = stats;
            Fov = Stats["fov"].CurrentValue;
        }

        public override void SetMapPosition(Map map, int x, int y)
        {
            base.SetMapPosition(map, x, y);
        }

        public void Move(Map map, int x, int y)
        {
            map.SetTileType(MapX, MapY, TileType.Walkable);
            base.SetMapPosition(map, x, y);
        }

        public bool IsPlayerInFov()
        {
            var target = Globals.Map.Player;
            var distanceSq = Vec2Int.DistanceSquared(new Vec2Int(MapX, MapY),new Vec2Int(target.MapX, target.MapY));
            return distanceSq <= Fov * Fov;
        }

        public bool IsPlayerInAttackRange()
        {
            var target = Globals.Map.Player;
            return Math.Abs(MapX - target.MapX) <= 1 &&
                   Math.Abs(MapY - target.MapY) <= 1;
        }
    }
}