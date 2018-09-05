using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using MonoGameTest2.Managers;

namespace MonoGameTest2.GameStates
{
    public abstract class GameState
    {
        protected GameManager GameManager => GameManager.Instance;

        public abstract void Initialize();
        public abstract void LoadContent(ContentManager contentManager);
        public abstract void Update();
        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
