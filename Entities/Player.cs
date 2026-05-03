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
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(GameResources.Pixel, Bounds, Color.White);
        }
    }
}
