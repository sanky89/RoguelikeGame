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
        public Rectangle RoomRect { get; private set; }

        public Rectangle PaddedRoomRect { get; private set; }

        public Room(Rectangle rect)
        {
            RoomRect = rect;
            PaddedRoomRect = new Rectangle(rect.X-1, rect.Y-1, rect.Width+1, rect.Height+1);
        }

        public Room(int x, int y, int w, int h)
        {
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

        public Vector2 GetRandomPointInsideRoom()
        {
            Random random = new Random();
            return new Vector2(random.Next(RoomRect.X + 1, RoomRect.Right), random.Next(RoomRect.Y + 1, RoomRect.Bottom));
        }
    }
}
