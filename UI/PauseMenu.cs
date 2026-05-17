using System;
using DeadOrbit.Core;
using DeadOrbit.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DeadOrbit.UI
{
    public class PauseMenu
    {
        private readonly GraphicsDevice _graphics;
        private int _selectedIndex = 0;

        private readonly string[] _menuKeys =
        {
            "menu_continue",
            "menu_options",
            "menu_save",
            "menu_quit",
        };

        private const int ButtonWidth = 240;
        private const int ButtonHeight = 44;
        private const int ButtonSpacing = 12;

        public bool IsVisible { get; private set; } = false;

        public PauseMenu(GraphicsDevice graphics)
        {
            _graphics = graphics;
        }

        public bool Update()
        {
            if (!IsVisible)
                return false;

            if (Systems.InputSystem.UiUp)
                _selectedIndex = (_selectedIndex - 1 + _menuKeys.Length) % _menuKeys.Length;

            if (Systems.InputSystem.UiDown)
                _selectedIndex = (_selectedIndex + 1) % _menuKeys.Length;

            if (Systems.InputSystem.UsePressed)
            {
                return ExecuteSelected();
            }

            return false;
        }

        public void Show()
        {
            IsVisible = true;
            _selectedIndex = 0;
        }

        public void Hide()
        {
            IsVisible = false;
        }

        private bool ExecuteSelected()
        {
            switch (_selectedIndex)
            {
                case 0:
                    Hide();
                    return true;
                case 1:
                    Console.WriteLine("[PAUSE] Налаштування — не реалізовано");
                    return false;
                case 2:
                    Console.WriteLine("[PAUSE] Зберегти — не реалізовано");
                    return false;
                case 3:
                    Console.WriteLine("[PAUSE] Вийти — не реалізовано");
                    return false;
            }
            return false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!IsVisible)
                return;

            int screenW = _graphics.Viewport.Width;
            int screenH = _graphics.Viewport.Height;

            spriteBatch.Draw(
                GameResources.Pixel,
                new Rectangle(0, 0, screenW, screenH),
                Color.Black * 0.6f
            );

            if (GameResources.DefaultFont != null)
            {
                string title = LocalizationManager.Get("pause_menu");
                Vector2 titleSize = GameResources.DefaultFont.MeasureString(title);
                spriteBatch.DrawString(
                    GameResources.DefaultFont,
                    title,
                    new Vector2(screenW / 2f - titleSize.X / 2f, screenH / 2f - 140),
                    Color.White
                );
            }

            int totalH = _menuKeys.Length * ButtonHeight + (_menuKeys.Length - 1) * ButtonSpacing;
            int startY = screenH / 2 - totalH / 2;

            for (int i = 0; i < _menuKeys.Length; i++)
            {
                int x = screenW / 2 - ButtonWidth / 2;
                int y = startY + i * (ButtonHeight + ButtonSpacing);

                bool isSelected = i == _selectedIndex;

                string currentLabel = LocalizationManager.Get(_menuKeys[i]);

                spriteBatch.Draw(
                    GameResources.Pixel,
                    new Rectangle(x, y, ButtonWidth, ButtonHeight),
                    isSelected ? new Color(80, 80, 80, 220) : new Color(30, 30, 30, 180)
                );

                DrawBorder(
                    spriteBatch,
                    new Rectangle(x, y, ButtonWidth, ButtonHeight),
                    isSelected ? Color.White : new Color(100, 100, 100, 200),
                    2
                );

                if (GameResources.DefaultFont != null)
                {
                    Vector2 textSize = GameResources.DefaultFont.MeasureString(currentLabel);
                    Vector2 textPos = new Vector2(
                        x + ButtonWidth / 2f - textSize.X / 2f,
                        y + ButtonHeight / 2f - textSize.Y / 2f
                    );

                    spriteBatch.DrawString(
                        GameResources.DefaultFont,
                        currentLabel,
                        textPos + new Vector2(1, 1),
                        Color.Black * 0.8f
                    );

                    spriteBatch.DrawString(
                        GameResources.DefaultFont,
                        currentLabel,
                        textPos,
                        isSelected ? Color.White : Color.Gray
                    );
                }
            }
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
