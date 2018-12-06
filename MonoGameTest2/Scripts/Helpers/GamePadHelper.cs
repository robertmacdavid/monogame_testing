using Microsoft.Xna.Framework.Input;
using MonoGameTest2.Managers;
using System.Collections.Generic;

namespace MonoGameTest2.Helpers
{
    public static class GamePadHelper
    {
        public static bool GetButtonDown(this GamePadState gamePadState, Buttons button)
        {
            return gamePadState.IsConnected && gamePadState.IsButtonDown(button) && GameManager.PreviousGamePadState.IsButtonUp(button);
        }

        public static bool GetButtonUp(this GamePadState gamePadState, Buttons button)
        {
            return gamePadState.IsConnected && gamePadState.IsButtonUp(button) && GameManager.PreviousGamePadState.IsButtonDown(button);
        }
    }
}
