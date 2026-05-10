using DeadOrbit.Core;
using DeadOrbit.Interfaces;
using DeadOrbit.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DeadOrbit.Entities
{
    public abstract class ResourceNode : GameObject, ICollidable
    {
        public int HP;
        public bool IsDestroyed => HP <= 0;
        public bool BlocksMovement => !IsDestroyed;
        public abstract InventoryItem GetDrop();

        public Rectangle Bounds =>
            new Rectangle((int)Position.X, (int)Position.Y, TileGrid.TileSize, TileGrid.TileSize);

        protected Color NodeColor;

        protected ResourceNode(int tileX, int tileY, int hp, Color color)
        {
            PlaceOnGrid(tileX, tileY);
            HP = hp;
            NodeColor = color;
        }

        public void Mine(int damage)
        {
            if (!IsDestroyed)
                HP -= damage;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsDestroyed)
                return;

            spriteBatch.Draw(GameResources.Pixel, Bounds, NodeColor);
        }
    }
}
