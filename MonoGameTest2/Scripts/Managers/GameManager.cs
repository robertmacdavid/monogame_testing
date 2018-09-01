using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonoGameTest2.Entites;
using MonoGameTest2.Helpers;

namespace MonoGameTest2.Managers
{
    class GameManager
    {
        private static GameManager _instance;
        public static GameManager Instance { get { return _instance ?? (_instance = new GameManager()); } }

        public float DeltaTime;
        public Player Player;
        public KeyboardState PreviousKeyboardState;

        private SpriteFont _font;
        private bool _showDebugInfo = false;

        public void LoadContent(ContentManager contentManager)
        {
            var avatar = contentManager.Load<Texture2D>("images/SmileyWalk");
            _font = contentManager.Load<SpriteFont>("default_font");

            Player = new Player(avatar, new Vector2(0, 0));
            Player.LoadContent(contentManager);
        }

        public void Update(GameTime gameTime)
        {
            if (KeyboardHelper.GetKeyUp(Keys.F1))
            {
                _showDebugInfo = !_showDebugInfo;
            }

            DeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Player.HandleInput();
            Player.Update();

            PreviousKeyboardState = Keyboard.GetState();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Player.Draw(spriteBatch);

            if (_showDebugInfo)
            {
                spriteBatch.Begin();
                spriteBatch.DrawString(_font, string.Format("Debug Info\nDelta Time: {0}\nPlayer Position: {1}", DeltaTime, Player.Position), new Vector2(0, 0), Color.White);
                spriteBatch.End();
            }
        }
    }
}
