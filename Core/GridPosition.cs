using Microsoft.Xna.Framework;

namespace DeadOrbit.Core
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
    }
}
