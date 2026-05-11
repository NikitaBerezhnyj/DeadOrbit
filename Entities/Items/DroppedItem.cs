using DeadOrbit;
using DeadOrbit.Core;
using DeadOrbit.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DeadOrbit.Entities.Items
{
    public class DroppedItem : GameObject
    {
        public InventoryItem Item;
        public bool IsPickedUp = false;
        public float PickupDelay = 0.5f;

        private const int Size = 12;
        private const float MagnetRange = TileGrid.TileSize * 1f;
        private const float PickupRange = TileGrid.TileSize * 0.6f;
        private const float MagnetSpeed = 180f;

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

        public override void Update(GameTime gameTime)
        {
            if (PickupDelay > 0)
            {
                PickupDelay -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                return;
            }
        }

        public void UpdateAttraction(GameTime gameTime, Vector2 playerPosition)
        {
            if (IsPickedUp || PickupDelay > 0)
                return;

            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            var myCenter = Position + new Vector2(TileGrid.TileSize / 2f);
            var playerCenter = playerPosition + new Vector2(TileGrid.TileSize / 2f);
            float dist = Vector2.Distance(myCenter, playerCenter);

            if (dist < MagnetRange)
            {
                var dir = Vector2.Normalize(playerCenter - myCenter);

                float speedMultiplier = 1f + (1f - dist / MagnetRange) * 2f;
                Position += dir * MagnetSpeed * speedMultiplier * dt;
            }
        }

        public bool IsInPickupRange(Vector2 playerPosition)
        {
            var myCenter = Position + new Vector2(TileGrid.TileSize / 2f);
            var playerCenter = playerPosition + new Vector2(TileGrid.TileSize / 2f);
            return Vector2.Distance(myCenter, playerCenter) < PickupRange;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsPickedUp)
                return;

            if (Item.SpriteSource.HasValue)
            {
                spriteBatch.Draw(
                    GameResources.Tileset,
                    Bounds,
                    Item.SpriteSource.Value,
                    Color.White
                );
            }
            else
            {
                spriteBatch.Draw(GameResources.Pixel, Bounds, Item.Color);
            }
        }
    }
}
