using DeadOrbit.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DeadOrbit.Entities
{
    public class DroppedItem : GameObject
    {
        public InventoryItem Item;
        public bool IsPickedUp = false;

        private const int Size = 12;

        public Rectangle Bounds =>
            new Rectangle(
                (int)Position.X + (TileGrid.TileSize - Size) / 2,
                (int)Position.Y + (TileGrid.TileSize - Size) / 2,
                Size,
                Size
            );

        public DroppedItem(Vector2 worldPosition, InventoryItem item)
        {
            Position = worldPosition;
            Item = item;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsPickedUp)
                return;
            spriteBatch.Draw(GameResources.Pixel, Bounds, Item.Color);
        }
    }
}
