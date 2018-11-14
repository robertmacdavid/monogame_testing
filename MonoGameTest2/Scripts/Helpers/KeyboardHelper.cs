using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

using MonoGameTest2.Managers;

namespace MonoGameTest2.Helpers
{
    public struct TypedKey
    {
        public Keys Key;
        public string String;
        public bool Shift;
        public bool CapsLock;
        public bool Ctrl;
        public bool Alt;
    }

    public static class KeyboardHelper
    {
        public static bool GetKeyDown(this KeyboardState keyboardState, Keys key)
        {
            return keyboardState.IsKeyDown(key) && GameManager.PreviousKeyboardState.IsKeyUp(key);
        }

        public static bool GetKeyUp(this KeyboardState keyboardState, Keys key)
        {
            return keyboardState.IsKeyUp(key) && GameManager.PreviousKeyboardState.IsKeyDown(key);
        }

        public static Keys[] GetKeysDown(this KeyboardState keyboardState)
        {
            var keysDown = new List<Keys>();
            var lastKeys = GameManager.PreviousKeyboardState.GetPressedKeys();
            var newKeys = keyboardState.GetPressedKeys();

            foreach (var newKey in newKeys)
            {
                var found = false;

                foreach (var lastKey in lastKeys)
                {
                    if (newKey == lastKey)
                    {
                        found = true;
                        break;
                    }
                }

                if (!found) {
                    keysDown.Add(newKey);
                }
            }

            return keysDown.ToArray();
        }

        public static TypedKey GetTypedLetter(this KeyboardState keyboardState)
        {
            var keysDown = keyboardState.GetKeysDown();

            var typedKey = new TypedKey()
            {
                Key = keysDown.Length > 0 ? keysDown[0] : Keys.None,
                CapsLock = keyboardState.CapsLock,
                String = "",
            };

            if (keysDown.Length == 0)
            {
                return typedKey;
            }

            var pressedKeys = keyboardState.GetPressedKeys();

            foreach (var pressedKey in pressedKeys)
            {
                switch (pressedKey) {
                    case Keys.LeftShift:
                    case Keys.RightShift:
                        typedKey.Shift = true;
                        break;
                    case Keys.LeftControl:
                    case Keys.RightControl:
                        typedKey.Ctrl = true;
                        break;
                    case Keys.LeftAlt:
                    case Keys.RightAlt:
                        typedKey.Alt = true;
                        break;
                }
            }

            if (typedKey.Key >= Keys.A && typedKey.Key <= Keys.Z)
            {
                typedKey.String = char.ConvertFromUtf32((int)typedKey.Key);
                if (typedKey.Shift == typedKey.CapsLock) {
                    typedKey.String = typedKey.String.ToLower();
                }
            }
            else if (typedKey.Key >= Keys.D0 && typedKey.Key <= Keys.D9)
            {
                typedKey.String = char.ConvertFromUtf32(((int)typedKey.Key - (int)Keys.D0) + '0');
            }
            else if (typedKey.Key >= Keys.NumPad0 && typedKey.Key <= Keys.NumPad9)
            {
                typedKey.String = char.ConvertFromUtf32(((int)typedKey.Key - (int)Keys.NumPad0) + '0');
            }
            else if (typedKey.Key == Keys.Space)
            {
                typedKey.String = " ";
            }

            return typedKey;
        }
    }
}
