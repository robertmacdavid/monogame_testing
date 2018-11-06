using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using MonoGameTest2.Managers;

namespace MonoGameTest2.GameStates
{
    public abstract class GameState
    {
        public abstract string Name { get; }
        protected GameManager GameManager => GameManager.Instance;
        protected UIManager UIManager => GameManager.UIManager;

        public abstract void Initialize();
        public abstract void LoadContent();
        public abstract void Update(bool blockMouseUpdates);
        public abstract void Draw();
        public abstract void UnloadContent();
    }
}
