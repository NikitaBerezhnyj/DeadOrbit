using DeadOrbit.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DeadOrbit.Entities
{
    public class BaseStation : GameObject
    {
        public Rectangle Bounds =>
            new Rectangle((int)Position.X, (int)Position.Y, TileGrid.TileSize, TileGrid.TileSize);

        public bool IsActivated;

        public BaseStation(int tileX, int tileY)
        {
            PlaceOnGrid(tileX, tileY);
        }

        public void Check(Player player)
        {
            if (player.GridPos == GridPos)
                IsActivated = true;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(GameResources.Pixel, Bounds, IsActivated ? Color.Lime : Color.Crimson);
        }
    }
}
