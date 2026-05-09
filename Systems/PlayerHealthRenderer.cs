using DeadOrbit.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DeadOrbit.Systems
{
    public class PlayerHealthRenderer
    {
        private const int BarWidth = 160;
        private const int BarHeight = 12;
        private const int Margin = 12;

        private readonly Player _player;

        public PlayerHealthRenderer(Player player)
        {
            _player = player;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            float ratio = (float)_player.HP / _player.MaxHP;

            spriteBatch.Draw(
                GameResources.Pixel,
                new Rectangle(Margin, Margin, BarWidth, BarHeight),
                new Color(60, 20, 20, 200)
            );

            spriteBatch.Draw(
                GameResources.Pixel,
                new Rectangle(Margin, Margin, (int)(BarWidth * ratio), BarHeight),
                Color.Crimson
            );

            int t = 1;
            var border = new Rectangle(Margin, Margin, BarWidth, BarHeight);
            spriteBatch.Draw(
                GameResources.Pixel,
                new Rectangle(border.X, border.Y, border.Width, t),
                Color.DarkRed
            );
            spriteBatch.Draw(
                GameResources.Pixel,
                new Rectangle(border.X, border.Bottom - t, border.Width, t),
                Color.DarkRed
            );
            spriteBatch.Draw(
                GameResources.Pixel,
                new Rectangle(border.X, border.Y, t, border.Height),
                Color.DarkRed
            );
            spriteBatch.Draw(
                GameResources.Pixel,
                new Rectangle(border.Right - t, border.Y, t, border.Height),
                Color.DarkRed
            );
        }
    }
}
