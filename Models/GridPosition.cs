using System;
using DeadOrbit.Core;
using Microsoft.Xna.Framework;

namespace DeadOrbit.Models
{
    public struct GridPosition
    {
        public int X;
        public int Y;

        public GridPosition(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Vector2 ToWorld() => TileGrid.ToWorld(X, Y);

        public static GridPosition operator +(GridPosition a, GridPosition b) =>
            new GridPosition(a.X + b.X, a.Y + b.Y);

        public static bool operator ==(GridPosition a, GridPosition b) => a.X == b.X && a.Y == b.Y;

        public static bool operator !=(GridPosition a, GridPosition b) => !(a == b);

        public override string ToString() => $"({X}, {Y})";

        public override bool Equals(object obj)
        {
            if (obj is GridPosition other)
                return X == other.X && Y == other.Y;
            return false;
        }

        public override int GetHashCode() => HashCode.Combine(X, Y);
    }
}
