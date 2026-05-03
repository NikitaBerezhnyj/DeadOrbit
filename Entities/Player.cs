using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DeadOrbit
{
    public class Player : GameObject
    {
        private float Speed = 250f;

        public Rectangle Bounds => new Rectangle((int)Position.X, (int)Position.Y, 20, 20);

        public Player(Vector2 startPosition)
        {
            Position = startPosition;
        }

        public override void Update(GameTime gameTime)
        {
            Vector2 direction = InputSystem.GetMovementDirection();
            Position += direction * Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (InputSystem.AttackPressed)
            {
                Console.WriteLine("[PLAYER] Attack / Mine");
            }

            if (InputSystem.UsePressed)
            {
                Console.WriteLine("[PLAYER] Use / Place");
            }

            if (InputSystem.DropPressed)
            {
                Console.WriteLine("[PLAYER] Drop item");
            }

            if (InputSystem.PausePressed)
            {
                Console.WriteLine("[GAME] Pause triggered");
            }

            if (InputSystem.CraftPressed)
            {
                Console.WriteLine("[UI] Craft menu opened");
            }

            if (InputSystem.NextItem)
            {
                Console.WriteLine($"[INVENTORY] +");
            }

            if (InputSystem.PrevItem)
            {
                Console.WriteLine($"[INVENTORY] -");
            }

            if (InputSystem.UiUp)
            {
                Console.WriteLine("[UI] Navigate UP");
            }

            if (InputSystem.UiDown)
            {
                Console.WriteLine("[UI] Navigate DOWN");
            }

            if (InputSystem.UiLeft)
            {
                Console.WriteLine("[UI] Navigate LEFT");
            }

            if (InputSystem.UiRight)
            {
                Console.WriteLine("[UI] Navigate RIGHT");
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(GameResources.Pixel, Bounds, Color.White);
        }
    }
}
