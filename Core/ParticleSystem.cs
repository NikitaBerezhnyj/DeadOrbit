using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DeadOrbit.Core
{
    public class Particle
    {
        public Vector2 Position;
        public Vector2 Velocity;
        public float Life;
        public float MaxLife;
        public Color Color;
        public int Size;
    }

    public class ParticleSystem
    {
        private readonly List<Particle> _particles = new();
        private readonly Random _rnd = new();

        public void Emit(Vector2 worldPos, Color color, int count = 6)
        {
            for (int i = 0; i < count; i++)
            {
                float angle = (float)(_rnd.NextDouble() * MathF.PI * 2);
                float speed = 20f + (float)_rnd.NextDouble() * 60f;
                float life = 0.3f + (float)_rnd.NextDouble() * 0.3f;

                _particles.Add(
                    new Particle
                    {
                        Position = worldPos,
                        Velocity = new Vector2(MathF.Cos(angle), MathF.Sin(angle)) * speed,
                        Life = life,
                        MaxLife = life,
                        Color = color,
                        Size = _rnd.Next(2, 5),
                    }
                );
            }
        }

        public void Update(float dt)
        {
            for (int i = _particles.Count - 1; i >= 0; i--)
            {
                var p = _particles[i];
                p.Life -= dt;
                if (p.Life <= 0)
                {
                    _particles.RemoveAt(i);
                    continue;
                }

                p.Velocity.Y += 40f * dt;
                p.Velocity *= 0.92f;
                p.Position += p.Velocity * dt;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var p in _particles)
            {
                float alpha = p.Life / p.MaxLife;
                var rect = new Rectangle((int)p.Position.X, (int)p.Position.Y, p.Size, p.Size);
                spriteBatch.Draw(GameResources.Pixel, rect, p.Color * alpha);
            }
        }
    }
}
