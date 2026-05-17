using System;
using Microsoft.Xna.Framework;

namespace DeadOrbit.Rendering
{
    public class ShakeEffect
    {
        private float _timer;
        private float _intensity;
        private const float Duration = 0.15f;

        public bool IsActive => _timer > 0;

        public void Trigger(float intensity = 3f)
        {
            _timer = Duration;
            _intensity = intensity;
        }

        public void Update(float dt)
        {
            if (_timer > 0)
                _timer -= dt;
        }

        public Vector2 GetOffset(GameTime gameTime)
        {
            if (!IsActive)
                return Vector2.Zero;
            float t = (float)gameTime.TotalGameTime.TotalSeconds;
            float strength = (_timer / Duration) * _intensity;
            return new Vector2(MathF.Sin(t * 60f) * strength, MathF.Cos(t * 47f) * strength);
        }
    }
}
