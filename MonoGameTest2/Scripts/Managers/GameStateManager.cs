using System;
using System.Collections.Generic;

using MonoGameTest2.GameStates;

namespace MonoGameTest2.Managers
{
    public class GameStateManager
    {
        public GameState CurrentState {
            get {
                GameState currentState;

                try
                {
                    currentState = _states.Peek();
                }
                catch (InvalidOperationException)
                {
                    return null;
                }

                return currentState;
            }
        }

        private Stack<GameState> _states;

        public GameStateManager()
        {
            _states = new Stack<GameState>();
        }

        public void AddState(GameState newState)
        {
            newState.Initialize();
            newState.LoadContent();
            _states.Push(newState);
        }

        public void Update(bool blockMouseUpdates)
        {
            if (CurrentState != null)
            {
                CurrentState.Update(blockMouseUpdates);
            }
        }

        public void Draw()
        {
            if (CurrentState != null)
            {
                CurrentState.Draw();
            }
        }

        public void RemoveState()
        {
            var state = _states.Pop();
            state.UnloadContent();
        }
    }
}
