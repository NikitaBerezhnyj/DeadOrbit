using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DeadOrbit.Entities
{
    public class Beacon : GameObject
    {
        public Rectangle Bounds => new Rectangle((int)Position.X, (int)Position.Y, 50, 50);
        public bool IsReady;

        public Beacon(Vector2 position)
        {
            Position = position;
        }

        public void Check(List<BaseStation> bases)
        {
            IsReady = bases.All(b => b.IsActivated);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                GameResources.Pixel,
                Bounds,
                IsReady ? Color.Gold : Color.DarkSlateGray
            );
        }
    }
}
