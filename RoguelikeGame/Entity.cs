﻿using Microsoft.Xna.Framework;

namespace RoguelikeGame
{
    public class Entity
    {
        public const int ENERGY_THRESHOLD = 20;
        protected Character _character;

        public int Id { get; set; }
        public Color Color => Globals.IsAscii ? _character.Color : Color.White;
        public Rectangle SourceRect => _character.GetSourceRect();
        public int MapX { get; protected set; }
        public int MapY { get; protected set; }

        public Entity(Character character)
        {
            _character = character;
        }

        public virtual void Update(GameTime gameTime) { }

        public virtual void SetMapPosition(Map map, int x, int y)
        {
            MapX = x;
            MapY = y;
            map.SetTileType(x,y,TileType.Entity);
        }

        public virtual void PerformAction()
        {
        }
    }
}
