using System.Collections.Generic;
using System.Linq;
using DeadOrbit.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DeadOrbit.Entities.Structures
{
    public class Beacon : GameObject
    {
        public Rectangle Bounds =>
            new Rectangle((int)Position.X, (int)Position.Y, TileGrid.TileSize, TileGrid.TileSize);
        public bool IsReady;

        public Beacon(int tileX, int tileY)
        {
            PlaceOnGrid(tileX, tileY);
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
