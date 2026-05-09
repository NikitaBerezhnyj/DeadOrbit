using DeadOrbit.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DeadOrbit.Systems
{
    public class HotbarRenderer
    {
        private const int SlotSize = 48;
        private const int SlotPadding = 6;
        private const int BottomMargin = 16;
        private const int IconPadding = 6;
        private const int BarPadding = 10;

        private readonly Inventory _inventory;
        private readonly GraphicsDevice _graphics;

        public HotbarRenderer(Inventory inventory, GraphicsDevice graphics)
        {
            _inventory = inventory;
            _graphics = graphics;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            int screenW = _graphics.Viewport.Width;
            int screenH = _graphics.Viewport.Height;

            int totalW = Inventory.Size * SlotSize + (Inventory.Size - 1) * SlotPadding;
            int startX = (screenW - totalW) / 2;
            int startY = screenH - SlotSize - BottomMargin;

            Rectangle hotbarBg = new Rectangle(
                startX - BarPadding,
                startY - BarPadding,
                totalW + BarPadding * 2,
                SlotSize + BarPadding * 2
            );

            DrawRect(spriteBatch, hotbarBg, new Color(15, 15, 15, 200));

            DrawBorder(spriteBatch, hotbarBg, new Color(120, 120, 120, 220), 2);

            for (int i = 0; i < Inventory.Size; i++)
            {
                int x = startX + i * (SlotSize + SlotPadding);
                bool isActive = i == _inventory.ActiveIndex;

                DrawRect(
                    spriteBatch,
                    new Rectangle(x, startY, SlotSize, SlotSize),
                    isActive ? new Color(80, 80, 80, 220) : new Color(30, 30, 30, 180)
                );

                DrawBorder(
                    spriteBatch,
                    new Rectangle(x, startY, SlotSize, SlotSize),
                    isActive ? Color.White : new Color(100, 100, 100, 200),
                    2
                );

                var item = _inventory.Slots[i];
                if (!item.IsEmpty)
                {
                    DrawRect(
                        spriteBatch,
                        new Rectangle(
                            x + IconPadding,
                            startY + IconPadding,
                            SlotSize - IconPadding * 2,
                            SlotSize - IconPadding * 2
                        ),
                        item.Color
                    );
                }
            }
        }

        private void DrawRect(SpriteBatch sb, Rectangle rect, Color color)
        {
            sb.Draw(GameResources.Pixel, rect, color);
        }

        private void DrawBorder(SpriteBatch sb, Rectangle rect, Color color, int thickness)
        {
            sb.Draw(
                GameResources.Pixel,
                new Rectangle(rect.X, rect.Y, rect.Width, thickness),
                color
            );

            sb.Draw(
                GameResources.Pixel,
                new Rectangle(rect.X, rect.Bottom - thickness, rect.Width, thickness),
                color
            );

            sb.Draw(
                GameResources.Pixel,
                new Rectangle(rect.X, rect.Y, thickness, rect.Height),
                color
            );

            sb.Draw(
                GameResources.Pixel,
                new Rectangle(rect.Right - thickness, rect.Y, thickness, rect.Height),
                color
            );
        }
    }
}
