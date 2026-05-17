using System;
using System.Collections.Generic;
using DeadOrbit.Core;
using DeadOrbit.Entities.Items;
using DeadOrbit.Interfaces;
using DeadOrbit.Managers;
using DeadOrbit.Models;
using DeadOrbit.Rendering;
using DeadOrbit.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DeadOrbit.Entities
{
    public class Player : GameObject
    {
        private const float Speed = 160f;

        public int HP { get; private set; } = 10;
        public int MaxHP { get; private set; } = 10;
        private float _stunTimer = 0f;
        private float _iFrames = 0f;
        public bool IsInvincible => _iFrames > 0;
        private Vector2 _knockbackVelocity = Vector2.Zero;
        private const float KnockbackDecay = 10f;
        public bool IsStunned => _stunTimer > 0;
        private readonly SwingAnimation _swing = new();

        public Vector2 FacingDirection { get; private set; } = new Vector2(0, 1);
        public InventoryManager InventoryManager { get; private set; }

        public Rectangle Bounds =>
            new Rectangle(
                (int)Position.X + 4,
                (int)Position.Y + 4,
                TileGrid.TileSize - 8,
                TileGrid.TileSize - 8
            );

        public Player(int tileX, int tileY)
        {
            PlaceOnGrid(tileX, tileY);
            InventoryManager = new InventoryManager();
        }

        public override void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            _swing.Update(dt);

            if (_iFrames > 0)
                _iFrames -= dt;

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

            Position.X = MathHelper.Clamp(Position.X, 0, TileGrid.WorldPixelW - TileGrid.TileSize);
            Position.Y = MathHelper.Clamp(Position.Y, 0, TileGrid.WorldPixelH - TileGrid.TileSize);
        }

        public void ResolveCollisions(IEnumerable<ICollidable> collidables)
        {
            CollisionSystem.Resolve(ref Position, Bounds, collidables);
        }

        public void PlaySwingAnimation()
        {
            _swing.Trigger(FacingDirection);
        }

        public void TakeDamage(int amount, Vector2 knockbackDir)
        {
            if (_iFrames > 0)
                return;

            HP -= amount;
            _stunTimer = 0.5f;
            _iFrames = 0.6f;
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

        public DroppedItem TryDrop()
        {
            var active = InventoryManager.ActiveItem;

            if (active == null || active.IsEmpty)
                return null;

            if (active.Type == ItemType.Tool || active.Type == ItemType.Weapon)
                return null;

            Vector2 dir = FacingDirection == Vector2.Zero ? Vector2.UnitY : FacingDirection;

            Vector2 dropPos = Position + dir * TileGrid.TileSize;

            var droppedItem = new InventoryItem(
                active.Name,
                active.Type,
                1,
                active.Color,
                active.SpriteSource
            );

            var dropped = new DroppedItem(dropPos, droppedItem, applyImpulse: true);
            dropped.PickupDelay = 0.8f;

            InventoryManager.DropActive();

            return dropped;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(GameResources.Pixel, Bounds, Color.White);
            _swing.Draw(spriteBatch, Position);
        }
    }
}
