using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoguelikeGame
{
    public class Room
    {
        private Random _rng;

        public Rectangle RoomRect { get; private set; }

        public Rectangle PaddedRoomRect { get; private set; }

        public Room(Random rng, Rectangle rect)
        {
            _rng = rng;
            RoomRect = rect;
            PaddedRoomRect = new Rectangle(rect.X-1, rect.Y-1, rect.Width+1, rect.Height+1);
        }

        public Room(Random rng, int x, int y, int w, int h)
        {
            _rng=rng;
            RoomRect = new Rectangle(x, y, w, h);
            PaddedRoomRect = new Rectangle(x - 1, y - 1, w + 1, h + 1);
        }

        public bool Intersects(Room other)
        {
            return RoomRect.Intersects(other.RoomRect);
        }

        public bool IntersectsWithPadding(Room other)
        {
            return PaddedRoomRect.Intersects(other.PaddedRoomRect);
        }

        public Vec2Int GetRandomPointInsideRoom()
        {
            return new Vec2Int(_rng.Next(RoomRect.X + 1, RoomRect.Right), GameConstants.Rng.Next(RoomRect.Y + 1, RoomRect.Bottom));
        }
    }
}
