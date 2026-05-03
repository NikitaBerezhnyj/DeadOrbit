using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DeadOrbit
{
    public class Player : GameObject
    {
        private float Speed = 250f;
        private Texture2D _pixel;

        public Rectangle Bounds => new Rectangle((int)Position.X, (int)Position.Y, 20, 20);

        public Player(Vector2 startPosition, Texture2D pixel)
        {
            Position = startPosition;
            _pixel = pixel;
        }

        public override void Update(GameTime gameTime)
        {
            Vector2 direction = InputSystem.GetMovementDirection();
            Position += direction * Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_pixel, Bounds, Color.White);
        }
    }
}