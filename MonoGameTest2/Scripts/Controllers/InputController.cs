using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGameTest2.Controllers;
using MonoGameTest2.Helpers;
using MonoGameTest2.UI;
using System;
using System.Collections.Generic;

namespace MonoGameTest2.Controllers
{
    [Flags]
    public enum InputActions
    {
        Accept = 1 << 0,
        Cancel = 1 << 1,
        Talk = 1 << 2,
        Attack = 1 << 3,
        Dodge = 1 << 4,
        Block = 1 << 5,
        MoveLeft = 1 << 6,
        MoveRight = 1 << 7,
        MoveUp = 1 << 8,
        MoveDown = 1 << 9,
        AimLeft = 1 << 10,
        AimRight = 1 << 11,
        AimUp = 1 << 12,
        AimDown = 1 << 13,
        Pause = 1 << 14,
        Inventory = 1 << 15,
        ToggleConsole = 1 << 16,
        ToggleHitboxes = 1 << 17,
        ToggleRealTimeDebug = 1 << 18,
    }

    public enum ButtonStates
    {
        /// <summary>
        /// When the button changes from pressed to released.
        /// </summary>
        Up,

        /// <summary>
        /// When the button changes from released to pressed.
        /// </summary>
        Down,

        /// <summary>
        /// When the button is released. 
        /// </summary>
        Released,

        /// <summary>
        /// When the button is pressed.
        /// </summary>
        Pressed,
    }

    public struct ControllerInput
    {
        public bool GamePad;
        public bool Analog;
        public Keys Key;
        public Buttons Button;
        public ButtonStates ButtonState;

        public ControllerInput(Buttons button, ButtonStates buttonState)
        {
            switch(button)
            {
                case Buttons.LeftThumbstickLeft:
                case Buttons.LeftThumbstickRight:
                case Buttons.LeftThumbstickUp:
                case Buttons.LeftThumbstickDown:
                case Buttons.RightThumbstickLeft:
                case Buttons.RightThumbstickRight:
                case Buttons.RightThumbstickUp:
                case Buttons.RightThumbstickDown:
                    Analog = true;
                    break;
                default:
                    Analog = false;
                    break;
            }

            GamePad = true;
            Key = Keys.None;
            Button = button;
            ButtonState = buttonState;
        }

        public ControllerInput(Keys key, ButtonStates buttonState)
        {
            Analog = false;
            GamePad = false;
            Key = key;
            Button = 0;
            ButtonState = buttonState;
        }
    }

    public static class InputController
    {
        public static Vector2 MoveDirection { get; set; }
        public static Vector2 AimDirection { get; set; }

        private static InputActions _currentActions;
        private static Dictionary<ControllerInput, InputActions> _actions;

        static InputController()
        {
            _actions = new Dictionary<ControllerInput, InputActions>();
        }

        public static void RegisterInput(ControllerInput input, InputActions action)
        {
            _actions[input] = action;
        }

        public static void HandleInput()
        {
            var keyboardState = Keyboard.GetState();
            var gamePadState = GamePad.GetState(PlayerIndex.One);
            var newMoveDirection = new Vector2();
            var newAimDirection = new Vector2();

            // Gather the set of actions that should be called this tick
            // Adding them to a set ensures multiple keys bound to a single
            // action won't result in multiple calls
            _currentActions = 0;
            foreach (var input in _actions.Keys)
            {
                var action = _actions[input];
                var movement = (action & (
                        InputActions.MoveLeft | InputActions.MoveRight | InputActions.MoveUp | InputActions.MoveDown |
                        InputActions.AimLeft | InputActions.AimRight | InputActions.AimUp | InputActions.AimDown
                    )) > 0;

                if (!movement)
                {
                    if (input.GamePad)
                    {
                        switch (input.ButtonState)
                        {
                            case ButtonStates.Up:
                                _currentActions |= gamePadState.GetButtonUp(input.Button) ? action : 0;
                                break;
                            case ButtonStates.Down:
                                _currentActions |= gamePadState.GetButtonDown(input.Button) ? action : 0;
                                break;
                            case ButtonStates.Released:
                                _currentActions |= gamePadState.IsButtonUp(input.Button) ? action : 0;
                                break;
                            case ButtonStates.Pressed:
                                _currentActions |= gamePadState.IsButtonDown(input.Button) ? action : 0;
                                break;
                        }
                    }
                    else
                    {
                        switch (input.ButtonState)
                        {
                            case ButtonStates.Up:
                                _currentActions |= keyboardState.GetKeyUp(input.Key) ? action : 0;
                                break;
                            case ButtonStates.Down:
                                _currentActions |= keyboardState.GetKeyDown(input.Key) ? action : 0;
                                break;
                            case ButtonStates.Released:
                                _currentActions |= keyboardState.IsKeyUp(input.Key) ? action : 0;
                                break;
                            case ButtonStates.Pressed:
                                _currentActions |= keyboardState.IsKeyDown(input.Key) ? action : 0;
                                break;
                        }
                    }
                }
                else
                {
                    var analogValue = 0f;
                    if (input.GamePad)
                    {
                        switch (input.Button)
                        {
                            case Buttons.LeftThumbstickUp:
                                analogValue += gamePadState.ThumbSticks.Left.Y;
                                break;
                            case Buttons.RightThumbstickUp:
                                analogValue += gamePadState.ThumbSticks.Right.Y;
                                break;
                            case Buttons.LeftThumbstickDown:
                                analogValue -= gamePadState.ThumbSticks.Left.Y;
                                break;
                            case Buttons.RightThumbstickDown:
                                analogValue -= gamePadState.ThumbSticks.Right.Y;
                                break;
                            case Buttons.LeftThumbstickLeft:
                                analogValue -= gamePadState.ThumbSticks.Left.X;
                                break;
                            case Buttons.RightThumbstickLeft:
                                analogValue -= gamePadState.ThumbSticks.Right.X;
                                break;
                            case Buttons.LeftThumbstickRight:
                                analogValue += gamePadState.ThumbSticks.Left.X;
                                break;
                            case Buttons.RightThumbstickRight:
                                analogValue += gamePadState.ThumbSticks.Right.X;
                                break;
                            default:
                                analogValue += gamePadState.IsButtonDown(input.Button) ? 1f : 0;
                                break;
                        }
                    }
                    else
                    {
                        analogValue += keyboardState.IsKeyDown(input.Key) ? 1f : 0;
                    }

                    switch (action)
                    {
                        case InputActions.MoveLeft:
                            newMoveDirection.X -= analogValue;
                            break;
                        case InputActions.MoveRight:
                            newMoveDirection.X += analogValue;
                            break;
                        case InputActions.MoveUp:
                            newMoveDirection.Y -= analogValue;
                            break;
                        case InputActions.MoveDown:
                            newMoveDirection.Y += analogValue;
                            break;
                        case InputActions.AimLeft:
                            newAimDirection.X -= analogValue;
                            break;
                        case InputActions.AimRight:
                            newAimDirection.X += analogValue;
                            break;
                        case InputActions.AimUp:
                            newAimDirection.Y -= analogValue;
                            break;
                        case InputActions.AimDown:
                            newAimDirection.Y += analogValue;
                            break;
                    }
                }
            }

            MoveDirection = newMoveDirection;
            AimDirection = newAimDirection;
        }

        public static bool HasInput(InputActions action)
        {
            return (_currentActions & action) > 0;
        }
    }
}
