using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGameTest2.Controllers
{
    public interface IInputController
    {
        void HandleInput();
    }
    public class InputEventHandler : IInputController
    {
        private KeyboardState currKeyState;
        private KeyboardState prevKeyState;

        private List<Keys> newKeys;
        private List<Keys> heldKeys;
        private List<Keys> releasedKeys;
        private Dictionary<Keys, Action> keyPressActions;
        private Dictionary<Keys, Action> keyHoldActions;
        private Dictionary<Keys, Action> keyReleaseActions;

        public InputEventHandler()
        {
            currKeyState = Keyboard.GetState();

            newKeys = new List<Keys>();
            heldKeys = new List<Keys>();
            releasedKeys = new List<Keys>();

            keyPressActions = new Dictionary<Keys, Action>();
            keyHoldActions = new Dictionary<Keys, Action>();
            keyReleaseActions = new Dictionary<Keys, Action>();
        }

        public void HandleInput()
        {
            prevKeyState = currKeyState;
            currKeyState = Keyboard.GetState();

            newKeys.Clear();
            heldKeys.Clear();
            releasedKeys.Clear();

            // Determine the raising and lowering of keys
            foreach (var key in prevKeyState.GetPressedKeys())
            {
                if (currKeyState.IsKeyUp(key))
                    releasedKeys.Add(key);
                else if (currKeyState.IsKeyDown(key))
                    heldKeys.Add(key);
            }
            foreach (var key in currKeyState.GetPressedKeys())
            {
                if (prevKeyState.IsKeyUp(key))
                    newKeys.Add(key);
            }

            // Gather the set of actions that should be called this tick
            // Adding them to a set ensures multiple keys bound to a single
            // action won't result in multiple calls
            HashSet<Action> actionsToCall = new HashSet<Action>();
            foreach (var key in newKeys)
            {
                if (keyPressActions.TryGetValue(key, out Action action))
                    actionsToCall.Add(action);
            }
            foreach (var key in heldKeys)
            {
                if (keyHoldActions.TryGetValue(key, out Action action))
                    actionsToCall.Add(action);
            }
            foreach (var key in releasedKeys)
            {
                if (keyReleaseActions.TryGetValue(key, out Action action))
                    actionsToCall.Add(action);
            }


            // Call the actions
            foreach (Action action in actionsToCall)
                action();

        }

        private void _AddKeyHandlers(Keys[] keys, Action[] actions, 
                                     Dictionary<Keys, Action> HandlerSet)
        {
            if (keys.Length != actions.Length)
                throw new ArgumentException("The number of keys and actions are not equal!");
            for (int i = 0; i < keys.Length; i++)
                HandlerSet[keys[i]] = actions[i];
        }


        public void AddKeyPressHandler(Keys key, Action action)
        {  AddKeyPressHandlers(new Keys[1] { key }, new Action[1] { action });  }
        public void AddKeyPressHandlers(Keys[] keys, Action[] actions)
        {  _AddKeyHandlers(keys, actions, keyPressActions);  }


        public void AddKeyHoldHandler(Keys key, Action action)
        {  AddKeyHoldHandlers(new Keys[1] { key }, new Action[1] { action });  }
        public void AddKeyHoldHandlers(Keys[] keys, Action[] actions)
        {  _AddKeyHandlers(keys, actions, keyHoldActions);  }


        public void AddKeyReleaseHandler(Keys key, Action action)
        {  AddKeyReleaseHandlers(new Keys[1] { key }, new Action[1] { action });  }
        public void AddKeyReleaseHandlers(Keys[] keys, Action[] actions)
        {  _AddKeyHandlers(keys, actions, keyReleaseActions);  }
    }
}
