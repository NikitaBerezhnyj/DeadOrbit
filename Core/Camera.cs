using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DeadOrbit.Core
{
    public class Camera
    {
        private readonly GraphicsDevice _graphics;

        public Camera(GraphicsDevice graphics)
        {
            _graphics = graphics;
        }

        public Matrix GetTransform(Vector2 targetPosition)
        {
            int screenW = _graphics.Viewport.Width;
            int screenH = _graphics.Viewport.Height;

            Vector2 center = targetPosition + new Vector2(TileGrid.TileSize / 2f);

            return Matrix.CreateTranslation(screenW / 2f - center.X, screenH / 2f - center.Y, 0);
        }
    }
}
