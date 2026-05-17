using System;
using DeadOrbit.Core;
using DeadOrbit.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DeadOrbit.Rendering
{
    public class SwingAnimation
    {
        private float _timer = 0f;
        private const float Duration = 0.25f;
        private Vector2 _direction;
        private bool _isActive = false;

        public bool IsActive => _isActive;

        public void Trigger(Vector2 facingDirection)
        {
            if (facingDirection == Vector2.Zero)
                return;
            _direction = facingDirection;
            _timer = Duration;
            _isActive = true;
        }

        public void Update(float dt)
        {
            if (!_isActive)
                return;
            _timer -= dt;
            if (_timer <= 0)
                _isActive = false;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 playerPosition)
        {
            if (!_isActive)
                return;

            float progress = 1f - (_timer / Duration);
            int half = TileGrid.TileSize / 2;
            Vector2 playerCenter = playerPosition + new Vector2(half);

            Vector2 perp = new Vector2(-_direction.Y, _direction.X);

            float swingT = progress * 2f - 1f;

            float widthFactor = 1f - MathF.Abs(swingT);
            int maxWidth = 20;
            int maxHeight = 6;
            int w = (int)(maxWidth * widthFactor) + 2;
            int h = (int)(maxHeight * widthFactor) + 2;

            float swingOffset = swingT * (half + 8f);
            Vector2 swingPos = playerCenter + _direction * (half + 4f) + perp * swingOffset;

            float alpha = widthFactor * 0.85f;

            if (progress > 0.15f && progress < 0.85f)
            {
                for (int i = 1; i <= 3; i++)
                {
                    float pastT = swingT - i * 0.15f;
                    float pastWidth = (1f - MathF.Abs(pastT)) * (maxWidth - i * 4);
                    if (pastWidth <= 0)
                        continue;

                    float pastOffset = pastT * (half + 8f);
                    Vector2 pastPos = playerCenter + _direction * (half + 4f) + perp * pastOffset;

                    int pw = (int)pastWidth + 1;
                    int ph = MathF.Max(1f, h - i * 1.5f) > 0 ? (int)MathF.Max(1f, h - i * 1.5f) : 1;

                    var tailRect = new Rectangle(
                        (int)pastPos.X - pw / 2,
                        (int)pastPos.Y - ph / 2,
                        pw,
                        ph
                    );
                    spriteBatch.Draw(
                        GameResources.Pixel,
                        tailRect,
                        Color.White * (alpha * (1f - i * 0.3f))
                    );
                }
            }

            var rect = new Rectangle((int)swingPos.X - w / 2, (int)swingPos.Y - h / 2, w, h);
            spriteBatch.Draw(GameResources.Pixel, rect, Color.White * alpha);
        }
    }
}
