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
        private GameTime _lastGameTime;
        public bool IsDestroyed => HP <= 0;
        public bool BlocksMovement => !IsDestroyed;
        private readonly ShakeEffect _shake = new();
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
            if (IsDestroyed)
                return;
            HP -= damage;
            _shake.Trigger(intensity: 3f);
        }

        public override void Update(GameTime gameTime)
        {
            _lastGameTime = gameTime;
            _shake.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsDestroyed)
                return;

            spriteBatch.Draw(GameResources.Pixel, Bounds, NodeColor);
        }

        protected Vector2 GetShakeOffset() =>
            _lastGameTime == null ? Vector2.Zero : _shake.GetOffset(_lastGameTime);
    }
}
