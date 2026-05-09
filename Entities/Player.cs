using System;
using System.Collections.Generic;
using DeadOrbit.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DeadOrbit
{
    public class Player : GameObject
    {
        private const float Speed = 160f;

        public Vector2 FacingDirection { get; private set; } = Vector2.Zero;
        public Inventory Inventory { get; private set; }

        public Rectangle Bounds =>
            new Rectangle(
                (int)Position.X + 4,
                (int)Position.Y + 4,
                TileGrid.TileSize - 8,
                TileGrid.TileSize - 8
            );

        public GridPosition GridPos =>
            TileGrid.ToGrid(Position + new Vector2(TileGrid.TileSize / 2f));

        public Player(int tileX, int tileY)
        {
            PlaceOnGrid(tileX, tileY);
            Inventory = new Inventory();
        }

        public override void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Vector2 dir = InputSystem.GetMovementDirection();

            if (dir.LengthSquared() > 0.01f)
            {
                dir.Normalize();
                FacingDirection = dir;
                Position += dir * Speed * dt;
            }

            if (InputSystem.NextItem)
                Inventory.Next();
            if (InputSystem.PrevItem)
                Inventory.Prev();
        }

        public void ResolveCollisions(IEnumerable<ICollidable> collidables)
        {
            CollisionSystem.Resolve(ref Position, Bounds, collidables);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(GameResources.Pixel, Bounds, Color.White);
        }
    }
}
