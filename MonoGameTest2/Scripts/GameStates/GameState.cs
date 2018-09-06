using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using MonoGameTest2.Managers;

namespace MonoGameTest2.GameStates
{
    public abstract class GameState
    {
        protected GameManager GameManager => GameManager.Instance;

        public abstract void Initialize();
        public abstract void LoadContent();
        public abstract void Update();
        public abstract void Draw();
        public abstract void UnloadContent();
    }
}
