using DeadOrbit.Core;
using DeadOrbit.Interfaces;
using DeadOrbit.Models;
using DeadOrbit.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DeadOrbit.Entities.Resources
{
    public abstract class ResourceNode : GameObject, ICollidable
    {
        public int HP;
        private GameTime _lastGameTime;
        public bool IsDestroyed => HP <= 0;
        public bool BlocksMovement => !IsDestroyed;
        private readonly ShakeEffect _shake = new();
        protected abstract Rectangle? GetSpriteSource();
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

        public void Mine(int damage, ParticleSystem particles = null)
        {
            if (IsDestroyed)
                return;
            HP -= damage;
            _shake.Trigger(intensity: 3f);

            particles?.Emit(Position + new Vector2(TileGrid.TileSize / 2f), NodeColor, count: 8);
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

            Vector2 offset = GetShakeOffset();
            var dest = new Rectangle(
                (int)(Position.X + offset.X),
                (int)(Position.Y + offset.Y),
                TileGrid.TileSize,
                TileGrid.TileSize
            );

            var source = GetSpriteSource();
            if (source.HasValue)
                spriteBatch.Draw(GameResources.Tileset, dest, source.Value, Color.White);
            else
                spriteBatch.Draw(GameResources.Pixel, dest, NodeColor);
        }

        protected Vector2 GetShakeOffset() =>
            _lastGameTime == null ? Vector2.Zero : _shake.GetOffset(_lastGameTime);
    }
}
