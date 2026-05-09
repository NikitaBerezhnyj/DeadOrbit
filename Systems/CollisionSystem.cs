using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace DeadOrbit.Core
{
    public static class CollisionSystem
    {
        public static void Resolve(
            ref Vector2 position,
            Rectangle bounds,
            IEnumerable<ICollidable> collidables
        )
        {
            int offsetX = bounds.X - (int)position.X;
            int offsetY = bounds.Y - (int)position.Y;

            foreach (var obj in collidables)
            {
                if (!obj.BlocksMovement)
                    continue;

                Rectangle self = new Rectangle(
                    (int)position.X + offsetX,
                    (int)position.Y + offsetY,
                    bounds.Width,
                    bounds.Height
                );

                Rectangle other = obj.Bounds;
                if (!self.Intersects(other))
                    continue;

                int overlapLeft = self.Right - other.Left;
                int overlapRight = other.Right - self.Left;
                int overlapTop = self.Bottom - other.Top;
                int overlapBottom = other.Bottom - self.Top;

                bool fromLeft = overlapLeft < overlapRight;
                bool fromTop = overlapTop < overlapBottom;

                int minX = fromLeft ? overlapLeft : -overlapRight;
                int minY = fromTop ? overlapTop : -overlapBottom;

                if (Math.Abs(minX) < Math.Abs(minY))
                    position.X -= minX;
                else
                    position.Y -= minY;
            }
        }
    }
}
