using Microsoft.Xna.Framework;

namespace DeadOrbit.Interfaces
{
    public interface ICollidable
    {
        Rectangle Bounds { get; }
        bool BlocksMovement { get; }
    }
}
