using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace DeadOrbit
{
    public static class InputSystem
    {
        public static Vector2 GetMovementDirection()
        {
            Vector2 direction = Vector2.Zero;
            var kState = Keyboard.GetState();
            var gState = GamePad.GetState(PlayerIndex.One);
            Console.WriteLine(GamePad.GetState(PlayerIndex.One).IsConnected);

            if (kState.IsKeyDown(Keys.W) || kState.IsKeyDown(Keys.Up))
                direction.Y -= 1;
            if (kState.IsKeyDown(Keys.S) || kState.IsKeyDown(Keys.Down))
                direction.Y += 1;
            if (kState.IsKeyDown(Keys.A) || kState.IsKeyDown(Keys.Left))
                direction.X -= 1;
            if (kState.IsKeyDown(Keys.D) || kState.IsKeyDown(Keys.Right))
                direction.X += 1;

            if (gState.IsConnected)
            {
                if (
                    Math.Abs(gState.ThumbSticks.Left.X) > 0.1f
                    || Math.Abs(gState.ThumbSticks.Left.Y) > 0.1f
                )
                {
                    direction.X = gState.ThumbSticks.Left.X;
                    direction.Y = -gState.ThumbSticks.Left.Y;
                }
            }

            if (direction != Vector2.Zero)
                direction.Normalize();

            return direction;
        }
    }
}
