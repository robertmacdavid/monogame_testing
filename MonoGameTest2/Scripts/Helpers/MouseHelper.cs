using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using MonoGameTest2.Managers;

namespace MonoGameTest2.Helpers
{
    public enum MouseButtons
    {
        LeftButton,
        RightButton,
        MiddleButton,
        Mouse4,
        Mouse5,
        None
    }

    public static class MouseHelper
    {
        public static bool GetButtonPressed(this MouseState mouseState, MouseButtons button)
        {
            switch (button)
            {
                case MouseButtons.LeftButton:
                    return mouseState.LeftButton == ButtonState.Pressed;
                case MouseButtons.MiddleButton:
                    return mouseState.MiddleButton == ButtonState.Pressed;
                case MouseButtons.RightButton:
                    return mouseState.RightButton == ButtonState.Pressed;
                case MouseButtons.Mouse4:
                    return mouseState.XButton1 == ButtonState.Pressed;
                case MouseButtons.Mouse5:
                    return mouseState.XButton2 == ButtonState.Pressed;
                default:
                    return false;
            }
        }

        public static bool GetButtonReleased(this MouseState mouseState, MouseButtons button)
        {
            switch (button)
            {
                case MouseButtons.LeftButton:
                    return mouseState.LeftButton == ButtonState.Released;
                case MouseButtons.MiddleButton:
                    return mouseState.MiddleButton == ButtonState.Released;
                case MouseButtons.RightButton:
                    return mouseState.RightButton == ButtonState.Released;
                case MouseButtons.Mouse4:
                    return mouseState.XButton1 == ButtonState.Released;
                case MouseButtons.Mouse5:
                    return mouseState.XButton2 == ButtonState.Released;
                default:
                    return false;
            }
        }

        public static bool GetButtonDown(this MouseState mouseState, MouseButtons button)
        {
            return mouseState.GetButtonPressed(button) && GameManager.PreviousMouseState.GetButtonReleased(button);
        }

        public static bool GetButtonUp(this MouseState mouseState, MouseButtons button)
        {
            return mouseState.GetButtonReleased(button) && GameManager.PreviousMouseState.GetButtonPressed(button);
        }

        public static Vector2 GetPosition(this MouseState mouseState)
        {
            return mouseState.Position.ToVector2() / GameManager.Instance.Zoom;
        }

        public static Vector2 GetScreenPosition(this MouseState mouseState)
        {
            var mousePosition = mouseState.GetPosition();
            return new Vector2(mousePosition.X / GameManager.NATIVE_SCREEN_WIDTH, mousePosition.Y / GameManager.NATIVE_SCREEN_HEIGHT);
        }

        public static Vector2 GetPositionDelta(this MouseState mouseState)
        {
            return (mouseState.GetPosition() - GameManager.PreviousMouseState.GetPosition());
        }

        public static int GetScrollDelta(this MouseState mouseState)
        {
            return mouseState.ScrollWheelValue - GameManager.PreviousMouseState.ScrollWheelValue;
        }
    }
}
