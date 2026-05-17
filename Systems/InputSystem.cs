using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace DeadOrbit.Systems
{
    public static class InputSystem
    {
        private static KeyboardState _prevKeyboard;
        private static KeyboardState _currKeyboard;
        private static GamePadState _prevGamepad;
        private static GamePadState _currGamepad;
        private static MouseState _prevMouse;
        private static MouseState _currMouse;

        public static void Update()
        {
            _prevKeyboard = _currKeyboard;
            _currKeyboard = Keyboard.GetState();
            _prevGamepad = _currGamepad;
            _currGamepad = GamePad.GetState(PlayerIndex.One);
            _prevMouse = _currMouse;
            _currMouse = Mouse.GetState();
        }

        public static Vector2 GetMovementDirection()
        {
            Vector2 direction = Vector2.Zero;
            if (_currKeyboard.IsKeyDown(Keys.W) || _currKeyboard.IsKeyDown(Keys.Up))
                direction.Y -= 1;
            if (_currKeyboard.IsKeyDown(Keys.S) || _currKeyboard.IsKeyDown(Keys.Down))
                direction.Y += 1;
            if (_currKeyboard.IsKeyDown(Keys.A) || _currKeyboard.IsKeyDown(Keys.Left))
                direction.X -= 1;
            if (_currKeyboard.IsKeyDown(Keys.D) || _currKeyboard.IsKeyDown(Keys.Right))
                direction.X += 1;

            if (_currGamepad.IsConnected)
            {
                Vector2 stick = _currGamepad.ThumbSticks.Left;
                if (Math.Abs(stick.X) > 0.1f || Math.Abs(stick.Y) > 0.1f)
                {
                    direction.X = stick.X;
                    direction.Y = -stick.Y;
                }
            }

            if (direction != Vector2.Zero)
                direction.Normalize();
            return direction;
        }

        public static bool ActionPressed =>
            IsGamepadPressed(Buttons.X)
            || WasKeyPressed(Keys.Space)
            || WasMousePressed(MouseButton.Left);

        public static bool UsePressed =>
            IsGamepadPressed(Buttons.A)
            || WasKeyPressed(Keys.E)
            || WasMousePressed(MouseButton.Right);

        public static bool DropPressed => IsGamepadPressed(Buttons.Y) || WasKeyPressed(Keys.Q);

        public static bool PausePressed =>
            IsGamepadPressed(Buttons.Start) || WasKeyPressed(Keys.Escape);

        public static bool CraftPressed =>
            IsGamepadPressed(Buttons.Back) || WasKeyPressed(Keys.Tab);

        public static bool NextItem => IsGamepadPressed(Buttons.RightTrigger) || ScrollDelta < 0;

        public static bool PrevItem => IsGamepadPressed(Buttons.LeftTrigger) || ScrollDelta > 0;

        public static int HotkeySlot => GetHotkeySlot();

        public static bool UiUp => IsGamepadPressed(Buttons.DPadUp) || WasKeyPressed(Keys.Up);

        public static bool UiDown => IsGamepadPressed(Buttons.DPadDown) || WasKeyPressed(Keys.Down);

        public static bool UiLeft => IsGamepadPressed(Buttons.DPadLeft) || WasKeyPressed(Keys.Left);

        public static bool UiRight =>
            IsGamepadPressed(Buttons.DPadRight) || WasKeyPressed(Keys.Right);

        public static bool MouseLeftPressed => WasMousePressed(MouseButton.Left);
        public static bool EnterPressed => WasKeyPressed(Keys.Enter);

        public static Vector2 MouseScreenPosition => new Vector2(_currMouse.X, _currMouse.Y);

        private enum MouseButton
        {
            Left,
            Right,
            Middle,
        }

        private static bool WasMousePressed(MouseButton button) =>
            button switch
            {
                MouseButton.Left => _currMouse.LeftButton == ButtonState.Pressed
                    && _prevMouse.LeftButton == ButtonState.Released,
                MouseButton.Right => _currMouse.RightButton == ButtonState.Pressed
                    && _prevMouse.RightButton == ButtonState.Released,
                MouseButton.Middle => _currMouse.MiddleButton == ButtonState.Pressed
                    && _prevMouse.MiddleButton == ButtonState.Released,
                _ => false,
            };

        private static int ScrollDelta => _currMouse.ScrollWheelValue - _prevMouse.ScrollWheelValue;

        private static int GetHotkeySlot()
        {
            if (WasKeyPressed(Keys.D1))
                return 0;
            if (WasKeyPressed(Keys.D2))
                return 1;
            if (WasKeyPressed(Keys.D3))
                return 2;
            if (WasKeyPressed(Keys.D4))
                return 3;
            if (WasKeyPressed(Keys.D5))
                return 4;
            if (WasKeyPressed(Keys.D6))
                return 5;
            if (WasKeyPressed(Keys.D7))
                return 6;
            if (WasKeyPressed(Keys.D8))
                return 7;
            return -1;
        }

        private static bool IsGamepadPressed(Buttons button)
        {
            return _currGamepad.IsConnected
                && _currGamepad.IsButtonDown(button)
                && !_prevGamepad.IsButtonDown(button);
        }

        private static bool WasKeyPressed(Keys key)
        {
            return _currKeyboard.IsKeyDown(key) && !_prevKeyboard.IsKeyDown(key);
        }
    }
}
