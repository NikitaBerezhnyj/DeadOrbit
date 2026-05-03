using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DeadOrbit.Entities
{
    public class BaseStation : GameObject
    {
        public Rectangle Bounds => new Rectangle((int)Position.X, (int)Position.Y, 40, 40);
        public bool IsActivated;

        public BaseStation(Vector2 position)
        {
            Position = position;
        }

        public void Check(Player player)
        {
            if (player.Bounds.Intersects(Bounds))
                IsActivated = true;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(GameResources.Pixel, Bounds, IsActivated ? Color.Lime : Color.Crimson);
        }
    }
}
