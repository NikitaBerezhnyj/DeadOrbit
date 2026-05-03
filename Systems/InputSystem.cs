using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace DeadOrbit
{
    public static class InputSystem
    {
        private static KeyboardState _prevKeyboard;
        private static KeyboardState _currKeyboard;

        private static GamePadState _prevGamepad;
        private static GamePadState _currGamepad;

        public static void Update()
        {
            _prevKeyboard = _currKeyboard;
            _currKeyboard = Keyboard.GetState();

            _prevGamepad = _currGamepad;
            _currGamepad = GamePad.GetState(PlayerIndex.One);
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

        public static bool AttackPressed =>
            IsGamepadPressed(Buttons.X) || WasKeyPressed(Keys.Space);

        public static bool UsePressed => IsGamepadPressed(Buttons.A) || WasKeyPressed(Keys.E);

        public static bool DropPressed => IsGamepadPressed(Buttons.Y) || WasKeyPressed(Keys.Q);

        public static bool PausePressed =>
            IsGamepadPressed(Buttons.Start) || WasKeyPressed(Keys.Escape);

        public static bool CraftPressed =>
            IsGamepadPressed(Buttons.Back) || WasKeyPressed(Keys.Tab);

        public static bool NextItem => IsGamepadPressed(Buttons.RightTrigger);

        public static bool PrevItem => IsGamepadPressed(Buttons.LeftTrigger);

        public static bool UiUp => IsGamepadPressed(Buttons.DPadUp) || WasKeyPressed(Keys.Up);

        public static bool UiDown => IsGamepadPressed(Buttons.DPadDown) || WasKeyPressed(Keys.Down);

        public static bool UiLeft => IsGamepadPressed(Buttons.DPadLeft) || WasKeyPressed(Keys.Left);

        public static bool UiRight =>
            IsGamepadPressed(Buttons.DPadRight) || WasKeyPressed(Keys.Right);

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
