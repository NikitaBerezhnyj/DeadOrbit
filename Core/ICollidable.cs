using Microsoft.Xna.Framework;

namespace DeadOrbit.Core
{
    public interface ICollidable
    {
        Rectangle Bounds { get; }
        bool BlocksMovement { get; }
    }
}
