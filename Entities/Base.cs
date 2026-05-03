using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DeadOrbit.Entities
{
    public class Base
    {
        public Rectangle Bounds;
        public bool IsActivated;

        public Base(Rectangle bounds)
        {
            Bounds = bounds;
            IsActivated = false;
        }

        public void Update(Player player)
        {
            if (player.Bounds.Intersects(Bounds))
                IsActivated = true;
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D pixel)
        {
            spriteBatch.Draw(pixel, Bounds, IsActivated ? Color.Lime : Color.Crimson);
        }
    }
}