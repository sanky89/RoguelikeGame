using System;

namespace RoguelikeGame
{
    public class Vec2Int : IEquatable<Vec2Int>
    {
        public Vec2Int(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; set; }
        public int Y { get; set; }

        public static Vec2Int Zero => new(0, 0);
        public static Vec2Int One => new(1, 1);
        public static Vec2Int Left => new(-1, 0);
        public static Vec2Int Right => new(1, 0);
        public static Vec2Int Up => new(0, 1);
        public static Vec2Int Down => new(0, -1);

        public int MagnitudeSquared => (X * X) + (Y * Y);
        public float Magnitude => (float)Math.Sqrt(MagnitudeSquared);

        public static Vec2Int operator +(Vec2Int a, Vec2Int b)
        {
            return new Vec2Int(a.X + b.X, a.Y + b.Y);
        }

        public static Vec2Int operator -(Vec2Int a, Vec2Int b)
        {
            return new Vec2Int(a.X - b.X, a.Y - b.Y);
        }

        public static Vec2Int operator *(Vec2Int a, int i)
        {
            return new Vec2Int(a.X * i, a.Y * i);
        }

        public static Vec2Int operator *(int i, Vec2Int a)
        {
            return new Vec2Int(a.X * i, a.Y * i);
        }

        public static Vec2Int operator /(Vec2Int a, int i)
        {
            if (i == 0)
            {
                throw new DivideByZeroException();
            }
            return new Vec2Int(a.X / i, a.Y / i);
        }

        public static bool operator ==(Vec2Int a, Vec2Int b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Vec2Int a, Vec2Int b)
        {
            return !(a == b);
        }

        public static Vec2Int Negate(Vec2Int v)
        {
            return new Vec2Int(-v.X, -v.Y);
        }

        public static int DistanceSquared(Vec2Int a, Vec2Int b)
        {
            return (a - b).MagnitudeSquared;
        }

        public static float Distance(Vec2Int a, Vec2Int b)
        {
            return (a - b).Magnitude;
        }

        public override bool Equals(object other)
        {
            return other is Vec2Int otherVec && Equals(otherVec);
        }

        public bool Equals(Vec2Int other)
        {
            return X == other.X && Y == other.Y;
        }

        public override string ToString()
        {
            return $"({X},{Y})";
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
    }
}
