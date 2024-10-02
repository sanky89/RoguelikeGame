using System;

namespace RoguelikeGame
{
    public class Monster : Entity
    {
        public readonly string Name;
        public readonly Stats Stats;
        public readonly int Fov;
        private Direction _facing;
        
        public Direction Facing => _facing;

        public Monster(Character character, string name, int id, Stats stats) : base(character)
        {
            Id = id;
            Name = name;
            Stats = stats;
            Fov = Stats["fov"].CurrentValue;
            _facing = Direction.LEFT;
        }

        public override void SetMapPosition(Map map, int x, int y)
        {
            base.SetMapPosition(map, x, y);
        }

        public void Move(Map map, int x, int y)
        {
            map.SetTileType(MapX, MapY, TileType.Walkable);
            if(map.Player.MapX < MapX)
            {
                _facing = Direction.LEFT;
            }
            else if(map.Player.MapX > MapX)
            {
                _facing = Direction.RIGHT;
            }
            
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