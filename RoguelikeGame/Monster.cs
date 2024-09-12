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
        private Fov _fov;
        private Map _map;

        public Monster(Character character, string name, int id, Stats stats) : base(character)
        {
            Id = id;
            Name = name;
            Stats = stats;
        }

        public override void SetMapPosition(Map map, int x, int y)
        {
            base.SetMapPosition(map, x, y);
            _map = map;
            _fov = new Fov(map, 6);
            _fov.UpdateFov(MapX, MapY);
        }

        public void Move(Map map, int x, int y)
        {
            map.SetTileType(MapX, MapY, TileType.Walkable);
            base.SetMapPosition(map, x, y);
            _fov.UpdateFov(MapX, MapY);
        }

        public bool IsPlayerInFov()
        {
            var target = Globals.Map.Player;
            var distanceSq = Vec2Int.DistanceSquared(new Vec2Int(MapX, MapY),new Vec2Int(target.MapX, target.MapY));
            return distanceSq <= _fov.FovMax * _fov.FovMax;
        }

        public bool IsPlayerInAttackRange()
        {
            var target = Globals.Map.Player;
            return Math.Abs(MapX - target.MapX) == 1 ||
                   Math.Abs(MapY - target.MapY) == 1;
        }
    }
}