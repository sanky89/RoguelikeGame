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

        private int _energy;

        public Monster(GameRoot gameRoot, Character character, string name, int id, Stats stats) : base(gameRoot, character)
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

        public override void PerformAction()
        {
            _energy += Stats["energyGain"].CurrentValue;

            if(_energy < Entity.ENERGY_THRESHOLD)
            {
                return;
            }

            if(IsPlayerInFov())
            {
                if(IsPlayerInAttackRange())
                {
                    //System.Console.WriteLine($"player is in {Name}_{Id} fov");
                    _gameRoot.CombatManager.ResolveCombat(_gameRoot.Map.Player, this, out var log, false);
                    _gameRoot.ActionLog.AddLog(log);
                    return;
                }
                var startNode = new Node(MapX, MapY);
                var endNode = new Node(_gameRoot.Map.Player.MapX, _gameRoot.Map.Player.MapY);
                var path = _gameRoot.Map.Pathfinder.CalculatePath(startNode, endNode);
                if(path != null && path.Count > 0)
                {
                    var pathString = "";
                    foreach (var node in path)
                    {
                        pathString += $" ({node.X},{node.Y}) ->";
                    }
                    //System.Console.WriteLine(pathString);
                    Move(_gameRoot.Map, path[0].X, path[0].Y);
                }
            }

            _energy = 0;
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
            var target = _gameRoot.Map.Player;
            var distanceSq = Vec2Int.DistanceSquared(new Vec2Int(MapX, MapY),new Vec2Int(target.MapX, target.MapY));
            return distanceSq <= Fov * Fov;
        }

        public bool IsPlayerInAttackRange()
        {
            var target = _gameRoot.Map.Player;
            return Math.Abs(MapX - target.MapX) <= 1 &&
                   Math.Abs(MapY - target.MapY) <= 1;
        }
    }
}