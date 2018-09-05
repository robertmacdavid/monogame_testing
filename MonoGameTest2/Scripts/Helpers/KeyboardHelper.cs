using Microsoft.Xna.Framework.Input;

using MonoGameTest2.Managers;

namespace MonoGameTest2.Helpers
{
    public static class KeyboardHelper
    {
        public static bool GetKeyDown(Keys key)
        {
            var currentKeyboardState = Keyboard.GetState();
            return currentKeyboardState.IsKeyDown(key) && GameManager.Instance.PreviousKeyboardState.IsKeyUp(key);
        }

        public static bool GetKeyUp(Keys key)
        {
            var currentKeyboardState = Keyboard.GetState();
            return currentKeyboardState.IsKeyUp(key) && GameManager.Instance.PreviousKeyboardState.IsKeyDown(key);
        }
    }
}
