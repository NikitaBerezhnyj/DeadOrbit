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

        public int HP { get; private set; } = 10;
        public int MaxHP { get; private set; } = 10;
        private float _stunTimer = 0f;
        private Vector2 _knockbackVelocity = Vector2.Zero;
        private const float KnockbackDecay = 10f;
        public bool IsStunned => _stunTimer > 0;

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

            if (_knockbackVelocity.LengthSquared() > 1f)
            {
                Position += _knockbackVelocity * dt;
                _knockbackVelocity -= _knockbackVelocity * KnockbackDecay * dt;
            }

            if (_stunTimer > 0)
            {
                _stunTimer -= dt;
                return;
            }

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

        public void TakeDamage(int amount, Vector2 knockbackDir)
        {
            HP -= amount;
            _stunTimer = 0.5f;
            _knockbackVelocity = knockbackDir * 150f;

            if (HP <= 0)
            {
                HP = MaxHP;
                Console.WriteLine("[GAME] GAME OVER — HP reset");
            }
            else
            {
                Console.WriteLine($"[PLAYER] HP: {HP}/{MaxHP}");
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(GameResources.Pixel, Bounds, Color.White);
        }
    }
}
