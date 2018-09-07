using Microsoft.Xna.Framework.Input;

using MonoGameTest2.Managers;

namespace MonoGameTest2.Helpers
{
    public static class KeyboardHelper
    {
        public static bool GetKeyDown(this KeyboardState keyboardState, Keys key)
        {
            return keyboardState.IsKeyDown(key) && GameManager.Instance.PreviousKeyboardState.IsKeyUp(key);
        }

        public static bool GetKeyUp(this KeyboardState keyboardState, Keys key)
        {
            return keyboardState.IsKeyUp(key) && GameManager.Instance.PreviousKeyboardState.IsKeyDown(key);
        }
    }
}
