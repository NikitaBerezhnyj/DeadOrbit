using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DeadOrbit.Entities
{
    public class Beacon
    {
        public Rectangle Bounds;
        public bool IsReady;

        public Beacon(Rectangle bounds)
        {
            Bounds = bounds;
            IsReady = false;
        }

        public void Update(List<Base> bases)
        {
            IsReady = bases.All<Base>(b => b.IsActivated);
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D pixel)
        {
            spriteBatch.Draw(pixel, Bounds, IsReady ? Color.Gold : Color.DarkSlateGray);
        }
    }
}
