using System;
using System.Collections.Generic;
using DeadOrbit.Core;
using DeadOrbit.Entities.Items;
using DeadOrbit.Interfaces;
using DeadOrbit.Models;
using DeadOrbit.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DeadOrbit.Entities.Enemies
{
    public enum EnemyState
    {
        Idle,
        Wander,
        Chase,
        Attack,
    }

    public abstract class Enemy : GameObject, ICollidable
    {
        public int HP;
        public int MaxHP;
        public int Damage;
        public float Speed;
        public bool IsDefeated => HP <= 0;
        public bool BlocksMovement => !IsDefeated;

        private List<GridPosition> _path = null;
        private int _pathIndex = 0;
        private float _pathUpdateTimer = 0f;
        private const float PathUpdateInterval = 0.5f;

        public Rectangle Bounds =>
            new Rectangle((int)Position.X, (int)Position.Y, TileGrid.TileSize, TileGrid.TileSize);

        protected EnemyState State = EnemyState.Idle;
        protected float AggroRange;
        protected float AttackRange;
        protected float AttackCooldown;

        private float _idleTimer = 0f;
        private float _wanderTimer = 0f;
        private Vector2 _wanderTarget;
        private bool _hasWanderTarget = false;
        private readonly Random _rnd = new();
        private const float WanderSpeed = 0.4f;
        private float _attackTimer = 0f;
        private float _stunTimer = 0f;
        private Vector2 _knockbackVelocity = Vector2.Zero;
        private const float KnockbackDecay = 8f;

        protected Enemy(int tileX, int tileY, int hp, int damage, float speed, float aggroRange)
        {
            PlaceOnGrid(tileX, tileY);
            HP = MaxHP = hp;
            Damage = damage;
            Speed = speed;
            AggroRange = aggroRange;
            AttackRange = TileGrid.TileSize * 0.9f;
            AttackCooldown = 1.5f;
        }

        public abstract InventoryItem GetDrop();

        public void TakeDamage(int amount, Vector2 knockbackDir, float knockbackMultiplier = 1.0f)
        {
            if (_stunTimer > 0)
                return;

            HP -= amount;
            _stunTimer = 0.4f;
            _knockbackVelocity = knockbackDir * 120f * knockbackMultiplier;

            Console.WriteLine($"[ENEMY] {GetType().Name} HP: {HP}/{MaxHP}");
        }

        public override void Update(GameTime gameTime)
        {
            if (IsDefeated)
                return;

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

            _attackTimer -= dt;
        }

        public DroppedItem UpdateAI(GameTime gameTime, Player player, HashSet<GridPosition> blocked)
        {
            if (IsDefeated)
                return null;

            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            var center = Position + new Vector2(TileGrid.TileSize / 2f);
            var playerCenter = player.Position + new Vector2(TileGrid.TileSize / 2f);
            float dist = Vector2.Distance(center, playerCenter);

            switch (State)
            {
                case EnemyState.Idle:
                    if (dist <= AggroRange)
                    {
                        State = EnemyState.Chase;
                        break;
                    }

                    _idleTimer -= dt;
                    if (_idleTimer <= 0f)
                    {
                        _idleTimer = 2f + (float)_rnd.NextDouble() * 3f;
                        if (_rnd.NextDouble() < 0.6f)
                        {
                            State = EnemyState.Wander;
                            _wanderTimer = 1.5f + (float)_rnd.NextDouble() * 2f;
                            _hasWanderTarget = false;
                        }
                    }
                    break;

                case EnemyState.Wander:
                    if (dist <= AggroRange)
                    {
                        State = EnemyState.Chase;
                        _hasWanderTarget = false;
                        break;
                    }

                    _wanderTimer -= dt;
                    if (_wanderTimer <= 0f)
                    {
                        State = EnemyState.Idle;
                        _idleTimer = 1f + (float)_rnd.NextDouble() * 2f;
                        _hasWanderTarget = false;
                        break;
                    }

                    if (
                        !_hasWanderTarget
                        || Vector2.Distance(
                            Position + new Vector2(TileGrid.TileSize / 2f),
                            _wanderTarget
                        ) < 4f
                    )
                    {
                        float angle = (float)(_rnd.NextDouble() * MathF.PI * 2);
                        float radius = TileGrid.TileSize * (1f + (float)_rnd.NextDouble() * 2f);
                        _wanderTarget =
                            Position
                            + new Vector2(MathF.Cos(angle) * radius, MathF.Sin(angle) * radius);

                        _wanderTarget.X = MathHelper.Clamp(
                            _wanderTarget.X,
                            0,
                            TileGrid.WorldPixelW - TileGrid.TileSize
                        );
                        _wanderTarget.Y = MathHelper.Clamp(
                            _wanderTarget.Y,
                            0,
                            TileGrid.WorldPixelH - TileGrid.TileSize
                        );
                        _hasWanderTarget = true;
                    }

                    var wanderDir =
                        _wanderTarget - (Position + new Vector2(TileGrid.TileSize / 2f));
                    if (wanderDir.LengthSquared() > 1f)
                    {
                        wanderDir.Normalize();
                        Position += wanderDir * Speed * WanderSpeed * dt;
                    }
                    break;

                case EnemyState.Chase:
                    if (dist > AggroRange)
                    {
                        State = EnemyState.Idle;
                        _path = null;
                        break;
                    }
                    if (dist <= AttackRange)
                    {
                        State = EnemyState.Attack;
                        _path = null;
                        break;
                    }
                    MoveAlongPath(gameTime, player, blocked);
                    break;

                case EnemyState.Attack:
                    if (dist > AttackRange * 1.5f)
                    {
                        State = EnemyState.Chase;
                        break;
                    }
                    if (_attackTimer <= 0 && _stunTimer <= 0)
                    {
                        _attackTimer = AttackCooldown;
                        var knockDir = Vector2.Normalize(playerCenter - center);
                        CombatSystem.HitPlayer(player, Damage, knockDir);
                    }

                    break;
            }

            return null;
        }

        private void MoveAlongPath(GameTime gameTime, Player player, HashSet<GridPosition> blocked)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _pathUpdateTimer -= dt;

            if (_pathUpdateTimer <= 0f || _path == null)
            {
                _pathUpdateTimer = PathUpdateInterval;
                _path = PathfinderSystem.FindPath(GridPos, player.GridPos, blocked);
                _pathIndex = 1;
            }

            if (_path == null || _pathIndex >= _path.Count)
                return;

            Vector2 target = TileGrid.ToWorld(_path[_pathIndex]);
            Vector2 center = Position + new Vector2(TileGrid.TileSize / 2f);
            Vector2 targetCenter = target + new Vector2(TileGrid.TileSize / 2f);
            Vector2 dir = targetCenter - center;

            if (dir.Length() < 2f)
                _pathIndex++;
            else
            {
                dir.Normalize();
                Position += dir * Speed * dt;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsDefeated)
                return;
            spriteBatch.Draw(GameResources.Pixel, Bounds, GetBodyColor());

            int barW = TileGrid.TileSize;
            int barH = 4;
            int barX = (int)Position.X;
            int barY = (int)Position.Y - barH - 2;
            float ratio = (float)HP / MaxHP;

            spriteBatch.Draw(
                GameResources.Pixel,
                new Rectangle(barX, barY, barW, barH),
                Color.DarkRed
            );
            spriteBatch.Draw(
                GameResources.Pixel,
                new Rectangle(barX, barY, (int)(barW * ratio), barH),
                Color.Red
            );
        }

        public void ResolveCollisions(IEnumerable<ICollidable> collidables)
        {
            CollisionSystem.Resolve(ref Position, Bounds, collidables);
        }

        protected virtual Color GetBodyColor() => Color.Purple;
    }
}
